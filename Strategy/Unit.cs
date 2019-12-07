using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace MLG360.Strategy
{
    internal class Unit
    {
        private readonly Weapon _Weapon;

        public int PlayerId { get; }
        public Vector2 Pos { get; }
        public float Height { get; }
        public VerticalDynamic VerticalDynamic { get; }

        public float WeaponHeight => Height / 2;

        public Unit(int playerId, Vector2 pos, Weapon weapon, float height, VerticalDynamic verticalDynamic/*HorizontalMovement horizontalMovement, VerticalMovement verticalMovement*/)
        {
            PlayerId = playerId;
            Pos = pos;
            _Weapon = weapon;
            Height = height;
            VerticalDynamic = verticalDynamic;
        }

        public Action Act(IEnvironment environment)
        {
            Unit closestEnemy = null;
            foreach (var enemy in FindEnemies(environment))
                if (closestEnemy == null || Vector2.DistanceSquared(Pos, enemy.Pos) < Vector2.DistanceSquared(Pos, closestEnemy.Pos))
                    closestEnemy = enemy;

            Gun closestGun = null;
            foreach (var gun in environment.Guns)
                if (closestGun == null || Vector2.DistanceSquared(Pos, gun.Pos) < Vector2.DistanceSquared(Pos, closestGun.Pos))
                    closestGun = gun;

            Vector2 targetPos;
            if (_Weapon == null && closestGun != null)
                targetPos = closestGun.Pos;
            else if (closestEnemy != null)
                targetPos = closestEnemy.Pos;
            else
                targetPos = Pos;

            var aim = closestEnemy != null && _Weapon != null ? CalculateAim(closestEnemy, environment) : Vector2.UnitX;

            var currentTile = environment.Tiles.Single(t => t.Contains(Pos));
            bool jump;
            if (targetPos.X > Pos.X && environment.Tiles.Single(t => t.Pos.X == currentTile.Pos.X + 1 && t.Pos.Y == currentTile.Pos.Y).Type == TileType.Wall)
                jump = true;
            else if (targetPos.X < Pos.X && environment.Tiles.Single(t => t.Pos.X == currentTile.Pos.X - 1 && t.Pos.Y == currentTile.Pos.Y).Type == TileType.Wall)
                jump = true;
            else
                jump = targetPos.Y > Pos.Y;

            return new Action(
                targetPos.X > Pos.X ? HorizontalMovement.Right : HorizontalMovement.Left,
                jump ? VerticalMovement.Jump : VerticalMovement.JumpOff,
                aim,
                WeaponOperation.Shoot);
        }

        private IEnumerable<Unit> FindEnemies(IEnvironment environment) => environment.Units.Where(u => u.PlayerId != PlayerId);

        private Vector2 CalculateAim(Unit unit, IEnvironment environment)
        {
            var distance = Vector2.Distance(Pos, TakeCenter(unit));
            var timeToHit = distance / _Weapon.BulletSpeed; //TODO wrong?
            var aimPoint = TakeCenter(unit, PredictPos(unit, timeToHit, environment));

            return Vector2.Normalize(aimPoint - (Pos + WeaponHeight * Vector2.UnitY));
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

            return pos;
        }
    }
}
