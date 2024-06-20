namespace NetEvolve.ArchiDuct.Models.Documentation;

using System.Xml.Linq;
using NetEvolve.ArchiDuct.Extensions;
using NetEvolve.ArchiDuct.Models;
using static NetEvolve.ArchiDuct.Models.DocumentationXmlAttributeConstants;
using static NetEvolve.ArchiDuct.Models.DocumentationXmlPropertyConstants;

/// <summary>
/// Represents the xml documentation for a model.
/// </summary>
public sealed class ModelDocumentation
{
    private readonly XElement _documentation;

    private readonly string? _remarks;
    private readonly string? _returns;
    private readonly string? _summary;

    /// <summary>
    /// Gets the content from the xml &lt;remarks/&gt; tag.
    /// </summary>
    public string? Remarks => _remarks;

    /// <summary>
    /// Gets the content from the xml &lt;returns/&gt; tag.
    /// </summary>
    public string? Returns => _returns;

    /// <summary>
    /// Gets the content from the xml &lt;summary/&gt; tag.
    /// </summary>
    public string? Summary => _summary;

    private ModelDocumentation(XElement documentation)
        : this(
            documentation,
            GetElementValue(documentation, DocumentationXmlPropertyConstants.Summary),
            GetElementValue(documentation, DocumentationXmlPropertyConstants.Remarks),
            GetElementValue(documentation, DocumentationXmlPropertyConstants.Returns)
        ) { }

    private ModelDocumentation(XElement documentation, string? summary)
        : this(documentation, summary, null, null) { }

    private ModelDocumentation(
        XElement documentation,
        string? summary,
        string? remarks,
        string? returns
    )
    {
        _documentation = documentation;
        _summary = summary;
        _remarks = remarks;
        _returns = returns;
    }

    internal static ModelDocumentation? Default(XElement? documentation) =>
        documentation is not null ? new ModelDocumentation(documentation) : null;

    internal static ModelDocumentation? LoadParameter(
        ModelDocumentation? parentDocumentation,
        string name
    )
    {
        if (parentDocumentation is null)
        {
            return null;
        }

        var documentation = new ModelDocumentation(
            parentDocumentation._documentation,
            GetElements(parentDocumentation._documentation, Param)
                ?.FirstOrDefault(p => string.Equals(p.Attribute(Name)?.Value, name, Ordinal))
                .GetElementValue()
        );

        return documentation;
    }

    internal static ModelDocumentation? LoadTypeParameter(
        ModelDocumentation? parentDocumentation,
        string name
    )
    {
        if (parentDocumentation is null)
        {
            return null;
        }

        var documentation = new ModelDocumentation(
            parentDocumentation._documentation,
            GetElements(parentDocumentation._documentation, TypeParam)
                ?.FirstOrDefault(p => string.Equals(p.Attribute(Name)?.Value, name, Ordinal))
                .GetElementValue()
        );

        return documentation;
    }

    ///// <summary>
    ///// Determines the parentDocumentation element for the parameter <paramref name="elementName"/>.
    ///// </summary>
    ///// <param name="documentation">The parentDocumentation xml to search for the <paramref name="elementName"/>. Can be null.</param>
    ///// <param name="elementName">Property name within the parentDocumentation xml.</param>
    ///// <returns>Returns the full parentDocumentation for <paramref name="elementName"/> as xml.</returns>
    //private static XElement? GetElement(XElement? documentation, string? elementName) =>
    //    string.IsNullOrWhiteSpace(elementName)
    //        ? documentation
    //        : documentation?.Element(elementName.Trim());

    /// <summary>
    /// Determines the parentDocumentation elements for the parameter <paramref name="elementName"/>.
    /// </summary>
    /// <param name="documentation">The parentDocumentation xml to search for the <paramref name="elementName"/>. Can be null.</param>
    /// <param name="elementName">Property name within the parentDocumentation xml.</param>
    /// <returns>Returns the full parentDocumentation for <paramref name="elementName"/> as xml.</returns>
    private static IEnumerable<XElement>? GetElements(
        XElement? documentation,
        string? elementName
    ) =>
        string.IsNullOrWhiteSpace(elementName)
            ? documentation?.Elements()
            : documentation?.Elements(elementName.Trim());

    /// <summary>
    /// Determines the parentDocumentation content for the parameter <paramref name="elementName"/>.
    /// </summary>
    /// <param name="documentation">The parentDocumentation xml to search for the <paramref name="elementName"/>. Can be null.</param>
    /// <param name="elementName">Property name within the parentDocumentation xml.</param>
    /// <param name="convertElement">Possibility to format child elements.</param>
    /// <returns>Returns the parentDocumentation for <paramref name="elementName"/>.</returns>
    private static string? GetElementValue(
        XElement? documentation,
        string? elementName = null,
        Func<XNode?, string>? convertElement = null
    ) => documentation?.GetElementValue(elementName, convertElement);
}
