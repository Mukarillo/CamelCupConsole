using System;
using System.Linq;
using System.Collections.Generic;
using CamelCup.Boards;
using CamelCup.Utils;

namespace CamelCup
{
    public static class CommandManager
    {
        private static List<ConsoleCustomCommand> customCommands = new List<ConsoleCustomCommand>
        {
            new ConsoleCustomCommand("!help", "Displays the available commands.", DumpCommands),
            new ConsoleCustomCommand("!back", "Reset the turn of the current player.", TurnManager.Back),
            new ConsoleCustomCommand("!board", "Displays the board.", Board.Dump),
            new ConsoleCustomCommand("!players", "Displays the players and hes/shes belongs.", GameManager.LogPlayers),
            new ConsoleCustomCommand("!legbets", "Displays the bets placed on the current leg.", GameManager.LogLegBets),
            new ConsoleCustomCommand("!overallbets", "Displays the bets placed on the overall game.", GameManager.LogOverallBets),
            new ConsoleCustomCommand("!clear", "Clears the screen. Usefull to hide stuff from other players.", Console.Clear),
            new ConsoleCustomCommand("!first", "Displays the first camel.", Board.LogFirstCamel),
            new ConsoleCustomCommand("!last", "Displays the last camel.", Board.LogLastCamel),
            new ConsoleCustomCommand("!checkcamels", "Displays a list of camels that didn't moved this leg.", GameManager.LogAvailableCamels)
        };

        public static void DumpCommands()
        {
            ConsoleManager.Print("Available commands", 100);
            string pretty = "";
            for (int i = 0; i < customCommands.Count; i++)
            {
                pretty = i == customCommands.Count - 1 ? "  ╚═ " : "  ╠═ ";
                ConsoleManager.Print(pretty + customCommands[i].ToString(), 200);
            }
        }

        public static bool GetYesNoAnswer()
        {
            bool loop = true;
            string answer = "";
            do
            {
                answer = Console.ReadLine().ToUpper();
                CheckCustomCommands(answer);
                if (answer == "Y" || answer == "N")
                    loop = false;
                else
                    ConsoleManager.Print("Invalid answer, please type Y for yes or N for no.");

            } while (loop);

            return answer == "Y";
        }

        public static string GetMultipleChoiseAnswer(List<string> options)
        {
            string optionsText = "";
            options.ForEach(x => optionsText += x + ", ");
            optionsText = optionsText.Remove(optionsText.Length - 2);

            string invalidAnswer = "Invalid answer, please type one of these options: " + optionsText;

            bool loop = true;
            string answer = "";
            do
            {
                answer = Console.ReadLine().ToUpper();
                CheckCustomCommands(answer);
                if (options.Exists(x => x.ToUpper().Equals(answer.ToUpper())))
                    loop = false;
                else
                    ConsoleManager.Print(invalidAnswer);

            } while (loop);

            return answer;
        }

        private static void CheckCustomCommands(string command)
        {
            var cmd = customCommands.Find(x => x.command.Equals(command.ToLower()));
            if(cmd != null)
            {
                ConsoleManager.JumpLine();
                cmd.action.Invoke();
                ConsoleManager.JumpLine();
            }
        }
    }
}
