using Engine;
using Enums;

namespace Pieces
{
    public class Knight : Piece
    {
        public Knight(ChessColor color, ref Piece[,] board) : base(color, ref board) { }

        public override bool[,] GetMoves(Position from)
        {
            throw new NotImplementedException();
        }

        public override string? ToString()
        {
            return "N";
        }
    }
}