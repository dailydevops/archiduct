namespace NetEvolve.ArchiDuct.Models;

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Models.Abstractions;

/// <summary>
/// Describes a assembly implementation.
/// </summary>
public sealed class ModelAssembly : ModelBase
{
    /// <summary>
    /// Enumerable of all attributes within this assembly.
    /// </summary>
    public HashSet<ModelAttribute> Attributes { get; } = [];

    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.Assembly;

    /// <summary>
    /// Enumerable of all members within this assembly.
    /// </summary>
    public HashSet<ModelMemberBase> Members { get; } = [];

    /// <summary>
    /// Enumerable of all namespaces within this assembly.
    /// </summary>
    public HashSet<ModelNamespace> Namespaces { get; } = [];

    /// <summary>
    /// Enumerable of all references within this assembly.
    /// </summary>
    public HashSet<ModelReference> References { get; internal set; } = [];

    /// <summary>
    /// Enumerable of all types within this assembly.
    /// </summary>
    public HashSet<ModelTypeBase> Types { get; } = [];

    /// <summary>
    /// Version of the assembly.
    /// </summary>
    public Version Version { get; }

    /// <inheritdoc />
    internal ModelAssembly(IModule module, XElement? doc)
        : base($"A:{module.AssemblyName}", module.AssemblyName, module.AssemblyName, doc) =>
        Version = module.AssemblyVersion;
}
