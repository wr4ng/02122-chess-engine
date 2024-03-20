using System;
using System.Diagnostics;
using Bot;
using Chess;

public static class BotTimeTests
{
    public static void Main()
    {
        // Define the depths to test
        int[] depths = { 1, 2, 3, 4 };

        foreach (int depth in depths)
        {
            Board board = Board.DefaultBoard();
            // Create a new instance of the bot
            tempName bot = new tempName(depth);

            // Start the stopwatch
            Stopwatch stopwatch = Stopwatch.StartNew();

            bot.BestMove(board);
            // Stop the stopwatch and print the elapsed time
            stopwatch.Stop();
            Console.WriteLine($"Depth {depth}: Elapsed time: {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
