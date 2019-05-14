// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using System.Azure.Authentication;
using System.Buffers;
using System.Buffers.Cryptography;
using System.Buffers.Text;
using System.Security.Cryptography;
using System.Text;
using System.Text.Utf8;
using System.Runtime.InteropServices;

namespace System.Azure.Experimental.Benchmarks
{
    public class Storage
    {
        static string canonicalizedResource = "/myaccount /mycontainer\ncomp:metadata\nrestype:container\ntimeout:20";
        static string fakeKey = "TjW7xr4kKR67qgt2y3fAAMxvC2neMHT6cKawiliGCsDkxSS34V0EnwL8GKrA6ZTIfNrXK91t1Ey3RmEKQLrrCA==";
        static string verb = "GET";

        static Utf8String canonicalizedResourceU8 = (Utf8String)"/myaccount /mycontainer\ncomp:metadata\nrestype:container\ntimeout:20";
        static Utf8String verbU8 = (Utf8String)"GET";

        static DateTime utc = DateTime.UtcNow;
        static byte[] buffer = new byte[BufferSize];
        static Sha256 sha;
        const int BufferSize = 256;

        [GlobalSetup]
        public void Setup()
        {
            var keyBytes = Key.ComputeKeyBytes(fakeKey);
            sha = Sha256.Create(keyBytes);
        }

        [Benchmark(Baseline = true)]
        public string Msdn()
            => StorageBaselineFromMsdn(fakeKey, verb, canonicalizedResource, utc);

        [Benchmark]
        public void Primitive()
        {
            if (!TryWritePrimitive(buffer, sha, verb, canonicalizedResource, utc, out int bytesWritten))
            {
                throw new Exception("TryWrite failed");
            }
        }

        [Benchmark]
        public void TryWrite()
        {
            if (!StorageAccessSignature.TryWrite(buffer, sha, "GET", canonicalizedResource, utc, out int bytesWritten))
            {
                throw new Exception("TryWrite failed");
            }
        }

        [Benchmark]
        public void TryWriteUtf8()
        {
            if (!StorageAccessSignature.TryWrite(buffer, sha, verbU8, canonicalizedResourceU8, utc, out int bytesWritten))
            {
                throw new Exception("TryWrite failed");
            }
        }

        static string StorageBaselineFromMsdn(string key, string verb, string canonicalizedResource, DateTime utc)
        {
            string canonicalizedString = verb + "\n"
                + "\n"
                + "\n"
                + "\n"
                + "\n"
                + "\n"
                + "\n"
                + "\n"
                + "\n"
                + "\n"
                + "\n"
                + "\n"
                + "x-ms-date:" + utc.ToString("r") + "\n"
               + canonicalizedResource;

            using (HMACSHA256 hmacSha256 = new HMACSHA256(Convert.FromBase64String(key)))
            {
                Byte[] dataToHmac = Encoding.UTF8.GetBytes(canonicalizedString);
                return Convert.ToBase64String(hmacSha256.ComputeHash(dataToHmac));
            }
        }

        static bool TryWritePrimitive(Span<byte> output, Sha256 hash, string verb, string canonicalizedResource, DateTime utc, out int bytesWritten)
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
                if (TextEncodings.Utf16.ToUtf8(MemoryMarshal.AsBytes(verb.AsSpan()), output, out consumed, out written) != OperationStatus.Done)
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
            if (!Utf8Formatter.TryFormat(utc, free, out written, 'R'))
            {
                bytesWritten = 0;
                return false;
            }
            free[written] = (byte)'\n';
            bytesWritten += written + 1;
            free = output.Slice(bytesWritten);

            if (TextEncodings.Utf16.ToUtf8(MemoryMarshal.AsBytes(canonicalizedResource.AsSpan()), free, out consumed, out written) != OperationStatus.Done)
            {
                bytesWritten = 0;
                return false;
            }
            bytesWritten += written;

            var formatted = output.Slice(0, bytesWritten);

            hash.Append(formatted);
            if (!hash.TryWrite(output, out written))
            {
                throw new NotImplementedException("need to resize buffer");
            }

            if (Base64.EncodeToUtf8InPlace(output, written, out written) != OperationStatus.Done)
            {
                bytesWritten = 0;
                return false;
            }

            bytesWritten = written;
            return true;
        }

        static readonly byte[] s_GET = Encoding.UTF8.GetBytes("GET\n");

        static readonly byte[] s_emptyHeaders = Encoding.UTF8.GetBytes("\n\n\n\n\n\n\n\n\n\n\nx-ms-date:");
    }
}
