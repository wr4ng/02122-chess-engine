using UnityEngine;

public class BoardUI : MonoBehaviour
{
	[SerializeField]
	private Color primaryColor;
	[SerializeField]
	private Color secondaryColor;
	[SerializeField]
	private GameObject tilePrefab;

	void Start()
	{
		GenerateBoard();
	}

	void GenerateBoard()
	{
		Vector3 offset = new Vector3(-3.5f, -3.5f, 0);

		for (int file = 0; file < 8; file++)
		{
			for (int rank = 0; rank < 8; rank++)
			{
				// Instantiate tile
				Vector3 tilePosition = new Vector3(rank, file, 0) + offset;
				GameObject tileObject = Instantiate(tilePrefab, tilePosition, Quaternion.identity, transform);

				// Set tile color
				SpriteRenderer spriteRenderer = tileObject.GetComponent<SpriteRenderer>();
				spriteRenderer.color = (file + rank) % 2 != 0 ? primaryColor : secondaryColor;

				// Set tile coordinates
				Tile tile = tileObject.GetComponent<Tile>();
				tile.SetCoordinate(file, rank);
			}
		}
	}
}