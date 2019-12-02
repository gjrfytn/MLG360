using System.Collections.Generic;
using System.Linq;

namespace MLG360.Model.CustomData
{
    public class Polygon : CustomData
    {
        public const int TAG = 3;

        private readonly ColoredVertex[] _Vertices;

        public IEnumerable<ColoredVertex> Vertices => _Vertices;

        public Polygon(IEnumerable<ColoredVertex> vertices)
        {
            _Vertices = vertices.ToArray();
        }

        public static new Polygon ReadFrom(System.IO.BinaryReader reader)
        {
            if (reader == null)
                throw new System.ArgumentNullException(nameof(reader));

            var vertices = new ColoredVertex[reader.ReadInt32()];
            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i] = ColoredVertex.ReadFrom(reader);
            }

            var result = new Polygon(vertices);

            return result;
        }

        public override void WriteTo(System.IO.BinaryWriter writer)
        {
            if (writer == null)
                throw new System.ArgumentNullException(nameof(writer));

            writer.Write(TAG);

            writer.Write(_Vertices.Length);
            foreach (var verticesElement in _Vertices)
            {
                verticesElement.WriteTo(writer);
            }
        }
    }
}
