using Chess;

namespace Tests.MoveGeneration;

[TestClass]
public class QueenTests
{
	[TestMethod]
	public void GenerateQueenMove()
	{
		Board board = Board.FromFEN("k7/8/8/8/3Q4/8/8/7K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> queenMoves = TestUtil.FilterForPieceType(legalMoves, Piece.Queen, board);
		Assert.AreEqual(27, queenMoves.Count);
	}
	[TestMethod]
	public void GenerateQueenMoveBlocked()
	{
		Board board = Board.FromFEN("k7/8/8/4N3/3Q4/3R4/8/7K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> queenMoves = TestUtil.FilterForPieceType(legalMoves, Piece.Queen, board);
		Assert.AreEqual(20, queenMoves.Count);
	}
	[TestMethod]
	public void GenerateQueenMoveAttack()
	{
		Board board = Board.FromFEN("k7/8/8/4n3/3Q4/3r4/8/7K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> queenMoves = TestUtil.FilterForPieceType(legalMoves, Piece.Queen, board);
		Assert.AreEqual(22, queenMoves.Count);
	}
	[TestMethod]
	public void GenerateQueenMoveAtCorner()
	{
		Board board = Board.FromFEN("k7/r7/8/8/8/8/8/Q6K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> queenMoves = TestUtil.FilterForPieceType(legalMoves, Piece.Queen, board);
		Assert.AreEqual(19, queenMoves.Count);
	}
}