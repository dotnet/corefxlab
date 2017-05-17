// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System.Text.Json
{
    public struct JsonReader
    {
        // We are using a ulong to represent our nested state, so we can only go 64 levels deep.
        private const int MaxDepth = sizeof(ulong) * 8;

        private readonly JsonEncoderState _encoderState;
        private readonly TextEncoder _encoder;

        private ReadOnlySpan<byte> _buffer;

        // Depth tracks the recursive depth of the nested objects / arrays within the JSON data.
        private int _depth;

        // This mask represents a tiny stack to track the state during nested transitions.
        // The first bit represents the state of the current level (1 == object, 0 == array).
        // Each subsequent bit is the parent / containing type (object or array). Since this
        // reader does a linear scan, we only need to keep a single path as we go through the data.
        private ulong _containerMask;

        // These next 2 properties are used to check for whether we can take the fast path
        // for invariant UTF-8 or UTF-16 processing. Otherwise, we need to go through the
        // slow path which makes use of the (possibly generic) encoder.
        private bool UseFastUtf8 => _encoderState == JsonEncoderState.UseFastUtf8;
        private bool UseFastUtf16 => _encoderState == JsonEncoderState.UseFastUtf16;

        // These properties are helpers for determining the current state of the reader
        private bool IsRoot => _depth == 1;
        private bool InArray => (_containerMask & 1) == 0 && (_depth > 0);
        private bool InObject => (_containerMask & 1) == 1;

        /// <summary>
        /// Gets the token type of the last processed token in the JSON stream.
        /// </summary>
        public JsonTokenType TokenType { get; private set; }

        /// <summary>
        /// Gets the value as a ReadOnlySpan<byte> of the last processed token. The contents of this
        /// is only relevant when <see cref="TokenType" /> is <see cref="JsonTokenType.Value" /> or
        /// <see cref="JsonTokenType.PropertyName" />. Otherwise, this value should be set to
        /// <see cref="ReadOnlySpan{T}.Empty"/>.
        /// </summary>
        public ReadOnlySpan<byte> Value { get; private set; }

        /// <summary>
        /// Gets the JSON value type of the last processed token. The contents of this
        /// is only relevant when <see cref="TokenType" /> is <see cref="JsonTokenType.Value" /> or
        /// <see cref="JsonTokenType.PropertyName" />.
        /// </summary>
        public JsonValueType ValueType { get; private set; }

        /// <summary>
        /// Gets the encoder instance used when the reader was constructed.
        /// </summary>
        public TextEncoder Encoder => _encoder;

        /// <summary>
        /// Constructs a new JsonReader instance. This is a stack-only type.
        /// </summary>
        /// <param name="data">The <see cref="Span{byte}"/> value to consume. </param>
        /// <param name="encoder">An encoder used for decoding bytes from <paramref name="data"/> into characters.</param>
        public JsonReader(ReadOnlySpan<byte> data, TextEncoder encoder)
        {
            _buffer = data;
            _encoder = encoder;
            _depth = 0;
            _containerMask = 0;

            if (encoder.IsInvariantUtf8)
                _encoderState = JsonEncoderState.UseFastUtf8;
            else if (encoder.IsInvariantUtf16)
                _encoderState = JsonEncoderState.UseFastUtf16;
            else
                _encoderState = JsonEncoderState.UseFullEncoder;

            TokenType = JsonTokenType.None;
            Value = ReadOnlySpan<byte>.Empty;
            ValueType = JsonValueType.Unknown;
        }

        /// <summary>
        /// Read the next token from the data buffer.
        /// </summary>
        /// <returns>True if the token was read successfully, else false.</returns>
        public bool Read()
        {
            ref byte bytes = ref _buffer.DangerousGetPinnableReference();
            int count = _buffer.Length;
            int skip = SkipWhiteSpace(ref bytes, count);

            ref byte next = ref Unsafe.Add(ref bytes, skip);
            count -= skip;

            int step = GetNextCharAscii(ref next, count, out char ch);
            if (step == 0) return false;

            switch (TokenType)
            {
                case JsonTokenType.None:
                    if (ch == JsonConstants.OpenBrace)
                        StartObject();
                    else if (ch == JsonConstants.OpenBracket)
                        StartArray();
                    else
                        throw new JsonReaderException();
                    break;

                case JsonTokenType.StartObject:
                    if (ch == JsonConstants.CloseBrace)
                        EndObject();
                    else
                        step = ConsumePropertyName(ref next, count);
                    break;

                case JsonTokenType.StartArray:
                case JsonTokenType.PropertyName:
                    step = ConsumeValue(ch, step, ref next, count);
                    break;

                case JsonTokenType.EndArray:
                case JsonTokenType.EndObject:
                case JsonTokenType.Value:
                    step = ConsumeNext(ch, step, ref next, count);
                    if (step == 0) return false;
                    break;
            }

            _buffer = _buffer.Slice(skip + step);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void StartObject()
        {
            if (_depth > MaxDepth)
                throw new JsonReaderException();

            _depth++;
            _containerMask = (_containerMask << 1) | 1;
            TokenType = JsonTokenType.StartObject;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EndObject()
        {
            if (!InObject || _depth <= 0)
                throw new JsonReaderException();

            _depth--;
            _containerMask >>= 1;
            TokenType = JsonTokenType.EndObject;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void StartArray()
        {
            if (_depth > MaxDepth)
                throw new JsonReaderException();

            _depth++;
            _containerMask = (_containerMask << 1);
            TokenType = JsonTokenType.StartArray;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EndArray()
        {
            if (!InArray || _depth <= 0)
                throw new JsonReaderException();

            _depth--;
            _containerMask >>= 1;
            TokenType = JsonTokenType.EndArray;
        }

        /// <summary>
        /// This method consumes the next token regardless of whether we are inside an object or an array.
        /// For an object, it reads the next property name token. For an array, it just reads the next value.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumeNext(char marker, int markerBytes, ref byte src, int count)
        {
            int skip = markerBytes;

            switch (marker)
            {
                case JsonConstants.ListSeperator:
                    {
                        skip += SkipWhiteSpace(ref Unsafe.Add(ref src, markerBytes), count - markerBytes);
                        count -= skip;
                        ref byte next = ref Unsafe.Add(ref src, skip);
                        if (InObject)
                            return skip + ConsumePropertyName(ref next, count);
                        else if (InArray)
                        {
                            int step = GetNextCharAscii(ref next, count, out char ch);
                            if (step == 0) return 0;
                            return skip + ConsumeValue(ch, step, ref next, count);
                        }
                        else
                            throw new JsonReaderException();
                    }

                case JsonConstants.CloseBrace:
                    EndObject();
                    return skip;

                case JsonConstants.CloseBracket:
                    EndArray();
                    return skip;

                default:
                    throw new JsonReaderException();
            }
        }

        /// <summary>
        /// This method contains the logic for processing the next value token and determining
        /// what type of data it is.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumeValue(char marker, int markerBytes, ref byte src, int count)
        {
            TokenType = JsonTokenType.Value;

            switch (marker)
            {
                case JsonConstants.Quote:
                    return ConsumeString(ref src, count);

                case JsonConstants.OpenBrace:
                    StartObject();
                    return markerBytes;

                case JsonConstants.OpenBracket:
                    StartArray();
                    return markerBytes;

                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return ConsumeNumber(ref src, count, false);

                case '-':
                    int step = GetNextCharAscii(ref src, count, out char ch);
                    if (step == 0) throw new JsonReaderException();
                    return (ch == 'I')
                        ? ConsumeInfinity(ref src, count, true)
                        : ConsumeNumber(ref src, count, true);

                case 'f':
                    return ConsumeFalse(ref src, count);

                case 't':
                    return ConsumeTrue(ref src, count);

                case 'n':
                    return ConsumeNull(ref src, count);

                case 'u':
                    return ConsumeUndefined(ref src, count);

                case 'N':
                    return ConsumeNaN(ref src, count);

                case 'I':
                    return ConsumeInfinity(ref src, count, false);

                case '/':
                    // TODO: Comments?
                    throw new NotImplementedException();
            }

            return 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumeNumber(ref byte src, int count, bool negative)
        {
            if (UseFastUtf8)
            {
                unsafe
                {
                    fixed (byte* pSrc = &src)
                    {
                        if (!PrimitiveParser.InvariantUtf8.TryParseInt64(pSrc, count, out long value, out int consumed))
                            throw new JsonReaderException();

                        // TODO: We need to do something with the value here.

                        // Calculate the real start of the number based on our current buffer location.
                        int startIndex = (int)Unsafe.ByteOffset(ref _buffer.DangerousGetPinnableReference(), ref src);

                        Value = _buffer.Slice(startIndex, consumed);
                        ValueType = JsonValueType.Number;
                        return consumed;
                    }
                }
            }
            else if (UseFastUtf16)
            {
                unsafe
                {
                    fixed (byte* pSrc = &src)
                    {
                        char* pChars = (char*)pSrc;
                        if (!PrimitiveParser.InvariantUtf16.TryParseInt64(pChars, count >> 1, out long value, out int consumed))
                            throw new JsonReaderException();

                        // TODO: We need to do something with the value here.

                        // Calculate the real start of the number based on our current buffer location.
                        int startIndex = (int)Unsafe.ByteOffset(ref _buffer.DangerousGetPinnableReference(), ref src);

                        // consumed is in characters, but our buffer is in bytes, so we need to double it for buffer slicing.
                        int bytesConsumed = consumed << 1;

                        Value = _buffer.Slice(startIndex, bytesConsumed);
                        ValueType = JsonValueType.Number;
                        return bytesConsumed;
                    }
                }
            }
            else
                throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumeNaN(ref byte src, int count)
        {
            Value = ReadOnlySpan<byte>.Empty;
            ValueType = JsonValueType.NaN;

            if (UseFastUtf8)
            {
                if (count < 3
                    || Unsafe.Add(ref src, 0) != 'N'
                    || Unsafe.Add(ref src, 1) != 'a'
                    || Unsafe.Add(ref src, 2) != 'N')
                {
                    throw new JsonReaderException();
                }

                return 3;
            }
            else if (UseFastUtf16)
            {
                ref char chars = ref Unsafe.As<byte, char>(ref src);
                if (count < 6
                    || Unsafe.Add(ref chars, 0) != 'N'
                    || Unsafe.Add(ref chars, 1) != 'a'
                    || Unsafe.Add(ref chars, 2) != 'N')
                {
                    throw new JsonReaderException();
                }

                return 6;
            }
            else
                throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumeNull(ref byte src, int count)
        {
            Value = ReadOnlySpan<byte>.Empty;
            ValueType = JsonValueType.Null;

            if (UseFastUtf8)
            {
                if (count < 4
                    || Unsafe.Add(ref src, 0) != 'n'
                    || Unsafe.Add(ref src, 1) != 'u'
                    || Unsafe.Add(ref src, 2) != 'l'
                    || Unsafe.Add(ref src, 3) != 'l')
                {
                    throw new JsonReaderException();
                }

                return 4;
            }
            else if (UseFastUtf16)
            {
                ref char chars = ref Unsafe.As<byte, char>(ref src);
                if (count < 8
                    || Unsafe.Add(ref chars, 0) != 'n'
                    || Unsafe.Add(ref chars, 1) != 'u'
                    || Unsafe.Add(ref chars, 2) != 'l'
                    || Unsafe.Add(ref chars, 3) != 'l')
                {
                    throw new JsonReaderException();
                }

                return 8;
            }
            else
                throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumeInfinity(ref byte src, int count, bool negative)
        {
            Value = ReadOnlySpan<byte>.Empty;
            ValueType = !negative ? JsonValueType.Infinity : JsonValueType.NegativeInfinity;

            int idx = negative ? 1 : 0;
            if (UseFastUtf8)
            {
                if (count < 8 + idx
                    || Unsafe.Add(ref src, idx++) != 'I'
                    || Unsafe.Add(ref src, idx++) != 'n'
                    || Unsafe.Add(ref src, idx++) != 'f'
                    || Unsafe.Add(ref src, idx++) != 'i'
                    || Unsafe.Add(ref src, idx++) != 'n'
                    || Unsafe.Add(ref src, idx++) != 'i'
                    || Unsafe.Add(ref src, idx++) != 't'
                    || Unsafe.Add(ref src, idx++) != 'y')
                {
                    throw new JsonReaderException();
                }

                return idx;
            }
            else if (UseFastUtf16)
            {
                ref char chars = ref Unsafe.As<byte, char>(ref src);
                if (count < 16 + idx
                    || Unsafe.Add(ref chars, idx++) != 'I'
                    || Unsafe.Add(ref chars, idx++) != 'n'
                    || Unsafe.Add(ref chars, idx++) != 'f'
                    || Unsafe.Add(ref chars, idx++) != 'i'
                    || Unsafe.Add(ref chars, idx++) != 'n'
                    || Unsafe.Add(ref chars, idx++) != 'i'
                    || Unsafe.Add(ref chars, idx++) != 't'
                    || Unsafe.Add(ref chars, idx++) != 'y')
                {
                    throw new JsonReaderException();
                }

                return idx;
            }
            else
                throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumeUndefined(ref byte src, int count)
        {
            Value = ReadOnlySpan<byte>.Empty;
            ValueType = JsonValueType.Undefined;

            if (UseFastUtf8)
            {
                if (count < 9
                    || Unsafe.Add(ref src, 0) != 'u'
                    || Unsafe.Add(ref src, 1) != 'n'
                    || Unsafe.Add(ref src, 2) != 'd'
                    || Unsafe.Add(ref src, 3) != 'e'
                    || Unsafe.Add(ref src, 4) != 'f'
                    || Unsafe.Add(ref src, 5) != 'i'
                    || Unsafe.Add(ref src, 6) != 'n'
                    || Unsafe.Add(ref src, 7) != 'e'
                    || Unsafe.Add(ref src, 8) != 'd')
                {
                    throw new JsonReaderException();
                }

                return 9;
            }
            else if (UseFastUtf16)
            {
                ref char chars = ref Unsafe.As<byte, char>(ref src);
                if (count < 18
                    || Unsafe.Add(ref chars, 0) != 'u'
                    || Unsafe.Add(ref chars, 1) != 'n'
                    || Unsafe.Add(ref chars, 2) != 'd'
                    || Unsafe.Add(ref chars, 3) != 'e'
                    || Unsafe.Add(ref chars, 4) != 'f'
                    || Unsafe.Add(ref chars, 5) != 'i'
                    || Unsafe.Add(ref chars, 6) != 'n'
                    || Unsafe.Add(ref chars, 7) != 'e'
                    || Unsafe.Add(ref chars, 8) != 'd')
                {
                    throw new JsonReaderException();
                }

                return 18;
            }
            else
                throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumeFalse(ref byte src, int count)
        {
            Value = ReadOnlySpan<byte>.Empty;
            ValueType = JsonValueType.False;

            if (UseFastUtf8)
            {
                if (count < 5
                    || Unsafe.Add(ref src, 0) != 'f'
                    || Unsafe.Add(ref src, 1) != 'a'
                    || Unsafe.Add(ref src, 2) != 'l'
                    || Unsafe.Add(ref src, 3) != 's'
                    || Unsafe.Add(ref src, 4) != 'e')
                {
                    throw new JsonReaderException();
                }

                return 5;
            }
            else if (UseFastUtf16)
            {
                ref char chars = ref Unsafe.As<byte, char>(ref src);
                if (count < 10
                    || Unsafe.Add(ref chars, 0) != 'f'
                    || Unsafe.Add(ref chars, 1) != 'a'
                    || Unsafe.Add(ref chars, 2) != 'l'
                    || Unsafe.Add(ref chars, 3) != 's'
                    || Unsafe.Add(ref chars, 4) != 'e')
                {
                    throw new JsonReaderException();
                }

                return 10;
            }
            else
                throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumeTrue(ref byte src, int count)
        {
            Value = ReadOnlySpan<byte>.Empty;
            ValueType = JsonValueType.True;

            if (UseFastUtf8)
            {
                if (count < 4
                    || Unsafe.Add(ref src, 0) != 't'
                    || Unsafe.Add(ref src, 1) != 'r'
                    || Unsafe.Add(ref src, 2) != 'u'
                    || Unsafe.Add(ref src, 3) != 'e')
                {
                    throw new JsonReaderException();
                }

                return 4;
            }
            else if (UseFastUtf16)
            {
                ref char chars = ref Unsafe.As<byte, char>(ref src);
                if (count < 8
                    || Unsafe.Add(ref chars, 0) != 't'
                    || Unsafe.Add(ref chars, 1) != 'r'
                    || Unsafe.Add(ref chars, 2) != 'u'
                    || Unsafe.Add(ref chars, 3) != 'e')
                {
                    throw new JsonReaderException();
                }

                return 8;
            }
            else
                throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumePropertyName(ref byte src, int count)
        {
            if (UseFastUtf8)
                return ConsumePropertyNameUtf8(ref src, count);
            else if (UseFastUtf16)
                return ConsumePropertyNameUtf16(ref src, count);
            else
                return ConsumePropertyNameSlow(ref src, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumePropertyNameUtf8(ref byte src, int count)
        {
            int consumed = ConsumeStringUtf8(ref src, count);
            if (consumed == 0) throw new JsonReaderException();

            consumed += SkipWhiteSpaceUtf8(ref Unsafe.Add(ref src, consumed), count - consumed);
            if (consumed >= count) throw new JsonReaderException();

            // The next character must be a key / value seperator. Validate and skip.
            if (Unsafe.Add(ref src, consumed++) != JsonConstants.KeyValueSeperator)
                throw new JsonReaderException();

            TokenType = JsonTokenType.PropertyName;
            return consumed;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumePropertyNameUtf16(ref byte src, int count)
        {
            int consumed = ConsumeStringUtf16(ref src, count);
            if (consumed == 0) throw new JsonReaderException();

            consumed += SkipWhiteSpaceUtf16(ref Unsafe.Add(ref src, consumed), count - consumed);
            if (consumed >= count) throw new JsonReaderException();

            // The next character must be a key / value seperator
            if (Unsafe.As<byte, char>(ref Unsafe.Add(ref src, consumed)) != JsonConstants.KeyValueSeperator)
                throw new JsonReaderException();

            consumed += 2; // Skip the key / value seperator
            TokenType = JsonTokenType.PropertyName;
            return consumed;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumePropertyNameSlow(ref byte src, int count)
        {
            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumeString(ref byte src, int count)
        {
            if (UseFastUtf8)
                return ConsumeStringUtf8(ref src, count);
            else if (UseFastUtf16)
                return ConsumeStringUtf16(ref src, count);
            else
                return ConsumeStringSlow(ref src, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumeStringUtf8(ref byte src, int count)
        {
            // The first character MUST be a JSON string quote
            if (src != JsonConstants.Quote) throw new JsonReaderException();

            // If we are in this method, the first char is already known to be a JSON quote character.
            // Skip through the bytes until we find the closing JSON quote character.
            int idx = 1;
            while (idx < count && Unsafe.Add(ref src, idx++) != JsonConstants.Quote) ;

            // If we hit the end of the source and never saw an ending quote, then fail.
            if (idx == count && Unsafe.Add(ref src, idx - 1) != JsonConstants.Quote)
                throw new JsonReaderException();

            // Calculate the real start of the property name based on our current buffer location.
            // Also, skip the opening JSON quote character.
            int startIndex = (int)Unsafe.ByteOffset(ref _buffer.DangerousGetPinnableReference(), ref src) + 1;

            Value = _buffer.Slice(startIndex, idx - 2); // -2 to exclude the quote characters.
            ValueType = JsonValueType.String;
            return idx;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumeStringUtf16(ref byte src, int count)
        {
            ref char chars = ref Unsafe.As<byte, char>(ref src);
            count >>= 1; // sizeof(char) is 2, so we have half as many characters as count.

            // The first character MUST be a JSON string quote
            if (chars != JsonConstants.Quote) throw new JsonReaderException();

            // Skip through the bytes until we find the closing JSON quote character.
            int idx = 1;
            while (idx < count && Unsafe.Add(ref chars, idx++) != JsonConstants.Quote) ;

            // If we hit the end of the source and never saw an ending quote, then fail.
            if (idx == count && Unsafe.Add(ref chars, idx - 1) != JsonConstants.Quote)
                throw new JsonReaderException();

            // Calculate the real start of the property name based on our current buffer location.
            // Also, skip the opening JSON quote character.
            int startIndex = (int)Unsafe.ByteOffset(ref _buffer.DangerousGetPinnableReference(), ref src) + 2;

            // idx is in characters, but our buffer is in bytes, so we need to double it for buffer slicing.
            // Also, remove the quote characters as well.
            int length = (idx - 2) << 1;

            Value = _buffer.Slice(startIndex, length);
            ValueType = JsonValueType.String;
            return idx * sizeof(char);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumeStringSlow(ref byte src, int count)
        {
            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int SkipWhiteSpace(ref byte src, int count)
        {
            if (UseFastUtf8)
                return SkipWhiteSpaceUtf8(ref src, count);
            else if (UseFastUtf16)
                return SkipWhiteSpaceUtf16(ref src, count);
            else
                return SkipWhiteSpaceSlow(ref src, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int SkipWhiteSpaceUtf8(ref byte src, int count)
        {
            int idx = 0;
            while (idx < count)
            {
                switch (Unsafe.Add(ref src, idx))
                {
                    case (byte)JsonConstants.Space:
                    case (byte)JsonConstants.CarriageReturn:
                    case (byte)JsonConstants.LineFeed:
                    case (byte)'\t':
                        idx++;
                        break;

                    default:
                        return idx;
                }
            }

            return idx;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int SkipWhiteSpaceUtf16(ref byte src, int count)
        {
            ref char chars = ref Unsafe.As<byte, char>(ref src);
            count >>= 1;

            int idx = 0;
            while (idx < count)
            {
                switch (Unsafe.Add(ref chars, idx))
                {
                    case JsonConstants.Space:
                    case JsonConstants.CarriageReturn:
                    case JsonConstants.LineFeed:
                    case '\t':
                        idx++;
                        break;

                    default:
                        return idx * sizeof(char);
                }
            }

            return idx * sizeof(char);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int SkipWhiteSpaceSlow(ref byte src, int count)
        {
            int idx = 0;
            while (idx < count)
            {
                int skip = GetNextCharAscii(ref Unsafe.Add(ref src, idx), count, out char ch);
                if (skip == 0)
                    break;

                switch (ch)
                {
                    case JsonConstants.Space:
                    case JsonConstants.CarriageReturn:
                    case JsonConstants.LineFeed:
                    case '\t':
                        idx += skip;
                        break;

                    default:
                        return idx;
                }
            }

            return idx;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetNextCharAscii(ref byte src, int count, out char ch)
        {
            if (UseFastUtf8)
            {
                if (count < 1)
                {
                    ch = default(char);
                    return 0;
                }

                ch = (char)src;
                return 1;
            }
            else if (UseFastUtf16)
            {
                if (count < 2)
                {
                    ch = default(char);
                    return 0;
                }

                ch = Unsafe.As<byte, char>(ref src);
                return 2;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
