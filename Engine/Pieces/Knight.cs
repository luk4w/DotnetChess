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
            int[] rowJumps = { -2, -2, -1, -1, 1, 1, 2, 2 };
            int[] colJump = { -1, 1, -2, 2, -2, 2, -1, 1 };

            for (int i = 0; i < 8; i++)
            {
                int x = from.X + rowJumps[i];
                int y = from.Y + colJump[i];
                if (x >= 0 && x < 8 && y >= 0 && y < 8 && (Board[x, y] is Empty || Board[x, y].Color != Color))
                    moves[x, y] = true;             
            }
            return moves;
        }

        public override string? ToString()
        {
            return "N";
        }
    }
}