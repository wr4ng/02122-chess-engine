namespace Chess
{
	public static class BitBoard
	{
		public static bool HasOne(ulong bitboard, int file, int rank)
		{
			return ((bitboard >> (rank * 8 + file)) & 1) != 0;
		}

		public static ulong SetOne(ulong bitboard, int file, int rank)
		{
			return bitboard | ((ulong)1 << (rank * 8 + file));
		}
	}
}