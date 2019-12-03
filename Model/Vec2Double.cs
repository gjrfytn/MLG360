namespace MLG360.Model
{
    public class Vec2Double
    {
        public double X { get; }
        public double Y { get; }

        public Vec2Double(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Vec2Double ReadFrom(System.IO.BinaryReader reader)
        {
            if (reader == null)
                throw new System.ArgumentNullException(nameof(reader));

            return new Vec2Double(reader.ReadDouble(), reader.ReadDouble());
        }

        public void WriteTo(System.IO.BinaryWriter writer)
        {
            if (writer == null)
                throw new System.ArgumentNullException(nameof(writer));

            writer.Write(X);
            writer.Write(Y);
        }
    }
}
