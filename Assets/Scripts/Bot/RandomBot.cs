using System;
using Chess;

namespace Bot
{
	public class RandomBot : Bot
	{
		Random rng = new Random();

		public Move GetBestMove(string fen)
		{
			try
			{
				Board board = Board.FromFEN(fen);
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
	}
}