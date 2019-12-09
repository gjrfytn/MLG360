using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace MLG360.Strategy
{
    internal class Unit : GameObject
    {
        private readonly Vector2 _Size;
        private readonly Weapon _Weapon;

        public int PlayerId { get; }
        public VerticalDynamic VerticalDynamic { get; }
        public float Health { get; }
        public float MaxHealth { get; }

        private float WeaponHeight => Height / 2;
        private Vector2 WeaponPoint => Pos + WeaponHeight * Vector2.UnitY;

        public Unit(int playerId, Vector2 pos, Weapon weapon, Vector2 size, VerticalDynamic verticalDynamic, float health, float maxHealth) : base(pos, size)
        {
            PlayerId = playerId;
            _Weapon = weapon;
            _Size = size;
            VerticalDynamic = verticalDynamic;
            Health = health;
            MaxHealth = maxHealth;
        }

        public Action Act(IEnvironment environment)
        {
            var targetPos = Pos;
            var weaponOperation = new WeaponOperation(Vector2.UnitX, WeaponOperation.ActionType.None);
            if (_Weapon == null)
            {
                var closestGun = FindClosestGun(environment);
                if (closestGun != null)
                    targetPos = closestGun.Pos;
            }
            else
            {
                var closestEnemy = FindClosestEnemy(environment);
                if (closestEnemy != null)
                {
                    weaponOperation = OperateWeapon(closestEnemy, environment);

                    targetPos = PositionSelf(closestEnemy, weaponOperation.Action == WeaponOperation.ActionType.Shoot, environment);
                }
            }

            var currentTile = environment.Tiles.Single(t => t.Contains(Pos));
            VerticalMovement verticalMovement;
            if (targetPos.X > Pos.X && environment.GetRightTile(currentTile).IsWall ||
                targetPos.X < Pos.X && environment.GetLeftTile(currentTile).IsWall)
                verticalMovement = VerticalMovement.Jump;
            else
                verticalMovement = targetPos.Y > Pos.Y ? VerticalMovement.Jump : VerticalMovement.JumpOff;

            return new Action(
                targetPos.X > Pos.X ? HorizontalMovement.Right : HorizontalMovement.Left,
                verticalMovement,
                weaponOperation);
        }

        private IEnumerable<Unit> FindEnemies(IEnvironment environment) => environment.Units.Where(u => u.PlayerId != PlayerId);
        private Unit FindClosestEnemy(IEnvironment environment) => PickClosest(FindEnemies(environment));
        private Gun FindClosestGun(IEnvironment environment) => PickClosest(environment.Guns);
        private HealthPack FindClosestHealthPack(IEnvironment environment) => PickClosest(environment.HealthPacks);

        private Vector2 PositionSelf(Unit closestEnemy, bool enemyInSight, IEnvironment environment)
        {
            var closestHP = FindClosestHealthPack(environment);

            if (closestHP != null)
            {
                const float healthPanicThreshold = 0.5f;
                if (closestEnemy.Health >= Health && Health / MaxHealth <= healthPanicThreshold)
                    return closestHP.Pos;
            }

            if (enemyInSight || _Weapon.NeedsReload)
            {
                //TODO plant mine if hp is not too close.

                closestHP = FindClosestHealthPack(environment);
                if (closestHP != null)
                {
                    var tileWithHP = environment.Tiles.Single(t => t.Contains(closestHP.Pos));
                    var leftTile = environment.GetLeftTile(tileWithHP);
                    var rightTile = environment.GetRightTile(tileWithHP);

                    return Vector2.DistanceSquared(Pos, leftTile.Bottom) < Vector2.DistanceSquared(Pos, rightTile.Bottom) && !leftTile.IsWall ?
                        leftTile.Bottom : rightTile.Bottom;
                }
                else if (_Weapon.NeedsReload) //TODO remove check?
                {
                    var solidTiles = environment.Tiles.Where(t => t.IsWall || t.Type == TileType.Platform || t.Type == TileType.Ladder).ToArray();
                    var freeTiles = environment.Tiles.Where(t => (t.Type == TileType.Empty || t.Type == TileType.Ladder) &&
                                                                 solidTiles.Any(st => st.Pos.X == t.Pos.X && st.Pos.Y == t.Pos.Y - 1))
                                                     .OrderBy(t => Vector2.DistanceSquared(Pos, t.Bottom));

                    foreach (var tile in freeTiles)
                    {
                        var tileWeaponPoint = tile.Bottom + WeaponHeight * Vector2.UnitY;
                        var enemyCenter = TakeCenter(closestEnemy);
                        var aimToEnemy = enemyCenter - tileWeaponPoint;
                        var hasSight = HasLineOfSight(tileWeaponPoint, aimToEnemy, Vector2.Distance(tileWeaponPoint, enemyCenter), 0.01f, environment);

                        if (!hasSight)
                            return tile.Pos;
                    }
                }
            }

            return closestEnemy.Pos;
        }

        private Vector2 CalculateAim(Unit unit, IEnvironment environment)
        {
            var distance = Vector2.Distance(Pos, TakeCenter(unit));
            var timeToHit = distance / _Weapon.BulletSpeed; //TODO wrong?
            var aimPoint = TakeCenter(unit, PredictPos(unit, timeToHit, environment));

            return Vector2.Normalize(aimPoint - WeaponPoint);
        }

        private WeaponOperation OperateWeapon(Unit enemy, IEnvironment environment)
        {
            if (enemy == null || _Weapon == null)
                return new WeaponOperation(Vector2.UnitX, WeaponOperation.ActionType.None);

            var aim = CalculateAim(enemy, environment);
            var action = HasLineOfSight(WeaponPoint, aim, Vector2.Distance(WeaponPoint, TakeCenter(enemy)), 2 * _Weapon.BulletSize, environment) ?
                WeaponOperation.ActionType.Shoot : WeaponOperation.ActionType.None;

            #region DEBUG
#if DEBUG
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
#endif
            #endregion

            return new WeaponOperation(aim, action);
        }

        private T PickClosest<T>(IEnumerable<T> objects) where T : GameObject => objects.OrderBy(e => Vector2.DistanceSquared(Pos, e.Pos)).FirstOrDefault();

        private static bool HasLineOfSight(Vector2 from, Vector2 aim, float distance, float size, IEnvironment environment)
        {
            var halfSize = size / 2;
            var wallTiles = FindWallTiles(environment).ToArray();

            const float checkStep = 0.25f;
            for (var checkDist = checkStep; checkDist <= distance; checkDist += checkStep)
            {
                var checkPoint = from + checkDist * aim;

                var corners = new[]
                {
                    checkPoint + new Vector2(-halfSize, halfSize),
                    checkPoint + new Vector2(halfSize, halfSize),
                    checkPoint + new Vector2(halfSize, -halfSize),
                    checkPoint - new Vector2(halfSize, halfSize),
                };

                foreach (var corner in corners)
                    if (wallTiles.Any(t => t.Contains(corner)))
                        return false;
            }

            return true;
        }

        private static Vector2 TakeCenter(Unit unit) => TakeCenter(unit, unit.Pos);
        private static Vector2 TakeCenter(Unit unit, Vector2 pos) => pos + unit.Height / 2 * Vector2.UnitY;

        private static Vector2 PredictPos(Unit unit, float time, IEnvironment environment)
        {
            var dy = unit.VerticalDynamic.CalculateDPos(time);

            if (dy == 0)
                return unit.Pos;

            var pos = unit.Pos + dy * Vector2.UnitY;

            var solidTilesAbovePos = environment.Tiles
                                                .Where(t => t.Type != TileType.Empty &&
                                                            t.Pos.Y < unit.Pos.Y &&
                                                            t.Top.Y >= pos.Y &&
                                                            t.InXArea(pos))
                                                .ToArray();

            if (solidTilesAbovePos.Any())
                return new Vector2(unit.Pos.X, solidTilesAbovePos.OrderByDescending(t => t.Top.Y).First().Top.Y);

            // TODO Horizontal prediction.

            return pos;
        }

        public static IEnumerable<Tile> FindWallTiles(IEnvironment environment) => environment.Tiles.Where(t => t.IsWall);
    }
}
