using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CatMath {
    using System.Collections.Concurrent;
    using Structures;
    using Structures.Linq;

    public static partial class Functions {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Id() { 
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Id<T>(T value) => 
            value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<TIn, TOut> Const<TIn, TOut>(TOut value) =>
            _ => value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T2, T1, T3> Flip<T1, T2, T3>(this Func<T1, T2, T3> function) =>
            (y, x) => function(x, y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T2, Func<T1, T3>> Flip<T1, T2, T3>(this Func<T1, Func<T2, T3>> function) =>
            y => x => function(x)(y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T3, T1, T2, T4> Permute1<T1, T2, T3, T4>(this Func<T1, T2, T3, T4> function) =>
            (z, x, y) => function(x, y, z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T3, T1, T2, T4> Permute2<T1, T2, T3, T4>(this Func<T1, T2, T3, T4> function) =>
            (z, x, y) => function(x, y, z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T1, T3> ComposeWith<T1, T2, T3>(this Func<T1, T2> func1, Func<T2, T3> func2) =>
            x => func2(func1(x));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T2> ComposeWith<T1, T2>(this Func<T1> func1, Func<T1, T2> func2) =>
            () => func2(func1());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T, T> Compose<T>(this IEnumerable<Func<T, T>> functions) =>
            functions.Aggregate(ComposeWith);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (T2, T1) Swap<T1, T2>(this (T1, T2) data) =>
            (data.Item2, data.Item1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<Unit, T> WithUnit<T>(this Func<T> data) =>
            _ => data();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Action<T> WithoutUnit<T>(Func<T, Unit> action) =>
            x => action(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T, Unit> WithUnit<T>(Action<T> action) =>
            new Func<T, Unit>(x => { action(x); return default; });

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<TIn, TOut> Memoize<TIn, TOut>(this Func<TIn, TOut> function, IEqualityComparer<TIn> comparer, bool isMultithreaded = true, bool isComplexFunction = true) {
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T1, T2, T3> Detuple<T1, T2, T3>(this Func<(T1, T2), T3> function) =>
            (x, y) => function((x, y));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T1 First<T1, T2>(T1 value, T2 _) =>
            value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T1 First<T1, T2>((T1, T2) item) => item.Item1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T2 Second<T1, T2>(T1 _, T2 value) =>
            value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T2 Second<T1, T2>((T1, T2) item) => item.Item2;
    }
}
