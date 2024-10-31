namespace NetEvolve.ArchiDuct.Models;

using System;
using ICSharpCode.Decompiler.TypeSystem;
using NetEvolve.ArchiDuct.Models.Abstractions;
using NetEvolve.ArchiDuct.Models.Documentation;

/// <summary>
/// Describes a reference, or dependency, to another assembly.
/// </summary>
public sealed class ModelReference : ModelBase
{
    /// <inheritdoc />
    public override ModelKind Kind => ModelKind.Reference;

    /// <summary>
    /// Version of the assembly.
    /// </summary>
    public Version Version { get; }

    internal ModelReference(IModule module)
        : base(
            $"A:{module.AssemblyName}",
            module.AssemblyName,
            module.AssemblyName,
            (ModelDocumentation?)null
        ) => Version = module.AssemblyVersion;
}
