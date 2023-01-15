using Engine;
using Enums;

namespace Pieces
{
    public class King : Piece
    {
        public King(ChessColor color, Piece[,] board) : base(color, ref board) { }

        public override bool[,] GetMoves(Position from)
        {
            bool[,] moves = new bool[8, 8];
            int row = from.X;
            int col = from.Y;

            // N
            if (row - 1 >= 0 && (Board[row - 1, col] is Empty || Board[row - 1, col].Color != Color))
                moves[row - 1, col] = true;

            // S
            if (row + 1 < 8 && (Board[row + 1, col] is Empty || Board[row + 1, col].Color != Color))
                moves[row + 1, col] = true;

            // W
            if (col - 1 >= 0 && (Board[row, col - 1] is Empty || Board[row, col - 1].Color != Color))
                moves[row, col - 1] = true;

            // E
            if (col + 1 < 8 && (Board[row, col + 1] is Empty || Board[row, col + 1].Color != Color))
                moves[row, col + 1] = true;

            // NW
            if (row - 1 >= 0 && col - 1 >= 0 && (Board[row - 1, col - 1] is Empty || Board[row - 1, col - 1].Color != Color))
                moves[row - 1, col - 1] = true;
            
            // NE
            if (row - 1 >= 0 && col + 1 >= 0 && (Board[row - 1, col + 1] is Empty || Board[row - 1, col + 1].Color != Color))
                moves[row - 1, col + 1] = true;
            
            // SW
            if (row + 1 < 8 && col - 1 >= 0 && (Board[row + 1, col - 1] is Empty || Board[row + 1, col - 1].Color != Color))
                moves[row + 1, col - 1] = true;
            
            // SW
            if (row + 1 < 8 && col + 1 < 8 && (Board[row + 1, col + 1] is Empty || Board[row + 1, col + 1].Color != Color))
                moves[row + 1, col + 1] = true;

            return moves;
        }

        public override string ToString()
        {
            return "K";
        }
    }
}