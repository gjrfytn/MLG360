using MLG360.Model;
using System.Numerics;

namespace MLG360
{
    internal static class Extensions
    {
        public static Vec2Float CastToVec2Float(this Vec2Double value) => new Vec2Float((float)value.X, (float)value.Y);
        public static Vector2 CastToVector2(this Vec2Double value) => new Vector2((float)value.X, (float)value.Y);
        public static Vec2Float Convert(this Vector2 value) => new Vec2Float(value.X, value.Y);

        public static Strategy.Unit Convert(this Unit value, Game game, bool tempWorkaround)
        {
            Strategy.VerticalDynamic.Type verticalDynamicType;
            if (value.JumpState.MaxTime == 0 || tempWorkaround)
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
                value.Id,
                value.PlayerId,
                value.Position.CastToVector2(),
                value.Weapon?.Convert(),
                value.Size.CastToVector2(),
                (float)game.Properties.UnitMaxHorizontalSpeed,
                (float)game.Properties.UnitJumpSpeed,
                (float)value.JumpState.MaxTime,
                (float)game.Properties.UnitJumpTime,
                //(float)_Game.Properties.UnitSize.Y, u.Stand ? HorizontalMovement.None : (u.WalkedRight ? HorizontalMovement.Right : HorizontalMovement.Left),
                verticalDynamic,
                value.Health,
                game.Properties.UnitMaxHealth,
                new Environment(game),
                new Scoretable(game));
        }

        public static Strategy.Weapon Convert(this Weapon value)
        {
            return new Strategy.Weapon(
                (float)value.Parameters.Bullet.Speed,
                value.Parameters.Bullet.Damage,
                (float)(2 * value.Parameters.Explosion?.Radius ?? 0),
                value.Parameters.Explosion?.Damage ?? 0,
                (float)value.Parameters.Bullet.Size,
                (float)(value.FireTimer ?? 0),
                (float)value.Parameters.FireRate,
                (float)value.Spread);
        }
    }
}
