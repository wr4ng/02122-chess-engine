namespace Representation;

[TestClass]
public class UtilTests
{
	[TestMethod]
	public void CoordinateToStringTest()
	{
		Assert.AreEqual("a1", Util.CoordinateToString(0, 0));
		Assert.AreEqual("h8", Util.CoordinateToString(7, 7));
		Assert.AreEqual("c5", Util.CoordinateToString(4, 2));
		Assert.AreEqual("f7", Util.CoordinateToString(6, 5));
	}
}