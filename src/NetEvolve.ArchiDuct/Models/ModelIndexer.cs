﻿namespace NetEvolve.ArchiDuct.Models;

using System.Collections.Generic;
using System.Xml.Linq;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Models.Abstractions;

/// <summary>
/// Describes an indexer implementation.
/// </summary>
public sealed class ModelIndexer : ModelProperty
{
    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.Indexer;

    /// <inheritdoc />
    public HashSet<ModelParameter> Parameters { get; internal set; } = default!;

    /// <inheritdoc />
    internal ModelIndexer(IProperty indexer, ModelTypeBase parent, XElement? doc)
        : base(indexer, parent, doc) { }
}
