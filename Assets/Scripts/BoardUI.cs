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
	[SerializeField] private GameObject pieceTilePrefab;
	[Header("Sprites")]
	[SerializeField] private Sprite pawnSprite;
	[SerializeField] private Sprite knighSprite;
	[SerializeField] private Sprite bishopSprite;
	[SerializeField] private Sprite rookSprite;
	[SerializeField] private Sprite queenSprite;
	[SerializeField] private Sprite kingSprite;

	// Singleton
	public static BoardUI instance { get; private set; }

	// UI Board
	private Tile[,] tiles;
	private SpriteRenderer[,] pieceTiles;

	private (int, int) highlightedCoordinates;
	private Tile highlightedTile;

	private void Awake()
	{
		// Singleton setup
		if (instance != null)
		{
			Destroy(this);
		}
		else
		{
			instance = this;
		}
	}

	private void Start()
	{
		GenerateBoard();
		Board b = Board.ImportFromFEN(FEN.STARTING_POSITION_FEN);
		UpdateBoard(b);
	}

	private void GenerateBoard()
	{
		tiles = new Tile[Board.BOARD_SIZE, Board.BOARD_SIZE];
		pieceTiles = new SpriteRenderer[Board.BOARD_SIZE, Board.BOARD_SIZE];
		Vector3 offset = new Vector3(-3.5f, -3.5f, 0);

		for (int file = 0; file < 8; file++)
		{
			for (int rank = 0; rank < 8; rank++)
			{
				// Instantiate tile and pieceTile
				Vector3 tilePosition = new Vector3(file, rank, 0) + offset;
				GameObject tileObject = Instantiate(tilePrefab, tilePosition, Quaternion.identity, transform);
				GameObject pieceTileObject = Instantiate(pieceTilePrefab, tilePosition, Quaternion.identity, transform);

				// Set tile coordinates and color
				Tile tile = tileObject.GetComponent<Tile>();
				tile.SetCoordinate(file, rank);
				tile.SetColor(GetBaseColor(file, rank));
				tiles[file, rank] = tile;

				SpriteRenderer pieceTileRenderer = pieceTileObject.GetComponent<SpriteRenderer>();
				pieceTiles[file, rank] = pieceTileRenderer;
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
				if (piece == null)
				{
					pieceTiles[file, rank].sprite = null;
				}
				else
				{
					pieceTiles[file, rank].sprite = PieceToSprite(piece.GetPieceType());
					pieceTiles[file, rank].color = piece.GetColor() == Chess.Color.White ? whiteColor : blackColor;
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
			highlightedTile.SetColor(GetBaseColor(highlightedCoordinates.Item1, highlightedCoordinates.Item2));
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