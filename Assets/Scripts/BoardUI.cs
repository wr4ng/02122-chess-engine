using System;
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

	private (int file, int rank) highlightedCoordinates;
	private Tile highlightedTile;

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
		highlightedTile = null;
		highlightedCoordinates = (-1, -1);

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

	public void HandleTileClick(int file, int rank)
	{
		// Highlight tile when no tile is highlighted
		if (highlightedCoordinates == (-1, -1))
		{
			highlightedTile = tiles[file, rank];
			highlightedTile.SetHighlight(true);
			highlightedCoordinates = (file, rank);
		}
		// Removing highlighted tile when repressing tile
		else if ((file, rank) == highlightedCoordinates)
		{
			highlightedTile.SetHighlight(false);
			highlightedTile = null;
			highlightedCoordinates = (-1, -1);
		}
		// Another tile was already selected
		else
		{
			// Try to perform move (if selected tiles corresponds to a legal move)
			GameManager.Instance.TryMove(highlightedCoordinates, (file, rank));

			// Reset previously highlighted square
			highlightedTile.SetHighlight(false);
			highlightedTile = null;
			highlightedCoordinates = (-1, -1);
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