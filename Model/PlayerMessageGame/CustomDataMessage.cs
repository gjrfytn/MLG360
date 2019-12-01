namespace MLG360.Model.PlayerMessageGame
{
    public class CustomDataMessage : PlayerMessageGame
    {
        public const int TAG = 0;

        public CustomData.CustomData Data { get; }

        public CustomDataMessage(CustomData.CustomData data)
        {
            Data = data;
        }

        public static new CustomDataMessage ReadFrom(System.IO.BinaryReader reader)
        {
            var result = new CustomDataMessage(CustomData.CustomData.ReadFrom(reader));

            return result;
        }

        public override void WriteTo(System.IO.BinaryWriter writer)
        {
            if (writer == null)
                throw new System.ArgumentNullException(nameof(writer));

            writer.Write(TAG);
            Data.WriteTo(writer);
        }
    }
}
