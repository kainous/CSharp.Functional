using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Maat.Functional.Structures;

namespace Maat.Functional {
    using static Functions;
    static partial class Functions {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Result<T> Ok<T>(T value) =>
            new SuccessCase<T>(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Result<T> Error<T>(Exception ex) =>
            new FailureCase<T>(ex);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Result<TResult, TException> Try<TResult, TException>(Func<TResult> tryFunction, Action<TResult>? finallyFunction = null)
          where TException : Exception {
            TResult result = default!;
            try {
                result = tryFunction();
            }
            catch (TException ex) {
                return new FailureCase<TResult, TException>(ex);
            }
            finally {
                finallyFunction?.Invoke(result);
            }
            return new SuccessCase<TResult, TException>(result);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Result<T> Try<T>(Func<T> tryFunction, Action<T>? finallyFunction = null) {
            T result = default!;
            try {
                result = tryFunction();
            }
            catch (Exception ex) {
                return new FailureCase<T>(ex);
            }
            finally {
                finallyFunction?.Invoke(result);
            }

            return new SuccessCase<T>(result);
        }
    }

    namespace Structures {
        internal sealed class SuccessCase<T, TException>
            : Result<T, TException>
            where TException : Exception {

            private readonly T _value;
            public SuccessCase(T value) =>
                _value = value;

            public override TResult If<TResult>(Func<T, TResult> successFunction, Func<TException, TResult> failureFunction) =>
                successFunction(_value);
        }

        internal sealed class FailureCase<T, TException>
          : Result<T, TException>
          where TException : Exception {
            private readonly TException _exception;

            public FailureCase(TException exception) =>
                _exception = exception;

            public override TResult If<TResult>(Func<T, TResult> successFunction, Func<TException, TResult> failureFunction) =>
                failureFunction(_exception);
        }

        public abstract class Result<T, TException>
          where TException : Exception {
            [MethodImpl(AggressiveOptimization)]
            public abstract TResult If<TResult>(Func<T, TResult> successFunction, Func<TException, TResult> failureFunction);

            [MethodImpl(AggressiveOptimization)]
            public Result<T, TException> OrElse(Result<T, TException> alternateValue) =>
                If(_ => this, _ => alternateValue);

            [MethodImpl(AggressiveOptimization)]
            public static Result<T, TException> operator |(Result<T, TException> value, Result<T, TException> alternateValue) =>
                value.If(_ => value, _ => alternateValue);

            [MethodImpl(AggressiveOptimization)]
            public T OrElse(T alternateValue) =>
                If(Id, _ => alternateValue);

            [MethodImpl(AggressiveOptimization)]
            public static T operator |(Result<T, TException> value, T alternateValue) =>
                value.If(Id, _ => alternateValue);
        }

        internal sealed class SuccessCase<T> : Result<T> {
            private readonly T _value;
            public SuccessCase(T value) =>
                _value = value;

            public override TResult If<TResult>(Func<T, TResult> successFunction, Func<Exception, TResult> failureFunction) =>
                successFunction(_value);
        }

        internal sealed class FailureCase<T> : Result<T> {
            private readonly Exception _exception;

            public FailureCase(Exception exception) =>
                _exception = exception;

            public override TResult If<TResult>(Func<T, TResult> successFunction, Func<Exception, TResult> failureFunction) =>
                failureFunction(_exception);
        }

        public abstract class Result<T> {
            [MethodImpl(AggressiveOptimization)]
            public abstract TResult If<TResult>(Func<T, TResult> successFunction, Func<Exception, TResult> failureFunction);

            [MethodImpl(AggressiveOptimization)]
            public Result<T> OrElse(Result<T> alternateValue) =>
                If(_ => this, _ => alternateValue);

            [MethodImpl(AggressiveOptimization)]
            public T OrElse(T alternateValue) =>
                If(Id, _ => alternateValue);

            [MethodImpl(AggressiveOptimization)]
            public static Result<T> operator |(Result<T> value, Result<T> alternateValue) =>
                value.If(_ => value, _ => alternateValue);

            [MethodImpl(AggressiveOptimization)]
            public static T operator |(Result<T> value, T alternateValue) =>
                value.If(Id, _ => alternateValue);
        }

        namespace Linq {
            public static class Result {
                [MethodImpl(AggressiveOptimization)]
                public static Result<TResult> ForgetExceptionType<TResult, TException>(this Result<TResult, TException> result)
                  where TException : Exception =>
                    result.If(Ok, Error<TResult>);

                [MethodImpl(AggressiveOptimization)]
                public static Opt<T> ToOpt<T>(this Result<T> result) =>
                    result.If(OneOf, _ => None<T>());

                [MethodImpl(AggressiveOptimization)]
                public static Opt<T> ToOpt<T, TException>(this Result<T, TException> result)
                  where TException : Exception =>
                    result.If(OneOf, _ => None<T>());

                [MethodImpl(AggressiveOptimization)]
                public static Result<TResult, TException> Select<TSource, TResult, TException>(this Result<TSource, TException> result, Func<TSource, TResult> transform)
                  where TException : Exception =>
                    result.If<Result<TResult, TException>>(
                        val => new SuccessCase<TResult, TException>(transform(val)),
                        err => new FailureCase<TResult, TException>(err));

                [MethodImpl(AggressiveOptimization)]
                public static Result<TResult, TExceptionOut> Select<TSource, TResult, TExceptionIn, TExceptionOut>(this Result<TSource, TExceptionIn> result, Func<TSource, TResult> transform, Func<TExceptionIn, TExceptionOut> exceptionTransformer)
                  where TExceptionIn : Exception
                  where TExceptionOut : Exception =>
                    result.If<Result<TResult, TExceptionOut>>(
                        val => new SuccessCase<TResult, TExceptionOut>(transform(val)),
                        err => new FailureCase<TResult, TExceptionOut>(exceptionTransformer(err)));

                [MethodImpl(AggressiveOptimization)]
                public static Result<TResult> Select<TSource, TResult>(this Result<TSource> result, Func<TSource, TResult> transform) =>
                    result.If<Result<TResult>>(
                        val => new SuccessCase<TResult>(transform(val)),
                        err => new FailureCase<TResult>(err));

                [MethodImpl(AggressiveOptimization)]
                public static Result<T, TException> SelectMany<T, TException>(this Result<Result<T, TException>, TException> result)
                  where TException : Exception =>
                    result.If(
                        Id,
                        err => new FailureCase<T, TException>(err));


                [MethodImpl(AggressiveOptimization)]
                public static IEnumerable<T> AsEnumerable<T>(this Result<T> items) =>
                    items.If(
                        x => Enumerable.Repeat(x, 1),
                        _ => Enumerable.Empty<T>());


                [MethodImpl(AggressiveOptimization)]
                public static Result<TResult> SelectMany<TSource, TMiddle, TResult>(this Result<TSource> source, Func<TSource, Result<TMiddle>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector) =>
                    source.If(
                        val =>
                            middleSelector(val)
                            .If(mid => Ok(resultSelector(val, mid)), Error<TResult>),
                        Error<TResult>);

                [MethodImpl(AggressiveOptimization)]
                public static IEnumerable<TResult> SelectMany<TSource, TMiddle, TResult>(this IEnumerable<TSource> source, Func<TSource, Result<TMiddle>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector) =>
                    source.SelectMany(x => middleSelector(x).AsEnumerable(), resultSelector);

                //[MethodImpl(AggressiveOptimization)]
                //public static IObservable<TResult> SelectMany<TSource, TMiddle, TResult>(this IObservable<TSource> source, Func<TSource, Result<TMiddle>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector) =>
                //    source.SelectMany(x => middleSelector(x).If(x => Observable.Return(x),_=>Observable.Empty<TMiddle>()), resultSelector);

                //[MethodImpl(AggressiveOptimization)]
                //public static IAsyncEnumerable<TResult> SelectMany<TSource, TMiddle, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, Result<TMiddle>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector) =>
                //    source.SelectMany(x => middleSelector(x).If(x => AsyncEnumerable.Repeat(x,1), _ => AsyncEnumerable.Empty<T>()), resultSelector);

                [MethodImpl(AggressiveOptimization)]
                public static Result<TResult> SelectMany<TSource, TResult>(this Result<TSource> source, Func<TSource, Result<TResult>> transform) =>
                    source.If(transform, Error<TResult>);

                [MethodImpl(AggressiveOptimization)]
                public static Result<T> SelectMany<T>(this Result<Result<T>> source) =>
                    source.If(Id, Error<T>);

                [MethodImpl(AggressiveOptimization)]
                public static IEnumerable<T> SelectMany<T>(this IEnumerable<Result<T>> source) =>
                    source.SelectMany(Id, Second);

                //[MethodImpl(AggressiveOptimization)]
                //public static IObservable<T> SelectMany<T>(this IObservable<Result<T>> source) =>
                //    source.SelectMany(Id, Second);
                //
                //[MethodImpl(AggressiveOptimization)]
                //public static IAsyncEnumerable<T> SelectMany<T>(this IAsyncEnumerable<Result<T>> source) =>
                //    source.SelectMany(Id, Second);

                [MethodImpl(AggressiveOptimization)]
                public static Func<Result<T1>, Result<T2>> Map<T1, T2>(Func<T1, T2> transform) =>
                    x => x.Select(transform);



                /************************************/




                [MethodImpl(AggressiveOptimization)]
                public static IEnumerable<T> AsEnumerable<T, TException>(this Result<T, TException> items)
                  where TException : Exception =>
                    items.If(
                        x => Enumerable.Repeat(x, 1),
                        _ => Enumerable.Empty<T>());


                [MethodImpl(AggressiveOptimization)]
                public static Result<TResult, TException> SelectMany<TSource, TMiddle, TResult, TException>(this Result<TSource, TException> source, Func<TSource, Result<TMiddle, TException>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector)
                  where TException : Exception =>
                    source.If(
                        val =>
                            middleSelector(val)
                            .If<TResult>(
                                mid => new SuccessCase<TResult, TException>(resultSelector(val, mid)),
                                err => new FailureCase<TResult, TException>(err)),
                        err => new FailureCase<TResult, TException>(err));

                [MethodImpl(AggressiveOptimization)]
                public static Result<TResult> SelectMany<TSource, TMiddle, TResult, TException>(this Result<TSource> source, Func<TSource, Result<TMiddle, TException>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector)
                  where TException : Exception =>
                    source.If(
                        val =>
                            middleSelector(val)
                            .If(
                                mid => new Result<TResult>(resultSelector(val, mid)),
                                err => new Result<TResult>(err)),
                        err => new Result<TResult>(err));

                [MethodImpl(AggressiveOptimization)]
                public static IEnumerable<TResult> SelectMany<TSource, TMiddle, TResult, TException>(this IEnumerable<TSource> source, Func<TSource, Result<TMiddle, TException>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector)
                  where TException : Exception =>
                    source.SelectMany(x => middleSelector(x).AsEnumerable(), resultSelector);

                //[MethodImpl(AggressiveOptimization)]
                //public static IObservable<TResult> SelectMany<TSource, TMiddle, TResult, TException>(this IObservable<TSource> source, Func<TSource, Result<TMiddle, TException>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector)
                //  where TException : Exception =>
                //    source.SelectMany(x => middleSelector(x).AsObservable(), resultSelector);
                //
                //[MethodImpl(AggressiveOptimization)]
                //public static IAsyncEnumerable<TResult> SelectMany<TSource, TMiddle, TResult, TException>(this IAsyncEnumerable<TSource> source, Func<TSource, Result<TMiddle, TException>> middleSelector, Func<TSource, TMiddle, TResult> resultSelector)
                //  where TException : Exception =>
                //    source.SelectMany(x => middleSelector(x).AsAsyncEnumerable(), resultSelector);

                [MethodImpl(AggressiveOptimization)]
                public static Result<TResult, TException> SelectMany<TSource, TResult, TException>(this Result<TSource, TException> source, Func<TSource, Result<TResult, TException>> transform)
                  where TException : Exception =>
                    source.If(transform, err => new Result<TResult, TException>(err));

                public static Result<TResult> SelectMany<TSource, TResult, TException>(this Result<TSource, TException> source, Func<TSource, Result<TResult>> transform)
                  where TException : Exception =>
                    source.If(transform, Error<TResult>);

                [MethodImpl(AggressiveOptimization)]
                public static Result<T> SelectMany<T, TException>(this Result<Result<T>, TException> source)
                  where TException : Exception =>
                    source.If(Id, Error<T>);

                [MethodImpl(AggressiveOptimization)]
                public static Result<T> SelectMany<T, TException>(this Result<Result<T, TException>> source)
                  where TException : Exception =>
                    source.If(val => val.If(Ok, Error<T>), Error<T>);

                [MethodImpl(AggressiveOptimization)]
                public static IEnumerable<T> SelectMany<T, TException>(this IEnumerable<Result<T, TException>> source)
                  where TException : Exception =>
                    source.SelectMany(Id, Second);

                //[MethodImpl(AggressiveOptimization)]
                //public static IObservable<T> SelectMany<T, TException>(this IObservable<Result<T, TException>> source)
                //  where TException : Exception =>
                //    source.SelectMany(Id, Second);
                //
                //[MethodImpl(AggressiveOptimization)]
                //public static IAsyncEnumerable<T> SelectMany<T, TException>(this IAsyncEnumerable<Result<T, TException>> source)
                //  where TException : Exception =>
                //    source.SelectMany(Id, Second);

                [MethodImpl(AggressiveOptimization)]
                public static Func<Result<T1, TException>, Result<T2, TException>> Map<T1, T2, TException>(Func<T1, T2> transform)
                  where TException : Exception =>
                    x => x.Select(transform);
            }
        }
    }
}
