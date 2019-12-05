namespace MLG360.Model.Items
{
    public abstract class Item
    {
        public abstract void WriteTo(System.IO.BinaryWriter writer);

        public static Item ReadFrom(System.IO.BinaryReader reader)
        {
            if (reader == null)
                throw new System.ArgumentNullException(nameof(reader));

            switch (reader.ReadInt32())
            {
                case HealthPack.TAG:
                    return HealthPack.ReadFrom(reader);
                case Weapon.TAG:
                    return Weapon.ReadFrom(reader);
                case Mine.TAG:
                    return new Mine();
                default:
                    throw new System.Exception("Unexpected discriminant value");
            }
        }       
    }
}
