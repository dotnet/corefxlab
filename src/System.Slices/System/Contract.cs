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
                throw NewArgumentException();
            }
        }

        public static void RequiresNonNegative(int n)
        {
            if (n < 0)
            {
                throw NewArgumentOutOfRangeException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RequiresInRange(int start, uint length)
        {
            if ((uint)start >= length)
            {
                throw NewArgumentOutOfRangeException();
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RequiresInRange(uint start, uint length)
        {
            if (start >= length)
            {
                throw NewArgumentOutOfRangeException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RequiresInInclusiveRange(int start, uint length)
        {
            if ((uint)start > length)
            {
                throw NewArgumentOutOfRangeException();
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RequiresInInclusiveRange(uint start, uint length)
        {
            if (start > length)
            {
                throw NewArgumentOutOfRangeException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RequiresInInclusiveRange(int start, int length, uint existingLength)
        {
            if ((uint)start > existingLength
                || length < 0
                || (uint)(start + length) > existingLength)
            {
                throw NewArgumentOutOfRangeException();
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RequiresInInclusiveRange(uint start, uint length, uint existingLength)
        {
            if (start > existingLength
                || length < 0
                || (start + length) > existingLength)
            {
                throw NewArgumentOutOfRangeException();
            }
        }

        
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Exception NewArgumentException()
        {
            return new ArgumentException();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Exception NewArgumentOutOfRangeException()
        {
            return new ArgumentOutOfRangeException();
        }
    }
}

