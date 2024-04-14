namespace Chess
{
	public struct NewMove
	{
		public (int file, int rank) from, to;
		public int capturedPiece; // = NewPiece.None if move isn't capture
		public int promotionType; // The type to promote to
		public bool isEnPassantCapture;

		//TODO enPassant, castle, promotion

		// Standard NewMove constructor
		public NewMove((int file, int rank) from, (int file, int rank) to, int capturedPiece, bool isEnPassantCapture = false)
		{
			this.from = from;
			this.to = to;
			this.capturedPiece = capturedPiece;
			promotionType = NewPiece.None;
			this.isEnPassantCapture = isEnPassantCapture;
		}

		// Used for promotion moves
		public NewMove((int file, int rank) from, (int file, int rank) to, int capturedPiece, int promotionType)
		{
			this.from = from;
			this.to = to;
			this.capturedPiece = capturedPiece;
			this.promotionType = promotionType;
			isEnPassantCapture = false;
		}

		public override readonly string ToString() => $"Move: {from} -> {to}. Capture: {NewPiece.ToString(capturedPiece)}. Promotion: {NewPiece.ToString(promotionType)}";
	}
}