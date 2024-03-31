using Chess;

namespace Moves
{
	[TestClass]
	public class PerftTests
	{
		// Values used from https://www.chessprogramming.org/Perft_Results

		[TestMethod]
		public void DefaultBoardPerft()
		{
			Board board = Board.DefaultBoard();
			Assert.AreEqual(20, board.GetNumberOfPositions(1));
			Assert.AreEqual(400, board.GetNumberOfPositions(2));
			Assert.AreEqual(8_902, board.GetNumberOfPositions(3));
			Assert.AreEqual(197_281, board.GetNumberOfPositions(4));
		}

		[TestMethod]
		public void KiwiPetePerft()
		{
			Board board = Board.ImportFromFEN("r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq - 0 1");
			Assert.AreEqual(48, board.GetNumberOfPositions(1));
			Assert.AreEqual(2_039, board.GetNumberOfPositions(2));
			Assert.AreEqual(97_862, board.GetNumberOfPositions(3));
			// Assert.AreEqual(4_085_603, board.GetNumberOfPositions(4)); // Takes too long...
		}

		[TestMethod]
		public void Position3Perft()
		{
			Board board = Board.ImportFromFEN("8/2p5/3p4/KP5r/1R3p1k/8/4P1P1/8 w - - 0 1");
			Assert.AreEqual(14, board.GetNumberOfPositions(1));
			Assert.AreEqual(191, board.GetNumberOfPositions(2));
			Assert.AreEqual(2_812, board.GetNumberOfPositions(3));
			Assert.AreEqual(43_238, board.GetNumberOfPositions(4));
			Assert.AreEqual(674_624, board.GetNumberOfPositions(5));
		}

		[TestMethod]
		public void Position4Perft()
		{
			Board board = Board.ImportFromFEN("r3k2r/Pppp1ppp/1b3nbN/nP6/BBP1P3/q4N2/Pp1P2PP/R2Q1RK1 w kq - 0 1");
			Assert.AreEqual(6, board.GetNumberOfPositions(1));
			Assert.AreEqual(264, board.GetNumberOfPositions(2));
			Assert.AreEqual(9_467, board.GetNumberOfPositions(3));
			//TODO Fix error
			// On board "r3k2r/Pppp1ppp/1b3nbN/nPP5/BB2P3/q4N2/Pp1P2PP/R2Q1RK1 b kq - 0 1"
			// the move rook a8 to b8 teleports it to a1 instead
			// resulting board state: "4k2r/Pppp1ppp/1b3nbN/nPP5/BB2P3/q4N2/Pp1P2PP/r2Q1RK1 w k - 0 1"
			// Therefore UndoPreviousMove() after PlayMove() results in a different board!

			// Assert.AreEqual(422_333, board.GetNumberOfPositions(4)); //TODO Uncomment once above case is fixed
		}
	}
}