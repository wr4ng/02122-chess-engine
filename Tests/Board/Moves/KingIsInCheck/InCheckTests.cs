using Chess;

namespace KingIsInCheck
{

	[TestClass]
	public class InCheckTests
	{
		[TestMethod]
		public void CheckedByPawn()
		{
			Board board = Board.ImportFromFEN("k7/8/8/8/8/8/6p1/7K b KQkq - 0 1");
			Assert.IsTrue(Attack.IsAttackedPawns((7, 0), board, board.GetCurrentPlayer()));
			board = Board.ImportFromFEN("k7/8/8/8/8/8/7p/6K1 b KQkq - 0 1");
			Assert.IsTrue(Attack.IsAttackedPawns((6, 0), board, board.GetCurrentPlayer()));
			board = Board.ImportFromFEN("k7/8/8/8/8/6K1/7p/8 b KQkq - 0 1");
			Assert.IsFalse(Attack.IsAttackedPawns((6, 2), board, board.GetCurrentPlayer()));
			board = Board.ImportFromFEN("k7/8/8/8/8/7K/6p1/8 b KQkq - 0 1");
			Assert.IsFalse(Attack.IsAttackedPawns((6, 2), board, board.GetCurrentPlayer()));
		}
		[TestMethod]
		public void CheckedByBishop()
		{
			Board board = Board.ImportFromFEN("k7/8/8/3b4/8/8/8/7K b KQkq - 0 1");
			Assert.IsTrue(Attack.IsAttackedDiagonally((7, 0), board, board.GetCurrentPlayer()));
			board = Board.ImportFromFEN("k7/8/8/8/8/7b/8/5K2 b KQkq - 0 1");
			Assert.IsTrue(Attack.IsAttackedDiagonally((5, 0), board, board.GetCurrentPlayer()));
			board = Board.ImportFromFEN("k7/8/8/5K2/8/7b/8/8 b KQkq - 0 1");
			Assert.IsTrue(Attack.IsAttackedDiagonally((5, 4), board, board.GetCurrentPlayer()));
			board = Board.ImportFromFEN("k7/8/8/5K2/8/3b4/8/8 b KQkq - 0 1");
			Assert.IsTrue(Attack.IsAttackedDiagonally((5, 4), board, board.GetCurrentPlayer()));
		}
		[TestMethod]
		public void CheckedByRook()
		{
			Board board = Board.ImportFromFEN("k7/8/7r/8/8/8/8/7K b KQkq - 0 1");
			Assert.IsTrue(Attack.IsAttackedHorizontally((7, 0), board, board.GetCurrentPlayer()));
			board = Board.ImportFromFEN("k7/8/8/8/8/8/8/3r3K b KQkq - 0 1");
			Assert.IsTrue(Attack.IsAttackedHorizontally((7, 0), board, board.GetCurrentPlayer()));
			board = Board.ImportFromFEN("k7/8/8/8/4K3/8/8/4r3 b KQkq - 0 1");
			Assert.IsTrue(Attack.IsAttackedHorizontally((4, 3), board, board.GetCurrentPlayer()));
			board = Board.ImportFromFEN("k7/8/8/8/4K2r/8/8/8 b KQkq - 0 1");
			Assert.IsTrue(Attack.IsAttackedHorizontally((4, 3), board, board.GetCurrentPlayer()));
		}
		[TestMethod]
		public void CheckedByKnight()
		{
			Board board = Board.ImportFromFEN("k7/8/8/8/4K3/2n5/8/8 b KQkq - 0 1");
			Assert.IsTrue(Attack.IsAttackedKnights((4, 3), board, board.GetCurrentPlayer()));
		}
		[TestMethod]
		public void CheckedByQueen()
		{
			Board board = Board.ImportFromFEN("k7/8/8/8/4K3/8/2q5/8 b KQkq - 0 1");
			Assert.IsTrue(Attack.IsAttackedDiagonally((4, 3), board, board.GetCurrentPlayer()));
			board = Board.ImportFromFEN("k7/8/8/8/4K3/8/4q3/8 b KQkq - 0 1");
			Assert.IsTrue(Attack.IsAttackedHorizontally((4, 3), board, board.GetCurrentPlayer()));
		}
		[TestMethod]
		public void CheckedByKing()
		{
			Board board = Board.ImportFromFEN("8/8/8/8/4K3/4k3/8/8 b KQkq - 0 1");
			Assert.IsTrue(Attack.IsAttackedKings((4, 3), board, board.GetCurrentPlayer()));
		}
	}
}