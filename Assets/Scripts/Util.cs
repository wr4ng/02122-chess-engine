using System;
using Chess;

public class Util
{
	public static string CoordinateToString(int file, int rank)
	{
		if (!IsValidCoordinate(file, rank))
		{
			throw new ArgumentException($"Invalid rank and file: file {file}, rank {rank}");
		}
		return $"{(char)('a' + file)}{rank + 1}";
	}

	public static bool IsValidCoordinate(int file, int rank)
	{
		return 0 <= file || file < Board.BOARD_SIZE || 0 <= rank || rank < Board.BOARD_SIZE;
	}
}