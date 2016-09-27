// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System
{
    static class Contract
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Requires(bool condition)
        {
            if (!condition)
            {
                ThrowArgumentException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RequiresNonNegative(int n)
        {
            if (n < 0)
            {
                ThrowArgumentOutOfRangeException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RequiresInRange(int start, uint length)
        {
            if ((uint)start >= length)
            {
                ThrowArgumentOutOfRangeException();
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RequiresInRange(uint start, uint length)
        {
            if (start >= length)
            {
                ThrowArgumentOutOfRangeException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RequiresInInclusiveRange(int start, uint length)
        {
            if ((uint)start > length)
            {
                ThrowArgumentOutOfRangeException();
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RequiresInInclusiveRange(uint start, uint length)
        {
            if (start > length)
            {
                ThrowArgumentOutOfRangeException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RequiresInInclusiveRange(int start, int length, uint existingLength)
        {
            if ((uint)start > existingLength
                || length < 0
                || (uint)(start + length) > existingLength)
            {
                ThrowArgumentOutOfRangeException();
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RequiresInInclusiveRange(uint start, uint length, uint existingLength)
        {
            if (start > existingLength
                || length < 0
                || (start + length) > existingLength)
            {
                ThrowArgumentOutOfRangeException();
            }
        }

        internal static InvalidOperationException InvalidOperationExceptionForBoxingSpans() => new InvalidOperationException("Spans must not be boxed");

        private static void ThrowArgumentException()
        {
            throw new ArgumentException();
        }

        private static void ThrowArgumentOutOfRangeException()
        {
            throw new ArgumentOutOfRangeException();
        }
    }
}

