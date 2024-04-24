using Chess;

namespace Tests.MoveGeneration.InCheck;

[TestClass]
public class BishopCheckTests
{
	[TestMethod]
	public void BishopOneLegalMoveBlock()
	{
		NewBoard board = NewBoard.FromFEN("k7/8/8/8/4b3/8/4B3/7K w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> bishopMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.Bishop, board);
		Assert.AreEqual(1, bishopMoves.Count);
	}

	[TestMethod]
	public void BishopOneLegalMoveCapture()
	{
		NewBoard board = NewBoard.FromFEN("k7/8/8/3B4/4b3/8/8/7K w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> bishopMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.Bishop, board);
		Assert.AreEqual(1, bishopMoves.Count);
	}

	[TestMethod]
	public void BishopNoLegalMove()
	{
		NewBoard board = NewBoard.FromFEN("k7/8/8/8/4b3/1B6/8/7K w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> bishopMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.Bishop, board);
		Assert.AreEqual(0, bishopMoves.Count);
	}

	[TestMethod]
	public void BishopTwoLegalMoveCaptureAndBlock()
	{
		NewBoard board = NewBoard.FromFEN("k7/8/8/8/4b3/8/6B1/7K w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> bishopMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.Bishop, board);
		Assert.AreEqual(2, bishopMoves.Count);
	}
}