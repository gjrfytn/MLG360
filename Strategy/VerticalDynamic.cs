namespace MLG360.Strategy
{
    internal class VerticalDynamic
    {
        private readonly Type _Type;
        private readonly float _FallSpeed;
        private readonly float _ThrowSpeed;
        private readonly float _ThrowTimeRemain;

        public enum Type
        {
            None,
            Falling,
            ThrownUp
        }

        public VerticalDynamic(Type type, float fallSpeed, float throwSpeed, float throwTimeRemain)
        {
            _Type = type;
            _FallSpeed = fallSpeed;
            _ThrowSpeed = throwSpeed;
            _ThrowTimeRemain = throwTimeRemain;
        }

        public float CalculateDPos(float dtime)
        {
            switch (_Type)
            {
                case Type.None: return 0;
                case Type.Falling: return CalculateFallDPos(dtime);
                case Type.ThrownUp: return _ThrowSpeed * System.Math.Min(_ThrowTimeRemain, dtime) + CalculateFallDPos(System.Math.Max(dtime - _ThrowTimeRemain, 0));
                default: throw new System.ArgumentOutOfRangeException(nameof(_Type));
            }
        }

        private float CalculateFallDPos(float dtime) => -_FallSpeed * dtime;
    }
}
