using System.Numerics;

namespace MLG360.Strategy
{
    internal abstract class GameObject : Rectangle
    {
        private readonly Vector2 _Size;

        public Vector2 Pos { get; } // Bottom center

        protected override Vector2 Center => Pos + _Size.Y * Vector2.UnitY;
        protected override float Width => _Size.X;
        protected override float Height => _Size.Y;

        public GameObject(Vector2 pos, Vector2 size)
        {
            Pos = pos;
            _Size = size;
        }
    }
}
