using Chess;

namespace Tests.MoveGeneration.InCheck;

[TestClass]
public class KingCheckTests
{
	[TestMethod]
	public void KingOneLegalMoveEscape()
	{
		NewBoard board = NewBoard.FromFEN("k7/8/8/8/4b3/8/8/r6K w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> kingMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.King, board);
		Assert.AreEqual(1, kingMoves.Count);
	}

	[TestMethod]
	public void KingOneLegalMoveAttack()
	{
		NewBoard board = NewBoard.FromFEN("k6r/8/8/8/8/8/6b1/r6K w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> kingMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.King, board);
		Assert.AreEqual(1, kingMoves.Count);
	}

	[TestMethod]
	public void KingNoLegalMove()
	{
		NewBoard board = NewBoard.FromFEN("k6r/8/8/8/8/5b2/8/r6K w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> kingMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.King, board);
		Assert.AreEqual(0, kingMoves.Count);
	}

	[TestMethod]
	public void KingTwoLegalMoveAttackAndEscape()
	{
		NewBoard board = NewBoard.FromFEN("k7/8/8/8/8/8/6b1/r6K w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> kingMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.King, board);
		Assert.AreEqual(2, kingMoves.Count);
	}
}