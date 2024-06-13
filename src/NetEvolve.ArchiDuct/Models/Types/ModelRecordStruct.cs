namespace NetEvolve.ArchiDuct.Models.Types;

using System.Xml.Linq;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Models.Abstractions;

/// <summary>
/// Describes a record struct implementation.
/// </summary>
public sealed class ModelRecordStruct : ModelTypeBase
{
    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.RecordStruct;

    /// <inheritdoc />
    internal ModelRecordStruct(
        ITypeDefinition typeDefinition,
        ModelBase parentEntity,
        XElement? documentation
    )
        : base(typeDefinition, parentEntity, documentation) { }
}
