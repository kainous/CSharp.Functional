using System;
using System.Collections.Generic;

namespace Maat.Functional {
    internal class PropertyEqualityComparer<TClass, TProperty> : IEqualityComparer<TClass> {
        private readonly Func<TClass, TProperty> _getter;
        private readonly IEqualityComparer<TProperty> _propertyComparer;

        public PropertyEqualityComparer(Func<TClass, TProperty> getter, IEqualityComparer<TProperty>? propertyComparer = null) {
            _getter = getter;
            _propertyComparer = propertyComparer ?? EqualityComparer<TProperty>.Default;
        }

        public bool Equals(TClass? x, TClass? y) =>
            ReferenceEquals(x, y)
            || x is not null && y is not null && _propertyComparer.Equals(_getter(x), _getter(y));

        public int GetHashCode(TClass obj) =>
            _propertyComparer.GetHashCode(_getter(obj)!);
    }
}
