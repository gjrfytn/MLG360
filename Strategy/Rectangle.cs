using System.Numerics;

namespace MLG360.Strategy
{
    internal abstract class Rectangle
    {
        private const float _Epsilon = 0.000001f;

        private float HalfWidth => Width / 2;
        private float HalfHeight => Height / 2;
        private Vector2 TopLeft => Center + new Vector2(-HalfWidth, HalfHeight - _Epsilon);
        private Vector2 TopRight => Center + new Vector2(HalfWidth - _Epsilon, HalfHeight - _Epsilon);
        private Vector2 BottomRight => Center + new Vector2(HalfWidth - _Epsilon, -HalfHeight);
        private Vector2 BottomLeft => Center - new Vector2(HalfWidth, HalfHeight);

        protected abstract Vector2 Center { get; }
        protected abstract float Width { get; }
        protected abstract float Height { get; }

        public bool Contains(Vector2 pos) => InXArea(pos) && InYArea(pos);
        public bool InXArea(Vector2 pos) => Center.X - HalfWidth <= pos.X && Center.X + HalfWidth > pos.X;
        public bool InYArea(Vector2 pos) => Center.Y - HalfHeight <= pos.Y && Center.Y + HalfHeight > pos.Y;
        public bool Intersects(Rectangle rect) => Contains(rect.TopLeft) || Contains(rect.TopRight) || Contains(rect.BottomRight) || Contains(rect.BottomLeft) ||
                                                  rect.Contains(TopLeft) || rect.Contains(TopRight) || rect.Contains(BottomRight) || rect.Contains(BottomLeft);
    }
}
