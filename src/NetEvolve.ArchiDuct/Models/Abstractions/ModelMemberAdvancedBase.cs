namespace NetEvolve.ArchiDuct.Models.Abstractions;

using System.Collections.Generic;
using System.Xml.Linq;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Models;

/// <inheritdoc />
public abstract class ModelMemberAdvancedBase : ModelMemberBase
{
    /// <summary>
    /// Collection of all member IDs that overload this member.
    /// </summary>
    /// <value>Read-only collection of all overload members.</value>
    public HashSet<string>? OverloadedMembers { get; internal set; }

    /// <summary>
    /// Collection of all member parameters for this member.
    /// </summary>
    /// <value>Read-only collection of all described parameters..</value>
    public HashSet<ModelParameter>? Parameters { get; internal set; }

    /// <summary>
    /// Collection of all type parameters within this type.
    /// </summary>
    /// <value>Read-only collection of all described members.</value>
    public HashSet<ModelTypeParameter>? TypeParameters { get; internal set; }

    private protected ModelMemberAdvancedBase(IMember member, ModelTypeBase parent, XElement? doc)
        : base(member, parent, doc) { }
}
