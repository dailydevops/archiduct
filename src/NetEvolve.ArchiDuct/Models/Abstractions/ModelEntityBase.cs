namespace NetEvolve.ArchiDuct.Models.Abstractions;

using System.Xml.Linq;
using NetEvolve.ArchiDuct.Models.Documentation;

/// <inheritdoc />
public abstract class ModelEntityBase : ModelBase
{
    /// <summary>
    /// Enumerable of all attributes within this type or member.
    /// </summary>
    public HashSet<ModelAttribute> Attributes { get; internal set; } = [];

    /// <summary>
    /// Unique ID string that identifies the parent, could be namespace or type.
    /// </summary>
    public string ParentId { get; }

    private protected ModelEntityBase(
        string id,
        string name,
        string fullName,
        ModelBase parent,
        ModelDocumentation? doc
    )
        : base(id, name, fullName, doc) => ParentId = parent.Id;

    private protected ModelEntityBase(
        string id,
        string name,
        string fullName,
        ModelBase parent,
        XElement? doc
    )
        : base(id, name, fullName, doc) => ParentId = parent.Id;
}
