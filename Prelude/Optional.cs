namespace CatMath {
    public struct Optional<T> {
        private readonly T _value;
        private readonly bool _hasValue;

        private Optional(T value) {
            _hasValue = true;
            _value = value;
        }

        public static implicit operator Optional<T>(T value) =>
            new(value);

        public static T operator |(Optional<T> value, T alternate) =>
            value._hasValue ? value._value : alternate;

        public static Optional<T> operator |(Optional<T> value, Optional<T> alternate) =>
            value._hasValue ? value : alternate;
    }
}
