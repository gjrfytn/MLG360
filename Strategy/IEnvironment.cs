using System.Collections.Generic;

namespace MLG360.Strategy
{
    internal interface IEnvironment
    {
        IEnumerable<Unit> Units { get; }
        IEnumerable<Gun> Guns { get; }
        IEnumerable<HealthPack> HealthPacks { get; }
        IEnumerable<Tile> Tiles { get; }

        Tile GetLeftTile(Tile tile);
        Tile GetRightTile(Tile tile);
    }
}