namespace NetEvolve.ArchiDuct.Models;

using System.Collections.Generic;
using NetEvolve.ArchiDuct.Abstractions;
using static System.StringComparer;

internal sealed class Architecture : IArchitecture
{
    /// <inheritdoc cref="IArchitecture.Assemblies" />
    public AssemblyDictionary Assemblies { get; }

    /// <inheritdoc cref="IArchitecture.Members" />
    public MembersDictionary Members { get; }

    /// <inheritdoc cref="IArchitecture.Namespaces" />
    public NamespaceDictionary Namespaces { get; }

    /// <inheritdoc cref="IArchitecture.Types" />
    public TypesDictionary Types { get; }

    internal Architecture(List<ModelAssembly> modelAssemblies)
    {
        // TODO: Switch back to `x => x.FullName`, when https://github.com/orgs/VerifyTests/discussions/1289 is solved

        Assemblies = modelAssemblies.ToDictionaryInternal(x => x.Id, Ordinal);

        Members = modelAssemblies
            .SelectMany(x => x.Members)
            .ToDictionaryInternal(x => x.Id, Ordinal);

        Namespaces = modelAssemblies
            .SelectMany(x => x.Namespaces)
            .ToDictionaryInternal(x => x.Id, Ordinal);

        Types = modelAssemblies.SelectMany(x => x.Types).ToDictionaryInternal(x => x.Id, Ordinal);
    }
}
