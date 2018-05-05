using System;
using System.Collections.Generic;
using CamelCup.Utils;

namespace CamelCup.Actions
{
    public class BetOverallAction : BaseAction
    {
        public override int rewardValue => throw new NotImplementedException();
        public override string description => "Place a bet on which camel will win or loose the game.";

        public override void AskActionParameters() 
        { 
            ConsoleManager.Print("What kind of bet you want to place", 200);
            ConsoleManager.Print("  ╠═ [0] Overall winner", 200);
            ConsoleManager.Print("  ╚═ [1] Overall looser", 200);
            string type = CommandManager.GetMultipleChoiseAnswer(new List<string> { "0", "1" });
            bool isWinnerBet = type == "0";

            ConsoleManager.Print("Which camel do you want to place a bet?");
            List<string> opts = new List<string>();
            var betCards = owner.overallBetCards;
            for (int i = 0; i < betCards.Count; i++)
            {
                string pretty = i == betCards.Count - 1 ? "  ╚═" : "  ╠═";
                ConsoleManager.Print(pretty + $"[{i}] {betCards[i].camel.fullName}", 200);
                opts.Add(i.ToString());
            }

            var bet = betCards[int.Parse(CommandManager.GetMultipleChoiseAnswer(opts))];
            owner.overallBetCards.Remove(bet);
            GameManager.QueueOverallBet(bet, isWinnerBet);

            ConsoleManager.Print($"Player {owner.name} made a bet on {bet.camel.fullName}. (type !clear to erase history)", 200);
        }
    }
}
