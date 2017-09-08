// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Buffers
{
    public abstract class BufferEncoder : IBufferOperation
    {
        public abstract OperationStatus Encode(ReadOnlySpan<byte> input, Span<byte> output, out int consumed, out int written);

        public virtual OperationStatus EncodeInPlace(Span<byte> buffer, int inputLength, out int written)
        {
            written = 0;
            return OperationStatus.NotSupported;
        }

        OperationStatus IBufferOperation.Execute(ReadOnlySpan<byte> input, Span<byte> output, out int consumed, out int written) =>
            Encode(input, output, out consumed, out written);
    }
}

