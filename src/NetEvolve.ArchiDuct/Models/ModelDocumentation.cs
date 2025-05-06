namespace NetEvolve.ArchiDuct.Models;

using System.Xml.Linq;
using NetEvolve.ArchiDuct.Extensions;

/// <summary>
/// Represents the xml doc for a model.
/// </summary>
public sealed class ModelDocumentation
{
    private readonly XElement _documentation;

    /// <summary>
    /// Gets the content from the xml &lt;remarks/&gt; tag.
    /// </summary>
    public string? Remarks { get; }

    /// <summary>
    /// Gets the content from the xml &lt;returns/&gt; tag.
    /// </summary>
    public string? Returns { get; }

    /// <summary>
    /// Gets the content from the xml &lt;summary/&gt; tag.
    /// </summary>
    public string? Summary { get; }

    private ModelDocumentation(XElement doc)
        : this(doc, null, null, null) { }

    private ModelDocumentation(XElement doc, string? summary)
        : this(doc, summary, null, null) { }

    private ModelDocumentation(XElement doc, string? summary, string? remarks, string? returns)
    {
        _documentation = doc;
        Summary = summary ?? GetElementValue(doc, DocumentationXmlPropertyConstants.Summary);
        Remarks = remarks ?? GetElementValue(doc, DocumentationXmlPropertyConstants.Remarks);
        Returns = returns ?? GetElementValue(doc, DocumentationXmlPropertyConstants.Returns);
    }

    internal static ModelDocumentation? Default(XElement? doc) => doc is not null ? new ModelDocumentation(doc) : null;

    internal static ModelDocumentation? LoadParameter(ModelDocumentation? parent, string name)
    {
        if (parent is null)
        {
            return null;
        }

        var doc = new ModelDocumentation(
            parent._documentation,
            GetElements(parent._documentation, DocumentationXmlPropertyConstants.Param)
                ?.FirstOrDefault(p =>
                    string.Equals(p.Attribute(DocumentationXmlAttributeConstants.Name)?.Value, name, Ordinal)
                )
                .GetElementValue()
        );

        return doc;
    }

    internal static ModelDocumentation? LoadTypeParameter(ModelDocumentation? parent, string name)
    {
        if (parent is null)
        {
            return null;
        }

        var doc = new ModelDocumentation(
            parent._documentation,
            GetElements(parent._documentation, DocumentationXmlPropertyConstants.TypeParam)
                ?.FirstOrDefault(p =>
                    string.Equals(p.Attribute(DocumentationXmlAttributeConstants.Name)?.Value, name, Ordinal)
                )
                .GetElementValue()
        );

        return doc;
    }

    /// <summary>
    /// Determines the parent elements for the parameter <paramref name="elementName"/>.
    /// </summary>
    /// <param name="doc">The parent xml to search for the <paramref name="elementName"/>. Can be null.</param>
    /// <param name="elementName">Property name within the parent xml.</param>
    /// <returns>Returns the full parent for <paramref name="elementName"/> as xml.</returns>
    private static IEnumerable<XElement>? GetElements(XElement? doc, string? elementName) =>
        string.IsNullOrWhiteSpace(elementName) ? doc?.Elements() : doc?.Elements(elementName.Trim());

    /// <summary>
    /// Determines the parent content for the parameter <paramref name="elementName"/>.
    /// </summary>
    /// <param name="doc">The parent xml to search for the <paramref name="elementName"/>. Can be null.</param>
    /// <param name="elementName">Property name within the parent xml.</param>
    /// <param name="convertElement">Possibility to format child elements.</param>
    /// <returns>Returns the parent for <paramref name="elementName"/>.</returns>
    private static string? GetElementValue(
        XElement? doc,
        string? elementName = null,
        Func<XNode?, string>? convertElement = null
    ) => string.IsNullOrWhiteSpace(elementName) ? null : doc?.GetElementValue(elementName.Trim(), convertElement);
}
