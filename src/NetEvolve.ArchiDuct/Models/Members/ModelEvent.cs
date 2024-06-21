namespace NetEvolve.ArchiDuct.Models.Members;

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

    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.Event;

    /// <summary>
    /// Gets the method describtion of the invoke accessor.
    /// </summary>
    public ModelMethod? InvokeAccessor { get; }

    /// <summary>
    /// Gets the method describtion of the remove accessor.
    /// </summary>
    public ModelMethod? RemoveAccessor { get; }

    /// <inheritdoc />
    internal ModelEvent(IEvent @event, ModelTypeBase parentEntity, XElement? documentation)
        : base(@event, parentEntity, documentation)
    {
        if (@event.AddAccessor is not null)
        {
            AddAccessor = new ModelMethod(@event.AddAccessor, parentEntity, documentation);
        }

        if (@event.InvokeAccessor is not null)
        {
            InvokeAccessor = new ModelMethod(@event.InvokeAccessor, parentEntity, documentation);
        }

        if (@event.RemoveAccessor is not null)
        {
            RemoveAccessor = new ModelMethod(@event.RemoveAccessor, parentEntity, documentation);
        }
    }
}
