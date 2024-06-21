namespace NetEvolve.ArchiDuct.Models;

using System;
using System.Collections.Generic;

internal static class ArchitectureExtensions
{
    internal static
#if NET8_0_OR_GREATER
    FrozenDictionary<TKey, TSource>
#else
    Dictionary<TKey, TSource>
#endif
    ToDictionaryInternal<TSource, TKey>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        IEqualityComparer<TKey>? comparer
    )
        where TKey : notnull =>
        source.DistinctBy(keySelector)
#if NET8_0_OR_GREATER
        .ToFrozenDictionary
#else
        .ToDictionary
#endif
        (keySelector, comparer);
}
