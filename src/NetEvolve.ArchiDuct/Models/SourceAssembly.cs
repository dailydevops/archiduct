namespace NetEvolve.ArchiDuct.Models;

using System.Collections.Generic;
using System.Reflection;

internal sealed record SourceAssembly(Assembly Assembly)
{
    public HashSet<SourceFilter> Filters { get; init; } = [];
}
