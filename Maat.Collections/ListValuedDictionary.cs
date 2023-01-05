using System.Collections.Generic;
using System.Linq;

using Maat.Functional.Structures;

using static Maat.Functional.Functions;

namespace Maat.Collections {
    //internal class LazyCurriedDictionary<TKey1, TKey2, TValue> : IDictionary<TKey1, IDictionary<TKey2, TValue>> {

    //}

    public class ListValuedDictionary<TKey, TValue> : ILookup<TKey, TValue> {
        private readonly Dictionary<TKey, IList<TValue>> _items;

        public ListValuedDictionary(IEnumerable<KeyValuePair<TKey, IList<TValue>>>  items, IEqualityComparer<TKey> keyComparer) {
            _items =
                new Dictionary<TKey, IList<TValue>>(items, keyComparer ?? EqualityComparer<TKey>.Default);
        }

        public bool ContainsKey(TKey key) =>
            _items.ContainsKey(key);

        public int Count =>
            _items.Count;

        public IList<TValue> this[TKey key] =>
            _items[key];

        public TValue this[TKey key, int index] =>
            this[key][index];

        public Opt<IList<TValue>> TryGetValue(TKey key) =>
            _items.TryGetValue(key, out var result)
            ? new(result)
            : default;

        public Opt<TValue> TryGetValue(TKey key, int index) =>
            index >= 0
            && _items.TryGetValue(key, out var list)
            && list.Count > index
            ? new(list[index])
            : default; 


        //public IDictionary<(TKey, int), TValue> AsCurried()

        #region ILookup<TKey, TValue>
        private struct Grouping : IGrouping<TKey, TValue> {
            public TKey Key { get; init; }

            private readonly IList<TValue> _items;

            public Grouping(TKey key, IList<TValue> items) {
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
}
