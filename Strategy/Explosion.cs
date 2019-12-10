using System.Numerics;

namespace MLG360.Strategy
{
    internal class Explosion : Rectangle
    {
        public int Damage { get; }

        protected override Vector2 Center { get; }
        protected override float Width { get; }
        protected override float Height { get; }

        public Explosion(Vector2 pos, float size, int damage)
        {
            Center = pos;
            Width = size;
            Height = size;
            Damage = damage;

#if DEBUG
            {
                Debug.Instance?.Draw(
                    new Model.Debugging.Rect(new Model.Vec2Float(Center.X - Width / 2, Center.Y - Height / 2),
                    new Model.Vec2Float(Width, Height),
                    new Model.ColorFloat(1, 0, 0, 0.2f)));
            }
#endif
        }
    }
}
