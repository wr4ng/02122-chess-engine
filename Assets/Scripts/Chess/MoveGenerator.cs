using System.Collections.Generic;
using System.Linq;

namespace Chess
{
	public class MoveGenerator
	{
		static PieceType[] PROMOTION_PIECES = { PieceType.Rook, PieceType.Knight, PieceType.Bishop, PieceType.Queen };

		public static List<Move> GenerateLegalMoves(Board board)
		{
			var pseudoLegalMoves = GeneratePseudoLegalMoves(board);
			var legalMoves = pseudoLegalMoves.Where(move => IsLegal(board, move)).ToList();
			return legalMoves;
		}

		private static bool IsLegal(Board board, Move move)
		{
			// Make the move
			board.MakeMove(move);

			(int file, int rank) kingPosition = board.GetKingPosition(board.GetCurrentPlayer().Opposite());
			// If the king is in check after the move, the move wasn't legal
			bool isLegal = !Attack.IsAttacked(kingPosition, board, board.GetCurrentPlayer());

			// Unmake the move
			board.UnmakeMove(move);
			return isLegal;
		}

		public static List<Move> GeneratePseudoLegalMoves(Board board)
		{
			List<Move> moves = new List<Move>();
			for (int file = 0; file < 8; file++)
			{
				for (int rank = 0; rank < 8; rank++)
				{
					Piece piece = board.GetPiece((file, rank));
					if (piece != null && piece.GetColor() == board.GetCurrentPlayer())
					{
						(int, int) coords = (file, rank);
						switch (piece.GetPieceType())
						{
							case PieceType.Pawn:
								moves.AddRange(GeneratePawnMove(coords, board));
								break;
							case PieceType.Bishop:
								moves.AddRange(GenerateBishopMove(coords, board));
								break;
							case PieceType.Rook:
								moves.AddRange(GenerateRookMove(coords, board));
								break;
							case PieceType.Queen:
								moves.AddRange(GenerateQueenMove(coords, board));
								break;
							case PieceType.Knight:
								moves.AddRange(GenerateKnightMove(coords, board));
								break;
							case PieceType.King:
								moves.AddRange(GenerateKingMove(coords, board));
								break;
						}
					}
				}
			}
			moves.AddRange(GenerateCastlingMoves(board));
			return moves;
		}

		private static List<Move> GeneratePawnMove((int file, int rank) start, Board board)
		{
			List<Move> moves = new List<Move>();

			Color color = board.GetPiece(start).GetColor();
			(int file, int rank) direction = (color == Color.White) ? (0, 1) : (0, -1);

			// Foward Moves
			(int file, int rank) end = Util.AddTuples(start, direction);
			bool isBlocked = board.GetPiece(end) != null;
			if (!isBlocked)
			{
				// If pawn is moving to the opposite back rank, handle promotion
				if (end.rank == 0 || end.rank == 7)
				{
					moves.AddRange(PromotionMoves(start, end));
				}
				else
				{
					Move move = Move.SimpleMove(start, end, PieceType.Pawn);
					moves.Add(move);

					// Double forward move
					bool atStartPosition = (color == Color.White && start.rank == 1) || (color == Color.Black && start.rank == 6);
					if (atStartPosition)
					{
						var doubleEnd = Util.AddTuples(end, direction);
						isBlocked = board.GetPiece(doubleEnd) != null;
						if (!isBlocked)
						{
							Move doubleMove = Move.SimpleMove(start, doubleEnd, PieceType.Pawn);
							moves.Add(doubleMove);
						}
					}
				}
			}
			// Attack moves
			(int, int)[] attackPositions = { Util.AddTuples(direction, (1, 0)), Util.AddTuples(direction, (-1, 0)) };
			foreach ((int, int) attackPosition in attackPositions)
			{
				end = Util.AddTuples(start, attackPosition);
				if (Util.InBoard(end))
				{
					Piece attackedPiece = board.GetPiece(end);
					if (attackedPiece != null && attackedPiece.GetColor() != color)
					{
						// Capture promotion
						if (end.rank == 0 || end.rank == 7)
						{
							moves.AddRange(PromotionMoves(start, end, isCapture: true, attackedPiece));
						}
						// Pawn capture (no promotion)
						else
						{
							Move move = Move.CaptureMove(start, end, PieceType.Pawn, attackedPiece);
							moves.Add(move);
						}
					}
					else if (end == board.GetEnPassantCoords())
					{
						// Calculate position of captured pawn
						var capturedPosition = (end.file, end.rank - direction.rank);
						var capturedPiece = board.GetPiece(capturedPosition);
						Move move = Move.EnPassantMove(start, end, capturedPiece, capturedPosition);
						moves.Add(move);
					}
				}
			}
			return moves;
		}

		// Returns the four possible promotion moves given a start and end square
		private static List<Move> PromotionMoves((int file, int rank) start, (int file, int rank) end, bool isCapture = false, Piece capturePiece = null)
		{
			var moves = new List<Move>();
			foreach (PieceType promotionPieceType in PROMOTION_PIECES)
			{
				moves.Add(Move.PromotionMove(start, end, promotionPieceType, isCapture, capturePiece));
			}
			return moves;
		}

		private static List<Move> GenerateBishopMove((int, int) start, Board board)
		{
			return MoveByDirection(start, board, new (int, int)[] { (-1, -1), (-1, 1), (1, -1), (1, 1) }, PieceType.Bishop); //TODO vi kunne lave directions til enums maybe?
		}

		private static List<Move> GenerateRookMove((int, int) start, Board board)
		{
			return MoveByDirection(start, board, new (int, int)[] { (-1, 0), (1, 0), (0, -1), (0, 1) }, PieceType.Rook);
		}

		private static List<Move> GenerateQueenMove((int, int) start, Board board)
		{
			return MoveByDirection(start, board, new (int, int)[] { (-1, 0), (1, 0), (0, -1), (0, 1), (-1, -1), (-1, 1), (1, -1), (1, 1) }, PieceType.Queen);
		}

		private static List<Move> GenerateKnightMove((int, int) start, Board board)
		{
			return MoveByOffset(start, board, new (int, int)[] { (1, 2), (2, 1), (2, -1), (1, -2), (-1, -2), (-2, -1), (-2, 1), (-1, 2) }, PieceType.Knight);
		}

		private static List<Move> GenerateKingMove((int, int) start, Board board)
		{
			return MoveByOffset(start, board, new (int, int)[] { (1, 1), (1, 0), (1, -1), (0, 1), (0, -1), (-1, 1), (-1, 0), (-1, -1) }, PieceType.King);
		}

		private static List<Move> MoveByDirection((int, int) start, Board board, (int, int)[] directions, PieceType pieceType)
		{
			List<Move> moves = new List<Move>();
			Color color = board.GetPiece(start).GetColor();
			Piece attackedPiece;
			foreach ((int, int) direction in directions)
			{
				(int, int) position = Util.AddTuples(start, direction);
				while (Util.InBoard(position))
				{
					attackedPiece = board.GetPiece(position);
					if (attackedPiece == null)
					{
						Move move = Move.SimpleMove(start, position, pieceType);
						moves.Add(move);
					}
					else if (attackedPiece.GetColor() != color)
					{
						Move move = Move.CaptureMove(start, position, pieceType, attackedPiece);
						moves.Add(move);
						break;
					}
					else
					{
						break; //because it would be a piece of same color
					}
					position = Util.AddTuples(position, direction);
				}
			}
			return moves;
		}

		public static List<Move> MoveByOffset((int, int) start, Board board, (int, int)[] offsets, PieceType pieceType)
		{
			List<Move> moves = new List<Move>();
			Color color = board.GetPiece(start).GetColor();
			Piece attackedPiece;
			foreach ((int, int) offset in offsets)
			{
				if (!Util.InBoard(Util.AddTuples(start, offset))) continue;
				(int, int) position = Util.AddTuples(start, offset);
				attackedPiece = board.GetPiece(position);
				if (attackedPiece == null)
				{
					Move move = Move.SimpleMove(start, position, pieceType);
					moves.Add(move);
				}
				else if (attackedPiece.GetColor() != color)
				{
					Move move = Move.CaptureMove(start, position, pieceType, attackedPiece);
					moves.Add(move);
				}
			}
			return moves;
		}

		private static List<Move> GenerateCastlingMoves(Board board)
		{
			List<Move> moves = new List<Move>();

			Color kingColor = board.GetCurrentPlayer();
			int kingRank = (kingColor == Color.White) ? 0 : 7;
			// Cannot castle out of check
			bool kingUnderAttack = Attack.IsAttacked(board.GetKingPosition(kingColor), board, kingColor.Opposite());
			if (kingUnderAttack)
			{
				return moves;
			}
			// Check if can castle to king and queenside
			CastlingRights castlingRights = board.GetCastlingRights();
			// TODO Check if there is a rook in the correct position
			if (CanCastleKingside(board, castlingRights, kingColor, kingRank))
			{
				Move kingSide = Move.CastleMove((4, kingRank), (6, kingRank), (7, kingRank), (5, kingRank));
				moves.Add(kingSide);
			}
			if (CanCastleQueenside(board, castlingRights, kingColor, kingRank))
			{
				Move queenSide = Move.CastleMove((4, kingRank), (2, kingRank), (0, kingRank), (3, kingRank));
				moves.Add(queenSide);
			}
			return moves;
		}

		private static bool CanCastleKingside(Board board, CastlingRights castlingRights, Color kingColor, int kingRank)
		{
			// Check if the king has the right to castle (king and rook hasn't moved)
			bool canCastleKingside = (kingColor == Color.White) ? castlingRights.HasFlag(CastlingRights.WhiteKingside) : castlingRights.HasFlag(CastlingRights.BlackKingside);
			if (!canCastleKingside)
			{
				return false;
			}
			// Check if the required squares are empty and not under attack
			bool squaresEmptyAndNotAttacked = CanCastle(board, new (int, int)[] { (5, kingRank), (6, kingRank) }, kingColor.Opposite());
			return squaresEmptyAndNotAttacked;
		}

		private static bool CanCastleQueenside(Board board, CastlingRights castlingRights, Color kingColor, int kingRank)
		{
			// Check if the king has the right to castle (king and rook hasn't moved)
			bool canCastleQueenside = (kingColor == Color.White) ? castlingRights.HasFlag(CastlingRights.WhiteQueenside) : castlingRights.HasFlag(CastlingRights.BlackQueenside);
			if (!canCastleQueenside)
			{
				return false;
			}
			// Check if the required squares are empty and not under attack
			bool squaresEmptyAndNotAttacked = CanCastle(board, new (int, int)[] { (3, kingRank), (2, kingRank), (1, kingRank) }, kingColor.Opposite());
			return squaresEmptyAndNotAttacked;
		}

		// TODO Is there a better name for this method
		private static bool CanCastle(Board board, (int, int)[] squaresToBeEmpty, Color attackingColor)
		{
			foreach ((int file, int rank) square in squaresToBeEmpty)
			{
				Piece piece = board.GetPiece(square);
				if (piece != null) return false;
				// When castling queenside, b1 or b8 doesn't have to not under attack
				if (square.file != 1 && Attack.IsAttacked(square, board, attackingColor)) return false;
			}
			return true;
		}
	}
}