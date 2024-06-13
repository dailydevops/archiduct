namespace NetEvolve.ArchiDuct.Models.Abstractions;

using System.Collections.Generic;
using System.Xml.Linq;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Models.Members;

/// <inheritdoc />
public abstract class ModelMemberAdvancedBase : ModelMemberBase
{
    /// <summary>
    /// Collection of all member IDs that overload this member.
    /// </summary>
    /// <value>Read-only collection of all overload members.</value>
    public IEnumerable<string>? OverloadedMembers { get; internal set; } = default!;

    /// <summary>
    /// Collection of all member parameters for this member.
    /// </summary>
    /// <value>Read-only collection of all described parameters..</value>
    public IEnumerable<ModelParameter>? Parameters { get; internal set; } = default!;

    /// <summary>
    /// Collection of all type parameters within this type.
    /// </summary>
    /// <value>Read-only collection of all described members.</value>
    public IEnumerable<ModelTypeParameter>? TypeParameters { get; internal set; } = default!;

    private protected ModelMemberAdvancedBase(
        IMember member,
        ModelTypeBase parent,
        XElement? documentation
    )
        : base(member, parent, documentation) { }
}
