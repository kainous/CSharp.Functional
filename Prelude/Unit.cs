using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CatMath.Structures {
    public struct Unit : IEquatable<Unit> {
        public static Unit Default { get; } =
            new Unit();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Unit _) => 
            true;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) =>
            obj is Unit;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() =>
            0;

        public override string ToString() =>
            "()";

#pragma warning disable IDE0060 // Remove unused parameter
        public static bool operator ==(Unit _, Unit __) =>
            true;

        public static bool operator !=(Unit _, Unit __) =>
            false;
#pragma warning restore IDE0060 // Remove unused parameter
    }
}
