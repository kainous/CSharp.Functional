using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CatMath {
    using System.Collections.Concurrent;
    using System.Diagnostics.CodeAnalysis;
    using Structures;
    using Structures.Linq;

    public static partial class Functions {
        public const MethodImplOptions Aggressive = MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization;

        [MethodImpl(Aggressive)]
        public static void Id() {
        }

        [MethodImpl(Aggressive)]
        public static T Id<T>(T value) =>
            value;

        [MethodImpl(Aggressive)]
        public static Func<TIn, TOut> Const<TIn, TOut>(TOut value) =>
            _ => value;

        [MethodImpl(Aggressive)]
        public static Func<T2, T1, T3> Flip<T1, T2, T3>(this Func<T1, T2, T3> function) =>
            (y, x) => function(x, y);

        [MethodImpl(Aggressive)]
        public static Func<T2, Func<T1, T3>> Flip<T1, T2, T3>(this Func<T1, Func<T2, T3>> function) =>
            y => x => function(x)(y);

        [MethodImpl(Aggressive)]
        public static Func<T3, T1, T2, T4> Permute1<T1, T2, T3, T4>(this Func<T1, T2, T3, T4> function) =>
            (z, x, y) => function(x, y, z);

        [MethodImpl(Aggressive)]
        public static Func<T3, T1, T2, T4> Permute2<T1, T2, T3, T4>(this Func<T1, T2, T3, T4> function) =>
            (z, x, y) => function(x, y, z);

        [MethodImpl(Aggressive)]
        public static Func<T1, T3> ComposeWith<T1, T2, T3>(this Func<T1, T2> func1, Func<T2, T3> func2) =>
            x => func2(func1(x));

        [MethodImpl(Aggressive)]
        public static Func<T2> ComposeWith<T1, T2>(this Func<T1> func1, Func<T1, T2> func2) =>
            () => func2(func1());

        [MethodImpl(Aggressive)]
        public static Func<T, T> Compose<T>(this IEnumerable<Func<T, T>> functions) =>
            functions.Aggregate(ComposeWith);

        [MethodImpl(Aggressive)]
        public static (T2, T1) Swap<T1, T2>(this (T1, T2) data) =>
            (data.Item2, data.Item1);

        [MethodImpl(Aggressive)]
        public static Func<Unit, T> WithUnit<T>(this Func<T> data) =>
            _ => data();

        [MethodImpl(Aggressive)]
        public static Action<T> WithoutUnit<T>(Func<T, Unit> action) =>
            x => action(x);

        [MethodImpl(Aggressive)]
        public static Func<T, Unit> WithUnit<T>(Action<T> action) =>
            new Func<T, Unit>(x => { action(x); return default; });

        [MethodImpl(Aggressive)]
        public static Func<TIn, TOut> Memoize<TIn, TOut>(this Func<TIn, TOut> function, IEqualityComparer<TIn>? comparer = null, bool isMultithreaded = true, bool isComplexFunction = true)
          where TIn : notnull {
            comparer ??= EqualityComparer<TIn>.Default;
            if (!isMultithreaded) {
                var dict = new Dictionary<TIn, TOut>(comparer);
                return key => {
                    if (!dict.TryGetValue(key, out var result)) {
                        result = function(key);
                        dict[key] = result;
                    }
                    return result;
                };
            }
            else if (isComplexFunction) {
                var dict = new ConcurrentDictionary<TIn, Lazy<TOut>>(comparer);
                return key => dict.GetOrAdd(key, k => Lazy.Create(() => function(k))).Value;
            }
            else {
                var dict = new ConcurrentDictionary<TIn, TOut>(comparer);
                return key => dict.GetOrAdd(key, function);
            }
        }

        [MethodImpl(Aggressive)]
        public static Func<T1, T2, TOut> Memoize<T1, T2, TOut>(this Func<T1, T2, TOut> function, IEqualityComparer<T1> comparer1, IEqualityComparer<T2> comparer2, bool isMultithreaded = true, bool isComplexFunction = true) =>
            Normalize(Memoize(
                x => function(x.Item1, x.Item2),
                new TupleEqualityComparer<T1, T2>(comparer1, comparer2),
                isMultithreaded,
                isComplexFunction));

        [MethodImpl(Aggressive)]
        public static Func<T1, T2, T3> Detuple<T1, T2, T3>(this Func<(T1, T2), T3> function) =>
        (x, y) => function((x, y));

        [MethodImpl(Aggressive)]
        public static T1 First<T1, T2>(T1 value, T2 _) =>
            value;

        [MethodImpl(Aggressive)]
        public static T1 First<T1, T2>((T1, T2) item) => item.Item1;

        [MethodImpl(Aggressive)]
        public static T2 Second<T1, T2>(T1 _, T2 value) =>
            value;

        [MethodImpl(Aggressive)]
        public static T2 Second<T1, T2>((T1, T2) item) => item.Item2;

        [MethodImpl(Aggressive)]
        public static Func<T1, T2, TOut> Normalize<T1, T2, TOut>(Func<ValueTuple<T1, T2>, TOut> function) =>
            (x, y) => function(ValueTuple.Create(x, y));

        [MethodImpl(Aggressive)]
        public static Func<T1, T2, TOut> Normalize<T1, T2, TOut>(Func<Tuple<T1, T2>, TOut> function) =>
            (x, y) => function(Tuple.Create(x, y));

        [MethodImpl(Aggressive)]
        public static Func<T1, T2, TOut> Normalize<T1, T2, TOut>(Func<KeyValuePair<T1, T2>, TOut> function) =>
            (x, y) => function(KeyValuePair.Create(x, y));

        [MethodImpl(Aggressive)]
        public static Func<T1, T2, T3, TOut> Normalize<T1, T2, T3, TOut>(Func<ValueTuple<T1, T2, T3>, TOut> function) =>
            (x1, x2, x3) => function(ValueTuple.Create(x1, x2, x3));

        [MethodImpl(Aggressive)]
        public static Func<T1, T2, T3, TOut> Normalize<T1, T2, T3, TOut>(Func<ValueTuple<T1, T2>, T3, TOut> function) =>
            (x1, x2, x3) => function(ValueTuple.Create(x1, x2), x3);

        [MethodImpl(Aggressive)]
        public static Func<T1, T2, T3, TOut> Normalize<T1, T2, T3, TOut>(Func<T1, ValueTuple<T2, T3>, TOut> function) =>
            (x1, x2, x3) => function(x1, ValueTuple.Create(x2, x3));

        [MethodImpl(Aggressive)]
        public static Func<T1, T2, T3, TOut> Normalize<T1, T2, T3, TOut>(Func<Tuple<T1, T2, T3>, TOut> function) =>
            (x1, x2, x3) => function(Tuple.Create(x1, x2, x3));

        [MethodImpl(Aggressive)]
        public static Func<T1, T2, T3, TOut> Normalize<T1, T2, T3, TOut>(Func<Tuple<T1, T2>, T3, TOut> function) =>
            (x1, x2, x3) => function(Tuple.Create(x1, x2), x3);

        [MethodImpl(Aggressive)]
        public static Func<T1, T2, T3, TOut> Normalize<T1, T2, T3, TOut>(Func<T1, Tuple<T2, T3>, TOut> function) =>
            (x1, x2, x3) => function(x1, Tuple.Create(x2, x3));

        [MethodImpl(Aggressive)]
        public static Func<T1, T2, T3, TOut> Normalize<T1, T2, T3, TOut>(Func<KeyValuePair<T1, T2>, T3, TOut> function) =>
            (x1, x2, x3) => function(KeyValuePair.Create(x1, x2), x3);

        [MethodImpl(Aggressive)]
        public static Func<T1, T2, T3, TOut> Normalize<T1, T2, T3, TOut>(Func<T1, KeyValuePair<T2, T3>, TOut> function) =>
            (x1, x2, x3) => function(x1, KeyValuePair.Create(x2, x3));
    }
}
