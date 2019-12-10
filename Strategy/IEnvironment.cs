using System.Collections.Generic;

namespace MLG360.Strategy
{
    internal interface IEnvironment
    {
        float DTime { get; }
        IEnumerable<Unit> Units { get; }
        IEnumerable<Gun> Guns { get; }
        IEnumerable<HealthPack> HealthPacks { get; }
        IEnumerable<Tile> Tiles { get; }
        IEnumerable<Bullet> Bullets { get; }

        Tile GetLeftTile(Tile tile);
        Tile GetRightTile(Tile tile);
    }
}