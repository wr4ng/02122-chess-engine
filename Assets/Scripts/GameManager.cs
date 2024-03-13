using System;
using System.Linq;
using Chess;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static string IMPORT_FEN = string.Empty;
	public static GameManager Instance { get; private set; }

	private Board board;

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
				Debug.Log(exception);
				board = Board.DefaultBoard();
			}
		}
		else
		{
			board = Board.DefaultBoard();
		}
		// Set UI for board
		BoardUI.Instance.UpdateBoard(board);
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

	public void TryMove((int file, int rank) start, (int file, int rank) end)
	{
		var legalMoves = MoveGenerator.GenerateLegalMoves(board);
		// TODO Handle promotion, since start and end are the same½
		var moves = legalMoves.Where(move => move.GetStartSquare() == start && move.GetEndSquare() == end).ToList();
		if (moves.Count == 0)
		{
			// TODO Show error to player
			Debug.Log($"No move starting at {FEN.CoordinateToFEN(start)} and ending at {FEN.CoordinateToFEN(end)}");
		}
		else
		{
			// Use first move with matching start and end
			// TODO Handle promotion
			board.MakeMove(moves[0]);
			BoardUI.Instance.UpdateBoard(board);
		}
		Debug.Log(board.GetCastlingRights().ToString());
	}
}