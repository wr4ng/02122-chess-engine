using System;
using System.Collections.Generic;
using System.IO;

namespace Chess
{
	public static class NewPiece
	{
		public const int None = 0;
		public const int Pawn = 1;
		public const int Bishop = 2;
		public const int Knight = 3;
		public const int Rook = 4;
		public const int Queen = 5;
		public const int King = 6;

		public const int White = 8;
		public const int Black = 16;

		// Get least significant three bits
		public static int Type(int piece) => piece & 0b00111;

		// Get most significant two bits
		public static int Color(int piece) => piece & 0b11000;

		// XY--- & 0b11000 = XY000. Check if that is equal to color
		public static bool IsColor(int piece, int color) => Color(piece) == color;
	}

	public struct NewMove
	{
		public (int file, int rank) from, to;

		public NewMove((int file, int rank) from, (int file, int rank) to)
		{
			this.from = from;
			this.to = to;
		}

		public override String ToString() => $"Move: {from} -> {to}";
	}

	public class NewBoard
	{
		public int[,] squares;
		public int colorToMove;
		public int oppositeColor => colorToMove == NewPiece.White ? NewPiece.Black : NewPiece.White;

		public (int file, int rank)[] kingSquares; // 0 = White, 1 = King

		public NewMoveGenerator moveGenerator;

		public static int ColorIndex(int color) => (color == NewPiece.White) ? 0 : 1;

		public NewBoard()
		{
			squares = new int[8, 8];
			colorToMove = NewPiece.White;
			kingSquares = new (int file, int rank)[2];
			moveGenerator = new NewMoveGenerator(this);
		}

		public static NewBoard FromFEN(string fen)
		{
			NewBoard board = new NewBoard();

			//TODO Handle the remaining FEN pieces
			string fenBoard = fen.Split(' ')[0];
			int file = 0, rank = 7;

			foreach (char c in fenBoard)
			{
				if (char.IsDigit(c))
				{
					file += (int)char.GetNumericValue(c);
				}
				else if (c == '/')
				{
					//TODO Handle invalid FEN
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
						'k' => NewPiece.King
						//TODO Handle invalid chars
					};
					// Save king positions
					if (type == NewPiece.King)
					{
						board.kingSquares[ColorIndex(color)] = (file, rank);
					}

					board.squares[file, rank] = color | type;
					file++;
				}
			}
			//TODO Verify the number of kings on each side (1 of each exactly)
			return board;
		}

		public override string ToString()
		{
			string board = "";
			for (int rank = 7; rank >= 0; rank--)
			{
				for (int file = 0; file < 8; file++)
				{
					char type = (squares[file, rank] & 0b111) switch
					{
						NewPiece.None => '*',
						NewPiece.Pawn => 'p',
						NewPiece.Bishop => 'b',
						NewPiece.Knight => 'n',
						NewPiece.Rook => 'r',
						NewPiece.Queen => 'q',
						NewPiece.King => 'k',
					};
					char piece = (squares[file, rank] & 0b11000) == NewPiece.White ? char.ToUpper(type) : type;
					board += piece;
				}
				board += "\n";
			}
			return board;
		}
	}

	public class NewMoveGenerator
	{
		NewBoard board;

		public List<(int file, int rank)> pinned;
		public List<(int file, int rank)> checkers;
		public bool[] attackedAroundKing;

		// Constant arrays of directiojns used for looping
		readonly static (int dx, int dy)[] kingDirections = new (int dx, int dy)[8] { (-1, -1), (0, -1), (1, -1), (1, 0), (1, 1), (0, 1), (-1, 1), (-1, 0) };
		readonly static (int dx, int dy)[] knightDirections = new (int dx, int dy)[8] { (-1, -2), (1, -2), (-2, -1), (2, -1), (-2, 1), (2, 1), (-1, 2), (1, 2) };
		//TODO Maybe make it (int, int)[][] instead of [,]
		readonly static (int dx, int dy)[,] pawnDirections = new (int dx, int dy)[2, 2] { { (-1, 1), (1, 1) }, { (-1, -1), (1, -1) } };

		public NewMoveGenerator(NewBoard board)
		{
			this.board = board;
			// Precompute move data
			PrecomputeData();
		}

		// Generate legal moves for the current position of board
		public List<NewMove> GenerateMoves()
		{
			// Check the king positions for checks and pins
			CheckKingPosition();
			List<NewMove> moves = GetKingMoves();

			// If the number of checks is > 1, then only the king can move
			if (checkers.Count > 1) return moves;

			// Else, loop through all friendly pieces to generate their moves
			//TODO Use piecelists?
			for (int file = 0; file < 8; file++)
			{
				for (int rank = 0; rank < 8; rank++)
				{
					int piece = board.squares[file, rank];
					// If piece isn't friendly (excluding king as moves for king have already been generated) continue
					if ((NewPiece.Color(piece) != board.colorToMove) || (NewPiece.Type(piece) == NewPiece.King)) continue;
					// Check type of piece and generate appropriate moves
					int pieceType = NewPiece.Type(piece);

					var pieceMoves = pieceType switch
					{
						NewPiece.Pawn => GetPawnMoves((file, rank)),
						NewPiece.Knight => GetKnightMoves((file, rank)),
						NewPiece.Bishop => GetSlidingMoves((file, rank), pieceType),
						NewPiece.Rook => GetSlidingMoves((file, rank), pieceType),
						NewPiece.Queen => GetSlidingMoves((file, rank), pieceType),
					};

					moves.AddRange(pieceMoves);
				}
			}
			return moves;
		}

		// TODO Keep track of blocking bitboard (where pieces can move to block a checker) and capture bitboard (checker location)
		public void CheckKingPosition()
		{
			// Check attack data around king
			(int kingFile, int kingRank) = board.kingSquares[NewBoard.ColorIndex(board.colorToMove)];

			// Which spaces we for sure know are attacked around the king after checking in each direction (to avoid re-calculating the squares in move generation)
			attackedAroundKing = new bool[8] { false, false, false, false, false, false, false, false };

			// Initialize list of pinned pieces and checking pieces
			pinned = new();
			checkers = new();

			for (int dirIndex = 0; dirIndex < kingDirections.Length; dirIndex++)
			{
				var (dx, dy) = kingDirections[dirIndex];
				bool foundFriendlyPiece = false;
				(int file, int rank) friendlySquare = (-1, -1);

				//TODO Instead of looping until the end of the board, we can precalculate the number of squares in each direction for each square (8 directions * 64 squares = 512 ints)
				int i = 1;
				while (true)
				{
					// Calculate current square in direction (dx, dy)
					(int file, int rank) = (kingFile + dx * i, kingRank + dy * i);
					// Only continue while inside the board
					if (!(0 <= file && file < 8 && 0 <= rank && rank < 8)) break;

					int piece = board.squares[file, rank];
					// Continue if square is empty
					if (piece == NewPiece.None)
					{
						i++;
						continue;
					}

					// Check if piece is friendly
					if (NewPiece.IsColor(piece, board.colorToMove))
					{
						// If we already found a friendly piece, two friendly pieces found in this direction and there is no pin
						if (foundFriendlyPiece)
						{
							break;
						}
						else
						{
							// Save friendly piece as it may be pinned
							foundFriendlyPiece = true;
							friendlySquare = (file, rank);
							i++;
						}
					}
					else
					{
						// Enemy piece. Check if it can attack in this direction
						bool isDiagonal = dx * dy != 0;
						int attackerType = NewPiece.Type(piece);

						// The piece can attack the king if it is a queen, or a bishop/rook with their respetive directions
						bool canAttack = (attackerType == NewPiece.Queen) || (isDiagonal && (attackerType == NewPiece.Bishop)) || (!isDiagonal && (attackerType == NewPiece.Rook));
						if (canAttack)
						{
							// If we found a friendly piece before, it is pinned
							if (foundFriendlyPiece)
							{
								pinned.Add(friendlySquare);
							}
							else
							{
								// King is checked by piece in the current direction. If the piece is more than 1 square away, it cannot move in that direction
								// If it is 1 square away, the king may be able to capture it, and we then don't want to skip checking it later
								attackedAroundKing[dirIndex] = i > 1;
								// Cannot move in the opposite direction, as sliding piece would still check it
								attackedAroundKing[(dirIndex + 4) % 8] = true;
								checkers.Add((file, rank));
							}
						}
						break;
					}
				}
			}
			// Old Check for knights
			// foreach (var (dx, dy) in knightDirections)
			// {
			// 	(int file, int rank) = (kingFile + dx, kingRank + dy);
			// 	// If outside board, continue
			// 	if (!(0 <= file && file < 8 && 0 <= rank && rank < 8)) continue;
			// 	// Check if attacking piece is enemy knight
			// 	if (board.squares[file, rank] == (board.oppositeColor | NewPiece.Knight))
			// 	{
			// 		checkers.Add((file, rank));
			// 	}
			// }

			// Check for knights. Loop through precomputed knight moves
			foreach ((int file, int rank) in knightMoves[kingFile, kingRank])
			{
				// Check if enemy knight is present there
				if (board.squares[file, rank] == (board.oppositeColor | NewPiece.Knight))
				{
					checkers.Add((file, rank));
				}
			}
			// Check for pawns
			for (int i = 0; i < pawnDirections.GetLength(1); i++)
			{
				var (dx, dy) = pawnDirections[NewBoard.ColorIndex(board.colorToMove), i];
				(int file, int rank) = (kingFile + dx, kingRank + dy);
				// If outside board, continue
				if (!(0 <= file && file < 8 && 0 <= rank && rank < 8)) continue;
				// Check if piece is enemy pawn
				if (board.squares[file, rank] == (board.oppositeColor | NewPiece.Pawn))
				{
					checkers.Add((file, rank));
				}
			}
		}

		public List<NewMove> GetKingMoves()
		{
			List<NewMove> kingMoves = new();

			for (int i = 0; i < kingDirections.Length; i++)
			{
				// Ignore squacres we have already determined are attacked (because king is in check)
				if (!attackedAroundKing[i])
				{
					(int kingFile, int kingRank) = board.kingSquares[NewBoard.ColorIndex(board.colorToMove)];
					(int file, int rank) = (kingFile + kingDirections[i].dx, kingRank + kingDirections[i].dy);
					// If outside board, continue
					if (!(0 <= file && file < 8 && 0 <= rank && rank < 8)) continue;
					// If there is a friendly piece, continue
					if (NewPiece.Color(board.squares[file, rank]) == board.colorToMove) continue;
					// Check is this square is attacked by enemy piece
					if (!IsAttacked((file, rank), board.oppositeColor))
					{
						kingMoves.Add(new NewMove((kingFile, kingRank), (file, rank)));
					}
				}
			}
			return kingMoves;
		}

		public List<NewMove> GetPawnMoves((int file, int rank) square)
		{
			List<NewMove> moves = new();

			bool isPinned = pinned.Contains(square);
			(int dx, int dy) dirToKing = (-1, -1);
			if (isPinned)
			{
				dirToKing = GetDirection(square, board.kingSquares[NewBoard.ColorIndex(board.colorToMove)]);
			}
			// Forward moves
			// If pinned, can only possily move forward if king is on the same file
			bool canMoveForward = !isPinned || (square.file == board.kingSquares[NewBoard.ColorIndex(board.colorToMove)].file);
			if (canMoveForward)
			{
				int forward = board.colorToMove == NewPiece.White ? 1 : -1;
				// Cannot move forward if any piece is in front of the pawn
				// Don't need to check if outside board, since can't have pawn on back rank
				bool isBlocked = board.squares[square.file, square.rank + forward] != NewPiece.None;
				if (!isBlocked)
				{
					if (square.rank + forward == 0 || square.rank + forward == 7)
					{
						// TODO Handle promotion
					}
					else
					{
						moves.Add(new NewMove(square, (square.file, square.rank + forward)));
					}
					// Double forward move
					bool atStartPosition = (board.colorToMove == NewPiece.White && square.rank == 1) || (board.colorToMove == NewPiece.Black && square.rank == 6);
					if (atStartPosition)
					{
						bool doubleBlocked = board.squares[square.file, square.rank + 2 * forward] != NewPiece.None;
						if (!doubleBlocked)
						{
							moves.Add(new NewMove(square, (square.file, square.rank + 2 * forward)));
						}
					}
				}
			}
			// Capture moves
			for (int i = 0; i < pawnDirections.GetLength(1); i++)
			{
				var (dx, dy) = pawnDirections[NewBoard.ColorIndex(board.colorToMove), i];
				(int file, int rank) = (square.file + dx, square.rank + dy);
				// If outside board, continue
				if (!InsideBoard(file, rank)) continue;
				// If pinned, direction must match direction to king
				if (isPinned && !((dirToKing.dx == dx && dirToKing.dy == dy) || (dirToKing.dx == -dx && dirToKing.dy == -dy))) continue;

				int piece = board.squares[file, rank];
				// TODO Check for en Passant (and for en Passant discovered check!)
				// If friendly or empty continue
				if (NewPiece.IsColor(piece, board.colorToMove) || piece == NewPiece.None) continue;

				// Then it must be enemy and a valid capture
				// TODO Check for promotion capture
				moves.Add(new NewMove(square, (file, rank)));
			}
			return moves;
		}

		public List<NewMove> GetKnightMoves((int file, int rank) square)
		{
			List<NewMove> moves = new();
			// If this knight is pinned, then it cannot move
			if (pinned.Contains(square)) return moves;
			// Else add precomputed moves
			foreach ((int file, int rank) in knightMoves[square.file, square.rank])
			{
				// Check that (file, rank) isn't a friendly piece
				if (NewPiece.Color(board.squares[file, rank]) != board.colorToMove)
				{
					// To check capture check NewPiece.Color(board.squares[file, rank]) == board.oppositeColor
					// TODO If king is checked by 1 piece, only add move if it captures that piece or is blocking it
					moves.Add(new NewMove(square, (file, rank)));
				}
			}
			return moves;
		}

		public List<NewMove> GetSlidingMoves((int file, int rank) square, int pieceType)
		{
			List<NewMove> moves = new();
			// Diagonals start at 0 and alternates
			// Orthogonals start at 1 and alternates
			int startIndex = (pieceType == NewPiece.Rook) ? 1 : 0;
			int inc = (pieceType == NewPiece.Queen) ? 1 : 2;

			bool isPinned = pinned.Contains(square);

			for (int i = startIndex; i < kingDirections.Length; i += inc)
			{
				(int dx, int dy) = kingDirections[i];

				// If the piece is pinned, it can only move towards or away from it's king
				if (isPinned)
				{
					var dirToKing = GetDirection(square, board.kingSquares[NewBoard.ColorIndex(board.colorToMove)]);
					bool movingTowardsKing = dirToKing == (dx, dy) || dirToKing == (-dx, -dy);
					if (!movingTowardsKing) continue;
				}

				int toEdge = squaresToEdge[square.file][square.rank][i];

				for (int j = 1; j <= toEdge; j++)
				{
					(int file, int rank) = (square.file + j * dx, square.rank + j * dy);
					int piece = board.squares[file, rank];
					// If square is empty, add move to it and continue
					if (piece == NewPiece.None)
					{
						moves.Add(new NewMove(square, (file, rank)));
						continue;
					}
					// If piece is friendly, stop looking in this direction
					if (NewPiece.IsColor(piece, board.colorToMove)) break;

					// Else piece was opponent. Can therefore be captured
					moves.Add(new NewMove(square, (file, rank)));
					// Stop looking in this direction
					break;
				}
			}
			return moves;
		}

		//TODO Use bitboards to check for pawns, knights and kings. Use directions for queen/bishop/rook (i don't like Magic Numbers TM)
		public bool IsAttacked((int file, int rank) square, int attackingColor)
		{
			// Old check for knights
			// foreach (var (dx, dy) in knightDirections)
			// {
			// 	(int file, int rank) = (square.file + dx, square.rank + dy);
			// 	// If outside board, continue
			// 	if (!(0 <= file && file < 8 && 0 <= rank && rank < 8)) continue;
			// 	// Check if attacking piece is enemy knight
			// 	if (board.squares[file, rank] == (attackingColor | NewPiece.Knight))
			// 	{
			// 		return true;
			// 	}
			// }
			// Check for knights
			// Loop through precomputed knight moves
			foreach ((int file, int rank) in knightMoves[square.file, square.rank])
			{
				// Check if enemy knight is present there
				if (board.squares[file, rank] == (attackingColor | NewPiece.Knight))
				{
					return true;
				}
			}
			// Check for pawns
			for (int i = 0; i < pawnDirections.GetLength(1); i++)
			{
				var (dx, dy) = pawnDirections[NewBoard.ColorIndex(board.colorToMove), i];
				(int file, int rank) = (square.file + dx, square.rank + dy);
				// If outside board, continue
				if (!(0 <= file && file < 8 && 0 <= rank && rank < 8)) continue;
				// Check if piece is enemy pawn
				if (board.squares[file, rank] == (attackingColor | NewPiece.Pawn))
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
				if (board.squares[file, rank] == (attackingColor | NewPiece.King))
				{
					return true;
				}
			}
			// Check for sliding pieces
			for (int i = 0; i < kingDirections.Length; i++)
			{
				(int dx, int dy) = kingDirections[i];
				int toEdge = squaresToEdge[square.file][square.rank][i];

				for (int j = 1; j <= toEdge; j++)
				{
					(int file, int rank) = (square.file + j * dx, square.rank + j * dy);
					int piece = board.squares[file, rank];
					// Empty square, continue
					if (piece == NewPiece.None) continue;
					// If piece isn't attacking, break
					if (!NewPiece.IsColor(piece, attackingColor)) break;
					// Else the piece is an enemy piece. Check if it can attack in the given direction
					bool isDiagonal = dx * dy != 0;
					int attackerType = NewPiece.Type(piece);
					// Can attack if piece is a queen, a bishop on a diagonal or a rook on non-diagonal
					if ((attackerType == NewPiece.Queen) || (isDiagonal && (attackerType == NewPiece.Bishop)) || (!isDiagonal && (attackerType == NewPiece.Rook)))
					{
						return true;
					}
					// If not, then not attacked in this direction
					break;
				}
			}
			return false;
		}

		// Pre-calculated values
		// file, rank, dir (SW, S, SE, E, NE, N, NW, W)
		public static int[][][] squaresToEdge; //TODO Maybe make readonly and move to PrecomputedData class

		// [file, rank][moveNo]
		public static (int file, int rank)[,][] knightMoves;

		static void PrecomputeData()
		{
			ComputeSquaresToEdge();
			ComputeKnightMoves();
			//TODO Precompute pawn and king moves?
		}

		static void ComputeSquaresToEdge()
		{
			squaresToEdge = new int[8][][];

			for (int file = 0; file < 8; file++)
			{
				squaresToEdge[file] = new int[8][];

				for (int rank = 0; rank < 8; rank++)
				{
					squaresToEdge[file][rank] = new int[8];

					int south = rank;
					int west = file;
					int north = 7 - south;
					int east = 7 - west;

					// SW, S, SE, E, NE, N, NW, W
					squaresToEdge[file][rank][0] = Math.Min(south, west);
					squaresToEdge[file][rank][1] = south;
					squaresToEdge[file][rank][2] = Math.Min(south, east);
					squaresToEdge[file][rank][3] = east;
					squaresToEdge[file][rank][4] = Math.Min(north, east);
					squaresToEdge[file][rank][5] = north;
					squaresToEdge[file][rank][6] = Math.Min(north, west);
					squaresToEdge[file][rank][7] = west;
				}
			}
		}

		static void ComputeKnightMoves()
		{
			knightMoves = new (int, int)[8, 8][];

			for (int file = 0; file < 8; file++)
			{
				for (int rank = 0; rank < 8; rank++)
				{
					List<(int, int)> moves = new();
					foreach ((int dx, int dy) in knightDirections)
					{
						(int file, int rank) square = (file + dx, rank + dy);
						if (InsideBoard(square.file, square.rank))
						{
							moves.Add(square);
						}
					}
					knightMoves[file, rank] = moves.ToArray();
				}
			}
		}

		public static bool InsideBoard(int file, int rank) => 0 <= file && file < 8 && 0 <= rank && rank < 8;

		// Calculate the direction between two squares. Returns (0,0) if they are not on a diagonal or orthogonal line
		public static (int dx, int dy) GetDirection((int file, int rank) start, (int file, int rank) end)
		{
			if (start.file == end.rank)
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
			// Shouldn't happen!
			return (0, 0);
		}
	}
}