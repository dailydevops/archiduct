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

#pragma warning disable IDE0060, RCS1163 // Remove unused parameter
    internal ModelAttribute(IAttribute attribute, ITypeDefinition typeDefinition, XElement? doc)
#pragma warning restore IDE0060, RCS1163 // Remove unused parameter
        : base(typeDefinition.GetIdString(), typeDefinition.Name, typeDefinition.FullName, doc) { }
}
