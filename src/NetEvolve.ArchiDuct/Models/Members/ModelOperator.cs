namespace NetEvolve.ArchiDuct.Models.Members;

using System.Xml.Linq;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Models.Abstractions;

/// <summary>
/// Describes an operation implementation.
/// </summary>
public sealed class ModelOperator : ModelMemberAdvancedBase
{
    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.Operator;

    /// <inheritdoc />
    internal ModelOperator(IMethod @operator, ModelTypeBase parentEntity, XElement? documentation)
        : base(@operator, parentEntity, documentation) { }
}
