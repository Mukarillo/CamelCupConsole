using System;
using System.Collections.Generic;
using CamelCup.Boards.Pieces;

namespace CamelCup.Utils
{
    public class LegBet : Bet
    {
        public override string betPrefix => "Leg";
		public List<int> rewards = new List<int>();

        public LegBet(Camel camel, List<int> rewards, Player owner = null) : base(camel, owner)
        {
            this.rewards = rewards;
        }

		public override string ToString()
		{
            return base.ToString() + $", 1st -> {TextUtils.PlusMinusInt(rewards[0])} coins, 2nd -> {TextUtils.PlusMinusInt(rewards[1])} coin, < 3rd -> {TextUtils.PlusMinusInt(rewards[2])}";
		}
	}
}
