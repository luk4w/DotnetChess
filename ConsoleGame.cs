using Source;
using Enums;
using Exceptions;
using Pieces;

namespace DotnetChess
{
    public class ConsoleGame : Chess
    {
        public void Run()
        {
            StartGame();
        }

        public override Position SelectInput(ChessColor playerTurnColor)
        {
            Console.Write($"{playerTurnColor}; Choose a valid piece: ");
            string? choice = Console.ReadLine();
            if (!String.IsNullOrEmpty(choice))
                return StringToPosition(choice);
            throw new Exception("Null position!");
        }

        public override Position MoveInput()
        {
            Console.Write("Move to: ");
            string? choice = Console.ReadLine();
            if (!String.IsNullOrEmpty(choice))
                return StringToPosition(choice);
            throw new Exception("Null position!");
        }

        public override void ShowLegalMoves(bool[,] legalMoves)
        {
            Console.Clear();
            PrintLegalMoves(legalMoves);
        }

        public override void UpdateBoard()
        {
            Console.Clear();
            ToString();
        }

        public Position StringToPosition(string value)
        {
            char[] column = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };
            if (value.Length != 2)
                throw new ChessException("Invalid position!");
            else if (int.Parse(value[1].ToString()) < 0 || int.Parse(value[1].ToString()) > 8)
                throw new ChessException("Invalid row!");
            else if (!column.Contains(value[0]))
                throw new ChessException("Invalid column!");

            int x = 8 - int.Parse(value[1].ToString());
            int y = Array.IndexOf(column, value[0]);

            return new Position(x, y);
        }

        public override char Promote()
        {
            char[] pieces = { 'N', 'B', 'R', 'Q' };
            string? value = null;
            while (string.IsNullOrEmpty(value) || value.Length != 1)
            {
                Console.Write("Promote to: (N, B, R, Q)");
                value = Console.ReadLine();
                if (value == null)
                {
                    Console.WriteLine($"Null value!");
                    continue;
                }
                else if (!pieces.Contains(value[0])) 
                {
                    Console.WriteLine($"Invalid piece value: {value}");
                    continue;
                }
                return value[0];
            }
            throw new ChessException("Invalid promotion!");
        }

        private void PrintLegalMoves(bool[,] availableMoves)
        {
            for (int i = 0; i < 8; i++)
            {
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.Write($" {8 - i} ");
                for (int j = 0; j < 8; j++)
                {
                    if (i % 2 == 0 && j % 2 != 0 || i % 2 != 0 && j % 2 == 0)
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                    else
                        Console.BackgroundColor = ConsoleColor.DarkGray;

                    if (availableMoves[i, j] == true)
                    {
                        if (Board.GetPiece(i, j) is Empty)
                            Console.BackgroundColor = ConsoleColor.DarkGreen;
                        else
                            Console.BackgroundColor = ConsoleColor.Red;
                    }

                    if (Board.GetPiece(i, j) is not Empty)
                        if (Board.GetPiece(i, j).Color == ChessColor.White)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write($" {Board.GetPiece(i, j).ToString()} ");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write($" {Board.GetPiece(i, j).ToString()} ");
                        }
                    else
                        Console.Write("   ");

                }
                Console.WriteLine();
            }
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("    a  b  c  d  e  f  g  h ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }

        public override string ToString()
        {
            for (int i = 0; i < 8; i++)
            {
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.Write($" {8 - i} ");
                for (int j = 0; j < 8; j++)
                {
                    if (i % 2 == 0 && j % 2 != 0 || i % 2 != 0 && j % 2 == 0)
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                    else
                        Console.BackgroundColor = ConsoleColor.DarkGray;

                    if (Board.GetPiece(i, j) is not Empty)
                        if (Board.GetPiece(i, j).Color == ChessColor.White)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write($" {Board.GetPiece(i, j).ToString()} ");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write($" {Board.GetPiece(i, j).ToString()} ");
                        }
                    else
                        Console.Write("   ");

                }
                Console.WriteLine();
            }
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("    a  b  c  d  e  f  g  h ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            return string.Empty;
        }
    }
}