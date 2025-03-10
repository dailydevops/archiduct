﻿namespace NetEvolve.ArchiDuct.Models;

using System.Xml.Linq;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Models.Abstractions;

/// <summary>
/// Describes an property implementation.
/// </summary>
public class ModelProperty : ModelMemberBase
{
    /// <summary>
    /// Gets the getter method. Can be null.
    /// </summary>
    public ModelMethod? Getter { get; }

    /// <summary>
    /// Gets the init only stter method. Can be null if the setter is not init only.
    /// </summary>
    public ModelMethod? InitOnly { get; }

    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.Property;

    /// <summary>
    /// Gets the setter method. Can be null.
    /// </summary>
    public ModelMethod? Setter { get; }

    /// <summary>
    /// Gets the content from the xml &lt;value/&gt; tag.
    /// </summary>
    public string? Value { get; set; }

    /// <inheritdoc />
    internal ModelProperty(IProperty property, ModelTypeBase parent, XElement? doc)
        : base(property, parent, doc)
    {
        if (property.Getter is IMethod getter)
        {
            Getter = new ModelMethod(getter, parent, doc);
        }

        if (property.Setter is IMethod setter)
        {
            if (setter.IsInitOnly)
            {
                InitOnly = new ModelMethod(setter, parent, doc);
            }
            else
            {
                Setter = new ModelMethod(setter, parent, doc);
            }
        }
    }
}
