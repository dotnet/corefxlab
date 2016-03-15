// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Text.Utf16
{
    public static class Utf16LittleEndianEncoder
    {
        const uint MaskLow10Bits = 0x3FF;

        public static bool TryDecodeCodePoint(ReadOnlySpan<byte> buffer, out UnicodeCodePoint codePoint, out int encodedBytes)
        {
            if (buffer.Length < 2)
            {
                codePoint = default(UnicodeCodePoint);
                encodedBytes = default(int);
                // buffer too small
                return false;
            }

            uint codePointValue = buffer.Read<ushort>();
            encodedBytes = 2;
            if (UnicodeCodePoint.IsSurrogate((UnicodeCodePoint)codePointValue))
            {
                // TODO: Check if compiler optimized it so codePointValue low range is checked only once
                if (!UnicodeCodePoint.IsHighSurrogate((UnicodeCodePoint)codePointValue) || buffer.Length < 4)
                {
                    codePoint = default(UnicodeCodePoint);
                    encodedBytes = default(int);
                    // invalid high surrogate or buffer too small
                    return false;
                }
                unchecked
                {
                    codePointValue -= UnicodeConstants.Utf16HighSurrogateFirstCodePoint;
                    encodedBytes += 2;
                }
                // high surrogate contains 10 first bits of the code point
                codePointValue <<= 10;

                uint lowSurrogate = buffer.Read<uint>() >> 16;
                if (!UnicodeCodePoint.IsLowSurrogate((UnicodeCodePoint)lowSurrogate))
                {
                    codePoint = default(UnicodeCodePoint);
                    encodedBytes = default(int);
                    // invalid low surrogate character
                    return false;
                }

                unchecked
                {
                    lowSurrogate -= UnicodeConstants.Utf16LowSurrogateFirstCodePoint;
                }
                codePointValue |= lowSurrogate;
            }

            codePoint = (UnicodeCodePoint)codePointValue;

            return true;
        }

        public static bool TryEncodeCodePoint(UnicodeCodePoint codePoint, Span<byte> buffer, out int encodedBytes)
        {
            if (!UnicodeCodePoint.IsSupportedCodePoint(codePoint))
            {
                encodedBytes = default(int);
                return false;
            }

            // TODO: Can we add this in UnicodeCodePoint class?
            // Should be represented as Surrogate?
            encodedBytes = ((uint)codePoint >= 0x10000) ? 4 : 2;

            if (buffer.Length < encodedBytes)
            {
                codePoint = default(UnicodeCodePoint);
                encodedBytes = default(int);
                // buffer too small
                return false;
            }

            if (encodedBytes == 2)
            {
                unchecked
                {
                    buffer.Write((ushort)codePoint);
                }
            }
            else
            {
                unchecked
                {
                    uint highSurrogate = ((uint)codePoint >> 10) + UnicodeConstants.Utf16HighSurrogateFirstCodePoint;
                    uint lowSurrogate = ((uint)codePoint & MaskLow10Bits) + UnicodeConstants.Utf16LowSurrogateFirstCodePoint;

                    buffer.Write(highSurrogate | (lowSurrogate << 16));
                }
            }
            return true;
        }

        // TODO: Should we rewrite this to not use char.ConvertToUtf32 or is it fast enough?
        public static bool TryDecodeCodePointFromString(string s, int index, out UnicodeCodePoint codePoint, out int encodedChars)
        {
            if (index < 0 || index >= s.Length)
            {
                codePoint = default(UnicodeCodePoint);
                encodedChars = 0;
                return false;
            }

            if (index == s.Length - 1 && char.IsSurrogate(s[index]))
            {
                codePoint = default(UnicodeCodePoint);
                encodedChars = 0;
                return false;
            }

            encodedChars = char.IsHighSurrogate(s[index]) ? 2 : 1;
            codePoint = (UnicodeCodePoint)(unchecked((uint)char.ConvertToUtf32(s, index)));

            return true;
        }
    }
}
