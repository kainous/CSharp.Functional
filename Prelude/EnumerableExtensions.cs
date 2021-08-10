using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CatMath {
    public static class EnumerableExtensions {

    }

    public static class Ord {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static T Max<T>(this IComparer<T> comparer, params T[] items) {
            if (items.Length == 0) {
                throw new ArgumentException("items must be non-empty", nameof(items));
            }

            var current = items[0];
            // Do not optimize items.Length out of this loop, or the compiler will lose
            // context and begin testing bounds every iteration
            for (int i = 1; i < items.Length; i++) {
                var item = items[i];
                current =
                    comparer.Compare(current, item) > 0
                    ? item
                    : current;
            }

            return current;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static T Max<T>(this IEnumerable<T> items, IComparer<T> comparer) =>
            items.Aggregate((acc, elem) => comparer.Compare(acc, elem) > 0 ? acc : elem);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static T Min<T>(this IComparer<T> comparer, params T[] items) {
            if (items.Length == 0) {
                throw new ArgumentException("items must be non-empty", nameof(items));
            }

            var current = items[0];
            // Do not optimize items.Length out of this loop, or the compiler will lose
            // context and begin testing bounds every iteration
            for (int i = 1; i < items.Length; i++) {
                var item = items[i];
                current =
                    comparer.Compare(current, item) < 0
                    ? item
                    : current;
            }

            return current;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static T Min<T>(this IEnumerable<T> items, IComparer<T> comparer) =>
            items.Aggregate((acc, elem) => comparer.Compare(acc, elem) < 0 ? acc : elem);
    }
}
