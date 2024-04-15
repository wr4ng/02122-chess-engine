using Chess;
using UnityEngine;

public class NewBoardTest : MonoBehaviour
{
	public void TestNewBoard()
	{
		Board board = Board.ImportFromFEN(FEN.STARTING_POSITION_FEN);
		NewBoard newboard = NewBoard.FromFEN(FEN.STARTING_POSITION_FEN);

		Debug.Log("NewBoardTest: Testing NewBoard.FromFEN");

		System.Diagnostics.Stopwatch stopwatch = new();

		int depth = 4;

		// Before
		stopwatch.Start();
		int a = board.GetNumberOfPositions(depth);
		stopwatch.Stop();

		Debug.Log($"Board. Depth={depth}. Time={stopwatch.ElapsedMilliseconds}ms");
		Debug.Log(a);

		// After
		stopwatch = new();
		stopwatch.Start();
		int b = newboard.GetNumberOfPositions(depth);
		stopwatch.Stop();

		Debug.Log($"Newboard. Depth={depth}. Time={stopwatch.ElapsedMilliseconds}ms");
		Debug.Log(b);
	}
}
