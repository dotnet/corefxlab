// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.Json.Serialization
{
    // Temporary extension methods until we move to corefx
    internal static class TempExtensionMethods
    {
        // TODO: Replace with existing static helper in corefx
        // System/Text/Json/Reader/JsonReaderHelper.Unescaping.cs#L42
        public static string TranscodeHelper(ReadOnlySpan<byte> utf8Unescaped)
        {
            try
            {
#if BUILDING_INBOX_LIBRARY
                return JsonConverter.s_utf8Encoding.GetString(utf8Unescaped);
#else
                if (utf8Unescaped.IsEmpty)
                {
                    return string.Empty;
                }
                unsafe
                {
                    fixed (byte* bytePtr = utf8Unescaped)
                    {
                        return JsonConverter.s_utf8Encoding.GetString(bytePtr, utf8Unescaped.Length);
                    }
                }
#endif
            }
            catch (DecoderFallbackException ex)
            {
                // We want to be consistent with the exception being thrown
                // so the user only has to catch a single exception.
                // Since we already throw InvalidOperationException for mismatch token type,
                // and while unescaping, using that exception for failure to decode invalid UTF-8 bytes as well.
                // Therefore, wrapping the DecoderFallbackException around an InvalidOperationException.
                throw new InvalidOperationException("Cannot transcode invalid UTF-8 JSON text to UTF-16 string.", ex);
            }
        }
    }
}
