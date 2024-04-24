using System.Diagnostics;
using Bot;
using UnityEngine;

namespace Chess.Testing
{
	public class BotTest : MonoBehaviour
	{
		private string fen = "r5k1/1q1r1bpp/p3pp2/6P1/2P1PPN1/1P1QR3/2B1K3/8 w - - 0 1";

		public void TestWhiteBoi()
		{
			for (int depth = 1; depth <= 3; depth++)
			{
				Board board = Board.FromFEN(fen);
				UnityEngine.Debug.Log($"Testing board position: {fen}");

				// Create a new instance of the bot
				WhiteBoi bot = new WhiteBoi(depth);
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