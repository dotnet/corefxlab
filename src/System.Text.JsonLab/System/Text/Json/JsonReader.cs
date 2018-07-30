// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Buffers.Reader;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Text.JsonLab
{
    public ref struct Utf8JsonReader
    {
        // We are using a ulong to represent our nested state, so we can only go 64 levels deep.
        private const int MaxDepth = sizeof(ulong) * 8;

        private ReadOnlySpan<byte> _buffer;

        private BufferReader _reader;

        // Depth tracks the recursive depth of the nested objects / arrays within the JSON data.
        private int _depth;

        // This mask represents a tiny stack to track the state during nested transitions.
        // The first bit represents the state of the current level (1 == object, 0 == array).
        // Each subsequent bit is the parent / containing type (object or array). Since this
        // reader does a linear scan, we only need to keep a single path as we go through the data.
        private ulong _containerMask;

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

        readonly bool _isSingleSegment;

        /// <summary>
        /// Constructs a new JsonReader instance. This is a stack-only type.
        /// </summary>
        /// <param name="data">The <see cref="Span{byte}"/> value to consume. </param>
        /// <param name="encoder">An encoder used for decoding bytes from <paramref name="data"/> into characters.</param>
        public Utf8JsonReader(ReadOnlySpan<byte> data)
        {
            _reader = default;
            _isSingleSegment = true;
            _buffer = data;
            _depth = 0;
            _containerMask = 0;

            TokenType = JsonTokenType.None;
            Value = ReadOnlySpan<byte>.Empty;
            ValueType = JsonValueType.Unknown;
        }

        public Utf8JsonReader(in ReadOnlySequence<byte> data)
        {
            _reader = BufferReader.Create(data);
            _isSingleSegment = _reader.Sequence.IsSingleSegment;
            _buffer = data.First.Span;
            _depth = 0;
            _containerMask = 0;

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
            if (_isSingleSegment)
            {
                return ReadSingleSegment();
            }
            else
            {
                return ReadMultiSegment();
            }
        }

        private void SkipWhiteSpace()
        {
            while (true)
            {
                int val = _reader.Peek();
                if (val != JsonConstants.Space && val != JsonConstants.CarriageReturn && val != JsonConstants.LineFeed && val != '\t')
                    break;
                _reader.Advance(1);
            }
        }

        private bool ReadMultiSegment()
        {
            SkipWhiteSpace();
            if (_reader.UnreadSegment.Length < 1)
            {
                return false;
            }

            int ch = _reader.Peek();

            switch (TokenType)
            {
                case JsonTokenType.None:
                    _reader.Advance(1);
                    if (ch == JsonConstants.OpenBrace)
                        StartObject();
                    else if (ch == JsonConstants.OpenBracket)
                        StartArray();
                    else
                        JsonThrowHelper.ThrowJsonReaderException();
                    break;
                case JsonTokenType.StartObject:
                    _reader.Advance(1);
                    if (ch == JsonConstants.CloseBrace)
                        EndObject();
                    else
                    {
                        if (ch != JsonConstants.Quote) JsonThrowHelper.ThrowJsonReaderException();
                        ConsumePropertyNameUtf8MultiSegment();
                    }
                    break;
                case JsonTokenType.StartArray:
                    if (ch == JsonConstants.CloseBracket)
                    {
                        _reader.Advance(1);
                        EndArray();
                    }
                    else
                        ConsumeValueUtf8MultiSegment((char)ch);
                    break;
                case JsonTokenType.PropertyName:
                    if (ConsumeValueUtf8MultiSegment((char)ch) == 0) return false;
                    break;
                case JsonTokenType.EndObject:
                case JsonTokenType.Value:
                case JsonTokenType.EndArray:
                    if (ConsumeNextUtf8MultiSegment((char)ch) == 0) return false;
                    break;
            }

            return true;
        }

        private bool ReadSingleSegment()
        {
            ref byte bytes = ref MemoryMarshal.GetReference(_buffer);
            int length = _buffer.Length;
            int skip = SkipWhiteSpaceUtf8(ref bytes, length);

            ref byte next = ref Unsafe.Add(ref bytes, skip);
            length -= skip;

            if (length < 1)
            {
                return false;
            }

            char ch = (char)next;
            int step = 1;

            switch (TokenType)
            {
                case JsonTokenType.None:
                    if (ch == JsonConstants.OpenBrace)
                        StartObject();
                    else if (ch == JsonConstants.OpenBracket)
                        StartArray();
                    else
                        JsonThrowHelper.ThrowJsonReaderException();
                    break;

                case JsonTokenType.StartObject:
                    if (ch == JsonConstants.CloseBrace)
                        EndObject();
                    else
                        step = ConsumePropertyNameUtf8(ref next, length);
                    break;

                case JsonTokenType.StartArray:
                    if (ch == JsonConstants.CloseBracket)
                        EndArray();
                    else
                        step = ConsumeValueUtf8(ch, step, ref next, length);
                    break;

                case JsonTokenType.PropertyName:
                    step = ConsumeValueUtf8(ch, step, ref next, length);
                    if (step == 0) return false;
                    break;

                case JsonTokenType.EndArray:
                case JsonTokenType.EndObject:
                case JsonTokenType.Value:
                    step = ConsumeNextUtf8(ch, step, ref next, length);
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
                JsonThrowHelper.ThrowJsonReaderException();

            _depth++;
            _containerMask = (_containerMask << 1) | 1;
            TokenType = JsonTokenType.StartObject;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EndObject()
        {
            if (!InObject || _depth <= 0)
                JsonThrowHelper.ThrowJsonReaderException();

            _depth--;
            _containerMask >>= 1;
            TokenType = JsonTokenType.EndObject;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void StartArray()
        {
            if (_depth > MaxDepth)
                JsonThrowHelper.ThrowJsonReaderException();

            _depth++;
            _containerMask = (_containerMask << 1);
            TokenType = JsonTokenType.StartArray;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EndArray()
        {
            if (!InArray || _depth <= 0)
                JsonThrowHelper.ThrowJsonReaderException();

            _depth--;
            _containerMask >>= 1;
            TokenType = JsonTokenType.EndArray;
        }

        private int ConsumeNextUtf8MultiSegment(char marker)
        {
            switch (marker)
            {
                case (char)JsonConstants.ListSeperator:
                    {
                        _reader.Advance(1);
                        SkipWhiteSpace();
                        if (InObject)
                        {
                            if (_reader.Read() != JsonConstants.Quote) JsonThrowHelper.ThrowJsonReaderException();
                            ConsumePropertyNameUtf8MultiSegment();
                            return 1;
                        }
                        else if (InArray)
                        {
                            if (_reader.End)
                            {
                                return 0;
                            }
                            return ConsumeValueUtf8MultiSegment((char)_reader.Peek());
                        }
                        else
                        {
                            JsonThrowHelper.ThrowJsonReaderException();
                            return default;
                        }
                    }

                case (char)JsonConstants.CloseBrace:
                    _reader.Advance(1);
                    EndObject();
                    return 1;

                case (char)JsonConstants.CloseBracket:
                    _reader.Advance(1);
                    EndArray();
                    return 1;

                default:
                    JsonThrowHelper.ThrowJsonReaderException();
                    return default;
            }
        }

        /// <summary>
        /// This method consumes the next token regardless of whether we are inside an object or an array.
        /// For an object, it reads the next property name token. For an array, it just reads the next value.
        /// </summary>
        private int ConsumeNextUtf8(char marker, int markerBytes, ref byte src, int length)
        {
            int skip = markerBytes;
            switch (marker)
            {
                case (char)JsonConstants.ListSeperator:
                    {
                        skip += SkipWhiteSpaceUtf8(ref Unsafe.Add(ref src, markerBytes), length - markerBytes);
                        length -= skip;
                        ref byte next = ref Unsafe.Add(ref src, skip);
                        if (InObject)
                            return skip + ConsumePropertyNameUtf8(ref next, length);
                        else if (InArray)
                        {
                            if (length < 1)
                            {
                                return 0;
                            }

                            char ch = (char)next;
                            return skip + ConsumeValueUtf8(ch, 1, ref next, length);
                        }
                        else
                        {
                            JsonThrowHelper.ThrowJsonReaderException();
                            return default;
                        }
                    }

                case (char)JsonConstants.CloseBrace:
                    EndObject();
                    return skip;

                case (char)JsonConstants.CloseBracket:
                    EndArray();
                    return skip;

                default:
                    JsonThrowHelper.ThrowJsonReaderException();
                    return default;
            }
        }

        private int ConsumeValueUtf8MultiSegment(char marker)
        {
            TokenType = JsonTokenType.Value;

            switch (marker)
            {
                case (char)JsonConstants.Quote:
                    _reader.Advance(1);
                    return ConsumeStringUtf8MultiSegment();

                case (char)JsonConstants.OpenBrace:
                    _reader.Advance(1);
                    StartObject();
                    ValueType = JsonValueType.Object;
                    return 1;

                case (char)JsonConstants.OpenBracket:
                    _reader.Advance(1);
                    StartArray();
                    ValueType = JsonValueType.Array;
                    return 1;

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
                    return ConsumeNumberUtf8MultiSegment();

                case '-':
                    if (_reader.End) JsonThrowHelper.ThrowJsonReaderException();
                    char ch = (char)_reader.Peek();
                    return (ch == 'I')
                        ? ConsumeInfinityUtf8MultiSegment(true)
                        : ConsumeNumberUtf8MultiSegment();

                case 'f':
                    return ConsumeFalseUtf8MultiSegment();

                case 't':
                    return ConsumeTrueUtf8MultiSegment();

                case 'n':
                    return ConsumeNullUtf8MultiSegment();

                case 'u':
                    return ConsumeUndefinedUtf8MultiSegment();

                case 'N':
                    return ConsumeNaNUtf8MultiSegment();

                case 'I':
                    return ConsumeInfinityUtf8MultiSegment(false);

                case '/':
                    // TODO: Comments?
                    JsonThrowHelper.ThrowNotImplementedException();
                    return default;
            }

            return 0;
        }

        /// <summary>
        /// This method contains the logic for processing the next value token and determining
        /// what type of data it is.
        /// </summary>
        private int ConsumeValueUtf8(char marker, int markerBytes, ref byte src, int length)
        {
            TokenType = JsonTokenType.Value;

            switch (marker)
            {
                case (char)JsonConstants.Quote:
                    return ConsumeStringUtf8(ref src, length);

                case (char)JsonConstants.OpenBrace:
                    StartObject();
                    ValueType = JsonValueType.Object;
                    return markerBytes;

                case (char)JsonConstants.OpenBracket:
                    StartArray();
                    ValueType = JsonValueType.Array;
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
                    return ConsumeNumberUtf8(ref src, length);

                case '-':
                    if (length < 1) JsonThrowHelper.ThrowJsonReaderException();
                    char ch = (char)src;
                    return (ch == 'I')
                        ? ConsumeInfinityUtf8(ref src, length, true)
                        : ConsumeNumberUtf8(ref src, length);

                case 'f':
                    return ConsumeFalseUtf8(ref src, length);

                case 't':
                    return ConsumeTrueUtf8(ref src, length);

                case 'n':
                    return ConsumeNullUtf8(ref src, length);

                case 'u':
                    return ConsumeUndefinedUtf8(ref src, length);

                case 'N':
                    return ConsumeNaNUtf8(ref src, length);

                case 'I':
                    return ConsumeInfinityUtf8(ref src, length, false);

                case '/':
                    // TODO: Comments?
                    JsonThrowHelper.ThrowNotImplementedException();
                    return default;
            }

            return 0;
        }

        private bool TryReadUntill(ref BufferReader reader, out ReadOnlySequence<byte> bytes)
        {
            BufferReader copy = reader;
            SequencePosition start = reader.Position;
            while (!reader.End)
            {
                SequencePosition end = reader.Position;
                int b = reader.Peek();
                if (b == JsonConstants.ListSeperator || b == JsonConstants.CloseBrace || b == JsonConstants.CloseBracket || b == JsonConstants.CarriageReturn
                    || b == JsonConstants.LineFeed || b == JsonConstants.Space)
                {
                    bytes = reader.Sequence.Slice(start, end);
                    return true;
                }
                reader.Advance(1);
            }
            reader = copy;
            bytes = default;
            return false;
        }

        private int ConsumeNumberUtf8MultiSegment()
        {
            if (!TryReadUntill(ref _reader, out ReadOnlySequence<byte> sequence))
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }

            if (sequence.IsSingleSegment)
            {
                Value = sequence.First.Span;
            }
            else
            {
                Value = sequence.ToSpan();
            }

            ValueType = JsonValueType.Number;
            return 1;
        }

        private int ConsumeNumberUtf8(ref byte src, int length)
        {
            int idx = 0;
            // Scan until we find a list separator, array end, or object end.
            while (idx < length)
            {
                ref byte b = ref Unsafe.Add(ref src, idx);
                // TODO: Fix terminating condition
                if (b == JsonConstants.ListSeperator || b == JsonConstants.CloseBrace || b == JsonConstants.CloseBracket || b == JsonConstants.CarriageReturn
                    || b == JsonConstants.LineFeed || b == JsonConstants.Space)
                    break;
                idx++;
            }

            // Calculate the real start of the number based on our current buffer location.
            int startIndex = (int)Unsafe.ByteOffset(ref MemoryMarshal.GetReference(_buffer), ref src);

            Value = _buffer.Slice(startIndex, idx);
            ValueType = JsonValueType.Number;
            return idx;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumeNaNUtf8MultiSegment()
        {
            Value = ReadOnlySpan<byte>.Empty;
            ValueType = JsonValueType.NaN;

            if (_reader.Read() != 'N' || _reader.Read() != 'a' || _reader.Read() != 'N')
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }

            return 3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumeNaNUtf8(ref byte src, int length)
        {
            Value = ReadOnlySpan<byte>.Empty;
            ValueType = JsonValueType.NaN;

            if (length < 3
                || Unsafe.Add(ref src, 0) != 'N'
                || Unsafe.Add(ref src, 1) != 'a'
                || Unsafe.Add(ref src, 2) != 'N')
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }

            return 3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumeNullUtf8MultiSegment()
        {
            Value = JsonConstants.NullValue;
            ValueType = JsonValueType.Null;

            if (_reader.Read() != 'n' || _reader.Read() != 'u' || _reader.Read() != 'l' || _reader.Read() != 'l')
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }

            return 4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumeNullUtf8(ref byte src, int length)
        {
            Value = JsonConstants.NullValue;
            ValueType = JsonValueType.Null;

            if (length < 4
                || Unsafe.Add(ref src, 0) != 'n'
                || Unsafe.Add(ref src, 1) != 'u'
                || Unsafe.Add(ref src, 2) != 'l'
                || Unsafe.Add(ref src, 3) != 'l')
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }

            return 4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumeInfinityUtf8MultiSegment(bool negative)
        {
            Value = ReadOnlySpan<byte>.Empty;

            int answer = 8;
            if (negative)
            {
                answer = 9;
                ValueType = JsonValueType.NegativeInfinity;
            }
            else
            {
                ValueType = JsonValueType.Infinity;
            }

            if (_reader.Read() != 'I'
                || _reader.Read() != 'n'
                || _reader.Read() != 'f'
                || _reader.Read() != 'i'
                || _reader.Read() != 'n'
                || _reader.Read() != 'i'
                || _reader.Read() != 't'
                || _reader.Read() != 'y')
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }
            return answer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumeInfinityUtf8(ref byte src, int length, bool negative)
        {
            Value = ReadOnlySpan<byte>.Empty;

            ref byte idx = ref src;

            int answer = 8;
            if (negative)
            {
                answer = 9;
                ValueType = JsonValueType.NegativeInfinity;
                idx = Unsafe.Add(ref idx, 1);
            }
            else
            {
                ValueType = JsonValueType.Infinity;
            }

            if (length < answer
                || Unsafe.Add(ref idx, 0) != 'I'
                || Unsafe.Add(ref idx, 1) != 'n'
                || Unsafe.Add(ref idx, 2) != 'f'
                || Unsafe.Add(ref idx, 3) != 'i'
                || Unsafe.Add(ref idx, 4) != 'n'
                || Unsafe.Add(ref idx, 5) != 'i'
                || Unsafe.Add(ref idx, 6) != 't'
                || Unsafe.Add(ref idx, 7) != 'y')
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }
            return answer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumeUndefinedUtf8MultiSegment()
        {
            Value = ReadOnlySpan<byte>.Empty;
            ValueType = JsonValueType.Undefined;

            if (_reader.Read() != 'u'
                || _reader.Read() != 'n'
                || _reader.Read() != 'd'
                || _reader.Read() != 'e'
                || _reader.Read() != 'f'
                || _reader.Read() != 'i'
                || _reader.Read() != 'n'
                || _reader.Read() != 'e'
                || _reader.Read() != 'd')
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }

            return 9;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumeUndefinedUtf8(ref byte src, int length)
        {
            Value = ReadOnlySpan<byte>.Empty;
            ValueType = JsonValueType.Undefined;

            if (length < 9
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
                JsonThrowHelper.ThrowJsonReaderException();
            }

            return 9;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumeFalseUtf8MultiSegment()
        {
            Value = JsonConstants.FalseValue;
            ValueType = JsonValueType.False;

            if (_reader.Read() != 'f'
                || _reader.Read() != 'a'
                || _reader.Read() != 'l'
                || _reader.Read() != 's'
                || _reader.Read() != 'e')
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }

            return 5;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumeFalseUtf8(ref byte src, int length)
        {
            Value = JsonConstants.FalseValue;
            ValueType = JsonValueType.False;

            if (length < 5
                || Unsafe.Add(ref src, 0) != 'f'
                || Unsafe.Add(ref src, 1) != 'a'
                || Unsafe.Add(ref src, 2) != 'l'
                || Unsafe.Add(ref src, 3) != 's'
                || Unsafe.Add(ref src, 4) != 'e')
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }

            return 5;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumeTrueUtf8MultiSegment()
        {
            Value = JsonConstants.TrueValue;
            ValueType = JsonValueType.True;

            if (_reader.Read() != 't'
                || _reader.Read() != 'r'
                || _reader.Read() != 'u'
                || _reader.Read() != 'e')
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }

            return 4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ConsumeTrueUtf8(ref byte src, int length)
        {
            Value = JsonConstants.TrueValue;
            ValueType = JsonValueType.True;

            if (length < 4
                || Unsafe.Add(ref src, 0) != 't'
                || Unsafe.Add(ref src, 1) != 'r'
                || Unsafe.Add(ref src, 2) != 'u'
                || Unsafe.Add(ref src, 3) != 'e')
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }

            return 4;
        }

        private void ConsumePropertyNameUtf8MultiSegment()
        {
            if (!BufferReaderExtensions.TryReadUntill(ref _reader, out ReadOnlySequence<byte> sequence, JsonConstants.Quote))
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }

            if (sequence.IsSingleSegment)
            {
                Value = sequence.First.Span;
            }
            else
            {
                Value = sequence.ToSpan();
            }

            ValueType = JsonValueType.String;

            SkipWhiteSpace();
            if (_reader.End) JsonThrowHelper.ThrowJsonReaderException();

            // The next character must be a key / value seperator. Validate and skip.
            if (_reader.Read() != JsonConstants.KeyValueSeperator)
                JsonThrowHelper.ThrowJsonReaderException();

            TokenType = JsonTokenType.PropertyName;
        }

        private int ConsumePropertyNameUtf8(ref byte src, int length)
        {
            if (src != JsonConstants.Quote) JsonThrowHelper.ThrowJsonReaderException();
            int consumed = 1;
            while (consumed < length && Unsafe.Add(ref src, consumed++) != JsonConstants.Quote) ;

            if (consumed == length && Unsafe.Add(ref src, consumed - 1) != JsonConstants.Quote)
                JsonThrowHelper.ThrowJsonReaderException();

            int startIndex = (int)Unsafe.ByteOffset(ref MemoryMarshal.GetReference(_buffer), ref src) + 1;

            Value = _buffer.Slice(startIndex, consumed - 2); // -2 to exclude the quote characters.
            ValueType = JsonValueType.String;

            // SkipWhiteSpaceUtf8
            while (consumed < length)
            {
                byte val = Unsafe.Add(ref src, consumed);
                if (val == JsonConstants.Space || val == JsonConstants.CarriageReturn || val == JsonConstants.LineFeed || val == '\t')
                    consumed++;
                else
                    break;
            }
            if (consumed == length) JsonThrowHelper.ThrowJsonReaderException();

            // The next character must be a key / value seperator. Validate and skip.
            if (Unsafe.Add(ref src, consumed++) != JsonConstants.KeyValueSeperator)
                JsonThrowHelper.ThrowJsonReaderException();

            TokenType = JsonTokenType.PropertyName;
            return consumed;
        }

        private int ConsumeStringUtf8MultiSegment()
        {
            if (!BufferReaderExtensions.TryReadUntill(ref _reader, out ReadOnlySequence<byte> sequence, JsonConstants.Quote))
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }

            if (sequence.IsSingleSegment)
            {
                Value = sequence.First.Span;
            }
            else
            {
                Value = sequence.ToSpan();
            }

            ValueType = JsonValueType.String;
            return 1;
        }

        private int ConsumeStringUtf8(ref byte src, int length)
        {
            // The first character MUST be a JSON string quote
            if (src != JsonConstants.Quote) JsonThrowHelper.ThrowJsonReaderException();

            // If we are in this method, the first char is already known to be a JSON quote character.
            // Skip through the bytes until we find the closing JSON quote character.
            int idx = 1;
            while (idx < length && Unsafe.Add(ref src, idx++) != JsonConstants.Quote) ;

            // If we hit the end of the source and never saw an ending quote, then fail.
            if (idx == length && Unsafe.Add(ref src, idx - 1) != JsonConstants.Quote)
                JsonThrowHelper.ThrowJsonReaderException();

            // Calculate the real start of the property name based on our current buffer location.
            // Also, skip the opening JSON quote character.
            int startIndex = (int)Unsafe.ByteOffset(ref MemoryMarshal.GetReference(_buffer), ref src) + 1;

            Value = _buffer.Slice(startIndex, idx - 2); // -2 to exclude the quote characters.
            ValueType = JsonValueType.String;
            return idx;
        }

        private int SkipWhiteSpaceUtf8(ref byte src, int length)
        {
            int idx = 0;
            while (idx < length)
            {
                byte val = Unsafe.Add(ref src, idx);
                if (val == JsonConstants.Space || val == JsonConstants.CarriageReturn || val == JsonConstants.LineFeed || val == '\t')
                    idx++;
                else
                    break;
            }

            return idx;
        }
    }
}
