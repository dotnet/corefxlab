// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Binary.Base64;
using System.Buffers;
using System.Text.Encoders;

namespace System.Azure.Authentication
{
    public static class Key {
        public static byte[] ComputeKeyBytes(string key)
        {
            int size = key.Length * 2;
            var buffer = size < 128 ? stackalloc byte[size] : (Span<byte>)new byte[size];

            int written, consumed;
            if (Utf16.ToUtf8(key.AsReadOnlySpan().AsBytes(), buffer, out consumed, out written) != OperationStatus.Done)
            {
                throw new NotImplementedException("need to resize buffer");
            }

            var keyBytes = new byte[64];
            var result = Base64.Utf8ToBytes(buffer.Slice(0, written), keyBytes, out consumed, out written);
            if (result != OperationStatus.Done || written != 64)
            {
                throw new NotImplementedException("need to resize buffer");
            }
            return keyBytes;
        }
    }
}
