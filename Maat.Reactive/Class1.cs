using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Maat.Functional;
using Maat.Functional.Structures;

using static Maat.Functional.Functions;

namespace Maat.Reactive {
    public static class ReactiveOpt {
        [MethodImpl(AggressiveOptimization)]
        public static IObservable<T> AsObservable<T>(this Opt<T> items) =>
                    items.TryGetValue(out var result)
                    ? Observable.Return(result)
                    : Observable.Empty<T>();

        [MethodImpl(AggressiveOptimization)]
        public static IAsyncEnumerable<T> AsAsyncEnumerable<T>(this Opt<T> item) =>
            item.TryGetValue(out var result)
            ? AsyncEnumerable.Repeat(result, 1)
            : AsyncEnumerable.Empty<T>();

        [MethodImpl(AggressiveOptimization)]
        public static IObservable<TResult> SelectMany<TSource, TMiddle, TResult>(this IObservable<TSource> source, Func<TSource, Opt<TMiddle>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector) =>
                    source.SelectMany(
                        x => middleSelector(x).AsObservable(),
                        resultSelector);

        [MethodImpl(AggressiveOptimization)]
        public static IAsyncEnumerable<TResult> SelectMany<TSource, TMiddle, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, Opt<TMiddle>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector) =>
            source.SelectMany(
                x => middleSelector(x).AsAsyncEnumerable(),
                resultSelector);

        [MethodImpl(AggressiveOptimization)]
        public static IObservable<T> SelectMany<T>(this IObservable<Opt<T>> source) =>
                    source.SelectMany(Id, Second);

        [MethodImpl(AggressiveOptimization)]
        public static IAsyncEnumerable<T> SelectMany<T>(this IAsyncEnumerable<Opt<T>> source) =>
            source.SelectMany(Id, Second);

        [MethodImpl(AggressiveOptimization)]
        public static async Task<Opt<T>> FirstOrOpt<T>(this IObservable<T> items, Func<T, bool> predicate) =>
                    // cannot assume T is nullable
                    // Would have been so much easier to do FirstOrDefault(predicate) ?? None<T>()
                    await
                    items
                    .Aggregate(None<T>(), (acc, elem) => predicate(elem) ? OneOf(elem) : acc)
                    .FirstAsync(a => a.If(_ => true, false));

        [MethodImpl(AggressiveOptimization)]
        public static async Task<Opt<T>> FirstOrOpt<T>(this IObservable<T> items) =>
            // cannot assume T is nullable
            // Would have been so much easier to do FirstOrDefault(predicate) ?? None<T>()
            await
            items
            .Select(OneOf)
            .FirstAsync();

        [MethodImpl(AggressiveOptimization)]
        public static async Task<Opt<T>> LastOrOpt<T>(this IObservable<T> items, Func<T, bool> predicate) =>
            // cannot assume T is nullable
            // Would have been so much easier to do FirstOrDefault(predicate) ?? None<T>()
            await
            items
            .Aggregate(None<T>(), (acc, elem) => predicate(elem) ? OneOf(elem) : acc)
            .LastAsync(a => a.If(_ => true, false));

        [MethodImpl(AggressiveOptimization)]
        public static async Task<Opt<T>> LastOrOpt<T>(this IObservable<T> items) =>
            // cannot assume T is nullable
            // Would have been so much easier to do LastOrDefault(predicate) ?? None<T>()
            await
            items
            .Select(OneOf)
            .LastAsync();

        [MethodImpl(AggressiveOptimization)]
        public static IObservable<T> AsObservable<T>(this Result<T> items) =>
                    items.If(
                        Observable.Return,
                        _ => Observable.Empty<T>());

        [MethodImpl(AggressiveOptimization)]
        public static IAsyncEnumerable<T> AsAsyncEnumerable<T>(this Result<T> items) =>
            items.If(
                x => AsyncEnumerable.Repeat(x, 1),
                _ => AsyncEnumerable.Empty<T>());

        [MethodImpl(AggressiveOptimization)]
        public static IObservable<T> AsObservable<T, TException>(this Result<T, TException> items)
                  where TException : Exception =>
                    items.If(
                        Observable.Return,
                        _ => Observable.Empty<T>());

        [MethodImpl(AggressiveOptimization)]
        public static IAsyncEnumerable<T> AsAsyncEnumerable<T, TException>(this Result<T, TException> items)
          where TException : Exception =>
            items.If(
                x => AsyncEnumerable.Repeat(x, 1),
                _ => AsyncEnumerable.Empty<T>());

        [MethodImpl(AggressiveOptimization)]
        public static IObservable<TResult> SelectMany<TSource, TMiddle, TResult>(this IObservable<TSource> source, Func<TSource, Result<TMiddle>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector) =>
            source.SelectMany(middleSelector.ComposeWith(AsObservable), resultSelector);

        [MethodImpl(AggressiveOptimization)]
        public static IAsyncEnumerable<TResult> SelectMany<TSource, TMiddle, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, Result<TMiddle>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector) =>
            source.SelectMany(middleSelector.ComposeWith(AsAsyncEnumerable), resultSelector);


    }
}
