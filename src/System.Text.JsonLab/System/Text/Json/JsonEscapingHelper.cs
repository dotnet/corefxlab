// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Text.JsonLab
{
    internal readonly struct JsonEscapingHelper
    {
        private const int ALLOWED_CHARS_BITMAP_LENGTH = 0x10000 / (8 * sizeof(uint));

        /// <summary>
        /// Maximum number of characters that this encoder can generate for each input character.
        /// </summary>
        // The worst case encoding is 6 output chars per input char: [input] U+FFFF -> [output] "\uFFFF"
        // We don't need to worry about astral code points since they're represented as encoded
        // surrogate pairs in the output.
        private const int MaxOutputCharactersPerInputCharacter = 12; // "\uFFFF\uFFFF" is the longest encoded form 

        internal readonly uint[] _allowedCharacters;

        // should be called in place of the default ctor
        public static JsonEscapingHelper CreateNew()
            => new JsonEscapingHelper(new uint[ALLOWED_CHARS_BITMAP_LENGTH]);

        private JsonEscapingHelper(uint[] allowedCharacters)
        {
            Debug.Assert(allowedCharacters.Length > 4);
            _allowedCharacters = allowedCharacters;
            _allowedCharacters[1] = 2952755003;
            _allowedCharacters[2] = 4026531839;
            _allowedCharacters[3] = 2147483646;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int FindFirstCharacterToEncode(ReadOnlySpan<char> text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (!IsCharacterAllowed(text[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        public bool IsCharacterAllowed(char character)
        {
            int codePoint = character;
            int index = codePoint >> 5;
            int offset = codePoint & 0x1F;
            return ((_allowedCharacters[index] >> offset) & 0x1U) != 0;
        }

        public string Encode(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            ReadOnlySpan<char> valueSpan = value.AsSpan();

            int firstCharacterToEncode = FindFirstCharacterToEncode(valueSpan);

            if (firstCharacterToEncode == -1)
                return value;

            int bufferSize = MaxOutputCharactersPerInputCharacter * value.Length;

            char[] array = ArrayPool<char>.Shared.Rent(bufferSize);
            Span<char> wholebuffer = array;
            int totalWritten = EncodeIntoBuffer(valueSpan, wholebuffer, firstCharacterToEncode);

            string str = wholebuffer.Slice(0, totalWritten).ToString();

            ArrayPool<char>.Shared.Return(array);

            return str;
        }

        public int EncodeIntoBuffer(ReadOnlySpan<char> value, Span<char> destination, int firstCharacterToEncode)
        {
            Debug.Assert(value.Length > 0);

            int totalWritten = 0;

            if (firstCharacterToEncode > 0)
            {
                value.Slice(0, firstCharacterToEncode).CopyTo(destination);
                totalWritten += firstCharacterToEncode;
            }

            int valueIndex = firstCharacterToEncode;

            char firstChar = value[valueIndex];
            char secondChar = firstChar;
            bool wasSurrogatePair = false;
            int charsWritten;

            // this loop processes character pairs (in case they are surrogates).
            // there is an if block below to process single last character.
            int secondCharIndex;
            for (secondCharIndex = valueIndex + 1; secondCharIndex < value.Length; secondCharIndex++)
            {
                if (!wasSurrogatePair)
                {
                    firstChar = secondChar;
                }
                else
                {
                    firstChar = value[secondCharIndex - 1];
                }
                secondChar = value[secondCharIndex];

                if (!WillEncode(firstChar))
                {
                    wasSurrogatePair = false;
                    destination[totalWritten++] = firstChar;
                }
                else
                {
                    int nextScalar = JsonEscapeUtilities.GetScalarValueFromUtf16(firstChar, secondChar, out wasSurrogatePair);
                    if (!TryEncodeUnicodeScalar(nextScalar, destination.Slice(totalWritten), out charsWritten))
                    {
                        throw new ArgumentException("Argument encoder does not implement MaxOutputCharsPerInputChar correctly.");
                    }

                    totalWritten += charsWritten;
                    if (wasSurrogatePair)
                    {
                        secondCharIndex++;
                    }
                }
            }

            if (secondCharIndex == value.Length)
            {
                firstChar = value[value.Length - 1];
                int nextScalar = JsonEscapeUtilities.GetScalarValueFromUtf16(firstChar, null, out wasSurrogatePair);
                if (!TryEncodeUnicodeScalar(nextScalar, destination.Slice(totalWritten), out charsWritten))
                {
                    throw new ArgumentException("Argument encoder does not implement MaxOutputCharsPerInputChar correctly.");
                }
                totalWritten += charsWritten;
            }

            return totalWritten;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool WillEncode(int unicodeScalar)
        {
            if (JsonEscapeUtilities.IsSupplementaryCodePoint(unicodeScalar))
                return true;
            return !IsUnicodeScalarAllowed(unicodeScalar);
        }

        // Determines whether the given character can be returned unencoded.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsUnicodeScalarAllowed(int unicodeScalar)
        {
            int index = unicodeScalar >> 5;
            int offset = unicodeScalar & 0x1F;
            return ((_allowedCharacters[index] >> offset) & 0x1U) != 0;
        }

        // Writes a scalar value as a JavaScript-escaped character (or sequence of characters).
        // See ECMA-262, Sec. 7.8.4, and ECMA-404, Sec. 9
        // http://www.ecma-international.org/ecma-262/5.1/#sec-7.8.4
        // http://www.ecma-international.org/publications/files/ECMA-ST/ECMA-404.pdf
        public bool TryEncodeUnicodeScalar(int unicodeScalar, Span<char> buffer, out int numberOfCharactersWritten)
        {
            // ECMA-262 allows encoding U+000B as "\v", but ECMA-404 does not.
            // Both ECMA-262 and ECMA-404 allow encoding U+002F SOLIDUS as "\/".
            // (In ECMA-262 this character is a NonEscape character.)
            // HTML-specific characters (including apostrophe and quotes) will
            // be written out as numeric entities for defense-in-depth.
            // See UnicodeEncoderBase ctor comments for more info.

            if (!WillEncode(unicodeScalar))
                return JsonEscapeUtilities.TryWriteScalarAsChar(unicodeScalar, buffer, out numberOfCharactersWritten);

            if (buffer.Length < 2)
            {
                numberOfCharactersWritten = 0;
                return false;
            }

            buffer[0] = '\\';

            switch (unicodeScalar)
            {
                case '\b':
                    buffer[1] = 'b';
                    break;
                case '\t':
                    buffer[1] = 't';
                    break;
                case '\n':
                    buffer[1] = 'n';
                    break;
                case '\f':
                    buffer[1] = 'f';
                    break;
                case '\r':
                    buffer[1] = 'r';
                    break;
                case '/':
                    buffer[1] = '/';
                    break;
                case '\\':
                    buffer[1] = '\\';
                    break;
                default:
                    return TryWriteEncodedScalarAsNumericEntity(unicodeScalar, buffer, out numberOfCharactersWritten);
            }

            numberOfCharactersWritten = 2;
            return true;
        }

        private bool TryWriteEncodedScalarAsNumericEntity(int unicodeScalar, Span<char> buffer, out int numberOfCharactersWritten)
        {
            if (JsonEscapeUtilities.IsSupplementaryCodePoint(unicodeScalar))
            {
                // Convert this back to UTF-16 and write out both characters.
                JsonEscapeUtilities.GetUtf16SurrogatePairFromAstralScalarValue(unicodeScalar, out char leadingSurrogate, out char trailingSurrogate);

                if (TryWriteEncodedSingleCharacter(leadingSurrogate, buffer, out int leadingSurrogateCharactersWritten) &&
                    TryWriteEncodedSingleCharacter(trailingSurrogate, buffer.Slice(leadingSurrogateCharactersWritten), out numberOfCharactersWritten))
                {
                    numberOfCharactersWritten += leadingSurrogateCharactersWritten;
                    return true;
                }
                else
                {
                    numberOfCharactersWritten = 0;
                    return false;
                }
            }
            else
            {
                // This is only a single character.
                return TryWriteEncodedSingleCharacter(unicodeScalar, buffer, out numberOfCharactersWritten);
            }
        }

        // Writes an encoded scalar value (in the BMP) as a JavaScript-escaped character.
        private bool TryWriteEncodedSingleCharacter(int unicodeScalar, Span<char> buffer, out int numberOfCharactersWritten)
        {
            Debug.Assert(!JsonEscapeUtilities.IsSupplementaryCodePoint(unicodeScalar), "The incoming value should've been in the BMP.");

            if (buffer.Length < 6)
            {
                numberOfCharactersWritten = 0;
                return false;
            }

            // Encode this as 6 chars "\uFFFF".
            buffer[0] = '\\';
            buffer[1] = 'u';
            buffer[2] = JsonEscapeUtilities.Int32LsbToHexDigit(unicodeScalar >> 12);
            buffer[3] = JsonEscapeUtilities.Int32LsbToHexDigit((int)((unicodeScalar >> 8) & 0xFU));
            buffer[4] = JsonEscapeUtilities.Int32LsbToHexDigit((int)((unicodeScalar >> 4) & 0xFU));
            buffer[5] = JsonEscapeUtilities.Int32LsbToHexDigit((int)(unicodeScalar & 0xFU));

            numberOfCharactersWritten = 6;
            return true;
        }
    }
}
