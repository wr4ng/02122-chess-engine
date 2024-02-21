using System;

namespace Chess
{
	public class Board
	{
		public const int BOARD_SIZE = 8;

		private Piece[,] board = new Piece[BOARD_SIZE, BOARD_SIZE];
		private Color currentPlayer;
		private CastlingRights castlingRights;
		private (int, int) enPassantSquare; // (file, rank), holds possible en Passant square
		private int halfmoveClock; // Used to determine fifty move rule
		private int fullmoveNumber; // The number of full moves made in the game

		private Board() { }

		public static Board ImportFromFEN(string fen)
		{
			Board board = new Board();
			// Validate FEN parts
			string[] fenParts = fen.Split(' ');
			if (fenParts.Length != 6)
			{
				throw new ArgumentException($"Invalid FEN string (wrong number of parts): {fen}");
			}
			// Parse each fen part
			board.board = FEN.ParseBoard(fenParts[0]);
			board.currentPlayer = FEN.ParsePlayer(fenParts[1]);
			board.castlingRights = FEN.ParseCastlingRights(fenParts[2]);
			board.enPassantSquare = FEN.ParseEnPassant(fenParts[3]);
			board.halfmoveClock = FEN.ParseHalfmoveClock(fenParts[4]);
			board.fullmoveNumber = FEN.ParseFullmoveNumber(fenParts[5]);
			// Return resulting board
			return board;
		}

		public string ExportToFEN()
		{
			string fen = "";
			//add the board state "https://en.wikipedia.org/wiki/Forsyth%E2%80%93Edwards_Notation"
			fen += FEN.BoardToFEN(this.board);
			fen += FEN.CurrentPlayerToFEN(this.currentPlayer);
			fen += FEN.CastlingRightsToFEN(this.castlingRights);
			fen += FEN.EnPassantToFEN(this.enPassantSquare);
			fen += FEN.HalfmoveClockToFEN(this.halfmoveClock);
			fen += FEN.FullmoveNumberToFEN(this.fullmoveNumber);
			return fen;
		}

		public Piece GetPiece(int file, int rank)
		{
			return board[file, rank];
		}

		public Color GetCurrentPlayer()
		{
			return currentPlayer;
		}

		public string GetCastlingRights()
		{
			return castlingRights.ToFENString();
		}

		public string GetEnPassantSquare()
		{
			return enPassantSquare == (-1, -1) ? "-" : FEN.CoordinateToFEN(enPassantSquare);
		}
		public (int,int) GetEnPassantCoords(){
			return enPassantSquare;
		}
		public Piece[,] GetBoard(){
			return board;
		}

		public int GetHalfmoveClock()
		{
			return halfmoveClock;
		}

		public int GetFullmoveNumber()
		{
			return fullmoveNumber;
		}

		public override string ToString()
		{
			String result = "";
			for (int rank = BOARD_SIZE - 1; 0 <= rank; rank--)
			{
				for (int file = 0; file < BOARD_SIZE; file++)
				{
					if (board[file, rank] == null)
					{
						result += "-";
					}
					else
					{
						result += board[file, rank].ToFENchar();
					}
				}
				result += "\n";
			}
			return result.Trim();
		}
	}
}