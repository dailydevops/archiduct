namespace NetEvolve.ArchiDuct.Models.Abstractions;

using System.Xml.Linq;

/// <inheritdoc />
public abstract class ModelEntityBase : ModelBase
{
    /// <summary>
    /// Enumerable of all attributes within this type or member.
    /// </summary>
    public HashSet<ModelAttribute> Attributes { get; internal set; } = [];

    /// <summary>
    /// Unique ID string that identifies the type or member, this includes the parent ID.
    /// </summary>
    public string FullId { get; private set; }

    /// <summary>
    /// Unique ID string that identifies the parent, could be namespace or type.
    /// </summary>
    public string ParentId { get; private set; }

    private protected ModelEntityBase(
        string id,
        string name,
        string fullName,
        ModelBase parentEntity,
        XElement? documentation
    )
        : base(id, name, fullName, documentation)
    {
        ParentId = parentEntity.Id;

        FullId = $"{ParentId}, {Id}";
    }
}
