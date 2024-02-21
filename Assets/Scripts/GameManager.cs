using System;
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
		BoardUI.Instance.UpdateBoard(board);
	}
}