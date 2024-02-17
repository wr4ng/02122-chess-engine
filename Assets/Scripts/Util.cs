using System;

public class Util
{
	static char[] files = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };

	public static string CoordinateToString(int rank, int file)
	{
		if (!IsValidCoordinate(rank, file))
		{
			throw new ArgumentException($"Invalid rank and file: rank {rank}, file {file}");
		}
		return $"{files[file]}{rank + 1}";
	}

	public static bool IsValidCoordinate(int rank, int file)
	{
		return rank < 0 || 8 <= rank || file < 0 || 8 <= file;
	}
}