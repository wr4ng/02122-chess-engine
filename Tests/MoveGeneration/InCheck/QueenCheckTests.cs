using Chess;

namespace Tests.MoveGeneration.InCheck;

[TestClass]
public class QueenCheckTests
{
	[TestMethod]
	public void QueenOneLegalMoveBlock()
	{
		Board board = Board.FromFEN("k7/8/8/8/4b3/8/8/6QK w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> queenMoves = TestUtil.FilterForPieceType(legalMoves, Piece.Queen, board);
		Assert.AreEqual(1, queenMoves.Count);
	}

	[TestMethod]
	public void QueenOneLegalMoveCapture()
	{
		Board board = Board.FromFEN("k7/8/8/8/4b3/8/8/4Q2K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> queenMoves = TestUtil.FilterForPieceType(legalMoves, Piece.Queen, board);
		Assert.AreEqual(1, queenMoves.Count);
	}

	[TestMethod]
	public void QueenNoLegalMove()
	{
		Board board = Board.FromFEN("k7/2Q5/8/8/4b3/8/8/7K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> queenMoves = TestUtil.FilterForPieceType(legalMoves, Piece.Queen, board);
		Assert.AreEqual(0, queenMoves.Count);
	}

	[TestMethod]
	public void QueenTwoLegalMoveCaptureAndBlock()
	{
		Board board = Board.FromFEN("k7/8/6Q1/8/4b3/8/8/7K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> queenMoves = TestUtil.FilterForPieceType(legalMoves, Piece.Queen, board);
		Assert.AreEqual(2, queenMoves.Count);
	}
}