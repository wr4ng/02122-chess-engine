namespace Chess
{
	public struct NewMove
	{
		public (int file, int rank) from, to;
		public int capturedPiece; // = NewPiece.None if move isn't capture
		public int promotionType; // The type to promote to

		//TODO enPassant, castle, promotion

		public NewMove((int file, int rank) from, (int file, int rank) to, int capturedPiece)
		{
			this.from = from;
			this.to = to;
			this.capturedPiece = capturedPiece;
			promotionType = NewPiece.None;
		}

		public NewMove((int file, int rank) from, (int file, int rank) to, int capturedPiece, int promotionType)
		{
			this.from = from;
			this.to = to;
			this.capturedPiece = capturedPiece;
			this.promotionType = promotionType;
		}

		public override readonly string ToString() => $"Move: {from} -> {to}. Capture: {NewPiece.ToString(capturedPiece)}. Promotion: {NewPiece.ToString(promotionType)}";
	}
}