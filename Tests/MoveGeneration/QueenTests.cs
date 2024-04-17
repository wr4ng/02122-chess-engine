using Chess;

namespace Tests.MoveGeneration;

[TestClass]
public class QueenTests
{
	[TestMethod]
	public void GenerateQueenMove()
	{
		NewBoard board = NewBoard.FromFEN("k7/8/8/8/3Q4/8/8/7K w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> queenMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.Queen, board);
		Assert.AreEqual(27, queenMoves.Count);
	}
	[TestMethod]
	public void GenerateQueenMoveBlocked()
	{
		NewBoard board = NewBoard.FromFEN("k7/8/8/4N3/3Q4/3R4/8/7K w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> queenMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.Queen, board);
		Assert.AreEqual(20, queenMoves.Count);
	}
	[TestMethod]
	public void GenerateQueenMoveAttack()
	{
		NewBoard board = NewBoard.FromFEN("k7/8/8/4n3/3Q4/3r4/8/7K w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> queenMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.Queen, board);
		Assert.AreEqual(22, queenMoves.Count);
	}
	[TestMethod]
	public void GenerateQueenMoveAtCorner()
	{
		NewBoard board = NewBoard.FromFEN("k7/r7/8/8/8/8/8/Q6K w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> queenMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.Queen, board);
		Assert.AreEqual(19, queenMoves.Count);
	}
}