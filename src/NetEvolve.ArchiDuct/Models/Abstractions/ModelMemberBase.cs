namespace NetEvolve.ArchiDuct.Models.Abstractions;

using System.Collections.Generic;
using System.Xml.Linq;
using ICSharpCode.Decompiler.Documentation;
using ICSharpCode.Decompiler.TypeSystem;

/// <summary>
/// Describes a member implementation. Could be a constructor, destructor, enum member, event, field, indexer, method, operator or property.
/// </summary>
public abstract class ModelMemberBase : ModelEntityBase
{
    /// <summary>
    /// Collection of all modifiers for this type.
    /// </summary>
    public HashSet<ModelModifier> Modifiers { get; internal set; } = [];

    /// <summary>
    /// Returns the type id of the return type.
    /// </summary>
    public string ReturnTypeId { get; internal set; }

    /// <inheritdoc />
    private protected ModelMemberBase(IMember member, ModelTypeBase parent, XElement? documentation)
        : base(member.GetIdString(), member.Name, member.FullName, parent, documentation) { }
}
