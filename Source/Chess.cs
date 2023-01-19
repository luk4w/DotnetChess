using Enums;
using Pieces;

namespace Source
{
    public abstract class Chess
    {
        protected Board Board;
        protected bool[,] AvailableMoves { get; set; }
        private int AvailableCount = 0;
        private bool IsRunning { get; set; }
        private ChessColor PlayerTurnColor { get; set; }

        public Chess()
        {
            Board = new Board();
            PlayerTurnColor = ChessColor.White;
            AvailableMoves = new bool[8, 8];

        }
        public void StartGame()
        {
            IsRunning = true;
            Loop();
        }

        private void SetAvailableMoves()
        {
            Array.Clear(AvailableMoves);
            AvailableCount = 0;
            if (Board.OnSelectedPiece() is not Empty && Board.OnSelectedPiece().Color == PlayerTurnColor && Board.OnSelectedPosition() != null)
            {

                bool[,] unfilteredMoves = Board.OnSelectedPieceMoves();
                Position kingPos = Board.OnSelectedKingPosition();
                Position pos = Board.OnSelectedPosition();
                bool[,] opponentMoves = Board.GetAllOpponentMoves(PlayerTurnColor);

                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                        if (Board.OnSelectedPiece() is King)
                        {
                            // Normal moves
                            if (unfilteredMoves[i, j] == true)
                            {
                                AvailableMoves[i, j] = opponentMoves[i, j] == false;
                                AvailableCount++;
                            }

                            // Castle
                            King king = (King)Board.OnSelectedPiece();
                            if (king.IsKingsideCastle())
                            {
                                // The king can not be in check
                                if (opponentMoves[pos.X, pos.Y] == false)
                                {
                                    bool available = true;
                                    for (int col = pos.Y + 1; col < pos.Y + 2; col++)
                                    {
                                        if (opponentMoves[pos.X, col] == true)
                                        {
                                            available = false;
                                            break;
                                        }
                                    }
                                    if (available)
                                        AvailableMoves[pos.X, pos.Y + 2] = true;
                                }
                            }

                            if (king.IsQueensideCastle())
                            {
                                // The king can not be in check
                                if (opponentMoves[pos.X, pos.Y] == false)
                                {
                                    bool available = true;
                                    for (int col = pos.Y - 1; col > pos.Y - 2; col--)
                                    {
                                        if (opponentMoves[pos.X, col] == true)
                                        {
                                            available = false;
                                            break;
                                        }
                                    }
                                    if (available)
                                        AvailableMoves[pos.X, pos.Y - 2] = true;
                                }
                            }
                        }
                        else if (unfilteredMoves[i, j])
                        {
                            Board.RemoveOnSelectedPiece();

                            opponentMoves = Board.GetAllOpponentMoves(PlayerTurnColor);
                            if (opponentMoves[kingPos.X, kingPos.Y])
                            {
                                Array.Clear(AvailableMoves);
                                Board.RestoreOnSelectedPiece();
                                break;
                            }
                            Board.RestoreOnSelectedPiece();

                            // Test moves
                            bool captured = Board.GetPiece(i, j) is not Empty;

                            Board.MoveOnSelectedPiece(i, j);

                            opponentMoves = Board.GetAllOpponentMoves(PlayerTurnColor);

                            if (opponentMoves[kingPos.X, kingPos.Y] == false)
                            {
                                AvailableCount++;
                                AvailableMoves[i, j] = true;
                            }

                            Board.MoveOnSelectedPiece(pos.X, pos.Y); // initial position

                            if (captured)
                                Board.RestoreCapturedPiece(i, j);
                        }
                if (AvailableCount > 0)
                {
                    ShowLegalMoves(AvailableMoves);
                    return;
                }
            }
            PlayTurn();
        }


        public void StopGame()
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
            Board.DeselectPiece();
            ShowLegalMoves(AvailableMoves);

            if (selectPiece == null)
                Board.SelectPiece(SelectInput(PlayerTurnColor));

            SetAvailableMoves();
            if(AvailableCount == 0)
                PlayTurn();
            
            Position moveTo = MoveInput();

            if (AvailableMoves[moveTo.X, moveTo.Y] == true)
            {
                if (Board.OnSelectedPiece() is King)
                {
                    if(Board.OnSelectedPosition().Y + 2 == moveTo.Y)
                        ((King)Board.OnSelectedPiece()).KingsideCastle();
                    else if(Board.OnSelectedPosition().Y - 2 == moveTo.Y)
                        ((King)Board.OnSelectedPiece()).QueensideCastle();
                }
                else
                    Board.MoveOnSelectedPiece(moveTo);

                Board.OnSelectedPiece().MoveCount++;
            }
            else if (Board.GetPiece(moveTo) is not Empty && Board.GetPieceColor(moveTo) == PlayerTurnColor)
            {
                Board.SelectPiece(moveTo);
                PlayTurn(moveTo);
            }
            else
            {
                PlayTurn();
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
        public abstract char Promote();
    }
}