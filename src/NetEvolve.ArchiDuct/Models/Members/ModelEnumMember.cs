namespace NetEvolve.ArchiDuct.Models.Members;

using System.Xml.Linq;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Models.Abstractions;

/// <summary>
/// Describes an enum member implementation.
/// </summary>
public sealed class ModelEnumMember : ModelMemberBase
{
    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.EnumMember;

    /// <inheritdoc />
    internal ModelEnumMember(IField enumMember, ModelTypeBase parentEntity, XElement? documentation)
        : base(enumMember, parentEntity, documentation) { }
}
