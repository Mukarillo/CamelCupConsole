using System;
using CamelCup.Utils;

namespace CamelCup.Boards.Pieces
{
    public abstract class BoardPiece
    {
        public int position = -1;

        public abstract string name { get; }
        public abstract string fullName { get; }

        public void ForcePiecePosition(int position)
        {
            this.position = Board.PlacePiece(this, position);
        }

        protected int ClampedPosition(int position)
        {
            return MathUtils.Clamp(position, 0, Board.MAX_SPACES);
        }
    }
}
