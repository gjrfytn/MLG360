namespace MLG360.Model
{
    public struct BulletParameters
    {
        public double Speed { get; set; }
        public double Size { get; set; }
        public int Damage { get; set; }

        public BulletParameters(double speed, double size, int damage)
        {
            Speed = speed;
            Size = size;
            Damage = damage;
        }

        public static BulletParameters ReadFrom(System.IO.BinaryReader reader)
        {
            if (reader == null)
                throw new System.ArgumentNullException(nameof(reader));

            var result = new BulletParameters();
            result.Speed = reader.ReadDouble();
            result.Size = reader.ReadDouble();
            result.Damage = reader.ReadInt32();

            return result;
        }

        public void WriteTo(System.IO.BinaryWriter writer)
        {
            if (writer == null)
                throw new System.ArgumentNullException(nameof(writer));

            writer.Write(Speed);
            writer.Write(Size);
            writer.Write(Damage);
        }
    }
}
