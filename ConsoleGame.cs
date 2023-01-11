using Engine;

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

        }


        public override string ToString()
        {
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.DarkBlue;

            for (int i = 0; i < 8; i++)
            {

            }
            
            return string.Empty;
        }

        
    }
}