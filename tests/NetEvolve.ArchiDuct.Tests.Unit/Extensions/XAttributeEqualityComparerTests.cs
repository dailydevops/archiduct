namespace NetEvolve.ArchiDuct.Tests.Unit.Extensions;

using System.Xml.Linq;
using NetEvolve.ArchiDuct.Extensions;

public class XAttributeEqualityComparerTests
{
    [Test]
    public async Task Equals_WithEqualAttributes_ReturnsTrue()
    {
        // Arrange
        var attribute1 = new XAttribute("Name", "Value");
        var attribute2 = new XAttribute("Name", "Value");

        // Act
        var result = XAttributeEqualityComparer.Instance.Equals(attribute1, attribute2);

        // Assert
        _ = await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equals_WithDifferentAttributes_ReturnsFalse()
    {
        // Arrange
        var attribute1 = new XAttribute("Name1", "Value1");
        var attribute2 = new XAttribute("Name2", "Value2");

        // Act
        var result = XAttributeEqualityComparer.Instance.Equals(attribute1, attribute2);

        // Assert
        _ = await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equals_WithNullAttributes_ReturnsTrue()
    {
        // Arrange
        XAttribute? attribute1 = null;
        XAttribute? attribute2 = null;

        // Act
        var result = XAttributeEqualityComparer.Instance.Equals(attribute1, attribute2);

        // Assert
        _ = await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task GetHashCode_WithNonNullAttribute_ReturnsHashCode()
    {
        // Arrange
        var attribute = new XAttribute("Name", "Value");

        // Act
        var hashCode = XAttributeEqualityComparer.Instance.GetHashCode(attribute);

        // Assert
        _ = await Assert.That(attribute.GetHashCode()).IsEqualTo(hashCode);
    }

    [Test]
    public async Task GetHashCode_WithNullAttribute_ReturnsDefaultHashCode()
    {
        // Arrange
        XAttribute? attribute = null;

        // Act
        var hashCode = XAttributeEqualityComparer.Instance.GetHashCode(attribute);

        // Assert
        _ = await Assert.That(hashCode).IsEqualTo(default);
    }
}
