// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;

namespace System.Text.Encodings.Web.Buffers
{
    public abstract class BufferEncoder : IBufferOperation
    {
        public abstract OperationStatus Encode(ReadOnlySpan<byte> input, Span<byte> output, out int consumed, out int written);
        public virtual bool TryEncode(ReadOnlySpan<byte> input, Span<byte> output, out int written)
        {
            if (Encode(input, output, out int consumed, out written) == OperationStatus.Done)
            {
                return true;
            }
            return false;
        }
        public virtual OperationStatus EncodeInPlace(Span<byte> buffer, int length, out int written)
        {
            written = 0;
            return OperationStatus.NotSupported;
        }

        OperationStatus IBufferOperation.Execute(ReadOnlySpan<byte> input, Span<byte> output, out int consumed, out int written) =>
            Encode(input, output, out consumed, out written);
    }

    public abstract class BufferDecoder : IBufferOperation
    {
        public abstract OperationStatus Decode(ReadOnlySpan<byte> input, Span<byte> output, out int consumed, out int written);
        public virtual bool TryDecode(ReadOnlySpan<byte> input, Span<byte> output, out int written)
        {
            if (Decode(input, output, out int consumed, out written) == OperationStatus.Done)
            {
                return true;
            }
            return false;
        }
        public virtual OperationStatus DecodeInPlace(Span<byte> buffer, int length, out int written)
        {
            written = 0;
            return OperationStatus.NotSupported;
        }

        OperationStatus IBufferOperation.Execute(ReadOnlySpan<byte> input, Span<byte> output, out int consumed, out int written) =>
            Decode(input, output, out consumed, out written);
    }
}

