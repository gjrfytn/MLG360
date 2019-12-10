using System.Collections.Generic;
using System.Numerics;

namespace MLG360.Strategy
{
    internal class Bullet : Rectangle
    {
        private readonly float _Size;

        private Vector2 _Pos;

        public Vector2 Velocity { get; }
        public int Damage { get; }
        public float ExplosionSize { get; }
        public int ExplosionDamage { get; }

        protected override Vector2 Center => _Pos;
        protected override float Width => _Size;
        protected override float Height => _Size;

        private Ray Path => new Ray(_Pos, Velocity, float.MaxValue);

        public Bullet(Vector2 pos, float size, Vector2 velocity, int damage, float explosionSize, int explosionDamage)
        {
            _Pos = pos;
            _Size = size;
            Velocity = velocity;
            Damage = damage;
            ExplosionSize = explosionSize;
            ExplosionDamage = explosionDamage;
        }

        public Hit FindHit(IEnumerable<Rectangle> rectangles)
        {
            var hitPoint = Path.FindIntersectionPoint(rectangles, new Vector2(_Size, _Size));

            return hitPoint.HasValue ? new Hit(hitPoint.Value, Vector2.Distance(_Pos, hitPoint.Value) / Velocity.Length()) : null;
        }

        // public void Move(float dtime) => _Pos += Velocity * dtime;
        // public Explosion Explode()
    }
}