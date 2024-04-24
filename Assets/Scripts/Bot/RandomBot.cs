using System;
using Chess;

namespace Bot
{
	public class RandomBot : Bot
	{
		Random rng = new Random();

		public NewMove GetBestMove(NewBoard board)
		{
			var possibleMoves = board.moveGenerator.GenerateMoves();
			int randomIndex = rng.Next(possibleMoves.Count);
			return possibleMoves[randomIndex];
		}

		public string Name() => "RandomBot";
	}
}