using System;
using System.Collections.Generic;
using System.Linq;

using Maat.Functional.Structures;

namespace Maat.Collections;

public static class DictionaryExtensions {
    //public static TValue TryGetValue<TKey1, TKey2, TValue> (this IDictionary<TKey1, IDictionary<TKey2, TValue>> dictionary, TKey1 key1, TKey2 key2)
}

//internal class ConcurrentListValuedDictionary

//public class ManyToManyRelation<T1, T2> {
//    private readonly SetValuedDictionary<T1, T2> _forward;
//    private readonly SetValuedDictionary<T2, T1> _reverse;
//}

public class SetValuedDictionary<TKey, TValue> : ILookup<TKey, TValue> {
    private readonly IDictionary<TKey, ISet<TValue>> _items;
    private readonly IEqualityComparer<TValue> _valueComparer;

    public SetValuedDictionary(ILookup<TKey, TValue> items, IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer) {
        _items = new Dictionary<TKey, ISet<TValue>>(keyComparer);
        throw new NotImplementedException();
    }

    public ISet<TValue> this[TKey key] =>
        _items[key];

    public Opt<ISet<TValue>> TryGetValue(TKey key) =>
        _items.TryGetValue(key, out var result)
        ? new(result)
        : default;

    public bool ContainsKey(TKey key) =>
        _items.ContainsKey(key);

    public int Count =>
        _items.Count;

    public void Add(ILookup<TKey, TValue> newItems) {
        foreach (var grouping in newItems) {
            if (_items.TryGetValue(grouping.Key, out var set)) {
                foreach (var item in grouping) {
                    set.Add(item);
                }
            }
            else {
                set = new HashSet<TValue>(grouping, _valueComparer);
                _items.Add(grouping.Key, set);
            }
        }
    }

    public void Add(IEnumerable<KeyValuePair<TKey, TValue>> items) {
        var grouped =
            items.ToLookup(
                i => i.Key, 
                i => i.Value);

        Add(grouped);
    }

    public bool Add(TKey key, TValue value) {
        if (_items.TryGetValue(key, out var set)) {
            return set.Add(value);
        }
        else {
            set = new HashSet<TValue>(Enumerable.Repeat(value, 1), _valueComparer);
            _items.Add(key, set);
            return true;
        }
    }

    public IEnumerable<KeyValuePair<TKey, TValue>> CanAdd(ILookup<TKey, TValue> newItems) {
        foreach (var grouping in newItems) {
            if (_items.TryGetValue(grouping.Key, out var set)) {
                foreach (var item in grouping) {
                    if (!set.Contains(item)) {
                        yield return KeyValuePair.Create(grouping.Key, item);
                    }
                }
            }
            else {
                foreach (var item in grouping) {
                    yield return KeyValuePair.Create(grouping.Key, item);
                }
            }
        }
    }

    public Opt<ISet<TValue>> TryRemove(TKey key) =>
        _items.Remove(key, out var result)
        ? new(result)
        : default;

    public bool TryRemove(TKey key, TValue value) {
        if (_items.TryGetValue(key, out var set) && set.Remove(value)) {
            if (set.Count == 0) {
                _items.Remove(key);
            }
            return true;
        }

        return false;
    }

    #region ILookup<TKey, TValue>
    private struct Grouping : IGrouping<TKey, TValue> {
        public TKey Key { get; init; }

        private readonly ISet<TValue> _items;

        public Grouping(TKey key, ISet<TValue> items) {
            Key = key;
            _items = items;
        }

        public IEnumerator<TValue> GetEnumerator() =>
            _items.GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }

    public IEnumerator<IGrouping<TKey, TValue>> GetEnumerator() {
        foreach (var item in _items) {
            yield return new Grouping(item.Key, item.Value);
        }
    }

    bool ILookup<TKey, TValue>.Contains(TKey key) =>
        ContainsKey(key);

    IEnumerable<TValue> ILookup<TKey, TValue>.this[TKey key] =>
        this[key];

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() =>
        GetEnumerator();

    #endregion
}
