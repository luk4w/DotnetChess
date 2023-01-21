using Enums;
using Pieces;

namespace Source
{
    public abstract class Chess
    {
        protected Board Board;
        protected bool[,] AvailableMoves { get; set; }
        private int AvailableCount = 0;
        private GameState State { get; set; }
        private ChessColor PlayerTurnColor { get; set; }

        private bool IsDraw { get; set; }

        public Chess()
        {
            Board = new Board();
            PlayerTurnColor = ChessColor.White;
            AvailableMoves = new bool[8, 8];

        }
        public void StartGame()
        {
            State = GameState.Running;
            Loop();
        }
        private void PlayTurn() => PlayTurn(null);

        private void EndTurn()
        {
            PlayerTurnColor = PlayerTurnColor == ChessColor.White ? ChessColor.Black : ChessColor.White;
            setGameState();
            Update();
            Board.DeselectPiece();
        }

        private void Loop()
        {
            Update();
            while (State == GameState.Running)
            {
                PlayTurn();
                EndTurn();
            }

            if (State == GameState.Draw)
            {
                Draw();
            }
            else if (State == GameState.Checkmate)
            {
                ChessColor color = PlayerTurnColor == ChessColor.Black ? ChessColor.White : ChessColor.Black;
                Winner(color);
            }
        }

        public void Update()
        {
            UpdateBoard();
        }

        private void PlayTurn(Position? selectPiece)
        {
            Array.Clear(AvailableMoves);
            AvailableCount = 0;
            Board.DeselectPiece();
            ShowLegalMoves(AvailableMoves);

            if (selectPiece == null)
                Board.SelectPiece(SelectInput(PlayerTurnColor));
            else
                Board.SelectPiece(selectPiece);

            SetAvailableMoves();

            if (AvailableCount == 0)
                PlayTurn();

            Position moveTo = MoveInput();

            if (AvailableMoves[moveTo.X, moveTo.Y] == true)
            {
                if (Board.OnSelectedPiece() is King)
                {
                    if (Board.OnSelectedPosition().Y + 2 == moveTo.Y)
                        ((King)Board.OnSelectedPiece()).KingsideCastle();
                    else if (Board.OnSelectedPosition().Y - 2 == moveTo.Y)
                        ((King)Board.OnSelectedPiece()).QueensideCastle();
                }
                else
                    Board.MoveOnSelectedPiece(moveTo);

                Board.OnSelectedPiece().MoveCount++;
            }
            else if (Board.GetPiece(moveTo) is not Empty && Board.GetPieceColor(moveTo) == PlayerTurnColor)
            {
                PlayTurn(moveTo);
            }
            else
            {
                PlayTurn();
            }
        }

        private void SetAvailableMoves()
        {
            if (Board.OnSelectedPiece() is not Empty && Board.OnSelectedPiece().Color == PlayerTurnColor && Board.OnSelectedPosition() != null)
            {

                bool[,] unfilteredMoves = Board.OnSelectedPieceMoves();
                Position kingPos = Board.GetKingPosition(PlayerTurnColor);
                Position pos = Board.OnSelectedPosition();
                bool[,] opponentMoves = Board.GetAllOpponentMoves(PlayerTurnColor);

                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                        if (Board.OnSelectedPiece() is King)
                        {
                            // Normal moves
                            if (unfilteredMoves[i, j] == true)
                            {
                                bool captured = Board.GetPiece(i, j) is not Empty;
                                Board.MoveOnSelectedPiece(i, j);
                                opponentMoves = Board.GetAllOpponentMoves(PlayerTurnColor);

                                if (opponentMoves[i, j] == false)
                                {
                                    AvailableCount++;
                                    AvailableMoves[i, j] = true;
                                }
                                Board.MoveOnSelectedPiece(pos.X, pos.Y);

                                if (captured)
                                    Board.RestoreCapturedPiece(i, j);
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
                                    {
                                        AvailableMoves[pos.X, pos.Y + 2] = true;
                                        AvailableCount++;
                                    }
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
                                    {
                                        AvailableMoves[pos.X, pos.Y - 2] = true;
                                        AvailableCount++;
                                    }
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

        private GameState setGameState()
        {
            Position kingPos = Board.GetKingPosition(PlayerTurnColor);
            bool[,] opponentMoves = Board.GetAllOpponentMoves(PlayerTurnColor);
            AvailableCount = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Board.GetPieceColor(i, j) == PlayerTurnColor)
                    {
                        Board.SelectPiece(i, j);
                        bool[,] unfilteredMoves = Board.OnSelectedPieceMoves();
                        for (int row = 0; row < unfilteredMoves.GetLength(0); row++)
                        {
                            for (int col = 0; col < unfilteredMoves.GetLength(1); col++)
                            {
                                if (Board.OnSelectedPiece() is King && unfilteredMoves[row, col] == true)
                                {
                                    bool captured = Board.GetPiece(row, col) is not Empty;
                                    Board.MoveOnSelectedPiece(row, col);
                                    opponentMoves = Board.GetAllOpponentMoves(PlayerTurnColor);

                                    // king here
                                    if (opponentMoves[row, col] == false)
                                    {
                                        AvailableCount++;
                                        AvailableMoves[row, col] = true;
                                    }
                                    Board.MoveOnSelectedPiece(i, j);

                                    if (captured)
                                        Board.RestoreCapturedPiece(row, col);
                                }
                                // Normal moves
                                else if (unfilteredMoves[row, col] == true)
                                {
                                    bool captured = Board.GetPiece(row, col) is not Empty;
                                    Board.MoveOnSelectedPiece(row, col);
                                    opponentMoves = Board.GetAllOpponentMoves(PlayerTurnColor);

                                    if (opponentMoves[kingPos.X, kingPos.Y] == false)
                                    {
                                        AvailableCount++;
                                        AvailableMoves[row, col] = true;
                                    }
                                    Board.MoveOnSelectedPiece(i, j);

                                    if (captured)
                                        Board.RestoreCapturedPiece(row, col);
                                }
                            }
                        }
                    }

                }
                if (AvailableCount == 0)
                    if (opponentMoves[kingPos.X, kingPos.Y] == true)
                        return GameState.Checkmate;
                    else
                        return GameState.Draw;
            }
            return GameState.Running;
        }

        public abstract Position SelectInput(ChessColor playerTurnColor);
        public abstract Position MoveInput();
        public abstract void ShowLegalMoves(bool[,] legalMoves);
        public abstract void UpdateBoard();
        public abstract char Promote();
        public abstract void Draw();
        public abstract void Winner(ChessColor color);
    }
}