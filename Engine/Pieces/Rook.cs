using Engine;
using Enums;

namespace Pieces
{
    public class Rook : Piece
    {
        public Rook(ChessColor color, Piece[,] board) : base(color, ref board) { }

        public override bool[,] GetMoves(Position from)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "R";
        }

    }
}