using System;
using System.Collections.Generic;
using CamelCup.Boards.Pieces;

namespace CamelCup.Utils
{
    public class Bet
    {
        public virtual string betPrefix => "Overall";
        public Camel camel;
        public Player owner;

        public Bet(Camel camel, Player owner)
        {
            this.owner = owner;
            this.camel = camel;
        }

		public override string ToString()
		{
            return $"{betPrefix} Bet Card -> Camel: {camel.fullName}";
		}
	}
}
