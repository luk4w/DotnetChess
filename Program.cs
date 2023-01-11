namespace DotnetChess
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ConsoleGame game = new ConsoleGame();
            game.Run();
            
            Console.ReadKey();
        }
    }
}