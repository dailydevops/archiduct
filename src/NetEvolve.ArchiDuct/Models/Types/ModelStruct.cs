namespace NetEvolve.ArchiDuct.Models.Types;

using System.Xml.Linq;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Models.Abstractions;

/// <summary>
/// Describes a struct implementation.
/// </summary>
public sealed class ModelStruct : ModelTypeBase
{
    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.Struct;

    /// <inheritdoc />
    internal ModelStruct(
        ITypeDefinition typeDefinition,
        ModelBase parentEntity,
        XElement? documentation
    )
        : base(typeDefinition, parentEntity, documentation) { }
}
