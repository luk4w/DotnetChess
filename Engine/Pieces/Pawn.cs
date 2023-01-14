using System.Diagnostics;
using Engine;
using Enums;

namespace Pieces
{
    public class Pawn : Piece
    {
        public Pawn(ChessColor color, Piece[,] board) : base(color, ref board) { }

        public override bool[,] GetMoves(Position from)
        {
            bool[,] moves = new bool[8, 8];
            int direction = this.Color == ChessColor.White ? -1 : 1;
            int row = from.X;
            int col = from.Y;

            // Up
            if (row + direction >= 0 && row + direction < moves.GetLength(0))
            {
                if (Board[row + direction, col] is Empty)
                {
                    moves[row + direction, col] = true;
                    // Move two squares
                    if (Board[row + (direction * 2), col] is Empty && MoveCount == 0)
                        moves[row + (direction * 2), col] = true; 
                }
            }

            // Verifica se a posição diagonal esquerda do peão está dentro do tabuleiro
            if (row + direction >= 0 && row + direction < moves.GetLength(0) && col - 1 >= 0)
            {
                // Marca a posição diagonal esquerda do peão como não válida
                moves[row + direction, col - 1] = false;
            }

            // Verifica se a posição diagonal direita do peão está dentro do tabuleiro
            if (row + direction >= 0 && row + direction < moves.GetLength(0) && col + 1 < moves.GetLength(1))
            {
                // Marca a posição diagonal direita do peão como não válida
                moves[row + direction, col + 1] = false;
            }

            return moves;
        }

        public override string ToString()
        {
            return "P";
        }
    }
}