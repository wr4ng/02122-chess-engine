using Chess;

namespace Tests.MoveGeneration.InCheck;

[TestClass]
public class KnightCheckTests
{
	[TestMethod]
	public void KnightOneLegalMoveBlock()
	{
		NewBoard board = NewBoard.FromFEN("k7/8/8/8/4b3/4N3/8/7K w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> knightMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.Knight, board);
		Assert.AreEqual(1, knightMoves.Count);
	}

	[TestMethod]
	public void KnightOneLegalMoveCapture()
	{
		NewBoard board = NewBoard.FromFEN("k7/8/8/8/4b3/8/5N2/7K w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> knightMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.Knight, board);
		Assert.AreEqual(1, knightMoves.Count);
	}

	[TestMethod]
	public void KnightNoLegalMove()
	{
		NewBoard board = NewBoard.FromFEN("k6N/8/8/8/4b3/8/8/7K w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> knightMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.Knight, board);
		Assert.AreEqual(0, knightMoves.Count);
	}
	[TestMethod]
	public void KnightTwoLegalMoveCaptureAndBlock()
	{
		NewBoard board = NewBoard.FromFEN("k7/8/8/6N1/4b3/8/8/7K w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> knightMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.Knight, board);
		Assert.AreEqual(2, knightMoves.Count);
	}
}