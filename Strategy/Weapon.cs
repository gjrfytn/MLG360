namespace MLG360.Strategy
{
    internal class Weapon
    {
        private readonly int _BulletDamage;
        private readonly float _FireTimer;
        private readonly float _FireRate;

        public float BulletSpeed { get; }
        public float BulletExplosionSize { get; }
        public int BulletExplosionDamage { get; }
        public float BulletSize { get; }
        public float Spread { get; }

        public bool NeedsReload => _FireTimer > _FireRate;
        public float DPS => (_BulletDamage + BulletExplosionDamage) / _FireRate;

        public Weapon(float bulletSpeed, int bulletDamage, float bulletExplosionSize, int bulletExplosionDamage, float bulletSize, float fireTimer, float fireRate, float spread)
        {
            BulletSpeed = bulletSpeed;
            _BulletDamage = bulletDamage;
            BulletExplosionSize = bulletExplosionSize;
            BulletExplosionDamage = bulletExplosionDamage;
            BulletSize = bulletSize;
            _FireTimer = fireTimer;
            _FireRate = fireRate;
            Spread = spread;
        }
    }
}
