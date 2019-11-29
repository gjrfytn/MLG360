namespace MLG360.Model.CustomData
{
    public abstract class CustomData
    {
        public abstract void WriteTo(System.IO.BinaryWriter writer);

        public static CustomData ReadFrom(System.IO.BinaryReader reader)
        {
            if (reader == null)
                throw new System.ArgumentNullException(nameof(reader));

            switch (reader.ReadInt32())
            {
                case Log.TAG:
                    return Log.ReadFrom(reader);
                case Rect.TAG:
                    return Rect.ReadFrom(reader);
                case Line.TAG:
                    return Line.ReadFrom(reader);
                case Polygon.TAG:
                    return Polygon.ReadFrom(reader);
                default:
                    throw new System.Exception("Unexpected discriminant value");
            }
        }
    }
}
