using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Maat.Functional.Structures;

public struct NotUsed : IEquatable<NotUsed> {
    public static NotUsed Default { get; } =
        new NotUsed();

    [MethodImpl(Functions.AggressiveOptimization)]
    public bool Equals(NotUsed _) => 
        true;

    [MethodImpl(Functions.AggressiveOptimization)]
    public override bool Equals(object? obj) =>
        obj is NotUsed;

    [MethodImpl(Functions.AggressiveOptimization)]
    public override int GetHashCode() =>
        0;

    public override string ToString() =>
        "()";

#pragma warning disable IDE0060 // Remove unused parameter
    public static bool operator ==(NotUsed _, NotUsed __) =>
        true;

    public static bool operator !=(NotUsed _, NotUsed __) =>
        false;
#pragma warning restore IDE0060 // Remove unused parameter
}
