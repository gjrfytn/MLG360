namespace MLG360.Model
{
    public struct Game
    {
        public int CurrentTick { get; set; }
        public Properties Properties { get; set; }
        public Level Level { get; set; }
        public Player[] Players { get; set; }
        public Unit[] Units { get; set; }
        public Bullet[] Bullets { get; set; }
        public Mine[] Mines { get; set; }
        public LootBox[] LootBoxes { get; set; }

        public Game(int currentTick, Properties properties, Level level, Player[] players, Unit[] units, Bullet[] bullets, Mine[] mines, LootBox[] lootBoxes)
        {
            CurrentTick = currentTick;
            Properties = properties;
            Level = level;
            Players = players;
            Units = units;
            Bullets = bullets;
            Mines = mines;
            LootBoxes = lootBoxes;
        }

        public static Game ReadFrom(System.IO.BinaryReader reader)
        {
            var result = new Game();
            result.CurrentTick = reader.ReadInt32();
            result.Properties = Properties.ReadFrom(reader);
            result.Level = Level.ReadFrom(reader);
            result.Players = new Player[reader.ReadInt32()];

            for (var i = 0; i < result.Players.Length; i++)
            {
                result.Players[i] = Player.ReadFrom(reader);
            }

            result.Units = new Unit[reader.ReadInt32()];
            for (var i = 0; i < result.Units.Length; i++)
            {
                result.Units[i] = Unit.ReadFrom(reader);
            }

            result.Bullets = new Bullet[reader.ReadInt32()];
            for (var i = 0; i < result.Bullets.Length; i++)
            {
                result.Bullets[i] = Bullet.ReadFrom(reader);
            }

            result.Mines = new Mine[reader.ReadInt32()];
            for (var i = 0; i < result.Mines.Length; i++)
            {
                result.Mines[i] = Mine.ReadFrom(reader);
            }

            result.LootBoxes = new LootBox[reader.ReadInt32()];
            for (var i = 0; i < result.LootBoxes.Length; i++)
            {
                result.LootBoxes[i] = LootBox.ReadFrom(reader);
            }

            return result;
        }

        public void WriteTo(System.IO.BinaryWriter writer)
        {
            writer.Write(CurrentTick);
            Properties.WriteTo(writer);
            Level.WriteTo(writer);
            writer.Write(Players.Length);
            foreach (var playersElement in Players)
            {
                playersElement.WriteTo(writer);
            }

            writer.Write(Units.Length);
            foreach (var unitsElement in Units)
            {
                unitsElement.WriteTo(writer);
            }

            writer.Write(Bullets.Length);
            foreach (var bulletsElement in Bullets)
            {
                bulletsElement.WriteTo(writer);
            }

            writer.Write(Mines.Length);
            foreach (var minesElement in Mines)
            {
                minesElement.WriteTo(writer);
            }

            writer.Write(LootBoxes.Length);
            foreach (var lootBoxesElement in LootBoxes)
            {
                lootBoxesElement.WriteTo(writer);
            }
        }
    }
}
