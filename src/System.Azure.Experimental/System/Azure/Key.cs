// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Text;
using System.Runtime.InteropServices;

namespace System.Azure.Authentication
{
    public static class Key {
        public static byte[] ComputeKeyBytes(string key)
            => ComputeKeyBytes(key.AsSpan());

        public static byte[] ComputeKeyBytes(this ReadOnlySpan<char> key)
        {
            var utf16Bytes = MemoryMarshal.AsBytes(key);
            int size = utf16Bytes.Length; // the input must be ASCII (i.e. Base64 encoded)

            var buffer = size < 128 ? stackalloc byte[size] : new byte[size];

            var result = Encodings.Utf16.ToUtf8(utf16Bytes, buffer, out int consumed, out int written);
            if (result != OperationStatus.Done)
            {
                throw new ArgumentOutOfRangeException(nameof(key), $"ToUtf8 returned {result}");
            }

            var keyBytes = new byte[64];
            result = Base64.DecodeFromUtf8(buffer.Slice(0, written), keyBytes, out consumed, out written);
            if (result != OperationStatus.Done)
            {
                throw new ArgumentOutOfRangeException(nameof(key), $"Base64.Decode returned {result}");
            }
            if (written != 64)
            {
                throw new ArgumentOutOfRangeException(nameof(key), $"{written}!={64}");
            }
            return keyBytes;
        }

        public static bool TryComputeKeyBytes(this ReadOnlySpan<char> key, Span<byte> keyBytes)
        {
            var utf16Bytes = MemoryMarshal.AsBytes(key);
            int size = utf16Bytes.Length; // the input must be ASCII (i.e. Base64 encoded)

            var buffer = size < 128 ? stackalloc byte[size] : new byte[size];

            var result = Encodings.Utf16.ToUtf8(utf16Bytes, buffer, out int consumed, out int written);
            if (result != OperationStatus.Done)
            {
                throw new ArgumentOutOfRangeException(nameof(key), $"ToUtf8 returned {result}");
            }

            result = Base64.DecodeFromUtf8(buffer.Slice(0, written), keyBytes, out consumed, out written);
            if (result == OperationStatus.Done) return true;
            if (result == OperationStatus.DestinationTooSmall) return false;

            throw new ArgumentOutOfRangeException(nameof(key), $"Base64.Decode returned {result}");
        }
    }
}
