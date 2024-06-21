namespace NetEvolve.ArchiDuct.Models.Members;

using System.Xml.Linq;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Models.Abstractions;

/// <summary>
/// Describes an property implementation.
/// </summary>
public class ModelProperty : ModelMemberBase
{
    /// <summary>
    /// Gets the getter method.
    /// </summary>
    public ModelMethod? Getter { get; }

    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.Property;

    /// <summary>
    /// Gets the setter method.
    /// </summary>
    public ModelMethod? Setter { get; }

    /// <summary>
    /// Gets the content from the xml &lt;value/&gt; tag.
    /// </summary>
    public string? Value { get; set; }

    /// <inheritdoc />
    internal ModelProperty(IProperty property, ModelTypeBase parentEntity, XElement? documentation)
        : base(property, parentEntity, documentation)
    {
        if (property.Getter is not null)
        {
            Getter = new ModelMethod(property.Getter, parentEntity, documentation);
        }

        if (property.Setter is not null)
        {
            Setter = new ModelMethod(property.Setter, parentEntity, documentation);
        }
    }
}
