using Chess;

namespace Moves
{
    [TestClass]
    public class KnightTests
    {

        [TestMethod]
        public void GenerateKnightMove()
        {
        Board board = Board.ImportFromFEN("7k/8/8/3N4/8/8/8/7K w - - 0 1");
        List<Move> moves = MoveGenerator.GenerateKnightMove((3,4),board);
        Assert.AreEqual(8,moves.Count);
        Assert.AreEqual((3,4),moves[0].start);
        Assert.AreEqual((1,3),moves[0].end);
        }
        [TestMethod]
        public void GenerateKnightMovesBlocked()
        {
        Board board = Board.ImportFromFEN("k7/8/2P5/8/3N4/1P3P2/8/7K w - - 0 1");
        List<Move> moves = MoveGenerator.GenerateKnightMove((3,3),board);
        Assert.AreEqual(5,moves.Count);
        }
        [TestMethod]
        public void GenerateKnightMovesBlockedWithAttacks()
        {
        Board board = Board.ImportFromFEN("k7/8/2P1n3/8/3N4/1P3P2/4p3/7K w - - 0 1");
        List<Move> moves = MoveGenerator.GenerateKnightMove((3,3),board);
        Assert.AreEqual(5,moves.Count);
        }
        [TestMethod]
        public void GenerateKnightMovesEdgeCase()
        {
        Board board = Board.ImportFromFEN("k7/8/4n3/7P/7N/1P5P/4p3/7K w - - 0 1");
        List<Move> moves = MoveGenerator.GenerateKnightMove((7,3),board);
        Assert.AreEqual(4,moves.Count);
        }
    }

}
