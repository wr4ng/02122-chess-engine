using Chess;

namespace KingIsInCheck
{
	[TestClass]
	public class KingTests
	{
		[TestMethod]
		public void KingOneLegalMoveEscape()
		{
			Board board = Board.ImportFromFEN("k7/8/8/8/4b3/8/8/r6K w - - 0 1");
			List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(board);
			List<Move> kingMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.King).ToList();
			Assert.AreEqual(1, kingMoves.Count);
		}
		[TestMethod]
		public void KingOneLegalMoveAttack()
		{
			Board board = Board.ImportFromFEN("k6r/8/8/8/8/8/6b1/r6K w - - 0 1");
			List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(board);
			List<Move> kingMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.King).ToList();
			Assert.AreEqual(1, kingMoves.Count);
		}
		[TestMethod]
		public void KingNoLegalMove()
		{
			Board board = Board.ImportFromFEN("k6r/8/8/8/8/5b2/8/r6K w - - 0 1");
			List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(board);
			List<Move> kingMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.King).ToList();
			Assert.AreEqual(0, kingMoves.Count);
		}
		[TestMethod]
		public void KingTwoLegalMoveAttackAndEscape()
		{
			Board board = Board.ImportFromFEN("k7/8/8/8/8/8/6b1/r6K w - - 0 1");
			List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(board);
			List<Move> kingMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.King).ToList();
			Assert.AreEqual(2, kingMoves.Count);
		}
	}
}