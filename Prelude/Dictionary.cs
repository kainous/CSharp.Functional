using System.Collections.Generic;

namespace Maat.Collections;

public static class Dictionary {
    public static IReadOnlyDictionary<TKey, TValue> ComposeEager<TKey, TMiddle, TValue>(this IReadOnlyDictionary<TKey, TMiddle> first, IReadOnlyDictionary<TMiddle, TValue> second, IEqualityComparer<TKey>? keyComparer = null)
      where TKey : notnull => 
        new EagerlyComposedDictionary<TKey, TMiddle, TValue>(first, second, keyComparer ?? EqualityComparer<TKey>.Default);

    public static IReadOnlyDictionary<TKey, TValue> ComposeLazy<TKey, TMiddle, TValue>(this IReadOnlyDictionary<TKey, TMiddle> first, IReadOnlyDictionary<TMiddle, TValue> second) =>
        new LazyComposedDictionary<TKey, TMiddle, TValue>(first, second);
}
