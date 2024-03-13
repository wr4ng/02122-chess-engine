using System;
using System.Collections.Generic;
using Chess;
using UnityEngine;

public class BoardUI : MonoBehaviour
{
	[Header("Colors")]
	[SerializeField] private UnityEngine.Color primaryColor;
	[SerializeField] private UnityEngine.Color secondaryColor;
	[SerializeField] private UnityEngine.Color primaryHighlightColor;
	[SerializeField] private UnityEngine.Color secondaryHighlightColor;
	[SerializeField] private UnityEngine.Color whiteColor;
	[SerializeField] private UnityEngine.Color blackColor;
	[Header("Prefabs")]
	[SerializeField] private GameObject tilePrefab;
	[Header("Sprites")]
	[SerializeField] private Sprite pawnSprite;
	[SerializeField] private Sprite knighSprite;
	[SerializeField] private Sprite bishopSprite;
	[SerializeField] private Sprite rookSprite;
	[SerializeField] private Sprite queenSprite;
	[SerializeField] private Sprite kingSprite;
	[Header("References")]
	[SerializeField] private SpriteRenderer currentPlayerFrame;

	// Singleton
	public static BoardUI Instance { get; private set; }

	// UI Board
	private Tile[,] tiles;

	// Selection
	private (int file, int rank) highlightedSquare;
	private List<(int file, int rank)> possibleMovesShown;

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
		// Generate board tiles. In Awake since GameManager calls UpdateBoard() in Start()
		GenerateBoard();
	}

	private void GenerateBoard()
	{
		// Initialize no highlighted tile
		highlightedSquare = (-1, -1);

		tiles = new Tile[Board.BOARD_SIZE, Board.BOARD_SIZE];
		Vector3 offset = new Vector3(-3.5f, -3.5f, 0);

		for (int file = 0; file < 8; file++)
		{
			for (int rank = 0; rank < 8; rank++)
			{
				// Instantiate tile and pieceTile
				Vector3 tilePosition = new Vector3(file, rank, 0) + offset;
				GameObject tileGameObject = Instantiate(tilePrefab, tilePosition, Quaternion.identity, transform);

				// Set tile coordinates, color and piece
				Tile tile = tileGameObject.GetComponent<Tile>();
				tile.Initialize(file, rank, GetBaseColor(file, rank), GetHighlightColor(file, rank));
				tiles[file, rank] = tile;
			}
		}
	}

	public void UpdateBoard(Board board)
	{
		for (int file = 0; file < Board.BOARD_SIZE; file++)
		{
			for (int rank = 0; rank < Board.BOARD_SIZE; rank++)
			{
				Piece piece = board.GetPiece(file, rank);
				Tile tile = tiles[file, rank];
				if (piece == null)
				{
					tile.SetPieceSprite(null);
				}
				else
				{
					Sprite pieceSprite = PieceToSprite(piece.GetPieceType());
					var pieceColor = piece.GetColor() == Chess.Color.White ? whiteColor : blackColor;
					tile.SetPieceSprite(pieceSprite, pieceColor);
				}
			}
		}
		// Update current player frame color
		currentPlayerFrame.color = board.GetCurrentPlayer() == Chess.Color.White ? whiteColor : blackColor;
	}

	public void SetHighlightedSquare((int file, int rank) square)
	{
		tiles[square.file, square.rank].SetHighlight(true);
		// If we have a previously highlighed square, reset it
		if (highlightedSquare != (-1, -1))
		{
			tiles[highlightedSquare.file, highlightedSquare.rank].SetHighlight(false);
		}
		highlightedSquare = square;
	}

	public void ClearHighlight()
	{
		tiles[highlightedSquare.file, highlightedSquare.rank].SetHighlight(false);
		highlightedSquare = (-1, -1);
	}

	public void ShowPossibleMoves(List<(int file , int rank)> possibleMoves)
	{
		foreach ((int file, int rank) in possibleMoves)
		{
			tiles[file, rank].SetFrame(true);
		}
		possibleMovesShown = possibleMoves;
	}

	public void ClearPossibleMoves()
	{
		if (possibleMovesShown == null)
		{
			return;
		}
		foreach ((int file, int rank) in possibleMovesShown)
		{
			tiles[file, rank].SetFrame(false);
		}
	}

	private Sprite PieceToSprite(PieceType type)
	{
		return type switch
		{
			PieceType.Pawn => pawnSprite,
			PieceType.Knight => knighSprite,
			PieceType.Bishop => bishopSprite,
			PieceType.Rook => rookSprite,
			PieceType.Queen => queenSprite,
			PieceType.King => kingSprite,
			_ => throw new ArgumentException($"Invalid PieceType: {type}")
		};
	}

	private UnityEngine.Color GetBaseColor(int rank, int file)
	{
		return (file + rank) % 2 != 0 ? primaryColor : secondaryColor;
	}

	private UnityEngine.Color GetHighlightColor(int rank, int file)
	{
		return (file + rank) % 2 != 0 ? primaryHighlightColor : secondaryHighlightColor;
	}
}