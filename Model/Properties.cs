namespace MLG360.Model
{
    public class Properties
    {
        public int MaxTickCount { get; set; }
        public int TeamSize { get; set; }
        public double TicksPerSecond { get; set; }
        public int UpdatesPerTick { get; set; }
        public Vec2Double LootBoxSize { get; set; }
        public Vec2Double UnitSize { get; set; }
        public double UnitMaxHorizontalSpeed { get; set; }
        public double UnitFallSpeed { get; set; }
        public double UnitJumpTime { get; set; }
        public double UnitJumpSpeed { get; set; }
        public double JumpPadJumpTime { get; set; }
        public double JumpPadJumpSpeed { get; set; }
        public int UnitMaxHealth { get; set; }
        public int HealthPackHealth { get; set; }
        public System.Collections.Generic.IDictionary<WeaponType, WeaponParameters> WeaponParameters { get; set; }
        public Vec2Double MineSize { get; set; }
        public ExplosionParameters MineExplosionParameters { get; set; }
        public double MinePrepareTime { get; set; }
        public double MineTriggerTime { get; set; }
        public double MineTriggerRadius { get; set; }
        public int KillScore { get; set; }

        public Properties(int maxTickCount, int teamSize, double ticksPerSecond, int updatesPerTick, Vec2Double lootBoxSize, Vec2Double unitSize, double unitMaxHorizontalSpeed, double unitFallSpeed, double unitJumpTime, double unitJumpSpeed, double jumpPadJumpTime, double jumpPadJumpSpeed, int unitMaxHealth, int healthPackHealth, System.Collections.Generic.IDictionary<WeaponType, WeaponParameters> weaponParameters, Vec2Double mineSize, ExplosionParameters mineExplosionParameters, double minePrepareTime, double mineTriggerTime, double mineTriggerRadius, int killScore)
        {
            MaxTickCount = maxTickCount;
            TeamSize = teamSize;
            TicksPerSecond = ticksPerSecond;
            UpdatesPerTick = updatesPerTick;
            LootBoxSize = lootBoxSize;
            UnitSize = unitSize;
            UnitMaxHorizontalSpeed = unitMaxHorizontalSpeed;
            UnitFallSpeed = unitFallSpeed;
            UnitJumpTime = unitJumpTime;
            UnitJumpSpeed = unitJumpSpeed;
            JumpPadJumpTime = jumpPadJumpTime;
            JumpPadJumpSpeed = jumpPadJumpSpeed;
            UnitMaxHealth = unitMaxHealth;
            HealthPackHealth = healthPackHealth;
            WeaponParameters = weaponParameters;
            MineSize = mineSize;
            MineExplosionParameters = mineExplosionParameters;
            MinePrepareTime = minePrepareTime;
            MineTriggerTime = mineTriggerTime;
            MineTriggerRadius = mineTriggerRadius;
            KillScore = killScore;
        }

        public static Properties ReadFrom(System.IO.BinaryReader reader)
        {
            if (reader == null)
                throw new System.ArgumentNullException(nameof(reader));

            var maxTickCount = reader.ReadInt32();
            var teamSize = reader.ReadInt32();
            var ticksPerSecond = reader.ReadDouble();
            var updatesPerTick = reader.ReadInt32();
            var lootBoxSize = Vec2Double.ReadFrom(reader);
            var unitSize = Vec2Double.ReadFrom(reader);
            var unitMaxHorizontalSpeed = reader.ReadDouble();
            var unitFallSpeed = reader.ReadDouble();
            var unitJumpTime = reader.ReadDouble();
            var unitJumpSpeed = reader.ReadDouble();
            var jumpPadJumpTime = reader.ReadDouble();
            var jumpPadJumpSpeed = reader.ReadDouble();
            var unitMaxHealth = reader.ReadInt32();
            var healthPackHealth = reader.ReadInt32();
            var weaponParametersSize = reader.ReadInt32();
            var weaponParameters = new System.Collections.Generic.Dictionary<WeaponType, WeaponParameters>(weaponParametersSize);

            for (var i = 0; i < weaponParametersSize; i++)
            {
                WeaponType weaponParametersKey;
                switch (reader.ReadInt32())
                {
                    case 0:
                        weaponParametersKey = WeaponType.Pistol;
                        break;
                    case 1:
                        weaponParametersKey = WeaponType.AssaultRifle;
                        break;
                    case 2:
                        weaponParametersKey = WeaponType.RocketLauncher;
                        break;
                    default:
                        throw new System.Exception("Unexpected discriminant value");
                }

                WeaponParameters weaponParametersValue;
                weaponParametersValue = Model.WeaponParameters.ReadFrom(reader);
                weaponParameters.Add(weaponParametersKey, weaponParametersValue);
            }

            var mineSize = Vec2Double.ReadFrom(reader);
            var mineExplosionParameters = ExplosionParameters.ReadFrom(reader);
            var minePrepareTime = reader.ReadDouble();
            var mineTriggerTime = reader.ReadDouble();
            var mineTriggerRadius = reader.ReadDouble();
            var killScore = reader.ReadInt32();

            var result = new Properties(
                maxTickCount,
                teamSize,
                ticksPerSecond,
                updatesPerTick,
                lootBoxSize,
                unitSize,
                unitMaxHorizontalSpeed,
                unitFallSpeed,
                unitJumpTime,
                unitJumpSpeed,
                jumpPadJumpTime,
                jumpPadJumpSpeed,
                unitMaxHealth,
                healthPackHealth,
                weaponParameters,
                mineSize,
                mineExplosionParameters,
                minePrepareTime,
                mineTriggerTime,
                mineTriggerRadius,
                killScore);

            return result;
        }
        public void WriteTo(System.IO.BinaryWriter writer)
        {
            if (writer == null)
                throw new System.ArgumentNullException(nameof(writer));

            writer.Write(MaxTickCount);
            writer.Write(TeamSize);
            writer.Write(TicksPerSecond);
            writer.Write(UpdatesPerTick);
            LootBoxSize.WriteTo(writer);
            UnitSize.WriteTo(writer);
            writer.Write(UnitMaxHorizontalSpeed);
            writer.Write(UnitFallSpeed);
            writer.Write(UnitJumpTime);
            writer.Write(UnitJumpSpeed);
            writer.Write(JumpPadJumpTime);
            writer.Write(JumpPadJumpSpeed);
            writer.Write(UnitMaxHealth);
            writer.Write(HealthPackHealth);
            writer.Write(WeaponParameters.Count);

            foreach (var weaponParametersEntry in WeaponParameters)
            {
                var weaponParametersKey = weaponParametersEntry.Key;
                var weaponParametersValue = weaponParametersEntry.Value;
                writer.Write((int)weaponParametersKey);
                weaponParametersValue.WriteTo(writer);
            }

            MineSize.WriteTo(writer);
            MineExplosionParameters.WriteTo(writer);
            writer.Write(MinePrepareTime);
            writer.Write(MineTriggerTime);
            writer.Write(MineTriggerRadius);
            writer.Write(KillScore);
        }
    }
}
