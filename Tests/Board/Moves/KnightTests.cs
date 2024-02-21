using Chess;

namespace Moves
{
    [TestClass]
    public class KnightTests
    {

        [TestMethod]
        public void GenerateKnightMove()
        {
            Board board = Board.ImportFromFEN("7k/8/8/8/3N4/8/8/7K w - - 0 1");
            List<Move> moves = MoveGenerator.GenerateKnightMove((3, 3), board);
            Assert.AreEqual(8, moves.Count);
        }
        [TestMethod]
        public void GenerateKnightMovesBlocked()
        {
            Board board = Board.ImportFromFEN("k7/8/4N3/5P2/3N4/5R2/8/7K w - - 0 1");
            List<Move> moves = MoveGenerator.GenerateKnightMove((3, 3), board);
            Assert.AreEqual(5, moves.Count);
        }
        [TestMethod]
        public void GenerateKnightMovesAttacks()
        {
            Board board = Board.ImportFromFEN("k7/8/4N3/5p2/3N4/5r2/8/7K w - - 0 1");
            List<Move> moves = MoveGenerator.GenerateKnightMove((3, 3), board);
            Assert.AreEqual(7, moves.Count);
        }
        [TestMethod]
        public void GenerateKnightMovesCorner()
        {
            Board board = Board.ImportFromFEN("k7/8/4r3/8/8/8/8/N6K w - - 0 1");
            List<Move> moves = MoveGenerator.GenerateKnightMove((0, 0), board);
            Assert.AreEqual(2, moves.Count);
        }
    }

}
