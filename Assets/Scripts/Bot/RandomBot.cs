using System;
using Chess;

namespace Bot
{
	public class RandomBot : Bot
	{
		Random rng = new Random();

		public Move GetBestMove(Board board)
		{
			var possibleMoves = board.GetLegalMoves();
			int randomIndex = rng.Next(possibleMoves.Count);
			return possibleMoves[randomIndex];
		}
	}
}