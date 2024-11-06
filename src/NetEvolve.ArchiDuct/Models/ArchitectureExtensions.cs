namespace NetEvolve.ArchiDuct.Models;

using System;
using System.Collections.Generic;

internal static class ArchitectureExtensions
{
    internal static FrozenDictionary<TKey, TSource> ToDictionaryInternal<TSource, TKey>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        IEqualityComparer<TKey>? comparer
    )
        where TKey : notnull =>
        source.DistinctBy(keySelector).ToFrozenDictionary(keySelector, comparer);
}
