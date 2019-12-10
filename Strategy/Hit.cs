using System.Numerics;

namespace MLG360.Strategy
{
    internal class Hit
    {
        public Vector2 Pos { get; }
        public float DTime { get; }

        public Hit(Vector2 pos, float dtime)
        {
            Pos = pos;
            DTime = dtime;
        }
    }
}
