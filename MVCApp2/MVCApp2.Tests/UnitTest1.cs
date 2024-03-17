using NUnit.Framework;
//using NUnit.Framework.Legacy;
namespace MVCApp2.Tests;

[TestFixture]
public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }

    [Test]
    public void testSumMethod()
    {
        int expectedSum1_1 = 2;

        int methodResult = sum(1, 1);

        //ClassicAssert.AreEqual(expectedSum1_1, methodResult);
        Assert.That(methodResult, Is.EqualTo(expectedSum1_1));
    }
    int sum(int parameter1, int parameter2)
    {
        return parameter1 + parameter2;
    }
}