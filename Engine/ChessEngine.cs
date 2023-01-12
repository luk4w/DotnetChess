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
            // Black pieces
            for (int j = 0; j < Board.GetLength(1); j++)
                Board[1, j] = new Pawn(ChessColor.Black, Board);

            Board[0, 0] = new Rook(ChessColor.Black, Board);
            Board[0, 7] = new Rook(ChessColor.Black, Board);

            Board[0, 1] = new Knight(ChessColor.Black, Board);
            Board[0, 6] = new Knight(ChessColor.Black, Board);

            Board[0, 2] = new Bishop(ChessColor.Black, Board);
            Board[0, 5] = new Bishop(ChessColor.Black, Board);

            Board[0, 3] = new Queen(ChessColor.Black, Board);
            Board[0, 4] = new King(ChessColor.Black, Board);

            // Empty squares
            for (int i = 2; i < 6; i++)
                for (int j = 0; j < Board.GetLength(1); j++)
                    Board[i, j] = new Empty(ChessColor.White, Board);

            // White Pieces
            for (int j = 0; j < Board.GetLength(1); j++)
                Board[6, j] = new Pawn(ChessColor.White, Board);

            Board[7, 0] = new Rook(ChessColor.White, Board);
            Board[7, 7] = new Rook(ChessColor.White, Board);

            Board[7, 1] = new Knight(ChessColor.White, Board);
            Board[7, 6] = new Knight(ChessColor.White, Board);

            Board[7, 2] = new Bishop(ChessColor.White, Board);
            Board[7, 5] = new Bishop(ChessColor.White, Board);

            Board[7, 3] = new Queen(ChessColor.White, Board);
            Board[7, 4] = new King(ChessColor.White, Board);
        }

        public void Exit()
        {
            IsRunning = false;
        }

        private void Loop()
        {
            while (IsRunning)
            {
                Update();

                SelectPiece();
                Update();

                MovePiece();

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