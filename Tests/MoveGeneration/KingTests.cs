using Chess;

namespace Tests.MoveGeneration;

[TestClass]
public class KingTests
{

	[TestMethod]
	public void GenerateKingMove()
	{
		Board board = Board.FromFEN("k7/8/8/8/3K4/8/8/8 w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> kingMoves = TestUtil.FilterForPieceType(legalMoves, Piece.King, board);
		Assert.AreEqual(8, kingMoves.Count);
	}

	[TestMethod]
	public void GenerateKingMoveBlocked()
	{
		Board board = Board.FromFEN("k7/8/8/8/3KB3/3N4/8/8 w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> kingMoves = TestUtil.FilterForPieceType(legalMoves, Piece.King, board);
		Assert.AreEqual(6, kingMoves.Count);
	}

	[TestMethod]
	public void GenerateKingMoveAttack()
	{
		Board board = Board.FromFEN("k7/8/8/8/4K3/4pp2/8/8 w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> kingMoves = TestUtil.FilterForPieceType(legalMoves, Piece.King, board);
		Assert.AreEqual(8, kingMoves.Count);
	}

	[TestMethod]
	public void GenerateKingMoveAtCorner()
	{
		Board board = Board.FromFEN("k7/1N6/8/8/8/8/8/7K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> kingMoves = TestUtil.FilterForPieceType(legalMoves, Piece.King, board);
		Assert.AreEqual(3, kingMoves.Count);
	}
}