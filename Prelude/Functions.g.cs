using System;
using System.Runtime.CompilerServices;

namespace CatMath {
    public static partial class Functions {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T2> Defer<T1, T2>(this Func<T1, T2> function, T1 x1) =>
            () => function(x1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Action<T1> Ignore<T1>(Action action) =>
            (_) => action();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T1, Func<T2, T3>> Curry<T1, T2, T3>(this Func<T1, T2, T3> function) =>
          x1 => x2 => function(x1, x2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T1, T2, T3> Uncurry<T1, T2, T3>(this Func<T1, Func<T2, T3>> function) =>
          (x1, x2) => function(x1)(x2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T1, T4> ComposeWith<T1, T2, T3, T4>(this Func<T1, T2> func1, Func<T2, T3> func2, Func<T3, T4> func3) =>
            func1
            .ComposeWith(func2)
            .ComposeWith(func3);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T3> ComposeWith<T1, T2, T3>(this Func<T1> func, Func<T1, T2> func1, Func<T2, T3> func2) =>
            func
            .ComposeWith(func1)
            .ComposeWith(func2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<(T1, T2), T3> Entuple<T1, T2, T3>(this Func<T1, T2, T3> function) =>
            x => function(x.Item1, x.Item2);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T3> Defer<T1, T2, T3>(this Func<T1, T2, T3> function, T1 x1, T2 x2) =>
            () => function(x1, x2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Action<T1, T2> Ignore<T1, T2>(Action action) =>
            (_, __) => action();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T1, Func<T2, Func<T3, T4>>> Curry<T1, T2, T3, T4>(this Func<T1, T2, T3, T4> function) =>
          x1 => x2 => x3 => function(x1, x2, x3);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T1, T2, T3, T4> Uncurry<T1, T2, T3, T4>(this Func<T1, Func<T2, Func<T3, T4>>> function) =>
          (x1, x2, x3) => function(x1)(x2)(x3);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T1, T5> ComposeWith<T1, T2, T3, T4, T5>(this Func<T1, T2> func1, Func<T2, T3> func2, Func<T3, T4> func3, Func<T4, T5> func4) =>
            func1
            .ComposeWith(func2)
            .ComposeWith(func3)
            .ComposeWith(func4);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T4> ComposeWith<T1, T2, T3, T4>(this Func<T1> func, Func<T1, T2> func1, Func<T2, T3> func2, Func<T3, T4> func3) =>
            func
            .ComposeWith(func1)
            .ComposeWith(func2)
            .ComposeWith(func3);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<(T1, T2, T3), T4> Entuple<T1, T2, T3, T4>(this Func<T1, T2, T3, T4> function) =>
            x => function(x.Item1, x.Item2, x.Item3);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T4> Defer<T1, T2, T3, T4>(this Func<T1, T2, T3, T4> function, T1 x1, T2 x2, T3 x3) =>
            () => function(x1, x2, x3);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Action<T1, T2, T3> Ignore<T1, T2, T3>(Action action) =>
            (_, __, ___) => action();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T1, Func<T2, Func<T3, Func<T4, T5>>>> Curry<T1, T2, T3, T4, T5>(this Func<T1, T2, T3, T4, T5> function) =>
          x1 => x2 => x3 => x4 => function(x1, x2, x3, x4);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T1, T2, T3, T4, T5> Uncurry<T1, T2, T3, T4, T5>(this Func<T1, Func<T2, Func<T3, Func<T4, T5>>>> function) =>
          (x1, x2, x3, x4) => function(x1)(x2)(x3)(x4);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T1, T6> ComposeWith<T1, T2, T3, T4, T5, T6>(this Func<T1, T2> func1, Func<T2, T3> func2, Func<T3, T4> func3, Func<T4, T5> func4, Func<T5, T6> func5) =>
            func1
            .ComposeWith(func2)
            .ComposeWith(func3)
            .ComposeWith(func4)
            .ComposeWith(func5);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T5> ComposeWith<T1, T2, T3, T4, T5>(this Func<T1> func, Func<T1, T2> func1, Func<T2, T3> func2, Func<T3, T4> func3, Func<T4, T5> func4) =>
            func
            .ComposeWith(func1)
            .ComposeWith(func2)
            .ComposeWith(func3)
            .ComposeWith(func4);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<(T1, T2, T3, T4), T5> Entuple<T1, T2, T3, T4, T5>(this Func<T1, T2, T3, T4, T5> function) =>
            x => function(x.Item1, x.Item2, x.Item3, x.Item4);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T5> Defer<T1, T2, T3, T4, T5>(this Func<T1, T2, T3, T4, T5> function, T1 x1, T2 x2, T3 x3, T4 x4) =>
            () => function(x1, x2, x3, x4);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Action<T1, T2, T3, T4> Ignore<T1, T2, T3, T4>(Action action) =>
            (_, __, ___, ____) => action();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, T6>>>>> Curry<T1, T2, T3, T4, T5, T6>(this Func<T1, T2, T3, T4, T5, T6> function) =>
          x1 => x2 => x3 => x4 => x5 => function(x1, x2, x3, x4, x5);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T1, T2, T3, T4, T5, T6> Uncurry<T1, T2, T3, T4, T5, T6>(this Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, T6>>>>> function) =>
          (x1, x2, x3, x4, x5) => function(x1)(x2)(x3)(x4)(x5);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T1, T7> ComposeWith<T1, T2, T3, T4, T5, T6, T7>(this Func<T1, T2> func1, Func<T2, T3> func2, Func<T3, T4> func3, Func<T4, T5> func4, Func<T5, T6> func5, Func<T6, T7> func6) =>
            func1
            .ComposeWith(func2)
            .ComposeWith(func3)
            .ComposeWith(func4)
            .ComposeWith(func5)
            .ComposeWith(func6);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T6> ComposeWith<T1, T2, T3, T4, T5, T6>(this Func<T1> func, Func<T1, T2> func1, Func<T2, T3> func2, Func<T3, T4> func3, Func<T4, T5> func4, Func<T5, T6> func5) =>
            func
            .ComposeWith(func1)
            .ComposeWith(func2)
            .ComposeWith(func3)
            .ComposeWith(func4)
            .ComposeWith(func5);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<(T1, T2, T3, T4, T5), T6> Entuple<T1, T2, T3, T4, T5, T6>(this Func<T1, T2, T3, T4, T5, T6> function) =>
            x => function(x.Item1, x.Item2, x.Item3, x.Item4, x.Item5);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T6> Defer<T1, T2, T3, T4, T5, T6>(this Func<T1, T2, T3, T4, T5, T6> function, T1 x1, T2 x2, T3 x3, T4 x4, T5 x5) =>
            () => function(x1, x2, x3, x4, x5);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Action<T1, T2, T3, T4, T5> Ignore<T1, T2, T3, T4, T5>(Action action) =>
            (_, __, ___, ____, _____) => action();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, T7>>>>>> Curry<T1, T2, T3, T4, T5, T6, T7>(this Func<T1, T2, T3, T4, T5, T6, T7> function) =>
          x1 => x2 => x3 => x4 => x5 => x6 => function(x1, x2, x3, x4, x5, x6);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T1, T2, T3, T4, T5, T6, T7> Uncurry<T1, T2, T3, T4, T5, T6, T7>(this Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, T7>>>>>> function) =>
          (x1, x2, x3, x4, x5, x6) => function(x1)(x2)(x3)(x4)(x5)(x6);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T1, T8> ComposeWith<T1, T2, T3, T4, T5, T6, T7, T8>(this Func<T1, T2> func1, Func<T2, T3> func2, Func<T3, T4> func3, Func<T4, T5> func4, Func<T5, T6> func5, Func<T6, T7> func6, Func<T7, T8> func7) =>
            func1
            .ComposeWith(func2)
            .ComposeWith(func3)
            .ComposeWith(func4)
            .ComposeWith(func5)
            .ComposeWith(func6)
            .ComposeWith(func7);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T7> ComposeWith<T1, T2, T3, T4, T5, T6, T7>(this Func<T1> func, Func<T1, T2> func1, Func<T2, T3> func2, Func<T3, T4> func3, Func<T4, T5> func4, Func<T5, T6> func5, Func<T6, T7> func6) =>
            func
            .ComposeWith(func1)
            .ComposeWith(func2)
            .ComposeWith(func3)
            .ComposeWith(func4)
            .ComposeWith(func5)
            .ComposeWith(func6);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<(T1, T2, T3, T4, T5, T6), T7> Entuple<T1, T2, T3, T4, T5, T6, T7>(this Func<T1, T2, T3, T4, T5, T6, T7> function) =>
            x => function(x.Item1, x.Item2, x.Item3, x.Item4, x.Item5, x.Item6);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T7> Defer<T1, T2, T3, T4, T5, T6, T7>(this Func<T1, T2, T3, T4, T5, T6, T7> function, T1 x1, T2 x2, T3 x3, T4 x4, T5 x5, T6 x6) =>
            () => function(x1, x2, x3, x4, x5, x6);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Action<T1, T2, T3, T4, T5, T6> Ignore<T1, T2, T3, T4, T5, T6>(Action action) =>
            (_, __, ___, ____, _____, ______) => action();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, T8>>>>>>> Curry<T1, T2, T3, T4, T5, T6, T7, T8>(this Func<T1, T2, T3, T4, T5, T6, T7, T8> function) =>
          x1 => x2 => x3 => x4 => x5 => x6 => x7 => function(x1, x2, x3, x4, x5, x6, x7);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8> Uncurry<T1, T2, T3, T4, T5, T6, T7, T8>(this Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, T8>>>>>>> function) =>
          (x1, x2, x3, x4, x5, x6, x7) => function(x1)(x2)(x3)(x4)(x5)(x6)(x7);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T1, T9> ComposeWith<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Func<T1, T2> func1, Func<T2, T3> func2, Func<T3, T4> func3, Func<T4, T5> func4, Func<T5, T6> func5, Func<T6, T7> func6, Func<T7, T8> func7, Func<T8, T9> func8) =>
            func1
            .ComposeWith(func2)
            .ComposeWith(func3)
            .ComposeWith(func4)
            .ComposeWith(func5)
            .ComposeWith(func6)
            .ComposeWith(func7)
            .ComposeWith(func8);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T8> ComposeWith<T1, T2, T3, T4, T5, T6, T7, T8>(this Func<T1> func, Func<T1, T2> func1, Func<T2, T3> func2, Func<T3, T4> func3, Func<T4, T5> func4, Func<T5, T6> func5, Func<T6, T7> func6, Func<T7, T8> func7) =>
            func
            .ComposeWith(func1)
            .ComposeWith(func2)
            .ComposeWith(func3)
            .ComposeWith(func4)
            .ComposeWith(func5)
            .ComposeWith(func6)
            .ComposeWith(func7);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<(T1, T2, T3, T4, T5, T6, T7), T8> Entuple<T1, T2, T3, T4, T5, T6, T7, T8>(this Func<T1, T2, T3, T4, T5, T6, T7, T8> function) =>
            x => function(x.Item1, x.Item2, x.Item3, x.Item4, x.Item5, x.Item6, x.Item7);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T8> Defer<T1, T2, T3, T4, T5, T6, T7, T8>(this Func<T1, T2, T3, T4, T5, T6, T7, T8> function, T1 x1, T2 x2, T3 x3, T4 x4, T5 x5, T6 x6, T7 x7) =>
            () => function(x1, x2, x3, x4, x5, x6, x7);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Action<T1, T2, T3, T4, T5, T6, T7> Ignore<T1, T2, T3, T4, T5, T6, T7>(Action action) =>
            (_, __, ___, ____, _____, ______, _______) => action();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, T9>>>>>>>> Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> function) =>
          x1 => x2 => x3 => x4 => x5 => x6 => x7 => x8 => function(x1, x2, x3, x4, x5, x6, x7, x8);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> Uncurry<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, T9>>>>>>>> function) =>
          (x1, x2, x3, x4, x5, x6, x7, x8) => function(x1)(x2)(x3)(x4)(x5)(x6)(x7)(x8);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T1, T10> ComposeWith<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Func<T1, T2> func1, Func<T2, T3> func2, Func<T3, T4> func3, Func<T4, T5> func4, Func<T5, T6> func5, Func<T6, T7> func6, Func<T7, T8> func7, Func<T8, T9> func8, Func<T9, T10> func9) =>
            func1
            .ComposeWith(func2)
            .ComposeWith(func3)
            .ComposeWith(func4)
            .ComposeWith(func5)
            .ComposeWith(func6)
            .ComposeWith(func7)
            .ComposeWith(func8)
            .ComposeWith(func9);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T9> ComposeWith<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Func<T1> func, Func<T1, T2> func1, Func<T2, T3> func2, Func<T3, T4> func3, Func<T4, T5> func4, Func<T5, T6> func5, Func<T6, T7> func6, Func<T7, T8> func7, Func<T8, T9> func8) =>
            func
            .ComposeWith(func1)
            .ComposeWith(func2)
            .ComposeWith(func3)
            .ComposeWith(func4)
            .ComposeWith(func5)
            .ComposeWith(func6)
            .ComposeWith(func7)
            .ComposeWith(func8);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<(T1, T2, T3, T4, T5, T6, T7, T8), T9> Entuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> function) =>
            x => function(x.Item1, x.Item2, x.Item3, x.Item4, x.Item5, x.Item6, x.Item7, x.Item8);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T9> Defer<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> function, T1 x1, T2 x2, T3 x3, T4 x4, T5 x5, T6 x6, T7 x7, T8 x8) =>
            () => function(x1, x2, x3, x4, x5, x6, x7, x8);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Action<T1, T2, T3, T4, T5, T6, T7, T8> Ignore<T1, T2, T3, T4, T5, T6, T7, T8>(Action action) =>
            (_, __, ___, ____, _____, ______, _______, ________) => action();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, T10>>>>>>>>> Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> function) =>
          x1 => x2 => x3 => x4 => x5 => x6 => x7 => x8 => x9 => function(x1, x2, x3, x4, x5, x6, x7, x8, x9);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Uncurry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, T10>>>>>>>>> function) =>
          (x1, x2, x3, x4, x5, x6, x7, x8, x9) => function(x1)(x2)(x3)(x4)(x5)(x6)(x7)(x8)(x9);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T1, T11> ComposeWith<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Func<T1, T2> func1, Func<T2, T3> func2, Func<T3, T4> func3, Func<T4, T5> func4, Func<T5, T6> func5, Func<T6, T7> func6, Func<T7, T8> func7, Func<T8, T9> func8, Func<T9, T10> func9, Func<T10, T11> func10) =>
            func1
            .ComposeWith(func2)
            .ComposeWith(func3)
            .ComposeWith(func4)
            .ComposeWith(func5)
            .ComposeWith(func6)
            .ComposeWith(func7)
            .ComposeWith(func8)
            .ComposeWith(func9)
            .ComposeWith(func10);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T10> ComposeWith<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Func<T1> func, Func<T1, T2> func1, Func<T2, T3> func2, Func<T3, T4> func3, Func<T4, T5> func4, Func<T5, T6> func5, Func<T6, T7> func6, Func<T7, T8> func7, Func<T8, T9> func8, Func<T9, T10> func9) =>
            func
            .ComposeWith(func1)
            .ComposeWith(func2)
            .ComposeWith(func3)
            .ComposeWith(func4)
            .ComposeWith(func5)
            .ComposeWith(func6)
            .ComposeWith(func7)
            .ComposeWith(func8)
            .ComposeWith(func9);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<(T1, T2, T3, T4, T5, T6, T7, T8, T9), T10> Entuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> function) =>
            x => function(x.Item1, x.Item2, x.Item3, x.Item4, x.Item5, x.Item6, x.Item7, x.Item8, x.Item9);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T10> Defer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> function, T1 x1, T2 x2, T3 x3, T4 x4, T5 x5, T6 x6, T7 x7, T8 x8, T9 x9) =>
            () => function(x1, x2, x3, x4, x5, x6, x7, x8, x9);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> Ignore<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action action) =>
            (_, __, ___, ____, _____, ______, _______, ________, _________) => action();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, T11>>>>>>>>>> Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> function) =>
          x1 => x2 => x3 => x4 => x5 => x6 => x7 => x8 => x9 => x10 => function(x1, x2, x3, x4, x5, x6, x7, x8, x9, x10);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Uncurry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, Func<T10, T11>>>>>>>>>> function) =>
          (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10) => function(x1)(x2)(x3)(x4)(x5)(x6)(x7)(x8)(x9)(x10);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T1, T12> ComposeWith<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Func<T1, T2> func1, Func<T2, T3> func2, Func<T3, T4> func3, Func<T4, T5> func4, Func<T5, T6> func5, Func<T6, T7> func6, Func<T7, T8> func7, Func<T8, T9> func8, Func<T9, T10> func9, Func<T10, T11> func10, Func<T11, T12> func11) =>
            func1
            .ComposeWith(func2)
            .ComposeWith(func3)
            .ComposeWith(func4)
            .ComposeWith(func5)
            .ComposeWith(func6)
            .ComposeWith(func7)
            .ComposeWith(func8)
            .ComposeWith(func9)
            .ComposeWith(func10)
            .ComposeWith(func11);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T11> ComposeWith<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Func<T1> func, Func<T1, T2> func1, Func<T2, T3> func2, Func<T3, T4> func3, Func<T4, T5> func4, Func<T5, T6> func5, Func<T6, T7> func6, Func<T7, T8> func7, Func<T8, T9> func8, Func<T9, T10> func9, Func<T10, T11> func10) =>
            func
            .ComposeWith(func1)
            .ComposeWith(func2)
            .ComposeWith(func3)
            .ComposeWith(func4)
            .ComposeWith(func5)
            .ComposeWith(func6)
            .ComposeWith(func7)
            .ComposeWith(func8)
            .ComposeWith(func9)
            .ComposeWith(func10);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10), T11> Entuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> function) =>
            x => function(x.Item1, x.Item2, x.Item3, x.Item4, x.Item5, x.Item6, x.Item7, x.Item8, x.Item9, x.Item10);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<T11> Defer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> function, T1 x1, T2 x2, T3 x3, T4 x4, T5 x5, T6 x6, T7 x7, T8 x8, T9 x9, T10 x10) =>
            () => function(x1, x2, x3, x4, x5, x6, x7, x8, x9, x10);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Ignore<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action action) =>
            (_, __, ___, ____, _____, ______, _______, ________, _________, __________) => action();

    }
}