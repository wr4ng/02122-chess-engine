using UnityEngine;

public class Tile : MonoBehaviour
{
	[SerializeField] private SpriteRenderer tileSpriteRenderer;
	[SerializeField] private SpriteRenderer pieceSpriteRenderer;

	private int file, rank;

	public void SetCoordinate(int file, int rank)
	{
		this.file = file;
		this.rank = rank;
	}

	void OnMouseDown()
	{
		BoardUI.Instance.HandleTileClick(file, rank);
	}

	public void SetColor(Color color)
	{
		tileSpriteRenderer.color = color;
	}

	public void SetPieceSprite(Sprite pieceSprite)
	{
		pieceSpriteRenderer.sprite = pieceSprite;
	}

	public void SetPieceSprite(Sprite pieceSprite, Color pieceColor)
	{
		pieceSpriteRenderer.sprite = pieceSprite;
		pieceSpriteRenderer.color = pieceColor;
	}
}