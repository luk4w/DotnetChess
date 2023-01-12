using Enums;
using Pieces;

namespace Engine
{
    public abstract class ChessEngine
    {
        protected Piece[,] Board { get; set; }
        protected Position? Selected { get; set; }
        private bool IsRunning { get; set; }
        private ChessColor PlayerTurn { get; set; }

        public ChessEngine()
        {
            Board = new Piece[8, 8];
            PlaceDefaultPieces();
            Selected = null;
            PlayerTurn = ChessColor.White;
        }
        public void RunEngine()
        {
            IsRunning = true;
            Loop();
        }

        private void PlaceDefaultPieces()
        {
            for (int j = 0; j < Board.GetLength(1); j++)
            {
                Board[1, j] = new Pawn(ChessColor.Black, Board);
            }

            Board[0, 0] = new Rook(ChessColor.Black, Board);
            Board[0, 7] = new Rook(ChessColor.Black, Board);

            Board[0, 1] = new Knight(ChessColor.Black, Board);
            Board[0, 6] = new Knight(ChessColor.Black, Board);

            Board[0, 2] = new Bishop(ChessColor.Black, Board);
            Board[0, 5] = new Bishop(ChessColor.Black, Board);

            Board[0, 3] = new Queen(ChessColor.Black, Board);
            Board[0, 4] = new King(ChessColor.Black, Board);

            for (int i = 0; i < Board.GetLength(0); i++)
            {
                for (int j = 0; j < Board.GetLength(0); j++)
                {
                    if(Board[i, j] == null)
                    {
                        Board[i, j] = new Empty(ChessColor.White, Board);
                    }
                }
            }

        }

        public void Exit()
        {
            IsRunning = false;
        }

        private void Loop()
        {
            while (IsRunning)
            {
                SelectPiece();
                MovePiece();
                Update();
            }
        }

        private void Update()
        {
            UpdateBoard();
            PlayerTurn = PlayerTurn == ChessColor.White ? ChessColor.Black : ChessColor.White;
        }

        public abstract void SelectPiece();
        public abstract void MovePiece();
        public abstract void UpdateBoard();
    }
}