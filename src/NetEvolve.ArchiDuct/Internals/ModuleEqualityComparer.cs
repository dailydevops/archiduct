namespace NetEvolve.ArchiDuct.Internals;

using ICSharpCode.Decompiler.TypeSystem;

internal sealed class ModuleEqualityComparer : IEqualityComparer<IModule>
{
    public bool Equals(IModule? x, IModule? y) =>
        x is not null && y is not null && x.FullAssemblyName.Equals(y.FullAssemblyName, Ordinal);

    public int GetHashCode(IModule? obj) => obj?.FullAssemblyName.GetHashCode(Ordinal) ?? default;
}
