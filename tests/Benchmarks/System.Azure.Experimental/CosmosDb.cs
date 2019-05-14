// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Azure.Authentication;
using System.Buffers;
using System.Buffers.Cryptography;
using System.Buffers.Text;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web.Utf8;
using System.Text.Utf8;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;


namespace System.Azure.Experimental.Benchmarks
{
    public class CosmosDb
    {
        static string fakeKey = "TjW7xr4kKR67qgt2y3fAAMxvC2neMHT6cKawiliGCsDkxSS34V0EnwL8GKrA6ZTIfNrXK91t1Ey3RmEKQLrrCA==";
        static string keyType = "master";
        static string resourceType = "dbs";
        static string version = "1.0";
        static string resourceId = "";
        static string verb = "GET";

        static Utf8String keyTypeU8 = (Utf8String)"master";
        static Utf8String resourceTypeU8 = (Utf8String)"dbs";
        static Utf8String versionU8 = (Utf8String)"1.0";
        static Utf8String resourceIdU8 = Utf8String.Empty;
        static Utf8String verbU8 = (Utf8String)"GET";

        static DateTime utc = DateTime.UtcNow;
        static byte[] output = new byte[256];
        static Sha256 sha;

        [GlobalSetup]
        public void Setup()
        {
            var keyBytes = Key.ComputeKeyBytes(fakeKey);
            sha = Sha256.Create(keyBytes);
        }

        [Benchmark(Baseline = true)]
        public string Msdn()
            => CosmosDbBaselineFromMsdn(fakeKey, keyType, verb, resourceId, resourceType, version, utc);

        [Benchmark]
        public bool Primitives()
            => TryWritePrimitives(output, sha, keyType, verb, resourceId, resourceType, version, utc, out int bytesWritten);

        [Benchmark]
        public bool Writer()
            => CosmosDbAuthorizationHeader.TryWrite(output, sha, keyType, verb, resourceId, resourceType, version, utc, out int bytesWritten);

        [Benchmark]
        public bool WriterUtf8()
            => CosmosDbAuthorizationHeader.TryWrite(output, sha, keyTypeU8, verbU8, resourceIdU8, resourceTypeU8, versionU8, utc, out int bytesWritten);

        static string CosmosDbBaselineFromMsdn(string key, string keyType, string verb, string resourceId, string resourceType, string tokenVersion, DateTime utc)
        {
            var keyBytes = Convert.FromBase64String(key);
            var hmacSha256 = new HMACSHA256 { Key = keyBytes };
            string utc_date = utc.ToString("r");

            string payLoad = string.Format(CultureInfo.InvariantCulture, "{0}\n{1}\n{2}\n{3}\n{4}\n",
                    verb.ToLowerInvariant(),
                    resourceType.ToLowerInvariant(),
                    resourceId,
                    utc_date.ToLowerInvariant(),
                    ""
            );

            byte[] hashPayLoad = hmacSha256.ComputeHash(Encoding.UTF8.GetBytes(payLoad));
            string signature = Convert.ToBase64String(hashPayLoad);

            var full = String.Format(CultureInfo.InvariantCulture, "type={0}&ver={1}&sig={2}",
                keyType,
                tokenVersion,
                signature);

            var result = Text.Encodings.Web.UrlEncoder.Default.Encode(full);

            return result;
        }

        static bool TryWritePrimitives(Span<byte> output, Sha256 hash, string keyType, string verb, string resourceId, string resourceType, string tokenVersion, DateTime utc, out int bytesWritten)
        {
            int totalWritten = 0;
            bytesWritten = 0;

            Span<byte> buffer = stackalloc byte[AuthenticationHeaderBufferSize];

            s_type.CopyTo(buffer);
            totalWritten += s_type.Length;

            if (TextEncodings.Utf16.ToUtf8(MemoryMarshal.AsBytes(keyType.AsSpan()), buffer.Slice(totalWritten), out int consumed, out int written) != OperationStatus.Done)
            {
                throw new NotImplementedException("need to resize buffer");
            }
            totalWritten += written;

            s_ver.CopyTo(buffer.Slice(totalWritten));
            totalWritten += s_ver.Length;

            if (TextEncodings.Utf16.ToUtf8(MemoryMarshal.AsBytes(tokenVersion.AsSpan()), buffer.Slice(totalWritten), out consumed, out written) != OperationStatus.Done)
            {
                throw new NotImplementedException("need to resize buffer");
            }
            totalWritten += written;

            s_sig.CopyTo(buffer.Slice(totalWritten));
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
                if (TextEncodings.Utf16.ToUtf8(MemoryMarshal.AsBytes(verb.AsSpan()), payload, out consumed, out written) != OperationStatus.Done)
                {
                    throw new NotImplementedException("need to resize buffer");
                }
                if (TextEncodings.Ascii.ToLowerInPlace(payload.Slice(0, written), out written) != OperationStatus.Done)
                {
                    throw new NotImplementedException("need to resize buffer");
                }

                payload[written] = (byte)'\n';
                totalWritten += written + 1;
            }

            var bufferSlice = payload.Slice(totalWritten);

            if (TextEncodings.Utf16.ToUtf8(MemoryMarshal.AsBytes(resourceType.AsSpan()), bufferSlice, out consumed, out written) != OperationStatus.Done)
            {
                throw new NotImplementedException("need to resize buffer");
            }
            if (TextEncodings.Ascii.ToLowerInPlace(bufferSlice.Slice(0, written), out written) != OperationStatus.Done)
            {
                throw new NotImplementedException("need to resize buffer");
            }
            bufferSlice[written] = (byte)'\n';
            totalWritten += written + 1;
            bufferSlice = payload.Slice(totalWritten);

            if (TextEncodings.Utf16.ToUtf8(MemoryMarshal.AsBytes(resourceId.AsSpan()), bufferSlice, out consumed, out written) != OperationStatus.Done)
            {
                throw new NotImplementedException("need to resize buffer");
            }
            bufferSlice[written] = (byte)'\n';
            totalWritten += written + 1;
            bufferSlice = payload.Slice(totalWritten);

            if (!Utf8Formatter.TryFormat(utc, bufferSlice, out written, 'l'))
            {
                throw new NotImplementedException("need to resize buffer");
            }
            bufferSlice[written] = (byte)'\n';
            totalWritten += written + 1;
            bufferSlice = payload.Slice(totalWritten);

            bufferSlice[0] = (byte)'\n';
            totalWritten += 1;

            hash.Append(buffer.Slice(front.Length, totalWritten));
            if (!hash.TryWrite(buffer.Slice(front.Length), out written))
            {
                throw new NotImplementedException("need to resize buffer");
            }
            if (Base64.EncodeToUtf8InPlace(buffer.Slice(front.Length), written, out written) != OperationStatus.Done)
            {
                throw new NotImplementedException("need to resize buffer");
            }

            var len = front.Length + written;
            if (UrlEncoder.Utf8.Encode(buffer.Slice(0, len), output, out consumed, out bytesWritten) != OperationStatus.Done)
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
