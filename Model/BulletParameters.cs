namespace MLG360.Model
{
    public class BulletParameters
    {
        public double Speed { get; }
        public double Size { get; }
        public int Damage { get; }

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

            return new BulletParameters(reader.ReadDouble(), reader.ReadDouble(), reader.ReadInt32());
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
