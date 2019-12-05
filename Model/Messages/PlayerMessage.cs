namespace MLG360.Model.Messages
{
    public abstract class PlayerMessage
    {
        public abstract void WriteTo(System.IO.BinaryWriter writer);

        public static PlayerMessage ReadFrom(System.IO.BinaryReader reader)
        {
            if (reader == null)
                throw new System.ArgumentNullException(nameof(reader));

            switch (reader.ReadInt32())
            {
                case DebugMessage.TAG:
                    return DebugMessage.ReadFrom(reader);
                case ActionMessage.TAG:
                    return ActionMessage.ReadFrom(reader);
                default:
                    throw new System.Exception("Unexpected discriminant value");
            }
        }
    }
}
