﻿namespace NetEvolve.ArchiDuct.Models;

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
    internal ModelExplicitEvent(IEvent @event, ModelTypeBase parent, XElement? doc)
        : base(@event, parent, doc) { }
}
