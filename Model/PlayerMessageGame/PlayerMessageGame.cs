namespace MLG360.Model.PlayerMessageGame
{
    public abstract class PlayerMessageGame
    {
        public abstract void WriteTo(System.IO.BinaryWriter writer);

        public static PlayerMessageGame ReadFrom(System.IO.BinaryReader reader)
        {
            if (reader == null)
                throw new System.ArgumentNullException(nameof(reader));

            switch (reader.ReadInt32())
            {
                case CustomDataMessage.TAG:
                    return CustomDataMessage.ReadFrom(reader);
                case ActionMessage.TAG:
                    return ActionMessage.ReadFrom(reader);
                default:
                    throw new System.Exception("Unexpected discriminant value");
            }
        }
    }
}
