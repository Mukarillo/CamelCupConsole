using System;
using CamelCup.Utils;

namespace CamelCup.Boards.Pieces
{
    public class Trap : BoardPiece
    {
        public override string name 
        {
            get
            {
                return $"Trap {TextUtils.PlusMinusInt(positionModifier)} ({owner.name})";
            }
        }
        public override string fullName => name;

        public int positionModifier;
        public Player owner;
        public int actionCoinValue;

        public Trap(int positionModifier, Player owner, int actionCoinValue)
        {
            this.positionModifier = positionModifier;
            this.owner = owner;
            this.actionCoinValue = actionCoinValue;
        }
    }
}