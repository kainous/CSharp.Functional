using System;
using System.Threading.Tasks;

namespace CatMath.Structures.Linq {
    public static class TaskExtensions {
        public static async Task<TResult> Select<TSource, TResult>(this Task<TSource> source, Func<TSource, TResult> transform) =>
            transform(await source);

        public static async Task<T> SelectMany<T>(this Task<Task<T>> task) =>
            await await task;

        public static async Task<TResult> SelectMany<TSource, TMiddle, TResult>(this Task<TSource> source, Func<TSource, Task<TMiddle>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector) {
            var s = await source;
            var m = await middleSelector(s);
            return resultSelector(s, m);
        }

        public static async Task<TResult> SelectMany<TSource, TResult>(this Task<TSource> source, Func<TSource, Task<TResult>> selector) =>
            await selector(await source);

        public static async Task<TResult> SelectMany<TSource, TMiddle, TResult>(this Task<TSource> source, Func<TSource, Func<TMiddle>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector) {
            var s = await source;
            var m = middleSelector(s);
            return resultSelector(s, m());
        }

        public static async Task<TResult> SelectMany<TSource, TMiddle, TResult>(this Task<TSource> source, Func<TSource, Func<Unit, TMiddle>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector) {
            var s = await source;
            var m = middleSelector(s);
            return resultSelector(s, m(default));
        }

        public static async Task<TResult> SelectMany<TSource, TMiddle, TResult>(this Task<TSource> source, Func<TSource, Lazy<TMiddle>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector) {
            var s = await source;
            var m = middleSelector(s);
            return resultSelector(s, m.Value);
        }
    }
}
