using System.Numerics;

namespace MLG360.Strategy
{
    internal class Tile
    {
        public Vector2 Pos { get; }
        public TileType Type { get; }

        public Tile(Vector2 pos, TileType type)
        {
            Pos = pos;
            Type = type;
        }

        public bool Contains(Unit unit) => Pos == new Vector2(System.MathF.Round(unit.Pos.X), System.MathF.Round(unit.Pos.Y));
    }
}