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

    /// <summary>
    /// The numeric value of the enum member.
    /// </summary>
    public string? Value { get; }

    /// <inheritdoc />
    internal ModelEnumMember(IField member, ModelTypeBase parent, XElement? doc)
        : base(member, parent, doc) => Value = member.GetConstantValue()!.ToString();
}
