using System.Collections.Generic;

namespace Chess
{
	public class NewBoard
	{
		public int[,] squares;
		public int colorToMove;
		public int oppositeColor => colorToMove == NewPiece.White ? NewPiece.Black : NewPiece.White;

		public (int file, int rank)[] kingSquares; // 0 = White, 1 = King

		public NewMoveGenerator moveGenerator;

		public Stack<NewMove> playedMoves;

		// En passant
		public (int file, int rank) enPassantSquare;
		public Stack<(int file, int rank)> previousEnPassantSquares;

		// Castling rights
		public CastlingRights castlingRights;
		public Stack<CastlingRights> previousCastlingRights;


		public static int ColorIndex(int color) => (color == NewPiece.White) ? 0 : 1;

		public NewBoard()
		{
			squares = new int[8, 8];
			colorToMove = NewPiece.White;
			kingSquares = new (int file, int rank)[2];
			moveGenerator = new NewMoveGenerator(this);
			playedMoves = new();

			enPassantSquare = (-1, -1);
			previousEnPassantSquares = new();

			castlingRights = CastlingRights.None;
			previousCastlingRights = new();
		}

		public static NewBoard FromFEN(string fen)
		{
			NewBoard board = new();

			string[] fenParts = fen.Split(' ');
			if (fenParts.Length != 6)
			{
				throw new System.ArgumentException("Invalid number of FEN parts!");
			}

			// Parse board
			string fenBoard = fenParts[0];
			int file = 0, rank = 7;
			int numWhiteKings = 0, numBlackKings = 0;

			foreach (char c in fenBoard)
			{
				if (char.IsDigit(c))
				{
					file += (int)char.GetNumericValue(c);
				}
				else if (c == '/')
				{
					//TODO Handle invalid row length
					file = 0;
					rank--;
				}
				else
				{
					int color = char.IsUpper(c) ? NewPiece.White : NewPiece.Black;
					int type = char.ToLower(c) switch
					{
						'p' => NewPiece.Pawn,
						'b' => NewPiece.Bishop,
						'n' => NewPiece.Knight,
						'r' => NewPiece.Rook,
						'q' => NewPiece.Queen,
						'k' => NewPiece.King,
						_ => throw new System.ArgumentException($"Invalid char found when parsing board: {c}")
					};
					// Save king positions
					if (type == NewPiece.King)
					{
						board.kingSquares[ColorIndex(color)] = (file, rank);
						// Increment number of kings
						if (color == NewPiece.White)
							numWhiteKings++;
						else
							numBlackKings++;
					}

					board.squares[file, rank] = color | type;
					file++;
				}
			}
			// Verify number of kings
			if (numWhiteKings != 1 || numBlackKings != 1)
			{
				throw new System.ArgumentException("Invalid number of kings!");
			}
			// Parse current player
			board.colorToMove = fenParts[1] switch
			{
				"w" => NewPiece.White,
				"b" => NewPiece.Black,
				_ => throw new System.ArgumentException($"Invalid current player: {fenParts[1]}")
			};
			// Parse castling rights
			board.castlingRights = FEN.ParseCastlingRights(fenParts[2]);
			// Parse en Passant square
			board.enPassantSquare = FEN.ParseEnPassant(fenParts[3]);
			// TODO halfMoveClock
			// TODO fullMoveNumber
			return board;
		}

		public override string ToString()
		{
			string board = "";
			for (int rank = 7; rank >= 0; rank--)
			{
				for (int file = 0; file < 8; file++)
				{
					char type = NewPiece.Type(squares[file, rank]) switch
					{
						NewPiece.None => '*',
						NewPiece.Pawn => 'p',
						NewPiece.Bishop => 'b',
						NewPiece.Knight => 'n',
						NewPiece.Rook => 'r',
						NewPiece.Queen => 'q',
						NewPiece.King => 'k',
						_ => '?' // Shoudn't be reached. Then squares[file, rank] has invalid type value (3 least significant bits)
					};
					char piece = (squares[file, rank] & 0b11000) == NewPiece.White ? char.ToUpper(type) : type;
					board += piece;
				}
				board += "\n";
			}
			return board;
		}

		public void MakeMove(NewMove move)
		{
			int piece = squares[move.from.file, move.from.rank];
			// Move piece
			squares[move.from.file, move.from.rank] = NewPiece.None;
			squares[move.to.file, move.to.rank] = piece;

			// Handle promotion
			if (move.promotionType != NewPiece.None)
			{
				squares[move.to.file, move.to.rank] = colorToMove | move.promotionType;
			}
			// En Passant
			// Add previous EP square to stack
			previousEnPassantSquares.Push(enPassantSquare);
			// If current move is double pawn move, set en Passant square (is piece is pawn is moves 2 ranks)
			bool doublePawnMove = (NewPiece.Type(piece) == NewPiece.Pawn) && (System.Math.Abs(move.from.rank - move.to.rank) == 2);
			if (doublePawnMove)
			{
				enPassantSquare = (move.from.file, move.from.rank + (colorToMove == NewPiece.White ? 1 : -1));
			}
			else
			{
				enPassantSquare = (-1, -1);
			}
			// Check for en Passant capture
			if (move.isEnPassantCapture)
			{
				// Remove pawn 1 below move.to
				int forward = colorToMove == NewPiece.White ? 1 : -1;
				squares[move.to.file, move.to.rank - forward] = NewPiece.None;

			}
			// If castle, also move rook
			if (move.isCastle)
			{
				// Calculate castle direction based on move.to.file. Queenside = c-file = 2. Kingside = g-file = 6
				bool isKingside = move.to.file == 6;
				// Get rank of king
				int kingRank = colorToMove == NewPiece.White ? 0 : 7;
				// TODO This could be done simpler
				if (isKingside)
				{
					// Move rook
					squares[7, kingRank] = NewPiece.None;
					squares[5, kingRank] = colorToMove | NewPiece.Rook;
				}
				else
				{
					// Move rook
					squares[0, kingRank] = NewPiece.None;
					squares[3, kingRank] = colorToMove | NewPiece.Rook;
				}
			}
			// Add previous castling rights to stack
			previousCastlingRights.Push(castlingRights);
			// Calculate new castling rights
			// If the king moves, remove all castling rights for it
			if (NewPiece.Type(piece) == NewPiece.King)
			{
				if (colorToMove == NewPiece.White)
					castlingRights = castlingRights.ClearRights(CastlingRights.AllWhite);
				else
					castlingRights = castlingRights.ClearRights(CastlingRights.AllBlack);
			}
			// If any piece moves to or from one of the corners, remove the corresponding castling right (ie. rook being captured or moved)
			// White Kingside
			if (move.from == (7, 0) || move.to == (7, 0))
				castlingRights = castlingRights.ClearRights(CastlingRights.WhiteKingside);
			// White Queenside
			if (move.from == (0, 0) || move.to == (0, 0))
				castlingRights = castlingRights.ClearRights(CastlingRights.WhiteQueenside);
			// Black Kingside
			if (move.from == (7, 7) || move.to == (7, 7))
				castlingRights = castlingRights.ClearRights(CastlingRights.BlackKingside);
			// Black Queenside
			if (move.from == (0, 7) || move.to == (0, 7))
				castlingRights = castlingRights.ClearRights(CastlingRights.BlackQueenside);
			// Update king position
			if (NewPiece.Type(piece) == NewPiece.King)
			{
				kingSquares[ColorIndex(colorToMove)] = move.to;
			}
			// Swap player
			colorToMove = oppositeColor;
			// Add move to stack of played moves
			playedMoves.Push(move);
		}

		public void UnmakeMove(NewMove move)
		{
			// Check if we're trying to unmake a move that hasn't been played yet
			bool hasMoves = playedMoves.TryPeek(out NewMove topMove);
			bool matchesTopMove = hasMoves && move.Equals(topMove);
			if (!matchesTopMove)
			{
				throw new System.ArgumentException($"Trying to unmake move which isn't the top move!\nPlayoed: {move}\nTop: {topMove}");
			}
			// If it does match, remove it from the stack
			playedMoves.Pop();

			// Get moving piece
			bool isPromotion = move.promotionType != NewPiece.None;
			int piece = isPromotion ? (oppositeColor | NewPiece.Pawn) : squares[move.to.file, move.to.rank];

			// Move back
			squares[move.from.file, move.from.rank] = piece;

			// Add captured piece back in (can be NewPiece.None)
			squares[move.to.file, move.to.rank] = move.capturedPiece;

			// Set previous en Passant square
			enPassantSquare = previousEnPassantSquares.Pop();
			// Check if en Passant capture
			if (move.isEnPassantCapture)
			{
				// Add pawn back for the current player
				int forward = colorToMove == NewPiece.White ? 1 : -1;
				squares[move.to.file, move.to.rank + forward] = colorToMove | NewPiece.Pawn;
			}
			// Update king square
			if (NewPiece.Type(piece) == NewPiece.King)
			{
				kingSquares[ColorIndex(oppositeColor)] = move.from;
			}
			//If castle, move rook back
			if (move.isCastle)
			{
				// Calculate castle direction based on move.to.file. Queenside = c-file = 2. Kingside = g-file = 6
				bool isKingside = move.to.file == 6;
				// Get rank of king
				int kingRank = oppositeColor == NewPiece.White ? 0 : 7;
				// TODO This could be done simpler
				if (isKingside)
				{
					// Move rook back
					squares[7, kingRank] = oppositeColor | NewPiece.Rook;
					squares[5, kingRank] = NewPiece.None;
				}
				else
				{
					// Move rook back
					squares[0, kingRank] = oppositeColor | NewPiece.Rook;
					squares[3, kingRank] = NewPiece.None;
				}
			}
			// Set previous castling rights
			castlingRights = previousCastlingRights.Pop();

			// Swap player
			colorToMove = oppositeColor;
		}

		public void UndoPreviousMove()
		{
			bool hasMove = playedMoves.TryPeek(out NewMove move);
			if (hasMove)
			{
				UnmakeMove(move);
			}
		}

		public int GetNumberOfPositions(int depth)
		{
			if (depth <= 0)
			{
				return 1;
			}
			var moves = moveGenerator.GenerateMoves();
			if (depth == 1)
			{
				return moves.Count;
			}
			int positions = 0;
			foreach (NewMove m in moves)
			{
				MakeMove(m);
				positions += GetNumberOfPositions(depth - 1);
				UndoPreviousMove();
			}
			return positions;
		}
	}
}