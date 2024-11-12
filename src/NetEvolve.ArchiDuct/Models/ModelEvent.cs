namespace NetEvolve.ArchiDuct.Models;

using System.Xml.Linq;
using ICSharpCode.Decompiler.TypeSystem;
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

    /// <summary>
    /// Gets the method describtion of the invoke accessor.
    /// </summary>
    public ModelMethod? InvokeAccessor { get; }

    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.Event;

    /// <summary>
    /// Gets the method describtion of the remove accessor.
    /// </summary>
    public ModelMethod? RemoveAccessor { get; }

    /// <inheritdoc />
    internal ModelEvent(IEvent @event, ModelTypeBase parent, XElement? doc)
        : base(@event, parent, doc)
    {
        if (@event.CanAdd)
        {
            AddAccessor = new ModelMethod(@event.AddAccessor, parent, doc);
        }

        if (@event.CanInvoke)
        {
            InvokeAccessor = new ModelMethod(@event.InvokeAccessor, parent, doc);
        }

        if (@event.CanRemove)
        {
            RemoveAccessor = new ModelMethod(@event.RemoveAccessor, parent, doc);
        }
    }
}
