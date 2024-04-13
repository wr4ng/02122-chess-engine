namespace Chess
{
	public struct NewMove
	{
		public (int file, int rank) from, to;

		public NewMove((int file, int rank) from, (int file, int rank) to)
		{
			this.from = from;
			this.to = to;
		}

		public override string ToString() => $"Move: {from} -> {to}";
	}
}