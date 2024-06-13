namespace NetEvolve.ArchiDuct.Abstractions;

#if NET8_0_OR_GREATER
using AssemblyDictionary = System.Collections.Frozen.FrozenDictionary<string, Models.ModelAssembly>;
using MembersDictionary = System.Collections.Frozen.FrozenDictionary<
    string,
    Models.Abstractions.ModelMemberBase
>;
using NamespaceDictionary = System.Collections.Frozen.FrozenDictionary<
    string,
    Models.ModelNamespace
>;
using TypesDictionary = System.Collections.Frozen.FrozenDictionary<
    string,
    Models.Abstractions.ModelTypeBase
>;
#else
using AssemblyDictionary = System.Collections.Generic.Dictionary<string, Models.ModelAssembly>;
using MembersDictionary = System.Collections.Generic.Dictionary<
    string,
    Models.Abstractions.ModelMemberBase
>;
using NamespaceDictionary = System.Collections.Generic.Dictionary<string, Models.ModelNamespace>;
using TypesDictionary = System.Collections.Generic.Dictionary<
    string,
    Models.Abstractions.ModelTypeBase
>;
#endif

public interface IArchitecture
{
    AssemblyDictionary Assemblies { get; }

    MembersDictionary Members { get; }

    NamespaceDictionary Namespaces { get; }

    TypesDictionary Types { get; }
}
