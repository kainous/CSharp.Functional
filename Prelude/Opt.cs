using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace Maat.Functional {
    using Structures;

    static partial class Functions {
        [MethodImpl(AggressiveOptimization)]
        public static Opt<T> AsOpt<T>(this T? value) where T : struct =>
            value.HasValue
            ? OneOf(value.Value)
            : default;

        [MethodImpl(AggressiveOptimization)]
        public static Opt<T> AsOpt<T>(this T? value) where T : class =>
            value is null
            ? default
            : OneOf(value);

        [MethodImpl(AggressiveOptimization)]
        public static Opt<T> AsOptIf<T>(this T value, bool condition) =>
            condition
            ? OneOf(value)
            : default;

        // Different usage, one that comes off as a constructor, vs 
        [MethodImpl(AggressiveOptimization)]
        public static Opt<T> OneOf<T>(T value) =>
            new(value);

        [MethodImpl(AggressiveOptimization)]
        public static Opt<T> None<T>() =>
            default;

        [MethodImpl(AggressiveOptimization)]
        public static Opt<T> Try<T>(Func<T> func, Action<T>? final = null) {
            T value = default!;
            try {
                value = func();
            }
            catch {
                return default;
            }
            finally {
                final?.Invoke(value);
            }
            return new(value);
        }

        [MethodImpl(AggressiveOptimization)]
        public static async Task<Opt<T>> TryAsync<T>(Func<Task<T>> func, Func<T, Task>? final = null) {
            T value = default!;
            try {
                value = await func();
            }
            catch {
                return default;
            }
            finally {
                if (final is not null) {
                    await final(value);
                }
            }
            return new(value);
        }
    }

    namespace Structures {
        using static Functions;

        public readonly struct Opt<T> : IEquatable<Opt<T>> {
            internal bool HasValue { get; }
            internal T Value { get; }

            public Opt(T value) {
                HasValue = true;
                Value = value;
            }

            [MethodImpl(AggressiveOptimization)]
            public bool TryGetValue([NotNullWhen(true)] out T value) {
                if (HasValue) {
                    value = Value!;
                    return true;
                }
                else {
                    value = default!;
                    return false;
                }
            }

            [MethodImpl(AggressiveOptimization)]
            public T OrElse(T alternateValue) =>
                HasValue
                ? Value
                : alternateValue;

            [MethodImpl(AggressiveOptimization)]
            public Opt<T> OrElse(Opt<T> alternateValue) =>
                HasValue
                ? this
                : alternateValue;

            [MethodImpl(AggressiveOptimization)]
            public TResult If<TResult>(Func<T, TResult> transform, TResult alternateValue) =>
                HasValue
                ? transform(Value)
                : alternateValue;

            [MethodImpl(AggressiveOptimization)]
            public static T operator |(Opt<T> value, T alternateValue) =>
                value.OrElse(alternateValue);

            [MethodImpl(AggressiveOptimization)]
            public static Opt<T> operator |(Opt<T> value, Opt<T> alternateValue) =>
                value.OrElse(alternateValue);

            [MethodImpl(AggressiveOptimization)]
            public bool Equals(Opt<T> other, IEqualityComparer<T> comparer) =>
                // Notice here, the single '&' is a non-short circuiting call (eager evaluation)
                (!TryGetValue(out var first) & !other.TryGetValue(out var second))
                || comparer.Equals(first, second);

            [MethodImpl(AggressiveOptimization)]
            public bool Equals(Opt<T> other) =>
                Equals(other, EqualityComparer<T>.Default);

            public override bool Equals(object? obj) =>
                obj is Opt<T> other && Equals(other);

            [MethodImpl(AggressiveOptimization)]
            public static bool operator ==(Opt<T> first, Opt<T> second) =>
                first.Equals(second, EqualityComparer<T>.Default);

            [MethodImpl(AggressiveOptimization)]
            public static bool operator !=(Opt<T> first, Opt<T> second) =>
                !first.Equals(second, EqualityComparer<T>.Default);

            public override int GetHashCode() =>
                TryGetValue(out var result)
                ? result.GetHashCode()
                : 0;

            public override string? ToString() =>
                TryGetValue(out var result)
                ? result.ToString()
                : string.Empty;
        }

        namespace Linq {
            public static class Opt {
                [MethodImpl(AggressiveOptimization)]
                public static Opt<T> Try<T>(Func<T> function) {
                    try {
                        return new(function());
                    }
                    catch {
                        return default;
                    }
                }

                [MethodImpl(AggressiveOptimization)]
                public static Opt<Guid> TryParse(this string text) {
                    return
                        Guid.TryParse(text, out var result)
                        ? new(result)
                        : default;
                }

                [MethodImpl(AggressiveOptimization)]
                public static Opt<Guid> TryParse(this ReadOnlySpan<char> text) {
                    return
                        Guid.TryParse(text, out var result)
                        ? new(result)
                        : default;
                }

                [MethodImpl(AggressiveOptimization)]
                public static IEnumerable<T> AsEnumerable<T>(this Opt<T> items) =>
                    items.TryGetValue(out var result)
                    ? Enumerable.Repeat(result, 1)
                    : Enumerable.Empty<T>();



                [MethodImpl(AggressiveOptimization)]
                public static Opt<TResult> SelectMany<TSource, TMiddle, TResult>(this Opt<TSource> source, Func<TSource, Opt<TMiddle>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector) =>
                    source.TryGetValue(out var s)
                    && middleSelector(s).TryGetValue(out var m)
                    ? OneOf(resultSelector(s, m))
                    : default;

                [MethodImpl(AggressiveOptimization)]
                public static IEnumerable<TResult> SelectMany<TSource, TMiddle, TResult>(this IEnumerable<TSource> source, Func<TSource, Opt<TMiddle>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector) =>
                    source.SelectMany(
                        x => middleSelector(x).AsEnumerable(),
                        resultSelector);



                [MethodImpl(AggressiveOptimization)]
                public static Opt<TResult> SelectMany<TSource, TResult>(this Opt<TSource> source, Func<TSource, Opt<TResult>> transform) =>
                    source.If(transform, default);

                [MethodImpl(AggressiveOptimization)]
                public static Opt<T> SelectMany<T>(this Opt<Opt<T>> source) =>
                    source.If(Id, default);

                [MethodImpl(AggressiveOptimization)]
                public static IEnumerable<T> SelectMany<T>(this IEnumerable<Opt<T>> source) =>
                    source.SelectMany(Id, Second);



                [MethodImpl(AggressiveOptimization)]
                public static Opt<TResult> Select<TSource, TResult>(this Opt<TSource> source, Func<TSource, TResult> transform) =>
                    source.If(
                        transform.ComposeWith(OneOf),
                        None<TResult>());

                [MethodImpl(AggressiveOptimization)]
                public static Opt<T> Where<T>(this Opt<T> source, Func<T, bool> predicate) =>
                    source.If(
                        x => x.AsOptIf(predicate(x)),
                        None<T>());

                [MethodImpl(AggressiveOptimization)]
                public static Opt<T> FirstOrOpt<T>(this IEnumerable<T> items, Func<T, bool> predicate) {
                    foreach (var item in items) {
                        if (predicate(item)) {
                            return OneOf(item);
                        }
                    }
                    return None<T>();
                }

                [MethodImpl(AggressiveOptimization)]
                public static Opt<T> FirstOrOpt<T>(this IEnumerable<T> items) {
                    foreach (var item in items) {
                        return OneOf(item);
                    }
                    return None<T>();
                }

                [MethodImpl(AggressiveOptimization)]
                public static Opt<T> LastOrOpt<T>(this IEnumerable<T> items, Func<T, bool> predicate) {
                    var result = None<T>();
                    foreach (var item in items) {
                        if (predicate(item)) {
                            result = OneOf(item);
                        }
                    }
                    return result;
                }

                [MethodImpl(AggressiveOptimization)]
                public static Opt<T> LastOrOpt<T>(this IEnumerable<T> items) =>
                    items.LastOrOpt(_ => true);

                [MethodImpl(AggressiveOptimization)]
                public static async Task<Opt<T>> FirstOrOpt<T>(this IAsyncEnumerable<T> items, Func<T, bool> predicate) {
                    await foreach (var item in items) {
                        if (predicate(item)) {
                            return OneOf(item);
                        }
                    }
                    return None<T>();
                }

                [MethodImpl(AggressiveOptimization)]
                public static async Task<Opt<T>> FirstOrOpt<T>(this IAsyncEnumerable<T> items) {
                    await foreach (var item in items) {
                        return OneOf(item);
                    }
                    return None<T>();
                }

                [MethodImpl(AggressiveOptimization)]
                public static async Task<Opt<T>> LastOrOpt<T>(this IAsyncEnumerable<T> items, Func<T, bool> predicate) {
                    var result = None<T>();
                    await foreach (var item in items) {
                        if (predicate(item)) {
                            result = OneOf(item);
                        }
                    }
                    return result;
                }

                [MethodImpl(AggressiveOptimization)]
                public static async Task<Opt<T>> LastOrOpt<T>(this IAsyncEnumerable<T> items) =>
                    await items.LastOrOpt(_ => true);

                // This is a natural transformation, but not a natural isomorphism
                [MethodImpl(AggressiveOptimization)]
                public static async Task<Opt<T>> TaskSwap<T>(this Opt<Task<T>> option) =>
                    await
                    option
                    .If<Task<Opt<T>>>(async x => OneOf(await x), Task.FromResult(None<T>()));

                [MethodImpl(AggressiveOptimization)]
                public static Func<Opt<T1>, Opt<T2>> Map<T1, T2>(Func<T1, T2> transform) =>
                    x => x.Select(transform);

                [MethodImpl(AggressiveOptimization)]
                public static Opt<TValue> TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) =>
                    dictionary.TryGetValue(key, out var result)
                    ? OneOf(result)
                    : default;

                [MethodImpl(AggressiveOptimization)]
                public static Opt<TValue> TryGetValue<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key) =>
                    dictionary.TryGetValue(key, out var result)
                    ? OneOf(result)
                    : default;
            }
        }
    }
}
