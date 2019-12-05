using MLG360.Model.Debugging;

namespace MLG360.Model.Messages
{
    public class DebugMessage : PlayerMessage
    {
        public const int TAG = 0;

        public DebugData Data { get; }

        public DebugMessage(DebugData data)
        {
            Data = data;
        }

        public static new DebugMessage ReadFrom(System.IO.BinaryReader reader)
        {
            return new DebugMessage(DebugData.ReadFrom(reader));
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
