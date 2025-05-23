﻿namespace NetEvolve.ArchiDuct.Tests.Unit.Extensions;

using System.Xml.Linq;
using NetEvolve.ArchiDuct.Extensions;

public class XElementEqualityComparerTests
{
    [Test]
    public async Task Equals_WhenXAndYAreNull_ReturnsTrue()
    {
        // Arrange
        XElement? x = null;
        XElement? y = null;

        // Act
        var result = XElementEqualityComparer.Instance.Equals(x, y);

        // Assert
        _ = await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equals_WhenXIsNullAndYIsNotNull_ReturnsFalse()
    {
        // Arrange
        XElement? x = null;
        var y = new XElement("element");

        // Act
        var result = XElementEqualityComparer.Instance.Equals(x, y);

        // Assert
        _ = await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equals_WhenXIsNotNullAndYIsNull_ReturnsFalse()
    {
        // Arrange
        var x = new XElement("element");
        XElement? y = null;

        // Act
        var result = XElementEqualityComparer.Instance.Equals(x, y);

        // Assert
        _ = await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equals_WhenXAndYAreSameInstance_ReturnsTrue()
    {
        // Arrange
        var x = new XElement("element");
        var y = x;

        // Act
        var result = XElementEqualityComparer.Instance.Equals(x, y);

        // Assert
        _ = await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equals_WhenXAndYHaveDifferentNames_ReturnsFalse()
    {
        // Arrange
        var x = new XElement("element1");
        var y = new XElement("element2");

        // Act
        var result = XElementEqualityComparer.Instance.Equals(x, y);

        // Assert
        _ = await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equals_WhenXAndYHaveDifferentAttributes_ReturnsFalse()
    {
        // Arrange
        var x = new XElement("element", new XAttribute("attr1", "value1"));
        var y = new XElement("element", new XAttribute("attr2", "value2"));

        // Act
        var result = XElementEqualityComparer.Instance.Equals(x, y);

        // Assert
        _ = await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equals_WhenXAndYHaveDifferentElements_ReturnsFalse()
    {
        // Arrange
        var x = new XElement("element", new XElement("child1"));
        var y = new XElement("element", new XElement("child2"));

        // Act
        var result = XElementEqualityComparer.Instance.Equals(x, y);

        // Assert
        _ = await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equals_WhenXAndYAreEqual_ReturnsTrue()
    {
        // Arrange
        var x = new XElement("element", new XAttribute("attr", "value"), new XElement("child"));
        var y = new XElement("element", new XAttribute("attr", "value"), new XElement("child"));

        // Act
        var result = XElementEqualityComparer.Instance.Equals(x, y);

        // Assert
        _ = await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task GetHashCode_WhenXIsNull_ReturnsDefaultHashCode()
    {
        // Arrange
        XElement? x = null;

        // Act
        var hashCode = XElementEqualityComparer.Instance.GetHashCode(x);

        // Assert
        _ = await Assert.That(hashCode).IsEqualTo(default);
    }

    [Test]
    public async Task GetHashCode_WhenXIsNotNull_ReturnsXHashCode()
    {
        // Arrange
        var x = new XElement("element");

        // Act
        var hashCode = XElementEqualityComparer.Instance.GetHashCode(x);

        // Assert
        _ = await Assert.That(hashCode).IsEqualTo(x.GetHashCode());
    }
}
