using Chess;

[TestClass]
public class NewTests
{
	[TestMethod]
	public void NewDefaultBoardPerft()
	{
		NewBoard board = NewBoard.FromFEN(FEN.STARTING_POSITION_FEN);
		Assert.AreEqual(20, board.GetNumberOfPositions(1));
		Assert.AreEqual(400, board.GetNumberOfPositions(2));
		Assert.AreEqual(8_902, board.GetNumberOfPositions(3));
		Assert.AreEqual(197_281, board.GetNumberOfPositions(4));
		Assert.AreEqual(4_865_609, board.GetNumberOfPositions(5));
	}
}