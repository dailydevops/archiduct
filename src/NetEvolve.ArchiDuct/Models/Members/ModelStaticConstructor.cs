namespace NetEvolve.ArchiDuct.Models.Members;

using System.Xml.Linq;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Models.Abstractions;

/// <summary>
/// Describes a static constructor implementation.
/// </summary>
public sealed class ModelStaticConstructor : ModelMemberBase
{
    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.StaticConstructor;

    /// <inheritdoc />
    internal ModelStaticConstructor(
        IMethod staticConstructor,
        ModelTypeBase parentEntity,
        XElement? documentation
    )
        : base(staticConstructor, parentEntity, documentation) { }
}
