// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Sequences;

namespace System.Buffers
{
    public static class VariousExtensions
    {
        public static ArraySegment<T> Slice<T>(this ArraySegment<T> source, int count)
        {
            var result = new ArraySegment<T>(source.Array, source.Offset + count, source.Count - count);
            return result;
        }

        public static ReadOnlySpan<byte> ToSingleSpan<T>(this T memorySequence) where T : ISequence<ReadOnlyMemory<byte>>
        {
            Position position = Position.First;
            ReadOnlyMemory<byte> memory;
            ResizableArray<byte> array = new ResizableArray<byte>(1024); // TODO: could this be rented from a pool?
            while (memorySequence.TryGet(ref position, out memory, advance: true)) {
                array.AddAll(memory.Span);
            }
            return array.Items.Slice(0, array.Count);
        }
    }
}
