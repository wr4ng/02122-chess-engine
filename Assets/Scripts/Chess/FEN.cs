using System;
using System.Linq;

namespace Chess
{
	public class FEN
	{
		public const string STARTING_POSITION_FEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"; //TODO ret sikker på at det sidste skal være 0

		public static string CoordinateToFEN(int file, int rank) //TODO thats no FEN is it er det ikke bare til string?
		{
			if (!IsValidCoordinate(file, rank))
			{
				throw new ArgumentException($"Invalid rank and file: file {file}, rank {rank}");
			}
			return $"{(char)('a' + file)}{rank + 1}";
		}

		public static string CoordinateToFEN((int, int) coordinate)
		{
			return CoordinateToFEN(coordinate.Item1, coordinate.Item2);
		}

		public static bool IsValidCoordinate(int file, int rank)
		{
			return 0 <= file || file < Board.BOARD_SIZE || 0 <= rank || rank < Board.BOARD_SIZE;
		}

		public static Piece[,] ParseBoard(string fen)
		{
			Piece[,] board = new Piece[8, 8];

			int file = 0;
			int rank = 7;

			for (int fenIndex = 0; fenIndex < fen.Length; fenIndex++)
			{
				if (int.TryParse(fen[fenIndex].ToString(), out int skip))
				{
					file += skip;
				}
				else if (fen[fenIndex] == '/')
				{
					if (file != 8)
					{
						throw new ArgumentException($"Invalid FEN string (invalid file length): {fen}");
					}
					file = 0;
					rank--;
					if (rank < 0)
					{
						throw new ArgumentException($"Invalid FEN string (too many ranks): {fen}");
					}
				}
				else
				{
					board[file, rank] = fen[fenIndex] switch
					{
						'P' => new Piece(PieceType.Pawn, Color.White),
						'N' => new Piece(PieceType.Knight, Color.White),
						'B' => new Piece(PieceType.Bishop, Color.White),
						'R' => new Piece(PieceType.Rook, Color.White),
						'Q' => new Piece(PieceType.Queen, Color.White),
						'K' => new Piece(PieceType.King, Color.White),
						'p' => new Piece(PieceType.Pawn, Color.Black),
						'n' => new Piece(PieceType.Knight, Color.Black),
						'b' => new Piece(PieceType.Bishop, Color.Black),
						'r' => new Piece(PieceType.Rook, Color.Black),
						'q' => new Piece(PieceType.Queen, Color.Black),
						'k' => new Piece(PieceType.King, Color.Black),
						_ => throw new ArgumentException($"Invalid FEN string (invalid piece character): {fen[fenIndex]}")
					};
					file++;
				}
			}
			return board;
		}

		public static Color ParsePlayer(string fen)
		{
			return fen switch
			{
				"w" => Color.White,
				"b" => Color.Black,
				_ => throw new ArgumentException($"Invalid FEN string (invalid player character): {fen}")
			};
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
	
		public static string BoardToFEN(Piece[,] board){
			string fen = "";
			for (int rank = 7; rank >= 0; rank--)
			{
				int emptyTiles = 0;
				for (int file = 0; file < Board.BOARD_SIZE; file++)
				{
					if (board[file, rank] == null)
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
						fen += board[file, rank].ToFENchar();
					}
				}
				if(emptyTiles != 0){	//TODO føler de her to sidste if's er grimme
					fen += emptyTiles;
				}
				if (rank != 0)
				{
					fen += "/";
				}
			}
			return fen;
		}
	}
}