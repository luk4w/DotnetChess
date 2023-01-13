using System.Diagnostics;
using Enums;
using Exceptions;
using Pieces;

namespace Engine
{
    public abstract class ChessEngine
    {
        protected Piece[,] Board { get; set; }
        private Position? Selected { get; set; }
        protected bool[,] AvailableMoves { get; set; }
        private bool IsRunning { get; set; }
        private ChessColor PlayerTurnColor { get; set; }
        private Position[] KingPositions { get; set; }

        public ChessEngine()
        {
            Board = new Piece[8, 8];
            PlaceDefaultPieces();
            Selected = null;
            PlayerTurnColor = ChessColor.White;
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
                if (Board[Selected.X, Selected.Y].Color == PlayerTurnColor)
                {
                    Position pos = Selected;
                    if (Board[pos.X, pos.Y] is Empty)
                        throw new ChessEngineException("Empty square moved!");

                    Piece selectedPiece = Board[pos.X, pos.Y];
                    bool[,] unfilteredMoves = selectedPiece.GetMoves(pos);
                    ChessColor opponentColor = selectedPiece.Color == ChessColor.White ? ChessColor.Black : ChessColor.White;
                    Position kingPos = selectedPiece.Color == ChessColor.White ? KingPositions[0] : KingPositions[1];

                    for (int i = 0; i < Board.GetLength(0); i++)
                    {
                        for (int j = 0; j < Board.GetLength(1); j++)
                        {
                            if (Board[i, j] is not Empty && Board[i, j].Color == opponentColor && unfilteredMoves[i, j])
                            {
                                if (selectedPiece is not King)
                                {
                                    // Move temporarily
                                    Board[i, j].Move(pos, new Position(i, j));

                                    bool[,] opponentMoves = Board[i, j].GetMoves(new Position(i, j));
                                    if (opponentMoves[kingPos.X, kingPos.Y] == false)
                                        AvailableMoves[i, j] = true;

                                    // Undo move
                                    Board[i, j].Move(new Position(i, j), pos);
                                }
                                else
                                {
                                    bool[,] opponentMoves = Board[i, j].GetMoves(new Position(i, j));
                                    if (opponentMoves[pos.X, pos.Y] == false)
                                        AvailableMoves[i, j] = true;
                                }
                            }
                        }
                    }

                    foreach (bool available in AvailableMoves)
                        if (available) return;
                }

                // No moves available
                PlayTurn();
            }
            else
                throw new ChessEngineException("Null piece position!");
        }

        public void StopEngine()
        {
            IsRunning = false;
        }

        private void Loop()
        {
            Update();
            while (IsRunning)
            {
                PlayTurn();
                EndTurn();
            }
        }

        public void Update()
        {
            UpdateBoard();
        }

        private void PlayTurn()
        {
            Selected = SelectInput();
            SetAvailableMoves();
            ShowLegalMoves(AvailableMoves);
            MovePiece();
        }

        public void MovePiece()
        {
            Position? moveTo = MoveInput();
            if (moveTo != null && Selected != null)
            {
                if (AvailableMoves[moveTo.X, moveTo.Y] == true)
                {
                    Board[Selected.X, Selected.X].Move(Selected, moveTo);

                    // Update king position
                    if (Board[Selected.X, Selected.X] is King)
                    {
                        int index = Board[Selected.X, Selected.X].Color == ChessColor.White ? 0 : 1;
                        KingPositions[index] = moveTo;
                    }
                }
                else
                    PlayTurn();
            }
            else
                throw new ChessEngineException("Null position");
        }

        private void EndTurn()
        {
            Selected = null;
            PlayerTurnColor = PlayerTurnColor == ChessColor.White ? ChessColor.Black : ChessColor.White;
            Update();
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

        public abstract Position? SelectInput();
        public abstract Position? MoveInput();
        public abstract void ShowLegalMoves(bool[,] legalMoves);
        public abstract void UpdateBoard();
    }
}