// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System
{
    static class Contract
    {
        public static void Abandon()
        {
            Environment.FailFast("A program error has occurred.");
        }

        public static void Assert(bool condition)
        {
            if (!condition)
            {
                Abandon();
            }
        }

        public static void Requires(bool condition)
        {
            if (!condition)
            {
                throw new ArgumentException();
            }
        }

        public static void RequiresNonNegative(int n)
        {
            if (n < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RequiresInRange(int start, int length)
        {
            if (!(start >= 0 && start < length))
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RequiresInInclusiveRange(int start, int length)
        {
            if (!(start >= 0 && start <= length))
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RequiresInInclusiveRange(int start, int end, int length)
        {
            if (!(start >= 0 && start <= end && end >= 0 && end <= length))
            {
                throw new ArgumentOutOfRangeException();
            }
        }
    }
}

