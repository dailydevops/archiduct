namespace NetEvolve.ArchiDuct.Models.Members;

using System.Xml.Linq;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Models.Abstractions;

/// <summary>
/// Describes a const implementation.
/// </summary>
public sealed class ModelConstant : ModelMemberBase
{
    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.Constant;

    /// <inheritdoc />
    internal ModelConstant(IField field, ModelTypeBase parentEntity, XElement? documentation)
        : base(field, parentEntity, documentation) { }
}
