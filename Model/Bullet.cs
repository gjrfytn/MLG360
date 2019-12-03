namespace MLG360.Model
{
    public class Bullet
    {
        public WeaponType WeaponType { get; }
        public int UnitId { get; }
        public int PlayerId { get; }
        public Vec2Double Position { get; }
        public Vec2Double Velocity { get; }
        public int Damage { get; }
        public double Size { get; }
        public ExplosionParameters ExplosionParameters { get; }

        public Bullet(WeaponType weaponType, int unitId, int playerId, Vec2Double position, Vec2Double velocity, int damage, double size, ExplosionParameters explosionParameters)
        {
            WeaponType = weaponType;
            UnitId = unitId;
            PlayerId = playerId;
            Position = position ?? throw new System.ArgumentNullException(nameof(position));
            Velocity = velocity ?? throw new System.ArgumentNullException(nameof(velocity));
            Damage = damage;
            Size = size;
            ExplosionParameters = explosionParameters;
        }

        public static Bullet ReadFrom(System.IO.BinaryReader reader)
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

            var unitId = reader.ReadInt32();
            var playerId = reader.ReadInt32();
            var position = Vec2Double.ReadFrom(reader);
            var velocity = Vec2Double.ReadFrom(reader);
            var damage = reader.ReadInt32();
            var size = reader.ReadDouble();
            var explosionParameters = reader.ReadBoolean() ? ExplosionParameters.ReadFrom(reader) : null;

            return new Bullet(weaponType, unitId, playerId, position, velocity, damage, size, explosionParameters);
        }

        public void WriteTo(System.IO.BinaryWriter writer)
        {
            if (writer == null)
                throw new System.ArgumentNullException(nameof(writer));

            writer.Write((int)WeaponType);
            writer.Write(UnitId);
            writer.Write(PlayerId);
            Position.WriteTo(writer);
            Velocity.WriteTo(writer);
            writer.Write(Damage);
            writer.Write(Size);

            if (ExplosionParameters == null)
                writer.Write(false);
            else
            {
                writer.Write(true);
                ExplosionParameters.WriteTo(writer);
            }
        }
    }
}
