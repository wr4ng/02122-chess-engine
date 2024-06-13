using Chess;

namespace Tests.MoveGeneration.InCheck;

[TestClass]
public class KnightCheckTests
{
	[TestMethod]
	public void KnightOneLegalMoveBlock()
	{
		Board board = Board.FromFEN("k7/8/8/8/4b3/4N3/8/7K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> knightMoves = TestUtil.FilterForPieceType(legalMoves, Piece.Knight, board);
		Assert.AreEqual(1, knightMoves.Count);
	}

	[TestMethod]
	public void KnightOneLegalMoveCapture()
	{
		Board board = Board.FromFEN("k7/8/8/8/4b3/8/5N2/7K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> knightMoves = TestUtil.FilterForPieceType(legalMoves, Piece.Knight, board);
		Assert.AreEqual(1, knightMoves.Count);
	}

	[TestMethod]
	public void KnightNoLegalMove()
	{
		Board board = Board.FromFEN("k6N/8/8/8/4b3/8/8/7K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> knightMoves = TestUtil.FilterForPieceType(legalMoves, Piece.Knight, board);
		Assert.AreEqual(0, knightMoves.Count);
	}
	[TestMethod]
	public void KnightTwoLegalMoveCaptureAndBlock()
	{
		Board board = Board.FromFEN("k7/8/8/6N1/4b3/8/8/7K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> knightMoves = TestUtil.FilterForPieceType(legalMoves, Piece.Knight, board);
		Assert.AreEqual(2, knightMoves.Count);
	}
}