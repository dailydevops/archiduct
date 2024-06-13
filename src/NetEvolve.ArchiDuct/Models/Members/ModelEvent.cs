namespace NetEvolve.ArchiDuct.Models.Members;

using System.Xml.Linq;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Models.Abstractions;

/// <summary>
/// Describes an event implementation.
/// </summary>
public class ModelEvent : ModelMemberBase
{
    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.Event;

    /// <inheritdoc />
    internal ModelEvent(IEvent @event, ModelTypeBase parentEntity, XElement? documentation)
        : base(@event, parentEntity, documentation) { }
}
