using Chess;

namespace Moves
{
	[TestClass]
	public class KingTests
	{

		[TestMethod]
		public void GenerateKingMove()
		{
			Board board = Board.ImportFromFEN("k7/8/8/8/3K4/8/8/8 w - - 0 1");
			List<Move> legalMoves = board.GetLegalMoves();
			List<Move> kingMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.King).ToList();
			Assert.AreEqual(8, kingMoves.Count);
		}

		[TestMethod]
		public void GenerateKingMoveBlocked()
		{
			Board board = Board.ImportFromFEN("k7/8/8/8/3KB3/3N4/8/8 w - - 0 1");
			List<Move> legalMoves = board.GetLegalMoves();
			List<Move> kingMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.King).ToList();
			Assert.AreEqual(6, kingMoves.Count);
		}

		[TestMethod]
		public void GenerateKingMoveAttack()
		{
			Board board = Board.ImportFromFEN("k7/8/8/8/4K3/4pp2/8/8 w - - 0 1");
			List<Move> legalMoves = board.GetLegalMoves();
			List<Move> kingMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.King).ToList();
			Assert.AreEqual(8, kingMoves.Count);
		}

		[TestMethod]
		public void GenerateKingMoveAtCorner()
		{
			Board board = Board.ImportFromFEN("k7/1N6/8/8/8/8/8/7K w - - 0 1");
			List<Move> legalMoves = board.GetLegalMoves();
			List<Move> kingMoves = legalMoves.Where(move => move.GetPieceType() == PieceType.King).ToList();
			Assert.AreEqual(3, kingMoves.Count);
		}

		//TODO Tests that castlingrights correctly updates
	}
}