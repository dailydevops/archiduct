namespace NetEvolve.ArchiDuct.Tests.Unit.Extensions;

using System.Xml.Linq;
using NetEvolve.ArchiDuct.Extensions;

public class XNameEqualityComparerTests
{
    [Test]
    public async Task Equals_WhenBothXNameAreNull_ReturnsTrue()
    {
        // Arrange
        XName? x = null;
        XName? y = null;

        // Act
        var result = XNameEqualityComparer.Instance.Equals(x, y);

        // Assert
        _ = await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equals_WhenOneXNameIsNull_ReturnsFalse()
    {
        // Arrange
        var x = XName.Get("element");
        XName? y = null;

        // Act
        var result = XNameEqualityComparer.Instance.Equals(x, y);

        // Assert
        _ = await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equals_WhenXNameLocalNamesAreEqual_ReturnsTrue()
    {
        // Arrange
        var x = XName.Get("element");
        var y = XName.Get("element");

        // Act
        var result = XNameEqualityComparer.Instance.Equals(x, y);

        // Assert
        _ = await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equals_WhenXNameLocalNamesAreNotEqual_ReturnsFalse()
    {
        // Arrange
        var x = XName.Get("element1");
        var y = XName.Get("element2");

        // Act
        var result = XNameEqualityComparer.Instance.Equals(x, y);

        // Assert
        _ = await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task GetHashCode_WhenXNameIsNull_ReturnsDefaultHashCode()
    {
        // Arrange
        XName? x = null;

        // Act
        var result = XNameEqualityComparer.Instance.GetHashCode(x);

        // Assert
        _ = await Assert.That(result).IsEqualTo(default);
    }

    [Test]
    public async Task GetHashCode_WhenXNameIsNotNull_ReturnsHashCode()
    {
        // Arrange
        var x = XName.Get("element");

        // Act
        var result = XNameEqualityComparer.Instance.GetHashCode(x);

        // Assert
        _ = await Assert.That(result).IsEqualTo(x.GetHashCode());
    }
}
