namespace NetEvolve.ArchiDuct.Models.Members;

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
    internal ModelConstructor(IMethod method, ModelTypeBase parentEntity, XElement? documentation)
        : base(method, parentEntity, documentation) { }
}
