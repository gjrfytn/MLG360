namespace MLG360.Model.CustomData
{
    public class Polygon : CustomData
    {
        public const int TAG = 3;

        public ColoredVertex[] Vertices { get; set; }

        public Polygon() { }

        public Polygon(ColoredVertex[] vertices)
        {
            Vertices = vertices;
        }

        public static new Polygon ReadFrom(System.IO.BinaryReader reader)
        {
            if (reader == null)
                throw new System.ArgumentNullException(nameof(reader));

            var result = new Polygon();
            result.Vertices = new ColoredVertex[reader.ReadInt32()];
            for (var i = 0; i < result.Vertices.Length; i++)
            {
                result.Vertices[i] = ColoredVertex.ReadFrom(reader);
            }

            return result;
        }

        public override void WriteTo(System.IO.BinaryWriter writer)
        {
            if (writer == null)
                throw new System.ArgumentNullException(nameof(writer));

            writer.Write(TAG);
            writer.Write(Vertices.Length);
            foreach (var verticesElement in Vertices)
            {
                verticesElement.WriteTo(writer);
            }
        }
    }
}
