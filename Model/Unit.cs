namespace MLG360.Model
{
    public class Unit
    {
        public int PlayerId { get; }
        public int Id { get; }
        public int Health { get; }
        public Vec2Double Position { get; }
        public Vec2Double Size { get; }
        public JumpState JumpState { get; }
        public bool WalkedRight { get; }
        public bool Stand { get; }
        public bool OnGround { get; }
        public bool OnLadder { get; }
        public int Mines { get; }
        public Weapon Weapon { get; }

        public Unit(int playerId, int id, int health, Vec2Double position, Vec2Double size, JumpState jumpState, bool walkedRight, bool stand, bool onGround, bool onLadder, int mines, Weapon weapon)
        {
            PlayerId = playerId;
            Id = id;
            Health = health;
            Position = position ?? throw new System.ArgumentNullException(nameof(position));
            Size = size ?? throw new System.ArgumentNullException(nameof(size));
            JumpState = jumpState ?? throw new System.ArgumentNullException(nameof(jumpState));
            WalkedRight = walkedRight;
            Stand = stand;
            OnGround = onGround;
            OnLadder = onLadder;
            Mines = mines;
            Weapon = weapon;
        }

        public static Unit ReadFrom(System.IO.BinaryReader reader)
        {
            if (reader == null)
                throw new System.ArgumentNullException(nameof(reader));

            var playerId = reader.ReadInt32();
            var id = reader.ReadInt32();
            var health = reader.ReadInt32();
            var position = Vec2Double.ReadFrom(reader);
            var size = Vec2Double.ReadFrom(reader);
            var jumpState = JumpState.ReadFrom(reader);
            var walkedRight = reader.ReadBoolean();
            var stand = reader.ReadBoolean();
            var onGround = reader.ReadBoolean();
            var onLadder = reader.ReadBoolean();
            var mines = reader.ReadInt32();
            var weapon = reader.ReadBoolean() ? Weapon.ReadFrom(reader) : null;

            return new Unit(playerId, id, health, position, size, jumpState, walkedRight, stand, onGround, onLadder, mines, weapon);
        }

        public void WriteTo(System.IO.BinaryWriter writer)
        {
            if (writer == null)
                throw new System.ArgumentNullException(nameof(writer));

            writer.Write(PlayerId);
            writer.Write(Id);
            writer.Write(Health);
            Position.WriteTo(writer);
            Size.WriteTo(writer);
            JumpState.WriteTo(writer);
            writer.Write(WalkedRight);
            writer.Write(Stand);
            writer.Write(OnGround);
            writer.Write(OnLadder);
            writer.Write(Mines);

            if (Weapon == null)
                writer.Write(false);
            else
            {
                writer.Write(true);
                Weapon.WriteTo(writer);
            }
        }
    }
}
