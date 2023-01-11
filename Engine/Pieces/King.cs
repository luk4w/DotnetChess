using Engine;
using Enums;

namespace Pieces
{
    public class King : Piece
    {
        public King(ChessColor color, ref Piece[,] board) : base(color, ref board) { }

        public override bool[,] GetMoves(Position from)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "K";
        }
    }
}