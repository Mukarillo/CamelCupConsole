using System;
using System.Collections.Generic;
using CamelCup.Utils;

namespace CamelCup.Boards.Pieces
{
    public class Camel : BoardPiece
    {
        public override string name => $"{ColorUtils.colorsName[color][0]}";
        public override string fullName => $"{ColorUtils.colorsName[color]}";

        public CamelColors color;

        public Camel(CamelColors color)
        {
            this.color = color;
        }

        public void MovePiece(int spaces)
        {
            position = Board.MovePiece(this, spaces);
        }
    }
}
