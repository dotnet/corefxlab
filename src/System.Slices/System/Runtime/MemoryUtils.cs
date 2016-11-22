// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Runtime
{
    internal static class MemoryUtils
    {
        /// <summary>
        /// platform independent fast memory comparison
        /// for x64 it is as fast as memcmp of msvcrt.dll, for x86 it is up to two times faster!!
        /// </summary>
        internal static bool MemCmp<[Primitive]T>(Span<T> first, Span<T> second)
            where T : struct
        {
            int length = first.Length;
            if (length != second.Length)
            {
                return false;
            }

            // @todo: Needs pinning support to be added to Span to restore the original compare strategy.
            for (int i = 0; i < length; i++)
            {
                if (!(first[i].Equals(second[i])))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// platform independent fast memory comparison
        /// for x64 it is as fast as memcmp of msvcrt.dll, for x86 it is up to two times faster!!
        /// </summary>
        internal static bool MemCmp<[Primitive]T>(ReadOnlySpan<T> first, ReadOnlySpan<T> second)
            where T : struct
        {
            int length = first.Length;
            if (length != second.Length)
            {
                return false;
            }

            // @todo: Needs pinning support to be added to Span to restore the original compare strategy.
            for (int i = 0; i < length; i++)
            {
                if (!(first[i].Equals(second[i])))
                    return false;
            }
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsPrimitiveValueType<T>()
        {
            // When inlined in the caller this all become a JIT time constant.
            // HACK: what about AOT scenarions?

            return typeof(T) == typeof(byte) ||
                   typeof(T) == typeof(char) ||
                   typeof(T) == typeof(sbyte) ||
                   typeof(T) == typeof(short) ||
                   typeof(T) == typeof(ushort) ||
                   typeof(T) == typeof(int) ||
                   typeof(T) == typeof(uint) ||
                   typeof(T) == typeof(long) ||
                   typeof(T) == typeof(ulong) ||
                   typeof(T) == typeof(IntPtr) ||
                   typeof(T) == typeof(UIntPtr) ||
                   typeof(T) == typeof(float) ||
                   typeof(T) == typeof(double) ||
                   typeof(T) == typeof(bool);
        }
    }
}
