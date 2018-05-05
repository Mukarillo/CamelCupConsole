using System;
using System.Collections.Generic;

namespace CamelCup.Actions
{
    public class BetLegAction : BaseAction
    {
        public override int rewardValue => throw new NotImplementedException();
        public override string description => "Place a bet on which camel will win this leg.";

        public override void AskActionParameters() 
        {
            ConsoleManager.Print("Which camel do you want to place a bet?");
            List<string> opts = new List<string>();
            var bestCards = GameManager.GetBestLegCardFromEachCamel();
            for (int i = 0; i < bestCards.Count; i++)
            {
                string pretty = i == bestCards.Count - 1 ? "╚═" : "╠═";
                ConsoleManager.Print(pretty + $"[{i}] {bestCards[i].ToString()}");
                opts.Add(i.ToString());
            }
            var card = bestCards[int.Parse(CommandManager.GetMultipleChoiseAnswer(opts))];

            GameManager.RemoveLegBetCard(card);
            owner.legBetCards.Add(card);

            ConsoleManager.Print($"Player {owner.name} made a bet on {card.camel.fullName}.");
        }
    }
}
