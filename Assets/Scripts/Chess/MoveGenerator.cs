using System.Collections.Generic;
using System.Linq;

namespace Chess
{
	public class MoveGenerator
	{
		public static List<Move> GenerateMoves(Board board)
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
								moves.Concat(GeneratePawnMove(coords, board));
								break;
							case PieceType.Bishop:
								moves.Concat(GenerateBishopMove(coords, board));
								break;
							case PieceType.Rook:
								moves.Concat(GenerateRookMove(coords, board));
								break;
							case PieceType.Queen:
								moves.Concat(GenerateQueenMove(coords, board));
								break;
							case PieceType.Knight:
								moves.Concat(GenerateKnightMove(coords, board));
								break;
							case PieceType.King:
								moves.Concat(GenerateKingMove(coords, board));
								break;
						}
					}
				}
			}
			AddCastlingMoves(board, moves);

			return moves;
		}
		public static List<Move> GeneratePawnMove((int, int) start, Board board)
		{
			List<Move> moves = new List<Move>();
			Color color = board.GetPiece(start).GetColor();
			(int, int) direction = (color == Color.White) ? (0, 1) : (0, -1);
			// Foward Moves
			(int, int) position = Util.AddTuples(start, direction);
			bool isBlocked = board.GetPiece(position) != null;
			if (!isBlocked)
			{
				if (position.Item2 == 0 || position.Item2 == 7)
				{
					// Promotion
					foreach (PieceType promotionPieceType in PieceTypeHelper.PromotionPieces)
					{
						Move move = Move.Promotion(start, position, promotionPieceType);
						AddMove(move, board, moves);
					}
				}
				else
				{
					// Not promotion
					Move move = new Move(start, position);
					AddMove(move, board, moves);
					bool atStartPosition = (color == Color.White && start.Item2 == 1) || (color == Color.Black && start.Item2 == 6);
					position = Util.AddTuples(position, direction);
					isBlocked = board.GetPiece(position) != null;
					if (atStartPosition && !isBlocked)
					{
						Move move2 = new Move(start, position);
						AddMove(move2, board, moves);
					}
				}
			}
			// Attack moves
			(int, int)[] attackPositions = { Util.AddTuples(direction, (1, 0)), Util.AddTuples(direction, (-1, 0)) };
			foreach ((int, int) attackPosition in attackPositions)
			{
				position = Util.AddTuples(start, attackPosition);
				if (Util.InBoard(position))
				{
					Piece attackedPiece = board.GetPiece(position);
					if (attackedPiece != null && attackedPiece.GetColor() != color)
					{
						Move move = new Move(start, position);
						AddMove(move, board, moves);
					}
					if (position == board.GetEnPassantCoords())
					{
						(int, int)[] startMove = new (int, int)[] { position, start };
						(int, int)[] end = new (int, int)[] { Util.AddTuples(position, (color == Color.White) ? (0, -1) : (0, 1)), position };
						Move move = new Move(startMove, end);
						move.SetEnpassant(true);
						AddMove(move, board, moves);
					}
				}
			}
			return moves;
		}

		public static List<Move> GenerateBishopMove((int, int) start, Board board)
		{
			return MoveByDirection(start, board, new (int, int)[] { (-1, -1), (-1, 1), (1, -1), (1, 1) }); //TODO vi kunne lave directions til enums maybe?
		}

		public static List<Move> GenerateRookMove((int, int) start, Board board)
		{
			return MoveByDirection(start, board, new (int, int)[] { (-1, 0), (1, 0), (0, -1), (0, 1) });
		}

		public static List<Move> GenerateQueenMove((int, int) start, Board board)
		{
			return MoveByDirection(start, board, new (int, int)[] { (-1, 0), (1, 0), (0, -1), (0, 1), (-1, -1), (-1, 1), (1, -1), (1, 1) });
		}

		public static List<Move> GenerateKnightMove((int, int) start, Board board)
		{
			return MoveByOffset(start, board, new (int, int)[] { (1, 2), (2, 1), (2, -1), (1, -2), (-1, -2), (-2, -1), (-2, 1), (-1, 2) });
		}

		public static List<Move> GenerateKingMove((int, int) start, Board board)
		{
			return MoveByOffset(start, board, new (int, int)[] { (1, 1), (1, 0), (1, -1), (0, 1), (0, -1), (-1, 1), (-1, 0), (-1, -1) });
		}

		public static List<Move> MoveByDirection((int, int) start, Board board, (int, int)[] directions)
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
						Move move = new Move(start, position);
						AddMove(move, board, moves);
					}
					else if (attackedPiece.GetColor() != color)
					{
						Move move = new Move(start, position);
						AddMove(move, board, moves);
						break;
					}
					else
					{
						break; // because it would be a piece of same color
					}
					position = Util.AddTuples(position, direction);
				}
			}
			return moves;
		}

		public static List<Move> MoveByOffset((int, int) start, Board board, (int, int)[] offsets)
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
					Move move = new Move(start, position);
					AddMove(move, board, moves);
				}
				else if (attackedPiece.GetColor() != color)
				{
					Move move = new Move(start, position);
					AddMove(move, board, moves);
				}
			}
			return moves;
		}

		private static void AddMove(Move move, Board board, List<Move> moves)
		{
			Color kingColor = board.GetCurrentPlayer();
			board.MakeMove(move);
			if (!Check.IsInCheck(board.GetKingPosition(kingColor), board))
			{
				moves.Add(move);
			}
			board.UnmakeMove(move);
		}

		public static void AddCastlingMoves(Board board, List<Move> moves)
		{
			int rank = (board.GetCurrentPlayer() == Color.White) ? 0 : 7;
			CastlingRights castlingRights = board.GetCastlingRights();
			if (Check.IsInCheck(board.GetKingPosition(board.GetCurrentPlayer()), board)) return;
			if (CanCastleKingside(board, castlingRights, rank))
			{
				(int, int)[] start = new (int, int)[] { (4, rank), (7, rank) };
				(int, int)[] end = new (int, int)[] { (6, rank), (5, rank) };
			}
			if (CanCastleQueenside(board, castlingRights, rank))
			{
				(int, int)[] start = new (int, int)[] { (4, rank), (0, rank) };
				(int, int)[] end = new (int, int)[] { (2, rank), (3, rank) };
			}
		}

		private static bool CanCastleKingside(Board board, CastlingRights castlingRights, int rank)
		{
			bool canCastleKingside = (rank == 0) ? castlingRights.HasFlag(CastlingRights.WhiteKingside) : castlingRights.HasFlag(CastlingRights.BlackKingside);
			if (!canCastleKingside) return false;
			return CanCastle(board, new (int, int)[] { (5, rank), (6, rank) });
		}

		private static bool CanCastleQueenside(Board board, CastlingRights castlingRights, int rank)
		{
			bool canCastleQueenside = (rank == 0) ? castlingRights.HasFlag(CastlingRights.WhiteKingside) : castlingRights.HasFlag(CastlingRights.BlackKingside);
			if (!canCastleQueenside) return false;
			return CanCastle(board, new (int, int)[] { (3, rank), (2, rank), (1, rank) });
		}

		private static bool CanCastle(Board board, (int, int)[] squaresToBeEmpty)
		{
			foreach ((int, int) square in squaresToBeEmpty)
			{
				Piece piece = board.GetPiece(square);
				if (piece != null) return false;
				if (square.Item1 != 1 && Check.IsInCheck(square, board)) return false;
			}
			return true;
		}
	}
}