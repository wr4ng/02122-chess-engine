using System;

[Flags]
public enum CastlingRights
{
	None = 0b0000,
	WhiteKingside = 0b0001,
	WhiteQueenside = 0b0010,
	BlackKingside = 0b0100,
	BlackQueenside = 0b1000
}

public static class CastlingRightsExtensions
{
	public static string ToFENString(this CastlingRights castlingRights)
	{
		string result = "";
		if (castlingRights.HasFlag(CastlingRights.WhiteKingside))
		{
			result += "K";
		}
		if (castlingRights.HasFlag(CastlingRights.WhiteQueenside))
		{
			result += "Q";
		}
		if (castlingRights.HasFlag(CastlingRights.BlackKingside))
		{
			result += "k";
		}
		if (castlingRights.HasFlag(CastlingRights.BlackQueenside))
		{
			result += "q";
		}
		if (result == "")
		{
			result = "-";
		}
		return result;
	}
}