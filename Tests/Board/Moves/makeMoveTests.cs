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
    }

}