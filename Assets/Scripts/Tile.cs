using UnityEngine;

public class Tile : MonoBehaviour
{
	[SerializeField] private SpriteRenderer tileSpriteRenderer;
	private int file, rank;

	public void SetCoordinate(int file, int rank)
	{
		this.file = file;
		this.rank = rank;
	}

	public void SetColor(UnityEngine.Color color)
	{
		tileSpriteRenderer.color = color;
	}

	void OnMouseDown()
	{
		BoardUI.instance.HandleTileClick(file, rank);
	}
}