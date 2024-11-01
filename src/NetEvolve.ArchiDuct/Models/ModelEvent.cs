namespace NetEvolve.ArchiDuct.Models;

using System.Xml.Linq;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Internals;
using NetEvolve.ArchiDuct.Models.Abstractions;

/// <summary>
/// Describes an event implementation.
/// </summary>
public class ModelEvent : ModelMemberBase
{
    /// <summary>
    /// Gets the method describtion of the add accessor.
    /// </summary>
    public ModelMethod? AddAccessor { get; }

    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.Event;

    /// <summary>
    /// Gets the method describtion of the remove accessor.
    /// </summary>
    public ModelMethod? RemoveAccessor { get; }

    /// <summary>
    /// Gets the return type.
    /// </summary>
    public ModelReturn Type { get; }

    /// <inheritdoc />
    internal ModelEvent(IEvent @event, ModelTypeBase parent, XElement? doc)
        : base(@event, parent, doc)
    {
        if (@event.AddAccessor is not null)
        {
            AddAccessor = new ModelMethod(@event.AddAccessor, parent, doc);
        }

        if (@event.RemoveAccessor is not null)
        {
            RemoveAccessor = new ModelMethod(@event.RemoveAccessor, parent, doc);
        }

        Type = ModelFactory.GetReturnType(@event)!;
    }
}
