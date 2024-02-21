using Chess;

namespace Representation;

[TestClass]
public class ToFENTests
{
	[TestMethod]
	public void BoardToFEN()
	{
        Board board = Board.ImportFromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
		Assert.AreEqual("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR", board.ExportToFEN().Split(" ")[0]);
	}

    [TestMethod]
	public void DifferentBoardToFEN()
	{
        Board board = Board.ImportFromFEN("rn1qkbnr/2pppppp/8/8/8/8/PPPPPP2/RNBQKBN1 b KQkq - 0 1");
		Assert.AreEqual("rn1qkbnr/2pppppp/8/8/8/8/PPPPPP2/RNBQKBN1", board.ExportToFEN().Split(" ")[0]);
	}
    [TestMethod]
    public void CurrentPlayerToFEN(){
        Board board = Board.ImportFromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        Assert.AreEqual("w", board.ExportToFEN().Split(" ")[1]);
        board = Board.ImportFromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR b KQkq - 0 1");
        Assert.AreEqual("b", board.ExportToFEN().Split(" ")[1]);
    }
    [TestMethod]
    public void CastlingRightsToFEN(){
        Board board = Board.ImportFromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        Assert.AreEqual("KQkq", board.ExportToFEN().Split(" ")[2]);
        board = Board.ImportFromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w Kkq - 0 1");
        Assert.AreEqual("Kkq", board.ExportToFEN().Split(" ")[2]);
        board = Board.ImportFromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQ - 0 1");
        Assert.AreEqual("KQ", board.ExportToFEN().Split(" ")[2]);
        board = Board.ImportFromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w - - 0 1");
        Assert.AreEqual("-", board.ExportToFEN().Split(" ")[2]);
    }
    [TestMethod]
    public void EnPassantToFEN(){
        Board board = Board.ImportFromFEN("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1");
        Assert.AreEqual("e3", board.ExportToFEN().Split(" ")[3]);
        board = Board.ImportFromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        Assert.AreEqual("-", board.ExportToFEN().Split(" ")[3]);
    }
    [TestMethod]
    public void HalfmoveClockToFEN(){
        Board board = Board.ImportFromFEN("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1");
        Assert.AreEqual("0", board.ExportToFEN().Split(" ")[4]);
        board = Board.ImportFromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 50 1");
        Assert.AreEqual("50", board.ExportToFEN().Split(" ")[4]);
    }
    [TestMethod]
    public void FullmoveNumberToFEN(){
        Board board = Board.ImportFromFEN("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1");
        Assert.AreEqual("1", board.ExportToFEN().Split(" ")[5]);
        board = Board.ImportFromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 50");
        Assert.AreEqual("50", board.ExportToFEN().Split(" ")[5]);
    }
    [TestMethod]
    public void FullFEN(){
        Board board = Board.ImportFromFEN("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1");
        Assert.AreEqual("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1", board.ExportToFEN());
        board = Board.ImportFromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 50");
        Assert.AreEqual("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 50", board.ExportToFEN());
    }

}