using Enums;
using Exceptions;
using Pieces;

namespace Engine
{
    public abstract class ChessEngine
    {
        protected Piece[,] Board { get; set; }
        protected Position? Selected { get; set; }
        protected bool[,] AvailableMoves { get; set; }
        private bool IsRunning { get; set; }
        private ChessColor PlayerTurn { get; set; }
        private Position[] KingPositions { get; set; }

        public ChessEngine()
        {
            Board = new Piece[8, 8];
            PlaceDefaultPieces();
            Selected = null;
            PlayerTurn = ChessColor.White;
            AvailableMoves = new bool[8, 8];
            KingPositions = new Position[2] { new Position(7, 4), new Position(0, 4) };
        }
        public void RunEngine()
        {
            IsRunning = true;
            Loop();
        }

        private void SetAvailableMoves()
        {
            Array.Clear(AvailableMoves);
            if (Selected != null)
            {
                if(Board[Selected.X, Selected.Y] is Empty)
                    throw new ChessEngineException("Impossible to move, empty square!");

                Piece p = Board[Selected.X, Selected.Y];
                bool[,] unfilteredMoves = p.GetMoves(Selected);
                ChessColor opponentColor = p.Color == ChessColor.White ? ChessColor.Black : ChessColor.White;
                Position kingPos = p.Color == ChessColor.White ? KingPositions[0] : KingPositions[1];

                for (int i = 0; i < Board.GetLength(0); i++)
                {
                    for (int j = 0; j < Board.GetLength(1); j++)
                    {
                        if (Board[i, j] is not Empty && Board[i, j].Color == opponentColor && unfilteredMoves[i, j] && Selected != null)
                        {
                            // Move temporarily
                            Board[i, j].Move(Selected, new Position(i, j));

                            bool[,] opponentMoves = Board[i, j].GetMoves(new Position(i, j));
                            if (opponentMoves[kingPos.X, kingPos.Y] == false)
                                AvailableMoves[i, j] = true;

                            // Undo move
                            Board[i, j].Move(new Position(i, j), Selected);
                        }
                    }
                }
            }
            else
                throw new ChessEngineException("The piece was not been selected!");
        }

        public void StopEngine()
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
                EndTurn();
            }
        }

        private void Update()
        {
            UpdateBoard();
        }

        private void SelectPiece()
        {
            SelectInput();
            SetAvailableMoves();
        }

        public void MovePiece()
        {
            MoveInput();
        }

        private void EndTurn()
        {
            Selected = null;
            PlayerTurn = PlayerTurn == ChessColor.White ? ChessColor.Black : ChessColor.White;
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

        public abstract void SelectInput();
        public abstract void MoveInput();
        public abstract void UpdateBoard();
    }
}