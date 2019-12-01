namespace MLG360.Model
{
    public class Vec2Float
    {
        public float X { get; }
        public float Y { get; }

        public Vec2Float(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Vec2Float ReadFrom(System.IO.BinaryReader reader)
        {
            if (reader == null)
                throw new System.ArgumentNullException(nameof(reader));

            var result = new Vec2Float(reader.ReadSingle(), reader.ReadSingle());

            return result;
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
