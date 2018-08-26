// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
    public static class VariantExtensions
    {
        // These are here as extension methods to facilitate experimenting with making the structs
        // not readonly and passing by ref "implicitly" (as opposed to in).

        public static unsafe ReadOnlySpan<Variant> ToSpan(in this Variant variant)
            => MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in variant), 1);

        public static ReadOnlySpan<Variant> ToSpan(in this Variant2 variant)
            => MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in variant.First), 2);

        public static ReadOnlySpan<Variant> ToSpan(in this Variant3 variant)
            => MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in variant.First), 3);
    }
}
