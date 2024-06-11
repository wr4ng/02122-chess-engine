using Chess;

namespace Bot
{
	public static class Evaluation
	{
		public static float ChechmateScore = 100000;
		public static int calls = 0;

		public static float EvaluatePosition(Board board)
		{
			calls++;
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
			//TODO In Board, keep track of the number of each piece, to avoid looping over the board
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
			//TODO Use switch-expression
			float weight;
			switch (Piece.Type(piece))
			{
				case Piece.Pawn:
					weight = Piece.IsColor(piece, Piece.White) ? 100 : -100;
					break;
				case Piece.Rook:
					weight = Piece.IsColor(piece, Piece.White) ? 500 : -500;
					break;
				case Piece.Bishop:
					weight = Piece.IsColor(piece, Piece.White) ? 300 : -300;
					break;
				case Piece.Knight:
					weight = Piece.IsColor(piece, Piece.White) ? 300 : -300;
					break;
				case Piece.Queen:
					weight = Piece.IsColor(piece, Piece.White) ? 900 : -900;
					break;
				case Piece.King:
					weight = Piece.IsColor(piece, Piece.White) ? 10000 : -10000;
					break;
				default:
					weight = 0;
					break;
			}
			return weight;
		}

		public static float PiecePosition(int piece, int index)
		{
			float weight;
			//TODO Use the type (int) of piece as index into PositionWeights
			switch (Piece.Type(piece))
			{
				case Piece.Pawn:
					weight = Piece.IsColor(piece, Piece.White) ? PositionWeights.pawnWeight[63 - index] : -PositionWeights.pawnWeight[index];
					break;
				case Piece.Rook:
					weight = Piece.IsColor(piece, Piece.White) ? PositionWeights.rookWeight[63 - index] : -PositionWeights.rookWeight[index];
					break;
				case Piece.Bishop:
					weight = Piece.IsColor(piece, Piece.White) ? PositionWeights.bishopWeight[63 - index] : -PositionWeights.bishopWeight[index];
					break;
				case Piece.Knight:
					weight = Piece.IsColor(piece, Piece.White) ? PositionWeights.knightWeight[63 - index] : -PositionWeights.knightWeight[index];
					break;
				case Piece.Queen:
					weight = Piece.IsColor(piece, Piece.White) ? PositionWeights.queenWeight[63 - index] : -PositionWeights.queenWeight[index];
					break;
				case Piece.King:
					weight = Piece.IsColor(piece, Piece.White) ? PositionWeights.kingWeight[63 - index] : -PositionWeights.kingWeight[index];
					break;
				default:
					throw new System.Exception($"Invalid piece type: {piece}");
			}
			return weight;
		}

		//Other evaluation functions
	}
}