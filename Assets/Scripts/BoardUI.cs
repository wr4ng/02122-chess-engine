using System;
using Chess;
using UnityEngine;

public class BoardUI : MonoBehaviour
{
	[Header("Colors")]
	[SerializeField] private UnityEngine.Color primaryColor;
	[SerializeField] private UnityEngine.Color secondaryColor;
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

	private Tile[,] tiles;
	private SpriteRenderer[,] pieceTiles;

	void Start()
	{
		GenerateBoard();
		Board b = Board.ImportFromFEN(FEN.STARTING_POSITION_FEN);
		UpdateBoard(b);
	}

	void GenerateBoard()
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

				// Set tile color
				SpriteRenderer spriteRenderer = tileObject.GetComponent<SpriteRenderer>();
				spriteRenderer.color = (file + rank) % 2 != 0 ? primaryColor : secondaryColor;

				// Set tile coordinates
				Tile tile = tileObject.GetComponent<Tile>();
				tile.SetCoordinate(file, rank);
				tiles[file, rank] = tile;

				SpriteRenderer pieceTileRenderer = pieceTileObject.GetComponent<SpriteRenderer>();
				pieceTiles[file, rank] = pieceTileRenderer;
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
}