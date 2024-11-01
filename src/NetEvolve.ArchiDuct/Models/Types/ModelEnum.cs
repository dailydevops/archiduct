namespace NetEvolve.ArchiDuct.Models.Types;

using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;
using ICSharpCode.Decompiler.Documentation;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Models.Abstractions;

/// <summary>
/// Describes an enum implementation.
/// </summary>
[SuppressMessage(
    "Naming",
    "CA1711:Identifiers should not have incorrect suffix",
    Justification = "As designed."
)]
public sealed class ModelEnum : ModelTypeBase
{
    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.Enum;

    /// <summary>
    /// The underlying type of the enum.
    /// </summary>
    public string? UnderlyingType { get; }

    /// <inheritdoc />
    internal ModelEnum(ITypeDefinition typeDefinition, ModelBase parent, XElement? doc)
        : base(typeDefinition, parent, doc) =>
        UnderlyingType = typeDefinition.EnumUnderlyingType!.GetDefinition().GetIdString();
}
