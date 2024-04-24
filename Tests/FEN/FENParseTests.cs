using Chess;

namespace Representation;

[TestClass]
public class FENTests
{
	[TestMethod]
	public void CoordinateToStringTest()
	{
		Assert.AreEqual("a1", FEN.CoordinateToFEN(0, 0));
		Assert.AreEqual("h8", FEN.CoordinateToFEN(7, 7));
		Assert.AreEqual("c5", FEN.CoordinateToFEN(2, 4));
		Assert.AreEqual("f7", FEN.CoordinateToFEN(5, 6));
	}

	[TestMethod]
	public void FENParseStartingPosition()
	{
		NewBoard board = NewBoard.FromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
		Assert.AreEqual("rnbqkbnr\npppppppp\n--------\n--------\n--------\n--------\nPPPPPPPP\nRNBQKBNR", board.ToString());
	}

	[TestMethod]
	public void FENParsePieces()
	{
		NewBoard board = NewBoard.FromFEN("k7/8/8/8/8/8/8/rnbqKBNR b KQkq - 0 1");
		Assert.AreEqual(NewPiece.Black | NewPiece.Rook, board.squares[0, 0]);
		Assert.AreEqual(NewPiece.Black | NewPiece.Knight, board.squares[1, 0]);
		Assert.AreEqual(NewPiece.Black | NewPiece.Bishop, board.squares[2, 0]);
		Assert.AreEqual(NewPiece.Black | NewPiece.Queen, board.squares[3, 0]);
		Assert.AreEqual(NewPiece.White | NewPiece.King, board.squares[4, 0]);
		Assert.AreEqual(NewPiece.White | NewPiece.Bishop, board.squares[5, 0]);
		Assert.AreEqual(NewPiece.White | NewPiece.Knight, board.squares[6, 0]);
		Assert.AreEqual(NewPiece.White | NewPiece.Rook, board.squares[7, 0]);
	}

	[TestMethod]
	public void FENParseInvalidParts()
	{
		Assert.ThrowsException<ArgumentException>(() => NewBoard.FromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq 0 1 1"));   // Too many parts
		Assert.ThrowsException<ArgumentException>(() => NewBoard.FromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq 0"));       // Too few parts
	}

	[TestMethod]
	public void FENParseInvalidBoardSetup()
	{
		Assert.ThrowsException<ArgumentException>(() => NewBoard.FromFEN("rnbqkbnr/ppppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq 0 1"));            // Too long rank
		Assert.ThrowsException<ArgumentException>(() => NewBoard.FromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR/RNBQKBNR w KQkq 0 1 1"));  // Too many ranks
		Assert.ThrowsException<ArgumentException>(() => NewBoard.FromFEN("rnbqkbnr/ppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq 0 1"));              // Too short rank

	}

	[TestMethod]
	public void FENParseInvalidPiece()
	{
		Assert.ThrowsException<ArgumentException>(() => NewBoard.FromFEN("k7/8/8/8/8/8/8/robqKBNR b KQkq - 0 1"));   // Invalid piece 'o'
	}

	[TestMethod]
	public void FENParseCurrentPlayer()
	{
		// White
		NewBoard board = NewBoard.FromFEN("k7/8/8/8/8/8/7q/K7 w KQkq - 0 1");
		Assert.AreEqual(NewPiece.White, board.colorToMove);
		// Black
		board = NewBoard.FromFEN("k7/8/8/8/8/8/7q/K7 b KQkq - 0 1");
		Assert.AreEqual(NewPiece.Black, board.colorToMove);
	}

	[TestMethod]
	public void FENParseCurrentPlayerInvalid()
	{
		Assert.ThrowsException<ArgumentException>(() => NewBoard.FromFEN("k7/8/8/8/8/8/7q/K7 Q KQkq - 0 1"));
	}

	[TestMethod]
	public void FENParseCastlingRights()
	{
		NewBoard board;
		// Full castling rights
		board = NewBoard.FromFEN("k7/8/8/8/8/8/7q/K7 w KQkq - 0 1");
		Assert.AreEqual("KQkq", board.castlingRights.ToFEN());

		// White limited
		board = NewBoard.FromFEN("k7/8/8/8/8/8/7q/K7 w Kkq - 0 1");
		Assert.AreEqual("Kkq", board.castlingRights.ToFEN());

		// Black limited
		board = NewBoard.FromFEN("k7/8/8/8/8/8/7q/K7 w KQ - 0 1");
		Assert.AreEqual("KQ", board.castlingRights.ToFEN());

		// None
		board = NewBoard.FromFEN("k7/8/8/8/8/8/7q/K7 w - - 0 1");
		Assert.AreEqual("-", board.castlingRights.ToFEN());
	}

	[TestMethod]
	public void FENParseCastlingRightsInvalid()
	{
		Assert.ThrowsException<ArgumentException>(() => NewBoard.FromFEN("k7/8/8/8/8/8/7q/K7 w INVALID - 0 1"));
	}

	[TestMethod]
	public void FENParseEnPassantSquare()
	{
		NewBoard board;
		// e3
		board = NewBoard.FromFEN("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1");
		Assert.AreEqual("e3", FEN.EnPassantToFEN(board.enPassantSquare));
		// No EP square
		board = NewBoard.FromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
		Assert.AreEqual("-", FEN.EnPassantToFEN(board.enPassantSquare));
	}

	[TestMethod]
	public void FENParseInvalidEnPassantSquare()
	{
		Assert.ThrowsException<ArgumentException>(() => NewBoard.FromFEN("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq k4 0 1"));
		Assert.ThrowsException<ArgumentException>(() => NewBoard.FromFEN("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e5 0 1"));
	}

	[TestMethod]
	public void FENParseHalfmoveClock()
	{
		NewBoard board = NewBoard.FromFEN("k7/8/8/8/8/8/7q/K7 w KQkq - 45 1");
		Assert.AreEqual(45, board.halfMoveClock);
	}

	[TestMethod]
	public void FENParseHalfmoveClockInvalid()
	{
		Assert.ThrowsException<ArgumentException>(() => NewBoard.FromFEN("k7/8/8/8/8/8/7q/K7 w KQkq - INVALID 1"));
	}

	[TestMethod]
	public void FENParseFullmoveNumber()
	{
		NewBoard board = NewBoard.FromFEN("k7/8/8/8/8/8/7q/K7 w KQkq - 0 34");
		Assert.AreEqual(34, board.fullMoveNumber);
	}

	[TestMethod]
	public void FENParseFullmoveNumberInvalid()
	{
		Assert.ThrowsException<ArgumentException>(() => NewBoard.FromFEN("k7/8/8/8/8/8/7q/K7 w KQkq - 0 INVALID"));

	}
}