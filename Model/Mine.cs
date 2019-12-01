namespace MLG360.Model
{
    public class Mine
    {
        public int PlayerId { get; set; }
        public Vec2Double Position { get; set; }
        public Vec2Double Size { get; set; }
        public MineState State { get; set; }
        public double? Timer { get; set; }
        public double TriggerRadius { get; set; }
        public ExplosionParameters ExplosionParameters { get; set; }

        public Mine(int playerId, Vec2Double position, Vec2Double size, MineState state, double? timer, double triggerRadius, ExplosionParameters explosionParameters)
        {
            PlayerId = playerId;
            Position = position;
            Size = size;
            State = state;
            Timer = timer;
            TriggerRadius = triggerRadius;
            ExplosionParameters = explosionParameters;
        }

        public static Mine ReadFrom(System.IO.BinaryReader reader)
        {
            if (reader == null)
                throw new System.ArgumentNullException(nameof(reader));

            var playerId = reader.ReadInt32();
            var position = Vec2Double.ReadFrom(reader);
            var size = Vec2Double.ReadFrom(reader);

            MineState state;
            switch (reader.ReadInt32())
            {
                case 0:
                    state = MineState.Preparing;
                    break;
                case 1:
                    state = MineState.Idle;
                    break;
                case 2:
                    state = MineState.Triggered;
                    break;
                case 3:
                    state = MineState.Exploded;
                    break;
                default:
                    throw new System.Exception("Unexpected discriminant value");
            }

            double? timer;
            if (reader.ReadBoolean())
            {
                timer = reader.ReadDouble();
            }
            else
            {
                timer = null;
            }

            var triggerRadius = reader.ReadDouble();
            var explosionParameters = ExplosionParameters.ReadFrom(reader);

            var result = new Mine(playerId, position, size, state, timer, triggerRadius, explosionParameters);

            return result;
        }
        public void WriteTo(System.IO.BinaryWriter writer)
        {
            if (writer == null)
                throw new System.ArgumentNullException(nameof(writer));

            writer.Write(PlayerId);
            Position.WriteTo(writer);
            Size.WriteTo(writer);
            writer.Write((int)State);

            if (!Timer.HasValue)
            {
                writer.Write(false);
            }
            else
            {
                writer.Write(true);
                writer.Write(Timer.Value);
            }

            writer.Write(TriggerRadius);
            ExplosionParameters.WriteTo(writer);
        }
    }
}
