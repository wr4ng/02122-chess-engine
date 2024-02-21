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
				tile.SetCoordinate(file, rank);
				tile.SetColor(GetBaseColor(file, rank));
				tile.SetPieceSprite(null);
				tiles[file, rank] = tile;
			}
		}
	}

	public void UpdateBoard(Board board)
	{
		// Reset highlighted tile on board update
		highlightedTile = null;
		highlightedCoordinates = (-1, -1);

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
	}

	public void HandleTileClick(int file, int rank)
	{
		// Highlight tile when no tile is highlighted
		if (highlightedCoordinates == (-1, -1))
		{
			highlightedTile = tiles[file, rank];
			highlightedTile.SetColor(GetHighlightColor(file, rank));
			highlightedCoordinates = (file, rank);
		}
		// Removing highlighted tile when repressing tile
		else if ((file, rank) == highlightedCoordinates)
		{
			highlightedTile.SetColor(GetBaseColor(file, rank));
			highlightedTile = null;
			highlightedCoordinates = (-1, -1);
		}
		// Another tile was already selected
		else
		{
			// TODO try to complete move
			Debug.Log($"{FEN.CoordinateToFEN(highlightedCoordinates)} - {FEN.CoordinateToFEN(file, rank)}");
			// Reset previously highlighted square
			highlightedTile.SetColor(GetBaseColor(highlightedCoordinates.file, highlightedCoordinates.rank));
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