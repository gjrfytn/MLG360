namespace MLG360.Model
{
    public class Game
    {
        public int CurrentTick { get; }
        public Properties Properties { get; }
        public Level Level { get; }
        public Player[] Players { get; }
        public Unit[] Units { get; }
        public Bullet[] Bullets { get; }
        public Mine[] Mines { get; }
        public LootBox[] LootBoxes { get; }

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
            if (reader == null)
                throw new System.ArgumentNullException(nameof(reader));

            var currentTick = reader.ReadInt32();
            var properties = Properties.ReadFrom(reader);
            var level = Level.ReadFrom(reader);
            var players = new Player[reader.ReadInt32()];

            for (var i = 0; i < players.Length; i++)
            {
                players[i] = Player.ReadFrom(reader);
            }

            var units = new Unit[reader.ReadInt32()];
            for (var i = 0; i < units.Length; i++)
            {
                units[i] = Unit.ReadFrom(reader);
            }

            var bullets = new Bullet[reader.ReadInt32()];
            for (var i = 0; i < bullets.Length; i++)
            {
                bullets[i] = Bullet.ReadFrom(reader);
            }

            var mines = new Mine[reader.ReadInt32()];
            for (var i = 0; i < mines.Length; i++)
            {
                mines[i] = Mine.ReadFrom(reader);
            }

            var lootBoxes = new LootBox[reader.ReadInt32()];
            for (var i = 0; i < lootBoxes.Length; i++)
            {
                lootBoxes[i] = LootBox.ReadFrom(reader);
            }

            var result = new Game(currentTick, properties, level, players, units, bullets, mines, lootBoxes);

            return result;
        }

        public void WriteTo(System.IO.BinaryWriter writer)
        {
            if (writer == null)
                throw new System.ArgumentNullException(nameof(writer));

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
