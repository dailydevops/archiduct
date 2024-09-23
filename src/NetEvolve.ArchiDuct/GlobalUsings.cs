global using static System.StringComparison;
#if NET8_0_OR_GREATER
global using System.Collections.Frozen;
global using AssemblyDictionary = System.Collections.Frozen.FrozenDictionary<
    string,
    NetEvolve.ArchiDuct.Models.ModelAssembly
>;
global using MembersDictionary = System.Collections.Frozen.FrozenDictionary<
    string,
    NetEvolve.ArchiDuct.Models.Abstractions.ModelMemberBase
>;
global using NamespaceDictionary = System.Collections.Frozen.FrozenDictionary<
    string,
    NetEvolve.ArchiDuct.Models.ModelNamespace
>;
global using TypesDictionary = System.Collections.Frozen.FrozenDictionary<
    string,
    NetEvolve.ArchiDuct.Models.Abstractions.ModelTypeBase
>;
#else
global using AssemblyDictionary = System.Collections.Generic.Dictionary<
    string,
    NetEvolve.ArchiDuct.Models.ModelAssembly
>;
global using MembersDictionary = System.Collections.Generic.Dictionary<
    string,
    NetEvolve.ArchiDuct.Models.Abstractions.ModelMemberBase
>;
global using NamespaceDictionary = System.Collections.Generic.Dictionary<
    string,
    NetEvolve.ArchiDuct.Models.ModelNamespace
>;
global using TypesDictionary = System.Collections.Generic.Dictionary<
    string,
    NetEvolve.ArchiDuct.Models.Abstractions.ModelTypeBase
>;
#endif
