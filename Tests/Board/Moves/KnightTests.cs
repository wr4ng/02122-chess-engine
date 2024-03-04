using Chess;

namespace Moves
{
    [TestClass]
    public class KnightTests
    {

        [TestMethod]
        public void GenerateKnightMove()
        {
            Board board = Board.ImportFromFEN("7k/8/8/8/3N4/8/8/7K w - - 0 1");
            List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(board);
			List<Move> knightMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.Knight).ToList();
            Assert.AreEqual(8, knightMoves.Count);
        }

        [TestMethod]
        public void GenerateKnightMovesBlocked()
        {
            Board board = Board.ImportFromFEN("k7/8/4N3/5P2/3N4/5R2/8/7K w - - 0 1");
            List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(board);
			List<Move> knightMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.Knight).ToList();
            Assert.AreEqual(12, knightMoves.Count);
        }

        [TestMethod]
        public void GenerateKnightMovesAttacks()
        {
            Board board = Board.ImportFromFEN("k7/8/4N3/5p2/3N4/5r2/8/7K w - - 0 1");
            List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(board);
			List<Move> knightMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.Knight).ToList();
            Assert.AreEqual(14, knightMoves.Count);
        }

        [TestMethod]
        public void GenerateKnightMovesCorner()
        {
            Board board = Board.ImportFromFEN("k7/8/4r3/8/8/8/8/N6K w - - 0 1");
            List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(board);
			List<Move> knightMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.Knight).ToList();
            Assert.AreEqual(2, knightMoves.Count);
        }
    }
}