using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CSharp.Functional.Structures.Linq {
    public static class FuncExtensions {
        private const MethodImplOptions Aggressive = MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveInlining;
        
        [MethodImpl(Aggressive)]
        public static T2 PipeTo<T1, T2>(this T1 value, Func<T1, T2> function) =>
            function(value);

        [MethodImpl(Aggressive)]
        public static Func<TResult> Select<TSource, TResult>(this Func<TSource> source, Func<TSource, TResult> transform) =>            
            () => transform(source());

        [MethodImpl(Aggressive)]
        public static Func<Unit, TResult> Select<TSource, TResult>(this Func<Unit, TSource> source, Func<TSource, TResult> transform) =>
            _ => transform(source(default));

        [MethodImpl(Aggressive)]
        public static Func<T> SelectMany<T>(this Func<Func<T>> source) =>
            source();

        [MethodImpl(Aggressive)]
        public static Func<Unit, T> SelectMany<T>(this Func<Unit, Func<Unit, T>> source) =>
            source(default);

        [MethodImpl(Aggressive)]
        public static Func<T> SelectMany<T>(this Func<Unit, Func<T>> source) =>
            source(default);

        [MethodImpl(Aggressive)]
        public static Func<Unit, T> SelectMany<T>(this Func<Func<Unit, T>> source) =>
            source();

        [MethodImpl(Aggressive)]
        public static Func<TResult> SelectMany<TSource, TMiddle, TResult>(this Func<TSource> source, Func<TSource, Func<TMiddle>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector) =>
            from s in source
            let m = middleSelector(s)()
            select resultSelector(s, m);

        [MethodImpl(Aggressive)]
        public static Func<TResult> SelectMany<TSource, TMiddle, TResult>(this Func<TSource> source, Func<TSource, Func<Unit, TMiddle>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector) =>
            from s in source
            let m = middleSelector(s)(default)
            select resultSelector(s, m);

        [MethodImpl(Aggressive)]
        public static Func<Unit, TResult> SelectMany<TSource, TMiddle, TResult>(this Func<Unit, TSource> source, Func<TSource, Func<TMiddle>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector) =>
            from s in source
            let m = middleSelector(s)()
            select resultSelector(s, m);

        public static Func<Unit, TResult> SelectMany<TSource, TMiddle, TResult>(this Func<Unit, TSource> source, Func<TSource, Func<Unit, TMiddle>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector) =>
            from s in source
            let m = middleSelector(s)(default)
            select resultSelector(s, m);

        [MethodImpl(Aggressive)]
        public static Func<TResult> SelectMany<TSource, TResult>(this Func<TSource> source, Func<TSource, Func<TResult>> resultSelector) =>
            from s in source
            select resultSelector(s)();

        [MethodImpl(Aggressive)]
        public static Func<Unit, TResult> SelectMany<TSource, TResult>(this Func<Unit, TSource> source, Func<TSource, Func<Unit, TResult>> resultSelector) =>
            from s in source
            select resultSelector(s)(default);

        [MethodImpl(Aggressive)]
        public static Func<TResult> SelectMany<TSource, TResult>(this Func<TSource> source, Func<TSource, Func<Unit, TResult>> resultSelector) =>
            from s in source
            select resultSelector(s)(default);

        [MethodImpl(Aggressive)]
        public static Func<Unit, TResult> SelectMany<TSource, TResult>(this Func<Unit, TSource> source, Func<TSource, Func<TResult>> resultSelector) =>
            from s in source
            select resultSelector(s)();

        [MethodImpl(Aggressive)]
        public static Lazy<TResult> SelectMany<TSource, TResult>(this Func<TSource> source, Func<TSource, Lazy<TResult>> resultSelector) =>                        
            Lazy.Create(
                from s in source
                select resultSelector(s).Value);

        [MethodImpl(Aggressive)]
        public static Lazy<TResult> SelectMany<TSource, TResult>(this Lazy<TSource> source, Func<TSource, Func<TResult>> resultSelector) =>
            from s in source            
            select resultSelector(source.Value)();

        [MethodImpl(Aggressive)]
        public static Lazy<TResult> SelectMany<TSource, TMiddle, TResult>(this Lazy<TSource> source, Func<TSource, Func<TMiddle>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector) =>
            from s in source
            let m = middleSelector(s)()
            select resultSelector(s, m);

        [MethodImpl(Aggressive)]
        public static Lazy<TResult> SelectMany<TSource, TMiddle, TResult>(this Func<TSource> source, Func<TSource, Lazy<TMiddle>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector) =>
            Lazy.Create(
                from s in source
                let m = middleSelector(s).Value
                select resultSelector(s, m));
    }
}
