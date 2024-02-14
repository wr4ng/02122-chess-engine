using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
	int rank;
	int file;

	public void SetCoordinate(int rank, int file)
	{
		this.rank = rank;
		this.file = file;
	}

	void OnMouseDown()
	{
		// TODO Handle tile click
		Debug.Log(Util.CoordinateToString(rank, file));
	}
}