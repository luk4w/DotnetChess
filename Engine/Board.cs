using Enums;
using Exceptions;
using Pieces;

namespace Engine
{
    public class Board
    {
        private Piece[,] Matrix;
        private List<Piece> CapturedPieceList;
        private Piece SelectedPiece;
        private Empty EmptySquare;
        private Piece LastRemovedPiece;
        private Position SelectedPosition;
        private Position[] KingPositions;

        public Board()
        {
            Matrix = new Piece[8, 8];
            PlaceDefaultPieces();
            CapturedPieceList = new List<Piece>();
            EmptySquare = new Empty(ChessColor.White, Matrix);
            SelectedPiece = EmptySquare;
            LastRemovedPiece = EmptySquare;
            KingPositions = new Position[2] { new Position(7, 4), new Position(0, 4) };
            SelectedPosition = new Position(8, 8); // Overflow initial position
        }

        public Piece OnSelectedPiece() => SelectedPiece;
        public Position OnSelectedPosition() => SelectedPosition;

        public void SelectPiece(int row, int col)
        {
            SelectedPiece = Matrix[row, col];
            SelectedPosition = new Position(row, col);
        }
        public void SelectPiece(Position pos)
        {
            SelectedPiece = Matrix[pos.X, pos.Y];
            SelectedPosition = new Position(pos.X, pos.Y);
        }

        public void DeselectPiece()
        {
            SelectedPiece = EmptySquare;
            SelectedPosition = new Position(8, 8); // Overflow initial position
        }

        public Piece GetPiece(int row, int col) => Matrix[row, col];
        public Piece GetPiece(Position pos) => Matrix[pos.X, pos.Y];

        public bool[,] GetPieceMoves(int row, int col) => Matrix[row, col].GetMoves(new Position(row, col));

        public bool[,] GetPieceMoves(Position pos) => Matrix[pos.X, pos.Y].GetMoves(new Position(pos.X, pos.Y));

        public ChessColor GetPieceColor(int row, int col) => Matrix[row, col].Color;
        public ChessColor GetPieceColor(Position pos) => Matrix[pos.X, pos.Y].Color;

        public bool[,] OnSelectedPieceMoves()
        {
            return SelectedPiece.GetMoves(SelectedPosition);
        }

        public Position OnSelectedKingPosition() => SelectedPiece.Color == ChessColor.White ? KingPositions[0] : KingPositions[1];

        public void RestoreOnSelectedPiece() => Matrix[SelectedPosition.X, SelectedPosition.Y] = LastRemovedPiece;

        public void RemoveOnSelectedPiece()
        {
            LastRemovedPiece = Matrix[SelectedPosition.X, SelectedPosition.Y];
            Matrix[SelectedPosition.X, SelectedPosition.Y] = EmptySquare;
        }

        public bool[,] GetAllOpponentMoves(ChessColor turnColor)
        {
            bool[,] result = new bool[8, 8];
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    if (GetPiece(i, j) is not Empty && GetPieceColor(i, j) != turnColor)
                    {
                        bool[,] temp = GetPieceMoves(i, j);
                        // Merge
                        for (int row = 0; row < 8; row++)
                        {
                            for (int col = 0; col < 8; col++)
                            {
                                if (temp[row, col] == true)
                                    result[row, col] = true;
                            }
                        }
                    }
                }
            return result;
        }

        public Piece GetLastRemovedPiece() => LastRemovedPiece;

        public bool MoveOnSelectedPiece(int row, int col)
        {
            return MoveOnSelectedPiece(new Position(row, col));
        } 

        public void RestoreCapturedPiece(int row, int col)
        {
            Matrix[row, col] = CapturedPieceList.Last();
            CapturedPieceList.Remove(CapturedPieceList.Last());
        }

        private void RemovePiece(int row, int col) => Matrix[row, col] = EmptySquare;

        public bool MoveOnSelectedPiece(Position to)
        {
            if (SelectedPiece is not Empty)
            {
                if (Matrix[to.X, to.Y] is not Empty)
                    CapturedPieceList.Add(Matrix[to.X, to.Y]);

                Matrix[to.X, to.Y] = SelectedPiece;
                RemovePiece(SelectedPosition.X, SelectedPosition.Y);

                SelectedPiece = Matrix[to.X, to.Y];
                SelectedPosition = to;

                return true;
            }
            else
                throw new ChessEngineException("Null piece moved!");
        }

        public void SetPiece(Piece piece, Position pos) => Matrix[pos.X, pos.Y] = piece;

        private void PlaceDefaultPieces()
        {
            // Black pieces
            for (int j = 0; j < Matrix.GetLength(1); j++)
                Matrix[1, j] = new Pawn(ChessColor.Black, Matrix);

            Matrix[0, 0] = new Rook(ChessColor.Black, Matrix);
            Matrix[0, 7] = new Rook(ChessColor.Black, Matrix);
            Matrix[0, 1] = new Knight(ChessColor.Black, Matrix);
            Matrix[0, 6] = new Knight(ChessColor.Black, Matrix);
            Matrix[0, 2] = new Bishop(ChessColor.Black, Matrix);
            Matrix[0, 5] = new Bishop(ChessColor.Black, Matrix);
            Matrix[0, 3] = new Queen(ChessColor.Black, Matrix);
            Matrix[0, 4] = new King(ChessColor.Black, Matrix);

            // White Pieces
            for (int j = 0; j < Matrix.GetLength(1); j++)
                Matrix[6, j] = new Pawn(ChessColor.White, Matrix);

            Matrix[7, 0] = new Rook(ChessColor.White, Matrix);
            Matrix[7, 7] = new Rook(ChessColor.White, Matrix);
            Matrix[7, 1] = new Knight(ChessColor.White, Matrix);
            Matrix[7, 6] = new Knight(ChessColor.White, Matrix);
            Matrix[7, 2] = new Bishop(ChessColor.White, Matrix);
            Matrix[7, 5] = new Bishop(ChessColor.White, Matrix);
            Matrix[7, 3] = new Queen(ChessColor.White, Matrix);
            Matrix[7, 4] = new King(ChessColor.White, Matrix);

            // Empty squares
            for (int i = 0; i < Matrix.GetLength(0); i++)
                for (int j = 0; j < Matrix.GetLength(1); j++)
                    if (Matrix[i, j] == null)
                        Matrix[i, j] = new Empty(ChessColor.White, Matrix);
        }
    }

}