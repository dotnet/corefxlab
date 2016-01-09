// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System
{
    static class Contract
    {
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
        public static void RequiresInRange(int start, int length)
        {
            if ((uint)start >= (uint)length)
            {
                throw NewArgumentOutOfRangeException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RequiresInInclusiveRange(int start, int length)
        {
            if ((uint)start > (uint)length)
            {
                throw NewArgumentOutOfRangeException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RequiresInInclusiveRange(int start, int length, int existingLength)
        {
            if ((uint)start > (uint)existingLength
                || length < 0
                || (uint)(start + length) > (uint)existingLength)
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

