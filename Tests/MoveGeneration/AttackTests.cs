using Chess;

namespace Tests.MoveGeneration;

	[TestClass]
	public class AttackTests
	{
		[TestMethod]
		public void AttackedByPawn()
		{
			Board board = Board.FromFEN("k7/8/8/8/8/8/6p1/7K b KQkq - 0 1");
			Assert.IsTrue(board.moveGenerator.IsAttacked((7, 0), board.colorToMove));
			board = Board.FromFEN("k7/8/8/8/8/8/7p/6K1 b KQkq - 0 1");
			Assert.IsTrue(board.moveGenerator.IsAttacked((6, 0), board.colorToMove));
			board = Board.FromFEN("k7/8/8/8/8/6K1/7p/8 b KQkq - 0 1");
			Assert.IsFalse(board.moveGenerator.IsAttacked((6, 2), board.colorToMove));
			board = Board.FromFEN("k7/8/8/8/8/7K/6p1/8 b KQkq - 0 1");
			Assert.IsFalse(board.moveGenerator.IsAttacked((6, 2), board.colorToMove));
		}

		[TestMethod]
		public void AttackedByBishop()
		{
			Board board = Board.FromFEN("k7/8/8/3b4/8/8/8/7K b KQkq - 0 1");
			Assert.IsTrue(board.moveGenerator.IsAttacked((7, 0), board.colorToMove));
			board = Board.FromFEN("k7/8/8/8/8/7b/8/5K2 b KQkq - 0 1");
			Assert.IsTrue(board.moveGenerator.IsAttacked((5, 0), board.colorToMove));
			board = Board.FromFEN("k7/8/8/5K2/8/7b/8/8 b KQkq - 0 1");
			Assert.IsTrue(board.moveGenerator.IsAttacked((5, 4), board.colorToMove));
			board = Board.FromFEN("k7/8/8/5K2/8/3b4/8/8 b KQkq - 0 1");
			Assert.IsTrue(board.moveGenerator.IsAttacked((5, 4), board.colorToMove));
		}

		[TestMethod]
		public void AttackedByRook()
		{
			Board board = Board.FromFEN("k7/8/7r/8/8/8/8/7K b KQkq - 0 1");
			Assert.IsTrue(board.moveGenerator.IsAttacked((7, 0), board.colorToMove));
			board = Board.FromFEN("k7/8/8/8/8/8/8/3r3K b KQkq - 0 1");
			Assert.IsTrue(board.moveGenerator.IsAttacked((7, 0), board.colorToMove));
			board = Board.FromFEN("k7/8/8/8/4K3/8/8/4r3 b KQkq - 0 1");
			Assert.IsTrue(board.moveGenerator.IsAttacked((4, 3), board.colorToMove));
			board = Board.FromFEN("k7/8/8/8/4K2r/8/8/8 b KQkq - 0 1");
			Assert.IsTrue(board.moveGenerator.IsAttacked((4, 3), board.colorToMove));
		}

		[TestMethod]
		public void AttackedByKnight()
		{
			Board board = Board.FromFEN("k7/8/8/8/4K3/2n5/8/8 b KQkq - 0 1");
			Assert.IsTrue(board.moveGenerator.IsAttacked((4, 3), board.colorToMove));
		}

		[TestMethod]
		public void AttackedByQueen()
		{
			Board board = Board.FromFEN("k7/8/8/8/4K3/8/2q5/8 b KQkq - 0 1");
			Assert.IsTrue(board.moveGenerator.IsAttacked((4, 3), board.colorToMove));
			board = Board.FromFEN("k7/8/8/8/4K3/8/4q3/8 b KQkq - 0 1");
			Assert.IsTrue(board.moveGenerator.IsAttacked((4, 3), board.colorToMove));
		}

		[TestMethod]
		public void AttackedByKing()
		{
			Board board = Board.FromFEN("8/8/8/8/4K3/4k3/8/8 b KQkq - 0 1");
			Assert.IsTrue(board.moveGenerator.IsAttacked((4, 3), board.colorToMove));
		}
	}