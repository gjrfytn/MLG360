using MLG360.Model;
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
            var aiUnit = new Strategy.Unit(unit.PlayerId, unit.Position.CastToVector2(), new Strategy.Weapon(), (float)game.Properties.UnitSize.Y);
            var action = aiUnit.Act(environment);

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

            var unitWeaponPos = aiUnit.Pos + aiUnit.WeaponHeight * Vector2.UnitY;
            debug.Draw(
                new Model.Debugging.Line(unitWeaponPos.Convert(),
                (unitWeaponPos + 30 * action.Aim).Convert(),
                0.1f,
                new ColorFloat(1, 0, 0, 0.5f)));

            return new UnitAction(
                velocity,
                action.VerticalMovement == VerticalMovement.Jump,
                action.VerticalMovement == VerticalMovement.JumpOff,
                new Vec2Double(action.Aim.X, action.Aim.Y),
                action.WeaponOperation == WeaponOperation.Shoot,
                action.WeaponOperation == WeaponOperation.Reload,
                false,
                false);
        }
    }
}