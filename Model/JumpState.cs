namespace MLG360.Model
{
    public struct JumpState
    {
        public bool CanJump { get; set; }
        public double Speed { get; set; }
        public double MaxTime { get; set; }
        public bool CanCancel { get; set; }

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

            var result = new JumpState();
            result.CanJump = reader.ReadBoolean();
            result.Speed = reader.ReadDouble();
            result.MaxTime = reader.ReadDouble();
            result.CanCancel = reader.ReadBoolean();

            return result;
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
