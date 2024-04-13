using Chess;
using UnityEngine;

public class NewBoardTest : MonoBehaviour
{
	public void TestNewBoard()
	{
		NewBoard board = NewBoard.FromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
		// NewBoard board = NewBoard.FromFEN("4k2r/8/8/q7/8/8/8/4K3 w k - 0 1");
		// NewBoard board = NewBoard.FromFEN("r1b1k1nr/ppp1pppp/8/3n3B/1q1p4/2K5/PPPQP1PP/RNB1b1NR w kq - 0 1");
		// board = NewBoard.FromFEN("r1b1k1nr/ppp1pppp/5n2/7B/1q1p4/2K5/PPPQP1PP/RNB1b1NR w kq - 0 1");
		// board = NewBoard.FromFEN("4kr2/8/8/q7/8/8/8/4K3 w - - 0 1");
		board = NewBoard.FromFEN("1nbqkbnr/pppppppp/8/8/8/2PPPP2/PP4PP/r2QKB2 w k - 0 1");

		Debug.Log("NewBoardTest: Testing NewBoard.FromFEN");
		Debug.Log(board);
		// Debug.Log($"White king: {board.kingSquares[0]}");
		// Debug.Log($"Black king: {board.kingSquares[1]}");

		var moves = board.moveGenerator.GenerateMoves();

		UnityEngine.Debug.Log($"Number of checks: {board.moveGenerator.checkers.Count}");
		foreach (var checker in board.moveGenerator.checkers)
		{
			UnityEngine.Debug.Log($"Checked by: {checker}");
		}
		UnityEngine.Debug.Log($"Number of pins: {board.moveGenerator.pinned.Count}");
		foreach (var pinned in board.moveGenerator.pinned)
		{
			UnityEngine.Debug.Log($"Pinned : {pinned}");
		}

		Debug.Log("Moves:");
		foreach (var move in moves)
		{
			Debug.Log($"{NewPiece.Type(board.squares[move.from.file, move.from.rank])}: {move}");
		}
		Debug.Log(moves.Count);
	}
}
