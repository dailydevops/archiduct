namespace NetEvolve.ArchiDuct.Models.Members;

using System.Xml.Linq;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Models.Abstractions;

/// <summary>
/// Describes an property implementation.
/// </summary>
public class ModelProperty : ModelMemberBase
{
    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.Property;

    /// <summary>
    /// Gets the content from the xml &lt;value/&gt; tag.
    /// </summary>
    public string? Value { get; set; }

    /// <inheritdoc />
    internal ModelProperty(IProperty property, ModelTypeBase parentEntity, XElement? documentation)
        : base(property, parentEntity, documentation) { }
}
