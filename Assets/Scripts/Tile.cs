using UnityEngine;

public class Tile : MonoBehaviour
{
	int file, rank;

	public void SetCoordinate(int file, int rank)
	{
		this.file = file;
		this.rank = rank;
	}

	void OnMouseDown()
	{
		// TODO Handle tile click
		Debug.Log(Util.CoordinateToString(file, rank));
	}
}