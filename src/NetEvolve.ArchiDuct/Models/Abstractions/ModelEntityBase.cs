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
        ModelBase parentEntity,
        ModelDocumentation? documentation
    )
        : base(id, name, fullName, documentation) => ParentId = parentEntity.Id;

    private protected ModelEntityBase(
        string id,
        string name,
        string fullName,
        ModelBase parentEntity,
        XElement? documentation
    )
        : base(id, name, fullName, documentation) => ParentId = parentEntity.Id;
}
