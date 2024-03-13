using System;
using System.Linq;
using System.Collections.Generic;

namespace Chess
{
	public class Board
	{
		public const int BOARD_SIZE = 8;
		public Draw draw;

		private Piece[,] board = new Piece[BOARD_SIZE, BOARD_SIZE];
		private Color currentPlayer;
		private CastlingRights castlingRights;
		private (int, int) enPassantSquare; 	// (file, rank), holds possible en Passant square
		private int halfmoveClock; 				// Used to determine fifty move rule
		private int fullmoveNumber; 			// The number of full moves made in the game

		private Stack<Move> playedMoves;    	// List of played moves
		private List<Move> legalMoves;   		// List of the currently legal moves

		private Board()
		{
			legalMoves = new List<Move>();
		}

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

			board.draw = new Draw(board.GetHalfmoveClock(), fen);

			// Calculate legal moves
			board.GenerateLegalMoves();

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

		public List<Move> GetLegalMoves()
		{
			return legalMoves;
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
		private void MakeMove(Move move, bool calculateNextLegalMoves = false)
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
			if (move.IsDoublePawnMove())
			{
				enPassantSquare = move.GetEnPassantSquare();
			}
			else
			{
				enPassantSquare = (-1, -1);
			}

			// Update halfmove clock in draw object
			draw.fiftyMoveRule(move.GetPieceType(), move.IsCapture());

			// Update position count in draw object
			draw.updatePositionCount(ExportToFEN());
			if (draw.getIsDraw())
			{
				// TODO Handle draw
			}

			SwapPlayer();
			playedMoves.Push(move);

			// After making a move, possibly calculate the new legal moves
			if (calculateNextLegalMoves)
			{
				GenerateLegalMoves();
			}
		}

		// TODO Update castling rights when unmaking a move
		private void UnmakeMove(Move move, bool calculateNextLegalMoves = false)
		{
			// Undo changes in draw object
			draw.undoDrawCount(ExportToFEN());

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

			SwapPlayer();
			playedMoves.Pop();

			// After making a move, possibly calculate the new legal moves
			if (calculateNextLegalMoves)
			{
				GenerateLegalMoves();
			}
		}

		public void PlayMove(Move move)
		{
			if (legalMoves.Contains(move))
			{
				MakeMove(move, true);
			}
			else
			{
				throw new ArgumentException("Move not legal!");
			}
		}

		public void UndoPreviousMove()
		{
			if (playedMoves.TryPeek(out Move previousMove))
			{
				UnmakeMove(previousMove, true);
			}
		}

		private void GenerateLegalMoves()
		{
			var pseudoLegalMoves = MoveGenerator.GeneratePseudoLegalMoves(this);
			legalMoves = pseudoLegalMoves.Where(move => IsLegal(move)).ToList();
		}

		private bool IsLegal(Move move)
		{
			// Make the move
			MakeMove(move);

			(int file, int rank) kingPosition = GetKingPosition(currentPlayer.Opposite());
			// If the king is in check after the move, the move wasn't legal
			bool isLegal = !Attack.IsAttacked(kingPosition, this, currentPlayer);

			// Unmake the move
			UnmakeMove(move);
			return isLegal;
		}

		public (int, int) GetKingPosition(Color color)
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