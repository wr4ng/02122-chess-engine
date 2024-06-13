namespace Chess
{
	public class Util
	{
		public static bool InsideBoard(int file, int rank) => 0 <= file && file < 8 && 0 <= rank && rank < 8;
	}
}