using Chess;

namespace Tests.MoveGeneration.InCheck;

[TestClass]
public class QueenCheckTests
{
	[TestMethod]
	public void QueenOneLegalMoveBlock()
	{
		NewBoard board = NewBoard.FromFEN("k7/8/8/8/4b3/8/8/6QK w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> queenMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.Queen, board);
		Assert.AreEqual(1, queenMoves.Count);
	}

	[TestMethod]
	public void QueenOneLegalMoveCapture()
	{
		NewBoard board = NewBoard.FromFEN("k7/8/8/8/4b3/8/8/4Q2K w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> queenMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.Queen, board);
		Assert.AreEqual(1, queenMoves.Count);
	}

	[TestMethod]
	public void QueenNoLegalMove()
	{
		NewBoard board = NewBoard.FromFEN("k7/2Q5/8/8/4b3/8/8/7K w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> queenMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.Queen, board);
		Assert.AreEqual(0, queenMoves.Count);
	}

	[TestMethod]
	public void QueenTwoLegalMoveCaptureAndBlock()
	{
		NewBoard board = NewBoard.FromFEN("k7/8/6Q1/8/4b3/8/8/7K w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> queenMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.Queen, board);
		Assert.AreEqual(2, queenMoves.Count);
	}
}