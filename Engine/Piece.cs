using Enums;
using Pieces;

namespace Engine
{
    public abstract class Piece
    {
        public ChessColor Color;
        private Piece[,] Board;

        public Piece(ChessColor color, ref Piece[,] board)
        {
            Color = color;
            Board = board;
        }

        public virtual void Move(Position from, Position to)
        {
            Board[to.X, to.Y] = Board[from.X, from.Y];
            Board[from.X, from.Y] = new Empty(ChessColor.White, Board);
        }

        public abstract bool[,] GetMoves(Position from);
    }
}