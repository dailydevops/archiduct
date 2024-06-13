namespace NetEvolve.ArchiDuct.Models;

using System.Collections.Generic;
using System.Reflection;

internal sealed record SourceAssembly
{
    public Assembly Assembly { get; init; } = default!;
    public HashSet<SourceFilter> Filters { get; init; } = [];
}
