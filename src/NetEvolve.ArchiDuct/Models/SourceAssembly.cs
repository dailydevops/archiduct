namespace NetEvolve.ArchiDuct.Models;

using System.Collections.Generic;
using System.Reflection;

internal readonly struct SourceAssembly : IEquatable<SourceAssembly>
{
    public HashSet<SourceFilter> Filters { get; } = [];
    public Assembly Assembly { get; }

    public SourceAssembly(Assembly assembly) => Assembly = assembly;

    public bool Equals(SourceAssembly other) => Assembly.Equals(other.Assembly);

    public override bool Equals(object? obj) => obj is SourceAssembly other && Equals(other);

    public override int GetHashCode() => Assembly.GetHashCode();
}
