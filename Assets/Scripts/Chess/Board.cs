using System;

namespace Chess
{
	public class Board
	{
		public const int BOARD_SIZE = 8;
		public const string DEFAULT_FEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

		private Piece[,] board = new Piece[BOARD_SIZE, BOARD_SIZE];
		private Color currentPlayer;
		private CastlingRights castlingRights;
		private (int, int) enPassantSquare; // (file, rank), holds possible en Passant square
		private int halfmoveClock; // Used to determine fifty move rule
		private int fullmoveNumber; // The number of full moves made in the game

		private Board() { }

		public static Board ImportFromFEN(string fen)
		{
			Board board = new Board();

			string[] fenParts = fen.Split(' ');
			if (fenParts.Length != 6)
			{
				throw new ArgumentException($"Invalid FEN string (wrong number of parts): {fen}");
			}
			parseBoard(fenParts[0], board);
			parsePlayer(fenParts[1], board);
			//TODO Parse castling rights
			parseEnPassant(fenParts[3], board);
			parseHalfmoveClock(fenParts[4], board);
			parseFullmoveNumber(fenParts[5], board);
			return board;
		}

		private static void parseBoard(string fen, Board board)
		{
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
					if(rank < 0)
					{
						throw new ArgumentException($"Invalid FEN string (too many ranks): {fen}");
					}
				}
				else
				{
					board.board[file, rank] = fen[fenIndex] switch
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
		}
		private static void parsePlayer(string fen, Board board)
		{
			board.currentPlayer = fen switch
			{
				"w" => Color.White,
				"b" => Color.Black,
				_ => throw new ArgumentException($"Invalid FEN string (invalid player character): {fen}")
			};
		}
		private static void parseEnPassant(string fen, Board board)
		{
			if (fen == "-")
			{
				board.enPassantSquare = (-1,-1);
				return;
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
			board.enPassantSquare = (file, rank-1);
		}
		private static void parseHalfmoveClock(string fen, Board board)
		{
			if (int.TryParse(fen, out int halfmove))
			{
				board.halfmoveClock = halfmove;
			}
			else
			{
				throw new ArgumentException($"Invalid FEN string (invalid halfmove number): {fen}");
			}
		}
		private static void parseFullmoveNumber(string fen, Board board)
		{
			if (int.TryParse(fen, out int fullmove))
			{
				board.fullmoveNumber = fullmove;
			}
			else
			{
				throw new ArgumentException($"Invalid FEN string (invalid fullmove number): {fen}");
			}
		}


		public string ExportToFEN()
		{
			//add the board state "https://en.wikipedia.org/wiki/Forsyth%E2%80%93Edwards_Notation"
			//add the current player
			//add the castling rights
			//add the en passant square
			//add the halfmove clock
			//add the fullmove number
			throw new NotImplementedException();
		}

		public Piece GetPiece(int file, int rank)
		{
			return board[file, rank];
		}

		public Color GetCurrentPlayer()
		{
			return currentPlayer;
		}

		public string GetCastlingRights()
		{
			return castlingRights.ToFENString();
		}

		public string GetEnPassantSquare()
		{
			return enPassantSquare == (-1, -1) ? "-" : Util.CoordinateToString(enPassantSquare.Item1, enPassantSquare.Item2);
		}

		public int GetHalfmoveClock()
		{
			return halfmoveClock;
		}

		public int GetFullmoveNumber()
		{
			return fullmoveNumber;
		}

		public override string ToString()
		{
			String result = "";
			for (int rank = BOARD_SIZE - 1; 0 <= rank; rank--)
			{
				for (int file = 0; file < BOARD_SIZE; file++)
				{
					if (board[file, rank] == null)
					{
						result += "-";
					}
					else
					{
						result += board[file, rank].ToFENchar();
					}
				}
				result += "\n";
			}
			return result.Trim();
		}
	}
}