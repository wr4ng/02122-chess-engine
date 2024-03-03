using Chess;

namespace Moves
{
    [TestClass]
    public class PawnTests
    {

        [TestMethod]
        public void GeneratePawnMoveTwoForward()
        {
            Board board = Board.ImportFromFEN("7k/8/8/8/8/8/7P/7K w - - 0 1");
            List<Move> moves = MoveGenerator.GeneratePawnMove((7, 1), board);
            Assert.AreEqual(2, moves.Count);
            Assert.AreEqual((7, 1), moves[0].GetStartSquare());
            Assert.AreEqual((7, 2), moves[0].GetEndSquare());
            Assert.AreEqual((7, 1), moves[1].GetStartSquare());
            Assert.AreEqual((7, 3), moves[1].GetEndSquare());
        }

        [TestMethod]
        public void GeneratePawnMoveOneForward()
        {
            Board board = Board.ImportFromFEN("7k/8/8/8/7p/8/7P/7K w - - 0 1");
            List<Move> moves = MoveGenerator.GeneratePawnMove((7, 1), board);
            Assert.AreEqual(1, moves.Count);
            Assert.AreEqual((7, 1), moves[0].GetStartSquare());
            Assert.AreEqual((7, 2), moves[0].GetEndSquare());
        }

        [TestMethod]
        public void GeneratePawnMoveNotStartingRank()
        {
            Board board = Board.ImportFromFEN("7k/8/8/8/8/7P/8/7K w - - 0 1");
            List<Move> moves = MoveGenerator.GeneratePawnMove((7, 2), board);
            Assert.AreEqual(1, moves.Count);
            Assert.AreEqual((7, 2), moves[0].GetStartSquare());
            Assert.AreEqual((7, 3), moves[0].GetEndSquare());
        }

        [TestMethod]
        public void GeneratePawnMoveNoMoves()
        {
            Board board = Board.ImportFromFEN("7k/8/8/8/8/7P/7P/7K w - - 0 1");
            List<Move> moves = MoveGenerator.GeneratePawnMove((7, 1), board);
            Assert.AreEqual(0, moves.Count);
        }

        [TestMethod]
        public void GeneratePawnMoveAllPossible()
        {
            Board board = Board.ImportFromFEN("7k/8/8/8/8/5p1p/6P1/7K w - - 0 1");
            List<Move> moves = MoveGenerator.GeneratePawnMove((6, 1), board);
            Assert.AreEqual(4, moves.Count);
            Assert.AreEqual((6, 1), moves[0].GetStartSquare());
            Assert.AreEqual((6, 2), moves[0].GetEndSquare());
            Assert.AreEqual((6, 1), moves[1].GetStartSquare());
            Assert.AreEqual((6, 3), moves[1].GetEndSquare());
            Assert.AreEqual((6, 1), moves[2].GetStartSquare());
            Assert.AreEqual((7, 2), moves[2].GetEndSquare());
            Assert.AreEqual((6, 1), moves[3].GetStartSquare());
            Assert.AreEqual((5, 2), moves[3].GetEndSquare());
        }

        [TestMethod]
        public void GeneratePawnMoveEdgeCases()
        {
            Board board = Board.ImportFromFEN("7k/8/8/8/8/6pp/7P/7K w - - 0 1");
            List<Move> moves = MoveGenerator.GeneratePawnMove((7, 1), board);
            Assert.AreEqual(1, moves.Count);
            Assert.AreEqual((7, 1), moves[0].GetStartSquare());
            Assert.AreEqual((6, 2), moves[0].GetEndSquare());
            board = Board.ImportFromFEN("7k/8/8/8/8/pp6/P7/7K w - - 0 1");
            moves = MoveGenerator.GeneratePawnMove((0, 1), board);
            Assert.AreEqual(1, moves.Count);
            Assert.AreEqual((0, 1), moves[0].GetStartSquare());
            Assert.AreEqual((1, 2), moves[0].GetEndSquare());
        }

        [TestMethod]
        public void GeneratePawnEnPassant()
        {
            Board board = Board.ImportFromFEN("7k/8/8/pP6/8/8/8/7K w - a6 0 1");
            List<Move> moves = MoveGenerator.GeneratePawnMove((1, 4), board);
            Assert.AreEqual(2, moves.Count);
            board = Board.ImportFromFEN("7k/8/8/1Pp5/8/8/8/7K w - c6 0 1");
            moves = MoveGenerator.GeneratePawnMove((1, 4), board);
            Assert.AreEqual(2, moves.Count);
        }

		[TestMethod]
		public void GeneratePawnSimplePromotion()
		{
			// White promotion
			Board board = Board.ImportFromFEN("k7/3P4/8/8/8/8/8/7K w - - 0 1");
			List<Move> promotionMoves = MoveGenerator.GeneratePawnMove((3, 6), board);
			Assert.AreEqual(4, promotionMoves.Count);

			// Black promotion
			board = Board.ImportFromFEN("k7/8/8/8/8/8/5p2/7K b - - 0 1");
			promotionMoves = MoveGenerator.GeneratePawnMove((5, 1), board);
			Assert.AreEqual(4, promotionMoves.Count);
		}

		//TODO Capture promotion tests
    }
}