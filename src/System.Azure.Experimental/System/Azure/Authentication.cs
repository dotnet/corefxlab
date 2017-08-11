// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Binary.Base64;
using System.Buffers;
using System.Buffers.Cryptography;
using System.Text;
using System.Text.Encoders;

namespace System.Azure.Authentication
{
    public class Signature
    {
        static readonly byte[] s_type = Encoding.UTF8.GetBytes("type=");
        static readonly byte[] s_ver = Encoding.UTF8.GetBytes("&ver=");
        static readonly byte[] s_sig = Encoding.UTF8.GetBytes("&sig=");

        static readonly byte[] s_get = Encoding.UTF8.GetBytes("get\n");
        static readonly byte[] s_post = Encoding.UTF8.GetBytes("post\n");
        static readonly byte[] s_delete = Encoding.UTF8.GetBytes("delete\n");

        const int AuthenticationHeaderBufferSize = 256;
        public static unsafe int Generate(Span<byte> output, Sha256 hash, string keyType, string verb, string resourceId, string resourceType, string tokenVersion, DateTime utc)
        {
            int written, consumed, totalWritten = 0;

            var pBuffer = stackalloc byte[AuthenticationHeaderBufferSize];
            var buffer = new Span<byte>(pBuffer, AuthenticationHeaderBufferSize);

            s_type.CopyTo(buffer);
            totalWritten += s_type.Length;
            var span = buffer.Slice(totalWritten);

            if (Utf16.ToUtf8(keyType.AsReadOnlySpan().AsBytes(), span, out consumed, out written) != TransformationStatus.Done)
            {
                throw new NotImplementedException();
            }
            totalWritten += written;
            span = buffer.Slice(totalWritten);

            s_ver.CopyTo(span);
            totalWritten += s_ver.Length;

            span = buffer.Slice(totalWritten);

            if (Utf16.ToUtf8(tokenVersion.AsReadOnlySpan().AsBytes(), span, out consumed, out written) != TransformationStatus.Done)
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
                if (Utf16.ToUtf8(verb.AsReadOnlySpan().AsBytes(), payload, out consumed, out written) != TransformationStatus.Done)
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

            if (Utf16.ToUtf8(resourceType.AsReadOnlySpan().AsBytes(), span, out consumed, out written) != TransformationStatus.Done)
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

            if (Utf16.ToUtf8(resourceId.AsReadOnlySpan().AsBytes(), span, out consumed, out written) != TransformationStatus.Done)
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
            UrlEncode(buffer.Slice(0, len), output, out int encodedBytes);
            return encodedBytes;
        }

        public unsafe static byte[] ComputeKeyBytes(string key)
        {
            const int bufferLength = 128;

            byte* pBuffer = stackalloc byte[bufferLength];
            int written, consumed;
            var buffer = new Span<byte>(pBuffer, bufferLength);
            if (Utf16.ToUtf8(key.AsReadOnlySpan().AsBytes(), buffer, out consumed, out written) != TransformationStatus.Done)
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

        static void UrlEncode(ReadOnlySpan<byte> input, Span<byte> output, out int written)
        {
            written = 0;
            for (int inputIndex = 0; inputIndex < input.Length; inputIndex++)
            {
                var next = input[inputIndex];
                if (IsAllowed[next])
                {
                    output[written++] = input[inputIndex];
                }
                else
                {
                    output[written++] = (byte)'%';
                    PrimitiveFormatter.TryFormat(next, output.Slice(written), out int formatted, 'X');
                    written += formatted;
                }
            }
        }

        static Signature()
        {
            // Unreserved
            IsAllowed['A'] = true;
            IsAllowed['B'] = true;
            IsAllowed['C'] = true;
            IsAllowed['D'] = true;
            IsAllowed['E'] = true;
            IsAllowed['F'] = true;
            IsAllowed['G'] = true;
            IsAllowed['H'] = true;
            IsAllowed['I'] = true;
            IsAllowed['J'] = true;
            IsAllowed['K'] = true;
            IsAllowed['L'] = true;
            IsAllowed['M'] = true;
            IsAllowed['N'] = true;
            IsAllowed['O'] = true;
            IsAllowed['P'] = true;
            IsAllowed['Q'] = true;
            IsAllowed['R'] = true;
            IsAllowed['S'] = true;
            IsAllowed['T'] = true;
            IsAllowed['U'] = true;
            IsAllowed['V'] = true;
            IsAllowed['W'] = true;
            IsAllowed['X'] = true;
            IsAllowed['Y'] = true;
            IsAllowed['Z'] = true;

            IsAllowed['a'] = true;
            IsAllowed['b'] = true;
            IsAllowed['c'] = true;
            IsAllowed['d'] = true;
            IsAllowed['e'] = true;
            IsAllowed['f'] = true;
            IsAllowed['g'] = true;
            IsAllowed['h'] = true;
            IsAllowed['i'] = true;
            IsAllowed['j'] = true;
            IsAllowed['k'] = true;
            IsAllowed['l'] = true;
            IsAllowed['m'] = true;
            IsAllowed['n'] = true;
            IsAllowed['o'] = true;
            IsAllowed['p'] = true;
            IsAllowed['q'] = true;
            IsAllowed['r'] = true;
            IsAllowed['s'] = true;
            IsAllowed['t'] = true;
            IsAllowed['u'] = true;
            IsAllowed['v'] = true;
            IsAllowed['w'] = true;
            IsAllowed['x'] = true;
            IsAllowed['y'] = true;
            IsAllowed['z'] = true;

            IsAllowed['0'] = true;
            IsAllowed['1'] = true;
            IsAllowed['2'] = true;
            IsAllowed['3'] = true;
            IsAllowed['4'] = true;
            IsAllowed['5'] = true;
            IsAllowed['6'] = true;
            IsAllowed['7'] = true;
            IsAllowed['8'] = true;
            IsAllowed['9'] = true;

            IsAllowed['-'] = true;
            IsAllowed['_'] = true;
            IsAllowed['.'] = true;
            IsAllowed['~'] = true;
        }
        static bool[] IsAllowed = new bool[256];
    }
}
