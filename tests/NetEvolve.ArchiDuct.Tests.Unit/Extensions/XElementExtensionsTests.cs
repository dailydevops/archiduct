namespace NetEvolve.ArchiDuct.Tests.Unit.Extensions;

using System;
using System.Xml.Linq;
using NetEvolve.ArchiDuct.Extensions;
using Xunit;

public class XElementExtensionsTests
{
    [Fact]
    public void GetCRefAttribute_Should_Return_CRefAttributeValue()
    {
        // Arrange
        var element = new XElement("TestElement");
        element.SetAttributeValue("cref", "TestCRef");

        // Act
        var crefAttribute = element.GetCRefAttribute();

        // Assert
        Assert.Equal("TestCRef", crefAttribute);
    }

    [Fact]
    public void GetCRefAttribute_Should_Return_Null_If_Attribute_Not_Present()
    {
        // Arrange
        var element = new XElement("TestElement");

        // Act
        var crefAttribute = element.GetCRefAttribute();

        // Assert
        Assert.Null(crefAttribute);
    }

    [Fact]
    public void GetElementValue_Should_Return_ElementValue()
    {
        // Arrange
        var element = new XElement("TestElement", "TestValue");

        // Act
        var elementValue = element.GetElementValue();

        // Assert
        Assert.Equal("TestValue", elementValue);
    }

    [Fact]
    public void GetElementValue_Should_Return_Null_If_Element_Not_Present()
    {
        // Arrange
        var element = new XElement("TestElement");

        // Act
        var elementValue = element.GetElementValue("NonExistentElement");

        // Assert
        Assert.Null(elementValue);
    }

    [Fact]
    public void GetHRefAttribute_Should_Return_HRefAttributeValue()
    {
        // Arrange
        var element = new XElement("TestElement");
        element.SetAttributeValue("href", "TestHRef");

        // Act
        var hrefAttribute = element.GetHRefAttribute();

        // Assert
        Assert.Equal("TestHRef", hrefAttribute);
    }

    [Fact]
    public void GetHRefAttribute_Should_Return_Null_If_Attribute_Not_Present()
    {
        // Arrange
        var element = new XElement("TestElement");

        // Act
        var hrefAttribute = element.GetHRefAttribute();

        // Assert
        Assert.Null(hrefAttribute);
    }

    [Fact]
    public void GetLangwordAttribute_Should_Return_LangwordAttributeValue()
    {
        // Arrange
        var element = new XElement("TestElement");
        element.SetAttributeValue("langword", "TestLangword");

        // Act
        var langwordAttribute = element.GetLangwordAttribute();

        // Assert
        Assert.Equal("TestLangword", langwordAttribute);
    }

    [Fact]
    public void GetLangwordAttribute_Should_Return_Null_If_Attribute_Not_Present()
    {
        // Arrange
        var element = new XElement("TestElement");

        // Act
        var langwordAttribute = element.GetLangwordAttribute();

        // Assert
        Assert.Null(langwordAttribute);
    }

    [Fact]
    public void GetNameAttribute_Should_Return_NameAttributeValue()
    {
        // Arrange
        var element = new XElement("TestElement");
        element.SetAttributeValue("name", "TestName");

        // Act
        var nameAttribute = element.GetNameAttribute();

        // Assert
        Assert.Equal("TestName", nameAttribute);
    }

    [Fact]
    public void GetNameAttribute_Should_Return_Null_If_Attribute_Not_Present()
    {
        // Arrange
        var element = new XElement("TestElement");

        // Act
        var nameAttribute = element.GetNameAttribute();

        // Assert
        Assert.Null(nameAttribute);
    }

    [Fact]
    public void HasInheritDoc_Should_Return_True_If_InheritDoc_Element_Present()
    {
        // Arrange
        var element = new XElement("TestElement");
        element.Add(new XElement("inheritdoc"));

        // Act
        var hasInheritDoc = element.HasInheritDoc(out _);

        // Assert
        Assert.True(hasInheritDoc);
    }

    [Fact]
    public void HasInheritDoc_Should_Return_False_If_InheritDoc_Element_Not_Present()
    {
        // Arrange
        var element = new XElement("TestElement");

        // Act
        var hasInheritDoc = element.HasInheritDoc(out _);

        // Assert
        Assert.False(hasInheritDoc);
    }

    [Fact]
    public void Merge_Should_Return_Right_Element_If_Left_Element_Null()
    {
        // Arrange
        XElement? left = null;
        var right = new XElement("TestElement");

        // Act
        var mergedElement = left.Merge(right);

        // Assert
        Assert.Equal(right, mergedElement);
    }

    [Fact]
    public void Merge_Should_Return_Left_Element_If_Right_Element_Null()
    {
        // Arrange
        var left = new XElement("TestElement");
        XElement? right = null;

        // Act
        var mergedElement = left.Merge(right);

        // Assert
        Assert.Equal(left, mergedElement);
    }

    [Fact]
    public void Merge_Should_Merge_Child_Elements_If_Present_In_Both_Left_And_Right_Elements()
    {
        // Arrange
        var left = new XElement("TestElement");
        left.Add(new XElement("ChildElement", "LeftValue"));
        var right = new XElement("TestElement");
        right.Add(new XElement("ChildElement", "RightValue"));

        // Act
        var mergedElement = left.Merge(right);

        // Assert
        Assert.NotNull(mergedElement);
        var mergedChildElement = mergedElement.Element("ChildElement");
        Assert.NotNull(mergedChildElement);
        Assert.Equal("RightValue", mergedChildElement.Value);
    }

    [Fact]
    public void Merge_Should_Not_Merge_Child_Elements_If_Present_In_IgnoredElements()
    {
        // Arrange
        var left = new XElement("TestElement");
        left.Add(new XElement("ChildElement", "LeftValue"));
        var right = new XElement("TestElement");
        right.Add(new XElement("ChildElement", "RightValue"));

        // Act
        var mergedElement = left.Merge(right, "ChildElement");

        // Assert
        Assert.NotNull(mergedElement);
        var mergedChildElement = mergedElement.Element("ChildElement");
        Assert.NotNull(mergedChildElement);
        Assert.Equal("LeftValue", mergedChildElement.Value);
    }

    [Fact]
    public void Sort_Should_Return_Null_If_Element_Null()
    {
        // Arrange
        XElement? element = null;

        // Act
        var sortedElement = element.Sort();

        // Assert
        Assert.Null(sortedElement);
    }

    [Fact]
    public void Sort_Should_Return_New_Element_With_Sorted_Child_Elements()
    {
        // Arrange
        var element = new XElement("TestElement");
        element.Add(new XElement("C"));
        element.Add(new XElement("B"));
        element.Add(new XElement("A"));

        // Act
        var sortedElement = element.Sort();

        // Assert
        Assert.NotNull(sortedElement);
        var sortedChildElements = sortedElement.Elements();
        Assert.Collection(
            sortedChildElements,
            e => Assert.Equal("A", e.Name.LocalName),
            e => Assert.Equal("B", e.Name.LocalName),
            e => Assert.Equal("C", e.Name.LocalName)
        );
    }

    [Fact]
    public void Sort_Should_Return_New_Element_With_Sorted_Attributes()
    {
        // Arrange
        var element = new XElement("TestElement");
        element.SetAttributeValue("C", "ValueC");
        element.SetAttributeValue("B", "ValueB");
        element.SetAttributeValue("A", "ValueA");

        // Act
        var sortedElement = element.Sort();

        // Assert
        Assert.NotNull(sortedElement);
        var sortedAttributes = sortedElement.Attributes();
        Assert.Collection(
            sortedAttributes,
            a => Assert.Equal("A", a.Name.LocalName),
            a => Assert.Equal("B", a.Name.LocalName),
            a => Assert.Equal("C", a.Name.LocalName)
        );
    }
}
