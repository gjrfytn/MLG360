namespace MLG360.Model
{
    public class JumpState
    {
        public bool CanJump { get; }
        public double Speed { get; }
        public double MaxTime { get; }
        public bool CanCancel { get; }

        public JumpState(bool canJump, double speed, double maxTime, bool canCancel)
        {
            CanJump = canJump;
            Speed = speed;
            MaxTime = maxTime;
            CanCancel = canCancel;
        }

        public static JumpState ReadFrom(System.IO.BinaryReader reader)
        {
            if (reader == null)
                throw new System.ArgumentNullException(nameof(reader));

            return new JumpState(reader.ReadBoolean(), reader.ReadDouble(), reader.ReadDouble(), reader.ReadBoolean());
        }

        public void WriteTo(System.IO.BinaryWriter writer)
        {
            if (writer == null)
                throw new System.ArgumentNullException(nameof(writer));

            writer.Write(CanJump);
            writer.Write(Speed);
            writer.Write(MaxTime);
            writer.Write(CanCancel);
        }
    }
}
