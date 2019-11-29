namespace MLG360.Model.CustomData
{
    public class Log : CustomData
    {
        public const int TAG = 0;

        public string Text { get; set; }

        public Log() { }

        public Log(string text)
        {
            Text = text;
        }

        public static new Log ReadFrom(System.IO.BinaryReader reader)
        {
            if (reader == null)
                throw new System.ArgumentNullException(nameof(reader));

            var result = new Log();
            result.Text = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadInt32()));

            return result;
        }

        public override void WriteTo(System.IO.BinaryWriter writer)
        {
            if (writer == null)
                throw new System.ArgumentNullException(nameof(writer));

            writer.Write(TAG);
            var textData = System.Text.Encoding.UTF8.GetBytes(Text);
            writer.Write(textData.Length);
            writer.Write(textData);
        }
    }
}
