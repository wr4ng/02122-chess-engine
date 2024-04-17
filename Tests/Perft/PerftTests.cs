using Chess;

namespace Tests.Perft;

[TestClass]
public class PerftTests
{
	// Uses values from https://www.chessprogramming.org/Perft_Results

	[TestMethod]
	public void PerftDefaultBoard()
	{
		NewBoard board = NewBoard.FromFEN(FEN.STARTING_POSITION_FEN);
		Assert.AreEqual(20, board.GetNumberOfPositions(1));
		Assert.AreEqual(400, board.GetNumberOfPositions(2));
		Assert.AreEqual(8_902, board.GetNumberOfPositions(3));
		Assert.AreEqual(197_281, board.GetNumberOfPositions(4));
		Assert.AreEqual(4_865_609, board.GetNumberOfPositions(5));
	}

	[TestMethod]
	public void PerftKiwiPete()
	{
		NewBoard board = NewBoard.FromFEN("r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq - 0 1");
		Assert.AreEqual(48, board.GetNumberOfPositions(1));
		Assert.AreEqual(2_039, board.GetNumberOfPositions(2));
		Assert.AreEqual(97_862, board.GetNumberOfPositions(3));
		Assert.AreEqual(4_085_603, board.GetNumberOfPositions(4));
	}

	[TestMethod]
	public void PerftPosition3()
	{
		NewBoard board = NewBoard.FromFEN("8/2p5/3p4/KP5r/1R3p1k/8/4P1P1/8 w - - 0 1");
		Assert.AreEqual(14, board.GetNumberOfPositions(1));
		Assert.AreEqual(191, board.GetNumberOfPositions(2));
		Assert.AreEqual(2_812, board.GetNumberOfPositions(3));
		Assert.AreEqual(43_238, board.GetNumberOfPositions(4));
		Assert.AreEqual(674_624, board.GetNumberOfPositions(5));
		Assert.AreEqual(11_030_083, board.GetNumberOfPositions(6));
	}

	[TestMethod]
	public void PerftPosition4()
	{
		NewBoard board = NewBoard.FromFEN("r3k2r/Pppp1ppp/1b3nbN/nP6/BBP1P3/q4N2/Pp1P2PP/R2Q1RK1 w kq - 0 1");
		Assert.AreEqual(6, board.GetNumberOfPositions(1));
		Assert.AreEqual(264, board.GetNumberOfPositions(2));
		Assert.AreEqual(9_467, board.GetNumberOfPositions(3));
		Assert.AreEqual(422_333, board.GetNumberOfPositions(4));
		Assert.AreEqual(15_833_292, board.GetNumberOfPositions(5));
	}

	[TestMethod]
	public void PerftPosition5()
	{
		NewBoard board = NewBoard.FromFEN("rnbq1k1r/pp1Pbppp/2p5/8/2B5/8/PPP1NnPP/RNBQK2R w KQ - 1 8");
		Assert.AreEqual(44, board.GetNumberOfPositions(1));
		Assert.AreEqual(1_486, board.GetNumberOfPositions(2));
		Assert.AreEqual(62_379, board.GetNumberOfPositions(3));
		Assert.AreEqual(2_103_487, board.GetNumberOfPositions(4));
	}

	[TestMethod]
	public void PerftPosition6()
	{
		NewBoard board = NewBoard.FromFEN("r4rk1/1pp1qppp/p1np1n2/2b1p1B1/2B1P1b1/P1NP1N2/1PP1QPPP/R4RK1 w - - 0 10");
		Assert.AreEqual(46, board.GetNumberOfPositions(1));
		Assert.AreEqual(2_079, board.GetNumberOfPositions(2));
		Assert.AreEqual(89_890, board.GetNumberOfPositions(3));
		Assert.AreEqual(3_894_594, board.GetNumberOfPositions(4));
	}

	//TODO Move to move generation
	[TestMethod]
	public void EnPassantDiscoveredCheck()
	{
		// In this position, pawn on d5 shouldn't be able to capture on c6 by EP, since it would lead to check by rook on h5
		NewBoard board = NewBoard.FromFEN("7k/8/8/K1pP3r/8/8/8/8 w - c6 0 1");
		List<NewMove> moves = board.moveGenerator.GenerateMoves();
		// Assert that the en Passant capture doesn't exists
		Assert.IsFalse(moves.Any(move => move.isEnPassantCapture));
	}
}