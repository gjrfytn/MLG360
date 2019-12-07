using System.Numerics;

namespace MLG360.Strategy
{
    internal class Loot : IGameObject
    {
        public Vector2 Pos { get; }

        public Loot(Vector2 pos)
        {
            Pos = pos;
        }
    }
}