namespace MLG360.Model
{
    public class ExplosionParameters
    {
        public double Radius { get; set; }
        public int Damage { get; set; }

        public ExplosionParameters(double radius, int damage)
        {
            Radius = radius;
            Damage = damage;
        }

        public static ExplosionParameters ReadFrom(System.IO.BinaryReader reader)
        {
            if (reader == null)
                throw new System.ArgumentNullException(nameof(reader));

            var result = new ExplosionParameters(reader.ReadDouble(), reader.ReadInt32());

            return result;
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
