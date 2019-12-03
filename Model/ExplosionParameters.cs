namespace MLG360.Model
{
    public class ExplosionParameters
    {
        public double Radius { get; }
        public int Damage { get; }

        public ExplosionParameters(double radius, int damage)
        {
            Radius = radius;
            Damage = damage;
        }

        public static ExplosionParameters ReadFrom(System.IO.BinaryReader reader)
        {
            if (reader == null)
                throw new System.ArgumentNullException(nameof(reader));

            return new ExplosionParameters(reader.ReadDouble(), reader.ReadInt32());
        }

        public void WriteTo(System.IO.BinaryWriter writer)
        {
            if (writer == null)
                throw new System.ArgumentNullException(nameof(writer));

            writer.Write(Radius);
            writer.Write(Damage);
        }
    }
}
