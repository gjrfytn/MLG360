using MLG360.Strategy;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace MLG360
{
    internal class Environment : IEnvironment
    {
        private const float _TileSize = 1;

        private readonly Model.Game _Game;

        public Environment(Model.Game game)
        {
            _Game = game;
        }

        public IEnumerable<Unit> Units => _Game.Units.Select(u => u.Convert(_Game));
        public IEnumerable<Gun> Guns => _Game.LootBoxes.Where(b => b.Item is Model.Items.Weapon).Select(w => new Gun(w.Position.CastToVector2()));
        public IEnumerable<HealthPack> HealthPacks => _Game.LootBoxes.Where(b => b.Item is Model.Items.HealthPack).Select(p => new HealthPack(p.Position.CastToVector2()));

        private List<Tile> _Tiles;
        public IEnumerable<Tile> Tiles
        {
            get
            {
                if (_Tiles == null)
                {
                    _Tiles = new List<Tile>();

                    const float tileHalfSize = _TileSize / 2;
                    for (var x = 0; x < _Game.Level.Tiles.Length; ++x)
                        for (var y = 0; y < _Game.Level.Tiles[x].Length; ++y)
                            _Tiles.Add(new Tile(new Vector2(x + tileHalfSize, y + tileHalfSize), Convert(_Game.Level.Tiles[x][y]), new Vector2(_TileSize, _TileSize)));
                }

                return _Tiles;
            }
        }

        private TileType Convert(Model.Tile value)
        {
            switch (value)
            {
                case Model.Tile.Empty: return TileType.Empty;
                case Model.Tile.Wall: return TileType.Wall;
                case Model.Tile.Platform: return TileType.Platform;
                case Model.Tile.Ladder: return TileType.Ladder;
                case Model.Tile.JumpPad: return TileType.JumpPad;
                default: throw new System.ArgumentOutOfRangeException(nameof(value));
            }
        }

        public Tile GetLeftTile(Tile tile) => Tiles.SingleOrDefault(t => t.Pos.X == tile.Pos.X - _TileSize && t.Pos.Y == tile.Pos.Y);
        public Tile GetRightTile(Tile tile) => Tiles.SingleOrDefault(t => t.Pos.X == tile.Pos.X + _TileSize && t.Pos.Y == tile.Pos.Y);
    }
}