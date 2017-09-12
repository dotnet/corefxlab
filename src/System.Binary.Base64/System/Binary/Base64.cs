// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Buffers;

namespace System.Binary.Base64
{
    public static partial class Base64
    {
        public static readonly BufferEncoder BytesToUtf8Encoder = new ToBase64Utf8();
        public static readonly BufferDecoder Utf8ToBytesDecoder = new FromBase64Utf8();

        public static OperationStatus BytesToUtf8(ReadOnlySpan<byte> bytes, Span<byte> utf8, out int consumed, out int written)
            =>Encode(bytes, utf8, out consumed, out written);

        public static int BytesToUtf8Length(int bytesLength) => ComputeEncodedUtf8Length(bytesLength);

        public static OperationStatus BytesToUtf8InPlace(Span<byte> buffer, int bytesLength, out int written)
            => EncodeInPlace(buffer, bytesLength, out written) ? OperationStatus.Done : OperationStatus.DestinationTooSmall;

        public static OperationStatus Utf8ToBytes(ReadOnlySpan<byte> utf8, Span<byte> bytes, out int consumed, out int written)
            => Decode(utf8, bytes, out consumed, out written);

        public static OperationStatus Utf8ToBytesInPlace(Span<byte> buffer, out int consumed, out int written)
            => DecodeInPlace(buffer, out consumed, out written);

        public static int Utf8ToBytesLength(ReadOnlySpan<byte> utf8) => ComputeDecodedUtf8Length(utf8);
    }
}
