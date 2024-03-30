using Chess;

// TODO Rename namespace to avoid having to use Bot.Bot
namespace Bot
{
	public interface Bot
	{
		public Move GetBestMove(Board board);
	}
}