namespace MLG360.Model.Debugging
{
    public class Line : DebugData
    {
        public const int TAG = 2;

        public Vec2Float P1 { get; }
        public Vec2Float P2 { get; }
        public float Width { get; }
        public ColorFloat Color { get; }

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

            return new Line(Vec2Float.ReadFrom(reader), Vec2Float.ReadFrom(reader), reader.ReadSingle(), ColorFloat.ReadFrom(reader));
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
