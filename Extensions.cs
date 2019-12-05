using MLG360.Model;
using System.Numerics;

namespace MLG360
{
    internal static class Extensions
    {
        public static Vec2Float CastToVec2Float(this Vec2Double value) => new Vec2Float((float)value.X, (float)value.Y);
        public static Vector2 CastToVector2(this Vec2Double value) => new Vector2((float)value.X, (float)value.Y);
    }
}
