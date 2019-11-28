namespace MLG360.Model
{
    public struct ExplosionParameters
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
            var result = new ExplosionParameters();
            result.Radius = reader.ReadDouble();
            result.Damage = reader.ReadInt32();

            return result;
        }

        public void WriteTo(System.IO.BinaryWriter writer)
        {
            writer.Write(Radius);
            writer.Write(Damage);
        }
    }
}
