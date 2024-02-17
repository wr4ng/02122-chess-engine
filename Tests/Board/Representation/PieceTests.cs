namespace Representation;

[TestClass]
public class PieceTests
{
	[TestMethod]
	public void CreateWhitePawn()
	{
		Piece p = new Piece(PieceType.Pawn, PieceColor.White);
		Assert.AreEqual("White Pawn", p.toString());
	}

	[TestMethod]
	public void CreateBlackRook()
	{
		Piece p = new Piece(PieceType.Rook, PieceColor.Black);
		Assert.AreEqual("Black Rook", p.toString());
	}

	[TestMethod]
	public void CreateWhiteKnight()
	{
		Piece p = new Piece(PieceType.Knight, PieceColor.White);
		Assert.AreEqual("White Knight", p.toString());
	}

	[TestMethod]
	public void CreateBlackBishop()
	{
		Piece p = new Piece(PieceType.Bishop, PieceColor.Black);
		Assert.AreEqual("Black Bishop", p.toString());
	}

	[TestMethod]
	public void CreateWhiteQueen()
	{
		Piece p = new Piece(PieceType.Queen, PieceColor.White);
		Assert.AreEqual("White Queen", p.toString());
	}

	[TestMethod]
	public void CreateBlackKing()
	{
		Piece p = new Piece(PieceType.King, PieceColor.Black);
		Assert.AreEqual("Black King", p.toString());
	}

	[TestMethod]
	public void WhitePawnToFEN()
	{
		Piece p = new Piece(PieceType.Pawn, PieceColor.White);
		Assert.AreEqual('P', p.toFENchar());
	}

	[TestMethod]
	public void BlackRookToFEN()
	{
		Piece p = new Piece(PieceType.Rook, PieceColor.Black);
		Assert.AreEqual('r', p.toFENchar());
	}

	[TestMethod]
	public void WhiteKnightToFEN()
	{
		Piece p = new Piece(PieceType.Knight, PieceColor.White);
		Assert.AreEqual('N', p.toFENchar());
	}

	[TestMethod]
	public void BlackBishopToFEN()
	{
		Piece p = new Piece(PieceType.Bishop, PieceColor.Black);
		Assert.AreEqual('b', p.toFENchar());
	}

	[TestMethod]
	public void WhiteQueenToFEN()
	{
		Piece p = new Piece(PieceType.Queen, PieceColor.White);
		Assert.AreEqual('Q', p.toFENchar());
	}

	[TestMethod]
	public void BlackKingToFEN()
	{
		Piece p = new Piece(PieceType.King, PieceColor.Black);
		Assert.AreEqual('k', p.toFENchar());
	}
}