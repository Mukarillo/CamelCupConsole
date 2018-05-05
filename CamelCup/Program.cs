using System;
using CamelCup.Utils;

namespace CamelCup
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;

            Console.Write(TextUtils.GAME_TITLE);
            Console.WriteLine("");
            Console.WriteLine(Environment.NewLine + "Original game: https://boardgamegeek.com/boardgame/153938/camel");
            Console.WriteLine("Board game designed by: Steffen Bogen");
            Console.WriteLine("Board game ilustrated by: Dennis Lohausen");
            Console.WriteLine(Environment.NewLine + "Cmd game created by: Murillo Pugliesi");
            Console.WriteLine("Github: https://github.com/Mukarillo");

            Console.Write(TextUtils.SEPARATOR);

            GameManager.RegisterPlayers();

            Console.Write(TextUtils.SEPARATOR);

            TurnManager.StartGame();
        }
    }
}
