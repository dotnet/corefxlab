// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Text.Encodings.Web.Buffers;

namespace System.Text.Encodings.Web.Internal
{
    internal class Utf8UriEncoder : BufferEncoder
    {
        public override OperationStatus Encode(ReadOnlySpan<byte> input, Span<byte> output, out int consumed, out int written)
        {
            if (Utf8.UrlEncoder.TryEncode(input, output, out written))
            {
                consumed = input.Length;
                return OperationStatus.Done;
            }

            // TODO: this needs to be implemented properly
            throw new NotImplementedException();
        }

        public override bool TryEncode(ReadOnlySpan<byte> input, Span<byte> output, out int written)
        {
            return Utf8.UrlEncoder.TryEncode(input, output, out written);
        }
    }

    internal class Utf8UriDecoder : BufferDecoder
    {
        public override OperationStatus Decode(ReadOnlySpan<byte> input, Span<byte> output, out int consumed, out int written)
        {
            written = Utf8.UrlEncoder.Decode(input, output);
            consumed = input.Length;
            return OperationStatus.Done;
        }

        public override OperationStatus DecodeInPlace(Span<byte> buffer, int length, out int written)
        {
            written = Utf8.UrlEncoder.DecodeInPlace(buffer.Slice(0, length));
            return OperationStatus.Done;
        }
    }
}
