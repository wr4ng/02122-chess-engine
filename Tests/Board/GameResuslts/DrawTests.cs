using Chess;

namespace ResultTests
{
	[TestClass]
	public class DrawTests
	{

		[TestMethod]
		public void AddStartPosition()
		{
			Board board = Board.DefaultBoard();
			Assert.AreEqual(1, board.draw.getPositionCount()["rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq -"]);
		}

		[TestMethod]
		public void SetHalfMoveClock()
		{
			Board board = Board.ImportFromFEN("8/5k2/3p4/1p1Pp2p/pP2Pp1P/P4P1K/8/8 b - - 84 6");
			Assert.AreEqual(84, board.draw.getHalfMoveClock());
		}

		[TestMethod]
		public void PawnPushHalfMoveClock()
		{
			Board board = Board.ImportFromFEN("rnbk1bnr/ppq1pppp/3p4/2p5/5P2/2P1P3/PP1P2PP/RNBQKBNR b - - 84 6");
			// Make move to update halfMoveClock
			List<Move> legalMoves = board.GetLegalMoves();
			List<Move> pawnMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.Pawn).ToList();
			board.PlayMove(pawnMoves[0]);
			
			// Check if halfMoveClock is 0 after move updated
			Assert.AreEqual(0, board.draw.getHalfMoveClock());
		}

		[TestMethod]
		public void DrawFromRepetion()
		{
			//Make board
			Board board = Board.ImportFromFEN("kp6/1p6/pp6/8/8/6PP/6P1/6PK b - - 84 6");
			Assert.AreEqual(1, board.draw.getPositionCount().Count);

			//Get king moves
			List<Move> legalMoves = board.GetLegalMoves();
			List<Move> kingMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.King).ToList();

			//Play king move
			board.PlayMove(kingMoves[0]);
			//Check if position count is updated
			Assert.AreEqual(2, board.draw.getPositionCount().Count);
			Assert.AreEqual(1, board.draw.getPositionCount()["1p6/kp6/pp6/8/8/6PP/6P1/6PK w - -"]);

			//Get king moves
			legalMoves = board.GetLegalMoves();
			kingMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.King).ToList();

			//Play king move
			board.PlayMove(kingMoves[0]);
			//Check if position count is updated
			Assert.AreEqual(3, board.draw.getPositionCount().Count);
			Assert.AreEqual(1, board.draw.getPositionCount()["1p6/kp6/pp6/8/8/6PP/6PK/6P1 b - -"]);

			//Get king moves
			legalMoves = board.GetLegalMoves();
			kingMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.King).ToList();

			//Play king move
			board.PlayMove(kingMoves[0]);
			//Check if position count is updated
			Assert.AreEqual(4, board.draw.getPositionCount().Count);
			
			//Get king moves
			legalMoves = board.GetLegalMoves();
			kingMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.King).ToList();

			//Play king move
			board.PlayMove(kingMoves[0]);
			//Check if position count is updated
			Assert.AreEqual(4, board.draw.getPositionCount().Count);
			Assert.AreEqual(2, board.draw.getPositionCount()["kp6/1p6/pp6/8/8/6PP/6P1/6PK b - -"]);

			//Get king moves
			legalMoves = board.GetLegalMoves();
			kingMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.King).ToList();

			//Play king move
			board.PlayMove(kingMoves[0]);
			//Check if position count is updated
			Assert.AreEqual(4, board.draw.getPositionCount().Count);
			Assert.AreEqual(2, board.draw.getPositionCount()["1p6/kp6/pp6/8/8/6PP/6P1/6PK w - -"]);

			//Get king moves
			legalMoves = board.GetLegalMoves();
			kingMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.King).ToList();

			//Play king move
			board.PlayMove(kingMoves[0]);
			//Check if position count is updated
			Assert.AreEqual(4, board.draw.getPositionCount().Count);
			Assert.AreEqual(2, board.draw.getPositionCount()["1p6/kp6/pp6/8/8/6PP/6PK/6P1 b - -"]);
			Assert.AreEqual("1p6/kp6/pp6/8/8/6PP/6PK/6P1 b - - 90 6", board.ExportToFEN());

			//Get king moves
			legalMoves = board.GetLegalMoves();
			kingMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.King).ToList();

			//Play king move
			board.PlayMove(kingMoves[0]);
			//Check if position count is updated
			Assert.AreEqual(2, board.draw.getPositionCount()["kp6/1p6/pp6/8/8/6PP/6PK/6P1 w - -"]);

			//Get king moves
			legalMoves = board.GetLegalMoves();
			kingMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.King).ToList();

			//Play king move
			board.PlayMove(kingMoves[0]);
			//Check if position count is updated
			Assert.AreEqual(4, board.draw.getPositionCount().Count);
			Assert.AreEqual(3, board.draw.getPositionCount()["kp6/1p6/pp6/8/8/6PP/6P1/6PK b - -"]);

			//Get king moves
			Assert.IsTrue(board.draw.getIsDraw());
		}
	}
}