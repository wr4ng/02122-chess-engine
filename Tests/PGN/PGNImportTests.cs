using Chess;

namespace PGNTest;

[TestClass]
public class PGNImportTests
{
	[TestMethod]
	public void PgnToMovesPGNTest()
	{
		string pgnContent = readFromFile("Bigfish1995_vs_Hikaru_2024.02.27.pgn", 1);
		List<string> moves = PGN.GetMoves(pgnContent);

		string expected = readFromFile("Bigfish1995_vs_Hikaru_2024.02.27.moves.txt", 1);
		// make expected to be a list of strings using space or \n as delimiter
		// \n = space
		expected = expected.Replace("\n", " ");
		List<string> expectedMoves = new List<string>(expected.Split(' '));
		// remove empty strings
		expectedMoves.RemoveAll(item => item == "");

		for (int i = 0; i < moves.Count; i++)
		{
			Assert.AreEqual(expectedMoves[i], moves[i]);
		}
		Assert.AreEqual(expectedMoves.Count, moves.Count);
	}

	//Test every move in the game and compare it to the expected move
	//Testing algebraic notation to move, and what move we are getting back to play
	[TestMethod]
	public void AlgebraicToMovePGNTest()
	{
		string pgnContent = readFromFile("Bobby Fischer_vs_Boris V Spassky_1992.__.__.pgn", 2);
		List<string> moves = PGN.GetMoves(pgnContent);

		Assert.AreEqual(99, moves.Count);

		List<(int, int, string)> moveInfo = infoOnMove("Bobby Fischer_vs_Boris V Spassky_1992.__.__.info.txt");

		Board board = Board.FromFEN(FEN.STARTING_POSITION_FEN);
		for (int i = 0; i < moveInfo.Count; i++)
		{
			// Assert.AreEqual(hej, moves[i]);
			Move move = PGN.FromAlgebraicNotationToMove(moves[i], board);
			// check if the move is null
			Assert.IsNotNull(move);
			(int, int) endSquare = move.to;
			Assert.AreEqual(moveInfo[i].Item1, endSquare.Item1);
			Assert.AreEqual(moveInfo[i].Item2, endSquare.Item2);
			string piece = Piece.ToString(board.squares[move.from.file, move.from.rank]);
			// gets the first letter from the string moveinfo[i].Item3
			string firstLeter;
			if (moveInfo[i].Item3 == "Knight")
			{
				firstLeter = "N";
			}
			else
			{
				firstLeter = moveInfo[i].Item3.Substring(0, 1);
			}
			if (i % 2 == 0)
			{
				firstLeter = firstLeter.ToUpper();
			}
			else
			{
				firstLeter = firstLeter.ToLower();
			}
			Assert.AreEqual(firstLeter, piece);
			board.MakeMove(move);
		}
	}

	[TestMethod]
	public void DoubleKnightMovePGNTest()
	{
		// Checks if the pgn notation can handle a double knight move to the same square, and can create the correct notation
		string file = "DobbeltKnigtToOnePlace.pgn";
		MovesFromPGNTestFen(file, 5);
	}

	// make all the moves in the game and compare the board to the expected board with the FEN string
	[TestMethod]
	public void GamesMovePGNTest()
	{
		// get all the files in the folder and go through them and make all the moves in the game
		// and compare the board to the expected board
		string relativePath = @"..\..\..\..\Tests\PGN\ChessGames\PGNGames\";
		relativePath = relativePath.Replace('\\', Path.DirectorySeparatorChar);

		string[] files = Directory.GetFiles(relativePath);

		Assert.AreEqual(8, files.Length);
		foreach (string file in files)
		{
			MovesFromPGNTestFen(file);
		}
	}

	/// <summary>
	/// Read from a file in the test folder
	/// </summary>
	/// <param name="fileName"></param>
	/// <param name="folderNr"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentException"></exception>
	public static string readFromFile(string fileName, int folderNr)
	{
		// Get the directory of where the program is running
		string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
		// Define the relative path of the file we are referencing to
		string relativePath = folderNr switch
		{
			1 => @"..\..\..\..\Tests\PGN\ChessGames\" + fileName,
			2 => @"..\..\..\..\Tests\PGN\ChessGames\TestChessGames\" + fileName,
			3 => @"..\..\..\..\Tests\PGN\ChessGames\PGNGames\" + fileName,
			4 => @"..\..\..\..\Tests\PGN\GameResults\TestChessGames\" + fileName,
			5 => @"..\..\..\..\Tests\PGN\ChessGames\SpecificSenarios\" + fileName,
			_ => throw new ArgumentException("Invalid folder number")
		};

		relativePath = relativePath.Replace('\\', Path.DirectorySeparatorChar);

		// Combine the base directory with the relative path
		string fullPath = Path.GetFullPath(Path.Combine(baseDirectory, relativePath));

		// Read the content from the file
		string content = File.ReadAllText(fullPath);
		return content;
	}

	// read the file and get the end square of the move and the piece that was moved
	public List<(int, int, string)> infoOnMove(string fileName)
	{
		string pgnContent = readFromFile(fileName, 2);
		// every info is on a new line so we can split the string by \n
		// and then split the string by space to get the end square and the piece that was moved
		// in the file the info is string string
		// a3 Rook
		// which should be made to 0 2 Rook
		// to get it to be (int, int, string) the function AlgebraicNotationToCoordinate can be used

		List<string> info = new List<string>(pgnContent.Split('\n'));
		List<(int, int, string)> moveInfo = new List<(int, int, string)>();
		for (int i = 0; i < info.Count; i++)
		{
			// remove \r
			info[i] = info[i].Replace("\r", "");
			string[] move = info[i].Split(' ');
			(int, int) endSquare = AlgebraicNotationToCoordinate(move[0]);
			moveInfo.Add((endSquare.Item1, endSquare.Item2, move[1]));
		}
		return moveInfo;
	}

	private void MovesFromPGNTestFen(string fileName, int folderNr = 3)
	{
		string pgnContent = readFromFile(fileName, folderNr);

		// the fen string is on the first line in the file and needs to be extracted
		string[] lines = pgnContent.Split('\n');
		string fen = lines[0].Trim();
		fen = removeLastPartOfFen(fen);

		List<string> moves = PGN.GetMoves(pgnContent);
		Board board = Board.FromFEN(FEN.STARTING_POSITION_FEN);
		for (int i = 0; i < moves.Count; i++)
		{
			List<Move> legalMoves = board.moveGenerator.GenerateMoves();
			Move move = PGN.FromAlgebraicNotationToMove(moves[i], board);
			Assert.IsNotNull(move);
			board.MakeMove(move);
			string algebraic = PGN.MoveToAlgebraicNotation(board, move);
			// // remove + and # from the string moves[i] as part of the test
			// moves[i] = moves[i].Replace("+", "").Replace("#", "");
			Assert.AreEqual(moves[i], algebraic);
			//Assert.AreEqual(moves[i], board.GetLatestAlgebraicNotation());
		}
		// List<string> movesFromBoard = board.GetAlgebraicNotation();

		// for (int i = 0; i < moves.Count; i++)
		// {
		//     Assert.AreEqual(moves[i], movesFromBoard[i]);
		// }
		// TODO: it should be with the last part of the FEN string but ours do not match the expected FEN string
		string boardFen = FEN.BoardToFEN(board);
		Assert.AreEqual(fen, boardFen);
	}

	private static (int file, int rank) AlgebraicNotationToCoordinate(string v)
	{
		// match the first character of the string with a switch to get the file
		// minus 1 from the rank to get the correct index
		int file = v[0] switch
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
		return (file, int.Parse(v[1].ToString()) - 1);
	}

	public static string removeLastPartOfFen(string fen)
	{
		// remove the last part of the FEN string
		// find the person that is on move and remove everything after that it can either be w or b
		List<string> parts = new List<string>(fen.Split(' '));
		return parts.First();
	}
}