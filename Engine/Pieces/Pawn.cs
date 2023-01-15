using System.Diagnostics;
using Engine;
using Enums;

namespace Pieces
{
    public class Pawn : Piece
    {
        public Pawn(ChessColor color, Piece[,] board) : base(color, ref board) { }

        public bool[,] GetAttackMoves(int row, int col)
        {
            bool[,] atkMoves = new bool[8, 8];
            int dir = this.Color == ChessColor.White ? -1 : 1;
    
            // NW and SW
            if (row + dir >= 0 && row + dir < 8 && col - 1 >= 0)
            {
                if (Board[row + dir, col - 1] is Empty || Board[row + dir, col + 1].Color == Color)
                    atkMoves[row + dir, col - 1] = true;
            }

            // NE and SE
            if (row + dir >= 0 && row + dir < 8 && col + 1 < 8)
            {
                if (Board[row + dir, col + 1] is Empty || Board[row + dir, col + 1].Color == Color)
                    atkMoves[row + dir, col + 1] = true;
            }
            return atkMoves;
        }

        public override bool[,] GetMoves(Position from)
        {
            bool[,] moves = new bool[8, 8];
            int dir = this.Color == ChessColor.White ? -1 : 1;
            int row = from.X;
            int col = from.Y;

            // N and S
            if (row + dir >= 0 && row + dir < moves.GetLength(0))
            {
                if (Board[row + dir, col] is Empty)
                {
                    moves[row + dir, col] = true;
                    // Move two squares
                    if (Board[row + (dir * 2), col] is Empty && MoveCount == 0)
                        moves[row + (dir * 2), col] = true;
                }
            }

            // NW and SW
            if (row + dir >= 0 && row + dir < moves.GetLength(0) && col - 1 >= 0)
            {
                if (Board[row + dir, col - 1] is not Empty && Board[row + dir, col - 1].Color != Color)
                    moves[row + dir, col - 1] = true;
            }

            // NE and SE
            if (row + dir >= 0 && row + dir < 8 && col + 1 < moves.GetLength(1))
            {
                if (Board[row + dir, col + 1] is not Empty && Board[row + dir, col + 1].Color != Color)
                    moves[row + dir, col + 1] = true;
            }

            return moves;
        }

        public override string ToString()
        {
            return "P";
        }
    }
}