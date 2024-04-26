using System;
using System.Linq;
using Chess;
using Bot;
using UnityEngine;
using System.Threading;

public class GameManager : MonoBehaviour
{
	// Singleton instance
	public static GameManager Instance { get; private set; }

	// Static parameters set from Menu
	public static string IMPORT_FEN = string.Empty;
	public static bool againstBot = false;
	public static BotType botType = BotType.RandomBot;

	private Board board;
	public Bot.Bot bot;

	private Thread botThread;
	public bool botIsCalculating = false;
	private bool botIsDone = false;
	private Move botMove;

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
				board = Board.FromFEN(IMPORT_FEN);
			}
			catch (Exception exception)
			{
				// TODO Handle error
				Debug.Log(exception);
				board = Board.FromFEN(FEN.STARTING_POSITION_FEN);
			}
		}
		else
		{
			board = Board.FromFEN(FEN.STARTING_POSITION_FEN);
		}
		// Set UI for board
		BoardUI.Instance.UpdateBoard(board);
		// Initialize Bot if against Bot
		if (againstBot)
		{
			bot = botType.CreateBot();
			Debug.Log($"Playing against: {botType}");
		}
	}

	private void Update()
	{
		// Undo moves on LeftArrow
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			board.UndoPreviousMove();
			BoardUI.Instance.UpdateBoard(board);
			// TODO Abort if bot is currently calculating
		}
		// Check if bot is done
		if (botIsCalculating && botIsDone)
		{
			botIsCalculating = false;
			botIsDone = false;
			board.MakeMove(botMove);
			BoardUI.Instance.UpdateBoard(board);
		}
	}

	// Try to perform a move from start to end, returning whether the move was performed or not
	public bool TryMove((int file, int rank) start, (int file, int rank) end)
	{
		var selectedMoves = board.moveGenerator.GenerateMoves().list.Where(move => move.from == start && move.to == end).ToList();
		Debug.Log($"{board.moveGenerator.GenerateMoves().list.Count} {board.moveGenerator.GenerateMoves().moveList.Count}");
		// TODO Handle promotion, since start and end are the same
		if (selectedMoves.Count == 0)
		{
			// TODO Show that move isn't legal to player
			Debug.Log($"No move starting at {FEN.CoordinateToFEN(start)} and ending at {FEN.CoordinateToFEN(end)}");
			return false;
		}
		else
		{
			// Use first move with matching start and end
			// TODO Handle promotion
			board.MakeMove(selectedMoves[0]);
			BoardUI.Instance.UpdateBoard(board);
			// Check if game has ended
			if (board.moveGenerator.GenerateMoves().list.Count == 0)
			{
				// Check if king is in check
				bool kingAttacked = board.moveGenerator.IsAttacked(board.kingSquares[Board.ColorIndex(board.colorToMove)], board.oppositeColor);
				if (kingAttacked)
				{
					// Checkmate
					Debug.Log($"{(board.oppositeColor == Piece.White ? "White" : "Black")} wins!");
				}
				else
				{
					// Stalemate
					Debug.Log("Stalemate!");
				}
				//TODO Show UI of result
			}
			else if (againstBot)
			{
				botThread = new Thread(CalculateNextMove);
				botThread.Start();
			}
			return true;
		}
	}

	public void ClickSquare((int file, int rank) clickedSquare)
	{
		if (botIsCalculating)
			return;
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
			var selectionMoves = board.moveGenerator.GenerateMoves().list.Where(move => move.from == clickedSquare).ToList();
			var selectionHasMoves = selectionMoves.Count() > 0;
			if (selectionHasMoves)
			{
				hasSelection = true;
				selectedSquare = clickedSquare;
				// Update selectedion UI
				BoardUI.Instance.SetHighlightedSquare(clickedSquare);
				// Map each move it it's ending square
				var selectionMoveEnds = selectionMoves.Select(move => move.to).ToList();
				// Show possible moves
				BoardUI.Instance.ShowPossibleMoves(selectionMoveEnds);
			}
		}
	}

	private void CalculateNextMove()
	{
		botIsCalculating = true;
		botMove = bot.GetBestMove(board);
		botIsDone = true;
	}

	private void OnDestroy()
	{
		if (botThread != null && botThread.IsAlive)
		{
			botThread.Abort();
		}
	}
}