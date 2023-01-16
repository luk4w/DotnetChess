using Source;
using Enums;

namespace Pieces
{
    public class Bishop : Piece
    {
        public Bishop(ChessColor color, Piece[,] board) : base(color, ref board) { }

        public override bool[,] GetMoves(Position from)
        {
            bool[,] moves = new bool[8, 8];
            int row = from.X;
            int col = from.Y;

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
            return "B";
        }
    }
}