using MLG360.Model.Debugging;
using System.IO;

namespace MLG360
{
    public class Debug
    {
        private BinaryWriter writer;

        public static Debug Instance { get; private set; }

        public Debug(BinaryWriter writer)
        {
            this.writer = writer;

            Instance = this;
        }

        public void Draw(DebugData customData)
        {
            new Model.Messages.DebugMessage(customData).WriteTo(writer);
            writer.Flush();
        }
    }
}