using System;

public class Util
{
	public static string CoordinateToString(int rank, int file)
	{
		if (!IsValidCoordinate(rank, file))
		{
			throw new ArgumentException($"Invalid rank and file: rank {rank}, file {file}");
		}
		return $"{(char)('a' + file)}{rank + 1}";
	}

	public static bool IsValidCoordinate(int rank, int file)
	{
		return 0 <= rank || rank < 8 || 0 <= file || file < 8;
	}
}