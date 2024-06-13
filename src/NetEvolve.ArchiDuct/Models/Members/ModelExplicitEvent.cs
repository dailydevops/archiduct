namespace NetEvolve.ArchiDuct.Models.Members;

using System.Xml.Linq;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Models.Abstractions;

/// <summary>
/// Describes an explicit event implementation.
/// </summary>
public sealed class ModelExplicitEvent : ModelEvent
{
    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.ExplicitEvent;

    /// <inheritdoc />
    internal ModelExplicitEvent(IEvent @event, ModelTypeBase parentEntity, XElement? documentation)
        : base(@event, parentEntity, documentation) { }
}
