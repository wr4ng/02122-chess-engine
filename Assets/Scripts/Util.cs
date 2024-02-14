public class Util
{
	static char[] files = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };

	public static string CoordinateToString(int rank, int file)
	{
		if (rank < 0 || 8 <= rank || file < 0 || 8 <= file)
		{
			// TODO Handle invalid coordinate
			return "??";
		}
		return $"{files[file]}{rank + 1}";
	}
}