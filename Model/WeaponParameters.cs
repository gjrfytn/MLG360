namespace MLG360.Model
{
    public class WeaponParameters
    {
        public int MagazineSize { get; set; }
        public double FireRate { get; set; }
        public double ReloadTime { get; set; }
        public double MinSpread { get; set; }
        public double MaxSpread { get; set; }
        public double Recoil { get; set; }
        public double AimSpeed { get; set; }
        public BulletParameters Bullet { get; set; }
        public ExplosionParameters Explosion { get; set; }

        public WeaponParameters(int magazineSize, double fireRate, double reloadTime, double minSpread, double maxSpread, double recoil, double aimSpeed, BulletParameters bullet, ExplosionParameters explosion)
        {
            MagazineSize = magazineSize;
            FireRate = fireRate;
            ReloadTime = reloadTime;
            MinSpread = minSpread;
            MaxSpread = maxSpread;
            Recoil = recoil;
            AimSpeed = aimSpeed;
            Bullet = bullet ?? throw new System.ArgumentNullException(nameof(bullet));
            Explosion = explosion;
        }

        public static WeaponParameters ReadFrom(System.IO.BinaryReader reader)
        {
            if (reader == null)
                throw new System.ArgumentNullException(nameof(reader));

            var magazineSize = reader.ReadInt32();
            var fireRate = reader.ReadDouble();
            var reloadTime = reader.ReadDouble();
            var minSpread = reader.ReadDouble();
            var maxSpread = reader.ReadDouble();
            var recoil = reader.ReadDouble();
            var aimSpeed = reader.ReadDouble();
            var bullet = BulletParameters.ReadFrom(reader);

            ExplosionParameters explosionParameters;
            if (reader.ReadBoolean())
            {
                explosionParameters = ExplosionParameters.ReadFrom(reader);
            }
            else
            {
                explosionParameters = null;
            }

            var result = new WeaponParameters(magazineSize, fireRate, reloadTime, minSpread, maxSpread, recoil, aimSpeed, bullet, explosionParameters);

            return result;
        }

        public void WriteTo(System.IO.BinaryWriter writer)
        {
            if (writer == null)
                throw new System.ArgumentNullException(nameof(writer));

            writer.Write(MagazineSize);
            writer.Write(FireRate);
            writer.Write(ReloadTime);
            writer.Write(MinSpread);
            writer.Write(MaxSpread);
            writer.Write(Recoil);
            writer.Write(AimSpeed);
            Bullet.WriteTo(writer);

            if (Explosion == null)
            {
                writer.Write(false);
            }
            else
            {
                writer.Write(true);
                Explosion.WriteTo(writer);
            }
        }
    }
}
