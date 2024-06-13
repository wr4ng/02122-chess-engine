using Chess;

namespace Tests.MoveGeneration;

[TestClass]
public class KnightTests
{

	[TestMethod]
	public void GenerateKnightMove()
	{
		Board board = Board.FromFEN("7k/8/8/8/3N4/8/8/7K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> knightMoves = TestUtil.FilterForPieceType(legalMoves, Piece.Knight, board);
		Assert.AreEqual(8, knightMoves.Count);
	}

	[TestMethod]
	public void GenerateKnightMovesBlocked()
	{
		Board board = Board.FromFEN("k7/8/4N3/5P2/3N4/5R2/8/7K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> knightMoves = TestUtil.FilterForPieceType(legalMoves, Piece.Knight, board);
		Assert.AreEqual(12, knightMoves.Count);
	}

	[TestMethod]
	public void GenerateKnightMovesAttacks()
	{
		Board board = Board.FromFEN("k7/8/4N3/5p2/3N4/5r2/8/7K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> knightMoves = TestUtil.FilterForPieceType(legalMoves, Piece.Knight, board);
		Assert.AreEqual(14, knightMoves.Count);
	}

	[TestMethod]
	public void GenerateKnightMovesCorner()
	{
		Board board = Board.FromFEN("k7/8/4r3/8/8/8/8/N6K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> knightMoves = TestUtil.FilterForPieceType(legalMoves, Piece.Knight, board);
		Assert.AreEqual(2, knightMoves.Count);
	}
}