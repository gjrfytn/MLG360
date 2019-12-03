namespace MLG360.Model.Items
{
    public class Weapon : Item
    {
        public const int TAG = 1;

        public WeaponType WeaponType { get; }

        public Weapon(WeaponType weaponType)
        {
            WeaponType = weaponType;
        }

        public static new Weapon ReadFrom(System.IO.BinaryReader reader)
        {
            if (reader == null)
                throw new System.ArgumentNullException(nameof(reader));

            WeaponType weaponType;
            switch (reader.ReadInt32())
            {
                case 0:
                    weaponType = WeaponType.Pistol;
                    break;
                case 1:
                    weaponType = WeaponType.AssaultRifle;
                    break;
                case 2:
                    weaponType = WeaponType.RocketLauncher;
                    break;
                default:
                    throw new System.Exception("Unexpected discriminant value");
            }

            return new Weapon(weaponType);
        }

        public override void WriteTo(System.IO.BinaryWriter writer)
        {
            if (writer == null)
                throw new System.ArgumentNullException(nameof(writer));

            writer.Write(TAG);
            writer.Write((int)WeaponType);
        }
    }
}
