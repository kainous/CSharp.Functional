using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Maat.Functional.Structures;
using Maat.Functional.Structures.Linq;

namespace Maat.Functional {
    public static class EnumerableExtensions {
        public static async IAsyncEnumerable<T> OnErrorResumeNext<T>(this IAsyncEnumerable<T> items, [EnumeratorCancellation] CancellationToken cancellationToken = default) {
            await using var e = items.GetAsyncEnumerator(cancellationToken);

            async ValueTask<bool> TryGetNext() {
                try {
                    return
                        !cancellationToken.IsCancellationRequested
                        && await e.MoveNextAsync();
                }
                catch {
                    return false;
                }
            }

            while (await TryGetNext()) {
                var item = Opt.Try(() => e.Current);
                if (item.HasValue) {
                    yield return item.Value!;
                }
            }
        }

        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action) {
            foreach (var item in items) {
                action(item);
            }
        }

        public static IEnumerable<TResult> SelectWithTry<T, TResult>(this IEnumerable<T> items, Func<T, TResult> selector, Action<Exception>? exceptionAction = null, Action? finalAction = null) {
            foreach (var item in items) {
                bool successful = false;
                TResult? result = default;
                try {
                    result = selector(item);
                    successful = true;
                }
                catch (Exception ex) {
                    exceptionAction?.Invoke(ex);
                }
                finally {
                    finalAction?.Invoke();
                }
                if (successful) {
                    yield return result!;
                }
            }
        }

        public static IEnumerable<T> Do<T>(this IEnumerable<T> items, Action<T> action) {
            foreach (var item in items) {
                action(item);
                yield return item;
            }
        }

        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action, Action<Exception>? exceptionAction = null, Action? finalAction = null) {
            foreach (var item in items) {
                try {
                    action(item);
                }
                catch (Exception ex) {
                    exceptionAction?.Invoke(ex);
                }
                finally {
                    finalAction?.Invoke();
                }
            }
        }
    }
}
