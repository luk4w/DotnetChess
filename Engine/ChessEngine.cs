using Enums;
using Pieces;

namespace Engine
{
    public abstract class ChessEngine
    {
        protected Piece[,] Board { get; set; }
        protected Position Select { get; set; }
        private bool IsRunning{ get; set; }
        private ChessColor PlayerTurn { get; set; }

        public ChessEngine()
        {
            Board = new Piece[8, 8];
            Select = new Position();
            PlayerTurn = ChessColor.White;
        }
        public void RunEngine()
        {
            IsRunning = true;
            Loop();
        }

        private void PlaceDefaultPieces()
        {
            Board[0,0] = new Rook(ChessColor.Black, Board);
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
        }

        public abstract void SelectPiece();
        public abstract void MovePiece();
        public abstract void UpdateBoard();
    }
}