// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Buffers.Text;

namespace System.Azure.Authentication
{
    public static class Key {
        public static byte[] ComputeKeyBytes(string key)
        {
            int size = key.Length * 2;
            var buffer = size < 128 ? stackalloc byte[size] : new byte[size];

            if (Encodings.Utf16.ToUtf8(key.AsReadOnlySpan().AsBytes(), buffer, out int consumed, out int written) != OperationStatus.Done)
            {
                throw new NotImplementedException("need to resize buffer");
            }

            var keyBytes = new byte[64];
            var result = Base64.DecodeFromUtf8(buffer.Slice(0, written), keyBytes, out consumed, out written);
            if (result != OperationStatus.Done || written != 64)
            {
                throw new NotImplementedException("need to resize buffer");
            }
            return keyBytes;
        }

        public static byte[] ComputeKeyBytes(this ReadOnlySpan<char> key)
        {
            int size = key.Length * 2;
            var buffer = size < 128 ? stackalloc byte[size] : new byte[size];

            if (Encodings.Utf16.ToUtf8(key.AsBytes(), buffer, out int consumed, out int written) != OperationStatus.Done)
            {
                throw new NotImplementedException("need to resize buffer");
            }

            var keyBytes = new byte[64];
            var result = Base64.DecodeFromUtf8(buffer.Slice(0, written), keyBytes, out consumed, out written);
            if (result != OperationStatus.Done || written != 64)
            {
                throw new NotImplementedException("need to resize buffer");
            }
            return keyBytes;
        }
    }
}
