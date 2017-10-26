// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Buffers
{
    public static class BufferExtensions
    {
        public static bool SequenceEqual<T>(this Memory<T> first, Memory<T> second) where T : struct, IEquatable<T>
        {
            return first.Span.SequenceEqual(second.Span);
        }

        public static bool SequenceEqual<T>(this ReadOnlyMemory<T> first, ReadOnlyMemory<T> second) where T : struct, IEquatable<T>
        {
            return first.Span.SequenceEqual(second.Span);
        }

        public static int LastIndexOf(this Span<byte> buffer, ReadOnlySpan<byte> values)
        {
            return LastIndexOf((ReadOnlySpan<byte>)buffer, values);
        }

        public static int LastIndexOf(this ReadOnlySpan<byte> buffer, ReadOnlySpan<byte> values)
        {
            if (buffer.Length < values.Length) return -1;
            if (values.Length == 0) return 0;

            int candidateLength = buffer.Length;
            var firstByte = values[0];
            while (true)
            {
                int index = LastIndexOf(buffer.Slice(0, candidateLength), firstByte);
                if (index == -1) return -1;
                var slice = buffer.Slice(index);
                if (slice.StartsWith(values)) return index;
                candidateLength = index;
            }
        }

        public static int LastIndexOf(this Span<byte> buffer, byte value)
        {
            return LastIndexOf((ReadOnlySpan<byte>)buffer, value);
        }

        public static int LastIndexOf(this ReadOnlySpan<byte> buffer, byte value)
        {
            for (int i = buffer.Length - 1; i >= 0; i--)
            {
                if (buffer[i] == value) return i;
            }
            return -1;
        }
        
        public static int SequenceCompareTo(this Span<byte> left, ReadOnlySpan<byte> right)
        {
            return SequenceCompareTo((ReadOnlySpan<byte>)left, right);
        }

        public static int SequenceCompareTo(this ReadOnlySpan<byte> left, ReadOnlySpan<byte> right)
        {
            var minLength = left.Length;
            if (minLength > right.Length) minLength = right.Length;
            for (int i = 0; i < minLength; i++)
            {
                var result = left[i].CompareTo(right[i]);
                if (result != 0) return result;
            }
            return left.Length.CompareTo(right.Length);
        }
    }
}
