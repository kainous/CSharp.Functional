using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Maat.Collections {
    internal class LazyComposedDictionary<TKey, TMiddle, TValue> : IReadOnlyDictionary<TKey, TValue> {
        private readonly IReadOnlyDictionary<TKey, TMiddle> _first;
        private readonly IReadOnlyDictionary<TMiddle, TValue> _second;

        public LazyComposedDictionary(IReadOnlyDictionary<TKey, TMiddle> first, IReadOnlyDictionary<TMiddle, TValue> second) {
            _first = first;
            _second = second;
        }

        public TValue this[TKey key] =>
            _second[_first[key]];

        public IEnumerable<TKey> Keys =>
            _first.Keys;

        public IEnumerable<TValue> Values =>
            _second.Values;

        public int Count =>
            _first.Count;

        public bool ContainsKey(TKey key) =>
            _first.ContainsKey(key);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
            foreach (var item in _first) {
                yield return KeyValuePair.Create(item.Key, this[item.Key]);
            }
        }

        public bool TryGetValue(TKey key, out TValue value) {
            if (_first.TryGetValue(key, out var middle) && _second.TryGetValue(middle, out var result)) {
                value = result;
                return true;
            }
            value = default!;
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }
}
