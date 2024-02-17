using System;

namespace Chess
{
	public class Board
	{
		public const int BOARD_SIZE = 8;

		private Piece[,] board = new Piece[BOARD_SIZE, BOARD_SIZE];
		private Color currentPlayer;
		private CastlingRights castlingRights;
		(int, int) enPassantSquare; // (file, rank)
		private int halfmoveClock; // Used to determine fifty move rule
		private int fullmoveNumber; // The number of full moves made in the game

		private Board() {}

		public static Board ImportFromFEN(string fen)
		{
			throw new NotImplementedException();
		}

		public string ExportToFEN()
		{
			//add the board state "https://en.wikipedia.org/wiki/Forsyth%E2%80%93Edwards_Notation"
			//add the current player
			//add the castling rights
			//add the en passant square
			//add the halfmove clock
			//add the fullmove number
			throw new NotImplementedException();
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
			return Util.CoordinateToString(enPassantSquare.Item1, enPassantSquare.Item2);
		}

		public int GetHalfmoveClock()
		{
			return halfmoveClock;
		}

		public int GetFullmoveNumber()
		{
			return fullmoveNumber;
		}
	}
}