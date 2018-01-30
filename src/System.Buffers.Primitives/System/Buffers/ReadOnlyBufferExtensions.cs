// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Buffers
{
    public static class ReadOnlyBufferExtensions
    {
        public static SequencePosition? PositionOf<T>(this ReadOnlyBuffer<T> buffer, T value) where T: IEquatable<T>
        {
            SequencePosition position = buffer.Start;
            SequencePosition result = position;
            while (buffer.TryGet(ref position, out var memory))
            {
                var index = memory.Span.IndexOf(value);
                if (index != -1)
                {
                    return buffer.GetPosition(result, index);
                }
                result = position;
            }
            return null;
        }
    }
}
