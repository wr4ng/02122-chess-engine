using System;

public class Board
{
	private Piece[,] board = new Piece[8, 8];
	private Color currentPlayer;
	private CastlingRights castlingRights;
	private string enPassantSquare; //same, string?
	private int halfmoveClock; //fifty move rule
	private int fullmoveNumber; //number of the full move

	public Board()
	{
		currentPlayer = Color.White;

		for (int rank = 0; rank < 8; rank++) //TODO check that file and rank is correct, idk
		{
			for (int file = 0; file < 8; file++)
			{
				board[rank, file] = null;
			}
		}
	}

	public void ImportFromFEN(string fen)
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

	public Piece GetPiece(int rank, int file)
	{
		return board[rank, file];
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
		return enPassantSquare;
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