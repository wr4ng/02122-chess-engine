namespace Chess
{
    public class Move
    {
        public (int, int)[] start, end;
        bool isCastle;
        public bool isEnPassant;
        bool isPromotion;
		private PieceType promotionPieceType;
        private Piece removedPiece;

        public Move((int, int) start, (int, int) end)
        {
            this.start = new (int, int)[1] { start };
            this.end = new (int, int)[1] { end };
        }

        public Move((int, int)[] start, (int, int)[] end)
        {
            this.start = start;
            this.end = end;
        }

		public static Move Promotion((int, int) start, (int, int) end, PieceType promotionPieceType)
		{
			Move m = new Move(start, end);
			m.isPromotion = true;
			m.promotionPieceType = promotionPieceType;
			return m;
		}

        public void SetEnpassant(bool isEnPassant)
        {
            this.isEnPassant = isEnPassant;
        }
        public void SetRemovedPiece(Piece piece)
        {
            removedPiece = piece;
        }
        public Piece GetRemovedPiece()
        {
            return removedPiece;
        }
        public void MakeMove(Board board)
        {
			if (isPromotion)
			{
				SetRemovedPiece(board.GetPiece(end[0]));
				board.SetPiece(end[0], new Piece(promotionPieceType, board.GetCurrentPlayer()));
				board.SetPiece(start[0], null);
				return;
			}
            for (int i = 0; i < start.Length; i++)
            {
                this.SetRemovedPiece(board.GetPiece(end[i]));
                board.SetPiece(end[i], board.GetPiece(start[i]));
                board.SetPiece(start[i], null);
                board.SwapPlayer();
            }
        }
        public void UnmakeMove(Board board)
        {
			if (isPromotion)
			{
				board.SetPiece(end[0], removedPiece);
				board.SetPiece(start[0], new Piece(PieceType.Pawn, board.GetCurrentPlayer().Opposite()));
				return;
			}
            for (int i = 0; i < start.Length; i++)
            {
                board.SetPiece(start[0], board.GetPiece(end[0]));
                board.SetPiece(end[0], this.GetRemovedPiece());
                board.SwapPlayer();
            }
        }
    }
}