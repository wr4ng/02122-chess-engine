using Chess;

namespace DrawTests
{
	[TestClass]
	public class positionCount
	{

		[TestMethod]
		public void AddStartPosition()
		{
			Board board = Board.DefaultBoard();
			Assert.AreEqual(1, board.draw.getPositionCount()["rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq -"]);
		}
    }
}