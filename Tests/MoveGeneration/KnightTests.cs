using Chess;

namespace Tests.MoveGeneration;

[TestClass]
public class KnightTests
{

	[TestMethod]
	public void GenerateKnightMove()
	{
		NewBoard board = NewBoard.FromFEN("7k/8/8/8/3N4/8/8/7K w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> knightMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.Knight, board);
		Assert.AreEqual(8, knightMoves.Count);
	}

	[TestMethod]
	public void GenerateKnightMovesBlocked()
	{
		NewBoard board = NewBoard.FromFEN("k7/8/4N3/5P2/3N4/5R2/8/7K w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> knightMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.Knight, board);
		Assert.AreEqual(12, knightMoves.Count);
	}

	[TestMethod]
	public void GenerateKnightMovesAttacks()
	{
		NewBoard board = NewBoard.FromFEN("k7/8/4N3/5p2/3N4/5r2/8/7K w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> knightMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.Knight, board);
		Assert.AreEqual(14, knightMoves.Count);
	}

	[TestMethod]
	public void GenerateKnightMovesCorner()
	{
		NewBoard board = NewBoard.FromFEN("k7/8/4r3/8/8/8/8/N6K w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> knightMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.Knight, board);
		Assert.AreEqual(2, knightMoves.Count);
	}
}