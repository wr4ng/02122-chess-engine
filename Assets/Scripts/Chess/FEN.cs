using System;
using System.Linq;

namespace Chess
{
	public class FEN
	{
		public const string STARTING_POSITION_FEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"; //TODO ret sikker på at det sidste skal være 0

		public static string CoordinateToFEN(int file, int rank) //TODO thats no FEN is it er det ikke bare til string?
		{
			if (!Util.InsideBoard(file, rank))
			{
				throw new ArgumentException($"Invalid rank and file: file {file}, rank {rank}");
			}
			return $"{(char)('a' + file)}{rank + 1}";
		}

		public static string CoordinateToFEN((int file, int rank) coordinate)
		{
			return CoordinateToFEN(coordinate.file, coordinate.rank);
		}

		public static CastlingRights ParseCastlingRights(string fen)
		{
			CastlingRights castlingRights = CastlingRights.None;
			// Validate input
			// TODO Validate order of castling rights?
			bool validLength = 0 <= fen.Length && fen.Length <= 4;
			bool uniqueCharacters = fen == new String(fen.Distinct().ToArray());
			if (!validLength || !uniqueCharacters)
			{
				throw new ArgumentException($"Invalid FEN string (invalid castling rights): {fen}");
			}
			// Parse characters
			foreach (char c in fen)
			{
				castlingRights |= c switch
				{
					'K' => CastlingRights.WhiteKingside,
					'Q' => CastlingRights.WhiteQueenside,
					'k' => CastlingRights.BlackKingside,
					'q' => CastlingRights.BlackQueenside,
					'-' => CastlingRights.None,
					_ => throw new ArgumentException($"Invalid FEN string (invalid castling rights): {fen}")
				};
			}
			return castlingRights;
		}

		public static (int, int) ParseEnPassant(string fen)
		{
			if (fen == "-")
			{
				return (-1, -1);
			}
			int file = fen[0] switch
			{
				'a' => 0,
				'b' => 1,
				'c' => 2,
				'd' => 3,
				'e' => 4,
				'f' => 5,
				'g' => 6,
				'h' => 7,
				_ => throw new ArgumentException($"Invalid FEN string (invalid file): {fen}")
			};
			if (!int.TryParse(fen[1].ToString(), out int rank))
			{
				throw new ArgumentException($"Invalid FEN string (invalid rank): {fen}");
			}
			if (rank != 3 && rank != 6)
			{
				throw new ArgumentException($"Invalid FEN string (invalid rank): {fen}");
			}
			return (file, rank - 1);
		}

		public static int ParseHalfmoveClock(string fen)
		{
			if (int.TryParse(fen, out int halfmove))
			{
				return halfmove;
			}
			else
			{
				throw new ArgumentException($"Invalid FEN string (invalid halfmove number): {fen}");
			}
		}

		public static int ParseFullmoveNumber(string fen)
		{
			if (int.TryParse(fen, out int fullmove))
			{
				return fullmove;
			}
			else
			{
				throw new ArgumentException($"Invalid FEN string (invalid fullmove number): {fen}");
			}
		}

		public static string BoardToFEN(Board board)
		{
			string fen = "";
			for (int rank = 7; rank >= 0; rank--)
			{
				int emptyTiles = 0;
				for (int file = 0; file < 8; file++)
				{
					if (board.squares[file, rank] == Piece.None)
					{
						emptyTiles++;
					}
					else
					{
						if (emptyTiles > 0)
						{
							fen += emptyTiles;
							emptyTiles = 0;
						}
						fen += Piece.ToString(board.squares[file, rank]);
					}
				}
				// Add number of empty space at end of rank if any
				if (emptyTiles != 0)
				{
					fen += emptyTiles;
				}
				// Add seperator between ranks
				if (rank != 0)
				{
					fen += "/";
				}
			}
			return fen;
		}

		public static string ColorToFEN(int color)
		{
			return Piece.IsColor(color, Piece.White) ? "w" : "b";
		}

		public static string EnPassantToFEN((int, int) enPassantSquare)
		{
			return enPassantSquare == (-1, -1) ? "-" : CoordinateToFEN(enPassantSquare);
		}
	}
}