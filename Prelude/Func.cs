using System;
using System.Collections.Generic;
using System.Text;

namespace CatMath.Structures.Linq {
    public static class FuncExtensions {
        public static Func<TResult> Select<TSource, TResult>(this Func<TSource> source, Func<TSource, TResult> transform) =>            
            () => transform(source());

        public static Func<Unit, TResult> Select<TSource, TResult>(this Func<Unit, TSource> source, Func<TSource, TResult> transform) =>
            _ => transform(source(default));

        public static Func<T> SelectMany<T>(this Func<Func<T>> source) =>
            source();

        public static Func<Unit, T> SelectMany<T>(this Func<Unit, Func<Unit, T>> source) =>
            source(default);

        public static Func<T> SelectMany<T>(this Func<Unit, Func<T>> source) =>
            source(default);

        public static Func<Unit, T> SelectMany<T>(this Func<Func<Unit, T>> source) =>
            source();

        public static Func<TResult> SelectMany<TSource, TMiddle, TResult>(this Func<TSource> source, Func<TSource, Func<TMiddle>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector) =>
            from s in source
            let m = middleSelector(s)()
            select resultSelector(s, m);

        public static Func<TResult> SelectMany<TSource, TMiddle, TResult>(this Func<TSource> source, Func<TSource, Func<Unit, TMiddle>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector) =>
            from s in source
            let m = middleSelector(s)(default)
            select resultSelector(s, m);

        public static Func<Unit, TResult> SelectMany<TSource, TMiddle, TResult>(this Func<Unit, TSource> source, Func<TSource, Func<TMiddle>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector) =>
            from s in source
            let m = middleSelector(s)()
            select resultSelector(s, m);

        public static Func<Unit, TResult> SelectMany<TSource, TMiddle, TResult>(this Func<Unit, TSource> source, Func<TSource, Func<Unit, TMiddle>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector) =>
            from s in source
            let m = middleSelector(s)(default)
            select resultSelector(s, m);

        public static Func<TResult> SelectMany<TSource, TResult>(this Func<TSource> source, Func<TSource, Func<TResult>> resultSelector) =>
            from s in source
            select resultSelector(s)();

        public static Func<Unit, TResult> SelectMany<TSource, TResult>(this Func<Unit, TSource> source, Func<TSource, Func<Unit, TResult>> resultSelector) =>
            from s in source
            select resultSelector(s)(default);

        public static Func<TResult> SelectMany<TSource, TResult>(this Func<TSource> source, Func<TSource, Func<Unit, TResult>> resultSelector) =>
            from s in source
            select resultSelector(s)(default);

        public static Func<Unit, TResult> SelectMany<TSource, TResult>(this Func<Unit, TSource> source, Func<TSource, Func<TResult>> resultSelector) =>
            from s in source
            select resultSelector(s)();

        public static Lazy<TResult> SelectMany<TSource, TResult>(this Func<TSource> source, Func<TSource, Lazy<TResult>> resultSelector) =>                        
            Lazy.Create(
                from s in source
                select resultSelector(s).Value);

        public static Lazy<TResult> SelectMany<TSource, TResult>(this Lazy<TSource> source, Func<TSource, Func<TResult>> resultSelector) =>
            from s in source            
            select resultSelector(source.Value)();

        public static Lazy<TResult> SelectMany<TSource, TMiddle, TResult>(this Lazy<TSource> source, Func<TSource, Func<TMiddle>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector) =>
            from s in source
            let m = middleSelector(s)()
            select resultSelector(s, m);

        public static Lazy<TResult> SelectMany<TSource, TMiddle, TResult>(this Func<TSource> source, Func<TSource, Lazy<TMiddle>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector) =>
            Lazy.Create(
                from s in source
                let m = middleSelector(s).Value
                select resultSelector(s, m));
    }
}
