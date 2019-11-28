using System.IO;

namespace MLG360
{
    public class Debug
    {
        private BinaryWriter writer;
        public Debug(BinaryWriter writer)
        {
            this.writer = writer;
        }
        public void Draw(Model.CustomData customData)
        {
            new Model.PlayerMessageGame.CustomDataMessage(customData).WriteTo(writer);
            writer.Flush();
        }
    }
}