using Source;
using Enums;

namespace Pieces
{
    public class Pawn : Piece
    {
        public bool EnPassant { get; set; }
        public Pawn(ChessColor color, Piece[,] board) : base(color, ref board) { }

        public void Promote(int row, int col, Piece piece) => Board[row, col] = piece;

        public bool[,] GetAttackMoves(int row, int col)
        {
            bool[,] atkMoves = new bool[8, 8];
            int dir = this.Color == ChessColor.White ? -1 : 1;
            int right = col + 1;
            int left = col - 1;

            // NW or SW
            if (row + dir >= 0 && row + dir < 8 && left >= 0)
            {
                if (Board[row + dir, left] is Empty || Board[row + dir, left].Color == Color)
                    atkMoves[row + dir, left] = true;
            }

            // NE or SE
            if (row + dir >= 0 && row + dir < 8 && right < 8)
            {
                if (Board[row + dir, right] is Empty || Board[row + dir, right].Color == Color)
                    atkMoves[row + dir, right] = true;
            }
            return atkMoves;
        }

        public override bool[,] GetMoves(Position from)
        {
            bool[,] moves = new bool[8, 8];
            int dir = this.Color == ChessColor.White ? -1 : 1;
            int row = from.X;
            int col = from.Y;
            int right = col + 1;
            int left = col - 1;

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
            if (row + dir >= 0 && row + dir < moves.GetLength(0) && left >= 0)
            {
                if (Board[row + dir, left] is not Empty && Board[row + dir, left].Color != Color)
                    moves[row + dir, left] = true;
            }

            // NE and SE
            if (row + dir >= 0 && row + dir < 8 && right < moves.GetLength(1))
            {
                if (Board[row + dir, right] is not Empty && Board[row + dir, right].Color != Color)
                    moves[row + dir, right] = true;
            }

            // En passant
            // NE or SE
            if (right < 8 && Board[row, right] is Pawn && ((Pawn)Board[row, right]).EnPassant == true && Board[row + dir, right] is Empty)
            {
                moves[row + dir, right] = true;
            }

            // NW or SW
            if (left >= 0 && Board[row, left] is Pawn && ((Pawn)Board[row, left]).EnPassant == true && Board[row + dir, left] is Empty)
            {
                moves[row + dir, left] = true;
            }

            return moves;
        }

        public override string ToString()
        {
            return "P";
        }
    }
}