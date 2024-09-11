namespace NetEvolve.ArchiDuct.Tests.Unit.Extensions;

using System.Xml.Linq;
using NetEvolve.ArchiDuct.Extensions;
using Xunit;

public class XNameEqualityComparerTests
{
    [Fact]
    public void Equals_WhenBothXNameAreNull_ReturnsTrue()
    {
        // Arrange
        XName? x = null;
        XName? y = null;

        // Act
        var result = XNameEqualityComparer.Instance.Equals(x, y);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_WhenOneXNameIsNull_ReturnsFalse()
    {
        // Arrange
        var x = XName.Get("element");
        XName? y = null;

        // Act
        var result = XNameEqualityComparer.Instance.Equals(x, y);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_WhenXNameLocalNamesAreEqual_ReturnsTrue()
    {
        // Arrange
        var x = XName.Get("element");
        var y = XName.Get("element");

        // Act
        var result = XNameEqualityComparer.Instance.Equals(x, y);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_WhenXNameLocalNamesAreNotEqual_ReturnsFalse()
    {
        // Arrange
        var x = XName.Get("element1");
        var y = XName.Get("element2");

        // Act
        var result = XNameEqualityComparer.Instance.Equals(x, y);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetHashCode_WhenXNameIsNull_ReturnsDefaultHashCode()
    {
        // Arrange
        XName? x = null;

        // Act
        var result = XNameEqualityComparer.Instance.GetHashCode(x);

        // Assert
        Assert.Equal(default, result);
    }

    [Fact]
    public void GetHashCode_WhenXNameIsNotNull_ReturnsHashCode()
    {
        // Arrange
        var x = XName.Get("element");

        // Act
        var result = XNameEqualityComparer.Instance.GetHashCode(x);

        // Assert
        Assert.Equal(x.GetHashCode(), result);
    }
}
