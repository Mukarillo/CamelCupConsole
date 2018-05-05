using System;
namespace CamelCup.Utils
{
    public static class MathUtils
    {
        public static T Clamp<T>(T value, T min, T max) where T : IComparable
        {
            if (value.CompareTo(min) < 0)
                return min;
            if (value.CompareTo(max) > 0)
                return max;
            
            return value;
        }
    }
}
