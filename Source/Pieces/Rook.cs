using Source;
using Enums;

namespace Pieces
{
    public class Rook : Piece
    {
        public Rook(ChessColor color, Piece[,] board) : base(color, ref board) { }

        public override bool[,] GetMoves(Position from)
        {
            bool [,] moves = new bool[8,8];
            int row = from.X;
            int col = from.Y;
            
            // Up
            for (int x = row - 1; x >= 0; x--)
            {
                if (Board[x, col] is not Empty)
                {
                    if (Board[x, col].Color != Color)
                        moves[x, col] = true;
                    break;
                }

                moves[x, col] = true;
            }

            // Down
            for (int x = row + 1; x < 8; x++)
            {
                if (Board[x, col] is not Empty)
                {
                    if (Board[x, col].Color != Color)
                        moves[x, col] = true;
                    break;
                }
                moves[x, col] = true;
            }

            // Right
            for (int y = col + 1; y < 8; y++)
            {
                if (Board[row, y] is not Empty)
                {
                    if (Board[row, y].Color != Color)
                        moves[row, y] = true;
                    break;
                }
                moves[row, y] = true;
            }

            // Left
            for (int y = col - 1; y >= 0; y--)
            {
                if (Board[row, y] is not Empty)
                {
                    if (Board[row, y].Color != Color)
                        moves[row, y] = true;
                    break;
                }
                moves[row, y] = true;
            }
            return moves;
        }

        public override string ToString()
        {
            return "R";
        }

    }
}