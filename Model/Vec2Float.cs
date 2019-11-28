namespace MLG360.Model
{
    public struct Vec2Float
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Vec2Float(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Vec2Float ReadFrom(System.IO.BinaryReader reader)
        {
            if (reader == null)
                throw new System.ArgumentNullException(nameof(reader));

            var result = new Vec2Float();
            result.X = reader.ReadSingle();
            result.Y = reader.ReadSingle();

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
