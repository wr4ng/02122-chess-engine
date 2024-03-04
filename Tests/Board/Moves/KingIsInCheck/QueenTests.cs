using Chess;

namespace KingIsInCheck
{
    [TestClass]
    public class QueenTests
    {
        [TestMethod]
        public void QueenOneLegalMoveBlock()
        {
            Board board = Board.ImportFromFEN("k7/8/8/8/4b3/8/8/6QK w - - 0 1");
            List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(board);
			List<Move> queenMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.Queen).ToList();
            Assert.AreEqual(1, queenMoves.Count);
        }

        [TestMethod]
        public void QueenOneLegalMoveAttack()
        {
            Board board = Board.ImportFromFEN("k7/8/8/8/4b3/8/8/4Q2K w - - 0 1");
            List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(board);
			List<Move> queenMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.Queen).ToList();
            Assert.AreEqual(1, queenMoves.Count);
        }
        [TestMethod]
        public void QueenNoLegalMove()
        {
            Board board = Board.ImportFromFEN("k7/2Q5/8/8/4b3/8/8/7K w - - 0 1");
            List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(board);
			List<Move> queenMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.Queen).ToList();
            Assert.AreEqual(0, queenMoves.Count);
        }

        [TestMethod]
        public void QueenTwoLegalMoveAttackAndBlock()
        {
            Board board = Board.ImportFromFEN("k7/8/6Q1/8/4b3/8/8/7K w - - 0 1");
            List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(board);
			List<Move> queenMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.Queen).ToList();
            Assert.AreEqual(2, queenMoves.Count);
        }
    }
}