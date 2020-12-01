using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CSharp.Functional {
    public static partial class Prelude {
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
        public static Func<T, T> Compose<T>(this IEnumerable<Func<T, T>> functions) =>
            functions.Aggregate(ComposeWith);
    }
}
