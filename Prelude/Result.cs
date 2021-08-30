using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CSharp.Functional.Structures;

namespace CSharp.Functional {
    using static Functions;
    static partial class Functions {
        public static Result<T> Ok<T>(T value) =>
            new(value);

        public static Result<T> Error<T>(Exception ex) =>
            new(ex);

        public static Result<TResult, TException> Try<TResult, TException>(Func<TResult> tryFunction, Action<TResult>? finallyFunction = null)
          where TException : Exception {
            TResult result = default!;
            try {
                result = tryFunction();
            }
            catch (TException ex) {
                return new(ex);
            }
            finally {
                if (finallyFunction is not null) {
                    finallyFunction(result);
                }
            }
            return new(result);
        }

        [MethodImpl(Aggressive)]
        public static Result<T> Try<T>(Func<T> func, Action<T>? final) {
            T value = default!;
            try {
                value = func();
            }
            catch (Exception ex) {
                return new(ex);
            }
            finally {
                final?.Invoke(value);
            }
            return new(value);
        }

        [MethodImpl(Aggressive)]
        public static async Task<Result<T>> TryAsync<T>(Func<Task<T>> func, Func<T, Task>? final) {
            T value = default!;
            try {
                value = await func();
            }
            catch (Exception ex) {
                return new(ex);
            }
            finally {
                final?.Invoke(value);
            }
            return new(value);
        }

        public static Result<T> Try<T>(Func<T> tryFunction, Action? finallyFunction = null) {
            try {
                return new Result<T>(tryFunction());
            }
            catch (Exception ex) {
                return new Result<T>(ex);
            }
            finally {
                finallyFunction?.Invoke();
            }
        }
    }

    namespace Structures {
        public sealed class Result<T, TException>
          where TException : Exception {
            private readonly T _ok = default!;
            private readonly TException? _err = null!;

            public Result(TException error) =>
                _err = error;

            public Result(T value) =>
                _ok = value;

            [MethodImpl(Aggressive)]
            public TResult If<TResult>(Func<T, TResult> successFunction, Func<TException, TResult> failureFunction) =>
                // Constructors prohibit the case that both are set, or neither are set
                _err is null ? successFunction(_ok) : failureFunction(_err);

            [MethodImpl(Aggressive)]
            public Result<T, TException> OrElse(Result<T, TException> alternateValue) =>
                If(_ => this, _ => alternateValue);

            [MethodImpl(Aggressive)]
            public static Result<T, TException> operator |(Result<T, TException> value, Result<T, TException> alternateValue) =>
                value.If(_ => value, _ => alternateValue);

            [MethodImpl(Aggressive)]
            public T OrElse(T alternateValue) =>
                If(Id, _ => alternateValue);

            [MethodImpl(Aggressive)]
            public static T operator |(Result<T, TException> value, T alternateValue) =>
                value.If(Id, _ => alternateValue);
        }

        public sealed class Result<T> {
            private readonly T _ok = default!;
            private readonly Exception? _err = default!;

            public Result(Exception error) =>
                _err = error;

            public Result(T value) =>
                _ok = value;

            [MethodImpl(Aggressive)]
            public TResult If<TResult>(Func<T, TResult> successFunction, Func<Exception, TResult> failureFunction) =>
                // Constructors prohibit the case that both are set, or neither are set
                _err is null ? successFunction(_ok) : failureFunction(_err);

            [MethodImpl(Aggressive)]
            public Result<T> OrElse(Result<T> alternateValue) =>
                If(_ => this, _ => alternateValue);

            [MethodImpl(Aggressive)]
            public static Result<T> operator |(Result<T> value, Result<T> alternateValue) =>
                value.If(_ => value, _ => alternateValue);

            [MethodImpl(Aggressive)]
            public T OrElse(T alternateValue) =>
                If(Id, _ => alternateValue);

            [MethodImpl(Aggressive)]
            public static T operator |(Result<T> value, T alternateValue) =>
                value.If(Id, _ => alternateValue);
        }

        namespace Linq {
            public static class Result {
                [MethodImpl(Aggressive)]
                public static Result<TResult> ForgetExceptionType<TResult, TException>(this Result<TResult, TException> result)
                  where TException : Exception =>
                    result.If<Result<TResult>>(
                        val => new(val),
                        err => new(err));

                [MethodImpl(Aggressive)]
                public static Opt<T> ToOpt<T>(this Result<T> result) =>
                    result.If(OneOf, _ => None<T>());

                [MethodImpl(Aggressive)]
                public static Opt<T> ToOpt<T, TException>(this Result<T, TException> result)
                  where TException : Exception =>
                    result.If(OneOf, _ => None<T>());

                [MethodImpl(Aggressive)]
                public static Result<TResult, TException> Select<TSource, TResult, TException>(this Result<TSource, TException> result, Func<TSource, TResult> transform)
                  where TException : Exception =>
                    result.If<Result<TResult, TException>>(
                        val => new(transform(val)),
                        err => new(err));

                [MethodImpl(Aggressive)]
                public static Result<TResult, TExceptionOut> Select<TSource, TResult, TExceptionIn, TExceptionOut>(this Result<TSource, TExceptionIn> result, Func<TSource, TResult> transform, Func<TExceptionIn, TExceptionOut> exceptionTransformer)
                  where TExceptionIn : Exception
                  where TExceptionOut : Exception =>
                    result.If<Result<TResult, TExceptionOut>>(
                        val => new(transform(val)),
                        err => new(exceptionTransformer(err)));

                [MethodImpl(Aggressive)]
                public static Result<TResult> Select<TSource, TResult>(this Result<TSource> result, Func<TSource, TResult> transform) =>
                    result.If<Result<TResult>>(
                        val => new(transform(val)),
                        err => new(err));

                [MethodImpl(Aggressive)]
                public static Result<T, TException> SelectMany<T, TException>(this Result<Result<T, TException>, TException> result)
                  where TException : Exception =>
                    result.If(
                        val => val,
                        err => new(err));


                [MethodImpl(Aggressive)]
                public static IEnumerable<T> AsEnumerable<T>(this Result<T> items) =>
                    items.If(
                        x => Enumerable.Repeat(x, 1),
                        _ => Enumerable.Empty<T>());

                [MethodImpl(Aggressive)]
                public static IObservable<T> AsObservable<T>(this Result<T> items) =>
                    items.If(
                        Observable.Return,
                        _ => Observable.Empty<T>());

                [MethodImpl(Aggressive)]
                public static IAsyncEnumerable<T> AsAsyncEnumerable<T>(this Result<T> items) =>
                    items.If(
                        x => AsyncEnumerable.Repeat(x, 1),
                        _ => AsyncEnumerable.Empty<T>());

                [MethodImpl(Aggressive)]
                public static Result<TResult> SelectMany<TSource, TMiddle, TResult>(this Result<TSource> source, Func<TSource, Result<TMiddle>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector) =>
                    source.If(
                        val =>
                            middleSelector(val)
                            .If(mid => Ok(resultSelector(val, mid)), Error<TResult>),
                        Error<TResult>);

                [MethodImpl(Aggressive)]
                public static IEnumerable<TResult> SelectMany<TSource, TMiddle, TResult>(this IEnumerable<TSource> source, Func<TSource, Result<TMiddle>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector) =>
                    source.SelectMany(x => middleSelector(x).AsEnumerable(), resultSelector);

                [MethodImpl(Aggressive)]
                public static IObservable<TResult> SelectMany<TSource, TMiddle, TResult>(this IObservable<TSource> source, Func<TSource, Result<TMiddle>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector) =>
                    source.SelectMany(x => middleSelector(x).AsObservable(), resultSelector);

                [MethodImpl(Aggressive)]
                public static IAsyncEnumerable<TResult> SelectMany<TSource, TMiddle, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, Result<TMiddle>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector) =>
                    source.SelectMany(x => middleSelector(x).AsAsyncEnumerable(), resultSelector);

                [MethodImpl(Aggressive)]
                public static Result<TResult> SelectMany<TSource, TResult>(this Result<TSource> source, Func<TSource, Result<TResult>> transform) =>
                    source.If(transform, Error<TResult>);

                [MethodImpl(Aggressive)]
                public static Result<T> SelectMany<T>(this Result<Result<T>> source) =>
                    source.If(Id, Error<T>);

                [MethodImpl(Aggressive)]
                public static IEnumerable<T> SelectMany<T>(this IEnumerable<Result<T>> source) =>
                    source.SelectMany(Id, Second);

                [MethodImpl(Aggressive)]
                public static IObservable<T> SelectMany<T>(this IObservable<Result<T>> source) =>
                    source.SelectMany(Id, Second);

                [MethodImpl(Aggressive)]
                public static IAsyncEnumerable<T> SelectMany<T>(this IAsyncEnumerable<Result<T>> source) =>
                    source.SelectMany(Id, Second);

                [MethodImpl(Aggressive)]
                public static Func<Result<T1>, Result<T2>> Map<T1, T2>(Func<T1, T2> transform) =>
                    x => x.Select(transform);



                /************************************/




                [MethodImpl(Aggressive)]
                public static IEnumerable<T> AsEnumerable<T, TException>(this Result<T, TException> items)
                  where TException : Exception =>
                    items.If(
                        x => Enumerable.Repeat(x, 1),
                        _ => Enumerable.Empty<T>());

                [MethodImpl(Aggressive)]
                public static IObservable<T> AsObservable<T, TException>(this Result<T, TException> items)
                  where TException : Exception =>
                    items.If(
                        Observable.Return,
                        _ => Observable.Empty<T>());

                [MethodImpl(Aggressive)]
                public static IAsyncEnumerable<T> AsAsyncEnumerable<T, TException>(this Result<T, TException> items)
                  where TException : Exception =>
                    items.If(
                        x => AsyncEnumerable.Repeat(x, 1),
                        _ => AsyncEnumerable.Empty<T>());

                [MethodImpl(Aggressive)]
                public static Result<TResult, TException> SelectMany<TSource, TMiddle, TResult, TException>(this Result<TSource, TException> source, Func<TSource, Result<TMiddle, TException>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector)
                  where TException : Exception =>
                    source.If(
                        val =>
                            middleSelector(val)
                            .If(
                                mid => new Result<TResult, TException>(resultSelector(val, mid)),
                                err => new Result<TResult, TException>(err)),
                        err => new Result<TResult, TException>(err));

                [MethodImpl(Aggressive)]
                public static Result<TResult> SelectMany<TSource, TMiddle, TResult, TException>(this Result<TSource> source, Func<TSource, Result<TMiddle, TException>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector)
                  where TException : Exception =>
                    source.If(
                        val =>
                            middleSelector(val)
                            .If(
                                mid => new Result<TResult>(resultSelector(val, mid)),
                                err => new Result<TResult>(err)),
                        err => new Result<TResult>(err));

                [MethodImpl(Aggressive)]
                public static IEnumerable<TResult> SelectMany<TSource, TMiddle, TResult, TException>(this IEnumerable<TSource> source, Func<TSource, Result<TMiddle, TException>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector)
                  where TException : Exception =>
                    source.SelectMany(x => middleSelector(x).AsEnumerable(), resultSelector);

                [MethodImpl(Aggressive)]
                public static IObservable<TResult> SelectMany<TSource, TMiddle, TResult, TException>(this IObservable<TSource> source, Func<TSource, Result<TMiddle, TException>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector)
                  where TException : Exception =>
                    source.SelectMany(x => middleSelector(x).AsObservable(), resultSelector);

                [MethodImpl(Aggressive)]
                public static IAsyncEnumerable<TResult> SelectMany<TSource, TMiddle, TResult, TException>(this IAsyncEnumerable<TSource> source, Func<TSource, Result<TMiddle, TException>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector)
                  where TException : Exception =>
                    source.SelectMany(x => middleSelector(x).AsAsyncEnumerable(), resultSelector);

                [MethodImpl(Aggressive)]
                public static Result<TResult, TException> SelectMany<TSource, TResult, TException>(this Result<TSource, TException> source, Func<TSource, Result<TResult, TException>> transform)
                  where TException : Exception =>
                    source.If(transform, err => new Result<TResult, TException>(err));

                public static Result<TResult> SelectMany<TSource, TResult, TException>(this Result<TSource, TException> source, Func<TSource, Result<TResult>> transform)
                  where TException : Exception =>
                    source.If(transform, Error<TResult>);

                [MethodImpl(Aggressive)]
                public static Result<T> SelectMany<T, TException>(this Result<Result<T>, TException> source)
                  where TException : Exception =>
                    source.If(Id, Error<T>);

                [MethodImpl(Aggressive)]
                public static Result<T> SelectMany<T, TException>(this Result<Result<T, TException>> source)
                  where TException : Exception =>
                    source.If(val => val.If(Ok, Error<T>), Error<T>);

                [MethodImpl(Aggressive)]
                public static IEnumerable<T> SelectMany<T, TException>(this IEnumerable<Result<T, TException>> source)
                  where TException : Exception =>
                    source.SelectMany(Id, Second);

                [MethodImpl(Aggressive)]
                public static IObservable<T> SelectMany<T, TException>(this IObservable<Result<T, TException>> source)
                  where TException : Exception =>
                    source.SelectMany(Id, Second);

                [MethodImpl(Aggressive)]
                public static IAsyncEnumerable<T> SelectMany<T, TException>(this IAsyncEnumerable<Result<T, TException>> source)
                  where TException : Exception =>
                    source.SelectMany(Id, Second);

                [MethodImpl(Aggressive)]
                public static Func<Result<T1, TException>, Result<T2, TException>> Map<T1, T2, TException>(Func<T1, T2> transform)
                  where TException : Exception =>
                    x => x.Select(transform);
            }
        }
    }
}
