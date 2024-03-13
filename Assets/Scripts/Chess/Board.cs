using System;
using System.Collections.Generic;

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

		private Stack<Move> playedMoves; // List of played moves

		private Board() { }

		public static Board ImportFromFEN(string fen)
		{
			// Initialize board
			Board board = new Board();
			board.playedMoves = new Stack<Move>();

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

		public void SetCastlingRights(CastlingRights castlingRight)
		{
			castlingRights = castlingRight;
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
			// Handle new en passant square
			move.SetPrevEnPassantSquare(enPassantSquare);
			if (move.IsDoublePawnMove()){
				enPassantSquare = move.GetEnPassantSquare();
			}
			else {
				enPassantSquare = (-1,-1);
			}
			move.SetPrevCastlingRights(castlingRights);
			castlingRights = UpdateAllCastlingRights(castlingRights, move);
			SwapPlayer();
			playedMoves.Push(move);
		}

		// TODO Update castling rights when unmaking a move
		public void UnmakeMove(Move move)
		{
			// Can only unmake moves that have previously been made
			if (!playedMoves.TryPeek(out Move topMove) || topMove != move)
			{
				throw new ArgumentException("Trying to unmake move which isn't the top move!");
			}
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
			enPassantSquare = move.GetPrevEnPassantSquare();
			castlingRights = move.GetPrevCastlingRights();
			SwapPlayer();
			playedMoves.Pop();
		}

		public void UndoPreviousMove()
		{
			if (playedMoves.TryPeek(out Move previousMove))
			{
				UnmakeMove(previousMove);
			}
		}
		public CastlingRights UpdateAllCastlingRights(CastlingRights castlingRights, Move move){
			castlingRights = UpdateCastlingRights(castlingRights, CastlingRights.WhiteKingside, move, (7,0), (4,0));
			castlingRights = UpdateCastlingRights(castlingRights, CastlingRights.BlackKingside, move, (7,7), (4,7));
			castlingRights = UpdateCastlingRights(castlingRights, CastlingRights.WhiteQueenside, move, (0,0), (4,0));
			castlingRights = UpdateCastlingRights(castlingRights, CastlingRights.BlackQueenside, move, (0,7), (4,7));
			return castlingRights;
		}

		public CastlingRights UpdateCastlingRights(CastlingRights castlingRights, CastlingRights rightToCheck, Move move, (int,int) rookPos, (int,int) kingPos){
			(int,int) moveStart = move.GetStartSquare();
			(int,int) moveEnd = move.GetCaptureSquare();
			if ((castlingRights & rightToCheck) == rightToCheck && (moveStart == rookPos || moveStart == kingPos || moveEnd == rookPos)){
				return castlingRights & (CastlingRights.All ^ rightToCheck);
			}
			return castlingRights;
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