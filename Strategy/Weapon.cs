namespace MLG360.Strategy
{
    internal class Weapon
    {
        public float BulletSpeed { get; }
        public float BulletExplosionRadius { get; }
        public float BulletSize { get; }

        public Weapon(float bulletSpeed, float bulletExplosionRadius, float bulletSize)
        {
            BulletSpeed = bulletSpeed;
            BulletExplosionRadius = bulletExplosionRadius;
            BulletSize = bulletSize;
        }
    }
}
