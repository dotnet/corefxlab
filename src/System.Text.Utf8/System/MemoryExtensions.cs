// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
    internal static class MemoryExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<byte> DangerousSliceWithoutValidation(this ReadOnlySpan<byte> span, int start)
        {
            Debug.Assert((uint)start <= (uint)span.Length);
            return MemoryMarshal.CreateReadOnlySpan(ref Unsafe.Add(ref MemoryMarshal.GetReference(span), start), span.Length - start);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<byte> DangerousSliceWithoutValidation(this ReadOnlySpan<byte> span, int start, int length)
        {
            Debug.Assert((uint)start <= (uint)span.Length);
            Debug.Assert((uint)length <= (uint)(span.Length - start));
            return MemoryMarshal.CreateReadOnlySpan(ref Unsafe.Add(ref MemoryMarshal.GetReference(span), start), length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<byte> DangerousSliceWithoutValidation(this Span<byte> span, int start)
        {
            Debug.Assert((uint)start <= (uint)span.Length);
            return MemoryMarshal.CreateReadOnlySpan(ref Unsafe.Add(ref MemoryMarshal.GetReference(span), start), span.Length - start);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<byte> DangerousSliceWithoutValidation(this Span<byte> span, int start, int length)
        {
            Debug.Assert((uint)start <= (uint)span.Length);
            Debug.Assert((uint)length <= (uint)(span.Length - start));
            return MemoryMarshal.CreateReadOnlySpan(ref Unsafe.Add(ref MemoryMarshal.GetReference(span), start), length);
        }
    }
}
