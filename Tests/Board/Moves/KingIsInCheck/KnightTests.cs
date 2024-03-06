using Chess;

namespace KingIsInCheck
{
    [TestClass]
    public class KnightTests
    {
        [TestMethod]
        public void KnightOneLegalMoveBlock()
        {
            Board board = Board.ImportFromFEN("k7/8/8/8/4b3/4N3/8/7K w - - 0 1");
            List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(board);
			List<Move> knightMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.Knight).ToList();
            Assert.AreEqual(1, knightMoves.Count);
        }

        [TestMethod]
        public void KnightOneLegalMoveAttack()
        {
            Board board = Board.ImportFromFEN("k7/8/8/8/4b3/8/5N2/7K w - - 0 1");
            List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(board);
			List<Move> knightMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.Knight).ToList();
            Assert.AreEqual(1, knightMoves.Count);
        }

        [TestMethod]
        public void KnightNoLegalMove()
        {
            Board board = Board.ImportFromFEN("k6N/8/8/8/4b3/8/8/7K w - - 0 1");
            List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(board);
			List<Move> knightMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.Knight).ToList();
            Assert.AreEqual(0, knightMoves.Count);
        }
        [TestMethod]
        public void KnightTwoLegalMoveAttackAndBlock()
        {
            Board board = Board.ImportFromFEN("k7/8/8/6N1/4b3/8/8/7K w - - 0 1");
            List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(board);
			List<Move> knightMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.Knight).ToList();
            Assert.AreEqual(2, knightMoves.Count);
        }
    }
}