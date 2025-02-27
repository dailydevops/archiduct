﻿namespace NetEvolve.ArchiDuct.Models;

using System.Xml.Linq;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Models.Abstractions;

/// <summary>
/// Describes an explicit method implementation.
/// </summary>
public sealed class ModelExplicitMethod : ModelMethod
{
    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.ExplicitMethod;

    /// <inheritdoc />
    internal ModelExplicitMethod(IMethod method, ModelTypeBase parent, XElement? doc)
        : base(method, parent, doc) { }
}
