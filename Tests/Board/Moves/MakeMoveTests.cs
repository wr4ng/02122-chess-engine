using Chess;

namespace Moves
{
	[TestClass]
	public class MakeMoveTests
	{
		[TestMethod]
		public void MakeAMove()
		{
			Board board = Board.ImportFromFEN("rnbqkbnr/pppppppp/8/8/8/8/1PPPPPPP/RNBQKBNR w KQkq - 0 1");
			Move move = Move.SimpleMove((0, 0), (0, 2), PieceType.Rook);
			board.MakeMove(move);
			Assert.AreEqual("rnbqkbnr\npppppppp\n--------\n--------\n--------\nR-------\n-PPPPPPP\n-NBQKBNR", board.ToString());
		}

		[TestMethod]
		public void UnmakeAMove()
		{
			Board board = Board.ImportFromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
			Move move = Move.SimpleMove((0, 1), (0, 2), PieceType.Pawn);
			board.MakeMove(move);
			board.UnmakeMove(move);
			Assert.AreEqual("rnbqkbnr\npppppppp\n--------\n--------\n--------\n--------\nPPPPPPPP\nRNBQKBNR", board.ToString());
		}

		[TestMethod]
		public void MakeACapture()
		{
			Board board = Board.ImportFromFEN("1nbqkbnr/pppppppp/8/8/8/r7/1PPPPPPP/RNBQKBNR w KQkq - 0 1");
			Move move = Move.CaptureMove((0, 0), (0, 2), PieceType.Rook, board.GetPiece((0, 2)));
			board.MakeMove(move);
			Assert.AreEqual("-nbqkbnr\npppppppp\n--------\n--------\n--------\nR-------\n-PPPPPPP\n-NBQKBNR", board.ToString());
			Assert.AreEqual('r', move.GetCapturedPiece().ToFENchar());
		}

		[TestMethod]
		public void UnmakeACapture()
		{
			Board board = Board.ImportFromFEN("1nbqkbnr/pppppppp/8/8/8/r7/1PPPPPPP/RNBQKBNR w KQkq - 0 1");
			Move move = Move.CaptureMove((0, 0), (0, 2), PieceType.Rook, board.GetPiece((0, 2)));
			board.MakeMove(move);
			board.UnmakeMove(move);
			Assert.AreEqual("-nbqkbnr\npppppppp\n--------\n--------\n--------\nr-------\n-PPPPPPP\nRNBQKBNR", board.ToString());
		}

		[TestMethod]
		public void MakeEnPassant()
		{
			Board board = Board.ImportFromFEN("k7/8/8/3pP3/8/8/8/K7 w KQkq d6 0 1");
			List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(board);
			List<Move> enPassantMoves = legalMoves.Where(move => move.IsEnPassant()).ToList();
			board.MakeMove(enPassantMoves[0]);
			Assert.AreEqual("k-------\n--------\n---P----\n--------\n--------\n--------\n--------\nK-------", board.ToString());

			board = Board.ImportFromFEN("k7/8/8/4Pp2/8/8/8/K7 w KQkq f6 0 1");
			legalMoves = MoveGenerator.GenerateLegalMoves(board);
			enPassantMoves = legalMoves.Where(move => move.IsEnPassant()).ToList();
			board.MakeMove(enPassantMoves[0]);
			Assert.AreEqual("k-------\n--------\n-----P--\n--------\n--------\n--------\n--------\nK-------", board.ToString());

			board = Board.ImportFromFEN("k7/8/8/8/3pP3/8/8/K7 b KQkq e3 0 1");
			legalMoves = MoveGenerator.GenerateLegalMoves(board);
			enPassantMoves = legalMoves.Where(move => move.IsEnPassant()).ToList();
			board.MakeMove(enPassantMoves[0]);
			Assert.AreEqual("k-------\n--------\n--------\n--------\n--------\n----p---\n--------\nK-------", board.ToString());

			board = Board.ImportFromFEN("k7/8/8/8/4Pp2/8/8/K7 b KQkq e3 0 1");
			legalMoves = MoveGenerator.GenerateLegalMoves(board);
			enPassantMoves = legalMoves.Where(move => move.IsEnPassant()).ToList();
			board.MakeMove(enPassantMoves[0]);
			Assert.AreEqual("k-------\n--------\n--------\n--------\n--------\n----p---\n--------\nK-------", board.ToString());
		}

		//TODO Tests that castlingrights correctly updates
		[TestMethod]
		public void MakeCastling()
		{
			Board board = Board.ImportFromFEN("4k3/8/8/8/8/8/8/R3K2R w KQkq - 0 1");
			List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(board);
			List<Move> castleMoves = legalMoves.Where(move => move.IsCastle()).ToList();
			foreach (Move m in castleMoves)
			{
				Console.WriteLine($"{m.GetStartSquare()} - {m.GetEndSquare()}");
			}
			board.MakeMove(castleMoves[0]);
			Assert.AreEqual("----k---\n--------\n--------\n--------\n--------\n--------\n--------\nR----RK-", board.ToString());
			board.UnmakeMove(castleMoves[0]);
			board.MakeMove(castleMoves[1]);
			Assert.AreEqual("----k---\n--------\n--------\n--------\n--------\n--------\n--------\n--KR---R", board.ToString());

			board = Board.ImportFromFEN("4k3/8/8/8/8/8/8/R3K2R w Kkq - 0 1");
			legalMoves = MoveGenerator.GenerateLegalMoves(board);
			castleMoves = legalMoves.Where(move => move.IsCastle()).ToList();
			Assert.AreEqual(1, castleMoves.Count);

			board = Board.ImportFromFEN("4k3/8/8/8/8/8/8/R3K2R w kq - 0 1");
			legalMoves = MoveGenerator.GenerateLegalMoves(board);
			castleMoves = legalMoves.Where(move => move.IsCastle()).ToList();
			Assert.AreEqual(0, castleMoves.Count);
		}

		[TestMethod]
		public void MakePromotion()
		{
			Board board = Board.ImportFromFEN("8/k4P2/8/8/8/8/8/7K w - - 0 1");
			List<Move> legalMoves = MoveGenerator.GenerateLegalMoves(board);
			List<Move> promotionMoves = legalMoves.Where(move => move.IsPromotion()).ToList();

			// Rook promotion
			board.MakeMove(promotionMoves[0]);
			Assert.IsTrue(board.ExportToFEN().StartsWith("5R2/k7/8/8/8/8/8/7K b"));
			board.UnmakeMove(promotionMoves[0]);
			Assert.AreEqual("8/k4P2/8/8/8/8/8/7K w - - 0 1", board.ExportToFEN());

			// Queen promotion
			board.MakeMove(promotionMoves[3]);
			Assert.IsTrue(board.ExportToFEN().StartsWith("5Q2/k7/8/8/8/8/8/7K b"));
			board.UnmakeMove(promotionMoves[3]);
			Assert.AreEqual("8/k4P2/8/8/8/8/8/7K w - - 0 1", board.ExportToFEN());
		}

		// TODO Capture promotion tests
	}

}