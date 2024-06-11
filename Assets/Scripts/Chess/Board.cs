using System;
using System.Collections.Generic;

namespace Chess
{
	public class Board
	{
		public int[,] squares;
		public int colorToMove;
		public int oppositeColor => colorToMove == Piece.White ? Piece.Black : Piece.White;

		public (int file, int rank)[] kingSquares; // 0 = White, 1 = King

		public MoveGenerator moveGenerator;

		public Stack<Move> playedMoves;

		// En passant
		public (int file, int rank) enPassantSquare;
		public Stack<(int file, int rank)> previousEnPassantSquares;

		// Castling rights
		public CastlingRights castlingRights;
		public Stack<CastlingRights> previousCastlingRights;

		// Zobrist
		public ulong hash;
		public Stack<ulong> previousHashes;

		// Draw
		public int halfMoveClock;
		public Stack<int> previousHalfMoveClock;
		public int fullMoveNumber;
		public Dictionary<ulong, int> repetitionMap;
		public DrawState drawState;

		public enum DrawState
		{
			None,
			FiftyMoveRule,
			Repetition
		}

		public Board()
		{
			squares = new int[8, 8];
			colorToMove = Piece.White;
			kingSquares = new (int file, int rank)[2];
			moveGenerator = new MoveGenerator(this);
			playedMoves = new();

			enPassantSquare = (-1, -1);
			previousEnPassantSquares = new();

			castlingRights = CastlingRights.None;
			previousCastlingRights = new();

			hash = 0;
			previousHashes = new();

			halfMoveClock = 0;
			fullMoveNumber = 0;
			previousHalfMoveClock = new();
			repetitionMap = new();
			drawState = DrawState.None;
		}

		public static Board FromFEN(string fen)
		{
			Board board = new();

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
					if (file > 8)
					{
						throw new System.ArgumentException("Invalid file length!");
					}
				}
				else if (c == '/')
				{
					if (file != 8)
					{
						throw new System.ArgumentException("Invalid file length!");
					}
					file = 0;
					rank--;
					if (rank < 0)
					{
						throw new System.ArgumentException($"Too many ranks!");
					}
				}
				else
				{
					int color = char.IsUpper(c) ? Piece.White : Piece.Black;
					int type = char.ToLower(c) switch
					{
						'p' => Piece.Pawn,
						'b' => Piece.Bishop,
						'n' => Piece.Knight,
						'r' => Piece.Rook,
						'q' => Piece.Queen,
						'k' => Piece.King,
						_ => throw new System.ArgumentException($"Invalid char found when parsing board: {c}")
					};
					// Save king positions
					if (type == Piece.King)
					{
						board.kingSquares[Piece.ColorIndex(color)] = (file, rank);
						// Increment number of kings
						if (color == Piece.White)
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
				"w" => Piece.White,
				"b" => Piece.Black,
				_ => throw new System.ArgumentException($"Invalid current player: {fenParts[1]}")
			};
			// Parse castling rights
			board.castlingRights = FEN.ParseCastlingRights(fenParts[2]);
			// Parse en Passant square
			board.enPassantSquare = FEN.ParseEnPassant(fenParts[3]);
			board.halfMoveClock = FEN.ParseHalfmoveClock(fenParts[4]);
			board.fullMoveNumber = FEN.ParseFullmoveNumber(fenParts[5]);

			// Calculate zobrist hash
			board.hash = Zobrist.GenerateHash(board);
			board.repetitionMap[board.hash] = 1;

			return board;
		}

		public string ToFEN()
		{
			string board = FEN.BoardToFEN(this);
			string player = FEN.ColorToFEN(colorToMove);
			string castling = castlingRights.ToFEN();
			string ep = FEN.EnPassantToFEN(enPassantSquare);
			string halfmove = halfMoveClock.ToString();
			string fullmove = fullMoveNumber.ToString();
			return $"{board} {player} {castling} {ep} {halfmove} {fullmove}";
		}

		public override string ToString()
		{
			string board = "";
			for (int rank = 7; rank >= 0; rank--)
			{
				for (int file = 0; file < 8; file++)
				{
					char type = Piece.Type(squares[file, rank]) switch
					{
						Piece.None => '-',
						Piece.Pawn => 'p',
						Piece.Bishop => 'b',
						Piece.Knight => 'n',
						Piece.Rook => 'r',
						Piece.Queen => 'q',
						Piece.King => 'k',
						_ => '?' // Shoudn't be reached. Then squares[file, rank] has invalid type value (3 least significant bits)
					};
					char piece = Piece.IsColor(squares[file, rank], Piece.White) ? char.ToUpper(type) : type;
					board += piece;
				}
				board += "\n";
			}
			return board.Trim();
		}

		public void MakeMove(Move move)
		{
			// Push previous hash to stack
			previousHashes.Push(hash);

			int piece = squares[move.from.file, move.from.rank];

			// Update halfMoveClock
			previousHalfMoveClock.Push(halfMoveClock);
			if (Piece.Type(piece) == Piece.Pawn || move.capturedPiece != Piece.None)
			{
				halfMoveClock = 0;
			}
			else
			{
				halfMoveClock++;
				if (halfMoveClock >= 100)
				{
					drawState = DrawState.FiftyMoveRule;
				}
			}

			// Remove piece from starting square
			squares[move.from.file, move.from.rank] = Piece.None;
			hash ^= Zobrist.pieces[Piece.ColorIndex(piece), Piece.TypeIndex(piece), move.from.file, move.from.rank];

			// Handle promotion
			if (move.promotionType != Piece.None)
			{
				// Add promoted piece to move.to
				squares[move.to.file, move.to.rank] = colorToMove | move.promotionType;
				hash ^= Zobrist.pieces[Piece.ColorIndex(colorToMove), Piece.TypeIndex(move.promotionType), move.to.file, move.to.rank];
			}
			else
			{
				// Add moved piece to move.to
				squares[move.to.file, move.to.rank] = piece;
				hash ^= Zobrist.pieces[Piece.ColorIndex(piece), Piece.TypeIndex(piece), move.to.file, move.to.rank];
			}

			// Handle non-ep capture (Zobrist)
			if (move.capturedPiece != Piece.None && !move.isEnPassantCapture)
			{
				// Remove captured piece from zobrist
				hash ^= Zobrist.pieces[Piece.ColorIndex(oppositeColor), Piece.TypeIndex(move.capturedPiece), move.to.file, move.to.rank];
			}

			// En Passant
			// Add previous EP square to stack
			previousEnPassantSquares.Push(enPassantSquare);
			// If we previously had EP square, remove it from hash
			if (enPassantSquare != (-1, -1))
			{
				hash ^= Zobrist.enpassant[enPassantSquare.file];
			}
			// If current move is double pawn move, set en Passant square (if piece is pawn and moves 2 ranks)
			bool doublePawnMove = (Piece.Type(piece) == Piece.Pawn) && (System.Math.Abs(move.from.rank - move.to.rank) == 2);
			if (doublePawnMove)
			{
				enPassantSquare = (move.from.file, move.from.rank + (colorToMove == Piece.White ? 1 : -1));
				hash ^= Zobrist.enpassant[move.from.file];
			}
			else
			{
				enPassantSquare = (-1, -1);
			}

			// Check for en Passant capture
			if (move.isEnPassantCapture)
			{
				// Remove pawn 1 below move.to
				int forward = colorToMove == Piece.White ? 1 : -1;
				squares[move.to.file, move.to.rank - forward] = Piece.None;
				hash ^= Zobrist.pieces[Piece.ColorIndex(oppositeColor), Piece.TypeIndex(Piece.Pawn), move.to.file, move.to.rank - forward];
			}
			// If castle, also move rook
			if (move.isCastle)
			{
				// Calculate castle direction based on move.to.file. Queenside = c-file = 2. Kingside = g-file = 6
				bool isKingside = move.to.file == 6;
				// Get rank of king
				int kingRank = colorToMove == Piece.White ? 0 : 7;
				if (isKingside)
				{
					// Move rook
					squares[7, kingRank] = Piece.None;
					squares[5, kingRank] = colorToMove | Piece.Rook;
					hash ^= Zobrist.pieces[Piece.ColorIndex(colorToMove), Piece.TypeIndex(Piece.Rook), 7, kingRank];
					hash ^= Zobrist.pieces[Piece.ColorIndex(colorToMove), Piece.TypeIndex(Piece.Rook), 5, kingRank];
				}
				else
				{
					// Move rook
					squares[0, kingRank] = Piece.None;
					squares[3, kingRank] = colorToMove | Piece.Rook;
					hash ^= Zobrist.pieces[Piece.ColorIndex(colorToMove), Piece.TypeIndex(Piece.Rook), 0, kingRank];
					hash ^= Zobrist.pieces[Piece.ColorIndex(colorToMove), Piece.TypeIndex(Piece.Rook), 3, kingRank];
				}
			}
			// Add previous castling rights to stack
			previousCastlingRights.Push(castlingRights);
			// Calculate new castling rights
			// If any piece moves to or from one of the corners, or the king moves, remove the corresponding castling right (ie. rook being captured or moved)
			// White Kingside
			if (castlingRights.HasFlag(CastlingRights.WhiteKingside) && ((colorToMove == Piece.White && Piece.Type(piece) == Piece.King) || move.from == (7, 0) || move.to == (7, 0)))
			{
				castlingRights = castlingRights.ClearRights(CastlingRights.WhiteKingside);
				hash ^= Zobrist.castle[0];
			}
			// White Queenside
			if (castlingRights.HasFlag(CastlingRights.WhiteQueenside) && ((colorToMove == Piece.White && Piece.Type(piece) == Piece.King) || move.from == (0, 0) || move.to == (0, 0)))
			{
				castlingRights = castlingRights.ClearRights(CastlingRights.WhiteQueenside);
				hash ^= Zobrist.castle[1];
			}
			// Black Kingside
			if (castlingRights.HasFlag(CastlingRights.BlackKingside) && ((colorToMove == Piece.Black && Piece.Type(piece) == Piece.King) || move.from == (7, 7) || move.to == (7, 7)))
			{
				castlingRights = castlingRights.ClearRights(CastlingRights.BlackKingside);
				hash ^= Zobrist.castle[2];
			}
			// Black Queenside
			if (castlingRights.HasFlag(CastlingRights.BlackQueenside) && ((colorToMove == Piece.Black && Piece.Type(piece) == Piece.King) || move.from == (0, 7) || move.to == (0, 7)))
			{
				castlingRights = castlingRights.ClearRights(CastlingRights.BlackQueenside);
				hash ^= Zobrist.castle[3];
			}
			// Update king position
			if (Piece.Type(piece) == Piece.King)
			{
				kingSquares[Piece.ColorIndex(colorToMove)] = move.to;
			}
			// Swap player
			colorToMove = oppositeColor;
			hash ^= Zobrist.side;
			// Add hash to repetition map
			if (repetitionMap.ContainsKey(hash))
			{
				repetitionMap[hash] += 1;
				if (repetitionMap[hash] >= 3)
				{
					drawState = DrawState.Repetition;
				}
			}
			else
			{
				repetitionMap[hash] = 1;
			}

			// Add move to stack of played moves
			playedMoves.Push(move);
			fullMoveNumber++;
		}

		public void UnmakeMove(Move move)
		{
			// Check if we're trying to unmake a move that hasn't been played yet
			bool hasMoves = playedMoves.TryPeek(out Move topMove);
			bool matchesTopMove = hasMoves && move.Equals(topMove);
			if (!matchesTopMove)
			{
				throw new System.ArgumentException($"Trying to unmake move which isn't the top move!\nPlayed: {move}\nTop: {topMove}");
			}
			// If it does match, remove it from the stack
			playedMoves.Pop();

			// Get moving piece
			bool isPromotion = move.promotionType != Piece.None;
			int piece = isPromotion ? (oppositeColor | Piece.Pawn) : squares[move.to.file, move.to.rank];

			// Move back
			squares[move.from.file, move.from.rank] = piece;

			// Add captured piece back in (can be Piece.None)
			squares[move.to.file, move.to.rank] = move.capturedPiece;

			// Set previous en Passant square
			enPassantSquare = previousEnPassantSquares.Pop();
			// Check if en Passant capture
			if (move.isEnPassantCapture)
			{
				// Add pawn back for the current player
				int forward = colorToMove == Piece.White ? 1 : -1;
				squares[move.to.file, move.to.rank + forward] = colorToMove | Piece.Pawn;
			}
			// Update king square
			if (Piece.Type(piece) == Piece.King)
			{
				kingSquares[Piece.ColorIndex(oppositeColor)] = move.from;
			}
			//If castle, move rook back
			if (move.isCastle)
			{
				// Calculate castle direction based on move.to.file. Queenside = c-file = 2. Kingside = g-file = 6
				bool isKingside = move.to.file == 6;
				// Get rank of king
				int kingRank = oppositeColor == Piece.White ? 0 : 7;
				if (isKingside)
				{
					// Move rook back
					squares[7, kingRank] = oppositeColor | Piece.Rook;
					squares[5, kingRank] = Piece.None;
				}
				else
				{
					// Move rook back
					squares[0, kingRank] = oppositeColor | Piece.Rook;
					squares[3, kingRank] = Piece.None;
				}
			}
			// Set previous castling rights
			castlingRights = previousCastlingRights.Pop();

			// Swap player
			colorToMove = oppositeColor;

			// Remove one from hash in repetition map
			repetitionMap[hash] = Math.Max(0, repetitionMap[hash] - 1);

			// Set previous hash
			hash = previousHashes.Pop();

			drawState = DrawState.None;

			fullMoveNumber--;
		}

		public void UndoPreviousMove()
		{
			bool hasMove = playedMoves.TryPeek(out Move move);
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
			foreach (Move m in moves)
			{
				MakeMove(m);
				positions += GetNumberOfPositions(depth - 1);
				UndoPreviousMove();
			}
			return positions;
		}
	}
}