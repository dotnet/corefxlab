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
    public static class Signature
    {
        static readonly byte[] s_type = Encoding.UTF8.GetBytes("type=");
        static readonly byte[] s_ver = Encoding.UTF8.GetBytes("&ver=");
        static readonly byte[] s_sig = Encoding.UTF8.GetBytes("&sig=");

        static readonly byte[] s_get = Encoding.UTF8.GetBytes("get\n");
        static readonly byte[] s_post = Encoding.UTF8.GetBytes("post\n");
        static readonly byte[] s_delete = Encoding.UTF8.GetBytes("delete\n");

        static readonly byte[] s_GET = Encoding.UTF8.GetBytes("GET\n");

        static readonly byte[] s_emptyHeaders = Encoding.UTF8.GetBytes("\n\n\n\n\n\n\n\n\n\n\nx-ms-date:");
        const int AuthenticationHeaderBufferSize = 256;

        public static bool TryWriteStorageSignature(Span<byte> output, Sha256 hash, string verb, string canonicalizedResource, DateTime utc, out int bytesWritten)
        {
            int written, consumed;
            bytesWritten = 0;

            if (verb.Equals("GET", StringComparison.Ordinal))
            {
                if (output.Length < 3)
                {
                    bytesWritten = 0;
                    return false;
                }
                s_GET.CopyTo(output);
                bytesWritten += s_GET.Length;
            }
            else
            {
                if (Utf16.ToUtf8(verb.AsSpan().AsBytes(), output, out consumed, out written) != TransformationStatus.Done)
                {
                    bytesWritten = 0;
                    return false;
                }

                output[written] = (byte)'\n';
                bytesWritten += written + 1;
            }

            var free = output.Slice(bytesWritten);
            s_emptyHeaders.CopyTo(free);
            bytesWritten += s_emptyHeaders.Length;

            free = output.Slice(bytesWritten);
            if (!PrimitiveFormatter.TryFormat(utc, free, out written, 'R'))
            {
                bytesWritten = 0;
                return false;
            }
            free[written] = (byte)'\n';
            bytesWritten += written + 1;
            free = output.Slice(bytesWritten);

            if (Utf16.ToUtf8(canonicalizedResource.AsSpan().AsBytes(), free, out consumed, out written) != TransformationStatus.Done)
            {
                bytesWritten = 0;
                return false;
            }
            bytesWritten += written;

            var formatted = output.Slice(0, bytesWritten);

            hash.Append(formatted);
            hash.GetHash(output.Slice(0, hash.OutputSize));

            if (!Base64.EncodeInPlace(output, hash.OutputSize, out written))
            {
                bytesWritten = 0;
                return false;
            }

            bytesWritten = written;
            return true;
        }

        public static bool TryWriteCosmosDbAuthorizationHeader(Span<byte> output, Sha256 hash, string keyType, string verb, string resourceId, string resourceType, string tokenVersion, DateTime utc, out int bytesWritten)
        {
            int written, consumed, totalWritten = 0;
            bytesWritten = 0;

            Span<byte> buffer;
            unsafe
            {
                var pBuffer = stackalloc byte[AuthenticationHeaderBufferSize];
                buffer = new Span<byte>(pBuffer, AuthenticationHeaderBufferSize);
            }

            s_type.CopyTo(buffer);
            totalWritten += s_type.Length;
            var bufferSlice = buffer.Slice(totalWritten);

            if (Utf16.ToUtf8(keyType.AsSpan().AsBytes(), bufferSlice, out consumed, out written) != TransformationStatus.Done)
            {
                throw new NotImplementedException("need to resize buffer");
            }
            totalWritten += written;
            bufferSlice = buffer.Slice(totalWritten);

            s_ver.CopyTo(bufferSlice);
            totalWritten += s_ver.Length;

            bufferSlice = buffer.Slice(totalWritten);

            if (Utf16.ToUtf8(tokenVersion.AsSpan().AsBytes(), bufferSlice, out consumed, out written) != TransformationStatus.Done)
            {
                throw new NotImplementedException("need to resize buffer");
            }
            totalWritten += written;
            bufferSlice = buffer.Slice(totalWritten);

            s_sig.CopyTo(bufferSlice);
            totalWritten += s_sig.Length;

            var front = buffer.Slice(0, totalWritten);

            var payload = buffer.Slice(totalWritten);
            totalWritten = 0;

            if (verb.Equals("GET", StringComparison.Ordinal) || verb.Equals("get", StringComparison.Ordinal))
            {
                s_get.CopyTo(payload);
                totalWritten += s_get.Length;
            }
            else if (verb.Equals("POST", StringComparison.Ordinal) || verb.Equals("post", StringComparison.Ordinal))
            {
                s_post.CopyTo(payload);
                totalWritten += s_post.Length;
            }
            else if (verb.Equals("DELETE", StringComparison.Ordinal) || verb.Equals("delete", StringComparison.Ordinal))
            {
                s_delete.CopyTo(payload);
                totalWritten += s_delete.Length;
            }
            else
            {
                if (Utf16.ToUtf8(verb.AsSpan().AsBytes(), payload, out consumed, out written) != TransformationStatus.Done)
                {
                    throw new NotImplementedException("need to resize buffer");
                }
                if (Ascii.ToLowerInPlace(payload.Slice(0, written), out written) != TransformationStatus.Done)
                {
                    throw new NotImplementedException("need to resize buffer");
                }

                payload[written] = (byte)'\n';
                totalWritten += written + 1;
            }

            bufferSlice = payload.Slice(totalWritten);

            if (Utf16.ToUtf8(resourceType.AsSpan().AsBytes(), bufferSlice, out consumed, out written) != TransformationStatus.Done)
            {
                throw new NotImplementedException("need to resize buffer");
            }
            if (Ascii.ToLowerInPlace(bufferSlice.Slice(0, written), out written) != TransformationStatus.Done)
            {
                throw new NotImplementedException("need to resize buffer");
            }
            bufferSlice[written] = (byte)'\n';
            totalWritten += written + 1;
            bufferSlice = payload.Slice(totalWritten);

            if (Utf16.ToUtf8(resourceId.AsSpan().AsBytes(), bufferSlice, out consumed, out written) != TransformationStatus.Done)
            {
                throw new NotImplementedException("need to resize buffer");
            }
            bufferSlice[written] = (byte)'\n';
            totalWritten += written + 1;
            bufferSlice = payload.Slice(totalWritten);

            if (!PrimitiveFormatter.TryFormat(utc, bufferSlice, out written, 'l'))
            {
                throw new NotImplementedException("need to resize buffer");
            }
            bufferSlice[written] = (byte)'\n';
            totalWritten += written + 1;
            bufferSlice = payload.Slice(totalWritten);

            bufferSlice[0] = (byte)'\n';
            totalWritten += 1;

            hash.Append(buffer.Slice(front.Length, totalWritten));
            hash.GetHash(buffer.Slice(front.Length, hash.OutputSize));
            if (!Base64.EncodeInPlace(buffer.Slice(front.Length), hash.OutputSize, out written))
            {
                throw new NotImplementedException("need to resize buffer");
            }

            var len = front.Length + written;
            if (!UrlEncoder.TryEncode(buffer.Slice(0, len), output, out bytesWritten))
            {
                bytesWritten = 0;
                return false;
            }
            return true;
        }

        public unsafe static byte[] ComputeKeyBytes(string key)
        {
            const int bufferLength = 128;

            byte* pBuffer = stackalloc byte[bufferLength];
            int written, consumed;
            var buffer = new Span<byte>(pBuffer, bufferLength);
            if (Utf16.ToUtf8(key.AsSpan().AsBytes(), buffer, out consumed, out written) != TransformationStatus.Done)
            {
                throw new NotImplementedException("need to resize buffer");
            }
            var keyBytes = new byte[64];
            var result = Base64.Decode(buffer.Slice(0, written), keyBytes, out consumed, out written);
            if (result != TransformationStatus.Done || written != 64)
            {
                throw new NotImplementedException("need to resize buffer");
            }
            return keyBytes;
        }
    }
}
