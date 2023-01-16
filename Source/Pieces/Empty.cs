using Source;
using Enums;
using Exceptions;

namespace Pieces
{
    public class Empty : Piece
    {
        public Empty(ChessColor color, Piece[,] board) : base(color, ref board) { }

        public override void Move(Position from, Position to)
        {
            throw new ChessEngineException($"Empty square!");
        }

        public override bool[,] GetMoves(Position from)
        {
            throw new ChessEngineException($"Empty square!");
        }
    }
}