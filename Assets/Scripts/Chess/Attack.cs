namespace Chess
{
	public class Attack
	{
		public static bool IsInCheck((int file, int rank) kingPosition, Board board)
		{
			return IsAttacked(kingPosition, board, board.GetCurrentPlayer());
		}

		public static bool IsAttacked((int file, int rank) square, Board board, Color color)
		{
			return IsAttackedDiagonally(square, board, color) ||
				   IsAttackedKnights(square, board, color) ||
				   IsAttackedHorizontally(square, board, color) ||
				   IsAttackedPawns(square, board, color) ||
				   IsAttackedKings(square, board, color);
		}

		public static bool IsAttackedDiagonally((int file, int rank) square, Board board, Color attackerColor)
		{
			var directions = new (int dx, int dy)[] { (-1, -1), (-1, 1), (1, -1), (1, 1) };
			return IsAttackedByDirection(square, board, directions, PieceType.Bishop | PieceType.Queen, attackerColor);
		}

		public static bool IsAttackedHorizontally((int file, int rank) square, Board board, Color attackerColor)
		{
			var directions = new (int dx, int dy)[] { (-1, 0), (1, 0), (0, -1), (0, 1) };
			return IsAttackedByDirection(square, board, directions, PieceType.Rook | PieceType.Queen, attackerColor);
		}

		public static bool IsAttackedKnights((int file, int rank) square, Board board, Color attackerColor)
		{
			(int, int)[] knightOffsets = new (int, int)[] { (1, 2), (2, 1), (2, -1), (1, -2), (-1, -2), (-2, -1), (-2, 1), (-1, 2) };
			return IsAttackedOffset(square, board, knightOffsets, PieceType.Knight, attackerColor);
		}

		public static bool IsAttackedPawns((int file, int rank) square, Board board, Color attackerColor)
		{
			var pawnOffsets = (attackerColor == Color.White) ? new (int, int)[] { (1, -1), (-1, -1) } : new (int, int)[] { (1, 1), (-1, 1) };
			return IsAttackedOffset(square, board, pawnOffsets, PieceType.Pawn, attackerColor);
		}

		public static bool IsAttackedKings((int file, int rank) square, Board board, Color attackerColor)
		{
			(int, int)[] placesForKings = new (int, int)[] { (1, 1), (1, 0), (1, -1), (0, 1), (0, -1), (-1, 1), (-1, 0), (-1, -1) };
			return IsAttackedOffset(square, board, placesForKings, PieceType.King, attackerColor);
		}

		private static bool IsAttackedByDirection((int file, int rank) square, Board board, (int dx, int dy)[] directions, PieceType attackerTypes, Color attackerColor)
		{
			// Loop through each direction and check if square is attacked
			foreach ((int dx, int dy) in directions)
			{
				// Loop while nextSquare is inside the board
				(int file, int rank) nextSquare = (square.file + dx, square.rank + dy);
				while (Util.InBoard(nextSquare))
				{
					var potentialThreat = board.GetPiece(nextSquare);
					if (potentialThreat != null)
					{
						// If the piece was not of the attacking color, break checking this direction
						if (potentialThreat.GetColor() != attackerColor)
						{
							break;
						}
						PieceType threatType = potentialThreat.GetPieceType();
						// Check if the threat is any of the specified types (by ANDing and checking if different from zero)
						if ((threatType & attackerTypes) != 0)
						{
							return true;
						}
					}
					// If nextSquare was empty, go to next square by adding current direction (dx, dy)
					nextSquare = (nextSquare.file + dx, nextSquare.rank + dy);
				}
			}
			// If no attacking piece was found in any of the directions, return false
			return false;
		}
		private static bool IsAttackedOffset((int file, int rank) square, Board board, (int dx, int dy)[] offsets, PieceType attackerType, Color attackerColor)
		{
			foreach ((int dx, int dy) in offsets)
			{
				var nextSquare = (square.file + dx, square.rank + dy);
				// If square is outside board, continue to next square
				if (!Util.InBoard(nextSquare)) { continue; }

				// If square is empty, continue to next square
				Piece potentialThreat = board.GetPiece(nextSquare);
				if (potentialThreat == null) { continue; }

				// If the piece is of the attacking color and type, return true
				if (potentialThreat.GetColor() == attackerColor && potentialThreat.GetPieceType() == attackerType)
				{
					return true;
				}
			}
			// If none of the squares contained an attacking piece of the specified type, return false
			return false;
		}
	}
}