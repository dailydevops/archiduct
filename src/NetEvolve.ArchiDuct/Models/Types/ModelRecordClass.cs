namespace NetEvolve.ArchiDuct.Models.Types;

using System.Xml.Linq;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Models.Abstractions;

/// <summary>
/// Describes a record class implementation.
/// </summary>
public sealed class ModelRecordClass : ModelTypeBase
{
    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.RecordClass;

    /// <inheritdoc />
    internal ModelRecordClass(
        ITypeDefinition typeDefinition,
        ModelBase parentEntity,
        XElement? documentation
    )
        : base(typeDefinition, parentEntity, documentation) { }
}
