using Chess;

namespace KingIsInCheck
{
    [TestClass]
    public class BishopTests
    {
        [TestMethod]
        public void BishopOneLegalMoveBlock()
        {
            Board board = Board.ImportFromFEN("k7/8/8/8/4b3/8/4B3/7K w - - 0 1");
            List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(board);
            List<Move> bishopMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.Bishop).ToList();
            Assert.AreEqual(1, bishopMoves.Count);
        }
        [TestMethod]
        public void BishopOneLegalMoveAttack()
        {
            Board board = Board.ImportFromFEN("k7/8/8/3B4/4b3/8/8/7K w - - 0 1");
            List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(board);
            List<Move> bishopMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.Bishop).ToList();
            Assert.AreEqual(1, bishopMoves.Count);
        }
        [TestMethod]
        public void BishopNoLegalMove()
        {
            Board board = Board.ImportFromFEN("k7/8/8/8/4b3/1B6/8/7K w - - 0 1");
            List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(board);
            List<Move> bishopMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.Bishop).ToList();
            Assert.AreEqual(0, bishopMoves.Count);
        }
        [TestMethod]
        public void BishopTwoLegalMoveAttackAndBlock()
        {
            Board board = Board.ImportFromFEN("k7/8/8/8/4b3/8/6B1/7K w - - 0 1");
            List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(board);
            List<Move> bishopMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.Bishop).ToList();
            Assert.AreEqual(2, bishopMoves.Count);
        }
    }
}