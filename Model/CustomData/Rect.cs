﻿namespace MLG360.Model.CustomData
{
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
            if (writer == null)
                throw new System.ArgumentNullException(nameof(writer));

            writer.Write(TAG);
            Pos.WriteTo(writer);
            Size.WriteTo(writer);
            Color.WriteTo(writer);
        }
    }
}