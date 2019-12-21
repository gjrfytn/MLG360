using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace MLG360.Strategy
{
    internal class Unit : GameObject
    {
        private readonly IEnvironment _Environment;
        private readonly IScoretable _Scoretable;

        private readonly int _Id;
        private readonly Weapon _Weapon;
        private readonly Vector2 _Size;
        private readonly float _RunSpeed;
        private readonly float _JumpSpeed;
        private readonly float _JumpTimeLeft;
        private readonly float _MaxJumpTime;

        public int PlayerId { get; }
        public VerticalDynamic VerticalDynamic { get; }
        public float Health { get; }
        public float MaxHealth { get; }

        private float WeaponHeight => Height / 2;
        private Vector2 WeaponPoint => Pos + WeaponHeight * Vector2.UnitY;

        public Unit(int id, int playerId, Vector2 pos, Weapon weapon, Vector2 size, float runSpeed, float jumpSpeed, float jumpTimeLeft, float maxJumpTime, VerticalDynamic verticalDynamic, float health, float maxHealth, IEnvironment environment, IScoretable scoretable) :
            base(pos, size)
        {
            _Id = id;
            PlayerId = playerId;
            _Weapon = weapon;
            _Size = size;
            _RunSpeed = runSpeed;
            _JumpSpeed = jumpSpeed;
            _JumpTimeLeft = jumpTimeLeft;
            _MaxJumpTime = maxJumpTime;
            VerticalDynamic = verticalDynamic;
            Health = health;
            MaxHealth = maxHealth;
            _Environment = environment;
            _Scoretable = scoretable;
        }

        public Action Act()
        {
            var targetPos = Pos;
            var weaponOperation = new WeaponOperation(Vector2.UnitX, WeaponOperation.ActionType.None);
            if (_Weapon == null)
            {
                var closestGun = FindClosestGun();
                if (closestGun != null)
                    targetPos = closestGun.Pos;
            }
            else
            {
                var closestEnemy = FindClosestEnemy();
                if (closestEnemy != null)
                {
                    weaponOperation = OperateWeapon(closestEnemy);

                    targetPos = PositionSelf(closestEnemy, weaponOperation.Action == WeaponOperation.ActionType.Shoot);
                }
            }

            var movement = Pathfind(targetPos);

            var dodgeMove = Dodge(movement.Horizontal);

            return new Action(dodgeMove ?? movement, weaponOperation);
        }

        private IEnumerable<Unit> FindEnemies() => _Environment.Units.Where(u => u.PlayerId != PlayerId);
        private IEnumerable<Unit> FindAllies() => _Environment.Units.Where(u => u.PlayerId == PlayerId && u._Id != _Id);
        private Unit FindClosestEnemy() => PickClosest(FindEnemies());
        private Unit FindClosestAlly() => PickClosest(FindAllies());
        private Gun FindClosestGun() => PickClosest(_Environment.Guns);
        private HealthPack FindClosestHealthPack() => PickClosest(_Environment.HealthPacks);

        private Movement Pathfind(Vector2 targetPos)
        {
            var currentTile = _Environment.Tiles.Single(t => t.Contains(Pos));
            var bottomTile = _Environment.GetBottomTile(currentTile);

            const float corrector = 0.000001f;
            if (bottomTile.Type == TileType.Empty || bottomTile.Type == TileType.Ladder) // Precision error compensation.
                currentTile = _Environment.Tiles.Single(t => t.Contains(Pos - corrector * Vector2.UnitY));

            var ally = FindClosestAlly();

            var rightTile = _Environment.GetRightTile(currentTile);
            var leftTile = _Environment.GetLeftTile(currentTile);

            VerticalMovement verticalMovement;
            if (targetPos.X > Pos.X && (rightTile.IsWall || (ally != null ? rightTile.InXArea(ally.Pos) && ally.InYArea(Pos) : false)) ||
                targetPos.X < Pos.X && (leftTile.IsWall || (ally != null ? leftTile.InXArea(ally.Pos) && ally.InYArea(Pos) : false)))
                verticalMovement = VerticalMovement.Jump;
            else
            {
                var diff = Pos.Y - targetPos.Y;
                if (diff >= 0 && diff < VerticalDynamic.FallSpeed * _Environment.DTime)
                    verticalMovement = VerticalMovement.None;
                else
                    verticalMovement = targetPos.Y > Pos.Y ? VerticalMovement.Jump : VerticalMovement.JumpOff;
            }

            const float jumpRefreshThreshold = 0.5f;
            const float jumpRefreshHeight = 0.2f;
            var jumpPercentLeft = _JumpTimeLeft / _MaxJumpTime;
            if (jumpPercentLeft <= jumpRefreshThreshold &&
                (bottomTile.Type == TileType.Platform || bottomTile.IsWall /*TODO Нужно Wall?*/) &&
                Pos.Y - bottomTile.Top.Y > corrector &&
                Pos.Y - bottomTile.Top.Y <= jumpRefreshHeight)
                verticalMovement = VerticalMovement.None;

            HorizontalMovement horizontalMovement;
            if (System.Math.Abs(targetPos.X - Pos.X) >= _RunSpeed * _Environment.DTime)
                horizontalMovement = targetPos.X > Pos.X ? HorizontalMovement.Right : HorizontalMovement.Left;
            else
                horizontalMovement = HorizontalMovement.None;

            return new Movement(horizontalMovement, verticalMovement);
        }

        private Vector2 PositionSelf(Unit closestEnemy, bool enemyInSight)
        {
            var closestHP = FindClosestHealthPack();

            if (closestHP != null)
            {
                var healthPanicThreshold = closestEnemy._Weapon?.DPS ?? 0;
                const float greedDist = 1.5f;
                if (Health <= healthPanicThreshold ||
                    Vector2.Distance(closestHP.Pos, closestEnemy.Pos) <= greedDist)
                    return closestHP.Pos;
            }

            if (enemyInSight || _Weapon.NeedsReload)
            {
                //TODO plant mine if hp is not too close.

                if (closestHP != null)
                {
                    var ally = FindAllies().FirstOrDefault();
                    if (ally != null &&
                        ally.Health < Health &&
                        _Environment.HealthPacks.Count() > 1)
                        closestHP = PickClosest(_Environment.HealthPacks.Where(hp => hp.Pos != closestHP.Pos));

                    var tileWithHP = _Environment.Tiles.Single(t => t.Contains(closestHP.Pos));
                    var leftTile = _Environment.GetLeftTile(tileWithHP);
                    var rightTile = _Environment.GetRightTile(tileWithHP);

                    return Vector2.DistanceSquared(Pos, leftTile.Bottom) < Vector2.DistanceSquared(Pos, rightTile.Bottom) && !leftTile.IsWall ?
                        leftTile.Bottom : rightTile.Bottom;
                }
                else if (_Weapon.NeedsReload) //TODO remove check?
                {
                    var solidTiles = _Environment.Tiles.Where(t => t.IsWall || t.Type == TileType.Platform || t.Type == TileType.Ladder).ToArray();
                    var freeTiles = _Environment.Tiles.Where(t => (t.Type == TileType.Empty || t.Type == TileType.Ladder) &&
                                                                 solidTiles.Any(st => st.Pos.X == t.Pos.X && st.Pos.Y == t.Pos.Y - 1))
                                                     .OrderBy(t => Vector2.DistanceSquared(Pos, t.Bottom));

                    foreach (var tile in freeTiles)
                    {
                        var tileWeaponPoint = tile.Bottom + WeaponHeight * Vector2.UnitY;
                        var enemyCenter = TakeCenter(closestEnemy);
                        var aimToEnemy = enemyCenter - tileWeaponPoint;
                        var hasSight = HasLineOfSight(tileWeaponPoint, aimToEnemy, Vector2.Distance(tileWeaponPoint, enemyCenter));

                        if (!hasSight)
                            return tile.Pos;
                    }
                }
            }

            return closestEnemy.Pos;
        }

        private Vector2 CalculateAim(Unit unit)
        {
            var distance = Vector2.Distance(Pos, TakeCenter(unit));
            var timeToHit = distance / _Weapon.BulletSpeed; //TODO wrong?
            var aimPoint = TakeCenter(unit, PredictPos(unit, timeToHit, HorizontalMovement.None, false));

            return Vector2.Normalize(aimPoint - WeaponPoint);
        }

        private WeaponOperation OperateWeapon(Unit enemy)
        {
            if (enemy == null || _Weapon == null)
                return new WeaponOperation(Vector2.UnitX, WeaponOperation.ActionType.None);

            var aim = CalculateAim(enemy);

            var hasLineOfSight = HasLineOfSight(WeaponPoint, aim, Vector2.Distance(WeaponPoint, TakeCenter(enemy)), 2 * _Weapon.BulletSize);

            var action = hasLineOfSight && CheckWeaponFireSafety(enemy, aim) ? WeaponOperation.ActionType.Shoot : WeaponOperation.ActionType.None;

#if DEBUG
            {
                Model.ColorFloat lineColor;
                switch (action)
                {
                    case WeaponOperation.ActionType.None:
                        lineColor = new Model.ColorFloat(1, 0, 0, 0.3f);
                        break;
                    case WeaponOperation.ActionType.Shoot:
                        lineColor = new Model.ColorFloat(0, 1, 0, 0.3f);
                        break;
                    case WeaponOperation.ActionType.Reload:
                        lineColor = new Model.ColorFloat(1, 1, 0, 0.3f);
                        break;
                    default: throw new System.ArgumentOutOfRangeException(nameof(action));
                }

                Debug.Instance?.Draw(
                    new Model.Debugging.Line(WeaponPoint.Convert(),
                    (WeaponPoint + 30 * aim).Convert(),
                    0.1f,
                    lineColor));
            }
#endif

            return new WeaponOperation(aim, action);
        }

        private T PickClosest<T>(IEnumerable<T> objects) where T : GameObject => objects.OrderBy(e => Vector2.DistanceSquared(Pos, e.Pos)).FirstOrDefault();
        private bool HasLineOfSight(Vector2 from, Vector2 aim, float distance) => !new Ray(from, aim, distance).Intersects(FindWallTiles());

        private bool HasLineOfSight(Vector2 from, Vector2 aim, float distance, float size)
        {
            IEnumerable<Rectangle> wallTiles = FindWallTiles();

            return !new Ray(from, aim, distance).Intersects(wallTiles.Concat(FindAllies()), new Vector2(size, size));
        }

        private bool CheckWeaponFireSafety(Unit enemy, Vector2 aim)
        {
            if (_Weapon.BulletExplosionSize > 0)
            {
                var canDamageSelf = false;
                var damageToEnemy = 0;

                var leftSpreadEdge = new Ray(WeaponPoint, Vector2.Transform(aim, Matrix3x2.CreateRotation(_Weapon.Spread)), _Weapon.BulletExplosionSize);
                var rightSpreadEdge = new Ray(WeaponPoint, Vector2.Transform(aim, Matrix3x2.CreateRotation(-_Weapon.Spread)), _Weapon.BulletExplosionSize);

#if DEBUG
                {
                    Debug.Instance?.Draw(
                        new Model.Debugging.Line(WeaponPoint.Convert(),
                        (WeaponPoint + 30 * Vector2.Transform(aim, Matrix3x2.CreateRotation(_Weapon.Spread))).Convert(),
                        0.1f,
                        new Model.ColorFloat(0.5f, 0.5f, 0.5f, 0.5f)));

                    Debug.Instance?.Draw(
                        new Model.Debugging.Line(WeaponPoint.Convert(),
                        (WeaponPoint + 30 * Vector2.Transform(aim, Matrix3x2.CreateRotation(-_Weapon.Spread))).Convert(),
                        0.1f,
                        new Model.ColorFloat(0.5f, 0.5f, 0.5f, 0.5f)));
                }
#endif

                var wallTiles = FindWallTiles().ToArray();
                var impactPoints = new[]
                {
                    leftSpreadEdge.FindIntersectionPoint(wallTiles, new Vector2(_Weapon.BulletSize, _Weapon.BulletSize)),
                    rightSpreadEdge.FindIntersectionPoint(wallTiles, new Vector2(_Weapon.BulletSize, _Weapon.BulletSize))
                    // TODO Enemy impact (sector class!)
                };

                var explosions = impactPoints.Where(p => p.HasValue).Select(p => new Explosion(p.Value, _Weapon.BulletExplosionSize, _Weapon.BulletExplosionDamage));

                foreach (var explosion in explosions)
                {
                    if (Intersects(explosion))
                    {
                        canDamageSelf = true;
                        damageToEnemy = enemy.Intersects(explosion) ? explosion.Damage : 0;

                        break;
                    }
                }

                if (canDamageSelf && damageToEnemy != 0)
                    return ShouldKamikaze(enemy, damageToEnemy);

                return !canDamageSelf;
            }

            return true;
        }

        private bool ShouldKamikaze(Unit enemy, int damageToEnemy)
        {
            var willDie = Health <= _Weapon.BulletExplosionDamage;
            var enemyWillDie = enemy.Health <= damageToEnemy;
            var hasMoreOrEqualPoints = _Scoretable.GetPlayerScore(PlayerId) >= _Scoretable.GetPlayerScore(enemy.PlayerId);

            if (willDie)
                return enemyWillDie && hasMoreOrEqualPoints;

            return true;
        }

        private Vector2 PredictPos(Unit unit, float dtime, HorizontalMovement horizontalMovement, bool willJumpOffPlatformOrLadder) //TODO workaround
        {
            float dx;
            switch (horizontalMovement)
            {
                case HorizontalMovement.None:
                    dx = 0;
                    break;
                case HorizontalMovement.Left:
                    dx = -unit._RunSpeed * dtime;
                    break;
                case HorizontalMovement.Right:
                    dx = unit._RunSpeed * dtime;
                    break;
                default: throw new System.ArgumentOutOfRangeException(nameof(horizontalMovement));
            }

            var dy = unit.VerticalDynamic.CalculateDPos(dtime);

            if (dy == 0)
                return unit.Pos + dx * Vector2.UnitX;

            var pos = unit.Pos + new Vector2(dx, dy);

            var solidTilesAbovePos = _Environment.Tiles
                                                .Where(t => (t.IsWall || t.Type == TileType.JumpPad || (t.Type == TileType.Platform || t.Type == TileType.Ladder) && !willJumpOffPlatformOrLadder) &&
                                                            t.Pos.Y < unit.Pos.Y &&
                                                            t.Top.Y >= pos.Y &&
                                                            t.InXArea(pos))
                                                .ToArray();

            if (solidTilesAbovePos.Any())
                return new Vector2(unit.Pos.X + dx, solidTilesAbovePos.OrderByDescending(t => t.Top.Y).First().Top.Y);

            // TODO Horizontal prediction.

            return pos;
        }

        private IEnumerable<Tile> FindWallTiles() => _Environment.Tiles.Where(t => t.IsWall);

        private Movement Dodge(HorizontalMovement currentHorizontalMovement)
        {
            var dodgeMove = DodgeBullets(currentHorizontalMovement);

            return dodgeMove ?? DodgeExplosions(currentHorizontalMovement);
        }

        private readonly Movement[] _DodgeMoves = new[]
        {
            new Movement(HorizontalMovement.Left, VerticalMovement.None),
            new Movement(HorizontalMovement.Right, VerticalMovement.None),
            new Movement(HorizontalMovement.None, VerticalMovement.Jump),
            new Movement(HorizontalMovement.None, VerticalMovement.JumpOff),
            new Movement(HorizontalMovement.Left, VerticalMovement.Jump),
            new Movement(HorizontalMovement.Right, VerticalMovement.Jump),
            new Movement(HorizontalMovement.Right, VerticalMovement.JumpOff),
            new Movement(HorizontalMovement.Left, VerticalMovement.JumpOff)
        };

        private Movement DodgeExplosions(HorizontalMovement currentHorizontalMovement)
        {
            var wallTiles = FindWallTiles().ToArray();
            var explosiveBullets = _Environment.Bullets.Where(b => b.ExplosionSize > 0);

            Movement result = null;
            foreach (var bullet in explosiveBullets)
            {
                var wallHit = bullet.FindHit(wallTiles);
                var explosion = new Explosion(wallHit.Pos, bullet.ExplosionSize, bullet.ExplosionDamage);

                var unitHit = false;
                for (float dt = 0; dt <= wallHit.DTime; dt += _Environment.DTime)
                {
                    var movedUnit = Clone(PredictPos(this, dt, currentHorizontalMovement, false));
                    unitHit = movedUnit.Intersects(explosion);

                    if (unitHit)
                        break;
                }

                if (unitHit)
                {
                    var minDodgeTime = float.MaxValue;
                    foreach (var dodgeMove in _DodgeMoves)
                    {
                        var moveVelocity = Convert(dodgeMove);

                        float? dodgeTime = null;
                        for (var dt = _Environment.DTime; dt <= wallHit.DTime; dt += _Environment.DTime)
                        {
                            Vector2 unitPos;
                            if (dodgeMove.Vertical == VerticalMovement.None)
                                unitPos = PredictPos(this, dt, dodgeMove.Horizontal, false);
                            else if (dodgeMove.Vertical == VerticalMovement.JumpOff)
                                unitPos = PredictPos(this, dt, dodgeMove.Horizontal, true);
                            else
                                unitPos = Pos + moveVelocity * dt;

                            var movedUnit = Clone(unitPos);

                            if (wallTiles.Any(t => t.Intersects(movedUnit)))
                                break;

                            if (!movedUnit.Intersects(explosion))
                            {
                                if (!dodgeTime.HasValue)
                                    dodgeTime = dt;
                            }
                            else if (dodgeTime.HasValue)
                            {
                                dodgeTime = null;

                                break;
                            }
                        }

                        if (dodgeTime.HasValue &&
                            dodgeTime.Value < minDodgeTime &&
                            dodgeTime.Value < wallHit.DTime + _Environment.DTime)
                        {
                            result = dodgeMove;
                            minDodgeTime = dodgeTime.Value;
                        }
                    }

                    if (result != null)
                    {
                        if (wallHit.DTime > minDodgeTime + 3 * _Environment.DTime)
                            result = null;

                        break;
                    }
                }
            }

            return result;
        }

        //TODO duplication
        private Movement DodgeBullets(HorizontalMovement currentHorizontalMovement)
        {
            var wallTiles = FindWallTiles().ToArray();

            Movement result = null;
            foreach (var bullet in _Environment.Bullets)
            {
                var wallHit = bullet.FindHit(wallTiles);

                Hit unitHit = null;
                for (float dt = 0; dt <= wallHit.DTime; dt += _Environment.DTime)
                {
                    var movedUnit = Clone(PredictPos(this, dt, currentHorizontalMovement, false));
                    unitHit = bullet.FindHit(new[] { movedUnit });

                    if (unitHit != null)
                        break;
                }

                if (unitHit != null)
                {
                    if (unitHit.DTime < wallHit.DTime)
                    {
                        var minDodgeTime = float.MaxValue;
                        foreach (var dodgeMove in _DodgeMoves)
                        {
                            var moveVelocity = Convert(dodgeMove);

                            float? dodgeTime = null;
                            for (var dt = _Environment.DTime; dt <= wallHit.DTime; dt += _Environment.DTime)
                            {
                                Vector2 unitPos;
                                if (dodgeMove.Vertical == VerticalMovement.None) //TODO jump time (<_>)
                                    unitPos = PredictPos(this, dt, dodgeMove.Horizontal, false);
                                else if (dodgeMove.Vertical == VerticalMovement.JumpOff)
                                    unitPos = PredictPos(this, dt, dodgeMove.Horizontal, true);
                                else
                                    unitPos = Pos + moveVelocity * dt;

                                var movedUnit = Clone(unitPos);

                                if (wallTiles.Any(t => t.Intersects(movedUnit)))
                                    break;

                                if (bullet.FindHit(new[] { movedUnit }) == null) //TODO Move bullet by dt too???
                                {
                                    if (!dodgeTime.HasValue)
                                        dodgeTime = dt;
                                }
                                else if (dodgeTime.HasValue)
                                {
                                    dodgeTime = null;

                                    break;
                                }
                            }

                            if (dodgeTime.HasValue &&
                                dodgeTime.Value < minDodgeTime &&
                                dodgeTime.Value < unitHit.DTime + _Environment.DTime)
                            {
                                result = dodgeMove;
                                minDodgeTime = dodgeTime.Value;
                            }
                        }

                        if (result != null)
                        {
                            if (unitHit.DTime > minDodgeTime + 3 * _Environment.DTime)
                                result = null;

                            break;
                        }
                    }
                }
            }

            return result;
        }

        private Vector2 Convert(Movement move)
        {
            float dx;
            switch (move.Horizontal)
            {
                case HorizontalMovement.None:
                    dx = 0;
                    break;
                case HorizontalMovement.Left:
                    dx = -_RunSpeed;
                    break;
                case HorizontalMovement.Right:
                    dx = _RunSpeed;
                    break;
                default: throw new System.ArgumentOutOfRangeException(nameof(move.Horizontal));
            }

            float dy;
            switch (move.Vertical)
            {
                case VerticalMovement.None:
                    dy = 0;
                    break;
                case VerticalMovement.Jump:
                    dy = _JumpSpeed;
                    break;
                case VerticalMovement.JumpOff:
                    dy = -VerticalDynamic.FallSpeed;
                    break;
                default: throw new System.ArgumentOutOfRangeException(nameof(move.Vertical));
            }

            return new Vector2(dx, dy);
        }

        private Unit Clone(Vector2 pos) => new Unit(_Id, PlayerId, pos, _Weapon, _Size, _RunSpeed, _JumpSpeed, _JumpTimeLeft, _MaxJumpTime, VerticalDynamic, Health, MaxHealth, _Environment, _Scoretable);

        private static Vector2 TakeCenter(Unit unit) => TakeCenter(unit, unit.Pos);
        private static Vector2 TakeCenter(Unit unit, Vector2 pos) => pos + unit.Height / 2 * Vector2.UnitY;
    }
}
