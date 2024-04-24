using Chess;

namespace Tests.MoveGeneration.InCheck;

[TestClass]
public class KingCheckTests
{
	[TestMethod]
	public void KingOneLegalMoveEscape()
	{
		Board board = Board.FromFEN("k7/8/8/8/4b3/8/8/r6K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> kingMoves = TestUtil.FilterForPieceType(legalMoves, Piece.King, board);
		Assert.AreEqual(1, kingMoves.Count);
	}

	[TestMethod]
	public void KingOneLegalMoveAttack()
	{
		Board board = Board.FromFEN("k6r/8/8/8/8/8/6b1/r6K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> kingMoves = TestUtil.FilterForPieceType(legalMoves, Piece.King, board);
		Assert.AreEqual(1, kingMoves.Count);
	}

	[TestMethod]
	public void KingNoLegalMove()
	{
		Board board = Board.FromFEN("k6r/8/8/8/8/5b2/8/r6K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> kingMoves = TestUtil.FilterForPieceType(legalMoves, Piece.King, board);
		Assert.AreEqual(0, kingMoves.Count);
	}

	[TestMethod]
	public void KingTwoLegalMoveAttackAndEscape()
	{
		Board board = Board.FromFEN("k7/8/8/8/8/8/6b1/r6K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> kingMoves = TestUtil.FilterForPieceType(legalMoves, Piece.King, board);
		Assert.AreEqual(2, kingMoves.Count);
	}
}