using System;

namespace CSharp.Functional.Structures.Linq {
    using static Functions;
    public static class Lazy {
        public static Lazy<T> AsLazy<T>(this Func<T> function) =>
            new Lazy<T>(function);

        public static Lazy<T> Create<T>(Func<T> lazyFactory) =>
            new Lazy<T>(lazyFactory);

        public static Lazy<TResult> Select<TSource, TResult>(this Lazy<TSource> source, Func<TSource, TResult> transform) =>
            new Lazy<TResult>(transform.Defer(source.Value));

        public static Lazy<T> SelectMany<T>(this Lazy<Lazy<T>> source) =>
            source.Value;

        public static Lazy<TResult> SelectMany<TSource, TMiddle, TResult>(this Lazy<TSource> source, Func<TSource, Lazy<TMiddle>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector) =>
            from s in source
            let m = middleSelector(s)
            select resultSelector(s, m.Value);

        public static Lazy<TResult> SelectMany<TSource, TResult>(this Lazy<TSource> source, Func<TSource, Lazy<TResult>> resultSelector) =>
            resultSelector(source.Value);
    }
}
