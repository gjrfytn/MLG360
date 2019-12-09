using System.Numerics;

namespace MLG360.Strategy
{
    internal class Loot : Rectangle, IGameObject
    {
        private readonly Vector2 _Size;

        public Vector2 Pos { get; }

        protected override Vector2 Center => Pos + _Size.Y * Vector2.UnitY; //TODO duplicate
        protected override float Width => _Size.X;
        protected override float Height => _Size.Y;

        public Loot(Vector2 pos, Vector2 size)
        {
            Pos = pos;
            _Size = size;
        }
    }
}