using Chess;

namespace Tests.BoardTests;

[TestClass]
public class ZobristTests
{
	[TestMethod]
	public void ZobristSimpleMove()
	{
		Board board = Board.FromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
		Move move = new Move((1, 0), (2, 2)); // Nc3

		board.MakeMove(move);
		ulong updatedHash = board.hash;

		Assert.AreEqual(Zobrist.GenerateHash(board), updatedHash);
	}

	[TestMethod]
	public void ZobristDoublePawn()
	{
		Board board = Board.FromFEN("r1bqk2r/ppp1bppp/n3pn2/1N1p4/3P1B2/4P3/PPP2PPP/R2QKBNR w KQkq - 1 6");
		Move move = new Move((7, 1), (7, 3)); // h4

		board.MakeMove(move);
		ulong updatedHash = board.hash;

		Assert.AreEqual(Zobrist.GenerateHash(board), updatedHash);
	}

	[TestMethod]
	public void ZobristCapture()
	{
		Board board = Board.FromFEN("r2q1rk1/pppbb1pp/n3p3/1N1pBpN1/3Pn2P/2P1P3/PP3PP1/R2QKB1R b KQ - 1 10");
		Move move = new Move((4, 6), (6, 4), capturedPiece: Piece.White | Piece.Knight); // bxg5

		board.MakeMove(move);
		ulong updatedHash = board.hash;

		Assert.AreEqual(Zobrist.GenerateHash(board), updatedHash);
	}

	[TestMethod]
	public void ZobristEnPassantCapture()
	{
		Board board = Board.FromFEN("8/8/3p4/KPp5/1R3p1k/8/4P1P1/8 w - c6 0 1");
		Move move = new Move((1, 4), (2, 5), capturedPiece: Piece.Black | Piece.Pawn, isEnPassantCapture: true); // bxc6

		board.MakeMove(move);
		ulong updatedHash = board.hash;

		Assert.AreEqual(Zobrist.GenerateHash(board), updatedHash);
	}

	[TestMethod]
	public void ZobristClearEnPassant()
	{
		Board board = Board.FromFEN("rnbqkbnr/p3pppp/8/1pP5/8/8/PP1PPPPP/RNBQKBNR w KQkq b6 0 1");
		Move move = new Move((3, 0), (1, 2)); // Qb2

		board.MakeMove(move);
		ulong updatedHash = board.hash;

		Assert.AreEqual(Zobrist.GenerateHash(board), updatedHash);
	}

	[TestMethod]
	public void ZobristCastling()
	{
		Board board = Board.FromFEN("rn1qk2r/pp2ppbp/2p1bnp1/3p4/P2P3N/6P1/1PPNPPBP/R1BQK2R w KQkq - 1 8");
		Move move = new Move((4, 0), (6, 0), isCastle: true); // O-O

		board.MakeMove(move);
		ulong updatedHash = board.hash;

		Assert.AreEqual(Zobrist.GenerateHash(board), updatedHash);
	}

	[TestMethod]
	public void ZobristPromotion()
	{
		Board board = Board.FromFEN("8/8/2P5/K7/5p1k/8/3pP3/8 b - - 0 1");
		Move move = new Move((3, 1), (3, 0), promotionType: Piece.Knight); //d1N

		board.MakeMove(move);
		ulong updatedHash = board.hash;

		Assert.AreEqual(Zobrist.GenerateHash(board), updatedHash);
	}

	[TestMethod]
	public void ZobristUnmake()
	{
		Board board = Board.FromFEN(FEN.STARTING_POSITION_FEN);
		Move move = board.moveGenerator.GenerateMoves()[0];

		ulong initialHash = board.hash;

		board.MakeMove(move);
		board.UndoPreviousMove();

		Assert.AreEqual(initialHash, board.hash);
	}
}
