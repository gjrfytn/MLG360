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
            Unit? nearestEnemy = null;
            foreach (var other in game.Units)
            {
                if (other.PlayerId != unit.PlayerId)
                {
                    if (!nearestEnemy.HasValue || DistanceSqr(unit.Position, other.Position) < DistanceSqr(unit.Position, nearestEnemy.Value.Position))
                    {
                        nearestEnemy = other;
                    }
                }
            }

            LootBox? nearestWeapon = null;
            foreach (var lootBox in game.LootBoxes)
            {
                if (lootBox.Item is Item.Weapon)
                {
                    if (!nearestWeapon.HasValue || DistanceSqr(unit.Position, lootBox.Position) < DistanceSqr(unit.Position, nearestWeapon.Value.Position))
                    {
                        nearestWeapon = lootBox;
                    }
                }
            }

            var targetPos = unit.Position;
            if (!unit.Weapon.HasValue && nearestWeapon.HasValue)
            {
                targetPos = nearestWeapon.Value.Position;
            }
            else if (nearestEnemy.HasValue)
            {
                targetPos = nearestEnemy.Value.Position;
            }

            debug.Draw(new CustomData.Log("Target pos: " + targetPos));
            var aim = new Vec2Double(0, 0);
            if (nearestEnemy.HasValue)
            {
                aim = new Vec2Double(nearestEnemy.Value.Position.X - unit.Position.X, nearestEnemy.Value.Position.Y - unit.Position.Y);
            }

            var jump = targetPos.Y > unit.Position.Y;
            if (targetPos.X > unit.Position.X && game.Level.Tiles[(int)(unit.Position.X + 1)][(int)unit.Position.Y] == Tile.Wall)
            {
                jump = true;
            }

            if (targetPos.X < unit.Position.X && game.Level.Tiles[(int)(unit.Position.X - 1)][(int)unit.Position.Y] == Tile.Wall)
            {
                jump = true;
            }

            var action = new UnitAction
            {
                Velocity = targetPos.X - unit.Position.X,
                Jump = jump,
                JumpDown = !jump,
                Aim = aim,
                Shoot = true,
                SwapWeapon = false,
                PlantMine = false
            };

            return action;
        }
    }
}