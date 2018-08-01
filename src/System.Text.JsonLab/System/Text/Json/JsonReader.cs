// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Buffers.Reader;
using System.Runtime.CompilerServices;

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
            _isSingleSegment = data.IsSingleSegment; //true;
            _buffer = data.First.Span;  //data.ToArray();
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
            return _isSingleSegment ? ReadSingleSegment() : ReadMultiSegment();
        }

        private void SkipWhiteSpace()
        {
            while (true)
            {
                byte val = (byte)_reader.Peek();
                if (val != JsonConstants.Space && val != JsonConstants.CarriageReturn && val != JsonConstants.LineFeed && val != JsonConstants.Tab)
                {
                    break;
                }
                _reader.Advance(1);
            }
        }

        private bool ReadMultiSegment()
        {
            SkipWhiteSpace();
            if (_reader.End)
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
            SkipWhiteSpaceUtf8();

            if (_buffer.Length < 1)
            {
                return false;
            }

            byte ch = _buffer[0];

            switch (TokenType)
            {
                case JsonTokenType.None:
                    _buffer = _buffer.Slice(1);
                    if (ch == JsonConstants.OpenBrace)
                        StartObject();
                    else if (ch == JsonConstants.OpenBracket)
                        StartArray();
                    else
                        JsonThrowHelper.ThrowJsonReaderException();
                    break;

                case JsonTokenType.StartObject:
                    _buffer = _buffer.Slice(1);
                    if (ch == JsonConstants.CloseBrace)
                        EndObject();
                    else
                    {
                        if (ch != JsonConstants.Quote) JsonThrowHelper.ThrowJsonReaderException();
                        ConsumePropertyNameUtf8();
                    }
                    break;

                case JsonTokenType.StartArray:
                    if (ch == JsonConstants.CloseBracket)
                    {
                        _buffer = _buffer.Slice(1);
                        EndArray();
                    }
                    else
                        ConsumeValueUtf8(ch);
                    break;

                case JsonTokenType.PropertyName:
                    ConsumeValueUtf8(ch);
                    break;

                case JsonTokenType.EndArray:
                case JsonTokenType.EndObject:
                case JsonTokenType.Value:
                    return ConsumeNextUtf8(ch);
            }

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
            _reader.Advance(1);
            switch (marker)
            {
                case (char)JsonConstants.ListSeperator:
                    SkipWhiteSpace();
                    if (InObject)
                    {
                        if (_reader.Read() != JsonConstants.Quote) JsonThrowHelper.ThrowJsonReaderException();
                        ConsumePropertyNameUtf8MultiSegment();
                    }
                    else if (InArray)
                    {
                        return ConsumeValueUtf8MultiSegment((char)_reader.Peek());
                    }
                    else
                    {
                        JsonThrowHelper.ThrowJsonReaderException();
                        return default;
                    }
                    break;

                case (char)JsonConstants.CloseBrace:
                    EndObject();
                    break;

                case (char)JsonConstants.CloseBracket:
                    EndArray();
                    break;

                default:
                    JsonThrowHelper.ThrowJsonReaderException();
                    return default;
            }
            return 1;
        }

        /// <summary>
        /// This method consumes the next token regardless of whether we are inside an object or an array.
        /// For an object, it reads the next property name token. For an array, it just reads the next value.
        /// </summary>
        private bool ConsumeNextUtf8(byte marker)
        {
            switch (marker)
            {
                case JsonConstants.ListSeperator:
                    {
                        SkipWhiteSpaceUtf8(1);
                        if (InObject)
                        {
                            if (_buffer.Length < 1)
                            {
                                return false;
                            }
                            if (_buffer[0] != JsonConstants.Quote) JsonThrowHelper.ThrowJsonReaderException();
                            _buffer = _buffer.Slice(1);
                            ConsumePropertyNameUtf8();
                        }
                        else if (InArray)
                        {
                            if (_buffer.Length < 1)
                            {
                                return false;
                            }

                            ConsumeValueUtf8(_buffer[0]);
                        }
                        else
                        {
                            JsonThrowHelper.ThrowJsonReaderException();
                        }
                    }
                    break;

                case JsonConstants.CloseBrace:
                    _buffer = _buffer.Slice(1);
                    EndObject();
                    break;

                case JsonConstants.CloseBracket:
                    _buffer = _buffer.Slice(1);
                    EndArray();
                    break;

                default:
                    JsonThrowHelper.ThrowJsonReaderException();
                    break;
            }
            return true;
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
        private void ConsumeValueUtf8(byte marker)
        {
            TokenType = JsonTokenType.Value;

            if (marker == JsonConstants.Quote)
            {
                _buffer = _buffer.Slice(1);
                ConsumeStringUtf8();
            }
            else if (marker == JsonConstants.OpenBrace)
            {
                _buffer = _buffer.Slice(1);
                StartObject();
                ValueType = JsonValueType.Object;
            }
            else if (marker == JsonConstants.OpenBracket)
            {
                _buffer = _buffer.Slice(1);
                StartArray();
                ValueType = JsonValueType.Array;
            }
            else if (marker - '0' <= '9' - '0')
            {
                ConsumeNumberUtf8();
            }
            else if (marker == '-')
            {
                if (_buffer.Length < 2) JsonThrowHelper.ThrowJsonReaderException();
                byte ch = _buffer[1];
                if (ch == 'I')
                    ConsumeInfinityUtf8(true);
                else
                    ConsumeNumberUtf8();
            }
            else if (marker == 'f')
            {
                ConsumeFalseUtf8();
            }
            else if (marker == 't')
            {
                ConsumeTrueUtf8();
            }
            else if (marker == 'n')
            {
                ConsumeNullUtf8();
            }
            else if (marker == 'u')
            {
                ConsumeUndefinedUtf8();
            }
            else if (marker == 'N')
            {
                ConsumeNaNUtf8();
            }
            else if (marker == 'I')
            {
                ConsumeInfinityUtf8(false);
            }
            else if (marker == '/')
            {
                // TODO: Comments?
                JsonThrowHelper.ThrowNotImplementedException();
            }
            else
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }
        }

        private int ConsumeNumberUtf8MultiSegment()
        {
            if (!BufferReaderExtensions.TryReadUntilAny(ref _reader, out ReadOnlySpan<byte> span, JsonConstants.Delimiters))
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }

            Value = span;

            ValueType = JsonValueType.Number;
            return 1;
        }

        private void ConsumeNumberUtf8()
        {
            int idx = _buffer.IndexOfAny(JsonConstants.Delimiters);
            if (idx == -1)
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }

            Value = _buffer.Slice(0, idx);
            ValueType = JsonValueType.Number;
            _buffer = _buffer.Slice(idx);
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
        private void ConsumeNaNUtf8()
        {
            Value = ReadOnlySpan<byte>.Empty;
            ValueType = JsonValueType.NaN;

            if (_buffer.Length < 3
                || _buffer[0] != 'N'
                || _buffer[1] != 'a'
                || _buffer[2] != 'N')
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }
            _buffer = _buffer.Slice(3);
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
        private void ConsumeNullUtf8()
        {
            Value = JsonConstants.NullValue;
            ValueType = JsonValueType.Null;

            if (_buffer.Length < 4
                || _buffer[0] != 'n'
                || _buffer[1] != 'u'
                || _buffer[2] != 'l'
                || _buffer[3] != 'l')
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }
            _buffer = _buffer.Slice(4);
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
        private void ConsumeInfinityUtf8(bool negative)
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

            if (_buffer.Length < answer
                || _buffer[answer - 8] != 'I'
                || _buffer[answer - 7] != 'n'
                || _buffer[answer - 6] != 'f'
                || _buffer[answer - 5] != 'i'
                || _buffer[answer - 4] != 'n'
                || _buffer[answer - 3] != 'i'
                || _buffer[answer - 2] != 't'
                || _buffer[answer - 1] != 'y')
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }

            _buffer = _buffer.Slice(answer);
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
        private void ConsumeUndefinedUtf8()
        {
            Value = ReadOnlySpan<byte>.Empty;
            ValueType = JsonValueType.Undefined;

            if (_buffer.Length < 9
                || _buffer[0] != 'u'
                || _buffer[1] != 'n'
                || _buffer[2] != 'd'
                || _buffer[3] != 'e'
                || _buffer[4] != 'f'
                || _buffer[5] != 'i'
                || _buffer[6] != 'n'
                || _buffer[7] != 'e'
                || _buffer[8] != 'd')
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }
            _buffer = _buffer.Slice(9);
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
        private void ConsumeFalseUtf8()
        {
            Value = JsonConstants.FalseValue;
            ValueType = JsonValueType.False;

            if (_buffer.Length < 5
                || _buffer[0] != 'f'
                || _buffer[1] != 'a'
                || _buffer[2] != 'l'
                || _buffer[3] != 's'
                || _buffer[4] != 'e')
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }
            _buffer = _buffer.Slice(5);
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
        private void ConsumeTrueUtf8()
        {
            Value = JsonConstants.TrueValue;
            ValueType = JsonValueType.True;

            if (_buffer.Length < 4
                || _buffer[0] != 't'
                || _buffer[1] != 'r'
                || _buffer[2] != 'u'
                || _buffer[3] != 'e')
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }
            _buffer = _buffer.Slice(4);
        }

        private void ConsumePropertyNameUtf8MultiSegment()
        {
            if (!BufferReaderExtensions.TryReadUntil(ref _reader, out ReadOnlySpan<byte> span, JsonConstants.Quote))
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }

            Value = span;

            ValueType = JsonValueType.String;

            SkipWhiteSpace();
            if (_reader.End) JsonThrowHelper.ThrowJsonReaderException();

            // The next character must be a key / value seperator. Validate and skip.
            if (_reader.Read() != JsonConstants.KeyValueSeperator)
                JsonThrowHelper.ThrowJsonReaderException();

            TokenType = JsonTokenType.PropertyName;
        }

        private void ConsumePropertyNameUtf8()
        {
            int i = _buffer.IndexOf(JsonConstants.Quote);
            if (i == -1)
                JsonThrowHelper.ThrowJsonReaderException();

            Value = _buffer.Slice(0, i);
            ValueType = JsonValueType.String;

            i++;
            // SkipWhiteSpaceUtf8
            while (i < _buffer.Length)
            {
                byte val = _buffer[i];
                if (val == JsonConstants.Space || val == JsonConstants.CarriageReturn || val == JsonConstants.LineFeed || val == JsonConstants.Tab)
                    i++;
                else
                    break;
            }
            if (i == _buffer.Length)
                JsonThrowHelper.ThrowJsonReaderException();

            // The next character must be a key / value seperator. Validate and skip.
            if (_buffer[i++] != JsonConstants.KeyValueSeperator)
                JsonThrowHelper.ThrowJsonReaderException();

            TokenType = JsonTokenType.PropertyName;
            _buffer = _buffer.Slice(i);
        }

        private int ConsumeStringUtf8MultiSegment()
        {
            if (!BufferReaderExtensions.TryReadUntil(ref _reader, out ReadOnlySpan<byte> span, JsonConstants.Quote))
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }

            Value = span;

            ValueType = JsonValueType.String;
            return 1;
        }

        private void ConsumeStringUtf8()
        {
            int i = _buffer.IndexOf(JsonConstants.Quote);
            if (i == -1)
                JsonThrowHelper.ThrowJsonReaderException();

            Value = _buffer.Slice(0, i);
            ValueType = JsonValueType.String;
            _buffer = _buffer.Slice(i + 1);
        }

        private void SkipWhiteSpaceUtf8(int i = 0)
        {
            for (; i < _buffer.Length; i++)
            {
                byte val = _buffer[i];
                if (val != JsonConstants.Space && val != JsonConstants.CarriageReturn && val != JsonConstants.LineFeed && val != JsonConstants.Tab)
                {
                    break;
                }
            }
            _buffer = _buffer.Slice(i);
        }
    }
}
