using System;
namespace CamelCup.Utils
{
    public static class RandomUtils
    {
        private static Random random = new Random();
        public static int InRange(int min, int max)
        {
            return random.Next(min, max);
        }
    }
}
