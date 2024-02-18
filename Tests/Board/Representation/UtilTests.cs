namespace Representation;

[TestClass]
public class UtilTests
{
	[TestMethod]
	public void CoordinateToStringTest()
	{
		Assert.AreEqual("a1", Util.CoordinateToString(0, 0));
		Assert.AreEqual("h8", Util.CoordinateToString(7, 7));
		Assert.AreEqual("e3", Util.CoordinateToString(4, 2));
		Assert.AreEqual("g6", Util.CoordinateToString(6, 5));
	}
}