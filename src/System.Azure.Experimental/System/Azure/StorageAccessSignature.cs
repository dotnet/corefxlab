// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Cryptography;
using System.Text.Utf8;
using System.Binary.Base64Experimental;
using System.Buffers.Text;
using System.Buffers;

namespace System.Azure.Authentication
{
    public static class StorageAccessSignature
    {
        private static Utf8String s_emptyHeaders = (Utf8String)"\n\n\n\n\n\n\n\n\n\n\nx-ms-date:"; // this wont be needed once we have UTF8 literals
        private static TransformationFormat s_toBase64 = new TransformationFormat(Base64Experimental.BytesToUtf8Encoder);

        public static bool TryWrite(Span<byte> output, Sha256 hash, string verb, string canonicalizedResource, DateTime utc, out int bytesWritten)
        {
            try
            {
                var writer = BufferWriter.Create(output);
                writer.WriteLine(verb);
                writer.Write("\n\n\n\n\n\n\n\n\n\n\nx-ms-date:");
                writer.WriteLine(utc, 'R');
                writer.Write(canonicalizedResource);
                hash.Append(writer.Written);
                writer.WrittenCount = 0;
                writer.WriteBytes(hash, s_toBase64);
                bytesWritten = writer.WrittenCount;
                return true;
            }
            catch (BufferWriter.BufferTooSmallException)
            {
                bytesWritten = 0;
                return false;
            }
        }

        public static bool TryWrite(Span<byte> output, Sha256 hash, Utf8Span verb, Utf8Span canonicalizedResource, DateTime utc, out int bytesWritten)
        {
            try
            {
                var writer = BufferWriter.Create(output);
                writer.WriteLine(verb);
                writer.Write(s_emptyHeaders);
                writer.WriteLine(utc, 'R');
                writer.Write(canonicalizedResource);
                hash.Append(writer.Written);
                writer.WrittenCount = 0;
                writer.WriteBytes(hash, s_toBase64);
                bytesWritten = writer.WrittenCount;
                return true;
            }
            catch (BufferWriter.BufferTooSmallException)
            {
                bytesWritten = 0;
                return false;
            }
        }
    }
}
