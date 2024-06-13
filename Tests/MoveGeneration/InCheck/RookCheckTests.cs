using Chess;

namespace Tests.MoveGeneration.InCheck;

[TestClass]
public class RookCheckTests
{
	[TestMethod]
	public void RookOneLegalMoveBlock()
	{
		Board board = Board.FromFEN("k7/8/8/8/4b3/8/3R4/7K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> rookMoves = TestUtil.FilterForPieceType(legalMoves, Piece.Rook, board);
		Assert.AreEqual(1, rookMoves.Count);
	}

	[TestMethod]
	public void RookOneLegalMoveCapture()
	{
		Board board = Board.FromFEN("k7/8/4R3/8/4b3/8/8/7K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> rookMoves = TestUtil.FilterForPieceType(legalMoves, Piece.Rook, board);
		Assert.AreEqual(1, rookMoves.Count);
	}

	[TestMethod]
	public void RookNoLegalMove()
	{
		Board board = Board.FromFEN("k7/8/3R4/8/4b3/8/8/7K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> rookMoves = TestUtil.FilterForPieceType(legalMoves, Piece.Rook, board);
		Assert.AreEqual(0, rookMoves.Count);
	}

	[TestMethod]
	public void RookTwoLegalMoveCaptureAndBlock()
	{
		Board board = Board.FromFEN("k7/8/8/8/4b3/8/4R3/7K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> rookMoves = TestUtil.FilterForPieceType(legalMoves, Piece.Rook, board);
		Assert.AreEqual(2, rookMoves.Count);
	}
}