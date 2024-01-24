using AdventOfCode.Datas;
using NUnit.Framework;

public class CoordTest
{
    private IntCoords _coordA = null;
    private IntCoords _coordB = null;

    [SetUp]
    public void Setup ()
    {
        _coordA = new IntCoords(2, 3);
        _coordB = new IntCoords(-4, -3);
    }

    [TearDown]
    public void TearDown()
    {
        _coordA = null;
        _coordB = null;
    }

    [Test]
    public void IntCoordsCreationTest()
    {
        Assert.That(_coordA, Is.Not.Null);
        Assert.That(_coordA.X, Is.EqualTo(2));
        Assert.That(_coordA.Y, Is.EqualTo(3));
        Assert.That(_coordB, Is.Not.Null);
        Assert.That(_coordB.X, Is.EqualTo(-4));
        Assert.That(_coordB.Y, Is.EqualTo(-3));
    }

    [Test]
    public void IntCoordsEqualsTest()
    {
        IntCoords cloneA = new IntCoords(2, 3);
        IntCoords cloneB = new IntCoords(-4, -3);

        Assert.That(cloneA == _coordA, Is.True);
        Assert.That(cloneB == _coordB, Is.True);
        Assert.That(cloneA == _coordB, Is.False);
    }

    [Test]
    public void IntCoordsNotEqualsTest()
    {
        IntCoords cloneA = new IntCoords(2, 3);
        Assert.That(_coordA != _coordB, Is.True);
        Assert.That(_coordA != cloneA, Is.False);
    }
}
