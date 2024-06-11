using Chess;

namespace Tests.BoardTests;

[TestClass]
public class MakeMoveTests
{
	[TestMethod]
	public void MakeMoveBoard()
	{
		// Move: Rook A1 -> A3 yields the correct board
		Board board = Board.FromFEN("rnbqkbnr/pppppppp/8/8/8/8/1PPPPPPP/RNBQKBNR w KQkq - 0 1");
		Move move = new Move((0, 0), (0, 2));
		board.MakeMove(move);
		Assert.AreEqual("rnbqkbnr/pppppppp/8/8/8/R7/1PPPPPPP/1NBQKBNR", FEN.BoardToFEN(board));
	}

	[TestMethod]
	public void UnmakeMoveBoard()
	{
		// Making and unmaking a move results in the original board
		Board board = Board.FromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
		Move move = new Move((0, 1), (0, 2));
		board.MakeMove(move);
		Assert.AreEqual("rnbqkbnr/pppppppp/8/8/8/P7/1PPPPPPP/RNBQKBNR", FEN.BoardToFEN(board));
		board.UndoPreviousMove();
		Assert.AreEqual("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR", FEN.BoardToFEN(board));
	}

	[TestMethod]
	public void MakeMoveCapture()
	{
		Board board = Board.FromFEN("1nbqkbnr/pppppppp/8/8/8/r7/1PPPPPPP/RNBQKBNR w KQkq - 0 1");
		// Capture black rook on A3 using white rook on A1
		Move move = new Move((0, 0), (0, 2), capturedPiece: Piece.Black | Piece.Rook);
		board.MakeMove(move);
		Assert.AreEqual("1nbqkbnr/pppppppp/8/8/8/R7/1PPPPPPP/1NBQKBNR", FEN.BoardToFEN(board));
		Assert.AreEqual(Piece.Black | Piece.Rook, move.capturedPiece);
	}

	[TestMethod]
	public void UnmakeMoveCapture()
	{
		Board board = Board.FromFEN("1nbqkbnr/pppppppp/8/8/8/r7/1PPPPPPP/RNBQKBNR w KQkq - 0 1");
		// Capture black rook on A3 using white rook on A1
		Move move = new Move((0, 0), (0, 2), capturedPiece: Piece.Black | Piece.Rook);
		board.MakeMove(move);
		board.UndoPreviousMove();
		Assert.AreEqual("1nbqkbnr/pppppppp/8/8/8/r7/1PPPPPPP/RNBQKBNR", FEN.BoardToFEN(board));
	}

	[TestMethod]
	public void MakeMoveEnPassant()
	{
		Board board = Board.FromFEN("k7/8/8/3pP3/8/8/8/K7 w KQkq d6 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> enPassantMoves = legalMoves.Where(move => move.isEnPassantCapture).ToList();
		board.MakeMove(enPassantMoves[0]);
		Assert.AreEqual("k-------\n--------\n---P----\n--------\n--------\n--------\n--------\nK-------", board.ToString());

		board = Board.FromFEN("k7/8/8/4Pp2/8/8/8/K7 w KQkq f6 0 1");
		legalMoves = board.moveGenerator.GenerateMoves();
		enPassantMoves = legalMoves.Where(move => move.isEnPassantCapture).ToList();
		board.MakeMove(enPassantMoves[0]);
		Assert.AreEqual("k-------\n--------\n-----P--\n--------\n--------\n--------\n--------\nK-------", board.ToString());

		board = Board.FromFEN("k7/8/8/8/3pP3/8/8/K7 b KQkq e3 0 1");
		legalMoves = board.moveGenerator.GenerateMoves();
		enPassantMoves = legalMoves.Where(move => move.isEnPassantCapture).ToList();
		board.MakeMove(enPassantMoves[0]);
		Assert.AreEqual("k-------\n--------\n--------\n--------\n--------\n----p---\n--------\nK-------", board.ToString());

		board = Board.FromFEN("k7/8/8/8/4Pp2/8/8/K7 b KQkq e3 0 1");
		legalMoves = board.moveGenerator.GenerateMoves();
		enPassantMoves = legalMoves.Where(move => move.isEnPassantCapture).ToList();
		board.MakeMove(enPassantMoves[0]);
		Assert.AreEqual("k-------\n--------\n--------\n--------\n--------\n----p---\n--------\nK-------", board.ToString());
	}
	//TODO UnmakeMoveEnPassant...

	[TestMethod]
	public void MakeMoveCastling()
	{
		Board board = Board.FromFEN("4k3/8/8/8/8/8/8/R3K2R w KQkq - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> castleMoves = legalMoves.Where(move => move.isCastle).ToList();
		board.MakeMove(castleMoves[0]);
		Assert.AreEqual("----k---\n--------\n--------\n--------\n--------\n--------\n--------\nR----RK-", board.ToString());
		board.UndoPreviousMove();
		board.MakeMove(castleMoves[1]);
		Assert.AreEqual("----k---\n--------\n--------\n--------\n--------\n--------\n--------\n--KR---R", board.ToString());

		board = Board.FromFEN("4k3/8/8/8/8/8/8/R3K2R w Kkq - 0 1");
		legalMoves = board.moveGenerator.GenerateMoves();
		castleMoves = legalMoves.Where(move => move.isCastle).ToList();
		Assert.AreEqual(1, castleMoves.Count);

		board = Board.FromFEN("4k3/8/8/8/8/8/8/R3K2R w kq - 0 1");
		legalMoves = board.moveGenerator.GenerateMoves();
		castleMoves = legalMoves.Where(move => move.isCastle).ToList();
		Assert.AreEqual(0, castleMoves.Count);
	}

	[TestMethod]
	public void MakeMovePromotion()
	{
		Board board = Board.FromFEN("8/k4P2/8/8/8/8/8/7K w - - 0 1");
		List<Move> legalMoves = board.moveGenerator.GenerateMoves();
		List<Move> promotionMoves = legalMoves.Where(move => move.promotionType != Piece.None).ToList();

		// Rook promotion
		board.MakeMove(promotionMoves[3]);
		Assert.AreEqual("5R2/k7/8/8/8/8/8/7K", FEN.BoardToFEN(board));
		board.UndoPreviousMove();
		Assert.AreEqual("8/k4P2/8/8/8/8/8/7K w - - 0 1", board.ToFEN());

		// Queen promotion
		board.MakeMove(promotionMoves[0]);
		Assert.AreEqual("5Q2/k7/8/8/8/8/8/7K", FEN.BoardToFEN(board));
		board.UndoPreviousMove();
		Assert.AreEqual("8/k4P2/8/8/8/8/8/7K w - - 0 1", board.ToFEN());
	}

	// TODO Capture promotion tests
}