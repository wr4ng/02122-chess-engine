using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Chess.Testing
{
	public class Perft : MonoBehaviour
	{
		[Header("Test Settings")]
		[SerializeField] private string fen = FEN.STARTING_POSITION_FEN;
		[SerializeField, Range(1, 4)] private int depth = 2;

		[Header("UI")]
		[SerializeField] private TMP_Text resultText;

		// Stopwatch
		System.Diagnostics.Stopwatch stopwatch;

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.S))
				SingleTest();
			else if (Input.GetKeyDown(KeyCode.D))
				DivideTest();
			else if (Input.GetKeyDown(KeyCode.Escape))
				SceneManager.LoadScene((int)SceneIndex.Menu);
		}

		// Run single perft test (non-divide)
		public void SingleTest()
		{
			Log($"PERFT: Starting single test on {(fen == FEN.STARTING_POSITION_FEN ? "startpos" : fen)}");
			bool success = Board.TryParseFEN(fen, out Board board);
			if (!success)
			{
				Log($"Couldn't parse FEN: {fen}\nCancelling perft test", isError: true);
				return;
			}
			stopwatch = new System.Diagnostics.Stopwatch();
			stopwatch.Start();
			int noOfPositions = board.GetNumberOfPositions(depth);
			stopwatch.Stop();

			Log($"PERFT: Depth={depth} Positions={noOfPositions} Time={stopwatch.ElapsedMilliseconds}ms");
		}

		// Run perft test with divided output for each move from initial board (to compare with stockfish)
		public void DivideTest()
		{
			Log($"PERFT: Starting divide test on {(fen == FEN.STARTING_POSITION_FEN ? "startpos" : fen)}");
			bool success = Board.TryParseFEN(fen, out Board board);
			if (!success)
			{
				Log($"Couldn't parse FEN: {fen}\nCancelling perft test!", isError: true);
				return;
			}
			stopwatch = new System.Diagnostics.Stopwatch();
			stopwatch.Start();
			int total = 0;

			foreach (Move m in board.GetLegalMoves())
			{
				board.PlayMove(m);
				// TODO Handle writing promotion moves (i.e. a7a8r)
				string move = $"{FEN.CoordinateToFEN(m.GetStartSquare())}{FEN.CoordinateToFEN(m.GetEndSquare())}";
				int positions = board.GetNumberOfPositions(depth - 1);
				total += positions;
				Log($"{move}: {positions}");
				board.UndoPreviousMove();
			}

			stopwatch.Stop();
			Log($"Nodes searched: {total}\nTime spent: {stopwatch.ElapsedMilliseconds}ms");
		}

		public void overallTest(){
			String csv = "Fen,Depth,NoOfPositions,Time\n";
			csv += singularTest(FEN.STARTING_POSITION_FEN,3);
			csv += singularTest("r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq - 0 1",3);
			csv += singularTest("8/2p5/3p4/KP5r/1R3p1k/8/4P1P1/8 w - - 0 1",3);
			csv += singularTest("r3k2r/Pppp1ppp/1b3nbN/nP6/BBP1P3/q4N2/Pp1P2PP/R2Q1RK1 w kq - 0 1",3);
			csv += singularTest("rnbq1k1r/pp1Pbppp/2p5/8/2B5/8/PPP1NnPP/RNBQK2R w KQ - 1 8",3);
			csv += singularTest("r4rk1/1pp1qppp/p1np1n2/2b1p1B1/2B1P1b1/P1NP1N2/1PP1QPPP/R4RK1 w - - 0 10",3);
			// csv += singularTest(FEN.STARTING_POSITION_FEN,5);
			// csv += singularTest("r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq - 0 1",5);
			// csv += singularTest("8/2p5/3p4/KP5r/1R3p1k/8/4P1P1/8 w - - 0 1",5);
			// csv += singularTest("r3k2r/Pppp1ppp/1b3nbN/nP6/BBP1P3/q4N2/Pp1P2PP/R2Q1RK1 w kq - 0 1",5);
			// csv += singularTest("rnbq1k1r/pp1Pbppp/2p5/8/2B5/8/PPP1NnPP/RNBQK2R w KQ - 1 8",5);
			// csv += singularTest("r4rk1/1pp1qppp/p1np1n2/2b1p1B1/2B1P1b1/P1NP1N2/1PP1QPPP/R4RK1 w - - 0 10",5);
			Log(csv);
		}

		private String singularTest(String fen,int depth){
			bool success = Board.TryParseFEN(fen, out Board board);

			stopwatch = new System.Diagnostics.Stopwatch();
			stopwatch.Start();
			int noOfPositions = board.GetNumberOfPositions(depth);
			stopwatch.Stop();
			Log($"PERFT:Position={fen} Depth={depth} Positions={noOfPositions} Time={stopwatch.ElapsedMilliseconds}ms");
			String retValue = $"{fen},{depth},{noOfPositions},{stopwatch.ElapsedMilliseconds}\n";
			return retValue;
		}

		// Utility method to log to both Unity console and UI if playing
		private void Log(string message, bool isError = false)
		{
			// Log to console
			if (isError)
				Debug.LogError(message);
			else
				Debug.Log(message);
			// If application is playing, show in UI
			if (Application.isPlaying)
			{
				resultText.text += $"{message}\n";
			}
		}
	}
}