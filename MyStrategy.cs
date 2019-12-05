using MLG360.Model;

namespace MLG360
{
    public class MyStrategy
    {
        private static double DistanceSqr(Vec2Double a, Vec2Double b)
        {
            return (a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y);
        }

        public UnitAction GetAction(Unit unit, Game game, Debug debug)
        {
            if (unit == null)
                throw new System.ArgumentNullException(nameof(unit));
            if (game == null)
                throw new System.ArgumentNullException(nameof(game));
            if (debug == null)
                throw new System.ArgumentNullException(nameof(debug));

            Unit nearestEnemy = null;
            foreach (var other in game.Units)
                if (other.PlayerId != unit.PlayerId &&
                    (nearestEnemy == null || DistanceSqr(unit.Position, other.Position) < DistanceSqr(unit.Position, nearestEnemy.Position)))
                    nearestEnemy = other;

            LootBox nearestWeapon = null;
            foreach (var lootBox in game.LootBoxes)
                if (lootBox.Item is Model.Items.Weapon &&
                    (nearestWeapon == null || DistanceSqr(unit.Position, lootBox.Position) < DistanceSqr(unit.Position, nearestWeapon.Position)))
                    nearestWeapon = lootBox;

            var targetPos = unit.Position;
            if (unit.Weapon == null && nearestWeapon != null)
                targetPos = nearestWeapon.Position;
            else if (nearestEnemy != null)
                targetPos = nearestEnemy.Position;

            debug.Draw(new Model.Debugging.Log("Target pos: " + targetPos));
            var aim = new Vec2Double(0, 0);
            if (nearestEnemy != null)
                aim = new Vec2Double(nearestEnemy.Position.X - unit.Position.X, nearestEnemy.Position.Y - unit.Position.Y);

            var jump = targetPos.Y > unit.Position.Y;
            if (targetPos.X > unit.Position.X && game.Level.Tiles[(int)(unit.Position.X + 1)][(int)unit.Position.Y] == Tile.Wall)
                jump = true;

            if (targetPos.X < unit.Position.X && game.Level.Tiles[(int)(unit.Position.X - 1)][(int)unit.Position.Y] == Tile.Wall)
                jump = true;

            return new UnitAction(targetPos.X - unit.Position.X, jump, !jump, aim, true, false, false, false);
        }
    }
}