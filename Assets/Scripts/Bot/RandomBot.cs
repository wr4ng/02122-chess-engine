using System;
using Chess;

namespace Bot
{
	public class RandomBot : Bot
	{
		Board boardOri;
		public RandomBot(Board boardOri)
		{
			this.boardOri = boardOri;
		}
		Random rng = new Random();

		public Move GetBestMove(Board board)
		{
			try
			{
				var possibleMoves = board.moveGenerator.GenerateMoves();
				int randomIndex = rng.Next(possibleMoves.Count);
				return possibleMoves[randomIndex];
			}
			catch
			{
				return new();
			}
		}

		public string Name() => "RandomBot";

		public float getEval()
		{
			return 0;
		}
	}
}