using Chess;

namespace Tests.MoveGeneration.InCheck;

[TestClass]
public class PawnCheckTests
{
	[TestMethod]
	public void PawnOneLegalMoveBlock()
	{
		Board board = Board.FromFEN("k7/8/8/8/4b3/8/5P2/7K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> pawnMoves = TestUtil.FilterForPieceType(legalMoves, Piece.Pawn, board);
		Assert.AreEqual(1, pawnMoves.Count);
	}

	[TestMethod]
	public void PawnOneLegalMoveCapture()
	{
		Board board = Board.FromFEN("k7/8/8/8/8/5b2/6P1/7K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> pawnMoves = TestUtil.FilterForPieceType(legalMoves, Piece.Pawn, board);
		Assert.AreEqual(1, pawnMoves.Count);
	}

	[TestMethod]
	public void PawnNoLegalMove()
	{
		Board board = Board.FromFEN("k7/8/8/8/4b3/8/4P3/7K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> pawnMoves = TestUtil.FilterForPieceType(legalMoves, Piece.Pawn, board);
		Assert.AreEqual(0, pawnMoves.Count);
	}

	[TestMethod]
	public void PawnEnPassantDiscoveredCheck()
	{
		// In this position, pawn on d5 shouldn't be able to capture on c6 by EP, since it would lead to check by rook on h5
		Board board = Board.FromFEN("7k/8/8/K1pP3r/8/8/8/8 w - c6 0 1");
		List<Move> moves = board.moveGenerator.GenerateMoves();
		// Assert that the en Passant capture doesn't exists
		Assert.IsFalse(moves.Any(move => move.isEnPassantCapture));
	}
}