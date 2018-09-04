using System;
using Microsoft.Xna.Framework;

namespace ArkanoidDXUniverse.Utilities
{
    public static class VectorHelpers
    {
        public static Vector2 Truncate(Vector2 vec, float maxValue)
            => vec.Length() > maxValue ? Vector2.Multiply(Vector2.Normalize(vec), maxValue) : vec;

        public static float Hypot(Vector2 a, Vector2 b)
            =>
                (float)
                    Math.Sqrt(Math.Pow(Math.Max(a.X, b.X) - Math.Min(a.X, b.X), 2) +
                              Math.Pow(Math.Max(a.Y, b.Y) - Math.Min(a.Y, b.Y), 2));

        public static float Length(Vector2 v1) => (float) Math.Sqrt(v1.X*v1.X + v1.Y*v1.Y);
    }
}