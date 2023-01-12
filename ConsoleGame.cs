using Engine;
using Pieces;

namespace DotnetChess
{
    public class ConsoleGame : ChessEngine
    {
        public void Run()
        {
            RunEngine();
        }

        public override void SelectPiece()
        {

        }

        public override void MovePiece()
        {

        }

        public override void UpdateBoard()
        {
            ToString();
            Console.ReadLine();
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

                    if (Board[i, j] is not Empty)
                        Console.Write($" {Board[i, j].ToString()} ");
                    else
                        Console.Write("   ");
                }
                Console.WriteLine();
            }
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("    a  b  c  d  e  f  g  h ");
            return string.Empty;
        }
    }
}