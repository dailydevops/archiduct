namespace NetEvolve.ArchiDuct.Models.Members;

using System.Xml.Linq;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Models.Abstractions;

/// <summary>
/// Describes a destructor implementation.
/// </summary>
public sealed class ModelDestructor : ModelMemberBase
{
    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.Destructor;

    /// <inheritdoc />
    internal ModelDestructor(
        IMethod destructor,
        ModelTypeBase parentEntity,
        XElement? documentation
    )
        : base(destructor, parentEntity, documentation) { }
}
