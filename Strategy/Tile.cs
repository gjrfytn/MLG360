using System.Numerics;

namespace MLG360.Strategy
{
    internal class Tile
    {
        private readonly Vector2 _Size;
        private readonly Vector2 _HalfSize;

        public Vector2 Pos { get; }
        public TileType Type { get; }
        public Vector2 Top => Pos + _HalfSize.Y * Vector2.UnitY;
        public Vector2 TopLeft => Pos + new Vector2(-_HalfSize.X, _HalfSize.Y);
        public Vector2 TopRight => Pos + new Vector2(_HalfSize.X, _HalfSize.Y);
        public Vector2 BottomRight => Pos + new Vector2(_HalfSize.X, -_HalfSize.Y);
        public Vector2 BottomLeft => Pos - new Vector2(_HalfSize.X, _HalfSize.Y);
        public bool IsWall => Type == TileType.Wall;

        public Tile(Vector2 pos, TileType type, Vector2 size)
        {
            Pos = pos;
            Type = type;
            _Size = size;
            _HalfSize = _Size / 2;
        }

        public bool Contains(Vector2 pos) => InXArea(pos) && InYArea(pos);
        public bool InXArea(Vector2 pos) => Pos.X - _HalfSize.X <= pos.X && Pos.X + _HalfSize.X > pos.X;
        public bool InYArea(Vector2 pos) => Pos.Y - _HalfSize.Y <= pos.Y && Pos.Y + _HalfSize.Y > pos.Y;
    }
}