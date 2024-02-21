using Chess;

namespace Moves
{
    [TestClass]
    public class BishopTests{

        [TestMethod]
        public void GenerateBishopMove()
        {
            Board board = Board.ImportFromFEN("7k/8/8/8/3B4/8/8/7K w - - 0 1");
            List<Move> moves = MoveGenerator.GenerateBishopMove((3,3),board);
            Assert.AreEqual(13,moves.Count);
        }

        [TestMethod]
        public void GenerateBishopMoveBlocked()
        {
            Board board = Board.ImportFromFEN("8/8/1Q6/4R3/3B4/8/1R6/8 w - - 0 1");
            List<Move> moves = MoveGenerator.GenerateBishopMove((3,3),board);
            Assert.AreEqual(5,moves.Count);
        }

    }

}