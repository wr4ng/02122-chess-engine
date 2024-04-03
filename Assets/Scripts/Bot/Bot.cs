using Chess;

// TODO Rename namespace to avoid having to use Bot.Bot
namespace Bot
{
	public interface Bot
	{
		public Move GetBestMove(Board board);
		public string Name();
	}

	// Int values are used when parsing since Unity events cannot pass Enums
	public enum BotType
	{
		RandomBot = 0,
		TempName = 1
	}

	public static class BotTypesExtensions
	{
		public static Bot CreateBot(this BotType botType)
		{
			Bot bot = botType switch
			{
				BotType.RandomBot => new RandomBot(),
				BotType.TempName => new WhiteBoi(depth: 3),
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