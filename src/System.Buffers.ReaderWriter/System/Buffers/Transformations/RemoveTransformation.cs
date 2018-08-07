// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Buffers.Operations
{
    public class RemoveTransformation : IBufferTransformation
    {
        private byte _value;
        public RemoveTransformation(byte valueToRemove)
        {
            _value = valueToRemove;
        }
        public OperationStatus Execute(ReadOnlySpan<byte> input, Span<byte> output, out int consumed, out int written)
        {
            written = 0;
            for (consumed = 0; consumed < input.Length; consumed++)
            {
                if (input[consumed] == _value) continue;
                if (written >= output.Length) return OperationStatus.DestinationTooSmall;
                output[written++] = input[consumed];
            }
            return OperationStatus.Done;
        }

        public OperationStatus Transform(Span<byte> buffer, int dataLength, out int written)
        {
            written = 0;
            for (int consumed = 0; consumed < dataLength; consumed++)
            {
                if (buffer[consumed] == _value) continue;
                if (written >= buffer.Length) return OperationStatus.DestinationTooSmall;
                buffer[written++] = buffer[consumed];
            }
            return OperationStatus.Done;
        }
    }
}

