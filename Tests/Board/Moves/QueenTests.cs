using Chess;

namespace Moves
{
    [TestClass]
    public class QueenTests
    {
        [TestMethod]
        public void GenerateQueenMove()
        {
            Board board = Board.ImportFromFEN("k7/8/8/8/3Q4/8/8/7K w - - 0 1");
            List<Move> moves = MoveGenerator.GenerateQueenMove((3, 3), board);
            Assert.AreEqual(27, moves.Count);
        }
        [TestMethod]
        public void GenerateQueenMoveBlocked()
        {
            Board board = Board.ImportFromFEN("k7/8/8/4N3/3Q4/3R4/8/7K w - - 0 1");
            List<Move> moves = MoveGenerator.GenerateQueenMove((3, 3), board);
            Assert.AreEqual(20, moves.Count);
        }
        [TestMethod]
        public void GenerateQueenMoveAttack()
        {
            Board board = Board.ImportFromFEN("k7/8/8/4n3/3Q4/3r4/8/7K w - - 0 1");
            List<Move> moves = MoveGenerator.GenerateQueenMove((3, 3), board);
            Assert.AreEqual(22, moves.Count);
        }
        [TestMethod]
        public void GenerateQueenMoveAtCorner()
        {
            Board board = Board.ImportFromFEN("k7/r7/8/8/8/8/8/Q6K w - - 0 1");
            List<Move> moves = MoveGenerator.GenerateQueenMove((7, 0), board);
            Assert.AreEqual(19, moves.Count);
        }
    }
}