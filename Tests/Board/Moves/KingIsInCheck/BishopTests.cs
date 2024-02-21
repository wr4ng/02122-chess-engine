using Chess;

namespace KingIsInCheck
{
    [TestClass]
    public class BishopTests
    {
        // [TestMethod]
        // public void BishopOneLegalMoveBlock()
        // {
        //     Board board = Board.ImportFromFEN("k7/8/8/8/4b3/8/4B3/7K w - - 0 1");
        //     List<Move> moves = MoveGenerator.GenerateBishopMove((4, 1), board);
        //     Assert.AreEqual(1, moves.Count);
        // }
        // [TestMethod]
        // public void BishopOneLegalMoveAttack()
        // {
        //     Board board = Board.ImportFromFEN("k7/8/8/3B4/4b3/8/8/7K w - - 0 1");
        //     List<Move> moves = MoveGenerator.GenerateBishopMove((3, 4), board);
        //     Assert.AreEqual(1, moves.Count);
        // }
        // [TestMethod]
        // public void BishopNoLegalMove()
        // {
        //     Board board = Board.ImportFromFEN("k7/8/8/8/4b3/1B6/8/7K w - - 0 1");
        //     List<Move> moves = MoveGenerator.GenerateBishopMove((1, 2), board);
        //     Assert.AreEqual(0, moves.Count);
        // }
        // [TestMethod]
        // public void BishopTwoLegalMoveAttackAndBlock()
        // {
        //     Board board = Board.ImportFromFEN("k7/8/8/8/4b3/8/6B1/7K w - - 0 1");
        //     List<Move> moves = MoveGenerator.GenerateBishopMove((6, 1), board);
        //     Assert.AreEqual(2, moves.Count);
        // }
    }
}