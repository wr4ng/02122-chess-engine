using Chess;

namespace KingIsInCheck
{
    [TestClass]
    public class KingTests
    {
        [TestMethod]
        public void KingOneLegalMoveEscape()
        {
            Board board = Board.ImportFromFEN("k7/8/8/8/4b3/8/8/r6K w - - 0 1");
            List<Move> moves = MoveGenerator.GenerateKingMove((7, 0), board);
            Assert.AreEqual(1, moves.Count);
        }
        [TestMethod]
        public void KingOneLegalMoveAttack()
        {
            Board board = Board.ImportFromFEN("k6r/8/8/8/8/8/6b1/r6K w - - 0 1");
            List<Move> moves = MoveGenerator.GenerateKingMove((7, 0), board);
            Assert.AreEqual(1, moves.Count);
        }
        [TestMethod]
        public void KingNoLegalMove()
        {
            Board board = Board.ImportFromFEN("k6r/8/8/8/8/5b2/8/r6K w - - 0 1");
            List<Move> moves = MoveGenerator.GenerateKingMove((7, 0), board);
            Assert.AreEqual(0, moves.Count);
        }
        [TestMethod]
        public void KingTwoLegalMoveAttackAndEscape()
        {
            Board board = Board.ImportFromFEN("k7/8/8/8/8/8/6b1/r6K w - - 0 1");
            List<Move> moves = MoveGenerator.GenerateKingMove((7, 0), board);
            Assert.AreEqual(2, moves.Count);
        }
    }
}