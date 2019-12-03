namespace MLG360.Model
{
    public class Level
    {
        public Tile[][] Tiles { get; }

        public Level(Tile[][] tiles)
        {
            Tiles = tiles;
        }

        public static Level ReadFrom(System.IO.BinaryReader reader)
        {
            if (reader == null)
                throw new System.ArgumentNullException(nameof(reader));

            var tiles = new Tile[reader.ReadInt32()][];
            for (var i = 0; i < tiles.Length; i++)
            {
                tiles[i] = new Tile[reader.ReadInt32()];
                for (var j = 0; j < tiles[i].Length; j++)
                {
                    switch (reader.ReadInt32())
                    {
                        case 0:
                            tiles[i][j] = Tile.Empty;
                            break;
                        case 1:
                            tiles[i][j] = Tile.Wall;
                            break;
                        case 2:
                            tiles[i][j] = Tile.Platform;
                            break;
                        case 3:
                            tiles[i][j] = Tile.Ladder;
                            break;
                        case 4:
                            tiles[i][j] = Tile.JumpPad;
                            break;
                        default:
                            throw new System.Exception("Unexpected discriminant value");
                    }
                }
            }

            return new Level(tiles);
        }

        public void WriteTo(System.IO.BinaryWriter writer)
        {
            if (writer == null)
                throw new System.ArgumentNullException(nameof(writer));

            writer.Write(Tiles.Length);
            foreach (var tilesElement in Tiles)
            {
                writer.Write(tilesElement.Length);
                foreach (var tilesElementElement in tilesElement)
                {
                    writer.Write((int)tilesElementElement);
                }
            }
        }
    }
}
