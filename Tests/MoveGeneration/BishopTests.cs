using Chess;

namespace Tests.MoveGeneration;

[TestClass]
public class BishopTests
{

	[TestMethod]
	public void GenerateBishopMove()
	{
		Board board = Board.FromFEN("7k/8/8/8/3B4/8/8/7K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> bishopMoves = TestUtil.FilterForPieceType(legalMoves, Piece.Bishop, board);
		Assert.AreEqual(13, bishopMoves.Count);
	}

	[TestMethod]
	public void GenerateBishopMoveBlocked()
	{
		Board board = Board.FromFEN("k7/8/8/8/3B4/2R1R3/8/7K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> bishopMoves = TestUtil.FilterForPieceType(legalMoves, Piece.Bishop, board);
		Assert.AreEqual(7, bishopMoves.Count);
	}

	[TestMethod]
	public void GenerateBishopMoveAttack()
	{
		Board board = Board.FromFEN("k7/8/8/8/3B4/2r1r3/8/7K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> bishopMoves = TestUtil.FilterForPieceType(legalMoves, Piece.Bishop, board);
		Assert.AreEqual(9, bishopMoves.Count);
	}

	[TestMethod]
	public void GenerateBishopMoveAtCorner()
	{
		Board board = Board.FromFEN("k7/8/8/8/8/8/8/B6K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> bishopMoves = TestUtil.FilterForPieceType(legalMoves, Piece.Bishop, board);
		Assert.AreEqual(7, bishopMoves.Count);
	}
}