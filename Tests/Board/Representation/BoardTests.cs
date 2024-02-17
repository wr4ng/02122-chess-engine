namespace Representation;

[TestClass]
public class BoardTests
{
	[TestMethod]
	public void FENOnePiece()
	{
		Board b = new Board();
		b.importFromFEN("7K/8/8/7p/8/8/8/k7 w KQkq - 0 1"); // i added 2 kings in case we check for game over when importing FEN
		Assert.AreEqual("Black Pawn", b.getPiece(7, 4).toString());
		Assert.AreEqual(null, b.getPiece(0, 0)); //kan man det?
	}

	[TestMethod]
	public void FENTwoPieces()
	{
		Board b = new Board();
		b.importFromFEN("k6K/8/8/7p/8/8/8/P7 w KQkq - 0 1");
		Assert.AreEqual("Black Pawn", b.getPiece(7, 4).toString());
		Assert.AreEqual("White Pawn", b.getPiece(0, 0).toString());
	}

	[TestMethod]
	public void FENOneOfEachPiece()
	{
		Board b = new Board();
		b.importFromFEN("k7/8/8/8/8/8/8/rnbqKBNR b KQkq - 0 1");
		Assert.AreEqual("Black Rook", b.getPiece(0, 0).toString());
		Assert.AreEqual("Black Knight", b.getPiece(1, 0).toString());
		Assert.AreEqual("Black Bishop", b.getPiece(2, 0).toString());
		Assert.AreEqual("Black Queen", b.getPiece(3, 0).toString());
		Assert.AreEqual("White King", b.getPiece(4, 0).toString());
		Assert.AreEqual("White Bishop", b.getPiece(5, 0).toString());
		Assert.AreEqual("White Knight", b.getPiece(6, 0).toString());
		Assert.AreEqual("White Rook", b.getPiece(7, 0).toString());
	}

	[TestMethod]
	public void FENCurrentPlayerWhite()
	{
		Board b = new Board();
		b.importFromFEN("k7/8/8/8/8/8/7q/K7 w KQkq - 0 1");
		Assert.AreEqual(playerColor.White, b.getCurrentPlayer());
	}

	[TestMethod]
	public void FENCurrentPlayerBlack()
	{
		Board b = new Board();
		b.importFromFEN("k7/8/8/8/8/8/7q/K7 b KQkq - 0 1");
		Assert.AreEqual(playerColor.Black, b.getCurrentPlayer());
	}

	[TestMethod]
	public void FENCastlingRights()
	{
		Board b = new Board();
		b.importFromFEN("k7/8/8/8/8/8/7q/K7 w KQkq - 0 1");
		Assert.AreEqual("KQkq", b.getCastlingRights());
	}

	[TestMethod]
	public void FENCastlingRightsWhiteLimited()
	{
		Board b = new Board();
		b.importFromFEN("k7/8/8/8/8/8/7q/K7 w Kkq - 0 1");
		Assert.AreEqual("Kkq", b.getCastlingRights());
	}

	[TestMethod]
	public void FENCastlingRightsBlackLimited()
	{
		Board b = new Board();
		b.importFromFEN("k7/8/8/8/8/8/7q/K7 w KQ - 0 1");
		Assert.AreEqual("KQ", b.getCastlingRights());
	}

	[TestMethod]
	public void FENCastlingRightsNone()
	{
		Board b = new Board();
		b.importFromFEN("k7/8/8/8/8/8/7q/K7 w - - 0 1");
		Assert.AreEqual("-", b.getCastlingRights());
	}

	[TestMethod]
	public void FENEnPassantSquare()
	{
		Board b = new Board();
		b.importFromFEN("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1");
		Assert.AreEqual("e3", b.getEnPassantSquare());
	}

	[TestMethod]
	public void FENNoEnPassantSquare()
	{
		Board b = new Board();
		b.importFromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
		Assert.AreEqual("-", b.getEnPassantSquare());
	}

	[TestMethod]
	public void FENHalfmoveClock()
	{
		Board b = new Board();
		b.importFromFEN("k7/8/8/8/8/8/7q/K7 w KQkq - 45 1");
		Assert.AreEqual(45, b.getHalfmoveClock());
	}

	[TestMethod]
	public void FENHelfmoveClockZero()
	{
		Board b = new Board();
		b.importFromFEN("k7/8/8/8/8/8/7q/K7 w KQkq - 0 1");
		Assert.AreEqual(0, b.getHalfmoveClock());
	}

	[TestMethod]
	public void FENFullmoveNumber()
	{
		Board b = new Board();
		b.importFromFEN("k7/8/8/8/8/8/7q/K7 w KQkq - 0 34");
		Assert.AreEqual(34, b.getFullmoveNumber());
	}
}