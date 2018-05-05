using System;
using System.Threading;
using CamelCup.Boards;
using CamelCup.Utils;

namespace CamelCup.Actions
{
    public class RollDieAction : BaseAction
    {
        public override int rewardValue => 1;
        public override string description => "Roll dices to move a random camel.";

        public override void DoAction(Player owner)
        {
            base.DoAction(owner);

            CamelColors color;
            var n = GameManager.RollRandomDice(out color);

            ConsoleManager.Print($"{owner.name} got the {ColorUtils.colorsName[color]} dice from the pyramid...", 200);
            ConsoleManager.Print($"{owner.name} rolled {n} in {ColorUtils.colorsName[color]}.", 500);

            owner.diceRolled++;
            owner.diceCoinValue += rewardValue;

            GameManager.GetCamel(color).MovePiece(2);

            if(!TurnManager.isGameOver)
                Board.Dump();
        }

        public override void AskActionParameters() { }
    }
}
