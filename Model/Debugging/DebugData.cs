namespace MLG360.Model.Debugging
{
    public abstract class DebugData
    {
        public abstract void WriteTo(System.IO.BinaryWriter writer);

        public static DebugData ReadFrom(System.IO.BinaryReader reader)
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
                case PlacedText.TAG:
                    return PlacedText.ReadFrom(reader);
                default:
                    throw new System.Exception("Unexpected discriminant value");
            }
        }
    }
}
