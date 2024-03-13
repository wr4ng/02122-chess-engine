using UnityEngine;

public class Tile : MonoBehaviour
{
	[SerializeField] private SpriteRenderer tileSpriteRenderer;
	[SerializeField] private SpriteRenderer pieceSpriteRenderer;

	private int file, rank;
	private bool isHighlighted;
	private Color baseColor;
	private Color highlightColor;

	public void Initialize(int file, int rank, Color baseColor, Color highlightColor)
	{
		this.file = file;
		this.rank = rank;
		this.baseColor = baseColor;
		this.highlightColor = highlightColor;

		isHighlighted = false;
		tileSpriteRenderer.color = baseColor;
		pieceSpriteRenderer.sprite = null;
	}

	private void OnMouseDown()
	{
		// Notify GameManager that the Square was clicked on
		GameManager.Instance.ClickSquare((file, rank));
	}

	private void OnMouseEnter()
	{
		// Highlight tile
		tileSpriteRenderer.color = highlightColor;
	}

	private void OnMouseExit()
	{
		// Reset tile highlight
		if (!isHighlighted)
		{
			tileSpriteRenderer.color = baseColor;
		}
	}

	public void SetHighlight(bool highlight)
	{
		if (highlight)
		{
			tileSpriteRenderer.color = highlightColor;
		}
		else
		{
			tileSpriteRenderer.color = baseColor;
		}
		isHighlighted = highlight;
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