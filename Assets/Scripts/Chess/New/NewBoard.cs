namespace Chess
{
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
			NewBoard board = new();

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
	}
}