using Chess;

namespace Moves
{
    [TestClass]
    public class RookTests
    {
        [TestMethod]
        public void GenerateRookMove()
        {
            Board board = Board.ImportFromFEN("7k/8/8/8/3R4/8/8/7K w - - 0 1");
            List<Move> moves = MoveGenerator.GenerateRookMove((3, 3), board);
            Assert.AreEqual(14, moves.Count);
        }
        [TestMethod]
        public void GenerateRookMoveBlocked()
        {
            Board board = Board.ImportFromFEN("k7/8/8/8/3RP3/3N4/8/7K w - - 0 1");
            List<Move> moves = MoveGenerator.GenerateRookMove((3, 3), board);
            Assert.AreEqual(7, moves.Count);
        }
        [TestMethod]
        public void GenerateRookMoveAttack()
        {
            Board board = Board.ImportFromFEN("k7/8/8/8/3Rp3/3n4/8/7K w - - 0 1");
            List<Move> moves = MoveGenerator.GenerateRookMove((3, 3), board);
            Assert.AreEqual(9, moves.Count);
        }
        [TestMethod]
        public void GenerateRookMoveAtCorner()
        {
            Board board = Board.ImportFromFEN("k7/8/8/8/4p3/n7/8/R6K w - - 0 1");
            List<Move> moves = MoveGenerator.GenerateRookMove((0, 0), board);
            Assert.AreEqual(8, moves.Count);
        }
    }

}