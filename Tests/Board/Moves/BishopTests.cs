using Chess;

namespace Moves
{
    [TestClass]
    public class BishopTests
    {

        [TestMethod]
        public void GenerateBishopMove()
        {
            Board board = Board.ImportFromFEN("7k/8/8/8/3B4/8/8/7K w - - 0 1");
            List<Move> moves = MoveGenerator.GenerateBishopMove((3, 3), board);
            Assert.AreEqual(13, moves.Count);
        }

        [TestMethod]
        public void GenerateBishopMoveBlocked()
        {
            Board board = Board.ImportFromFEN("k7/8/8/8/3B4/2R1R3/8/7K w - - 0 1");
            List<Move> moves = MoveGenerator.GenerateBishopMove((3, 3), board);
            Assert.AreEqual(7, moves.Count);
        }
        [TestMethod]
        public void GenerateBishopMoveAttack()
        {
            Board board = Board.ImportFromFEN("k7/8/8/8/3B4/2r1r3/8/7K w - - 0 1");
            List<Move> moves = MoveGenerator.GenerateBishopMove((3, 3), board);
            Assert.AreEqual(9, moves.Count);
        }
        [TestMethod]
        public void GenerateBishopMoveAtCorner()
        {
            Board board = Board.ImportFromFEN("k7/8/8/8/8/8/8/B6K w - - 0 1");
            List<Move> moves = MoveGenerator.GenerateBishopMove((0, 0), board);
            Assert.AreEqual(7, moves.Count);
        }

    }

}