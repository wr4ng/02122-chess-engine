using Chess;

namespace KingIsInCheck
{
	[TestClass]
	public class PawnTests
	{
		[TestMethod]
		public void PawnOneLegalMoveBlock()
		{
			Board board = Board.ImportFromFEN("k7/8/8/8/4b3/8/5P2/7K w - - 0 1");
			List<Move> legalMoves = board.GetLegalMoves();
			List<Move> pawnMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.Pawn).ToList();
			Assert.AreEqual(1, pawnMoves.Count);
		}

		[TestMethod]
		public void PawnOneLegalMoveAttack()
		{
			Board board = Board.ImportFromFEN("k7/8/8/8/8/5b2/6P1/7K w - - 0 1");
			List<Move> legalMoves = board.GetLegalMoves();
			List<Move> pawnMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.Pawn).ToList();
			Assert.AreEqual(1, pawnMoves.Count);
		}

		[TestMethod]
		public void PawnNoLegalMove()
		{
			Board board = Board.ImportFromFEN("k7/8/8/8/4b3/8/4P3/7K w - - 0 1");
			List<Move> legalMoves = board.GetLegalMoves();
			List<Move> pawnMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.Pawn).ToList();
			Assert.AreEqual(0, pawnMoves.Count);
		}
	}
}