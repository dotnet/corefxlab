// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Buffers;
using System.Buffers.Operations;
using System.Buffers.Text;

namespace System.Binary.Base64Experimental
{
    public static partial class Base64Experimental
    {
        public static readonly Utf8Decoder Utf8ToBytesDecoder = new Utf8Decoder();

        public sealed class Utf8Decoder : IBufferOperation, IBufferTransformation
        {
            public OperationStatus Decode(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
                => Base64.DecodeFromUtf8(source, destination, out bytesConsumed, out bytesWritten);

            public OperationStatus DecodeInPlace(Span<byte> buffer, int dataLength, out int written)
                => Base64.DecodeFromUtf8InPlace(buffer.Slice(0, dataLength), out written);

            OperationStatus IBufferOperation.Execute(ReadOnlySpan<byte> input, Span<byte> output, out int consumed, out int written)
                => Decode(input, output, out consumed, out written);

            OperationStatus IBufferTransformation.Transform(Span<byte> buffer, int dataLength, out int written)
                => DecodeInPlace(buffer, dataLength, out written);
        }
    }
}
