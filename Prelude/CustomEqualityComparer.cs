using System;
using System.Collections.Generic;

namespace Maat.Functional;

internal record CustomEqualityComparer<T>(Func<T, T, bool> equalityPredicate, Func<T, int> hashCodeGenerator) : IEqualityComparer<T> {
    public bool Equals(T? x, T? y) =>
        ReferenceEquals(x, y)
        || x is not null && y is not null && equalityPredicate(x, y);

    public int GetHashCode(T obj) =>
        hashCodeGenerator(obj);
}
