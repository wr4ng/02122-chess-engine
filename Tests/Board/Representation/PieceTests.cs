using Chess;

namespace Representation;

[TestClass]
public class PieceTests
{
	[TestMethod]
	public void CreatePieces()
	{
		Assert.AreEqual("White Pawn", new Piece(PieceType.Pawn, Color.White).ToString());
		Assert.AreEqual("White Knight", new Piece(PieceType.Knight, Color.White).ToString());
		Assert.AreEqual("White Queen", new Piece(PieceType.Queen, Color.White).ToString());
		Assert.AreEqual("Black Bishop", new Piece(PieceType.Bishop, Color.Black).ToString());
		Assert.AreEqual("Black Rook", new Piece(PieceType.Rook, Color.Black).ToString());
		Assert.AreEqual("Black King", new Piece(PieceType.King, Color.Black).ToString());
	}

	[TestMethod]
	public void SinglePieceToFEN()
	{
		Assert.AreEqual('P', new Piece(PieceType.Pawn, Color.White).ToFENchar());
		Assert.AreEqual('N', new Piece(PieceType.Knight, Color.White).ToFENchar());
		Assert.AreEqual('Q', new Piece(PieceType.Queen, Color.White).ToFENchar());
		Assert.AreEqual('b', new Piece(PieceType.Bishop, Color.Black).ToFENchar());
		Assert.AreEqual('r', new Piece(PieceType.Rook, Color.Black).ToFENchar());
		Assert.AreEqual('k', new Piece(PieceType.King, Color.Black).ToFENchar());
	}
}