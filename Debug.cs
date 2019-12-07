using MLG360.Model.Debugging;
using System.IO;

namespace MLG360
{
    public class Debug
    {
        private readonly BinaryWriter _Writer;

        public static Debug Instance { get; private set; }

        public Debug(BinaryWriter writer)
        {
            _Writer = writer;

            Instance = this;
        }

        public void Draw(DebugData customData)
        {
            new Model.Messages.DebugMessage(customData).WriteTo(_Writer);
            _Writer.Flush();
        }
    }
}