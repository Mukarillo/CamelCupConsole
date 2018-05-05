using System;
using System.Collections.Generic;

namespace CamelCup.Utils
{
    public static class ColorUtils
    {
        public static ConsoleColor GetConsoleColor(CamelColors color)
        {
            ConsoleColor toReturn = ConsoleColor.Black;
            switch(color)
            {
                case CamelColors.BLUE:
                    toReturn = ConsoleColor.Blue;
                    break;
                case CamelColors.GREEN:
                    toReturn = ConsoleColor.Green;
                    break;
                case CamelColors.ORANGE:
                    toReturn = ConsoleColor.Red;
                    break;
                case CamelColors.WHITE:
                    toReturn = ConsoleColor.Black;
                    break;
                case CamelColors.YELLOW:
                    toReturn = ConsoleColor.Yellow;
                    break;
            }

            return toReturn;
        }

        public static Dictionary<CamelColors, string> colorsName = new Dictionary<CamelColors, string>
        {
            {CamelColors.BLUE, "blue"},
            {CamelColors.GREEN, "green"},
            {CamelColors.ORANGE, "orange"},
            {CamelColors.WHITE, "white"},
            {CamelColors.YELLOW, "yellow"},
        };
    }
}
