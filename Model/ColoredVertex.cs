namespace MLG360.Model
{
    public class ColoredVertex
    {
        public Vec2Float Position { get; }
        public ColorFloat Color { get; }

        public ColoredVertex(Vec2Float position, ColorFloat color)
        {
            Position = position;
            Color = color;
        }

        public static ColoredVertex ReadFrom(System.IO.BinaryReader reader)
        {
            return new ColoredVertex(Vec2Float.ReadFrom(reader), ColorFloat.ReadFrom(reader));
        }

        public void WriteTo(System.IO.BinaryWriter writer)
        {
            Position.WriteTo(writer);
            Color.WriteTo(writer);
        }
    }
}
