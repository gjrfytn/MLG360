using System.Numerics;

namespace MLG360.Strategy
{
    internal class Tile : Rectangle
    {
        private readonly Vector2 _Size;
        private readonly Vector2 _HalfSize;

        public Vector2 Pos { get; }
        public TileType Type { get; }
        public Vector2 Top => Pos + _HalfSize.Y * Vector2.UnitY;
        public Vector2 Bottom => Pos - _HalfSize.Y * Vector2.UnitY;
        public bool IsWall => Type == TileType.Wall;

        protected override Vector2 Center => Pos;
        protected override float Width => _Size.X;
        protected override float Height => _Size.Y;

        public Tile(Vector2 pos, TileType type, Vector2 size)
        {
            Pos = pos;
            Type = type;
            _Size = size;
            _HalfSize = _Size / 2;
        }
    }
}