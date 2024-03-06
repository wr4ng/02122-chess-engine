namespace Chess
{
	public class Util
	{
		public static bool InBoard((int, int) position)
		{
			return position.Item1 >= 0 && position.Item1 < 8 && position.Item2 >= 0 && position.Item2 < 8;
		}

		public static (int, int) AddTuples((int, int) a, (int, int) b)
		{
			return (a.Item1 + b.Item1, a.Item2 + b.Item2);
		}
	}
}