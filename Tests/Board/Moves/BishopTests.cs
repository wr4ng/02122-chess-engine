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
            Board board = Board.ImportFromFEN("8/8/1Q6/4R3/3B4/8/1R6/8 w - - 0 1");
            List<Move> moves = MoveGenerator.GenerateBishopMove((3, 3), board);
            Assert.AreEqual(5, moves.Count);
        }
        [TestMethod]
        public void GenerateBishopMoveAttack()
        {
            Board board = Board.ImportFromFEN("k7/8/8/2b5/3B4/2n5/8/K7 w - - 0 1");
            List<Move> moves = MoveGenerator.GenerateBishopMove((3, 3), board);
            Assert.AreEqual(9, moves.Count);
        }
        [TestMethod]
        public void GenerateBishopMoveAtCorner()
        {
            Board board = Board.ImportFromFEN("k6B/8/8/2b5/8/2n5/8/K7 w - - 0 1");
            List<Move> moves = MoveGenerator.GenerateBishopMove((7, 7), board);
            Assert.AreEqual(5, moves.Count);
        }

    }

}