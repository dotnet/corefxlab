// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Sequences;

namespace System.Buffers
{
    public static class SequenceExtensions
    {
        // TODO: this cannot be an extension method (as I would like iot to be).
        // If I make it an extensions method, the compiler complains Span<T> cannot
        // be used as a type parameter.
        public static long IndexOf<TSequence>(TSequence sequence, byte value) where TSequence : ISequence<ReadOnlyMemory<byte>>
        {
            Position position = default;
            int totalIndex = 0;
            while (sequence.TryGet(ref position, out ReadOnlyMemory<byte> memory))
            {
                var index = MemoryExtensions.IndexOf(memory.Span, value);
                if (index != -1) return index + totalIndex;
                totalIndex += memory.Length;
            }
            return -1;
        }

        public static Position PositionOf<TSequence>(this TSequence sequence, byte value) where TSequence : ISequence<ReadOnlyMemory<byte>>
        {
            if (sequence == null) return Position.End;

            Position position = sequence.First;
            Position result = position;
            while (sequence.TryGet(ref position, out ReadOnlyMemory<byte> memory))
            {
                var index = MemoryExtensions.IndexOf(memory.Span, value);
                if (index != -1)
                {
                    result.Index += index;
                    return result;
                }
                result = position;
            }
            return Position.End;
        }
    }
}
