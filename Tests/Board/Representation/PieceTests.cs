namespace Representation;

[TestClass]
public class PieceTests
{
	[TestMethod]
	public void CreateWhitePawn()
	{
		Piece p = new Piece(PieceType.Pawn, Color.White);
		Assert.AreEqual("White Pawn", p.ToString());
	}

	[TestMethod]
	public void CreateBlackRook()
	{
		Piece p = new Piece(PieceType.Rook, Color.Black);
		Assert.AreEqual("Black Rook", p.ToString());
	}

	[TestMethod]
	public void CreateWhiteKnight()
	{
		Piece p = new Piece(PieceType.Knight, Color.White);
		Assert.AreEqual("White Knight", p.ToString());
	}

	[TestMethod]
	public void CreateBlackBishop()
	{
		Piece p = new Piece(PieceType.Bishop, Color.Black);
		Assert.AreEqual("Black Bishop", p.ToString());
	}

	[TestMethod]
	public void CreateWhiteQueen()
	{
		Piece p = new Piece(PieceType.Queen, Color.White);
		Assert.AreEqual("White Queen", p.ToString());
	}

	[TestMethod]
	public void CreateBlackKing()
	{
		Piece p = new Piece(PieceType.King, Color.Black);
		Assert.AreEqual("Black King", p.ToString());
	}

	[TestMethod]
	public void WhitePawnToFEN()
	{
		Piece p = new Piece(PieceType.Pawn, Color.White);
		Assert.AreEqual('P', p.ToFENchar());
	}

	[TestMethod]
	public void BlackRookToFEN()
	{
		Piece p = new Piece(PieceType.Rook, Color.Black);
		Assert.AreEqual('r', p.ToFENchar());
	}

	[TestMethod]
	public void WhiteKnightToFEN()
	{
		Piece p = new Piece(PieceType.Knight, Color.White);
		Assert.AreEqual('N', p.ToFENchar());
	}

	[TestMethod]
	public void BlackBishopToFEN()
	{
		Piece p = new Piece(PieceType.Bishop, Color.Black);
		Assert.AreEqual('b', p.ToFENchar());
	}

	[TestMethod]
	public void WhiteQueenToFEN()
	{
		Piece p = new Piece(PieceType.Queen, Color.White);
		Assert.AreEqual('Q', p.ToFENchar());
	}

	[TestMethod]
	public void BlackKingToFEN()
	{
		Piece p = new Piece(PieceType.King, Color.Black);
		Assert.AreEqual('k', p.ToFENchar());
	}
}