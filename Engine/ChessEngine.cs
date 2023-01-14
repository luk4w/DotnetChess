using System.Diagnostics;
using Enums;
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

        private bool SetAvailableMoves()
        {
            Array.Clear(AvailableMoves);
            if (Board.OnSelectedPiece() is not Empty && Board.OnSelectedPiece().Color == PlayerTurnColor && Board.OnSelectedPosition() != null)
            {

                bool[,] unfilteredMoves = Board.OnSelectedPieceMoves();
                Position kingPos = Board.OnSelectedKingPosition();
                Position pos = Board.OnSelectedPosition();
                int availableCount = 0;
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (unfilteredMoves[i, j])
                        {
                            Board.RemoveOnSelectedPiece();
                            bool[,] opponentMoves = Board.GetAllOpponentMoves(PlayerTurnColor);
                            if (opponentMoves[kingPos.X, kingPos.Y])
                            {
                                Array.Clear(AvailableMoves);
                                break;
                            }
                            Board.RestoreOnSelectedPiece();

                            // Test moves
                            bool captured = Board.GetPiece(i, j) is not Empty;

                            Board.MoveOnSelectedPiece(i, j);

                            opponentMoves = Board.GetAllOpponentMoves(PlayerTurnColor);

                            if (opponentMoves[kingPos.X, kingPos.Y] == false)
                            {
                                availableCount++;
                                AvailableMoves[i, j] = true;
                            }

                            Board.MoveOnSelectedPiece(pos.X, pos.Y); // initial position

                            if (captured)
                                Board.RestoreCapturedPiece(i, j);
                        }
                    }
                }
                if (availableCount > 0)
                    return true;
            }
            PlayTurn();
            return false;
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

        private void PlayTurn() => PlayTurn(null);

        private void PlayTurn(Position? selectPiece)
        {
            Array.Clear(AvailableMoves);
            ShowLegalMoves(AvailableMoves);

            if (selectPiece == null)
                Board.SelectPiece(SelectInput(PlayerTurnColor));

            //Debug.WriteLine($"{Board.OnSelectedPosition()}");
            if (SetAvailableMoves())
            {
                ShowLegalMoves(AvailableMoves);
                Position moveTo = MoveInput();
                if (AvailableMoves[moveTo.X, moveTo.Y] == true)
                {
                    Board.MoveOnSelectedPiece(moveTo);
                }
                else if (Board.GetPieceColor(moveTo) == PlayerTurnColor)
                {
                    Board.SelectPiece(moveTo);
                    PlayTurn(moveTo);
                }
                else
                {
                    PlayTurn();
                }
            }
        }

        private void EndTurn()
        {
            Board.DeselectPiece();
            PlayerTurnColor = PlayerTurnColor == ChessColor.White ? ChessColor.Black : ChessColor.White;
            Update();
        }

        public abstract Position SelectInput(ChessColor playerTurnColor);
        public abstract Position MoveInput();
        public abstract void ShowLegalMoves(bool[,] legalMoves);
        public abstract void UpdateBoard();
    }
}