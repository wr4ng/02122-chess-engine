using Chess;

namespace Representation{

[TestClass]
public class MoveTests{

    [TestMethod]
    public void GeneratePawnMoveTwoForward(){
        Board board = Board.ImportFromFEN("7k/8/8/8/8/8/7P/7K w - - 0 1");
        List<Move> moves = MoveGenerator.GeneratePawnMove((7,1),board);
        Assert.AreEqual(2,moves.Count);
        Assert.AreEqual((7,1),(moves[0].start));
        Assert.AreEqual((7,2),(moves[0].end));
        Assert.AreEqual((7,1),(moves[1].start));
        Assert.AreEqual((7,3),(moves[1].end));
    }

    [TestMethod]
    public void GeneratePawnMoveOneForward(){
        Board board = Board.ImportFromFEN("7k/8/8/8/7p/8/7P/7K w - - 0 1");
        List<Move> moves = MoveGenerator.GeneratePawnMove((7,1),board);
        Assert.AreEqual(1,moves.Count);
        Assert.AreEqual((7,1),(moves[0].start));
        Assert.AreEqual((7,2),(moves[0].end));
    }

    [TestMethod]
    public void GeneratePawnMoveNotStartingRank(){
        Board board = Board.ImportFromFEN("7k/8/8/8/8/7P/8/7K w - - 0 1");
        List<Move> moves = MoveGenerator.GeneratePawnMove((7,2),board);
        Assert.AreEqual(1,moves.Count);
        Assert.AreEqual((7,2),(moves[0].start));
        Assert.AreEqual((7,3),(moves[0].end));
    }

    [TestMethod]
    public void GeneratePawnMoveNoMoves(){
        Board board = Board.ImportFromFEN("7k/8/8/8/8/7P/7P/7K w - - 0 1");
        List<Move> moves = MoveGenerator.GeneratePawnMove((7,1),board);
        Assert.AreEqual(0,moves.Count);
    }

    [TestMethod]
    public void GeneratePawnMoveAllPossible(){
        Board board = Board.ImportFromFEN("7k/8/8/8/8/5p1p/6P1/7K w - - 0 1");
        List<Move> moves = MoveGenerator.GeneratePawnMove((6,1),board);
        Assert.AreEqual(4,moves.Count);
        Assert.AreEqual((6,1),(moves[0].start));
        Assert.AreEqual((6,2),(moves[0].end));
        Assert.AreEqual((6,1),(moves[1].start));
        Assert.AreEqual((6,3),(moves[1].end));
        Assert.AreEqual((6,1),(moves[2].start));
        Assert.AreEqual((5,2),(moves[2].end));
        Assert.AreEqual((6,1),(moves[3].start));
        Assert.AreEqual((7,2),(moves[3].end));
    }
    
    [TestMethod]
    public void GeneratePawnMoveEdgeCases(){
        Board board = Board.ImportFromFEN("7k/8/8/8/8/6pp/7P/7K w - - 0 1");
        List<Move> moves = MoveGenerator.GeneratePawnMove((7,1),board);
        Assert.AreEqual(1,moves.Count);
        Assert.AreEqual((7,1),(moves[0].start));
        Assert.AreEqual((6,2),(moves[0].end));
        board = Board.ImportFromFEN("7k/8/8/8/8/pp6/P7/7K w - - 0 1");
        moves = MoveGenerator.GeneratePawnMove((0,1),board);
        Assert.AreEqual(1,moves.Count);
        Assert.AreEqual((0,1),(moves[0].start));
        Assert.AreEqual((1,2),(moves[0].end));
    }

}

}