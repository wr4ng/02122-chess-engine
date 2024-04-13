namespace Chess
{
	public struct NewMove
	{
		public (int file, int rank) from, to;
		public int capturedPiece; // = NewPiece.None if move isn't capture

		//TODO enPassant, castle, promotion

		public NewMove((int file, int rank) from, (int file, int rank) to, int capturedPiece)
		{
			this.from = from;
			this.to = to;
			this.capturedPiece = capturedPiece;
		}

		public override readonly string ToString() => $"Move: {from} -> {to}. Capture: {NewPiece.ToString(capturedPiece)}";
	}
}