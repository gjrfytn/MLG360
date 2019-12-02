using System.Collections.Generic;
using System.Linq;

namespace MLG360.Model
{
    public class Game
    {
        private readonly Player[] _Players;
        private readonly Unit[] _Units;
        private readonly Bullet[] _Bullets;
        private readonly Mine[] _Mines;
        private readonly LootBox[] _LootBoxes;

        public int CurrentTick { get; }
        public Properties Properties { get; }
        public Level Level { get; }
        public IEnumerable<Player> Players => _Players;
        public IEnumerable<Unit> Units => _Units;
        public IEnumerable<Bullet> Bullets => _Bullets;
        public IEnumerable<Mine> Mines => _Mines;
        public IEnumerable<LootBox> LootBoxes => _LootBoxes;

        public Game(
            int currentTick,
            Properties properties,
            Level level,
            IEnumerable<Player> players,
            IEnumerable<Unit> units,
            IEnumerable<Bullet> bullets,
            IEnumerable<Mine> mines,
            IEnumerable<LootBox> lootBoxes)
        {
            CurrentTick = currentTick;
            Properties = properties;
            Level = level;
            _Players = players.ToArray();
            _Units = units.ToArray();
            _Bullets = bullets.ToArray();
            _Mines = mines.ToArray();
            _LootBoxes = lootBoxes.ToArray();
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

            writer.Write(_Players.Length);
            foreach (var playersElement in _Players)
            {
                playersElement.WriteTo(writer);
            }

            writer.Write(_Units.Length);
            foreach (var unitsElement in _Units)
            {
                unitsElement.WriteTo(writer);
            }

            writer.Write(_Bullets.Length);
            foreach (var bulletsElement in _Bullets)
            {
                bulletsElement.WriteTo(writer);
            }

            writer.Write(_Mines.Length);
            foreach (var minesElement in _Mines)
            {
                minesElement.WriteTo(writer);
            }

            writer.Write(_LootBoxes.Length);
            foreach (var lootBoxesElement in _LootBoxes)
            {
                lootBoxesElement.WriteTo(writer);
            }
        }
    }
}
