// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Sequences;

namespace System.Buffers
{
    public static class SequenceExtensions
    {
        public static long IndexOf(this ISequence<ReadOnlyMemory<byte>> sequence, byte value)
        {
            Position position = default;
            int totalIndex = 0;
            while (sequence.TryGet(ref position, out ReadOnlyMemory<byte> memory))
            {
                var index = memory.Span.IndexOf(value);
                if (index != -1) return index + totalIndex;
                totalIndex += memory.Length;
            }
            return -1;
        }

        public static Position PositionOf(this IMemoryList<byte> sequence, byte value)
        {
            while (sequence != null)
            {
                var index = sequence.Memory.Span.IndexOf(value);
                if (index != -1) return Position.Create(index, sequence);
                sequence = sequence.Rest;
            }
            return Position.End;
        }
    }
}
