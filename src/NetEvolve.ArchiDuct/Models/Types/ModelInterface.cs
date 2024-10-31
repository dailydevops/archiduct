namespace NetEvolve.ArchiDuct.Models.Types;

using System.Xml.Linq;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Models.Abstractions;

/// <summary>
/// Describes an interface implementation.
/// </summary>
public sealed class ModelInterface : ModelTypeBase
{
    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.Interface;

    /// <inheritdoc />
    internal ModelInterface(
        ITypeDefinition typeDefinition,
        ModelBase parentEntity,
        XElement? documentation
    )
        : base(typeDefinition, parentEntity, documentation) { }
}
