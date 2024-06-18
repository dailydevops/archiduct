namespace NetEvolve.ArchiDuct.Abstractions;

/// <summary>
/// Describes the requested assemblies, types, members and namespaces.
/// </summary>
public interface IArchitecture
{
    /// <summary>
    /// List of all assemblies.
    /// </summary>
    AssemblyDictionary Assemblies { get; }

    /// <summary>
    /// List of all members.
    /// </summary>
    MembersDictionary Members { get; }

    /// <summary>
    /// List of all namespaces.
    /// </summary>
    NamespaceDictionary Namespaces { get; }

    /// <summary>
    /// List of all types.
    /// </summary>
    TypesDictionary Types { get; }
}
