using System.Collections.Generic;

namespace Chess
{
	public class MoveGenerator
	{
		Board board;

		public List<(int file, int rank)> pinned;
		public List<(int file, int rank)> checkers;
		public bool[] attackedAroundKing;

		// Bitboards
		public ulong captureBitboard; // If king is in check, contains the locations of the checkers
		public ulong blockBitboard; // If king is in check, contains the squares where

		// Constant arrays of directiojns used for looping
		public readonly static (int dx, int dy)[] kingDirections = new (int dx, int dy)[8] { (-1, -1), (0, -1), (1, -1), (1, 0), (1, 1), (0, 1), (-1, 1), (-1, 0) };
		public readonly static (int dx, int dy)[] knightDirections = new (int dx, int dy)[8] { (-1, -2), (1, -2), (-2, -1), (2, -1), (-2, 1), (2, 1), (-1, 2), (1, 2) };
		//TODO Maybe make it (int, int)[][] instead of [,]
		public readonly static (int dx, int dy)[,] pawnDirections = new (int dx, int dy)[2, 2] { { (-1, 1), (1, 1) }, { (-1, -1), (1, -1) } };

		public MoveGenerator(Board board)
		{
			this.board = board;
			// Precompute move data
			PrecomputedData.Compute();
		}

		// Generate legal moves for the current position of board
		public List<Move> GenerateMoves(bool onlyCaptures = false)
		{
			// Check the king positions for checks and pins
			CheckKingPosition();
			EasierMoveList movesList = new EasierMoveList();
			GetKingMoves(movesList, onlyCaptures);
			// If the number of checks is > 1, then only the king can move
			if (checkers.Count > 1) { movesList.ConnectList(); return movesList.GetList(); }

			// Else, loop through all friendly pieces to generate their moves
			//TODO Use piecelists?
			for (int file = 0; file < 8; file++)
			{
				for (int rank = 0; rank < 8; rank++)
				{
					int piece = board.squares[file, rank];
					// If piece isn't friendly (excluding king as moves for king have already been generated) continue
					if ((Piece.Color(piece) != board.colorToMove) || (Piece.Type(piece) == Piece.King)) continue;
					// Check type of piece and generate appropriate moves
					int pieceType = Piece.Type(piece);

					switch (pieceType)
					{
						case Piece.Pawn:
							GetPawnMoves((file, rank), movesList, onlyCaptures);
							break;
						case Piece.Knight:
							GetKnightMoves((file, rank), movesList, onlyCaptures);
							break;
						case Piece.Bishop:
							GetSlidingMoves((file, rank), pieceType, movesList, onlyCaptures);
							break;
						case Piece.Rook:
							GetSlidingMoves((file, rank), pieceType, movesList, onlyCaptures);
							break;
						case Piece.Queen:
							GetSlidingMoves((file, rank), pieceType, movesList, onlyCaptures);
							break;
						default:
							break;
					}
				}
			}
			movesList.ConnectList();
			return movesList.GetList();
		}

		// TODO Keep track of blocking bitboard (where pieces can move to block a checker) and capture bitboard (checker location)
		public void CheckKingPosition()
		{
			// Check attack data around king
			(int kingFile, int kingRank) = board.kingSquares[Piece.ColorIndex(board.colorToMove)];

			// Which spaces we for sure know are attacked around the king after checking in each direction (to avoid re-calculating the squares in move generation)
			attackedAroundKing = new bool[8] { false, false, false, false, false, false, false, false };

			// Initialize list of pinned pieces and checking pieces
			pinned = new();
			checkers = new();

			// Initialize bitboard
			captureBitboard = 0;
			blockBitboard = 0;

			for (int dirIndex = 0; dirIndex < kingDirections.Length; dirIndex++)
			{
				var (dx, dy) = kingDirections[dirIndex];
				bool foundFriendlyPiece = false;
				(int file, int rank) friendlySquare = (-1, -1);

				ulong rayBitboard = 0;  // Bitboard having the current ray from the king. Is added to blockBitboard if checker is found

				int toEdge = PrecomputedData.squaresToEdge[kingFile][kingRank][dirIndex];
				for (int i = 1; i <= toEdge; i++)
				{
					// Calculate current square in direction (dx, dy)
					(int file, int rank) = (kingFile + dx * i, kingRank + dy * i);
					int piece = board.squares[file, rank];

					// Continue if square is empty
					if (piece == Piece.None)
					{
						rayBitboard = BitBoard.SetOne(rayBitboard, file, rank);
						continue;
					}

					// Check if piece is friendly
					if (Piece.IsColor(piece, board.colorToMove))
					{
						// If we already found a friendly piece, two friendly pieces found in this direction and there is no pin
						if (foundFriendlyPiece) break;
						else
						{
							// Save friendly piece as it may be pinned
							foundFriendlyPiece = true;
							friendlySquare = (file, rank);
						}
					}
					else
					{
						// Enemy piece. Check if it can attack in this direction
						bool isDiagonal = dx * dy != 0;
						int attackerType = Piece.Type(piece);

						// The piece can attack the king if it is a queen, or a bishop/rook with their respetive directions
						bool canAttack = (attackerType == Piece.Queen) || (isDiagonal && (attackerType == Piece.Bishop)) || (!isDiagonal && (attackerType == Piece.Rook));
						if (canAttack)
						{
							// If we found a friendly piece before, it is pinned
							if (foundFriendlyPiece)
							{
								pinned.Add(friendlySquare);
							}
							else
							{
								// King is checked by piece in the current direction. If the piece is more than 1 square away, king cannot move in that direction
								// If it is 1 square away, the king may be able to capture it, and we then don't want to skip checking it later
								attackedAroundKing[dirIndex] = i > 1;
								// Cannot move in the opposite direction, as sliding piece would still check it
								attackedAroundKing[(dirIndex + 4) % 8] = true;
								checkers.Add((file, rank));
								// Add one to the capture bitboard
								captureBitboard = BitBoard.SetOne(captureBitboard, file, rank);
								// Since we found sliding checker, add the rayBitboard to blockBitboard
								blockBitboard |= rayBitboard;
							}
						}
						break;
					}
				}
			}
			// Check for knights. Loop through precomputed knight moves
			foreach ((int file, int rank) in PrecomputedData.knightMoves[kingFile, kingRank])
			{
				// Check if enemy knight is present there
				if (board.squares[file, rank] == (board.oppositeColor | Piece.Knight))
				{
					checkers.Add((file, rank));
					// Add knight to captureBitboard
					captureBitboard = BitBoard.SetOne(captureBitboard, file, rank);
				}
			}
			// Check for pawns
			for (int i = 0; i < pawnDirections.GetLength(1); i++)
			{
				var (dx, dy) = pawnDirections[Piece.ColorIndex(board.colorToMove), i];
				(int file, int rank) = (kingFile + dx, kingRank + dy);
				// If outside board, continue
				if (!(0 <= file && file < 8 && 0 <= rank && rank < 8)) continue;
				// Check if piece is enemy pawn
				if (board.squares[file, rank] == (board.oppositeColor | Piece.Pawn))
				{
					checkers.Add((file, rank));
					// Add pawn to captureBitboard
					captureBitboard = BitBoard.SetOne(captureBitboard, file, rank);
				}
			}
			// If king isn't in check, set blockBitboard and captureBitboard to all 1's
			if (checkers.Count == 0)
			{
				captureBitboard = BitBoard.AllOnes;
				blockBitboard = BitBoard.AllOnes;
			}
		}

		public void GetKingMoves(EasierMoveList movesList, bool onlyCaptures)
		{
			(int kingFile, int kingRank) = board.kingSquares[Piece.ColorIndex(board.colorToMove)];

			for (int i = 0; i < kingDirections.Length; i++)
			{
				// Ignore squares we have already determined are attacked (because king is in check)
				if (!attackedAroundKing[i])
				{
					(int file, int rank) = (kingFile + kingDirections[i].dx, kingRank + kingDirections[i].dy);
					if (!(0 <= file && file < 8 && 0 <= rank && rank < 8)) continue;                            // If outside board, continue
					if (Piece.Color(board.squares[file, rank]) == board.colorToMove) continue;                  // If there is a friendly piece, continue
					if (!IsAttacked((file, rank), board.oppositeColor))                                         // Check is this square is attacked by enemy piece
					{
						bool isCapture = board.squares[file, rank] != Piece.None;
						Move move = new Move((kingFile, kingRank), (file, rank), board.squares[file, rank]);
						if (isCapture)
						{
							movesList.InsertCapture(move);
						}
						else if (!onlyCaptures)
						{
							movesList.Insert(move);
						}
					}
				}
			}
			// Add castle moves if king is not in check
			if (checkers.Count == 0 && !onlyCaptures)
			{
				GetCastleMoves(movesList);
			}
		}

		//TODO Split into two method (GetPawnForwardMoves and GetPawnAttackMoves)
		public void GetPawnMoves((int file, int rank) square, EasierMoveList movesList, bool onlyCaptures)
		{

			bool isPinned = pinned.Contains(square);
			// Get direction to king if pinned. (0,0) if not pinned (which doesn't)
			(int dx, int dy) dirToKing = (0, 0);
			if (isPinned)
			{
				dirToKing = GetDirection(square, board.kingSquares[Piece.ColorIndex(board.colorToMove)]);
			}
			// Forward moves
			// If pinned, can only possily move forward if king is on the same file
			bool canMoveForward = !isPinned || (square.file == board.kingSquares[Piece.ColorIndex(board.colorToMove)].file);
			if (canMoveForward && !onlyCaptures)
			{
				int forward = board.colorToMove == Piece.White ? 1 : -1;
				//TODO Calculate (file, rank) so we dont have to keep calculating it...
				// Cannot move forward if any piece is in front of the pawn
				// Don't need to check if outside board, since can't have pawn on back rank
				bool isBlocked = board.squares[square.file, square.rank + forward] != Piece.None;
				if (!isBlocked)
				{
					// Check block bitboard for forward move
					if (BitBoard.HasOne(blockBitboard, square.file, square.rank + forward))
					{
						if (square.rank + forward == 0 || square.rank + forward == 7)
						{
							GetPromotionMoves(square, (square.file, square.rank + forward), board.squares[square.file, square.rank + forward], movesList, true);
						}
						else
						{
							movesList.Insert(new Move(square, (square.file, square.rank + forward), board.squares[square.file, square.rank + forward]));
						}
					}
					// Double forward move
					bool atStartPosition = (board.colorToMove == Piece.White && square.rank == 1) || (board.colorToMove == Piece.Black && square.rank == 6);
					if (atStartPosition)
					{
						bool doubleBlocked = board.squares[square.file, square.rank + 2 * forward] != Piece.None;
						if (!doubleBlocked && BitBoard.HasOne(blockBitboard, square.file, square.rank + 2 * forward))
						{
							movesList.Insert(new Move(square, (square.file, square.rank + 2 * forward), board.squares[square.file, square.rank + 2 * forward]));
						}
					}
				}
			}
			// Capture moves
			for (int i = 0; i < pawnDirections.GetLength(1); i++)
			{
				var (dx, dy) = pawnDirections[Piece.ColorIndex(board.colorToMove), i];
				(int file, int rank) = (square.file + dx, square.rank + dy);
				// If outside board, continue
				if (!Util.InsideBoard(file, rank)) continue;
				// If pinned, direction must match direction to king
				if (isPinned && !((dirToKing.dx == dx && dirToKing.dy == dy) || (dirToKing.dx == -dx && dirToKing.dy == -dy))) continue;
				// Check blockBitboard and captureBitboard (en Passant capture can block)
				//TODO Make the en passant part more clear. En passant square is not on captureboard since only pawn can capture it. Maybe OR it together with block and capture bitboard?
				if (!BitBoard.HasOne(blockBitboard | captureBitboard, file, rank) && (file, rank) != board.enPassantSquare) continue;

				// Else check the piece at the square
				int piece = board.squares[file, rank];

				// Check for en Passant
				if ((file, rank) == board.enPassantSquare)
				{
					// Check for en Passant discovered check (by playing move and checking wether the king is attacked)
					//TODO Could possibly do it by checking if king is on same rank, and checking for sliding piece ignoring this pawn and the pawn to be captured
					Move ep = new Move(square, (file, rank), piece, isEnPassantCapture: true);
					board.MakeMove(ep);
					if (!IsAttacked(board.kingSquares[Piece.ColorIndex(board.oppositeColor)], board.colorToMove))
					{
						movesList.InsertCapture(ep);
					}
					board.UndoPreviousMove();
				}
				else if (Piece.IsColor(piece, board.colorToMove) || piece == Piece.None)
				{
					// If friendly or empty (non-EP) continue
					continue;
				}
				// Then it must be enemy and a valid capture
				// Check if pawn is moving to back rank, then add promotion moves
				else if (rank == 0 || rank == 7)
				{
					GetPromotionMoves(square, (file, rank), board.squares[file, rank], movesList);
				}
				else
				{
					movesList.InsertCapture(new Move(square, (file, rank), board.squares[file, rank]));
				}
			}
		}

		public void GetPromotionMoves((int, int) from, (int, int) to, int capturedPiece, EasierMoveList movesList, bool includeAllPromotions = true)
		{
			movesList.InsertCapture(new Move(from, to, capturedPiece: capturedPiece, promotionType: Piece.Queen));
			movesList.InsertCapture(new Move(from, to, capturedPiece: capturedPiece, promotionType: Piece.Knight));
			// Bishop and Rook promotions are never better than Queen promotion. Parameter used to exclude them from bot search
			if (includeAllPromotions)
			{
				movesList.InsertCapture(new Move(from, to, capturedPiece: capturedPiece, promotionType: Piece.Bishop));
				movesList.InsertCapture(new Move(from, to, capturedPiece: capturedPiece, promotionType: Piece.Rook));
			}
		}

		public void GetKnightMoves((int file, int rank) square, EasierMoveList movesList, bool onlyCaptures)
		{
			// If this knight is pinned, then it cannot move
			if (pinned.Contains(square)) return;
			// Else add precomputed moves
			foreach ((int file, int rank) in PrecomputedData.knightMoves[square.file, square.rank])
			{
				// Check that (file, rank) isn't a friendly piece
				if (Piece.Color(board.squares[file, rank]) != board.colorToMove)
				{
					// Check block and capture bitboards (all one if checkers.Count == 0)
					if (BitBoard.HasOne(captureBitboard, file, rank) || BitBoard.HasOne(blockBitboard, file, rank))
					{
						bool isCapture = board.squares[file, rank] != Piece.None;
						Move move = new Move(square, (file, rank), board.squares[file, rank]);
						if (isCapture)
						{
							movesList.InsertCapture(move);
						}
						else if (!onlyCaptures)
						{
							movesList.InsertCapture(move);
						}
					}
				}
			}
		}

		public void GetSlidingMoves((int file, int rank) square, int pieceType, EasierMoveList movesList, bool onlyCaptures)
		{
			// Diagonals start at 0 and alternates
			// Orthogonals start at 1 and alternates
			int startIndex = (pieceType == Piece.Rook) ? 1 : 0;
			int inc = (pieceType == Piece.Queen) ? 1 : 2;

			bool isPinned = pinned.Contains(square);

			for (int i = startIndex; i < kingDirections.Length; i += inc)
			{
				(int dx, int dy) = kingDirections[i];

				// If the piece is pinned, it can only move towards or away from it's king
				if (isPinned)
				{
					var dirToKing = GetDirection(square, board.kingSquares[Piece.ColorIndex(board.colorToMove)]);
					bool movingTowardsKing = dirToKing == (dx, dy) || dirToKing == (-dx, -dy);
					if (!movingTowardsKing) continue;
				}

				int toEdge = PrecomputedData.squaresToEdge[square.file][square.rank][i];

				for (int j = 1; j <= toEdge; j++)
				{
					(int file, int rank) = (square.file + j * dx, square.rank + j * dy);
					int piece = board.squares[file, rank];
					// If square is empty, add move to it and continue
					if (piece == Piece.None)
					{
						// Only add it if inside blockBitboard
						if (!onlyCaptures && BitBoard.HasOne(blockBitboard, file, rank))
						{
							movesList.Insert(new Move(square, (file, rank), board.squares[file, rank]));
						}
						continue;
					}
					// If piece is friendly, stop looking in this direction
					if (Piece.IsColor(piece, board.colorToMove)) break;

					// Else piece was opponent. Can therefore be captured
					// Only add it if matches captureBitboard
					if (BitBoard.HasOne(captureBitboard, file, rank))
					{
						movesList.InsertCapture(new Move(square, (file, rank), board.squares[file, rank]));
					}
					// Stop looking in this direction
					break;
				}
			}
		}

		public void GetCastleMoves(EasierMoveList movesList)
		{
			(int file, int rank) kingSquare = board.kingSquares[Piece.ColorIndex(board.colorToMove)];

			if (board.colorToMove == Piece.White)
			{
				// Kingside
				if (board.castlingRights.HasFlag(CastlingRights.WhiteKingside))
				{
					// Check if f1 and g1 are empty and not attacked
					bool canCastle = (board.squares[5, 0] == Piece.None) && !IsAttacked((5, 0), board.oppositeColor) &&
									 (board.squares[6, 0] == Piece.None) && !IsAttacked((6, 0), board.oppositeColor);
					if (canCastle)
					{
						movesList.Insert(new Move(kingSquare, (6, 0), Piece.None, isCastle: true));
					}
				}
				// Queenside
				if (board.castlingRights.HasFlag(CastlingRights.WhiteQueenside))
				{
					// Check if d1 and c1 are empty and not attacked, and that b1 is empty
					bool canCastle = (board.squares[3, 0] == Piece.None) && !IsAttacked((3, 0), board.oppositeColor) &&
									 (board.squares[2, 0] == Piece.None) && !IsAttacked((2, 0), board.oppositeColor) &&
									 (board.squares[1, 0] == Piece.None);
					if (canCastle)
					{
						movesList.Insert(new Move(kingSquare, (2, 0), Piece.None, isCastle: true));
					}
				}
			}
			// Black castle moves
			else
			{
				// Kingside
				if (board.castlingRights.HasFlag(CastlingRights.BlackKingside))
				{
					// Check if f1 and g1 are empty and not attacked
					bool canCastle = (board.squares[5, 7] == Piece.None) && !IsAttacked((5, 7), board.oppositeColor) &&
									 (board.squares[6, 7] == Piece.None) && !IsAttacked((6, 7), board.oppositeColor);
					if (canCastle)
					{
						movesList.Insert(new Move(kingSquare, (6, 7), Piece.None, isCastle: true));
					}
				}
				// Queenside
				if (board.castlingRights.HasFlag(CastlingRights.BlackQueenside))
				{
					// Check if d1 and c1 are empty and not attacked, and that b1 is empty
					bool canCastle = (board.squares[3, 7] == Piece.None) && !IsAttacked((3, 7), board.oppositeColor) &&
									 (board.squares[2, 7] == Piece.None) && !IsAttacked((2, 7), board.oppositeColor) &&
									 (board.squares[1, 7] == Piece.None);
					if (canCastle)
					{
						movesList.Insert(new Move(kingSquare, (2, 7), Piece.None, isCastle: true));
					}
				}
			}
		}

		//TODO Use bitboards to check for pawns, knights and kings. Use directions for queen/bishop/rook (i don't like Magic Numbers TM)
		public bool IsAttacked((int file, int rank) square, int attackingColor)
		{
			// Check for knights. Loop through precomputed knight moves
			foreach ((int file, int rank) in PrecomputedData.knightMoves[square.file, square.rank])
			{
				// Check if enemy knight is present there
				if (board.squares[file, rank] == (attackingColor | Piece.Knight))
				{
					return true;
				}
			}
			// Check for pawns
			for (int i = 0; i < pawnDirections.GetLength(1); i++)
			{
				var (dx, dy) = pawnDirections[Piece.ColorIndex(Piece.OppositeColor(attackingColor)), i];
				(int file, int rank) = (square.file + dx, square.rank + dy);
				// If outside board, continue
				if (!(0 <= file && file < 8 && 0 <= rank && rank < 8)) continue;
				// Check if piece is enemy pawn
				if (board.squares[file, rank] == (attackingColor | Piece.Pawn))
				{
					return true;
				}
			}
			// Check for kings
			foreach (var (dx, dy) in kingDirections)
			{
				(int file, int rank) = (square.file + dx, square.rank + dy);
				// If outside board, continue
				if (!(0 <= file && file < 8 && 0 <= rank && rank < 8)) continue;
				// Check if attacking piece is enemy knight
				if (board.squares[file, rank] == (attackingColor | Piece.King))
				{
					return true;
				}
			}
			// Check for sliding pieces
			for (int i = 0; i < kingDirections.Length; i++)
			{
				(int dx, int dy) = kingDirections[i];
				int toEdge = PrecomputedData.squaresToEdge[square.file][square.rank][i];

				for (int j = 1; j <= toEdge; j++)
				{
					(int file, int rank) = (square.file + j * dx, square.rank + j * dy);
					int piece = board.squares[file, rank];
					// Empty square, continue
					if (piece == Piece.None) continue;
					// If piece isn't attacking, break
					if (!Piece.IsColor(piece, attackingColor)) break;
					// Else the piece is an enemy piece. Check if it can attack in the given direction
					bool isDiagonal = dx * dy != 0;
					int attackerType = Piece.Type(piece);
					// Can attack if piece is a queen, a bishop on a diagonal or a rook on non-diagonal
					if ((attackerType == Piece.Queen) || (isDiagonal && (attackerType == Piece.Bishop)) || (!isDiagonal && (attackerType == Piece.Rook)))
					{
						return true;
					}
					// If not, then not attacked in this direction
					break;
				}
			}
			return false;
		}

		// Calculate the direction between two squares. Returns (0,0) if they are not on a diagonal or orthogonal line
		public static (int dx, int dy) GetDirection((int file, int rank) start, (int file, int rank) end)
		{
			if (start.file == end.file)
			{
				return (0, end.rank > start.rank ? 1 : -1);
			}
			if (start.rank == end.rank)
			{
				return (end.file > start.file ? 1 : -1, 0);
			}
			int dFile = end.file - start.file;
			int dRank = end.rank - start.rank;
			if (dFile > 0 & dRank > 0) return (1, 1);
			if (dFile < 0 & dRank > 0) return (-1, 1);
			if (dFile > 0 & dRank < 0) return (1, -1);
			if (dFile < 0 & dRank < 0) return (-1, -1);

			return (0, 0);
		}
		public List<Move> GenerateMovesNoPrio(bool onlyCaptures = false)
		{
			// Check the king positions for checks and pins
			CheckKingPosition();
			justaList movesList = new justaList();
			GetKingMovesNoPrio(movesList, onlyCaptures);
			// If the number of checks is > 1, then only the king can move
			if (checkers.Count > 1) { movesList.ConnectList(); return movesList.GetList(); }

			// Else, loop through all friendly pieces to generate their moves
			//TODO Use piecelists?
			for (int file = 0; file < 8; file++)
			{
				for (int rank = 0; rank < 8; rank++)
				{
					int piece = board.squares[file, rank];
					// If piece isn't friendly (excluding king as moves for king have already been generated) continue
					if ((Piece.Color(piece) != board.colorToMove) || (Piece.Type(piece) == Piece.King)) continue;
					// Check type of piece and generate appropriate moves
					int pieceType = Piece.Type(piece);

					switch (pieceType)
					{
						case Piece.Pawn:
							GetPawnMovesNoPrio((file, rank), movesList, onlyCaptures);
							break;
						case Piece.Knight:
							GetKnightMovesNoPrio((file, rank), movesList, onlyCaptures);
							break;
						case Piece.Bishop:
							GetSlidingMovesNoPrio((file, rank), pieceType, movesList, onlyCaptures);
							break;
						case Piece.Rook:
							GetSlidingMovesNoPrio((file, rank), pieceType, movesList, onlyCaptures);
							break;
						case Piece.Queen:
							GetSlidingMovesNoPrio((file, rank), pieceType, movesList, onlyCaptures);
							break;
						default:
							break;
					}
				}
			}
			movesList.ConnectList();
			return movesList.GetList();
		}
				public void GetKingMovesNoPrio(justaList movesList, bool onlyCaptures)
		{
			(int kingFile, int kingRank) = board.kingSquares[Piece.ColorIndex(board.colorToMove)];

			for (int i = 0; i < kingDirections.Length; i++)
			{
				// Ignore squares we have already determined are attacked (because king is in check)
				if (!attackedAroundKing[i])
				{
					(int file, int rank) = (kingFile + kingDirections[i].dx, kingRank + kingDirections[i].dy);
					if (!(0 <= file && file < 8 && 0 <= rank && rank < 8)) continue;                            // If outside board, continue
					if (Piece.Color(board.squares[file, rank]) == board.colorToMove) continue;                  // If there is a friendly piece, continue
					if (!IsAttacked((file, rank), board.oppositeColor))                                         // Check is this square is attacked by enemy piece
					{
						bool isCapture = board.squares[file, rank] != Piece.None;
						Move move = new Move((kingFile, kingRank), (file, rank), board.squares[file, rank]);
						if (isCapture)
						{
							movesList.InsertCapture(move);
						}
						else if (!onlyCaptures)
						{
							movesList.Insert(move);
						}
					}
				}
			}
			// Add castle moves if king is not in check
			if (checkers.Count == 0 && !onlyCaptures)
			{
				GetCastleMovesNoPrio(movesList);
			}
		}

		//TODO Split into two method (GetPawnForwardMoves and GetPawnAttackMoves)
		public void GetPawnMovesNoPrio((int file, int rank) square, justaList movesList, bool onlyCaptures)
		{

			bool isPinned = pinned.Contains(square);
			// Get direction to king if pinned. (0,0) if not pinned (which doesn't)
			(int dx, int dy) dirToKing = (0, 0);
			if (isPinned)
			{
				dirToKing = GetDirection(square, board.kingSquares[Piece.ColorIndex(board.colorToMove)]);
			}
			// Forward moves
			// If pinned, can only possily move forward if king is on the same file
			bool canMoveForward = !isPinned || (square.file == board.kingSquares[Piece.ColorIndex(board.colorToMove)].file);
			if (canMoveForward && !onlyCaptures)
			{
				int forward = board.colorToMove == Piece.White ? 1 : -1;
				//TODO Calculate (file, rank) so we dont have to keep calculating it...
				// Cannot move forward if any piece is in front of the pawn
				// Don't need to check if outside board, since can't have pawn on back rank
				bool isBlocked = board.squares[square.file, square.rank + forward] != Piece.None;
				if (!isBlocked)
				{
					// Check block bitboard for forward move
					if (BitBoard.HasOne(blockBitboard, square.file, square.rank + forward))
					{
						if (square.rank + forward == 0 || square.rank + forward == 7)
						{
							GetPromotionMovesNoPrio(square, (square.file, square.rank + forward), board.squares[square.file, square.rank + forward], movesList, true);
						}
						else
						{
							movesList.Insert(new Move(square, (square.file, square.rank + forward), board.squares[square.file, square.rank + forward]));
						}
					}
					// Double forward move
					bool atStartPosition = (board.colorToMove == Piece.White && square.rank == 1) || (board.colorToMove == Piece.Black && square.rank == 6);
					if (atStartPosition)
					{
						bool doubleBlocked = board.squares[square.file, square.rank + 2 * forward] != Piece.None;
						if (!doubleBlocked && BitBoard.HasOne(blockBitboard, square.file, square.rank + 2 * forward))
						{
							movesList.Insert(new Move(square, (square.file, square.rank + 2 * forward), board.squares[square.file, square.rank + 2 * forward]));
						}
					}
				}
			}
			// Capture moves
			for (int i = 0; i < pawnDirections.GetLength(1); i++)
			{
				var (dx, dy) = pawnDirections[Piece.ColorIndex(board.colorToMove), i];
				(int file, int rank) = (square.file + dx, square.rank + dy);
				// If outside board, continue
				if (!Util.InsideBoard(file, rank)) continue;
				// If pinned, direction must match direction to king
				if (isPinned && !((dirToKing.dx == dx && dirToKing.dy == dy) || (dirToKing.dx == -dx && dirToKing.dy == -dy))) continue;
				// Check blockBitboard and captureBitboard (en Passant capture can block)
				//TODO Make the en passant part more clear. En passant square is not on captureboard since only pawn can capture it. Maybe OR it together with block and capture bitboard?
				if (!BitBoard.HasOne(blockBitboard | captureBitboard, file, rank) && (file, rank) != board.enPassantSquare) continue;

				// Else check the piece at the square
				int piece = board.squares[file, rank];

				// Check for en Passant
				if ((file, rank) == board.enPassantSquare)
				{
					// Check for en Passant discovered check (by playing move and checking wether the king is attacked)
					//TODO Could possibly do it by checking if king is on same rank, and checking for sliding piece ignoring this pawn and the pawn to be captured
					Move ep = new Move(square, (file, rank), piece, isEnPassantCapture: true);
					board.MakeMove(ep);
					if (!IsAttacked(board.kingSquares[Piece.ColorIndex(board.oppositeColor)], board.colorToMove))
					{
						movesList.InsertCapture(ep);
					}
					board.UndoPreviousMove();
				}
				else if (Piece.IsColor(piece, board.colorToMove) || piece == Piece.None)
				{
					// If friendly or empty (non-EP) continue
					continue;
				}
				// Then it must be enemy and a valid capture
				// Check if pawn is moving to back rank, then add promotion moves
				else if (rank == 0 || rank == 7)
				{
					GetPromotionMovesNoPrio(square, (file, rank), board.squares[file, rank], movesList);
				}
				else
				{
					movesList.InsertCapture(new Move(square, (file, rank), board.squares[file, rank]));
				}
			}
		}

		public void GetPromotionMovesNoPrio((int, int) from, (int, int) to, int capturedPiece, justaList movesList, bool includeAllPromotions = true)
		{
			movesList.InsertCapture(new Move(from, to, capturedPiece: capturedPiece, promotionType: Piece.Queen));
			movesList.InsertCapture(new Move(from, to, capturedPiece: capturedPiece, promotionType: Piece.Knight));
			// Bishop and Rook promotions are never better than Queen promotion. Parameter used to exclude them from bot search
			if (includeAllPromotions)
			{
				movesList.InsertCapture(new Move(from, to, capturedPiece: capturedPiece, promotionType: Piece.Bishop));
				movesList.InsertCapture(new Move(from, to, capturedPiece: capturedPiece, promotionType: Piece.Rook));
			}
		}

		public void GetKnightMovesNoPrio((int file, int rank) square, justaList movesList, bool onlyCaptures)
		{
			// If this knight is pinned, then it cannot move
			if (pinned.Contains(square)) return;
			// Else add precomputed moves
			foreach ((int file, int rank) in PrecomputedData.knightMoves[square.file, square.rank])
			{
				// Check that (file, rank) isn't a friendly piece
				if (Piece.Color(board.squares[file, rank]) != board.colorToMove)
				{
					// Check block and capture bitboards (all one if checkers.Count == 0)
					if (BitBoard.HasOne(captureBitboard, file, rank) || BitBoard.HasOne(blockBitboard, file, rank))
					{
						bool isCapture = board.squares[file, rank] != Piece.None;
						Move move = new Move(square, (file, rank), board.squares[file, rank]);
						if (isCapture)
						{
							movesList.InsertCapture(move);
						}
						else if (!onlyCaptures)
						{
							movesList.InsertCapture(move);
						}
					}
				}
			}
		}

		public void GetSlidingMovesNoPrio((int file, int rank) square, int pieceType, justaList movesList, bool onlyCaptures)
		{
			// Diagonals start at 0 and alternates
			// Orthogonals start at 1 and alternates
			int startIndex = (pieceType == Piece.Rook) ? 1 : 0;
			int inc = (pieceType == Piece.Queen) ? 1 : 2;

			bool isPinned = pinned.Contains(square);

			for (int i = startIndex; i < kingDirections.Length; i += inc)
			{
				(int dx, int dy) = kingDirections[i];

				// If the piece is pinned, it can only move towards or away from it's king
				if (isPinned)
				{
					var dirToKing = GetDirection(square, board.kingSquares[Piece.ColorIndex(board.colorToMove)]);
					bool movingTowardsKing = dirToKing == (dx, dy) || dirToKing == (-dx, -dy);
					if (!movingTowardsKing) continue;
				}

				int toEdge = PrecomputedData.squaresToEdge[square.file][square.rank][i];

				for (int j = 1; j <= toEdge; j++)
				{
					(int file, int rank) = (square.file + j * dx, square.rank + j * dy);
					int piece = board.squares[file, rank];
					// If square is empty, add move to it and continue
					if (piece == Piece.None)
					{
						// Only add it if inside blockBitboard
						if (!onlyCaptures && BitBoard.HasOne(blockBitboard, file, rank))
						{
							movesList.Insert(new Move(square, (file, rank), board.squares[file, rank]));
						}
						continue;
					}
					// If piece is friendly, stop looking in this direction
					if (Piece.IsColor(piece, board.colorToMove)) break;

					// Else piece was opponent. Can therefore be captured
					// Only add it if matches captureBitboard
					if (BitBoard.HasOne(captureBitboard, file, rank))
					{
						movesList.InsertCapture(new Move(square, (file, rank), board.squares[file, rank]));
					}
					// Stop looking in this direction
					break;
				}
			}
		}
		public void GetCastleMovesNoPrio(justaList movesList)
		{
			(int file, int rank) kingSquare = board.kingSquares[Piece.ColorIndex(board.colorToMove)];

			if (board.colorToMove == Piece.White)
			{
				// Kingside
				if (board.castlingRights.HasFlag(CastlingRights.WhiteKingside))
				{
					// Check if f1 and g1 are empty and not attacked
					bool canCastle = (board.squares[5, 0] == Piece.None) && !IsAttacked((5, 0), board.oppositeColor) &&
									 (board.squares[6, 0] == Piece.None) && !IsAttacked((6, 0), board.oppositeColor);
					if (canCastle)
					{
						movesList.Insert(new Move(kingSquare, (6, 0), Piece.None, isCastle: true));
					}
				}
				// Queenside
				if (board.castlingRights.HasFlag(CastlingRights.WhiteQueenside))
				{
					// Check if d1 and c1 are empty and not attacked, and that b1 is empty
					bool canCastle = (board.squares[3, 0] == Piece.None) && !IsAttacked((3, 0), board.oppositeColor) &&
									 (board.squares[2, 0] == Piece.None) && !IsAttacked((2, 0), board.oppositeColor) &&
									 (board.squares[1, 0] == Piece.None);
					if (canCastle)
					{
						movesList.Insert(new Move(kingSquare, (2, 0), Piece.None, isCastle: true));
					}
				}
			}
			// Black castle moves
			else
			{
				// Kingside
				if (board.castlingRights.HasFlag(CastlingRights.BlackKingside))
				{
					// Check if f1 and g1 are empty and not attacked
					bool canCastle = (board.squares[5, 7] == Piece.None) && !IsAttacked((5, 7), board.oppositeColor) &&
									 (board.squares[6, 7] == Piece.None) && !IsAttacked((6, 7), board.oppositeColor);
					if (canCastle)
					{
						movesList.Insert(new Move(kingSquare, (6, 7), Piece.None, isCastle: true));
					}
				}
				// Queenside
				if (board.castlingRights.HasFlag(CastlingRights.BlackQueenside))
				{
					// Check if d1 and c1 are empty and not attacked, and that b1 is empty
					bool canCastle = (board.squares[3, 7] == Piece.None) && !IsAttacked((3, 7), board.oppositeColor) &&
									 (board.squares[2, 7] == Piece.None) && !IsAttacked((2, 7), board.oppositeColor) &&
									 (board.squares[1, 7] == Piece.None);
					if (canCastle)
					{
						movesList.Insert(new Move(kingSquare, (2, 7), Piece.None, isCastle: true));
					}
				}
			}
		}
	}
}