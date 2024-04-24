using Chess;

namespace Tests.MoveGeneration;

[TestClass]
public class BishopTests
{

	[TestMethod]
	public void GenerateBishopMove()
	{
		NewBoard board = NewBoard.FromFEN("7k/8/8/8/3B4/8/8/7K w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> bishopMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.Bishop, board);
		Assert.AreEqual(13, bishopMoves.Count);
	}

	[TestMethod]
	public void GenerateBishopMoveBlocked()
	{
		NewBoard board = NewBoard.FromFEN("k7/8/8/8/3B4/2R1R3/8/7K w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> bishopMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.Bishop, board);
		Assert.AreEqual(7, bishopMoves.Count);
	}

	[TestMethod]
	public void GenerateBishopMoveAttack()
	{
		NewBoard board = NewBoard.FromFEN("k7/8/8/8/3B4/2r1r3/8/7K w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> bishopMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.Bishop, board);
		Assert.AreEqual(9, bishopMoves.Count);
	}

	[TestMethod]
	public void GenerateBishopMoveAtCorner()
	{
		NewBoard board = NewBoard.FromFEN("k7/8/8/8/8/8/8/B6K w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> bishopMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.Bishop, board);
		Assert.AreEqual(7, bishopMoves.Count);
	}
}