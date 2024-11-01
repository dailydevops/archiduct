namespace NetEvolve.ArchiDuct.Models;

using System.Xml.Linq;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Models.Abstractions;

/// <summary>
/// Describes a method implementation.
/// </summary>
public sealed class ModelDestructor : ModelMemberBase
{
    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.Destructor;

    /// <inheritdoc />
    internal ModelDestructor(IMethod method, ModelTypeBase parent, XElement? doc)
        : base(method, parent, doc) { }
}
