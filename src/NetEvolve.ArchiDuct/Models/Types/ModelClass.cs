namespace NetEvolve.ArchiDuct.Models.Types;

using System.Xml.Linq;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Models.Abstractions;

/// <summary>
/// Describes a class implementation.
/// </summary>
public sealed class ModelClass : ModelTypeBase
{
    /// <summary>
    /// Gets a value indicating whether the class is a <see langword="record"/>.
    /// </summary>
    public bool IsRecord { get; }

    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.Class;

    /// <inheritdoc />
    internal ModelClass(
        ITypeDefinition typeDefinition,
        ModelBase parentEntity,
        XElement? documentation
    )
        : base(typeDefinition, parentEntity, documentation) => IsRecord = typeDefinition.IsRecord;
}
