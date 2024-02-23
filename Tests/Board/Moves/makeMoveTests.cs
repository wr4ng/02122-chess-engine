using Chess;

namespace Moves
{
    [TestClass]
    public class makeMoveTests
    {
        [TestMethod]
        public void MakeAMove()
        {
            Board board = Board.ImportFromFEN("rnbqkbnr/pppppppp/8/8/8/8/1PPPPPPP/RNBQKBNR w KQkq - 0 1");
            Move move = new Move((0, 0), (0, 2));
            board.MakeMove(move);
            Assert.AreEqual("rnbqkbnr\npppppppp\n--------\n--------\n--------\nR-------\n-PPPPPPP\n-NBQKBNR", board.ToString());
        }
        [TestMethod]
        public void UnmakeAMove()
        {
            Board board = Board.ImportFromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
            Move move = new Move((0, 1), (0, 2));
            board.MakeMove(move);
            board.UnmakeMove(move);
            Assert.AreEqual("rnbqkbnr\npppppppp\n--------\n--------\n--------\n--------\nPPPPPPPP\nRNBQKBNR", board.ToString());
        }
        [TestMethod]
        public void MakeACapture()
        {
            Board board = Board.ImportFromFEN("1nbqkbnr/pppppppp/8/8/8/r7/1PPPPPPP/RNBQKBNR w KQkq - 0 1");
            Move move = new Move((0, 0), (0, 2));
            board.MakeMove(move);
            Assert.AreEqual("-nbqkbnr\npppppppp\n--------\n--------\n--------\nR-------\n-PPPPPPP\n-NBQKBNR", board.ToString());
            Assert.AreEqual('r', move.GetRemovedPiece().ToFENchar());
        }
        [TestMethod]
        public void UnmakeACapture()
        {
            Board board = Board.ImportFromFEN("1nbqkbnr/pppppppp/8/8/8/r7/1PPPPPPP/RNBQKBNR w KQkq - 0 1");
            Move move = new Move((0, 0), (0, 2));
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
    }

}