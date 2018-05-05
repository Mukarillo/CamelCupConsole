using System;
using System.Collections.Generic;
using CamelCup.Boards;
using CamelCup.Boards.Pieces;
using CamelCup.Utils;

namespace CamelCup.Actions
{
    public class PlaceTrapAction : BaseAction
    {
        public override int rewardValue => 1;
        public override string description => "Place a trap on the board that will move forward one case or backward one case when a camel steps on that tile.";

        public override void AskActionParameters() 
        {
            ConsoleManager.Print("What kind of trap do you want to place: +1 or -1?");
            int positionModifier = int.Parse(CommandManager.GetMultipleChoiseAnswer(new List<string> { "+1", "-1" }));

            if (owner.trap != null)
                Board.RemovePiece(owner.trap);

            List<string> opts = new List<string>();
            var intopts = Board.GetAvailableTrapPositions();
            for (int i = 0; i < intopts.Count; i++)
            {
                opts.Add(intopts[i].ToString());
            }

            ConsoleManager.Print("Where in the board do you want to place the trap? (type !board to see the board)");
            int position = int.Parse(CommandManager.GetMultipleChoiseAnswer(opts));

            if (owner.trap == null)
                owner.trap = new Trap(positionModifier, owner, rewardValue);
            else
                owner.trap.positionModifier = positionModifier;

            owner.trap.ForcePiecePosition(position);

            ConsoleManager.Print($"Placing the {TextUtils.PlusMinusInt(positionModifier)} trap in tile number {position}");
        }
    }
}
