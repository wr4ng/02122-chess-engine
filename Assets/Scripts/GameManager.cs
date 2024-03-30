using System;
using System.Linq;
using Chess;
using Bot;
using UnityEngine;
using System.Diagnostics;

public class GameManager : MonoBehaviour
{
	public static string IMPORT_FEN = string.Empty;
	public static GameManager Instance { get; private set; }

	private Board board;
	private Bot.Bot bot = new TempName(2);

	private bool hasSelection;
	private (int file, int rank) selectedSquare = (-1, -1);

	private void Awake()
	{
		// Singleton setup
		if (Instance != null)
		{
			Destroy(this);
		}
		else
		{
			Instance = this;
		}
	}

	private void Start()
	{
		if (IMPORT_FEN != string.Empty)
		{
			try
			{
				board = Board.ImportFromFEN(IMPORT_FEN);
			}
			catch (Exception exception)
			{
				// TODO Handle error
				UnityEngine.Debug.Log(exception);
				board = Board.DefaultBoard();
			}
		}
		else
		{
			board = Board.DefaultBoard();
		}
		// Set UI for board
		BoardUI.Instance.UpdateBoard(board);
		//------------------------this is whack ass test------------------------------
		int[] depths = { 1, 2 };

		foreach (int depth in depths)
		{
			Board tempBoard = Board.ImportFromFEN("r5k1/1q1r1bpp/p3pp2/6P1/2P1PPN1/1P1QR3/2B1K3/8 w - - 0 1");
			// Create a new instance of the bot
			TempName bot = new TempName(depth);

			// Start the stopwatch
			Stopwatch stopwatch = Stopwatch.StartNew();

			bot.GetBestMove(tempBoard);
			// Stop the stopwatch and print the elapsed time
			stopwatch.Stop();
			UnityEngine.Debug.Log($"Depth {depth}: Elapsed time: {stopwatch.ElapsedMilliseconds} ms");
		}
		//------------------------this is whack ass test------------------------------

	}

	private void Update()
	{
		// Undo moves on LeftArrow
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			board.UndoPreviousMove();
			BoardUI.Instance.UpdateBoard(board);
		}
	}

	// Try to perform a move from start to end, returning whether the move was performed or not
	public bool TryMove((int file, int rank) start, (int file, int rank) end)
	{
		var selectedMoves = board.GetLegalMoves().Where(move => move.GetStartSquare() == start && move.GetEndSquare() == end).ToList();
		// TODO Handle promotion, since start and end are the same
		if (selectedMoves.Count == 0)
		{
			// TODO Show that move isn't legal to player
			UnityEngine.Debug.Log($"No move starting at {FEN.CoordinateToFEN(start)} and ending at {FEN.CoordinateToFEN(end)}");
			return false;
		}
		else
		{
			// Use first move with matching start and end
			// TODO Handle promotion
			board.PlayMove(selectedMoves[0]);
			BoardUI.Instance.UpdateBoard(board);
			if (board.gameOver)
			{
				string winner = (board.isDraw) ? "Draw" : board.GetCurrentPlayer().Opposite().ToString();
				UnityEngine.Debug.Log(winner);
			}
			else
			{
				Move botMove = bot.GetBestMove(board);
				board.PlayMove(botMove);
				BoardUI.Instance.UpdateBoard(board);
			}
			return true;
		}
	}

	public void ClickSquare((int file, int rank) clickedSquare)
	{
		if (hasSelection)
		{
			// Check if the same square was re-clicked
			if (selectedSquare == clickedSquare)
			{
				// If true, reset the selection
				hasSelection = false;
				selectedSquare = (-1, -1);
				BoardUI.Instance.ClearHighlight();
				BoardUI.Instance.ClearPossibleMoves();
			}
			else
			{
				// If selected and clicked square is different,
				// try to make a move from the selected square to the clicked square
				bool success = TryMove(selectedSquare, clickedSquare);
				if (success)
				{
					// If the move was performed, reset selection
					hasSelection = false;
					selectedSquare = (-1, -1);
					BoardUI.Instance.ClearHighlight();
					BoardUI.Instance.ClearPossibleMoves();
				}
			}
		}
		else
		{
			// Check if the clicked square has any possible moves before selection it
			var selectionMoves = board.GetLegalMoves().Where(move => move.GetStartSquare() == clickedSquare).ToList();
			var selectionHasMoves = selectionMoves.Count() > 0;
			if (selectionHasMoves)
			{
				hasSelection = true;
				selectedSquare = clickedSquare;
				// Update selectedion UI
				BoardUI.Instance.SetHighlightedSquare(clickedSquare);
				// Map each move it it's ending square
				var selectionMoveEnds = selectionMoves.Select(move => move.GetEndSquare()).ToList();
				// Show possible moves
				BoardUI.Instance.ShowPossibleMoves(selectionMoveEnds);
			}
		}
	}
}