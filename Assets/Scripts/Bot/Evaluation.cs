using Chess;

namespace Bot
{
	public class Evaluation
	{
		public static float ChechmateScore = 100000;

		public static float EvaluatePosition(Board board)
		{
			// Check if we have checkmate/stalemate
			if (board.moveGenerator.GenerateMoves().Count == 0)
			{
				bool kingAttacked = board.moveGenerator.IsAttacked(board.kingSquares[Piece.ColorIndex(board.colorToMove)], board.oppositeColor);
				if (kingAttacked)
					// No moves + king attacked = checkmate
					return board.colorToMove == Piece.White ? -ChechmateScore : ChechmateScore;
				else
					// No moves + king not attacked = stalemate
					return 0;
			}
			// Calculate piece score
			float score = 0;
			for (int file = 0; file < 8; file++)
			{
				for (int rank = 0; rank < 8; rank++)
				{
					int piece = board.squares[file, rank];
					if (piece != Piece.None)
					{
						score += PieceWeight(piece);
						score += PiecePosition(piece, file + 8 * rank);
					}
				}
			}
			return score;
		}

		public static float PieceWeight(int piece)
		{
			float weight = Piece.Type(piece) switch
			{
				Piece.Pawn   => 100,
				Piece.Knight => 300,
				Piece.Bishop => 300,
				Piece.Rook   => 500,
				Piece.Queen  => 900,
				Piece.King   => 10000, //NOTE: Not useful since king is never captured
				_            => 0
			};

			bool isWhite = Piece.IsColor(piece, Piece.White);
			return isWhite ? weight : -weight;
		}

		public static float PiecePosition(int piece, int index)
		{
			bool isWhite = Piece.IsColor(piece, Piece.White);
			int correctedIndex = isWhite ? 63 - index : index;
			float weight = PositionWeights.pieceWeights[Piece.TypeIndex(piece)][correctedIndex];
			return isWhite ? weight : -weight;
		}
	}
}