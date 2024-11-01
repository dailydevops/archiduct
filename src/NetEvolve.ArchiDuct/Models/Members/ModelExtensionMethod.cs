namespace NetEvolve.ArchiDuct.Models.Members;

using System.Xml.Linq;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Models.Abstractions;

/// <summary>
/// Describes an extension method implementation.
/// </summary>
public sealed class ModelExtensionMethod : ModelMethod
{
    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.ExtensionMethod;

    /// <inheritdoc />
    internal ModelExtensionMethod(IMethod method, ModelTypeBase parent, XElement? doc)
        : base(method, parent, doc) { }
}
