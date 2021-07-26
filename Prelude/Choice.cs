﻿using System;
using System.Runtime.CompilerServices;

namespace CatMath.Structures {

    // Either cannot be a struct, because structs allow for empty constructor
    // Either as a class may allow for null, therefore it's recommended that you use C# LangVersion 8 or above
    // In order to verify that you are not using null here
    public sealed class Choice<TThis, TThat> {
#pragma warning disable CS8601 // Possible null reference
        private readonly TThis _thisOne = default;
        private readonly TThat _thatOne = default;
#pragma warning restore CS8601 // Possible null reference
        private readonly bool _isThis = false;

        public Choice(TThis value) {
            _isThis = true;
            _thisOne = value;
        }

        public Choice(TThat value) {
            _thatOne = value;
        }

        [MethodImpl(Functions.Aggressive)]
        public T If<T>(Func<TThis, T> thisFunction, Func<TThat, T> thatFunction) =>
            // Constructors prohibit the case that both are set, or neither are set
            _isThis ? thisFunction(_thisOne) : thatFunction(_thatOne);
    }

    namespace Linq {
        public static class Choice {
            [MethodImpl(Functions.Aggressive)]
            public static Choice<TOut1, TOut2> Select<TIn1, TIn2, TOut1, TOut2>(this Choice<TIn1, TIn2> value, Func<TIn1, TOut1> transform1, Func<TIn2, TOut2> transform2) =>
                value.If(
                    x => new Choice<TOut1, TOut2>(transform1(x)),
                    y => new Choice<TOut1, TOut2>(transform2(y)));

            [MethodImpl(Functions.Aggressive)]
            public static Result<T, TException> AsResult<T, TException>(this Choice<T, TException> value) where TException : Exception =>
                value.If(
                    x => new Result<T, TException>(x),
                    y => new Result<T, TException>(y));
        }
    }
}
