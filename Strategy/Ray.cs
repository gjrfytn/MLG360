using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace MLG360.Strategy
{
    internal class Ray
    {
        private readonly Vector2 _Origin;
        private readonly Vector2 _Direction;
        private readonly float _Length;

        public Ray(Vector2 origin, Vector2 direction, float length)
        {
            _Origin = origin;
            _Direction = Vector2.Normalize(direction);
            _Length = length;
        }

        public bool Intersects(IEnumerable<Rectangle> rectangles) => Intersects(rectangles, Vector2.Zero);

        public bool Intersects(IEnumerable<Rectangle> rectangles, Vector2 slideBoxSize)
        {
            var rectanglesArr = rectangles.ToArray();

            const float checkStep = 0.25f;
            for (var checkDist = checkStep; checkDist <= _Length; checkDist += checkStep)
            {
                var checkPoint = _Origin + checkDist * _Direction;

                foreach (var boxCheckPoint in GetCheckPoints(checkPoint, slideBoxSize))
                    if (rectanglesArr.Any(t => t.Contains(boxCheckPoint)))
                        return true;
            }

            return false;
        }

        private static IEnumerable<Vector2> GetCheckPoints(Vector2 originCheckPoint, Vector2 boxSize)
        {
            if (boxSize == Vector2.Zero)
                return new[] { originCheckPoint };

            var halfSizeX = boxSize.X / 2;
            var halfSizeY = boxSize.Y / 2;

            return new[]
            {
                originCheckPoint + new Vector2(-halfSizeX, halfSizeY),
                originCheckPoint + new Vector2(halfSizeX, halfSizeY),
                originCheckPoint + new Vector2(halfSizeX, -halfSizeY),
                originCheckPoint - new Vector2(halfSizeX, halfSizeY),
            };
        }
    }
}
