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
		(int, int) enPassantSquare; // (file, rank)
		private int halfmoveClock; // Used to determine fifty move rule
		private int fullmoveNumber; // The number of full moves made in the game

		private Board() { }

		public static Board ImportFromFEN(string fen)
		{
			int fenIndex = 0;

			// Parse board state
			Board board = new Board();
			int file = 0;
			int rank = 7;
			while (fen[fenIndex] != ' ')
			{
				// Parse empty squares
				if (int.TryParse(fen[fenIndex].ToString(), out int skip))
				{
					file += skip;
				}
				// Parse rank skip
				else if (fen[fenIndex] == '/')
				{
					if (file != 8)
					{
						throw new ArgumentException($"Invalid FEN string (board setup): {fen}");
					}
					file = 0;
					rank--;
				}
				else
				{
					// Parse pieces
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
				fenIndex++;
			}
			fenIndex++;
			// Parse current player
			// Parse castling rights
			// Parse en passant square
			// Parse halfmove clock and fullmove number
			return board;
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
			return Util.CoordinateToString(enPassantSquare.Item1, enPassantSquare.Item2);
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