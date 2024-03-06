using System.ComponentModel;

namespace Chess
{
	public class Piece
	{
		private PieceType type;
		private Color color;

		public Piece(PieceType type, Color color)
		{
			this.type = type;
			this.color = color;
		}

		public PieceType GetPieceType()
		{
			return type;
		}

		public Color GetColor()
		{
			return color;
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
			return color == Color.White ? char.ToUpper(FENchar) : FENchar;
		}
	}

	public enum PieceType
	{
		Pawn   = 0b000001,
		Rook   = 0b000010,
		Knight = 0b000100,
		Bishop = 0b001000,
		Queen  = 0b010000,
		King   = 0b100000
	}

	public enum Color
	{
		White,
		Black
	}

	public static class ColorExtensions
	{
		public static Color Opposite(this Color color)
		{
			return color == Color.White ? Color.Black : Color.White;
		}
	}
}