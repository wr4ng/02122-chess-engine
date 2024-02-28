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
            Move move = Move.SimpleMove((0, 0), (0, 2));
            board.MakeMove(move);
            Assert.AreEqual("rnbqkbnr\npppppppp\n--------\n--------\n--------\nR-------\n-PPPPPPP\n-NBQKBNR", board.ToString());
        }
        [TestMethod]
        public void UnmakeAMove()
        {
            Board board = Board.ImportFromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
            Move move = Move.SimpleMove((0, 1), (0, 2));
            board.MakeMove(move);
            board.UnmakeMove(move);
            Assert.AreEqual("rnbqkbnr\npppppppp\n--------\n--------\n--------\n--------\nPPPPPPPP\nRNBQKBNR", board.ToString());
        }
        [TestMethod]
        public void MakeACapture()
        {
            Board board = Board.ImportFromFEN("1nbqkbnr/pppppppp/8/8/8/r7/1PPPPPPP/RNBQKBNR w KQkq - 0 1");
            Move move = Move.CaptureMove((0, 0), (0, 2), board.GetPiece((0, 2)));
            board.MakeMove(move);
            Assert.AreEqual("-nbqkbnr\npppppppp\n--------\n--------\n--------\nR-------\n-PPPPPPP\n-NBQKBNR", board.ToString());
            Assert.AreEqual('r', move.GetCapturedPiece().ToFENchar());
        }
        [TestMethod]
        public void UnmakeACapture()
        {
            Board board = Board.ImportFromFEN("1nbqkbnr/pppppppp/8/8/8/r7/1PPPPPPP/RNBQKBNR w KQkq - 0 1");
            Move move = Move.CaptureMove((0, 0), (0, 2), board.GetPiece((0, 2)));
            board.MakeMove(move);
            board.UnmakeMove(move);
            Assert.AreEqual("-nbqkbnr\npppppppp\n--------\n--------\n--------\nr-------\n-PPPPPPP\nRNBQKBNR", board.ToString());
        }
        [TestMethod]
        public void MakeEnPassant()
        {
            Board board = Board.ImportFromFEN("k7/8/8/3pP3/8/8/8/K7 w KQkq d6 0 1");
            List<Move> moves = MoveGenerator.GeneratePawnMove((4, 4), board);
            board.MakeMove(moves[1]);
            Assert.AreEqual("k-------\n--------\n---P----\n--------\n--------\n--------\n--------\nK-------", board.ToString());
            board = Board.ImportFromFEN("k7/8/8/4Pp2/8/8/8/K7 w KQkq f6 0 1");
            moves = MoveGenerator.GeneratePawnMove((4, 4), board);
            board.MakeMove(moves[1]);
            Assert.AreEqual("k-------\n--------\n-----P--\n--------\n--------\n--------\n--------\nK-------", board.ToString());

            board = Board.ImportFromFEN("k7/8/8/8/3pP3/8/8/K7 b KQkq e3 0 1");
            moves = MoveGenerator.GeneratePawnMove((3, 3), board);
            board.MakeMove(moves[1]);
            Assert.AreEqual("k-------\n--------\n--------\n--------\n--------\n----p---\n--------\nK-------", board.ToString());
            board = Board.ImportFromFEN("k7/8/8/8/4Pp2/8/8/K7 b KQkq e3 0 1");
            moves = MoveGenerator.GeneratePawnMove((5, 3), board);
            board.MakeMove(moves[1]);
            Assert.AreEqual("k-------\n--------\n--------\n--------\n--------\n----p---\n--------\nK-------", board.ToString());
        }

        [TestMethod]
        public void MakeCastling()
        {
            Board board = Board.ImportFromFEN("4k3/8/8/8/8/8/8/R3K2R w KQkq - 0 1");
            List<Move> moves = MoveGenerator.GenerateCastlingMoves(board);
            board.MakeMove(moves[0]);
            Assert.AreEqual("----k---\n--------\n--------\n--------\n--------\n--------\n--------\nR----RK-", board.ToString());
            board.UnmakeMove(moves[0]);
            board.MakeMove(moves[1]);
            Assert.AreEqual("----k---\n--------\n--------\n--------\n--------\n--------\n--------\n--KR---R", board.ToString());
            board = Board.ImportFromFEN("4k3/8/8/8/8/8/8/R3K2R w Kkq - 0 1");
            moves = MoveGenerator.GenerateCastlingMoves(board);
            Assert.AreEqual(1, moves.Count);
            board = Board.ImportFromFEN("4k3/8/8/8/8/8/8/R3K2R w kq - 0 1");
            moves = MoveGenerator.GenerateCastlingMoves(board);
            Assert.AreEqual(0, moves.Count);
        }
    }

}