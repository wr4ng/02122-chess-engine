using Chess;

namespace Tests.MoveGeneration;

[TestClass]
public class RookTests
{
	[TestMethod]
	public void GenerateRookMove()
	{
		NewBoard board = NewBoard.FromFEN("7k/8/8/8/3R4/8/8/7K w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> rookMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.Rook, board);
		Assert.AreEqual(14, rookMoves.Count);
	}

	[TestMethod]
	public void GenerateRookMoveBlocked()
	{
		NewBoard board = NewBoard.FromFEN("k7/8/8/8/3RP3/3N4/8/7K w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> rookMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.Rook, board);
		Assert.AreEqual(7, rookMoves.Count);
	}

	[TestMethod]
	public void GenerateRookMoveAttack()
	{
		NewBoard board = NewBoard.FromFEN("k7/8/8/8/3Rp3/3n4/8/7K w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> rookMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.Rook, board);
		Assert.AreEqual(9, rookMoves.Count);
	}

	[TestMethod]
	public void GenerateRookMoveAtCorner()
	{
		NewBoard board = NewBoard.FromFEN("k7/8/8/8/4p3/n7/8/R6K w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> rookMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.Rook, board);
		Assert.AreEqual(8, rookMoves.Count);
	}

	//TODO Tests that castlingrights correctly updates
}