using System.Diagnostics;
using Bot;
using UnityEngine;

namespace Chess.Testing
{
	public class BotTest : MonoBehaviour
	{
		private string fen = "rn2k2r/p1p1qppp/bp3n2/3pp1b1/3PN3/P1PQ1P2/1P1NP1PP/R1BB1RK1 w KQkq - 0 1";

		public void TestWhiteBoi()
		{
			for (int depth = 1; depth <= 7; depth++)
			{
				Board board = Board.FromFEN(fen);
				UnityEngine.Debug.Log($"Testing board position: {fen}");

				// Create a new instance of the bot
				WhiteBoi bot = new WhiteBoi(depth, board);
				// Start the stopwatch
				Stopwatch stopwatch = Stopwatch.StartNew();
				bot.GetBestMove(board);
				// Stop the stopwatch and print the elapsed time
				stopwatch.Stop();
				UnityEngine.Debug.Log($"Depth {depth}: Elapsed time: {stopwatch.ElapsedMilliseconds} ms");
			}
		}
	}
}