using Engine;
using Enums;

namespace Pieces
{
    public class Bishop : Piece
    {
        public Bishop(ChessColor color, Piece[,] board) : base(color, ref board) {}

        public override bool[,] GetMoves(Position from)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "B";
        }
    }
}