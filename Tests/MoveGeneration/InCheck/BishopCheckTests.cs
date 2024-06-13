using Chess;

namespace Tests.MoveGeneration.InCheck;

[TestClass]
public class BishopCheckTests
{
	[TestMethod]
	public void BishopOneLegalMoveBlock()
	{
		Board board = Board.FromFEN("k7/8/8/8/4b3/8/4B3/7K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> bishopMoves = TestUtil.FilterForPieceType(legalMoves, Piece.Bishop, board);
		Assert.AreEqual(1, bishopMoves.Count);
	}

	[TestMethod]
	public void BishopOneLegalMoveCapture()
	{
		Board board = Board.FromFEN("k7/8/8/3B4/4b3/8/8/7K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> bishopMoves = TestUtil.FilterForPieceType(legalMoves, Piece.Bishop, board);
		Assert.AreEqual(1, bishopMoves.Count);
	}

	[TestMethod]
	public void BishopNoLegalMove()
	{
		Board board = Board.FromFEN("k7/8/8/8/4b3/1B6/8/7K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> bishopMoves = TestUtil.FilterForPieceType(legalMoves, Piece.Bishop, board);
		Assert.AreEqual(0, bishopMoves.Count);
	}

	[TestMethod]
	public void BishopTwoLegalMoveCaptureAndBlock()
	{
		Board board = Board.FromFEN("k7/8/8/8/4b3/8/6B1/7K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> bishopMoves = TestUtil.FilterForPieceType(legalMoves, Piece.Bishop, board);
		Assert.AreEqual(2, bishopMoves.Count);
	}
}