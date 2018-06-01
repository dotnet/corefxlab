// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Operations;

namespace System.Text.Encodings.Web
{
    public class Utf8UriEncoder : IBufferOperation
    {
        public OperationStatus Encode(ReadOnlySpan<byte> input, Span<byte> output, out int consumed, out int written)
        {
            if (Utf8.UrlEncoder.TryEncode(input, output, out written))
            {
                consumed = input.Length;
                return OperationStatus.Done;
            }

            // TODO: this needs to be implemented properly
            throw new NotImplementedException();
        }

        OperationStatus IBufferOperation.Execute(ReadOnlySpan<byte> input, Span<byte> output, out int consumed, out int written)
            => Encode(input, output, out consumed, out written);
    }

    public class Utf8UriDecoder : IBufferTransformation
    {
        public OperationStatus Decode(ReadOnlySpan<byte> input, Span<byte> output, out int consumed, out int written)
        {
            written = Utf8.UrlEncoder.Decode(input, output);
            consumed = input.Length;
            return OperationStatus.Done;
        }

        public OperationStatus DecodeInPlace(Span<byte> buffer, int length, out int written)
        {
            written = Utf8.UrlEncoder.DecodeInPlace(buffer.Slice(0, length));
            return OperationStatus.Done;
        }

        OperationStatus IBufferOperation.Execute(ReadOnlySpan<byte> input, Span<byte> output, out int consumed, out int written)
            => Decode(input, output, out consumed, out written);

        OperationStatus IBufferTransformation.Transform(Span<byte> buffer, int dataLength, out int written)
            => DecodeInPlace(buffer, dataLength, out written);
    }
}
