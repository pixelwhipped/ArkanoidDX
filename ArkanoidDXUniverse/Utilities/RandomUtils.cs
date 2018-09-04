using System;
using System.Linq;

namespace ArkanoidDXUniverse.Utilities
{
    public static class RandomUtils
    {
        public static T RandomEnum<T>() => Enum
            .GetValues(typeof (T))
            .Cast<T>()
            .OrderBy(x => Arkanoid.Random.Next())
            .FirstOrDefault();

        public static bool ChanceIn(int chance)
        {
            var a = Arkanoid.Random.Next(chance);
            var b = Arkanoid.Random.Next(chance);
            return a == b;
        }
    }
}