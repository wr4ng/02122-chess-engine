using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess
{
	public class PGN
	{
		/// <summary>
		/// Import a PGN string and return the moves as an array of strings
		/// </summary>
		/// <param name="pgn"></param>
		public static List<string> GetMoves(String pgn)
		{
			// find the where 1. starts but is not inside a []
			bool insideBrackets = false;
			List<string> movesList = new List<string>();
			string moves = null;
			for (int i = 0; i < pgn.Length - 1; i++)
			{
				if (pgn[i] == '[')
				{
					insideBrackets = true;
				}
				else if (pgn[i] == ']')
				{
					insideBrackets = false;
				}
				else if (!insideBrackets && pgn[i] == '1' && pgn[i + 1] == '.')
				{
					// make a substring from the start of the moves to the end of the string
					moves = pgn.Substring(i);
					// remove all comments from the string
					for (int j = 0; j < moves.Length; j++)
					{
						if (moves[j] == '{')
						{
							int end = moves.IndexOf('}', j);
							moves = moves.Remove(j, end - j + 1);
						}
					}
					break;
				}
			}

			// replaces all the \n and \r with a space
			moves = moves.Replace("\n", " ");
			moves = moves.Replace("\r", " ");


			// go through the string and remove all comments which are inside {}
			for (int i = 0; i < moves.Length; i++)
			{
				if (moves[i] == '{')
				{
					int end = moves.IndexOf('}', i);
					moves = moves.Remove(i, end - i + 1);
				}
			}

			// split the string into an array of strings using the space character as a delimiter
			if (moves != null)
			{
				movesList = new List<string>(moves.Split(' '));
			}
			else
			{
				// if no moves were found, return an empty list
				return new List<string>();
			}

			// remove the string 1/2-1/2 or 0-1 or 1-0
			movesList.RemoveAll(item => item == "1/2-1/2" || item == "0-1" || item == "1-0");

			// remove spaces and \n inside the strings in the list
			for (int i = 0; i < movesList.Count; i++)
			{
				// if there is a . in the string, split the string into two strings
				// and remove the first string
				if (movesList[i].Contains("."))
				{
					movesList[i] = movesList[i].Split(".")[1];
				}
				movesList[i] = movesList[i].Replace(" ", "");
				movesList[i] = movesList[i].Replace("\n", "");
			}

			// remove any empty strings from the list
			movesList.RemoveAll(item => item == "" || item == " " || item == "\n");

			return movesList;
		}

		/// <summary>
		/// Convert a move from algebraic notation to a move object
		/// </summary>
		/// <param name="move"></param>
		/// <param name="board"></param>
		public static Move FromAlgebraicNotationToMove(string move, Board board)
		{
			// generate the list of legal moves
			List<Move> legalMoves = board.moveGenerator.GenerateMoves();
			// check first if it is a castle move
			// castling

			// fjern + og x fra trekket og # (skak, capture, skakmat)
			move = move.Replace("+", "");
			move = move.Replace("x", "");
			move = move.Replace("#", "");
			move = move.Replace(" ", "");

			if (move == "O-O")
			{
				return legalMoves.Find(m => m.isCastle && m.to.file == 6);
			}
			else if (move == "O-O-O")
			{
				return legalMoves.Find(m => m.isCastle && m.to.file == 2);
			}

			// first find the piece type
			// check the first character of the move string to find the piece type
			// if the first character is a lowercase letter, it is a pawn move
			int pieceType = 0;
			if (Char.IsUpper(move[0]))
			{
				pieceType = AlgebraicNotationToPieceType(move[0].ToString());
				//remove the first character from the move string
				move = move.Remove(0, 1);
			}
			else
			{
				// if the first character is a lowercase letter, it is a pawn move
				pieceType = Piece.Pawn;
			}

			int promotionPieceType = Piece.None;
			// if there is a = in the move, it is a promotion
			if (move.Contains("="))
			{
				// get the promotion piece type
				promotionPieceType = AlgebraicNotationToPieceType(move[move.Length - 1].ToString());
				// remove the last two characters from the move string
				move = move.Remove(move.Length - 2, 2);
			}

			// get the last two characters of the move string to get the end square
			// if there is more than two characters it is specified which piece is moving
			// as there can be more than one piece that can move to the same square of the same type
			// we are therefor checking the last two characters to get the end square
			(int file, int rank) endSquare = AlgebraicToCoordinate(move.Substring(move.Length - 2));

			// remove the last two characters from the move string
			move = move.Remove(move.Length - 2, 2);
			(int file, int rank) startSquare = (-1, -1);
			if (move.Length != 0)
			{
				startSquare = AlgebraicToCoordinate(move);
			}
			// if there is still a character in the move string, it is the file or rank or both of the piece that is moving

			// if there is a promotion piece type, we need to find the move with the correct promotion piece type

			if (promotionPieceType != Piece.None)
			{
				return legalMoves.Find(m => m.to.file == endSquare.file && m.to.rank == endSquare.rank && Piece.Type(board.squares[m.from.file, m.from.rank]) == pieceType && m.promotionType == promotionPieceType);
			}

			// we check if start square file is not -1 then there is two of the same pices that can move to the same square at that file
			if (startSquare.file != -1 && startSquare.rank != -1)
			{
				return legalMoves.Find(m => m.to.file == endSquare.file && m.to.rank == endSquare.rank && Piece.Type(board.squares[m.from.file, m.from.rank]) == pieceType && m.from.file == startSquare.file && m.from.rank == startSquare.rank);
			}
			else if (startSquare.file != -1)
			{
				return legalMoves.Find(m => m.to.file == endSquare.file && m.to.rank == endSquare.rank && Piece.Type(board.squares[m.from.file, m.from.rank]) == pieceType && m.from.file == startSquare.file);
			}
			else if (startSquare.rank != -1)
			{
				return legalMoves.Find(m => m.to.file == endSquare.file && m.to.rank == endSquare.rank && Piece.Type(board.squares[m.from.file, m.from.rank]) == pieceType && m.from.rank == startSquare.rank);
			}
			else
			{
				return legalMoves.Find(m => m.to.file == endSquare.file && m.to.rank == endSquare.rank && Piece.Type(board.squares[m.from.file, m.from.rank]) == pieceType);
			}
		}

		/// <summary>
		/// Findes the algebraic notation of a move that have just been made on the board
		/// </summary>
		/// <param name="board"></param>
		/// <param name="move"></param>
		public static string MoveToAlgebraicNotation(Board board, Move move)
		{
			string algebraicNotation = "";
			int movePiece = getPieceType(board, move);

			// check if the move is a castle move
			if (move.isCastle)
			{
				if (move.to.file == 6)
				{
					// kingside castle
					algebraicNotation = "O-O";
				}
				else
				{
					// queenside castle
					algebraicNotation = "O-O-O";
				}

				// it can either be a capture or a checkmate or a check
				string lastChar = EndChar(move, board);
				// adds the special character to the string

				if (lastChar == "" && move.capturedPiece != Piece.None)
				{
					algebraicNotation += "x";
				}
				algebraicNotation += lastChar;
				return algebraicNotation;
			}

			// pawn moves or a move is a promotion
			if (movePiece == Piece.Pawn || move.promotionType != Piece.None)
			{
				if (move.capturedPiece != Piece.None || move.isEnPassantCapture)
				{
					// if it is a capture move, add the file of the piece that is moving
					algebraicNotation += CoordinateToAlgebraic(move.from.file, -1);
					// add x to the string
					algebraicNotation += "x";
				}

				// add the end square to the string
				algebraicNotation += CoordinateToAlgebraic(move.to.file, move.to.rank);

				// if it is a promotion, we need to add the promotion piece type to the string after a =
				if (move.promotionType != Piece.None)
				{
					algebraicNotation += "=" + ToPGNstringPiece(move.promotionType);
				}
			}
			else
			{
				// add the piece type to the string
				algebraicNotation += ToPGNstringPiece(movePiece);
				// if there is more than one piece of the same type that can move to the same square
				// add the file or rank of the piece that is moving
				(int file, int rank) startSquare = move.from;
				// Findes the moves that are the same type and end square, but not the same start square¨

				// Undskyld mads
				// Unmake the move to find the other moves that are the same type and have the same end square
				board.UnmakeMove(move);
				// generate the moves again, for the board before the move
				List<Move> preMoves = board.moveGenerator.GenerateMoves();
				// find the moves that are the same type and have the same end square
				List<Move> samePieceMoves = preMoves.FindAll(m => getPieceTypeFrom(board, m) == movePiece && m.to == move.to && m.from != move.from);
				// make the move again
				board.MakeMove(move);

				if (samePieceMoves.Count > 0)
				{
					if (samePieceMoves.Exists(m => m.from.file == startSquare.file) && samePieceMoves.Exists(m => m.from.rank == startSquare.rank))
					{
						algebraicNotation += CoordinateToAlgebraic(startSquare.file, startSquare.rank);
					}
					else if (samePieceMoves.Exists(m => m.from.rank == startSquare.rank))
					{
						algebraicNotation += CoordinateToAlgebraic(startSquare.file, -1);
					}
					else if (samePieceMoves.Exists(m => m.from.file == startSquare.file))
					{
						algebraicNotation += CoordinateToAlgebraic(-1, startSquare.rank);
					}
					else
					{
						algebraicNotation += CoordinateToAlgebraic(startSquare.file, -1);
					}
				}

				// if the move is a capture move, add x to the string
				if (move.capturedPiece != Piece.None)
				{
					algebraicNotation += "x";
				}
				// add the end square to the string
				algebraicNotation += CoordinateToAlgebraic(move.to.file, move.to.rank);
			}
			// add the check or checkmate to the string
			algebraicNotation += EndChar(move, board);
			return algebraicNotation;
		}

		/// <summary>
		/// Findes piece type using the end square of the move
		/// </summary>
		private static int getPieceType(Board board, Move move)
		{
			// we are using to as finding the algeebric notation here only works if the move have been made
			return Piece.Type(board.squares[move.to.file, move.to.rank]);
		}

		/// <summary>
		/// Findes piece type using the start square of the move
		/// </summary>
		private static int getPieceTypeFrom(Board board, Move move)
		{
			return Piece.Type(board.squares[move.from.file, move.from.rank]);
		}

		private static int AlgebraicNotationToPieceType(string v)
		{
			return v switch
			{
				"R" => Piece.Rook,
				"N" => Piece.Knight,
				"B" => Piece.Bishop,
				"Q" => Piece.Queen,
				"K" => Piece.King,
				_ => throw new ArgumentException("Invalid piece type")
			};
		}

		public static (int file, int rank) AlgebraicToCoordinate(string v)
		{
			// match the first character of the string with a switch to get the file
			// minus 1 from the rank to get the correct index
			int file = -1;
			int rank = -1;
			if (Char.IsLetter(v[0]))
			{
				file = v[0] switch
				{
					'a' => 0,
					'b' => 1,
					'c' => 2,
					'd' => 3,
					'e' => 4,
					'f' => 5,
					'g' => 6,
					'h' => 7,
					_ => throw new ArgumentException("Invalid file")
				};
				//remove the first character from the string
				v = v.Remove(0, 1);
			}
			if (v.Length != 0 && Char.IsDigit(v[0]))
			{
				rank = int.Parse(v[0].ToString()) - 1;
			}
			return (file, rank);
		}

		/// <summary>
		/// adds the string with check or checkmate to the string
		/// </summary>
		private static string EndChar(Move move, Board board)
		{
			List<Move> legalMoves = board.moveGenerator.GenerateMoves();
			if (legalMoves.Count == 0 && board.moveGenerator.isInCheck)
			{
				// checkmate
				// check if there are any legal moves left, if not it is checkmate
				return "#";
			}
			else if (board.moveGenerator.isInCheck)
			{
				// check
				return "+";
			}
			return "";
		}

		public static string CoordinateToAlgebraic(int file, int rank)
		{
			string algebraic = "";
			algebraic += file switch
			{
				-1 => "",
				0 => "a",
				1 => "b",
				2 => "c",
				3 => "d",
				4 => "e",
				5 => "f",
				6 => "g",
				7 => "h",
				_ => throw new ArgumentException("Invalid file")
			};
			if (rank != -1)
			{
				algebraic += (rank + 1).ToString();
			}
			return algebraic;
		}

		public static string ToPGNstringPiece(int type)
		{
			string FENchar = Piece.Type(type) switch
			{

				Piece.Rook => "R",
				Piece.Knight => "N",
				Piece.Bishop => "B",
				Piece.Queen => "Q",
				Piece.King => "K",
				_ => throw new ArgumentException("Invalid piece type")
			};
			return FENchar;
		}
		
		public static string PrettyPgn(Stack<string> pgnStack){
			// want it to look like 1. then both white and black move and then 2. and so on
			String prettyPgn = "";
			int moveNumber = 1;
			List<string> moves = pgnStack.ToList();
			moves.Reverse();
			int i = 0;

			foreach (string move in moves)
			{
				if (i % 2 == 0)
				{
					prettyPgn += moveNumber + ". " + move + " ";
					moveNumber++;
				}
				else
				{
					prettyPgn += move + " ";
				}
				i++;
			}
			return prettyPgn;
		}
	}
}