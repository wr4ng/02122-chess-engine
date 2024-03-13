using Chess;

namespace Moves
{
    [TestClass]
    public class enPassantTests
    {
        [TestMethod]
        public void AddingEnPassantSquare(){
            Board board = Board.ImportFromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
            Move move = Move.DoublePawnMove((3,1),(3,3),(3,2));
            board.MakeMove(move);
            Assert.AreEqual((3,2), board.GetEnPassantCoords());
            board.UnmakeMove(move);
            Assert.AreEqual((-1,-1), board.GetEnPassantCoords());
        }

        [TestMethod]
        public void UpdatingEnPassantSquare(){
            Board board = Board.ImportFromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
            Move move = Move.DoublePawnMove((3,1),(3,3),(3,2));
            board.MakeMove(move);
            Move nextMove = Move.SimpleMove((1,7),(2,5),PieceType.Knight);
            board.MakeMove(nextMove);
            Assert.AreEqual((-1,-1), board.GetEnPassantCoords());
        }
    }
}