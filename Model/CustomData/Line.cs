namespace MLG360.Model.CustomData
{
    public class Line : CustomData
    {
        public const int TAG = 2;

        public Vec2Float P1 { get; set; }
        public Vec2Float P2 { get; set; }
        public float Width { get; set; }
        public ColorFloat Color { get; set; }

        public Line() { }

        public Line(Vec2Float p1, Vec2Float p2, float width, ColorFloat color)
        {
            P1 = p1;
            P2 = p2;
            Width = width;
            Color = color;
        }

        public static new Line ReadFrom(System.IO.BinaryReader reader)
        {
            if (reader == null)
                throw new System.ArgumentNullException(nameof(reader));

            var result = new Line();
            result.P1 = Vec2Float.ReadFrom(reader);
            result.P2 = Vec2Float.ReadFrom(reader);
            result.Width = reader.ReadSingle();
            result.Color = ColorFloat.ReadFrom(reader);

            return result;
        }

        public override void WriteTo(System.IO.BinaryWriter writer)
        {
            if (writer == null)
                throw new System.ArgumentNullException(nameof(writer));

            writer.Write(TAG);
            P1.WriteTo(writer);
            P2.WriteTo(writer);
            writer.Write(Width);
            Color.WriteTo(writer);
        }
    }
}
