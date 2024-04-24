using Chess;

namespace Tests.MoveGeneration;

[TestClass]
public class KingTests
{

	[TestMethod]
	public void GenerateKingMove()
	{
		NewBoard board = NewBoard.FromFEN("k7/8/8/8/3K4/8/8/8 w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> kingMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.King, board);
		Assert.AreEqual(8, kingMoves.Count);
	}

	[TestMethod]
	public void GenerateKingMoveBlocked()
	{
		NewBoard board = NewBoard.FromFEN("k7/8/8/8/3KB3/3N4/8/8 w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> kingMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.King, board);
		Assert.AreEqual(6, kingMoves.Count);
	}

	[TestMethod]
	public void GenerateKingMoveAttack()
	{
		NewBoard board = NewBoard.FromFEN("k7/8/8/8/4K3/4pp2/8/8 w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> kingMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.King, board);
		Assert.AreEqual(8, kingMoves.Count);
	}

	[TestMethod]
	public void GenerateKingMoveAtCorner()
	{
		NewBoard board = NewBoard.FromFEN("k7/1N6/8/8/8/8/8/7K w - - 0 1");
		List<NewMove> legalMoves = board.moveGenerator.GenerateMoves();
		List<NewMove> kingMoves = TestUtil.FilterForPieceType(legalMoves, NewPiece.King, board);
		Assert.AreEqual(3, kingMoves.Count);
	}

	//TODO Tests that castlingrights correctly updates
}