using Chess;

namespace Representation;

[TestClass]
public class FENTests
{
	[TestMethod]
	public void StartingPosition()
	{
		Board board = Board.ImportFromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
		Assert.AreEqual("rnbqkbnr\npppppppp\n--------\n--------\n--------\n--------\nPPPPPPPP\nRNBQKBNR", board.ToString());
	}

	[TestMethod]
	public void OneOfEachPiece()
	{
		Board b = Board.ImportFromFEN("k7/8/8/8/8/8/8/rnbqKBNR b KQkq - 0 1");
		Assert.AreEqual("Black Rook", b.GetPiece(0, 0).ToString());
		Assert.AreEqual("Black Knight", b.GetPiece(1, 0).ToString());
		Assert.AreEqual("Black Bishop", b.GetPiece(2, 0).ToString());
		Assert.AreEqual("Black Queen", b.GetPiece(3, 0).ToString());
		Assert.AreEqual("White King", b.GetPiece(4, 0).ToString());
		Assert.AreEqual("White Bishop", b.GetPiece(5, 0).ToString());
		Assert.AreEqual("White Knight", b.GetPiece(6, 0).ToString());
		Assert.AreEqual("White Rook", b.GetPiece(7, 0).ToString());
	}

	[TestMethod]
	public void InvalidParts()
	{
		Assert.ThrowsException<ArgumentException>(() => Board.ImportFromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq 0 1 1"));   // Too many parts
		Assert.ThrowsException<ArgumentException>(() => Board.ImportFromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq 0"));       // Too few parts
	}

	[TestMethod]
	public void InvalidBoardSetup()
	{
		Assert.ThrowsException<ArgumentException>(() => Board.ImportFromFEN("rnbqkbnr/ppppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq 0 1"));            // Too long rank
		Assert.ThrowsException<ArgumentException>(() => Board.ImportFromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR/RNBQKBNR w KQkq 0 1 1"));  // Too many ranks
		Assert.ThrowsException<ArgumentException>(() => Board.ImportFromFEN("rnbqkbnr/ppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq 0 1"));              // Too short rank

	}

	[TestMethod]
	public void InvalidPiece()
	{
		Assert.ThrowsException<ArgumentException>(() => Board.ImportFromFEN("k7/8/8/8/8/8/8/robqKBNR b KQkq - 0 1"));   // Invalid piece 'o'
	}

	[TestMethod]
	public void CurrentPlayer()
	{
		Board b = Board.ImportFromFEN("k7/8/8/8/8/8/7q/K7 w KQkq - 0 1");
		Assert.AreEqual(Color.White, b.GetCurrentPlayer());
		b = Board.ImportFromFEN("k7/8/8/8/8/8/7q/K7 b KQkq - 0 1");
		Assert.AreEqual(Color.Black, b.GetCurrentPlayer());
	}

	[TestMethod]
	public void CurrentPlayerInvalid()
	{
		Assert.ThrowsException<ArgumentException>(() => Board.ImportFromFEN("k7/8/8/8/8/8/7q/K7 Q KQkq - 0 1"));
	}

	[TestMethod]
	public void CastlingRights()
	{
		Board b = Board.ImportFromFEN("k7/8/8/8/8/8/7q/K7 w KQkq - 0 1");
		Assert.AreEqual("KQkq", b.GetCastlingRights());
	}

	[TestMethod]
	public void CastlingRightsWhiteLimited()
	{
		Board b = Board.ImportFromFEN("k7/8/8/8/8/8/7q/K7 w Kkq - 0 1");
		Assert.AreEqual("Kkq", b.GetCastlingRights());
	}

	[TestMethod]
	public void CastlingRightsBlackLimited()
	{
		Board b = Board.ImportFromFEN("k7/8/8/8/8/8/7q/K7 w KQ - 0 1");
		Assert.AreEqual("KQ", b.GetCastlingRights());
	}

	[TestMethod]
	public void CastlingRightsNone()
	{
		Board b = Board.ImportFromFEN("k7/8/8/8/8/8/7q/K7 w - - 0 1");
		Assert.AreEqual("-", b.GetCastlingRights());
	}

	[TestMethod]
	public void CastlingRightsInvalid()
	{
		Assert.ThrowsException<ArgumentException>(() => Board.ImportFromFEN("k7/8/8/8/8/8/7q/K7 w CASTLING - 0 1"));
	}

	[TestMethod]
	public void EnPassantSquare()
	{
		Board b = Board.ImportFromFEN("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1");
		Assert.AreEqual("e3", b.GetEnPassantSquare());
	}

	[TestMethod]
	public void NoEnPassantSquare()
	{
		Board b = Board.ImportFromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
		Assert.AreEqual("-", b.GetEnPassantSquare());
	}

	[TestMethod]
	public void InvalidEnPassantSquare()
	{
		Assert.ThrowsException<ArgumentException>(() => Board.ImportFromFEN("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq k4 0 1"));
		Assert.ThrowsException<ArgumentException>(() => Board.ImportFromFEN("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e5 0 1"));

	}

	[TestMethod]
	public void HalfmoveClock()
	{
		Board b = Board.ImportFromFEN("k7/8/8/8/8/8/7q/K7 w KQkq - 45 1");
		Assert.AreEqual(45, b.GetHalfmoveClock());
	}

	[TestMethod]
	public void HalfmoveClockInvalid()
	{
		Assert.ThrowsException<ArgumentException>(() => Board.ImportFromFEN("k7/8/8/8/8/8/7q/K7 w KQkq - DinMor 1"));

	}

	[TestMethod]
	public void FullmoveNumber()
	{
		Board b = Board.ImportFromFEN("k7/8/8/8/8/8/7q/K7 w KQkq - 0 34");
		Assert.AreEqual(34, b.GetFullmoveNumber());
	}

	[TestMethod]
	public void FullmoveNumberInvalid()
	{
		Assert.ThrowsException<ArgumentException>(() => Board.ImportFromFEN("k7/8/8/8/8/8/7q/K7 w KQkq - 0 MadsMorErFlot"));

	}
}