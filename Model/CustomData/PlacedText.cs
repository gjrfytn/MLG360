namespace MLG360.Model.CustomData
{
    public class PlacedText : CustomData
    {
        public const int TAG = 4;
        public string Text { get; set; }
        public Vec2Float Pos { get; set; }
        public TextAlignment Alignment { get; set; }
        public float Size { get; set; }
        public ColorFloat Color { get; set; }

        public PlacedText() { }

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

            var result = new PlacedText();
            result.Text = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadInt32()));
            result.Pos = Vec2Float.ReadFrom(reader);
            switch (reader.ReadInt32())
            {
                case 0:
                    result.Alignment = TextAlignment.Left;
                    break;
                case 1:
                    result.Alignment = TextAlignment.Center;
                    break;
                case 2:
                    result.Alignment = TextAlignment.Right;
                    break;
                default:
                    throw new System.Exception("Unexpected discriminant value");
            }

            result.Size = reader.ReadSingle();
            result.Color = ColorFloat.ReadFrom(reader);

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
            Pos.WriteTo(writer);
            writer.Write((int)Alignment);
            writer.Write(Size);
            Color.WriteTo(writer);
        }
    }
}
