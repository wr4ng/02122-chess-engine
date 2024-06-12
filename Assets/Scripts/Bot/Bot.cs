using Chess;

namespace Bot
{
	public interface Bot
	{
		public Move GetBestMove(Board board);
		public string Name();
		public void UpdateOp();
	}

	// Int values are used when parsing since Unity events cannot pass Enums
	public enum BotType
	{
		RandomBot = 0,
		WhiteBoi = 1
	}

	public static class BotTypesExtensions
	{
		public static Bot CreateBot(this BotType botType, int depth, Board board)
		{
			Bot bot = botType switch
			{
				BotType.RandomBot => new RandomBot(board),
				BotType.WhiteBoi => new WhiteBoi(depth, board),
				_ => null
			};
			if (bot == null)
			{
				UnityEngine.Debug.LogError($"Invalid BotType: {botType}");
			}
			return bot;
		}
	}
}