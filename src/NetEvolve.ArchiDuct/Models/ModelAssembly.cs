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
    private readonly Version _version;

    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.Assembly;

    /// <summary>
    /// Enumerable of all namespaces within this assembly.
    /// </summary>
    /// <value>Enumerable of all described namespaces.</value>
    public HashSet<ModelNamespace> Namespaces { get; internal set; } = [];

    /// <summary>
    /// Enumerable of all types within this assembly.
    /// </summary>
    /// <value>Enumerable of all described types.</value>
    public HashSet<ModelTypeBase> Types { get; internal set; } = [];

    /// <summary>
    /// Version of the assembly.
    /// </summary>
    public Version Version => _version;

    /// <inheritdoc />
    internal ModelAssembly(IModule module, XElement? documentation)
        : base(
            $"A:{module.AssemblyName}",
            module.AssemblyName,
            module.AssemblyName,
            documentation
        ) => _version = module.AssemblyVersion;
}
