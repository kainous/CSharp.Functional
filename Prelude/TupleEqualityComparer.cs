using System;
using System.Collections.Generic;

namespace CatMath {
    public class TupleEqualityComparer<T1, T2> : IEqualityComparer<(T1, T2)> {
        public IEqualityComparer<T1> Comparer1 { get; }
        public IEqualityComparer<T2> Comparer2 { get; }
        public TupleEqualityComparer(IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2) {
            Comparer1 = comparer1;
            Comparer2 = comparer2;
        }

        public bool Equals((T1, T2) x, (T1, T2) y) =>
            Comparer1.Equals(x.Item1, y.Item1)
            && Comparer2.Equals(x.Item2, y.Item2);

        public int GetHashCode((T1, T2) obj) =>
            HashCode.Combine(
                Comparer1.GetHashCode(obj.Item1),
                Comparer2.GetHashCode(obj.Item2));
    }
}
