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

            Span<byte> buffer;
            unsafe
            {
                var pBuffer = stackalloc byte[AuthenticationHeaderBufferSize];
                buffer = new Span<byte>(pBuffer, AuthenticationHeaderBufferSize);
            }

            if (verb.Equals("GET", StringComparison.Ordinal))
            {
                if(output.Length < 3)
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

        public static unsafe int GenerateCosmosDbAuthentication(Span<byte> output, Sha256 hash, string keyType, string verb, string resourceId, string resourceType, string tokenVersion, DateTime utc)
        {
            int written, consumed, totalWritten = 0;

            var pBuffer = stackalloc byte[AuthenticationHeaderBufferSize];
            var buffer = new Span<byte>(pBuffer, AuthenticationHeaderBufferSize);

            s_type.CopyTo(buffer);
            totalWritten += s_type.Length;
            var span = buffer.Slice(totalWritten);

            if (Utf16.ToUtf8(keyType.AsSpan().AsBytes(), span, out consumed, out written) != TransformationStatus.Done)
            {
                throw new NotImplementedException();
            }
            totalWritten += written;
            span = buffer.Slice(totalWritten);

            s_ver.CopyTo(span);
            totalWritten += s_ver.Length;

            span = buffer.Slice(totalWritten);

            if (Utf16.ToUtf8(tokenVersion.AsSpan().AsBytes(), span, out consumed, out written) != TransformationStatus.Done)
            {
                throw new NotImplementedException();
            }
            totalWritten += written;
            span = buffer.Slice(totalWritten);

            s_sig.CopyTo(span);
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
                    throw new NotImplementedException();
                }
                if (Ascii.ToLowerInPlace(payload.Slice(0, written), out written) != TransformationStatus.Done)
                {
                    throw new NotImplementedException();
                }

                payload[written] = (byte)'\n';
                totalWritten += written + 1;
            }

            span = payload.Slice(totalWritten);

            if (Utf16.ToUtf8(resourceType.AsSpan().AsBytes(), span, out consumed, out written) != TransformationStatus.Done)
            {
                throw new NotImplementedException();
            }
            if (Ascii.ToLowerInPlace(span.Slice(0, written), out written) != TransformationStatus.Done)
            {
                throw new NotImplementedException();
            }
            span[written] = (byte)'\n';
            totalWritten += written + 1;
            span = payload.Slice(totalWritten);

            if (Utf16.ToUtf8(resourceId.AsSpan().AsBytes(), span, out consumed, out written) != TransformationStatus.Done)
            {
                throw new NotImplementedException();
            }
            span[written] = (byte)'\n';
            totalWritten += written + 1;
            span = payload.Slice(totalWritten);

            if (!PrimitiveFormatter.TryFormat(utc, span, out written, 'l'))
            {
                throw new NotImplementedException();
            }
            span[written] = (byte)'\n';
            totalWritten += written + 1;
            span = payload.Slice(totalWritten);

            span[0] = (byte)'\n';
            totalWritten += 1;

            hash.Append(buffer.Slice(front.Length, totalWritten));
            hash.GetHash(buffer.Slice(front.Length, hash.OutputSize));
            if (!Base64.EncodeInPlace(buffer.Slice(front.Length), hash.OutputSize, out written))
            {
                throw new NotImplementedException();
            }

            var len = front.Length + written;
            UrlEncoder.Encode(buffer.Slice(0, len), output, out int encodedBytes);
            return encodedBytes;
        }

        public unsafe static byte[] ComputeKeyBytes(string key)
        {
            const int bufferLength = 128;

            byte* pBuffer = stackalloc byte[bufferLength];
            int written, consumed;
            var buffer = new Span<byte>(pBuffer, bufferLength);
            if (Utf16.ToUtf8(key.AsSpan().AsBytes(), buffer, out consumed, out written) != TransformationStatus.Done)
            {
                throw new NotImplementedException();
            }
            var keyBytes = new byte[64];
            var result = Base64.Decode(buffer.Slice(0, written), keyBytes, out consumed, out written);
            if (result != TransformationStatus.Done || written != 64)
            {
                throw new NotImplementedException();
            }
            return keyBytes;
        }
    }
}
