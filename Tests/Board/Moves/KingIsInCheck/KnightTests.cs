using Chess;

namespace KingIsInCheck
{
    [TestClass]
    public class KnightTests
    {
        // [TestMethod]
        // public void KnightOneLegalMoveBlock()
        // {
        //     Board board = Board.ImportFromFEN("k7/8/8/8/4b3/4N3/8/7K w - - 0 1");
        //     List<Move> moves = MoveGenerator.GenerateKnightMove((4, 2), board);
        //     Assert.AreEqual(1, moves.Count);
        // }
        // [TestMethod]
        // public void KnightOneLegalMoveAttack()
        // {
        //     Board board = Board.ImportFromFEN("k7/8/8/8/4b3/8/5N2/7K w - - 0 1");
        //     List<Move> moves = MoveGenerator.GenerateKnightMove((5, 2), board);
        //     Assert.AreEqual(1, moves.Count);
        // }
        // [TestMethod]
        // public void KnightNoLegalMove()
        // {
        //     Board board = Board.ImportFromFEN("k6N/8/8/8/4b3/8/8/7K w - - 0 1");
        //     List<Move> moves = MoveGenerator.GenerateKnightMove((7, 7), board);
        //     Assert.AreEqual(0, moves.Count);
        // }
        // [TestMethod]
        // public void KnightTwoLegalMoveAttackAndBlock()
        // {
        //     Board board = Board.ImportFromFEN("k7/8/8/6N1/4b3/8/8/7K w - - 0 1");
        //     List<Move> moves = MoveGenerator.GenerateKnightMove((6, 4), board);
        //     Assert.AreEqual(2, moves.Count);
        // }
    }
}