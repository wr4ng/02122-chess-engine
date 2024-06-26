using Chess;

namespace Moves
{
	[TestClass]
	public class enPassantTests
	{
		[TestMethod]
		public void EnPassantSetSquare()
		{
			// Default position
			Board board = Board.FromFEN(FEN.STARTING_POSITION_FEN);
			// Assert EP square is empty before move
			Assert.AreEqual((-1, -1), board.enPassantSquare);
			// Perform double forward pawn move (d2 -> d4)
			Move move = new Move((3, 1), (3, 3));
			board.MakeMove(move);
			// Assert EP square is d3 after move
			Assert.AreEqual((3, 2), board.enPassantSquare);
			// Check EP square is correctly reset after undoing previous move
			board.UndoPreviousMove();
			Assert.AreEqual((-1, -1), board.enPassantSquare);
		}

		[TestMethod]
		public void EnPassantSetSquare2()
		{
			Board board = Board.FromFEN(FEN.STARTING_POSITION_FEN);
			Assert.AreEqual((-1, -1), board.enPassantSquare);

			// White Pawn d2 -> d4
			Move pawnMove = new Move((3, 1), (3, 3));
			board.MakeMove(pawnMove);
			Assert.AreEqual((3, 2), board.enPassantSquare);

			// Black Knight g8 -> f6
			Move knightMove = new Move((6, 7), (5, 5));
			board.MakeMove(knightMove);
			Assert.AreEqual((-1, -1), board.enPassantSquare);

			board.UndoPreviousMove();
			Assert.AreEqual((3, 2), board.enPassantSquare);
		}
	}
}