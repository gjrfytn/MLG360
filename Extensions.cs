using MLG360.Model;
using System.Numerics;

namespace MLG360
{
    internal static class Extensions
    {
        public static Vec2Float CastToVec2Float(this Vec2Double value) => new Vec2Float((float)value.X, (float)value.Y);
        public static Vector2 CastToVector2(this Vec2Double value) => new Vector2((float)value.X, (float)value.Y);
        public static Vec2Float Convert(this Vector2 value) => new Vec2Float(value.X, value.Y);

        public static Strategy.Unit Convert(this Unit value, Game game)
        {
            Strategy.VerticalDynamic.Type verticalDynamicType;
            if (value.JumpState.MaxTime == 0)
                verticalDynamicType = Strategy.VerticalDynamic.Type.Falling;
            else if (!value.JumpState.CanCancel)
                verticalDynamicType = Strategy.VerticalDynamic.Type.ThrownUp;
            else
                verticalDynamicType = Strategy.VerticalDynamic.Type.None;

            var verticalDynamic = new Strategy.VerticalDynamic(
                verticalDynamicType,
                (float)game.Properties.UnitFallSpeed,
                (float)game.Properties.JumpPadJumpSpeed,
                (float)value.JumpState.MaxTime);

            return new Strategy.Unit(
                value.PlayerId,
                value.Position.CastToVector2(),
                value.Weapon != null ? new Strategy.Weapon((float)value.Weapon.Parameters.Bullet.Speed) : null,
                (float)value.Size.Y,
                //(float)_Game.Properties.UnitSize.Y, u.Stand ? HorizontalMovement.None : (u.WalkedRight ? HorizontalMovement.Right : HorizontalMovement.Left),
                verticalDynamic);
        }
    }
}
