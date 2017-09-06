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
    public struct CosmosDbAuthorizationHeader : IBufferFormattable {
        public Sha256 Hash;
        public string KeyType;
        public string Method;
        public string ResourceId;
        public string ResourceType;
        public string Version;
        public DateTime Time;

        public bool TryFormat(Span<byte> buffer, out int written, ParsedFormat format = default(ParsedFormat), SymbolTable symbolTable = null)
        {
            if (TryWrite(buffer, Hash, KeyType, Method, ResourceId, ResourceType, Version, Time, out written))
            {
                return true;
            }
            buffer.Clear();
            written = 0;
            return false;
        }

        public static bool TryWrite(Span<byte> output, Sha256 hash, string keyType, string verb, string resourceId, string resourceType, string tokenVersion, DateTime utc, out int bytesWritten)
        {
            int written, consumed, totalWritten = 0;
            bytesWritten = 0;

            Span<byte> buffer = stackalloc byte[AuthenticationHeaderBufferSize];

            s_type.CopyTo(buffer);
            totalWritten += s_type.Length;
            var bufferSlice = buffer.Slice(totalWritten);

            if (Utf16.ToUtf8(keyType.AsReadOnlySpan().AsBytes(), bufferSlice, out consumed, out written) != OperationStatus.Done)
            {
                throw new NotImplementedException("need to resize buffer");
            }
            totalWritten += written;
            bufferSlice = buffer.Slice(totalWritten);

            s_ver.CopyTo(bufferSlice);
            totalWritten += s_ver.Length;

            bufferSlice = buffer.Slice(totalWritten);

            if (Utf16.ToUtf8(tokenVersion.AsReadOnlySpan().AsBytes(), bufferSlice, out consumed, out written) != OperationStatus.Done)
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
                if (Utf16.ToUtf8(verb.AsReadOnlySpan().AsBytes(), payload, out consumed, out written) != OperationStatus.Done)
                {
                    throw new NotImplementedException("need to resize buffer");
                }
                if (Ascii.ToLowerInPlace(payload.Slice(0, written), out written) != OperationStatus.Done)
                {
                    throw new NotImplementedException("need to resize buffer");
                }

                payload[written] = (byte)'\n';
                totalWritten += written + 1;
            }

            bufferSlice = payload.Slice(totalWritten);

            if (Utf16.ToUtf8(resourceType.AsReadOnlySpan().AsBytes(), bufferSlice, out consumed, out written) != OperationStatus.Done)
            {
                throw new NotImplementedException("need to resize buffer");
            }
            if (Ascii.ToLowerInPlace(bufferSlice.Slice(0, written), out written) != OperationStatus.Done)
            {
                throw new NotImplementedException("need to resize buffer");
            }
            bufferSlice[written] = (byte)'\n';
            totalWritten += written + 1;
            bufferSlice = payload.Slice(totalWritten);

            if (Utf16.ToUtf8(resourceId.AsReadOnlySpan().AsBytes(), bufferSlice, out consumed, out written) != OperationStatus.Done)
            {
                throw new NotImplementedException("need to resize buffer");
            }
            bufferSlice[written] = (byte)'\n';
            totalWritten += written + 1;
            bufferSlice = payload.Slice(totalWritten);

            if (!Text.Formatters.Utf8.TryFormat(utc, bufferSlice, out written, 'l'))
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

        static readonly byte[] s_type = Encoding.UTF8.GetBytes("type=");
        static readonly byte[] s_ver = Encoding.UTF8.GetBytes("&ver=");
        static readonly byte[] s_sig = Encoding.UTF8.GetBytes("&sig=");

        static readonly byte[] s_get = Encoding.UTF8.GetBytes("get\n");
        static readonly byte[] s_post = Encoding.UTF8.GetBytes("post\n");
        static readonly byte[] s_delete = Encoding.UTF8.GetBytes("delete\n");

        const int AuthenticationHeaderBufferSize = 256;
    }
}
