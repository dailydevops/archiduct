namespace NetEvolve.ArchiDuct.Abstractions;

public interface IArchitecture
{
    AssemblyDictionary Assemblies { get; }

    MembersDictionary Members { get; }

    NamespaceDictionary Namespaces { get; }

    TypesDictionary Types { get; }
}
