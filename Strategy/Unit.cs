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

        public Unit(int playerId, Vector2 pos, Weapon weapon)
        {
            PlayerId = playerId;
            Pos = pos;
            _Weapon = weapon;
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

            Vector2 aim;
            if (closestEnemy != null)
                aim = Vector2.Normalize(closestEnemy.Pos - Pos);
            else
                aim = Vector2.UnitX;

            var currentTile = environment.Tiles.Single(t => t.Contains(this));
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
    }
}
