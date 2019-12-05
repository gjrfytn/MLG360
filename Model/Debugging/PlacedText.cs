namespace MLG360.Model.Debugging
{
    public class PlacedText : DebugData
    {
        public const int TAG = 4;
        public string Text { get; }
        public Vec2Float Pos { get; }
        public TextAlignment Alignment { get; }
        public float Size { get; }
        public ColorFloat Color { get; }

        public PlacedText(string text, Vec2Float pos, TextAlignment alignment, float size, ColorFloat color)
        {
            Text = text;
            Pos = pos;
            Alignment = alignment;
            Size = size;
            Color = color;
        }

        public static new PlacedText ReadFrom(System.IO.BinaryReader reader)
        {
            if (reader == null)
                throw new System.ArgumentNullException(nameof(reader));

            var text = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadInt32()));
            var pos = Vec2Float.ReadFrom(reader);
            TextAlignment alignment;
            switch (reader.ReadInt32())
            {
                case 0:
                    alignment = TextAlignment.Left;
                    break;
                case 1:
                    alignment = TextAlignment.Center;
                    break;
                case 2:
                    alignment = TextAlignment.Right;
                    break;
                default:
                    throw new System.Exception("Unexpected discriminant value");
            }

            var size = reader.ReadSingle();
            var color = ColorFloat.ReadFrom(reader);

            return new PlacedText(text, pos, alignment, size, color);
        }

        public override void WriteTo(System.IO.BinaryWriter writer)
        {
            if (writer == null)
                throw new System.ArgumentNullException(nameof(writer));

            writer.Write(TAG);
            var textData = System.Text.Encoding.UTF8.GetBytes(Text);
            writer.Write(textData.Length);
            writer.Write(textData);
            Pos.WriteTo(writer);
            writer.Write((int)Alignment);
            writer.Write(Size);
            Color.WriteTo(writer);
        }
    }
}
