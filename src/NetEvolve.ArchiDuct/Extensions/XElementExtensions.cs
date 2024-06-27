namespace NetEvolve.ArchiDuct.Extensions;

using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Xml.Linq;
using static NetEvolve.ArchiDuct.Models.DocumentationXmlAttributeConstants;
using static NetEvolve.ArchiDuct.Models.DocumentationXmlPropertyConstants;

internal static class XElementExtensions
{
    private static readonly Func<XNode?, string> _defaultConvert = node =>
        node switch
        {
            null => string.Empty,
            XElement element when element.Name.LocalName.Equals(Para, OrdinalIgnoreCase)
                => element.Value,
            XCData cData => cData.Value,
            XText text => text.Value,
            _ => node.ToString(),
        };

    public static string? GetCRefAttribute(this XElement? element) =>
        element?.Attribute(CRef)?.Value;

    public static string? GetElementValue(
        this XElement? element,
        string? elementName = default,
        Func<XNode?, string>? convertElement = default
    )
    {
        var nodes =
            elementName is null || string.IsNullOrWhiteSpace(elementName)
                ? element?.Nodes()
                : element?.Element(elementName.Trim())?.Nodes();

        var sb = new StringBuilder();

        if (convertElement is null)
        {
            convertElement = _defaultConvert;
        }

        if (nodes is not null)
        {
            foreach (var node in nodes)
            {
                _ = sb.Append(convertElement.Invoke(node));
            }
        }

        var result = sb.ToString().Trim();
        return string.IsNullOrEmpty(result) ? null : result;
    }

    public static string? GetHRefAttribute(this XElement? element) =>
        element?.Attribute(HRef)?.Value;

    public static string? GetLangwordAttribute(this XElement? element) =>
        element?.Attribute(Langword)?.Value;

    public static string? GetNameAttribute(this XElement? element) =>
        element?.Attribute(Name)?.Value;

    public static bool HasInheritDoc(
        this XElement? element,
        [NotNullWhen(true)] out XElement? inheritDoc
    )
    {
        inheritDoc = element?.Element(InheritDoc);
        return inheritDoc is not null;
    }

    public static XElement? Merge(
        this XElement? left,
        XElement? right,
        params string[] ignoredElements
    )
    {
        if (left is null)
        {
            return right;
        }

        if (right is null)
        {
            return left;
        }

        var rightElements = right.Elements().ToArray();
        foreach (var rightElement in rightElements)
        {
            var leftElements = left.Elements().ToArray();
            var leftElement = Array.Find(
                leftElements,
                x => XElementEqualityComparer.Instance.Equals(x, rightElement)
            );

            if (leftElement is null)
            {
                left.Add(rightElement);
            }
            else if (rightElement.HasElements)
            {
                left = leftElement.Merge(rightElement)!;
            }
            else if (
                ignoredElements is null
                || ignoredElements.Length == 0
                || !Array.Exists(
                    ignoredElements,
                    x =>
                        rightElement?.Name is not null
                        && rightElement.Name.ToString().Equals(x, OrdinalIgnoreCase)
                )
            )
            {
                leftElement.Value = rightElement.Value;
            }
        }

        return left;
    }

    public static XElement? Sort(this XElement? element)
    {
        if (element is null)
        {
            return null;
        }

        var newElement = new XElement(element.Name);

        if (element.Parent is not null)
        {
            newElement.SetValue(element.Value);
        }

        if (element.HasElements)
        {
            foreach (
                var child in element
                    .Elements()
                    .OrderBy(x => x.Name.LocalName, StringComparer.Ordinal)
                    .ThenBy(
                        x =>
                            x.Attributes()
                                .Select(y => y.ToString())
                                .OrderBy(y => y, StringComparer.Ordinal)
                                .FirstOrDefault(),
                        StringComparer.Ordinal
                    )
            )
            {
                newElement.Add(child.Sort());
            }
        }

        if (element.HasAttributes)
        {
            foreach (
                var attrib in element
                    .Attributes()
                    .OrderBy(y => y.ToString(), StringComparer.Ordinal)
            )
            {
                newElement.SetAttributeValue(attrib.Name, attrib.Value);
            }
        }
        return newElement;
    }
}
