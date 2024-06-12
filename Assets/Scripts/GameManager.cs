using System;
using System.Linq;
using System.Collections.Generic;
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
	public static string IMPORT_PGN = string.Empty;
	public static bool againstBot = false;
	public static BotType botType = BotType.RandomBot;
	public static int botDepth = 5;

	private Board board;
	public Bot.Bot bot;

	private Thread botThread;
	public bool botIsCalculating = false;
	private bool botIsDone = false;
	private Move botMove;

	private bool hasSelection;
	private (int file, int rank) selectedSquare = (-1, -1);

	public int selectedPromotionType = Piece.Queen;

	public bool gameEnded = false;

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
			catch
			{
				// Default to using standard starting position if IMPORT_FEN yielded an error
				board = Board.FromFEN(FEN.STARTING_POSITION_FEN);
			}
		} else if (IMPORT_PGN != string.Empty)
		{
			try
			{
				board = Board.FromPGN(IMPORT_PGN);
			}
			catch (Exception e)
			{
				UnityEngine.Debug.Log(e.Message);
				// Default to using standard starting position if IMPORT_PGN yielded an error
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
			bot = botType.CreateBot(botDepth,board);
		}
	}

	private void Update()
	{
		if (gameEnded) return;
		// Undo moves on LeftArrow
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			// Abort Bot thread if currently calculating
			if (botIsCalculating)
			{
				botThread.Abort();
				botIsCalculating = false;
				botIsDone = false;
			}
			ClearSelection();
			board.UndoPreviousMove();
			board.PopAlgNotation();
			bot.UpdateOp();
			BoardUI.Instance.UpdateBoard(board);
		}
		// Print the pgn of the game on space
		if (Input.GetKeyDown(KeyCode.Space))
		{
			UnityEngine.Debug.Log("PGN of the game:");
			UnityEngine.Debug.Log(PGN.PrettyPgn(board.pgnMoves));
		}
		// Check if bot is done
		if (botIsCalculating && botIsDone)
		{
			botIsCalculating = false;
			botIsDone = false;
			board.MakeMove(botMove);
			board.AddAlgNotation(botMove);
			BoardUI.Instance.UpdateBoard(board);
			// Check game status
			CheckGameDone();
		}

		UpdateSelectedPromotion();
	}

	private void UpdateSelectedPromotion()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			selectedPromotionType = Piece.Queen;
			InGameUI.Instance.SetPromotionSprite(Piece.Queen);
		}
		else if (Input.GetKeyDown(KeyCode.R))
		{
			selectedPromotionType = Piece.Rook;
			InGameUI.Instance.SetPromotionSprite(Piece.Rook);
		}
		else if (Input.GetKeyDown(KeyCode.N))
		{
			selectedPromotionType = Piece.Knight;
			InGameUI.Instance.SetPromotionSprite(Piece.Knight);
		}
		else if (Input.GetKeyDown(KeyCode.B))
		{
			selectedPromotionType = Piece.Bishop;
			InGameUI.Instance.SetPromotionSprite(Piece.Bishop);
		}
	}

	// Try to perform a move from start to end, returning whether the move was performed or not
	public bool TryMove((int file, int rank) start, (int file, int rank) end)
	{
		List<Move> selectedMoves = board.moveGenerator.GenerateMoves().Where(move => move.from == start && move.to == end).ToList();

		if (selectedMoves.Count == 0)
		{
			// No moves matching start -> end. Keep highlight but don't perform any moves
			return false;
		}
		else
		{

			if (selectedMoves.Count == 1)
			{
				board.MakeMove(selectedMoves[0]);
				board.AddAlgNotation(selectedMoves[0]);
			}
			else // selectedMoves.Count > 1. Handle promotion
			{
				try
				{
					Move promotionMove = selectedMoves.Find(move => move.promotionType == selectedPromotionType);
					board.MakeMove(promotionMove);
					board.AddAlgNotation(promotionMove);
				}
				catch (InvalidOperationException)
				{
					// Shouldn't happen! If it does, selectedPromotionType is probably invalid
					Debug.LogError($"No promotion move with selected promotion type: {selectedPromotionType}");
				}
			}
			BoardUI.Instance.UpdateBoard(board);
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
				ClearSelection();
			}
			else
			{
				// If selected and clicked square is different,
				// try to make a move from the selected square to the clicked square
				bool success = TryMove(selectedSquare, clickedSquare);
				if (success)
				{
					// If the move was performed, reset selection
					ClearSelection();

					// Check if game has ended
					bool done = CheckGameDone();
					if (!done && againstBot)
					{
						// Begin calculating bot move if game is still in progress
						botThread = new Thread(CalculateNextMove);
						botThread.Start();
					}
				}
			}
		}
		else
		{
			// Check if the clicked square has any possible moves before selection it
			var selectionMoves = board.moveGenerator.GenerateMoves().Where(move => move.from == clickedSquare).ToList();
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

	private void ClearSelection()
	{
		hasSelection = false;
		selectedSquare = (-1, -1);
		BoardUI.Instance.ClearHighlight();
		BoardUI.Instance.ClearPossibleMoves();
	}

	private bool CheckGameDone()
	{
		// Check for draw by repetition / 50-move-rule
		if (board.drawState == Board.DrawState.Repetition)
		{
			InGameUI.Instance.EndGame("Draw by repetition!");
			gameEnded = true;
			return true;
		}
		if (board.drawState == Board.DrawState.FiftyMoveRule)
		{
			InGameUI.Instance.EndGame("Draw by 50 move rule!");
			gameEnded = true;
			return true;
		}
		// Check if there are available moves left
		if (board.moveGenerator.GenerateMoves().Count > 0)
			return false;
		// Otherwise, check if checkmate or stalemate
		gameEnded = true;
		bool kingAttacked = board.moveGenerator.IsAttacked(board.kingSquares[Piece.ColorIndex(board.colorToMove)], board.oppositeColor);
		if (kingAttacked)
			InGameUI.Instance.EndGame($"{(board.oppositeColor == Piece.White ? "White" : "Black")} wins!");
		else
			InGameUI.Instance.EndGame("Stalemate!");

		return true;
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