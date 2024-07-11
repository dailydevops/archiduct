namespace NetEvolve.ArchiDuct.Models;

using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;
using ICSharpCode.Decompiler.Documentation;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Models.Abstractions;

/// <summary>
/// Represents an attribute.
/// </summary>
[SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "As designed."
)]
public sealed class ModelAttribute : ModelBase
{
    /// <inheritdoc/>
    public override ModelKind Kind => ModelKind.Attribute;

    internal ModelAttribute(
        IAttribute attribute,
        ITypeDefinition typeDefinition,
        XElement? documentation
    )
        : base(
            typeDefinition.GetIdString(),
            typeDefinition.Name,
            typeDefinition.FullName,
            documentation
        )
    {
        // TODO: Map constructor with values
    }
}
