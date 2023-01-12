using Engine;
using Enums;

namespace Pieces
{
    public class Knight : Piece
    {
        public Knight(ChessColor color, Piece[,] board) : base(color, ref board) { }

        public override bool[,] GetMoves(Position from)
        {
            bool [,] moves = new bool[8,8];
            return moves;
        }

        public override string? ToString()
        {
            return "N";
        }
    }
}