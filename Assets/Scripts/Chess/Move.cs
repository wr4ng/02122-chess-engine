namespace Chess
{
	public class Move
	{
		// Base move informaton (from -> to)
		(int file, int rank) startSquare, endSquare;

		// Capture moves
		bool isCapture;
		(int file, int rank) captureSquare;
		Piece capturedPiece;

		// En Passant moves
		bool isEnPassant;

		// Castling moves
		bool isCastle;
		(int file, int rank) rookStart, rookEnd;
		// TODO Handle updating castling rights when castling

		// TODO Promotion
		// bool isPromotion;

		public (int file, int rank) GetStartSquare() => startSquare;
		public (int file, int rank) GetEndSquare() => endSquare;
		public Piece GetCapturedPiece() => capturedPiece;

		private Move((int file, int rank) start, (int file, int rank) end)
		{
			startSquare = start;
			endSquare = end;
		}

		public static Move SimpleMove((int file, int rank) start, (int file, int rank) end)
		{
			return new Move(start, end);
		}

		public static Move CaptureMove((int file, int rank) start, (int file, int rank) end, Piece capturedPiece)
		{
			// CaptureMove is a SimpleMove with capture information
			Move move = SimpleMove(start, end);
			move.isCapture = true;
			move.capturedPiece = capturedPiece;
			move.captureSquare = end;
			return move;
		}

		public static Move EnPassantMove((int file, int rank) start, (int file, int rank) end, Piece capturedPiece, (int file, int rank) captureSquare)
		{
			// EnPassantMove is CaptureMove with different endSquare and captureSquare
			Move move = CaptureMove(start, end, capturedPiece);
			move.captureSquare = captureSquare;
			move.isEnPassant = true;
			return move;
		}

		public static Move CastleMove((int file, int rank) start, (int file, int rank) end, (int file, int rank) rookStart, (int file, int rank) rookEnd)
		{
			// CastleMove is a simple king move with additional rook move
			Move move = SimpleMove(start, end);
			move.rookStart = rookStart;
			move.rookEnd = rookEnd;
			move.isCastle = true;
			return move;
		}

		// TODO Move PromotionMove(...)

		// TODO Move to Board.cs
		public void MakeMove(Board board)
		{
			// TODO Handle promotion
			// If move is a capture, remove captured piece
			if (isCapture)
			{
				board.SetPiece(captureSquare, null);
			}
			// If move is a castle, move rook
			else if (isCastle)
			{
				board.SetPiece(rookEnd, board.GetPiece(rookStart));
				board.SetPiece(rookStart, null);
			}
			// Move main piece
			board.SetPiece(endSquare, board.GetPiece(startSquare));
			board.SetPiece(startSquare, null);

			board.SwapPlayer();
		}
		public void UnmakeMove(Board board)
		{
			// TODO Handle promotion
			// Move main piece back
			board.SetPiece(startSquare, board.GetPiece(endSquare));
			board.SetPiece(endSquare, null);
			// If castle, move rook back
			if (isCastle)
			{
				board.SetPiece(rookStart, board.GetPiece(rookEnd));
				board.SetPiece(rookEnd, null);
			}
			// If capture, re-add captured piece
			if (isCapture)
			{
				board.SetPiece(captureSquare, capturedPiece);
			}
			board.SwapPlayer();
		}
	}
}