namespace Chess
{
	public class Move
	{
		// Base move informaton (from -> to)
		(int file, int rank) startSquare, endSquare;
		PieceType pieceType;

		// Capture moves
		bool isCapture;
		(int file, int rank) captureSquare;
		Piece capturedPiece;

		// En Passant moves
		bool isEnPassant;
		bool isDoublePawnMove;
		(int file, int rank) enPassantSquare;
		(int file, int rank) prevEnPassantSquare = (-1,-1);

		// Castling moves
		bool isCastle;
		(int file, int rank) rookStart, rookEnd;
		CastlingRights prevCastlingRights;

		// TODO Handle updating castling rights when castling

		// Promotion moves
		bool isPromotion;
		PieceType promotionPiecetype;

		public (int file, int rank) GetStartSquare() => startSquare;
		public (int file, int rank) GetEndSquare() => endSquare;
		public PieceType GetPieceType() => pieceType;

		public bool IsCapture() => isCapture;
		public (int file, int rank) GetCaptureSquare() => captureSquare;
		public Piece GetCapturedPiece() => capturedPiece;

		public bool IsEnPassant() => isEnPassant;
		public bool IsDoublePawnMove() => isDoublePawnMove;
		public (int file, int rank) GetEnPassantSquare() => enPassantSquare;
		public (int file, int rank) GetPrevEnPassantSquare() => prevEnPassantSquare;
		public void SetPrevEnPassantSquare((int file, int rank) prevSquare) =>  prevEnPassantSquare = prevSquare;

		public CastlingRights GetPrevCastlingRights() => prevCastlingRights;
		public void SetPrevCastlingRights(CastlingRights prevRights) =>  prevCastlingRights = prevRights;

		public bool IsCastle() => isCastle;
		public (int file, int rank) GetRookStart() => rookStart;
		public (int file, int rank) GetRookEnd() => rookEnd;

		public bool IsPromotion() => isPromotion;
		public PieceType PromotionPieceType() => promotionPiecetype;

		private Move((int file, int rank) start, (int file, int rank) end, PieceType type)
		{
			startSquare = start;
			endSquare = end;
			pieceType = type;
		}

		public static Move SimpleMove((int file, int rank) start, (int file, int rank) end, PieceType pieceType)
		{
			return new Move(start, end, pieceType);
		}

		public static Move CaptureMove((int file, int rank) start, (int file, int rank) end, PieceType pieceType, Piece capturedPiece)
		{
			// CaptureMove is a SimpleMove with capture information
			Move move = SimpleMove(start, end, pieceType);
			move.isCapture = true;
			move.capturedPiece = capturedPiece;
			move.captureSquare = end;
			return move;
		}
		public static Move DoublePawnMove((int file, int rank) start, (int file, int rank) end, (int file, int rank) enPassantSquare){
			Move move = SimpleMove(start, end, PieceType.Pawn);
			move.isDoublePawnMove = true;
			move.enPassantSquare = enPassantSquare;
			return move;
		}

		public static Move EnPassantMove((int file, int rank) start, (int file, int rank) end, Piece capturedPiece, (int file, int rank) captureSquare)
		{
			// EnPassantMove is CaptureMove with different endSquare and captureSquare
			Move move = CaptureMove(start, end, PieceType.Pawn, capturedPiece);
			move.captureSquare = captureSquare;
			move.isEnPassant = true;
			return move;
		}

		public static Move CastleMove((int file, int rank) start, (int file, int rank) end, (int file, int rank) rookStart, (int file, int rank) rookEnd)
		{
			// CastleMove is a simple king move with additional rook move
			Move move = SimpleMove(start, end, PieceType.King);
			move.rookStart = rookStart;
			move.rookEnd = rookEnd;
			move.isCastle = true;
			return move;
		}

		public static Move PromotionMove((int, int) start, (int, int) end, PieceType promotionPieceType, bool isCapture = false, Piece capturedPiece = null)
		{
			Move move = new Move(start, end, PieceType.Pawn);
			move.isPromotion = true;
			move.promotionPiecetype = promotionPieceType;
			move.isCapture = isCapture;
			move.capturedPiece = capturedPiece;
			return move;
		}
	}
}