using System.Diagnostics;
using Bot;
using UnityEngine;

namespace Chess.Testing
{
	public class BotTest : MonoBehaviour
	{
		private string[] fens = {
		"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", 
		"r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq - 0 1",
		"8/2p5/3p4/KP5r/1R3p1k/8/4P1P1/8 w - - 0 1",
		"r3k2r/Pppp1ppp/1b3nbN/nP6/BBP1P3/q4N2/Pp1P2PP/R2Q1RK1 w kq - 0 1",
		"rnbq1k1r/pp1Pbppp/2p5/8/2B5/8/PPP1NnPP/RNBQK2R w KQ - 1 8",
		"r4rk1/1pp1qppp/p1np1n2/2b1p1B1/2B1P1b1/P1NP1N2/1PP1QPPP/R4RK1 w - - 0 10"};

		public void TestWhiteBoi()
		{
			string title = "Fen,Depth,Calls,Time\n";
			string csv = "";
			for (int depth = 2; depth <= 4; depth++)
			{
				csv += "non Pruned: " + depth + "\n";
				csv += title;
				csv += test(depth);	
			}
			for (int depth = 2; depth <= 4; depth++)
			{
				csv += "Pruned: " + depth + "\n";
				csv += title;
				csv += test2(depth);	
			}
			for (int depth = 2; depth <= 4; depth++)
			{
				csv += "Pruned + attack prio: " + depth + "\n";
				csv += title;
				csv += test3(depth);	
			}
			UnityEngine.Debug.Log(csv);
		}
		private string test(int depth){
			string csv = "";
			for(int i = 0; i < fens.Length; i++){
				Board board = Board.FromFEN(fens[i]);
				Evaluation.calls = 0;
				// Create a new instance of the bot
				WhiteBoi bot = new WhiteBoi(depth);
				// Start the stopwatch
				Stopwatch stopwatch = Stopwatch.StartNew();
				bot.GetBestNonPruned(board);
				// Stop the stopwatch and print the elapsed time
				stopwatch.Stop();
				csv += fens[i] + $",{depth},{stopwatch.ElapsedMilliseconds},{Evaluation.calls}\n";
			}

			return csv;
		}
		private string test2(int depth){
			string csv = "";
			for(int i = 0; i < fens.Length; i++){
				Board board = Board.FromFEN(fens[i]);
				Evaluation.calls = 0;
				// Create a new instance of the bot
				WhiteBoi bot = new WhiteBoi(depth);
				// Start the stopwatch
				Stopwatch stopwatch = Stopwatch.StartNew();
				bot.GetBestNoPrio(board);
				// Stop the stopwatch and print the elapsed time
				stopwatch.Stop();
				csv += fens[i] + $",{depth},{stopwatch.ElapsedMilliseconds},{Evaluation.calls}\n";
			}

			return csv;
		}
		private string test3(int depth){
			string csv = "";
			for(int i = 0; i < fens.Length; i++){
				Board board = Board.FromFEN(fens[i]);
				Evaluation.calls = 0;
				// Create a new instance of the bot
				WhiteBoi bot = new WhiteBoi(depth);
				// Start the stopwatch
				Stopwatch stopwatch = Stopwatch.StartNew();
				bot.GetBestMove(board);
				// Stop the stopwatch and print the elapsed time
				stopwatch.Stop();
				csv += fens[i] + $",{depth},{stopwatch.ElapsedMilliseconds},{Evaluation.calls}\n";
			}

			return csv;
		}
	}
}