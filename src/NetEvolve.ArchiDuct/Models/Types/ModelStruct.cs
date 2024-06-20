namespace NetEvolve.ArchiDuct.Models.Types;

using System.Xml.Linq;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Models.Abstractions;

/// <summary>
/// Describes a struct implementation.
/// </summary>
public sealed class ModelStruct : ModelTypeBase
{
    /// <summary>
    /// Gets a value indicating whether the struct is a <see langword="record"/>.
    /// </summary>
    public bool IsRecord { get; }

    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.Struct;

    /// <inheritdoc />
    internal ModelStruct(
        ITypeDefinition typeDefinition,
        ModelBase parentEntity,
        XElement? documentation
    )
        : base(typeDefinition, parentEntity, documentation) => IsRecord = typeDefinition.IsRecord;
}
