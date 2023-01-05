using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Maat.Functional.Structures;

namespace Maat.Collections; 
public sealed class AsyncCache<TKey, TValue> : IAsyncDisposable {
    private readonly CancellationTokenSource _cts = new();
    private readonly ConcurrentDictionary<TKey, Lazy<Task<TValue>>> _cache;

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static Lazy<Task<TValue>> Wrap(TValue value) =>
        new(() => Task.FromResult(value));

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static KeyValuePair<TKey, Lazy<Task<TValue>>> Wrap(KeyValuePair<TKey, TValue> pair) =>
        new(pair.Key, Wrap(pair.Value));

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static IEnumerable<KeyValuePair<TKey, Lazy<Task<TValue>>>> Wrap(IEnumerable<KeyValuePair<TKey, TValue>> pairs) =>
        pairs.Select(Wrap);

    public AsyncCache(int concurrencyLevel, int capacity) => _cache = new(concurrencyLevel, capacity);
    public AsyncCache() => _cache = new();
    public AsyncCache(IEnumerable<KeyValuePair<TKey, TValue>> items) => _cache = new(Wrap(items));
    public AsyncCache(IEnumerable<KeyValuePair<TKey, TValue>> items, IEqualityComparer<TKey> comparer) => _cache = new(Wrap(items), comparer);
    public AsyncCache(IEqualityComparer<TKey> comparer) => _cache = new(comparer);

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public Task<TValue> AddOrUpdate(TKey key, Func<TKey, Task<TValue>> addFunction, Func<TKey, TValue, Task<TValue>> updateFunction) =>
        _cache.AddOrUpdate(key, k => new(() => addFunction(k)), (k, prev) => new(async () => await updateFunction(k, await prev.Value))).Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public Task<TValue> GetOrAdd(TKey key, Func<TKey, Task<TValue>> addFunction) =>
        _cache.GetOrAdd(key, new Lazy<Task<TValue>>(() => addFunction(key))).Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public async Task<Opt<TValue>> TryGetValue(TKey key) =>
        _cache.TryGetValue(key, out var result)
        ? new(await result.Value)
        : default;

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public async Task<IReadOnlyDictionary<TKey, TValue>> AsReadOnlyDictionary() {
        var tasks = _cache.Select(async pair => KeyValuePair.Create(pair.Key, await pair.Value.Value));
        var result = await Task.WhenAll(tasks);
        return new Dictionary<TKey, TValue>(result);
    }

    public async ValueTask DisposeAsync() {
        _cts.Cancel();
        await AsReadOnlyDictionary();
    }
}
