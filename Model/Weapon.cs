namespace MLG360.Model
{
    public class Weapon
    {
        public WeaponType Typ { get; set; }
        public WeaponParameters Parameters { get; set; }
        public int Magazine { get; set; }
        public bool WasShooting { get; set; }
        public double Spread { get; set; }
        public double? FireTimer { get; set; }
        public double? LastAngle { get; set; }
        public int? LastFireTick { get; set; }

        private Weapon()
        {
        }

        public Weapon(WeaponType typ, WeaponParameters parameters, int magazine, bool wasShooting, double spread, double? fireTimer, double? lastAngle, int? lastFireTick)
        {
            Typ = typ;
            Parameters = parameters;
            Magazine = magazine;
            WasShooting = wasShooting;
            Spread = spread;
            FireTimer = fireTimer;
            LastAngle = lastAngle;
            LastFireTick = lastFireTick;
        }

        public static Weapon ReadFrom(System.IO.BinaryReader reader)
        {
            if (reader == null)
                throw new System.ArgumentNullException(nameof(reader));

            var result = new Weapon();
            switch (reader.ReadInt32())
            {
                case 0:
                    result.Typ = WeaponType.Pistol;
                    break;
                case 1:
                    result.Typ = WeaponType.AssaultRifle;
                    break;
                case 2:
                    result.Typ = WeaponType.RocketLauncher;
                    break;
                default:
                    throw new System.Exception("Unexpected discriminant value");
            }

            result.Parameters = WeaponParameters.ReadFrom(reader);
            result.Magazine = reader.ReadInt32();
            result.WasShooting = reader.ReadBoolean();
            result.Spread = reader.ReadDouble();

            if (reader.ReadBoolean())
            {
                result.FireTimer = reader.ReadDouble();
            }
            else
            {
                result.FireTimer = null;
            }

            if (reader.ReadBoolean())
            {
                result.LastAngle = reader.ReadDouble();
            }
            else
            {
                result.LastAngle = null;
            }

            if (reader.ReadBoolean())
            {
                result.LastFireTick = reader.ReadInt32();
            }
            else
            {
                result.LastFireTick = null;
            }

            return result;
        }

        public void WriteTo(System.IO.BinaryWriter writer)
        {
            if (writer == null)
                throw new System.ArgumentNullException(nameof(writer));

            writer.Write((int)Typ);
            Parameters.WriteTo(writer);
            writer.Write(Magazine);
            writer.Write(WasShooting);
            writer.Write(Spread);

            if (!FireTimer.HasValue)
            {
                writer.Write(false);
            }
            else
            {
                writer.Write(true);
                writer.Write(FireTimer.Value);
            }

            if (!LastAngle.HasValue)
            {
                writer.Write(false);
            }
            else
            {
                writer.Write(true);
                writer.Write(LastAngle.Value);
            }

            if (!LastFireTick.HasValue)
            {
                writer.Write(false);
            }
            else
            {
                writer.Write(true);
                writer.Write(LastFireTick.Value);
            }
        }
    }
}
