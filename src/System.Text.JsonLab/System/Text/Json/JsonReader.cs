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
            return _isSingleSegment ? ReadSingleSegment(ref _buffer) : ReadMultiSegment();
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

        private bool ReadSingleSegment(ref ReadOnlySpan<byte> buffer)
        {
            if (TokenType != JsonTokenType.None)
                SkipWhiteSpaceUtf8(ref buffer);

            if (buffer.Length < 1)
            {
                return false;
            }

            byte ch = buffer[0];

            switch (TokenType)
            {
                case JsonTokenType.None:
                    buffer = buffer.Slice(1);
                    if (ch == JsonConstants.OpenBrace)
                        StartObject();
                    else if (ch == JsonConstants.OpenBracket)
                        StartArray();
                    else
                        JsonThrowHelper.ThrowJsonReaderException();
                    break;

                case JsonTokenType.StartObject:
                    buffer = buffer.Slice(1);
                    if (ch == JsonConstants.CloseBrace)
                        EndObject();
                    else
                    {
                        if (ch != JsonConstants.Quote) JsonThrowHelper.ThrowJsonReaderException();
                        ConsumePropertyNameUtf8(ref buffer);
                    }
                    break;

                case JsonTokenType.StartArray:
                    if (ch == JsonConstants.CloseBracket)
                    {
                        buffer = buffer.Slice(1);
                        EndArray();
                    }
                    else
                        ConsumeValueUtf8(ref buffer, ch);
                    break;

                case JsonTokenType.PropertyName:
                    ConsumeValueUtf8(ref buffer, ch);
                    break;

                case JsonTokenType.EndArray:
                case JsonTokenType.EndObject:
                case JsonTokenType.Value:
                    return ConsumeNextUtf8(ref buffer, ch);
            }
            return true;
        }

        private void StartObject()
        {
            if (_depth > MaxDepth)
                JsonThrowHelper.ThrowJsonReaderException();

            _depth++;
            _containerMask = (_containerMask << 1) | 1;
            TokenType = JsonTokenType.StartObject;
        }

        private void EndObject()
        {
            if (!InObject || _depth <= 0)
                JsonThrowHelper.ThrowJsonReaderException();

            _depth--;
            _containerMask >>= 1;
            TokenType = JsonTokenType.EndObject;
        }

        private void StartArray()
        {
            if (_depth > MaxDepth)
                JsonThrowHelper.ThrowJsonReaderException();

            _depth++;
            _containerMask = (_containerMask << 1);
            TokenType = JsonTokenType.StartArray;
        }

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
        private bool ConsumeNextUtf8(ref ReadOnlySpan<byte> buffer, byte marker)
        {
            switch (marker)
            {
                case JsonConstants.ListSeperator:
                    {
                        SkipWhiteSpaceUtf8(ref buffer, 1);
                        if (InObject)
                        {
                            if (buffer.Length < 1)
                            {
                                return false;
                            }
                            if (buffer[0] != JsonConstants.Quote) JsonThrowHelper.ThrowJsonReaderException();
                            buffer = buffer.Slice(1);
                            ConsumePropertyNameUtf8(ref buffer);
                        }
                        else if (InArray)
                        {
                            if (buffer.Length < 1)
                            {
                                return false;
                            }

                            ConsumeValueUtf8(ref buffer, buffer[0]);
                        }
                        else
                        {
                            JsonThrowHelper.ThrowJsonReaderException();
                        }
                    }
                    break;

                case JsonConstants.CloseBrace:
                    buffer = buffer.Slice(1);
                    EndObject();
                    break;

                case JsonConstants.CloseBracket:
                    buffer = buffer.Slice(1);
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
                    return ConsumeNumberUtf8MultiSegment();

                case 'f':
                    return ConsumeFalseUtf8MultiSegment();

                case 't':
                    return ConsumeTrueUtf8MultiSegment();

                case 'n':
                    return ConsumeNullUtf8MultiSegment();

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
        private void ConsumeValueUtf8(ref ReadOnlySpan<byte> buffer, byte marker)
        {
            TokenType = JsonTokenType.Value;

            if (marker == JsonConstants.Quote)
            {
                buffer = buffer.Slice(1);
                ConsumeStringUtf8(ref buffer);
            }
            else if (marker == JsonConstants.OpenBrace)
            {
                buffer = buffer.Slice(1);
                StartObject();
                ValueType = JsonValueType.Object;
            }
            else if (marker == JsonConstants.OpenBracket)
            {
                buffer = buffer.Slice(1);
                StartArray();
                ValueType = JsonValueType.Array;
            }
            else if (marker - '0' <= '9' - '0')
            {
                ConsumeNumberUtf8(ref buffer);
            }
            else if (marker == '-')
            {
                if (buffer.Length < 2) JsonThrowHelper.ThrowJsonReaderException();
                ConsumeNumberUtf8(ref buffer);
            }
            else if (marker == 'f')
            {
                ConsumeFalseUtf8(ref buffer);
            }
            else if (marker == 't')
            {
                ConsumeTrueUtf8(ref buffer);
            }
            else if (marker == 'n')
            {
                ConsumeNullUtf8(ref buffer);
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

        private void ConsumeNumberUtf8(ref ReadOnlySpan<byte> buffer)
        {
            int idx = buffer.IndexOfAny(JsonConstants.Delimiters);
            if (idx == -1)
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }

            Value = buffer.Slice(0, idx);
            ValueType = JsonValueType.Number;
            buffer = buffer.Slice(idx);
        }

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

        private void ConsumeNullUtf8(ref ReadOnlySpan<byte> buffer)
        {
            Value = JsonConstants.NullValue;
            ValueType = JsonValueType.Null;

            if (buffer.Length < 4
                || buffer[0] != 'n'
                || buffer[1] != 'u'
                || buffer[2] != 'l'
                || buffer[3] != 'l')
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }
            buffer = buffer.Slice(4);
        }

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

        private void ConsumeFalseUtf8(ref ReadOnlySpan<byte> buffer)
        {
            Value = JsonConstants.FalseValue;
            ValueType = JsonValueType.False;

            if (buffer.Length < 5
                || buffer[0] != 'f'
                || buffer[1] != 'a'
                || buffer[2] != 'l'
                || buffer[3] != 's'
                || buffer[4] != 'e')
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }
            buffer = buffer.Slice(5);
        }

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

        private void ConsumeTrueUtf8(ref ReadOnlySpan<byte> buffer)
        {
            Value = JsonConstants.TrueValue;
            ValueType = JsonValueType.True;

            if (buffer.Length < 4
                || buffer[0] != 't'
                || buffer[1] != 'r'
                || buffer[2] != 'u'
                || buffer[3] != 'e')
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }
            buffer = buffer.Slice(4);
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

        private void ConsumePropertyNameUtf8(ref ReadOnlySpan<byte> buffer)
        {
            int i = buffer.IndexOf(JsonConstants.Quote);
            if (i == -1)
                JsonThrowHelper.ThrowJsonReaderException();

            Value = buffer.Slice(0, i);
            ValueType = JsonValueType.String;

            SkipWhiteSpaceUtf8(ref buffer, i + 1);
            if (buffer.Length < 1)
                JsonThrowHelper.ThrowJsonReaderException();

            // The next character must be a key / value seperator. Validate and skip.
            if (buffer[0] != JsonConstants.KeyValueSeperator)
                JsonThrowHelper.ThrowJsonReaderException();

            TokenType = JsonTokenType.PropertyName;
            buffer = buffer.Slice(1);
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

        private void ConsumeStringUtf8(ref ReadOnlySpan<byte> buffer)
        {
            int i = buffer.IndexOf(JsonConstants.Quote);
            if (i == -1)
                JsonThrowHelper.ThrowJsonReaderException();

            Value = buffer.Slice(0, i);
            ValueType = JsonValueType.String;
            buffer = buffer.Slice(i + 1);
        }

        private void SkipWhiteSpaceUtf8(ref ReadOnlySpan<byte> buffer, int i = 0)
        {
            for (; i < buffer.Length; i++)
            {
                byte val = buffer[i];
                if (val != JsonConstants.Space && val != JsonConstants.CarriageReturn && val != JsonConstants.LineFeed && val != JsonConstants.Tab)
                {
                    break;
                }
            }
            buffer = buffer.Slice(i);
        }
    }
}
