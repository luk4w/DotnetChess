using System.Diagnostics;
using Enums;
using Exceptions;
using Pieces;

namespace Engine
{
    public abstract class ChessEngine
    {
        protected Board Board;
        protected bool[,] AvailableMoves { get; set; }
        private bool IsRunning { get; set; }
        private ChessColor PlayerTurnColor { get; set; }


        public ChessEngine()
        {
            Board = new Board();
            PlayerTurnColor = ChessColor.White;
            AvailableMoves = new bool[8, 8];

        }
        public void RunEngine()
        {
            IsRunning = true;
            Loop();
        }

        private void SetAvailableMoves()
        {
            Array.Clear(AvailableMoves);
            if (Board.OnSelectedPiece() is not Empty && Board.OnSelectedPiece().Color == PlayerTurnColor && Board.OnSelectedPosition() != null)
            {

                bool[,] unfilteredMoves = Board.OnSelectedPieceMoves();
                Position kingPos = Board.OnSelectedKingPosition();
                Position pos = Board.OnSelectedPosition();
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        // Opponent moves
                        if (Board.GetPiece(i, j) is not Empty && Board.GetPieceColor(i, j) != PlayerTurnColor)
                        {
                            bool[,] opponentMoves = Board.GetPieceMoves(new Position(i, j));
                            Board.RemoveOnSelectedPiece();

                            if (opponentMoves[kingPos.X, kingPos.Y] == true)
                            {
                                Array.Clear(AvailableMoves);
                                break;
                            }
                            Board.RestoreOnSelectedPiece();
                        }

                        if (unfilteredMoves[i, j])
                        {
                            Debug.WriteLine($"Entoru aq {i} {j}");
                            bool captured = Board.GetPiece(i, j) is not Empty;
                            Board.MoveOnSelectedPiece(i, j);

                            
                            bool[,] opponentMoves2 = Board.GetPieceMoves(new Position(i, j));
                            AvailableMoves[i, j] = opponentMoves2[kingPos.X, kingPos.Y] == false;

                            Board.MoveOnSelectedPiece(pos.X, pos.Y); // initial position 
                            if (captured)
                                Board.RestoreCapturedPiece(i, j);

                        }
                    }
                }
                foreach (bool available in AvailableMoves)
                    if (available) return;
            }
            PlayTurn();
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
            Board.SelectPiece(SelectInput());

            SetAvailableMoves();
            ShowLegalMoves(AvailableMoves);

            Board.MoveOnSelectedPiece(MoveInput());
        }

        private void EndTurn()
        {
            Board.DeselectPiece();
            PlayerTurnColor = PlayerTurnColor == ChessColor.White ? ChessColor.Black : ChessColor.White;
            Update();
        }

        public abstract Position SelectInput();
        public abstract Position MoveInput();
        public abstract void ShowLegalMoves(bool[,] legalMoves);
        public abstract void UpdateBoard();
    }
}