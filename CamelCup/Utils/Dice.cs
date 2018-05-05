using System;
namespace CamelCup.Utils
{
    public class Dice
    {
        public CamelColors color;
        
        public Dice(CamelColors color)
        {
            this.color = color;
        }

        public int Roll()
        {
            var r = RandomUtils.InRange(0, 99);
            if (r % 3 == 0) return 3;
            if (r % 2 == 0) return 2;
            return 1;
        }
    }
}
