using Chess;

namespace KingIsInCheck
{
	[TestClass]
	public class RookTests
	{
		[TestMethod]
		public void RookOneLegalMoveBlock()
		{
			Board board = Board.ImportFromFEN("k7/8/8/8/4b3/8/3R4/7K w - - 0 1");
			List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(board);
			List<Move> rookMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.Rook).ToList();
			Assert.AreEqual(1, rookMoves.Count);
		}

		[TestMethod]
		public void RookOneLegalMoveAttack()
		{
			Board board = Board.ImportFromFEN("k7/8/4R3/8/4b3/8/8/7K w - - 0 1");
			List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(board);
			List<Move> rookMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.Rook).ToList();
			Assert.AreEqual(1, rookMoves.Count);
		}

		[TestMethod]
		public void RookNoLegalMove()
		{
			Board board = Board.ImportFromFEN("k7/8/3R4/8/4b3/8/8/7K w - - 0 1");
			List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(board);
			List<Move> rookMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.Rook).ToList();
			Assert.AreEqual(0, rookMoves.Count);
		}

		[TestMethod]
		public void RookTwoLegalMoveAttackAndBlock()
		{
			Board board = Board.ImportFromFEN("k7/8/8/8/4b3/8/4R3/7K w - - 0 1");
			List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(board);
			List<Move> rookMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.Rook).ToList();
			Assert.AreEqual(2, rookMoves.Count);
		}
	}
}