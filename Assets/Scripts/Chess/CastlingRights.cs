using System;

namespace Chess
{
	[Flags]
	public enum CastlingRights
	{
		None = 0b0000,
		WhiteKingside = 0b0001,
		WhiteQueenside = 0b0010,
		BlackKingside = 0b0100,
		BlackQueenside = 0b1000,
		AllWhite = WhiteKingside | WhiteQueenside,
		AllBlack = BlackKingside | BlackQueenside,
		All = AllWhite | AllBlack
	}

	public static class CastlingRightsExtensions
	{
		public static CastlingRights ClearRights(this CastlingRights castlingRights, CastlingRights flags)
		{
			return castlingRights & ~flags;
		}

		public static string ToFEN(this CastlingRights castlingRights)
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
}