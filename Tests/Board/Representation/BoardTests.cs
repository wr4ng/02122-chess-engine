namespace Representation;

[TestClass]
public class BoardTests
{
	[TestMethod]
	public void FENOnePiece()
	{
		Board b = new Board();
		b.ImportFromFEN("7K/8/8/7p/8/8/8/k7 w KQkq - 0 1"); // i added 2 kings in case we check for game over when importing FEN
		Assert.AreEqual("Black Pawn", b.GetPiece(7, 4).ToString());
		Assert.AreEqual(null, b.GetPiece(0, 0)); //kan man det?
	}

	[TestMethod]
	public void FENTwoPieces()
	{
		Board b = new Board();
		b.ImportFromFEN("k6K/8/8/7p/8/8/8/P7 w KQkq - 0 1");
		Assert.AreEqual("Black Pawn", b.GetPiece(7, 4).ToString());
		Assert.AreEqual("White Pawn", b.GetPiece(0, 0).ToString());
	}

	[TestMethod]
	public void FENOneOfEachPiece()
	{
		Board b = new Board();
		b.ImportFromFEN("k7/8/8/8/8/8/8/rnbqKBNR b KQkq - 0 1");
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
	public void FENCurrentPlayerWhite()
	{
		Board b = new Board();
		b.ImportFromFEN("k7/8/8/8/8/8/7q/K7 w KQkq - 0 1");
		Assert.AreEqual(Color.White, b.GetCurrentPlayer());
	}

	[TestMethod]
	public void FENCurrentPlayerBlack()
	{
		Board b = new Board();
		b.ImportFromFEN("k7/8/8/8/8/8/7q/K7 b KQkq - 0 1");
		Assert.AreEqual(Color.Black, b.GetCurrentPlayer());
	}

	[TestMethod]
	public void FENCastlingRights()
	{
		Board b = new Board();
		b.ImportFromFEN("k7/8/8/8/8/8/7q/K7 w KQkq - 0 1");
		Assert.AreEqual("KQkq", b.GetCastlingRights());
	}

	[TestMethod]
	public void FENCastlingRightsWhiteLimited()
	{
		Board b = new Board();
		b.ImportFromFEN("k7/8/8/8/8/8/7q/K7 w Kkq - 0 1");
		Assert.AreEqual("Kkq", b.GetCastlingRights());
	}

	[TestMethod]
	public void FENCastlingRightsBlackLimited()
	{
		Board b = new Board();
		b.ImportFromFEN("k7/8/8/8/8/8/7q/K7 w KQ - 0 1");
		Assert.AreEqual("KQ", b.GetCastlingRights());
	}

	[TestMethod]
	public void FENCastlingRightsNone()
	{
		Board b = new Board();
		b.ImportFromFEN("k7/8/8/8/8/8/7q/K7 w - - 0 1");
		Assert.AreEqual("-", b.GetCastlingRights());
	}

	[TestMethod]
	public void FENEnPassantSquare()
	{
		Board b = new Board();
		b.ImportFromFEN("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1");
		Assert.AreEqual("e3", b.GetEnPassantSquare());
	}

	[TestMethod]
	public void FENNoEnPassantSquare()
	{
		Board b = new Board();
		b.ImportFromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
		Assert.AreEqual("-", b.GetEnPassantSquare());
	}

	[TestMethod]
	public void FENHalfmoveClock()
	{
		Board b = new Board();
		b.ImportFromFEN("k7/8/8/8/8/8/7q/K7 w KQkq - 45 1");
		Assert.AreEqual(45, b.GetHalfmoveClock());
	}

	[TestMethod]
	public void FENHelfmoveClockZero()
	{
		Board b = new Board();
		b.ImportFromFEN("k7/8/8/8/8/8/7q/K7 w KQkq - 0 1");
		Assert.AreEqual(0, b.GetHalfmoveClock());
	}

	[TestMethod]
	public void FENFullmoveNumber()
	{
		Board b = new Board();
		b.ImportFromFEN("k7/8/8/8/8/8/7q/K7 w KQkq - 0 34");
		Assert.AreEqual(34, b.GetFullmoveNumber());
	}
}