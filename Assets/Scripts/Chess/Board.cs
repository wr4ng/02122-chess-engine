using System;

namespace Chess
{
	public class Board
	{
		public const int BOARD_SIZE = 8;

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
			// Validate FEN parts
			string[] fenParts = fen.Split(' ');
			if (fenParts.Length != 6)
			{
				throw new ArgumentException($"Invalid FEN string (wrong number of parts): {fen}");
			}
			// Parse each fen part
			board.board = FEN.ParseBoard(fenParts[0]);
			board.currentPlayer = FEN.ParsePlayer(fenParts[1]);
			board.castlingRights = FEN.ParseCastlingRights(fenParts[2]);
			board.enPassantSquare = FEN.ParseEnPassant(fenParts[3]);
			board.halfmoveClock = FEN.ParseHalfmoveClock(fenParts[4]);
			board.fullmoveNumber = FEN.ParseFullmoveNumber(fenParts[5]);
			// Return resulting board
			return board;
		}

		public static Board DefaultBoard()
		{
			return ImportFromFEN(FEN.STARTING_POSITION_FEN);
		}

		public string ExportToFEN()
		{
			string fen = "";
			//add the board state "https://en.wikipedia.org/wiki/Forsyth%E2%80%93Edwards_Notation"
			fen += FEN.BoardToFEN(this.board);
			fen += FEN.CurrentPlayerToFEN(this.currentPlayer);
			fen += FEN.CastlingRightsToFEN(this.castlingRights);
			fen += FEN.EnPassantToFEN(this.enPassantSquare);
			fen += FEN.HalfmoveClockToFEN(this.halfmoveClock);
			fen += FEN.FullmoveNumberToFEN(this.fullmoveNumber);
			return fen;
		}

		public Piece GetPiece(int file, int rank)
		{
			return board[file, rank];
		}

		public Piece GetPiece((int, int) coords)
		{
			return board[coords.Item1, coords.Item2];
		}

		public void SetPiece((int, int) coords, Piece piece)
		{
			board[coords.Item1, coords.Item2] = piece;
		}

		public void SwapPlayer()
		{
			currentPlayer = currentPlayer.Opposite();
		}

		public Color GetCurrentPlayer()
		{
			return currentPlayer;
		}

		public CastlingRights GetCastlingRights()
		{
			return castlingRights;
		}

		public string GetEnPassantSquare()
		{
			return enPassantSquare == (-1, -1) ? "-" : FEN.CoordinateToFEN(enPassantSquare);
		}

		public (int, int) GetEnPassantCoords()
		{
			return enPassantSquare;
		}

		public Piece[,] GetBoard()
		{
			return board;
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

		// TODO Update castling rights when making a move
		public void MakeMove(Move move)
		{
			// If move is a capture, remove captured piece
			if (move.IsCapture())
			{
				SetPiece(move.GetCaptureSquare(), null);
			}
			// If move is a castle, move rook
			else if (move.IsCastle())
			{
				SetPiece(move.GetRookEnd(), GetPiece(move.GetRookStart()));
				SetPiece(move.GetRookStart(), null);
			}
			// Move main piece
			if (move.IsPromotion())
			{
				SetPiece(move.GetEndSquare(), new Piece(move.PromotionPieceType(), currentPlayer));
			}
			else
			{
				SetPiece(move.GetEndSquare(), GetPiece(move.GetStartSquare()));
			}
			SetPiece(move.GetStartSquare(), null);

			SwapPlayer();
		}

		// TODO Update castling rights when unmaking a move
		public void UnmakeMove(Move move)
		{
			// Move main piece back
			if (move.IsPromotion())
			{
				SetPiece(move.GetStartSquare(), new Piece(PieceType.Pawn, currentPlayer.Opposite()));
			}
			else
			{
				SetPiece(move.GetStartSquare(), GetPiece(move.GetEndSquare()));
			}
			SetPiece(move.GetEndSquare(), null);
			// If castle, move rook back
			if (move.IsCastle())
			{
				SetPiece(move.GetRookStart(), GetPiece(move.GetRookEnd()));
				SetPiece(move.GetRookEnd(), null);
			}
			// If capture, re-add captured piece
			if (move.IsCapture())
			{
				SetPiece(move.GetCaptureSquare(), move.GetCapturedPiece());
			}
			SwapPlayer();
		}

		internal (int, int) GetKingPosition(Color color)
		{
			for (int file = 0; file < BOARD_SIZE; file++)
			{
				for (int rank = 0; rank < BOARD_SIZE; rank++)
				{
					if (board[file, rank] != null && board[file, rank].GetColor() == color && board[file, rank].GetPieceType() == PieceType.King)
					{
						return (file, rank);
					}
				}
			}
			return (-1, -1);
		}
	}
}