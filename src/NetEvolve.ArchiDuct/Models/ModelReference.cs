namespace NetEvolve.ArchiDuct.Models;

using System;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Models.Abstractions;

/// <summary>
/// Describes a reference, or dependency, to another assembly.
/// </summary>
public sealed class ModelReference : ModelBase
{
    private readonly Version _version;

    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.Reference;

    /// <summary>
    /// Version of the assembly.
    /// </summary>
    public Version Version => _version;

    internal ModelReference(IModule module)
        : base($"A:{module.AssemblyName}", module.AssemblyName, module.AssemblyName, null) =>
        _version = module.AssemblyVersion;
}
