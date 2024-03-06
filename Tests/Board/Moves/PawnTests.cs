using Chess;

namespace Moves
{
    [TestClass]
    public class PawnTests
    {
		// TODO Create Util method to extract pawn moves (possibly for each type?)

        [TestMethod]
        public void GeneratePawnMoveTwoForward()
        {
            Board board = Board.ImportFromFEN("7k/8/8/8/8/8/7P/7K w - - 0 1");
            List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(board);
			List<Move> pawnMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.Pawn).ToList();

            Assert.AreEqual(2, pawnMoves.Count);
            Assert.AreEqual((7, 1), pawnMoves[0].GetStartSquare());
            Assert.AreEqual((7, 2), pawnMoves[0].GetEndSquare());
            Assert.AreEqual((7, 1), pawnMoves[1].GetStartSquare());
            Assert.AreEqual((7, 3), pawnMoves[1].GetEndSquare());
        }

        [TestMethod]
        public void GeneratePawnMoveOneForward()
        {
            Board board = Board.ImportFromFEN("7k/8/8/8/7p/8/7P/7K w - - 0 1");
            List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(board);
			List<Move> pawnMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.Pawn).ToList();

            Assert.AreEqual(1, pawnMoves.Count);
            Assert.AreEqual((7, 1), pawnMoves[0].GetStartSquare());
            Assert.AreEqual((7, 2), pawnMoves[0].GetEndSquare());
        }

        [TestMethod]
        public void GeneratePawnMoveNotStartingRank()
        {
            Board board = Board.ImportFromFEN("7k/8/8/8/8/7P/8/7K w - - 0 1");
            List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(board);
			List<Move> pawnMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.Pawn).ToList();

            Assert.AreEqual(1, pawnMoves.Count);
            Assert.AreEqual((7, 2), pawnMoves[0].GetStartSquare());
            Assert.AreEqual((7, 3), pawnMoves[0].GetEndSquare());
        }

        [TestMethod]
        public void GeneratePawnMoveNoMoves()
        {
            Board board = Board.ImportFromFEN("7k/8/8/8/8/7P/7P/7K w - - 0 1");
            List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(board);
			List<Move> pawnMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.Pawn && move.GetStartSquare().rank == 1).ToList();
            Assert.AreEqual(0, pawnMoves.Count);
        }

        [TestMethod]
        public void GeneratePawnMoveAllPossible()
        {
            Board board = Board.ImportFromFEN("7k/8/8/8/8/5p1p/6P1/7K w - - 0 1");
            List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(board);
			List<Move> pawnMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.Pawn).ToList();

            Assert.AreEqual(4, pawnMoves.Count);
            Assert.AreEqual((6, 1), pawnMoves[0].GetStartSquare());
            Assert.AreEqual((6, 2), pawnMoves[0].GetEndSquare());
            Assert.AreEqual((6, 1), pawnMoves[1].GetStartSquare());
            Assert.AreEqual((6, 3), pawnMoves[1].GetEndSquare());
            Assert.AreEqual((6, 1), pawnMoves[2].GetStartSquare());
            Assert.AreEqual((7, 2), pawnMoves[2].GetEndSquare());
            Assert.AreEqual((6, 1), pawnMoves[3].GetStartSquare());
            Assert.AreEqual((5, 2), pawnMoves[3].GetEndSquare());
        }

        [TestMethod]
        public void GeneratePawnMoveEdgeCases()
        {
            Board board = Board.ImportFromFEN("7k/8/8/8/8/6pp/7P/7K w - - 0 1");
            List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(board);
			List<Move> pawnMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.Pawn).ToList();

            Assert.AreEqual(1, pawnMoves.Count);
            Assert.AreEqual((7, 1), pawnMoves[0].GetStartSquare());
            Assert.AreEqual((6, 2), pawnMoves[0].GetEndSquare());

            board = Board.ImportFromFEN("7k/8/8/8/8/pp6/P7/7K w - - 0 1");
            legalMoves = MoveGenerator.GenerateLegalMoves(board);
			pawnMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.Pawn).ToList();

            Assert.AreEqual(1, pawnMoves.Count);
            Assert.AreEqual((0, 1), pawnMoves[0].GetStartSquare());
            Assert.AreEqual((1, 2), pawnMoves[0].GetEndSquare());
        }

        [TestMethod]
        public void GeneratePawnEnPassant()
        {
            Board board = Board.ImportFromFEN("7k/8/8/pP6/8/8/8/7K w - a6 0 1");
            List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(board);
			List<Move> enPassantMoves = legalMoves.Where(move => move.IsEnPassant()).ToList();
            Assert.AreEqual(1, enPassantMoves.Count);

            board = Board.ImportFromFEN("7k/8/8/1Pp5/8/8/8/7K w - c6 0 1");
            legalMoves = MoveGenerator.GenerateLegalMoves(board);
			enPassantMoves = legalMoves.Where(move => move.IsEnPassant()).ToList();
            Assert.AreEqual(1, enPassantMoves.Count);
        }

		[TestMethod]
		public void GeneratePawnSimplePromotion()
		{
			// White promotion
			Board board = Board.ImportFromFEN("k7/3P4/8/8/8/8/8/7K w - - 0 1");
            List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(board);
			List<Move> promotionMoves = legalMoves.Where(move => move.IsPromotion()).ToList();
			Assert.AreEqual(4, promotionMoves.Count);

			// Black promotion
			board = Board.ImportFromFEN("k7/8/8/8/8/8/5p2/7K b - - 0 1");
            legalMoves = MoveGenerator.GenerateLegalMoves(board);
			promotionMoves = legalMoves.Where(move => move.IsPromotion()).ToList();
			Assert.AreEqual(4, promotionMoves.Count);
		}

		[TestMethod]
		public void GeneratePawnCapturePromotion()
		{
			Board board = Board.ImportFromFEN("2r4k/1P6/8/8/8/8/8/K7 w - - 0 1");
            List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(board);
			List<Move> promotionCaptures = legalMoves.Where(move => move.IsPromotion() && move.IsCapture()).ToList();
			Assert.AreEqual(4, promotionCaptures.Count);
		}
    }
}