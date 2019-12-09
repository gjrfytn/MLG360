using MLG360.Model;
using MLG360.Model.Debugging;
using MLG360.Strategy;
using System.Numerics;

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

            var environment = new Environment(game);
            var aiUnit = unit.Convert(game, environment);
            var action = aiUnit.Act();

            double velocity;
            switch (action.HorizontalMovement)
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
                    throw new System.ArgumentOutOfRangeException(nameof(action.HorizontalMovement));
            }

            return new UnitAction(
                velocity,
                action.VerticalMovement == VerticalMovement.Jump,
                action.VerticalMovement == VerticalMovement.JumpOff,
                new Vec2Double(action.WeaponOperation.Aim.X, action.WeaponOperation.Aim.Y),
                action.WeaponOperation.Action == WeaponOperation.ActionType.Shoot,
                action.WeaponOperation.Action == WeaponOperation.ActionType.Reload,
                false,
                false);
        }
    }
}