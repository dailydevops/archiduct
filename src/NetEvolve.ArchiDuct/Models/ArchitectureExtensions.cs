namespace NetEvolve.ArchiDuct.Models;

using System;
using System.Collections.Generic;

internal static class ArchitectureExtensions
{
#if NET8_0_OR_GREATER
    internal static FrozenDictionary<TKey, TSource> ToDictionaryInternal<TSource, TKey>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        IEqualityComparer<TKey>? comparer
    )
        where TKey : notnull => source.ToFrozenDictionary(keySelector, comparer);

#else
    internal static Dictionary<TKey, TSource> ToDictionaryInternal<TSource, TKey>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        IEqualityComparer<TKey>? comparer
    )
        where TKey : notnull => source.ToDictionary(keySelector, comparer);
#endif
}
