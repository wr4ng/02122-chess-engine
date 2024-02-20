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
}