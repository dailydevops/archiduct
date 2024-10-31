namespace NetEvolve.ArchiDuct.Models.Members;

using System.Xml.Linq;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Models.Abstractions;

/// <summary>
/// Describes an explicit property implementation.
/// </summary>
public sealed class ModelExplicitProperty : ModelProperty
{
    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.ExplicitProperty;

    /// <inheritdoc />
    internal ModelExplicitProperty(
        IProperty explicitProperty,
        ModelTypeBase parentEntity,
        XElement? documentation
    )
        : base(explicitProperty, parentEntity, documentation) { }
}
