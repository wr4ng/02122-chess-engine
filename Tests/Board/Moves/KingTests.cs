using Chess;

namespace Moves
{
    [TestClass]
    public class KingTests
    {

        [TestMethod]
        public void GenerateKingMove()
        {
            Board board = Board.ImportFromFEN("k7/8/8/8/3K4/8/8/8 w - - 0 1");
            List<Move> moves = MoveGenerator.GenerateKingMove((3, 3), board);
            Assert.AreEqual(8, moves.Count);
        }
        [TestMethod]
        public void GenerateKingMoveBlocked()
        {
            Board board = Board.ImportFromFEN("k7/8/8/8/3KB3/3N4/8/8 w - - 0 1");
            List<Move> moves = MoveGenerator.GenerateKingMove((3, 3), board);
            Assert.AreEqual(6, moves.Count);
        }
        [TestMethod]
        public void GenerateKingMoveAttack()
        {
            Board board = Board.ImportFromFEN("k7/8/8/8/3Kn3/3p4/8/8 w - - 0 1");
            List<Move> moves = MoveGenerator.GenerateKingMove((3, 3), board);
            Assert.AreEqual(8, moves.Count);
        }
        [TestMethod]
        public void GenerateKingMoveAtCorner()
        {
            Board board = Board.ImportFromFEN("k7/1N6/8/8/8/8/8/7K w - - 0 1");
            List<Move> moves = MoveGenerator.GenerateKingMove((7, 0), board);
            Assert.AreEqual(3, moves.Count);
        }
    }
}