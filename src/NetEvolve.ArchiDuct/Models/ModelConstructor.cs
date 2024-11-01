namespace NetEvolve.ArchiDuct.Models;

using System.Xml.Linq;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Models.Abstractions;

/// <summary>
/// Describes a constructor implementation.
/// </summary>
public sealed class ModelConstructor : ModelMemberAdvancedBase
{
    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.Constructor;

    /// <inheritdoc />
    internal ModelConstructor(IMethod method, ModelTypeBase parent, XElement? doc)
        : base(method, parent, doc) { }
}
