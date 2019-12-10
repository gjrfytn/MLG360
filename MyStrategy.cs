using MLG360.Model;
using MLG360.Strategy;

namespace MLG360
{
    public class MyStrategy
    {
        public UnitAction GetAction(Model.Unit unit, Game game, Debug debug)
        {
            if (unit == null)
                throw new System.ArgumentNullException(nameof(unit));
            if (game == null)
                throw new System.ArgumentNullException(nameof(game));
            if (debug == null)
                throw new System.ArgumentNullException(nameof(debug));

            var aiUnit = unit.Convert(game);
            var action = aiUnit.Act();

            double velocity;
            switch (action.Movement.Horizontal)
            {
                case HorizontalMovement.None:
                    velocity = 0;
                    break;
                case HorizontalMovement.Left:
                    velocity = -game.Properties.UnitMaxHorizontalSpeed;
                    break;
                case HorizontalMovement.Right:
                    velocity = game.Properties.UnitMaxHorizontalSpeed;
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException(nameof(action.Movement.Horizontal));
            }

            return new UnitAction(
                velocity,
                action.Movement.Vertical == VerticalMovement.Jump,
                action.Movement.Vertical == VerticalMovement.JumpOff,
                new Vec2Double(action.WeaponOperation.Aim.X, action.WeaponOperation.Aim.Y),
                action.WeaponOperation.Action == WeaponOperation.ActionType.Shoot,
                action.WeaponOperation.Action == WeaponOperation.ActionType.Reload,
                false,
                false);
        }
    }
}