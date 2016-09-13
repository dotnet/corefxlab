﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
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
            if (first.Length != second.Length)
            {
                return false;
            }

            unsafe
            {
                // prevent GC from moving memory
                var firstPinnedHandle = GCHandle.Alloc(first.Object, GCHandleType.Pinned);
                var secondPinnedHandle = GCHandle.Alloc(second.Object, GCHandleType.Pinned);

                try
                {
                    byte* firstPointer = ComputeAddress(first.Object, first.Offset);
                    byte* secondPointer = ComputeAddress(second.Object, second.Offset);

                    int step = sizeof(void*) * 5;
                    int totalBytesCount = first.Length * Unsafe.SizeOf<T>();
                    byte* firstPointerLimit = firstPointer + (totalBytesCount - step);

                    if (totalBytesCount > step)
                    {
                        while (firstPointer < firstPointerLimit)
                        {   // IMPORTANT: in order to get HUGE benefits of loop unrolling on x86 we use break instead of return
                            if (*((void**)firstPointer + 0) != *((void**)secondPointer + 0)) break;
                            if (*((void**)firstPointer + 1) != *((void**)secondPointer + 1)) break;
                            if (*((void**)firstPointer + 2) != *((void**)secondPointer + 2)) break;
                            if (*((void**)firstPointer + 3) != *((void**)secondPointer + 3)) break;
                            if (*((void**)firstPointer + 4) != *((void**)secondPointer + 4)) break;

                            firstPointer += step;
                            secondPointer += step;
                        }
                        if (firstPointer < firstPointerLimit) // the upper loop ended with break;
                        {
                            return false;
                        }
                    }
                    firstPointerLimit += step; // lets check the remaining bytes
                    while (firstPointer < firstPointerLimit)
                    {
                        if (*firstPointer != *secondPointer) break;

                        ++firstPointer;
                        ++secondPointer;
                    }

                    return firstPointer == firstPointerLimit;
                }
                finally
                {
                    if (firstPinnedHandle.IsAllocated)
                    {
                        firstPinnedHandle.Free();
                    }
                    if (secondPinnedHandle.IsAllocated)
                    {
                        secondPinnedHandle.Free();
                    }
                }
            }
        }

        /// <summary>
        /// platform independent fast memory comparison
        /// for x64 it is as fast as memcmp of msvcrt.dll, for x86 it is up to two times faster!!
        /// </summary>
        internal static bool MemCmp<[Primitive]T>(ReadOnlySpan<T> first, ReadOnlySpan<T> second)
            where T : struct
        {
            if (first.Length != second.Length)
            {
                return false;
            }

            unsafe
            {
                // prevent GC from moving memory
                var firstPinnedHandle = GCHandle.Alloc(first.Object, GCHandleType.Pinned);
                var secondPinnedHandle = GCHandle.Alloc(second.Object, GCHandleType.Pinned);

                try
                {
                    byte* firstPointer = ComputeAddress(first.Object, first.Offset);
                    byte* secondPointer = ComputeAddress(second.Object, second.Offset);

                    int step = sizeof(void*) * 5;
                    int totalBytesCount = first.Length * Unsafe.SizeOf<T>();
                    byte* firstPointerLimit = firstPointer + (totalBytesCount - step);

                    if (totalBytesCount > step)
                    {
                        while (firstPointer < firstPointerLimit)
                        {   // IMPORTANT: in order to get HUGE benefits of loop unrolling on x86 we use break instead of return
                            if (*((void**)firstPointer + 0) != *((void**)secondPointer + 0)) break;
                            if (*((void**)firstPointer + 1) != *((void**)secondPointer + 1)) break;
                            if (*((void**)firstPointer + 2) != *((void**)secondPointer + 2)) break;
                            if (*((void**)firstPointer + 3) != *((void**)secondPointer + 3)) break;
                            if (*((void**)firstPointer + 4) != *((void**)secondPointer + 4)) break;

                            firstPointer += step;
                            secondPointer += step;
                        }
                        if (firstPointer < firstPointerLimit) // the upper loop ended with break;
                        {
                            return false;
                        }
                    }
                    firstPointerLimit += step; // lets check the remaining bytes
                    while (firstPointer < firstPointerLimit)
                    {
                        if (*firstPointer != *secondPointer) break;

                        ++firstPointer;
                        ++secondPointer;
                    }

                    return firstPointer == firstPointerLimit;
                }
                finally
                {
                    if (firstPinnedHandle.IsAllocated)
                    {
                        firstPinnedHandle.Free();
                    }
                    if (secondPinnedHandle.IsAllocated)
                    {
                        secondPinnedHandle.Free();
                    }
                }
            }
        }

        private static unsafe byte* ComputeAddress(object obj, UIntPtr offset)
        {
            // obj must be pinned 
            return *(byte**)Unsafe.AsPointer(ref obj) + offset.ToUInt64();
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