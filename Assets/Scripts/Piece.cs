using System.ComponentModel;

public class Piece
{
	private PieceType type;
	private PieceColor color;

	public Piece(PieceType type, PieceColor color)
	{
		this.type = type;
		this.color = color;
	}

	public override string ToString()
	{
		return color.ToString() + " " + type.ToString();
	}

	public char ToFENchar()
	{
		char FENchar = type switch
		{
			PieceType.Pawn => 'p',
			PieceType.Rook => 'r',
			PieceType.Knight => 'n',
			PieceType.Bishop => 'b',
			PieceType.Queen => 'q',
			PieceType.King => 'k',
			_ => throw new InvalidEnumArgumentException()
		};
		return color == PieceColor.White ? char.ToUpper(FENchar) : FENchar;
	}
}

public enum PieceType
{
	Pawn,
	Rook,
	Knight,
	Bishop,
	Queen,
	King
}

public enum PieceColor
{
	Black,
	White
}