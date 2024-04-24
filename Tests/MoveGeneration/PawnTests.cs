using Chess;

namespace Tests.MoveGeneration;

[TestClass]
public class PawnTests
{
	[TestMethod]
	public void GeneratePawnMoveTwoForward()
	{
		Board board = Board.FromFEN("7k/8/8/8/8/8/7P/7K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> pawnMoves = TestUtil.FilterForPieceType(legalMoves, Piece.Pawn, board);

		Assert.AreEqual(2, pawnMoves.Count);
		Assert.AreEqual((7, 1), pawnMoves[0].from);
		Assert.AreEqual((7, 2), pawnMoves[0].to);
		Assert.AreEqual((7, 1), pawnMoves[1].from);
		Assert.AreEqual((7, 3), pawnMoves[1].to);
	}

	[TestMethod]
	public void GeneratePawnMoveOneForward()
	{
		Board board = Board.FromFEN("7k/8/8/8/7p/8/7P/7K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> pawnMoves = TestUtil.FilterForPieceType(legalMoves, Piece.Pawn, board);

		Assert.AreEqual(1, pawnMoves.Count);
		Assert.AreEqual((7, 1), pawnMoves[0].from);
		Assert.AreEqual((7, 2), pawnMoves[0].to);
	}

	[TestMethod]
	public void GeneratePawnMoveNotStartingRank()
	{
		Board board = Board.FromFEN("7k/8/8/8/8/7P/8/7K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> pawnMoves = TestUtil.FilterForPieceType(legalMoves, Piece.Pawn, board);

		Assert.AreEqual(1, pawnMoves.Count);
		Assert.AreEqual((7, 2), pawnMoves[0].from);
		Assert.AreEqual((7, 3), pawnMoves[0].to);
	}

	[TestMethod]
	public void GeneratePawnMoveNoMoves()
	{
		Board board = Board.FromFEN("8/8/8/8/8/7k/7P/7K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> pawnMoves = TestUtil.FilterForPieceType(legalMoves, Piece.Pawn, board);
		Assert.AreEqual(0, pawnMoves.Count);
	}

	[TestMethod]
	public void GeneratePawnMoveAllPossible()
	{
		Board board = Board.FromFEN("7k/8/8/8/8/5p1p/6P1/7K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> pawnMoves = TestUtil.FilterForPieceType(legalMoves, Piece.Pawn, board);

		Assert.AreEqual(4, pawnMoves.Count);
		Assert.AreEqual((6, 1), pawnMoves[0].from);
		Assert.AreEqual((6, 2), pawnMoves[0].to);
		Assert.AreEqual((6, 1), pawnMoves[1].from);
		Assert.AreEqual((6, 3), pawnMoves[1].to);
		Assert.AreEqual((6, 1), pawnMoves[2].from);
		Assert.AreEqual((5, 2), pawnMoves[2].to);
		Assert.AreEqual((6, 1), pawnMoves[3].from);
		Assert.AreEqual((7, 2), pawnMoves[3].to);
	}

	[TestMethod]
	public void GeneratePawnMoveEdgeCases()
	{
		// Blocked forward, can capture pawn on g3
		Board board = Board.FromFEN("7k/8/8/8/8/6pp/7P/7K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> pawnMoves = TestUtil.FilterForPieceType(legalMoves, Piece.Pawn, board);

		Assert.AreEqual(1, pawnMoves.Count);
		Assert.AreEqual((7, 1), pawnMoves[0].from);
		Assert.AreEqual((6, 2), pawnMoves[0].to);

		// Same on opposite edge file
		board = Board.FromFEN("7k/8/8/8/8/pp6/P7/7K w - - 0 1");
		legalMoves = board.moveGenerator.GenerateMoves();
		pawnMoves = TestUtil.FilterForPieceType(legalMoves, Piece.Pawn, board);

		Assert.AreEqual(1, pawnMoves.Count);
		Assert.AreEqual((0, 1), pawnMoves[0].from);
		Assert.AreEqual((1, 2), pawnMoves[0].to);
	}

	[TestMethod]
	public void GeneratePawnEnPassant()
	{
		Board board = Board.FromFEN("7k/8/8/pP6/8/8/8/7K w - a6 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> enPassantMoves = legalMoves.Where(move => move.isEnPassantCapture).ToList();
		Assert.AreEqual(1, enPassantMoves.Count);

		board = Board.FromFEN("7k/8/8/1Pp5/8/8/8/7K w - c6 0 1");
		legalMoves = board.moveGenerator.GenerateMoves();
		enPassantMoves = legalMoves.Where(move => move.isEnPassantCapture).ToList();
		Assert.AreEqual(1, enPassantMoves.Count);
	}

	[TestMethod]
	public void GeneratePawnSimplePromotion()
	{
		// White promotion
		Board board = Board.FromFEN("k7/3P4/8/8/8/8/8/7K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> promotionMoves = legalMoves.Where(move => move.promotionType != Piece.None).ToList();
		Assert.AreEqual(4, promotionMoves.Count);

		// Black promotion
		board = Board.FromFEN("k7/8/8/8/8/8/5p2/7K b - - 0 1");
		legalMoves = board.moveGenerator.GenerateMoves();
		promotionMoves = legalMoves.Where(move => move.promotionType != Piece.None).ToList();
		Assert.AreEqual(4, promotionMoves.Count);
	}

	[TestMethod]
	public void GeneratePawnCapturePromotion()
	{
		Board board = Board.FromFEN("2r4k/1P6/8/8/8/8/8/K7 w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> promotionCaptures = legalMoves.Where(move => move.promotionType != Piece.None && move.capturedPiece != Piece.None).ToList();
		Assert.AreEqual(4, promotionCaptures.Count);
	}
}
