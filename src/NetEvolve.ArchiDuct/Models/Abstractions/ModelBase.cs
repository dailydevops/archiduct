namespace NetEvolve.ArchiDuct.Models.Abstractions;

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using NetEvolve.ArchiDuct.Extensions;

/// <summary>
/// Represents a type or member description.
/// </summary>
public abstract class ModelBase
{
    internal readonly XElement? _documentation;

    /// <summary>
    /// Gets the fully qualified name of the class the return type is pointing to.
    /// </summary>
    /// <returns>
    /// "System.Int32[]" for int[]<br/>
    /// "System.Collections.Generic.List" for List&lt;string&gt;<br/>
    /// "System.Environment.SpecialFolder" for Environment.SpecialFolder
    /// </returns>
    public string FullName { get; }

    /// <summary>
    /// Unique ID string that identifies the type or member.
    /// </summary>
    /// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/#id-strings"/>
    public string Id { get; }

    /// <summary>
    /// Defines the described object type.
    /// </summary>
    public abstract ModelKind Kind { get; }

    /// <summary>
    /// Gets the short name of the class the return type is pointing to.
    /// </summary>
    /// <returns>
    /// "Int32[]" for int[]<br/>
    /// "List" for List&lt;string&gt;
    /// "SpecialFolder" for Environment.SpecialFolder
    /// </returns>
    public string Name { get; }

    /// <summary>
    /// Gets the content from the xml &lt;remarks/&gt; tag.
    /// </summary>
    public virtual string? Remarks => GetElementValue(DocumentationXmlPropertyConstants.Remarks);

    /// <summary>
    /// Gets the content from the xml &lt;returns/&gt; tag.
    /// </summary>
    public virtual string? Returns => GetElementValue(DocumentationXmlPropertyConstants.Returns);

    /// <summary>
    /// Gets the content from the xml &lt;summary/&gt; tag.
    /// </summary>
    public virtual string? Summary => GetElementValue(DocumentationXmlPropertyConstants.Summary);

    private protected ModelBase(string id, string name, string fullName, XElement? documentation)
    {
        Id = id;
        Name = name;
        FullName = fullName;
        _documentation = documentation;
    }

    /// <summary>
    /// Determines the documentation element for the parameter <paramref name="elementName"/>.
    /// </summary>
    /// <param name="elementName">Property name within the documentation xml.</param>
    /// <returns>Returns the full documentation for <paramref name="elementName"/> as xml.</returns>
    protected internal XElement? GetElement(string? elementName) =>
        string.IsNullOrWhiteSpace(elementName)
            ? _documentation
            : _documentation?.Element(elementName.Trim());

    /// <summary>
    /// Determines the documentation elements for the parameter <paramref name="elementName"/>.
    /// </summary>
    /// <param name="elementName">Property name within the documentation xml.</param>
    /// <returns>Returns the full documentation for <paramref name="elementName"/> as xml.</returns>
    protected internal IEnumerable<XElement>? GetElements(string? elementName) =>
        string.IsNullOrWhiteSpace(elementName)
            ? _documentation?.Elements()
            : _documentation?.Elements(elementName.Trim());

    /// <summary>
    /// Determines the documentation content for the parameter <paramref name="elementName"/>.
    /// </summary>
    /// <param name="elementName">Property name within the documentation xml.</param>
    /// <param name="convertElement">Possibility to format child elements.</param>
    /// <returns>Returns the documentation for <paramref name="elementName"/>.</returns>
    protected internal string? GetElementValue(
        string? elementName = null,
        Func<XNode?, string>? convertElement = null
    ) => _documentation.GetElementValue(elementName, convertElement);
}
