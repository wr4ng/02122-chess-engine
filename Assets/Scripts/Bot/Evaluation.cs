using Chess;

namespace Bot
{
	public class Evaluation
	{

		public static float EvaluatePosition(NewBoard board)
		{
			float score = 0;
			//TODO Handle game over (once Draw is implemented in NewBoard)
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
					if (piece != NewPiece.None)
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
			switch (NewPiece.Type(piece))
			{
				case NewPiece.Pawn:
					weight = NewPiece.IsColor(piece, NewPiece.White) ? 10 : -10;
					break;
				case NewPiece.Rook:
					weight = NewPiece.IsColor(piece, NewPiece.White) ? 50 : -50;
					break;
				case NewPiece.Bishop:
					weight = NewPiece.IsColor(piece, NewPiece.White) ? 30 : -30;
					break;
				case NewPiece.Knight:
					weight = NewPiece.IsColor(piece, NewPiece.White) ? 30 : -30;
					break;
				case NewPiece.Queen:
					weight = NewPiece.IsColor(piece, NewPiece.White) ? 90 : -90;
					break;
				case NewPiece.King:
					weight = NewPiece.IsColor(piece, NewPiece.White) ? 1000 : -1000;
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
			switch (NewPiece.Type(piece))
			{
				case NewPiece.Pawn:
					weight = NewPiece.IsColor(piece, NewPiece.White) ? PositionWeights.pawnWeight[63 - index] : -PositionWeights.pawnWeight[index];
					break;
				case NewPiece.Rook:
					weight = NewPiece.IsColor(piece, NewPiece.White) ? PositionWeights.rookWeight[63 - index] : -PositionWeights.rookWeight[index];
					break;
				case NewPiece.Bishop:
					weight = NewPiece.IsColor(piece, NewPiece.White) ? PositionWeights.bishopWeight[63 - index] : -PositionWeights.bishopWeight[index];
					break;
				case NewPiece.Knight:
					weight = NewPiece.IsColor(piece, NewPiece.White) ? PositionWeights.knightWeight[63 - index] : -PositionWeights.knightWeight[index];
					break;
				case NewPiece.Queen:
					weight = NewPiece.IsColor(piece, NewPiece.White) ? PositionWeights.queenWeight[63 - index] : -PositionWeights.queenWeight[index];
					break;
				case NewPiece.King:
					weight = NewPiece.IsColor(piece, NewPiece.White) ? PositionWeights.kingWeight[63 - index] : -PositionWeights.kingWeight[index];
					break;
				default:
					throw new System.Exception($"Invalid piece type: {piece}");
			}
			return weight;
		}

		//Other evaluation functions
	}
}