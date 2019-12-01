namespace MLG360.Model.Items
{
    public class HealthPack : Item
    {
        public const int TAG = 0;

        public int Health { get; set; }

        public HealthPack(int health)
        {
            Health = health;
        }

        public static new HealthPack ReadFrom(System.IO.BinaryReader reader)
        {
            if (reader == null)
                throw new System.ArgumentNullException(nameof(reader));

            var result = new HealthPack(reader.ReadInt32());

            return result;
        }

        public override void WriteTo(System.IO.BinaryWriter writer)
        {
            if (writer == null)
                throw new System.ArgumentNullException(nameof(writer));

            writer.Write(TAG);
            writer.Write(Health);
        }
    }
}
