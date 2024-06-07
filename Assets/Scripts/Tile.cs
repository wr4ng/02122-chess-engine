using UnityEngine;

public class Tile : MonoBehaviour
{
	[SerializeField] private SpriteRenderer tileSpriteRenderer;
	[SerializeField] private SpriteRenderer frameSpriteRenderer;
	[SerializeField] private SpriteRenderer pieceSpriteRenderer;

	private int file, rank;
	private bool isHighlighted = false;
	private Color baseColor;
	private Color highlightColor;

	public void Initialize(int file, int rank, Color baseColor, Color highlightColor)
	{
		this.file = file;
		this.rank = rank;
		this.baseColor = baseColor;
		this.highlightColor = highlightColor;

		tileSpriteRenderer.color = baseColor;
		frameSpriteRenderer.color = highlightColor;
		pieceSpriteRenderer.sprite = null;
	}

	private void OnMouseDown()
	{
		// Notify GameManager that the Square was clicked on
		GameManager.Instance.ClickSquare((file, rank));
	}

	private void OnMouseEnter()
	{
		if (GameManager.Instance.gameEnded) return;
		// Highlight tile
		tileSpriteRenderer.color = highlightColor;
	}

	private void OnMouseExit()
	{
		if (GameManager.Instance.gameEnded) return;
		// Reset tile highlight
		if (!isHighlighted)
		{
			tileSpriteRenderer.color = baseColor;
		}
	}

	public void SetHighlight(bool highlight)
	{
		if (GameManager.Instance.gameEnded) return;
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

	public void SetFrame(bool setFrame)
	{
		frameSpriteRenderer.enabled = setFrame;
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