namespace MLG360.Strategy
{
    internal class Weapon
    {
        private readonly float _FireTimer;
        private readonly float _FireRate;

        public float BulletSpeed { get; }
        public float BulletExplosionRadius { get; }
        public float BulletSize { get; }

        public bool NeedsReload => _FireTimer > _FireRate;

        public Weapon(float bulletSpeed, float bulletExplosionRadius, float bulletSize, float fireTimer, float fireRate)
        {
            BulletSpeed = bulletSpeed;
            BulletExplosionRadius = bulletExplosionRadius;
            BulletSize = bulletSize;
            _FireTimer = fireTimer;
            _FireRate = fireRate;
        }
    }
}
