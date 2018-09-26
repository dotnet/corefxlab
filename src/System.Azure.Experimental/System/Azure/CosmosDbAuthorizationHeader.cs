// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Binary.Base64Experimental;
using System.Buffers;
using System.Buffers.Cryptography;
using System.Buffers.Text;
using System.Buffers.Writer;
using System.Text.Encodings.Web.Utf8;
using System.Text.Utf8;
using static System.Buffers.Text.Encodings;

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

        private static TransformationFormat s_toBase64 = new TransformationFormat(Base64Experimental.BytesToUtf8Encoder);
        private static TransformationFormat s_toLower = new TransformationFormat(Ascii.ToLowercase);

        public static bool TryWrite(Span<byte> output, Sha256 hash, string keyType, string verb, string resourceId, string resourceType, string tokenVersion, DateTime utc, out int bytesWritten)
        {
            Span<byte> buffer = stackalloc byte[AuthenticationHeaderBufferSize];
            var writer = BufferWriter.Create(buffer);
            writer.Enlarge = (minumumSize) => { return new byte[minumumSize * 2]; };

            // compute signature hash
            writer.WriteLine(verb, s_toLower);
            writer.WriteLine(resourceType, s_toLower);
            writer.WriteLine(resourceId, s_toLower);
            writer.WriteLine(utc, 'l');
            writer.Write('\n');
            hash.Append(writer.Written);

            // combine token
            writer.WrittenCount = 0; // reuse writer and buffer
            writer.Write("type=");
            writer.Write(keyType);
            writer.Write("&ver=");
            writer.Write(tokenVersion);
            writer.Write("&sig=");

            writer.WriteBytes(hash, s_toBase64);

            if (UrlEncoder.Utf8.Encode(writer.Written, output, out var consumed, out bytesWritten) != OperationStatus.Done)
            {
                bytesWritten = 0;
                return false;
            }
            return true;
        }

        public static bool TryWrite(Span<byte> output, Sha256 hash, Utf8Span keyType, Utf8Span verb, Utf8Span resourceId, Utf8Span resourceType, Utf8Span tokenVersion, DateTime utc, out int bytesWritten)
        {
            Span<byte> buffer = stackalloc byte[AuthenticationHeaderBufferSize];
            var writer = BufferWriter.Create(buffer);
            writer.Enlarge = (minumumSize) => { return new byte[minumumSize * 2]; };

            // compute signature hash
            writer.WriteLine(verb, s_toLower);
            writer.WriteLine(resourceType, s_toLower);
            writer.WriteLine(resourceId, s_toLower);
            writer.WriteLine(utc, 'l');
            writer.Write('\n');
            hash.Append(writer.Written);

            // combine token
            writer.WrittenCount = 0; // reuse writer and buffer
            writer.Write(s_typeLiteral);
            writer.Write(keyType);
            writer.Write(s_verLiteral);
            writer.Write(tokenVersion);
            writer.Write(s_sigLiteral);

            writer.WriteBytes(hash, s_toBase64);

            if (UrlEncoder.Utf8.Encode(writer.Written, output, out var consumed, out bytesWritten) != OperationStatus.Done)
            {
                bytesWritten = 0;
                return false;
            }
            return true;
        }

        public bool TryFormat(Span<byte> buffer, out int written, StandardFormat format = default, SymbolTable symbolTable = null)
        {
            if (TryWrite(buffer, Hash, KeyType, Method, ResourceId, ResourceType, Version, Time, out written))
            {
                return true;
            }
            buffer.Clear();
            written = 0;
            return false;
        }

        const int AuthenticationHeaderBufferSize = 256;

        // These will go away once we have UTF8 literals in the language
        static readonly Utf8String s_typeLiteral = (Utf8String)"type=";
        static readonly Utf8String s_verLiteral = (Utf8String)"&ver=";
        static readonly Utf8String s_sigLiteral = (Utf8String)"&sig=";
    }
}
