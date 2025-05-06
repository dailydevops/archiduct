namespace NetEvolve.ArchiDuct.Models;

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
        Assemblies = modelAssemblies.ToDictionaryInternal(x => x.FullName, Ordinal);

        Members = modelAssemblies.SelectMany(x => x.Members).ToDictionaryInternal(x => x.FullName, Ordinal);

        Namespaces = modelAssemblies.SelectMany(x => x.Namespaces).ToDictionaryInternal(x => x.FullName, Ordinal);

        Types = modelAssemblies.SelectMany(x => x.Types).ToDictionaryInternal(x => x.FullName, Ordinal);
    }
}
