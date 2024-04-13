namespace Chess
{

	public static class NewPiece
	{
		public const int None = 0;
		public const int Pawn = 1;
		public const int Bishop = 2;
		public const int Knight = 3;
		public const int Rook = 4;
		public const int Queen = 5;
		public const int King = 6;

		public const int White = 8;
		public const int Black = 16;

		// Get least significant three bits
		public static int Type(int piece) => piece & 0b00111;

		// Get most significant two bits
		public static int Color(int piece) => piece & 0b11000;

		// XY--- & 0b11000 = XY000. Check if that is equal to color
		public static bool IsColor(int piece, int color) => Color(piece) == color;
	}
}