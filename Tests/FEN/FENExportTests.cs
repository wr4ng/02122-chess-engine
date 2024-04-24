using Chess;

namespace Representation;

[TestClass]
public class ToFENTests
{
	[TestMethod]
	public void FENExportBoardToFen()
	{
		Board board;

		// Standard position
		board = Board.FromFEN(FEN.STARTING_POSITION_FEN);
		Assert.AreEqual("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR", FEN.BoardToFEN(board));

		// Other position
		board = Board.FromFEN("rn1qkbnr/2pppppp/8/8/8/8/PPPPPP2/RNBQKBN1 b KQkq - 0 1");
		Assert.AreEqual("rn1qkbnr/2pppppp/8/8/8/8/PPPPPP2/RNBQKBN1", FEN.BoardToFEN(board));
	}

	[TestMethod]
	public void FENExportCurrentPlayer()
	{
		// White to move
		Board board = Board.FromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
		Assert.AreEqual("w", FEN.ColorToFEN(board.colorToMove));
		// Black to move
		board = Board.FromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR b KQkq - 0 1");
		Assert.AreEqual("b", FEN.ColorToFEN(board.colorToMove));
	}

	//TODO This is more CastlingRights related testing. Maybe move to other test class?
	[TestMethod]
	public void FENExportCastlingRights()
	{
		// Full castling rights
		Board board = Board.FromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
		Assert.AreEqual("KQkq", board.castlingRights.ToFEN());
		// White limited
		board = Board.FromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w Kkq - 0 1");
		Assert.AreEqual("Kkq", board.castlingRights.ToFEN());
		// Black limited
		board = Board.FromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQ - 0 1");
		Assert.AreEqual("KQ", board.castlingRights.ToFEN());
		// None
		board = Board.FromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w - - 0 1");
		Assert.AreEqual("-", board.castlingRights.ToFEN());
	}

	[TestMethod]
	public void FENExportEnPassant()
	{
		// e3 en Passant square
		Board board = Board.FromFEN("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1");
		Assert.AreEqual("e3", FEN.EnPassantToFEN(board.enPassantSquare));
		// No en Passant square
		board = Board.FromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
		Assert.AreEqual("-", FEN.EnPassantToFEN(board.enPassantSquare));
	}

	[TestMethod]
	public void FENExportFull()
	{
		string fenA = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1";
		Board board = Board.FromFEN(fenA);
		Assert.AreEqual(fenA, board.ToFEN());
		string fenB = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 50";
		board = Board.FromFEN(fenB);
		Assert.AreEqual(fenB, board.ToFEN());
	}
}