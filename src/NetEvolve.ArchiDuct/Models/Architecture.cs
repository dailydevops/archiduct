﻿namespace NetEvolve.ArchiDuct.Models;

using System.Collections.Generic;
using NetEvolve.ArchiDuct.Abstractions;
using static System.StringComparer;

internal sealed class Architecture : IArchitecture
{
    private readonly AssemblyDictionary _assemblies;
    private readonly MembersDictionary _members;
    private readonly NamespaceDictionary _namespaces;
    private readonly TypesDictionary _types;

    /// <inheritdoc cref="IArchitecture.Assemblies" />
    public AssemblyDictionary Assemblies => _assemblies;

    /// <inheritdoc cref="IArchitecture.Members" />
    public MembersDictionary Members => _members;

    /// <inheritdoc cref="IArchitecture.Namespaces" />
    public NamespaceDictionary Namespaces => _namespaces;

    /// <inheritdoc cref="IArchitecture.Types" />
    public TypesDictionary Types => _types;

    internal Architecture(List<ModelAssembly> modelAssemblies)
    {
        _assemblies = modelAssemblies.ToDictionaryInternal(x => x.FullName, Ordinal);

        _members = modelAssemblies
            .SelectMany(x => x.Members)
            .ToDictionaryInternal(x => x.FullName, Ordinal);

        _namespaces = modelAssemblies
            .SelectMany(x => x.Namespaces)
            .ToDictionaryInternal(x => x.FullName, Ordinal);

        _types = modelAssemblies
            .SelectMany(x => x.Types)
            .ToDictionaryInternal(x => x.FullName, Ordinal);
    }
}
