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
			Board board;
			try
			{
				board = Board.FromFEN(fen);
			}
			catch
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
			Board board;
			try
			{
				board = Board.FromFEN(fen);
			}
			catch
			{
				Log($"Couldn't parse FEN: {fen}\nCancelling perft test", isError: true);
				return;
			}
			stopwatch = new System.Diagnostics.Stopwatch();
			stopwatch.Start();
			int total = 0;

			foreach (Move m in board.moveGenerator.GenerateMoves())
			{
				board.MakeMove(m);
				// TODO Handle writing promotion moves (i.e. a7a8r)
				string move = $"{FEN.CoordinateToFEN(m.from)}{FEN.CoordinateToFEN(m.to)}";
				int positions = board.GetNumberOfPositions(depth - 1);
				total += positions;
				Log($"{move}: {positions}");
				board.UndoPreviousMove();
			}

			stopwatch.Stop();
			Log($"Nodes searched: {total}\nTime spent: {stopwatch.ElapsedMilliseconds}ms");
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