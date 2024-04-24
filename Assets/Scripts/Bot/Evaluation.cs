using Chess;

namespace Bot
{
	public class Evaluation
	{

		public static float EvaluatePosition(Board board)
		{
			float score = 0;
			//TODO Handle game over (once Draw is implemented in Board)
			// if (board.gameOver)
			// {
			//     if (board.isDraw)
			//     {
			//         return 0;
			//     }
			//     return (board.GetCurrentPlayer() == Color.White) ? int.MinValue : int.MaxValue;
			// }
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
					weight = Piece.IsColor(piece, Piece.White) ? 10 : -10;
					break;
				case Piece.Rook:
					weight = Piece.IsColor(piece, Piece.White) ? 50 : -50;
					break;
				case Piece.Bishop:
					weight = Piece.IsColor(piece, Piece.White) ? 30 : -30;
					break;
				case Piece.Knight:
					weight = Piece.IsColor(piece, Piece.White) ? 30 : -30;
					break;
				case Piece.Queen:
					weight = Piece.IsColor(piece, Piece.White) ? 90 : -90;
					break;
				case Piece.King:
					weight = Piece.IsColor(piece, Piece.White) ? 1000 : -1000;
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