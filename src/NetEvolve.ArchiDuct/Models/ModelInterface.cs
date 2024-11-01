namespace NetEvolve.ArchiDuct.Models;

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
    internal ModelInterface(ITypeDefinition typeDefinition, ModelBase parent, XElement? doc)
        : base(typeDefinition, parent, doc) { }
}
