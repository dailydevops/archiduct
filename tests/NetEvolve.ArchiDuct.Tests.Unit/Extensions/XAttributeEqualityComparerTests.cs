namespace NetEvolve.ArchiDuct.Tests.Unit.Extensions;

using System.Xml.Linq;
using NetEvolve.ArchiDuct.Extensions;
using Xunit;

public class XAttributeEqualityComparerTests
{
    [Fact]
    public void Equals_WithEqualAttributes_ReturnsTrue()
    {
        // Arrange
        var attribute1 = new XAttribute("Name", "Value");
        var attribute2 = new XAttribute("Name", "Value");

        // Act
        var result = XAttributeEqualityComparer.Instance.Equals(attribute1, attribute2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_WithDifferentAttributes_ReturnsFalse()
    {
        // Arrange
        var attribute1 = new XAttribute("Name1", "Value1");
        var attribute2 = new XAttribute("Name2", "Value2");

        // Act
        var result = XAttributeEqualityComparer.Instance.Equals(attribute1, attribute2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_WithNullAttributes_ReturnsTrue()
    {
        // Arrange
        XAttribute? attribute1 = null;
        XAttribute? attribute2 = null;

        // Act
        var result = XAttributeEqualityComparer.Instance.Equals(attribute1, attribute2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void GetHashCode_WithNonNullAttribute_ReturnsHashCode()
    {
        // Arrange
        var attribute = new XAttribute("Name", "Value");

        // Act
        var hashCode = XAttributeEqualityComparer.Instance.GetHashCode(attribute);

        // Assert
        Assert.Equal(attribute.GetHashCode(), hashCode);
    }

    [Fact]
    public void GetHashCode_WithNullAttribute_ReturnsDefaultHashCode()
    {
        // Arrange
        XAttribute? attribute = null;

        // Act
        var hashCode = XAttributeEqualityComparer.Instance.GetHashCode(attribute);

        // Assert
        Assert.Equal(default, hashCode);
    }
}
