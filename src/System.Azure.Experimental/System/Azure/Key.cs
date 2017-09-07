// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Binary.Base64;
using System.Buffers;
using System.Buffers.Cryptography;
using System.Text;
using System.Text.Encoders;
using System.Text.Encodings.Web.Utf8;

namespace System.Azure.Authentication
{
    public static class Key {
        public static byte[] ComputeKeyBytes(string key)
        {
            const int bufferLength = 128;

            int written, consumed;
            Span<byte> buffer = stackalloc byte[bufferLength];
            if (Utf16.ToUtf8(key.AsReadOnlySpan().AsBytes(), buffer, out consumed, out written) != OperationStatus.Done)
            {
                throw new NotImplementedException("need to resize buffer");
            }
            var keyBytes = new byte[64];
            var result = Base64.Decode(buffer.Slice(0, written), keyBytes, out consumed, out written);
            if (result != OperationStatus.Done || written != 64)
            {
                throw new NotImplementedException("need to resize buffer");
            }
            return keyBytes;
        }
    }
}
