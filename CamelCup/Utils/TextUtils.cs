using System;

namespace CamelCup.Utils
{
    public static class TextUtils
    {
        public static string GAME_TITLE = " ██████╗ █████╗ ███╗   ███╗███████╗██╗          ██████╗██╗   ██╗██████╗" + Environment.NewLine +
                                          "██╔════╝██╔══██╗████╗ ████║██╔════╝██║         ██╔════╝██║   ██║██╔══██╗" + Environment.NewLine +
                                          "██║     ███████║██╔████╔██║█████╗  ██║         ██║     ██║   ██║██████╔╝" + Environment.NewLine +
                                          "██║     ██╔══██║██║╚██╔╝██║██╔══╝  ██║         ██║     ██║   ██║██╔═══╝ " + Environment.NewLine +
                                          "╚██████╗██║  ██║██║ ╚═╝ ██║███████╗███████╗    ╚██████╗╚██████╔╝██║     " + Environment.NewLine +
                                          " ╚═════╝╚═╝  ╚═╝╚═╝     ╚═╝╚══════╝╚══════╝     ╚═════╝ ╚═════╝ ╚═╝     ";

        public static string SEPARATOR = Environment.NewLine + @"     .-.     .-.     .-.     .-.     .-.     .-.     .-." +
                                         Environment.NewLine + @"`._.'   `._.'   `._.'   `._.'   `._.'   `._.'   `._.'   `._.'" + 
                                                    Environment.NewLine + Environment.NewLine;
        
        public static string PlusMinusInt(int value)
        {
            if(value > 0)
            {
                return "+" + value;
            }
            return value.ToString();
        }

        public static string NoSignal(int value)
        {
            return Math.Abs(value).ToString();
        }

        public static string CoinOrCoins(int value)
        {
            return Math.Abs(value) > 1 ? "coins" : "coin";
        }

        public static string EarnedOrLost(int value)
        {
            return value < 0 ? "lost" : "earned";
        }

        public static string Colocation(int position)
        {
            if (position == 1)
                return "1st";
            if (position == 2)
                return "2nd";
            if (position == 1)
                return "3rd";
            
            return position+"th";
        }
    }
}
