namespace NetEvolve.ArchiDuct.Tests.Unit.Extensions;

using System;
using System.Xml.Linq;
using NetEvolve.ArchiDuct.Extensions;

public class XElementExtensionsTests
{
    [Test]
    public async Task GetCRefAttribute_Should_Return_CRefAttributeValue()
    {
        // Arrange
        var element = new XElement("TestElement");
        element.SetAttributeValue("cref", "TestCRef");

        // Act
        var crefAttribute = element.GetCRefAttribute();

        // Assert
        _ = await Assert.That(crefAttribute).IsEqualTo("TestCRef");
    }

    [Test]
    public async Task GetCRefAttribute_Should_Return_Null_If_Attribute_Not_Present()
    {
        // Arrange
        var element = new XElement("TestElement");

        // Act
        var crefAttribute = element.GetCRefAttribute();

        // Assert
        _ = await Assert.That(crefAttribute).IsNull();
    }

    [Test]
    public async Task GetElementValue_Should_Return_ElementValue()
    {
        // Arrange
        var element = new XElement("TestElement", "TestValue");

        // Act
        var elementValue = element.GetElementValue();

        // Assert
        _ = await Assert.That(elementValue).IsEqualTo("TestValue");
    }

    [Test]
    public async Task GetElementValue_Should_Return_Null_If_Element_Not_Present()
    {
        // Arrange
        var element = new XElement("TestElement");

        // Act
        var elementValue = element.GetElementValue("NonExistentElement");

        // Assert
        _ = await Assert.That(elementValue).IsNull();
    }

    [Test]
    public async Task GetHRefAttribute_Should_Return_HRefAttributeValue()
    {
        // Arrange
        var element = new XElement("TestElement");
        element.SetAttributeValue("href", "TestHRef");

        // Act
        var hrefAttribute = element.GetHRefAttribute();

        // Assert
        _ = await Assert.That(hrefAttribute).IsEqualTo("TestHRef");
    }

    [Test]
    public async Task GetHRefAttribute_Should_Return_Null_If_Attribute_Not_Present()
    {
        // Arrange
        var element = new XElement("TestElement");

        // Act
        var hrefAttribute = element.GetHRefAttribute();

        // Assert
        _ = await Assert.That(hrefAttribute).IsNull();
    }

    [Test]
    public async Task GetLangwordAttribute_Should_Return_LangwordAttributeValue()
    {
        // Arrange
        var element = new XElement("TestElement");
        element.SetAttributeValue("langword", "TestLangword");

        // Act
        var langwordAttribute = element.GetLangwordAttribute();

        // Assert
        _ = await Assert.That(langwordAttribute).IsEqualTo("TestLangword");
    }

    [Test]
    public async Task GetLangwordAttribute_Should_Return_Null_If_Attribute_Not_Present()
    {
        // Arrange
        var element = new XElement("TestElement");

        // Act
        var langwordAttribute = element.GetLangwordAttribute();

        // Assert
        _ = await Assert.That(langwordAttribute).IsNull();
    }

    [Test]
    public async Task GetNameAttribute_Should_Return_NameAttributeValue()
    {
        // Arrange
        var element = new XElement("TestElement");
        element.SetAttributeValue("name", "TestName");

        // Act
        var nameAttribute = element.GetNameAttribute();

        // Assert
        _ = await Assert.That(nameAttribute).IsEqualTo("TestName");
    }

    [Test]
    public async Task GetNameAttribute_Should_Return_Null_If_Attribute_Not_Present()
    {
        // Arrange
        var element = new XElement("TestElement");

        // Act
        var nameAttribute = element.GetNameAttribute();

        // Assert
        _ = await Assert.That(nameAttribute).IsNull();
    }

    [Test]
    public async Task HasInheritDoc_Should_Return_True_If_InheritDoc_Element_Present()
    {
        // Arrange
        var element = new XElement("TestElement");
        element.Add(new XElement("inheritdoc"));

        // Act
        var hasInheritDoc = element.HasInheritDoc(out _);

        // Assert
        _ = await Assert.That(hasInheritDoc).IsTrue();
    }

    [Test]
    public async Task HasInheritDoc_Should_Return_False_If_InheritDoc_Element_Not_Present()
    {
        // Arrange
        var element = new XElement("TestElement");

        // Act
        var hasInheritDoc = element.HasInheritDoc(out _);

        // Assert
        _ = await Assert.That(hasInheritDoc).IsFalse();
    }

    [Test]
    public async Task Merge_Should_Return_Right_Element_If_Left_Element_Null()
    {
        // Arrange
        XElement? left = null;
        var right = new XElement("TestElement");

        // Act
        var mergedElement = left.Merge(right);

        // Assert
        _ = await Assert.That(mergedElement).IsEqualTo(right);
    }

    [Test]
    public async Task Merge_Should_Return_Left_Element_If_Right_Element_Null()
    {
        // Arrange
        var left = new XElement("TestElement");
        XElement? right = null;

        // Act
        var mergedElement = left.Merge(right);

        // Assert
        _ = await Assert.That(mergedElement).IsEqualTo(left);
    }

    [Test]
    public async Task Merge_Should_Merge_Child_Elements_If_Present_In_Both_Left_And_Right_Elements()
    {
        // Arrange
        var left = new XElement("TestElement");
        left.Add(new XElement("ChildElement", "LeftValue"));
        var right = new XElement("TestElement");
        right.Add(new XElement("ChildElement", "RightValue"));

        // Act
        var mergedElement = left.Merge(right);

        // Assert
        mergedElement = await Assert.That(mergedElement).IsNotNull();
        var mergedChildElement = mergedElement.Element("ChildElement");
        _ = await Assert.That(mergedChildElement).IsNotNull();
        _ = await Assert.That(mergedChildElement!.Value).IsEqualTo("RightValue");
    }

    [Test]
    public async Task Merge_Should_Not_Merge_Child_Elements_If_Present_In_IgnoredElements()
    {
        // Arrange
        var left = new XElement("TestElement");
        left.Add(new XElement("ChildElement", "LeftValue"));
        var right = new XElement("TestElement");
        right.Add(new XElement("ChildElement", "RightValue"));

        // Act
        var mergedElement = left.Merge(right, "ChildElement");

        // Assert
        mergedElement = await Assert.That(mergedElement).IsNotNull();
        var mergedChildElement = mergedElement.Element("ChildElement");
        _ = await Assert.That(mergedChildElement).IsNotNull();
        _ = await Assert.That(mergedChildElement!.Value).IsEqualTo("LeftValue");
    }

    [Test]
    public async Task Sort_Should_Return_Null_If_Element_Null()
    {
        // Arrange
        XElement? element = null;

        // Act
        var sortedElement = element.Sort();

        // Assert
        _ = await Assert.That(sortedElement).IsNull();
    }

    [Test]
    public async Task Sort_Should_Return_New_Element_With_Sorted_Child_Elements()
    {
        // Arrange
        var element = new XElement("TestElement");
        element.Add(new XElement("C"));
        element.Add(new XElement("B"));
        element.Add(new XElement("A"));

        // Act
        var sortedElement = element.Sort();

        // Assert
        _ = await Assert.That(sortedElement).IsNotNull();
        var sortedChildElements = sortedElement!.Elements();
        _ = await Assert.That(sortedChildElements.Select(e => e.Name.LocalName)).IsEquivalentTo(["A", "B", "C"]);
    }

    [Test]
    public async Task Sort_Should_Return_New_Element_With_Sorted_Attributes()
    {
        // Arrange
        var element = new XElement("TestElement");
        element.SetAttributeValue("C", "ValueC");
        element.SetAttributeValue("B", "ValueB");
        element.SetAttributeValue("A", "ValueA");

        // Act
        var sortedElement = element.Sort();

        // Assert
        _ = await Assert.That(sortedElement).IsNotNull();
        var sortedAttributes = sortedElement!.Attributes();
        _ = await Assert.That(sortedAttributes.Select(a => a.Name.LocalName)).IsEquivalentTo(["A", "B", "C"]);
    }
}
