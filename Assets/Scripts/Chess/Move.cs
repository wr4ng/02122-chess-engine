namespace Chess
{
	public struct Move
	{
		public (int file, int rank) from, to;
		public int capturedPiece; // = Piece.None if move isn't capture
		public int promotionType; // The type to promote to
		public bool isEnPassantCapture;
		public bool isCastle;

		public Move((int file, int rank) from, (int file, int rank) to, int capturedPiece = Piece.None, bool isEnPassantCapture = false, bool isCastle = false, int promotionType = Piece.None)
		{
			this.from = from;
			this.to = to;
			this.capturedPiece = capturedPiece;
			this.promotionType = promotionType;
			this.isEnPassantCapture = isEnPassantCapture;
			this.isCastle = isCastle;
		}

		public override readonly string ToString() => $"Move: {from} -> {to}. Capture: {Piece.ToString(capturedPiece)}. Promotion: {Piece.ToString(promotionType)}. En Passant Capture: {isEnPassantCapture}. Castle: {isCastle}";
	}
}