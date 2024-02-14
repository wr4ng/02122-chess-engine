namespace Representation;

[TestClass]
public class BoardTests
{
    [TestMethod]
    public void FENWithOnePiece()
    {
        Board b = new Board();
        b.importFromFEN("8/8/8/7p/8/8/8/8");
        Assert.AreEqual("black Pawn", b.getPiece(7, 4).toString());
    }
}
