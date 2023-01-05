using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Maat.Collections; 
internal class EagerlyComposedDictionary<TKey, TMiddle, TValue> 
  : IReadOnlyDictionary<TKey, TValue>
  where TKey : notnull {
    private readonly IReadOnlyDictionary<TKey, TValue> _dictionary;

    public EagerlyComposedDictionary(IReadOnlyDictionary<TKey, TMiddle> first, IReadOnlyDictionary<TMiddle, TValue> second, IEqualityComparer<TKey> keyComparer) {
        var dictionary = new Dictionary<TKey, TValue>(first.Count, keyComparer);        

        foreach (var item in first) {
            dictionary.Add(item.Key, second[first[item.Key]]);
        }
        _dictionary = dictionary;
    }

    public TValue this[TKey key] =>
        _dictionary[key];

    public IEnumerable<TKey> Keys =>
        _dictionary.Keys;

    public IEnumerable<TValue> Values =>
        _dictionary.Values;

    public int Count =>
        _dictionary.Count;

    public bool ContainsKey(TKey key) =>
        _dictionary.ContainsKey(key);

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() =>
        _dictionary.GetEnumerator();

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) =>
        _dictionary.TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();
}
