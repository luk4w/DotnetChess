using Engine;
using Enums;

namespace Pieces
{
    public class Queen : Piece
    {
        public Queen(ChessColor color, Piece[,] board) : base(color, ref board) { }

        public override bool[,] GetMoves(Position from)
        {
            bool[,] moves = new bool[8, 8];

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

            // Northweast
            for (int i = row - 1, j = col - 1; i >= 0 && j >= 0; i--, j--)
            {
                if (Board[i, j] is not Empty)
                {
                    if (Board[i, j].Color != Color)
                        moves[i, j] = true;
                    break;
                }
                moves[i, j] = true;
            }

            // Norteast
            for (int i = row - 1, j = col + 1; i >= 0 && j < moves.GetLength(1); i--, j++)
            {
                if (Board[i, j] is not Empty)
                {
                    if (Board[i, j].Color != Color)
                        moves[i, j] = true;
                    break;
                }
                moves[i, j] = true;
            }

            // Southweast
            for (int i = row + 1, j = col - 1; i < moves.GetLength(0) && j >= 0; i++, j--)
            {
                if (Board[i, j] is not Empty)
                {
                    if (Board[i, j].Color != Color)
                        moves[i, j] = true;
                    break;
                }
                moves[i, j] = true;
            }

            // Southeast
            for (int i = row + 1, j = col + 1; i < moves.GetLength(0) && j < moves.GetLength(1); i++, j++)
            {
                if (Board[i, j] is not Empty)
                {
                    if (Board[i, j].Color != Color)
                        moves[i, j] = true;
                    break;
                }
                moves[i, j] = true;
            }

            return moves;
        }

        public override string ToString()
        {
            return "Q";
        }
    }
}