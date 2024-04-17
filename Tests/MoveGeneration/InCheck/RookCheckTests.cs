using Chess;

namespace Tests.MoveGeneration.InCheck;

[TestClass]
public class RookCheckTests
{
	[TestMethod]
	public void RookOneLegalMoveBlock()
	{
		NewBoard board = NewBoard.FromFEN("k7/8/8/8/4b3/8/3R4/7K w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> rookMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.Rook, board);
		Assert.AreEqual(1, rookMoves.Count);
	}

	[TestMethod]
	public void RookOneLegalMoveCapture()
	{
		NewBoard board = NewBoard.FromFEN("k7/8/4R3/8/4b3/8/8/7K w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> rookMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.Rook, board);
		Assert.AreEqual(1, rookMoves.Count);
	}

	[TestMethod]
	public void RookNoLegalMove()
	{
		NewBoard board = NewBoard.FromFEN("k7/8/3R4/8/4b3/8/8/7K w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> rookMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.Rook, board);
		Assert.AreEqual(0, rookMoves.Count);
	}

	[TestMethod]
	public void RookTwoLegalMoveCaptureAndBlock()
	{
		NewBoard board = NewBoard.FromFEN("k7/8/8/8/4b3/8/4R3/7K w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> rookMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.Rook, board);
		Assert.AreEqual(2, rookMoves.Count);
	}
}