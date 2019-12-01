namespace MLG360.Model
{
    public class Weapon
    {
        public WeaponType Typ { get; }
        public WeaponParameters Parameters { get; }
        public int Magazine { get; }
        public bool WasShooting { get; }
        public double Spread { get; }
        public double? FireTimer { get; }
        public double? LastAngle { get; }
        public int? LastFireTick { get; }

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

            WeaponType type;
            switch (reader.ReadInt32())
            {
                case 0:
                    type = WeaponType.Pistol;
                    break;
                case 1:
                    type = WeaponType.AssaultRifle;
                    break;
                case 2:
                    type = WeaponType.RocketLauncher;
                    break;
                default:
                    throw new System.Exception("Unexpected discriminant value");
            }

            var parameters = WeaponParameters.ReadFrom(reader);
            var magazine = reader.ReadInt32();
            var wasShooting = reader.ReadBoolean();
            var spread = reader.ReadDouble();

            double? fireTimer;
            if (reader.ReadBoolean())
            {
                fireTimer = reader.ReadDouble();
            }
            else
            {
                fireTimer = null;
            }

            double? lastAngle;
            if (reader.ReadBoolean())
            {
                lastAngle = reader.ReadDouble();
            }
            else
            {
                lastAngle = null;
            }

            int? lastFireTick;
            if (reader.ReadBoolean())
            {
                lastFireTick = reader.ReadInt32();
            }
            else
            {
                lastFireTick = null;
            }

            var result = new Weapon(type, parameters, magazine, wasShooting, spread, fireTimer, lastAngle, lastFireTick);

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
