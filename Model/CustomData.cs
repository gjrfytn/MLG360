namespace MLG360.Model
{
    public abstract class CustomData
    {
        public abstract void WriteTo(System.IO.BinaryWriter writer);

        public static CustomData ReadFrom(System.IO.BinaryReader reader)
        {
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

        public class Log : CustomData
        {
            public const int TAG = 0;

            public string Text { get; set; }

            public Log() { }

            public Log(string text)
            {
                Text = text;
            }

            public static new Log ReadFrom(System.IO.BinaryReader reader)
            {
                var result = new Log();
                result.Text = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadInt32()));
                
                return result;
            }

            public override void WriteTo(System.IO.BinaryWriter writer)
            {
                writer.Write(TAG);
                var textData = System.Text.Encoding.UTF8.GetBytes(Text);
                writer.Write(textData.Length);
                writer.Write(textData);
            }
        }

        public class Rect : CustomData
        {
            public const int TAG = 1;

            public Vec2Float Pos { get; set; }
            public Vec2Float Size { get; set; }
            public ColorFloat Color { get; set; }

            public Rect() { }

            public Rect(Vec2Float pos, Vec2Float size, ColorFloat color)
            {
                Pos = pos;
                Size = size;
                Color = color;
            }

            public static new Rect ReadFrom(System.IO.BinaryReader reader)
            {
                var result = new Rect();
                result.Pos = Vec2Float.ReadFrom(reader);
                result.Size = Vec2Float.ReadFrom(reader);
                result.Color = ColorFloat.ReadFrom(reader);

                return result;
            }

            public override void WriteTo(System.IO.BinaryWriter writer)
            {
                writer.Write(TAG);
                Pos.WriteTo(writer);
                Size.WriteTo(writer);
                Color.WriteTo(writer);
            }
        }

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
                var result = new Line();
                result.P1 = Vec2Float.ReadFrom(reader);
                result.P2 = Vec2Float.ReadFrom(reader);
                result.Width = reader.ReadSingle();
                result.Color = ColorFloat.ReadFrom(reader);

                return result;
            }

            public override void WriteTo(System.IO.BinaryWriter writer)
            {
                writer.Write(TAG);
                P1.WriteTo(writer);
                P2.WriteTo(writer);
                writer.Write(Width);
                Color.WriteTo(writer);
            }
        }

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
                writer.Write(TAG);
                writer.Write(Vertices.Length);
                foreach (var verticesElement in Vertices)
                {
                    verticesElement.WriteTo(writer);
                }
            }
        }
    }
}
