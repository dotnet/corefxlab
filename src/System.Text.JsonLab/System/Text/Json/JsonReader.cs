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

        private ReadOnlySequence<byte> _sequence;
        private SequencePosition _nextPosition;

        // Depth tracks the recursive depth of the nested objects / arrays within the JSON data.
        public int Depth { get; private set; }

        // This mask represents a tiny stack to track the state during nested transitions.
        // The first bit represents the state of the current level (1 == object, 0 == array).
        // Each subsequent bit is the parent / containing type (object or array). Since this
        // reader does a linear scan, we only need to keep a single path as we go through the data.
        private ulong _containerMask;

        // These properties are helpers for determining the current state of the reader
        private bool IsRoot => Depth == 1;
        private bool InArray => (_containerMask & 1) == 0 && (Depth > 0);
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
            _nextPosition = default;
            _sequence = default;
            _isSingleSegment = true;
            _buffer = data;
            Depth = 1;
            _containerMask = 0;

            TokenType = JsonTokenType.None;
            Value = ReadOnlySpan<byte>.Empty;
            ValueType = JsonValueType.Unknown;
        }

        public Utf8JsonReader(in ReadOnlySequence<byte> data)
        {
            _sequence = data;
            _isSingleSegment = data.IsSingleSegment; //true;
            _buffer = _sequence.First.Span;  //data.ToArray();
            Depth = 1;
            _containerMask = 0;

            TokenType = JsonTokenType.None;
            Value = ReadOnlySpan<byte>.Empty;
            ValueType = JsonValueType.Unknown;

            _nextPosition = _sequence.Start;
            if (!_isSingleSegment)
            {
                _sequence.TryGet(ref _nextPosition, out ReadOnlyMemory<byte> memory, true);
            }
        }

        /// <summary>
        /// Read the next token from the data buffer.
        /// </summary>
        /// <returns>True if the token was read successfully, else false.</returns>
        public bool Read()
        {
            return _isSingleSegment ? ReadSingleSegment(ref _buffer) : ReadMultiSegment();
        }

        private void SkipWhiteSpaceMultiSegment()
        {
            while (true)
            {
                byte val = _buffer[0];
                if (val != JsonConstants.Space && val != JsonConstants.CarriageReturn && val != JsonConstants.LineFeed && val != JsonConstants.Tab)
                {
                    break;
                }
                Advance(1);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ReadVal()
        {
            int val = -1;
            if (_buffer.Length == 0)
            {
                if (!TryGetNextSegment())
                    return val;
            }

            val = _buffer[0];
            Advance(1);
            return val;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Advance(int byteCount)
        {
            if (byteCount < _buffer.Length)
            {
                _buffer = _buffer.Slice(byteCount);
            }
            else
            {
                AdvanceNext(byteCount);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void AdvanceNext(int byteCount)
        {
            do
            {
                byteCount -= _buffer.Length;
                if (!TryGetNextSegment())
                {
                    _buffer = default;
                    break;
                }
            } while (byteCount >= _buffer.Length);

            _buffer = _buffer.Slice(byteCount);
        }

        private bool TryGetNextSegment()
        {
            SequencePosition position = _nextPosition;
            while (_sequence.TryGet(ref _nextPosition, out ReadOnlyMemory<byte> memory, advance: true))
            {
                if (memory.Length > 0)
                {
                    _sequence = _sequence.Slice(position);
                    _buffer = memory.Span;
                    return true;
                }
            }
            return false;
        }

        private bool ReadMultiSegment()
        {
            if (_buffer.Length < 1)
            {
                return false;
            }
            if (TokenType == JsonTokenType.None)
            {
                int val = _buffer[0];
                if (val == JsonConstants.OpenBrace)
                {
                    _containerMask = 1;
                    TokenType = JsonTokenType.StartObject;
                    Advance(1);
                }
                else if (val == JsonConstants.OpenBracket)
                {
                    TokenType = JsonTokenType.StartArray;
                    Advance(1);
                }
                else
                {
                    ConsumeSingleValue((byte)val);
                }
                return true;
            }

            SkipWhiteSpaceMultiSegment();

            if (_buffer.Length < 1)
            {
                return false;
            }
            int ch = _buffer[0];

            if (TokenType == JsonTokenType.StartObject)
            {
                Advance(1);
                if (ch == JsonConstants.CloseBrace)
                    EndObject();
                else
                {
                    if (ch != JsonConstants.Quote) JsonThrowHelper.ThrowJsonReaderException();
                    ConsumePropertyNameUtf8MultiSegment();
                }
            }
            else if (TokenType == JsonTokenType.StartArray)
            {
                if (ch == JsonConstants.CloseBracket)
                {
                    Advance(1);
                    EndArray();
                }
                else
                    ConsumeValueUtf8MultiSegment((byte)ch);
            }
            else if (TokenType == JsonTokenType.PropertyName)
            {
                ConsumeValueUtf8MultiSegment((byte)ch);
            }
            else
            {
                return ConsumeNextUtf8MultiSegment((byte)ch);
            }
            return true;
        }

        private bool ReadSingleSegment(ref ReadOnlySpan<byte> buffer)
        {
            if (TokenType == JsonTokenType.None)
            {
                if (buffer.Length < 1)
                {
                    return false;
                }
                if (buffer[0] == JsonConstants.OpenBrace)
                {
                    _containerMask = 1;
                    TokenType = JsonTokenType.StartObject;
                    buffer = buffer.Slice(1);
                }
                else if (buffer[0] == JsonConstants.OpenBracket)
                {
                    TokenType = JsonTokenType.StartArray;
                    buffer = buffer.Slice(1);
                }
                else
                {
                    ConsumeSingleValue(ref buffer, buffer[0]);
                }
                return true;
            }

            SkipWhiteSpaceUtf8(ref buffer);
            if (buffer.Length < 1)
            {
                return false;
            }

            byte ch = buffer[0];

            if (TokenType == JsonTokenType.StartObject)
            {
                buffer = buffer.Slice(1);
                if (ch == JsonConstants.CloseBrace)
                    EndObject();
                else
                {
                    if (ch != JsonConstants.Quote) JsonThrowHelper.ThrowJsonReaderException();
                    ConsumePropertyNameUtf8(ref buffer);
                }
            }
            else if (TokenType == JsonTokenType.StartArray)
            {
                if (ch == JsonConstants.CloseBracket)
                {
                    buffer = buffer.Slice(1);
                    EndArray();
                }
                else
                    ConsumeValueUtf8(ref buffer, ch);
            }
            else if (TokenType == JsonTokenType.PropertyName)
            {
                ConsumeValueUtf8(ref buffer, ch);
            }
            else
            {
                return ConsumeNextUtf8(ref buffer, ch);
            }
            return true;
        }

        public void Skip()
        {
            if (TokenType == JsonTokenType.PropertyName)
            {
                Read();
            }

            if (TokenType == JsonTokenType.StartObject || TokenType == JsonTokenType.StartArray)
            {
                int depth = Depth;
                while (Read() && depth < Depth)
                {
                }
            }
        }

        private void StartObject()
        {
            if (Depth > MaxDepth)
                JsonThrowHelper.ThrowJsonReaderException();

            Depth++;
            _containerMask = (_containerMask << 1) | 1;
            TokenType = JsonTokenType.StartObject;
        }

        private void EndObject()
        {
            if (!InObject || Depth <= 0)
                JsonThrowHelper.ThrowJsonReaderException();

            Depth--;
            _containerMask >>= 1;
            TokenType = JsonTokenType.EndObject;
        }

        private void StartArray()
        {
            if (Depth > MaxDepth)
                JsonThrowHelper.ThrowJsonReaderException();

            Depth++;
            _containerMask = (_containerMask << 1);
            TokenType = JsonTokenType.StartArray;
        }

        private void EndArray()
        {
            if (!InArray || Depth <= 0)
                JsonThrowHelper.ThrowJsonReaderException();

            Depth--;
            _containerMask >>= 1;
            TokenType = JsonTokenType.EndArray;
        }

        private bool ConsumeNextUtf8MultiSegment(byte marker)
        {
            Advance(1);
            switch (marker)
            {
                case JsonConstants.ListSeperator:
                    SkipWhiteSpaceMultiSegment();
                    if (InObject)
                    {
                        if (_buffer.Length < 1)
                        {
                            return false;
                        }
                        if (_buffer[0] != JsonConstants.Quote) JsonThrowHelper.ThrowJsonReaderException();
                        Advance(1);
                        ConsumePropertyNameUtf8MultiSegment();
                    }
                    else if (InArray)
                    {
                        if (_buffer.Length < 1)
                        {
                            return false;
                        }
                        ConsumeValueUtf8MultiSegment(_buffer[0]);
                    }
                    else
                    {
                        JsonThrowHelper.ThrowJsonReaderException();
                    }
                    break;

                case JsonConstants.CloseBrace:
                    EndObject();
                    break;

                case JsonConstants.CloseBracket:
                    EndArray();
                    break;

                default:
                    JsonThrowHelper.ThrowJsonReaderException();
                    break;
            }
            return true;
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

        private void ConsumeValueUtf8MultiSegment(byte marker)
        {
            TokenType = JsonTokenType.Value;

            if (marker == JsonConstants.Quote)
            {
                Advance(1);
                ConsumeStringUtf8MultiSegment();
            }
            else if (marker == JsonConstants.OpenBrace)
            {
                Advance(1);
                StartObject();
                ValueType = JsonValueType.Object;
            }
            else if (marker == JsonConstants.OpenBracket)
            {
                Advance(1);
                StartArray();
                ValueType = JsonValueType.Array;
            }
            else if (marker - '0' <= '9' - '0')
            {
                ConsumeNumberUtf8MultiSegment();
            }
            else if (marker == '-')
            {
                if (_buffer.Length < 1) JsonThrowHelper.ThrowJsonReaderException();
                ConsumeNumberUtf8MultiSegment();
            }
            else if (marker == 'f')
            {
                ConsumeFalseUtf8MultiSegment();
            }
            else if (marker == 't')
            {
                ConsumeTrueUtf8MultiSegment();
            }
            else if (marker == 'n')
            {
                ConsumeNullUtf8MultiSegment();
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
                int i = ConsumeStringUtf8(ref buffer);
                buffer = buffer.Slice(i);
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

        private void ConsumeSingleValue(byte marker)
        {
            TokenType = JsonTokenType.Value;

            if (marker == JsonConstants.Quote)
            {
                Advance(1);
                ConsumeStringUtf8MultiSegment();
            }
            else if (marker - '0' <= '9' - '0')
            {
                //TODO: Validate number
                Value = _sequence.ToArray();
                ValueType = JsonValueType.Number;
            }
            else if (marker == '-')
            {
                if (_buffer.Length < 1) JsonThrowHelper.ThrowJsonReaderException();
                Value = _sequence.ToArray();
                ValueType = JsonValueType.Number;
            }
            else if (marker == 'f')
            {
                ConsumeFalseUtf8MultiSegment();
            }
            else if (marker == 't')
            {
                ConsumeTrueUtf8MultiSegment();
            }
            else if (marker == 'n')
            {
                ConsumeNullUtf8MultiSegment();
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

        private void ConsumeSingleValue(ref ReadOnlySpan<byte> buffer, byte marker)
        {
            TokenType = JsonTokenType.Value;

            if (marker == JsonConstants.Quote)
            {
                buffer = buffer.Slice(1);
                int i = ConsumeStringUtf8(ref buffer);
                buffer = buffer.Slice(i);
            }
            else if (marker - '0' <= '9' - '0')
            {
                //TODO: Validate number
                Value = buffer;
                ValueType = JsonValueType.Number;
                buffer = buffer.Slice(buffer.Length);
            }
            else if (marker == '-')
            {
                if (buffer.Length < 2) JsonThrowHelper.ThrowJsonReaderException();
                Value = buffer;
                ValueType = JsonValueType.Number;
                buffer = buffer.Slice(buffer.Length);
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


        public bool TryReadUntilAny(out ReadOnlySpan<byte> span, ReadOnlySpan<byte> delimiters)
        {
            ReadOnlySpan<byte> remaining = _buffer;
            int index = remaining.IndexOfAny(delimiters);
            if (index != -1)
            {
                span = remaining.Slice(0, index);
                Advance(index);
                return true;
            }

            return TryReadUntilAnySlow(out span, delimiters, remaining.Length);
        }

        private bool TryReadUntilAnySlow(out ReadOnlySpan<byte> span, ReadOnlySpan<byte> delimiters, int skip)
        {
            ReadOnlySequence<byte> copy = _sequence;
            if (skip > 0)
                Advance(skip);
            
            while (_buffer.Length > 0)
            {
                ReadOnlySpan<byte> remaining = _buffer;
                int index = remaining.IndexOfAny(delimiters);
                if (index != -1)
                {
                    // Found one of the delimiters. Move to it, slice, then move past it.
                    if (index > 0)
                    {
                        Advance(index);
                    }

                    int sequenceLength = _sequence.First.Span.Length;


                    ReadOnlySequence<byte> sequence = copy.Slice(_sequence.Start, index + skip);
                    span = sequence.IsSingleSegment ? sequence.First.Span : sequence.ToArray();
                    return true;
                }

                Advance(remaining.Length);
            }

            span = default;
            return false;
        }

        private void ConsumeNumberUtf8MultiSegment()
        {
            if (!TryReadUntilAny(out ReadOnlySpan<byte> span, JsonConstants.Delimiters))
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }

            Value = span;

            ValueType = JsonValueType.Number;
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

        private void ConsumeNullUtf8MultiSegment()
        {
            Value = JsonConstants.NullValue;
            ValueType = JsonValueType.Null;

            if (ReadVal() != 'n' || ReadVal() != 'u' || ReadVal() != 'l' || ReadVal() != 'l')
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }
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

        private void ConsumeFalseUtf8MultiSegment()
        {
            Value = JsonConstants.FalseValue;
            ValueType = JsonValueType.False;

            if (ReadVal() != 'f'
                || ReadVal() != 'a'
                || ReadVal() != 'l'
                || ReadVal() != 's'
                || ReadVal() != 'e')
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }
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

        private void ConsumeTrueUtf8MultiSegment()
        {
            Value = JsonConstants.TrueValue;
            ValueType = JsonValueType.True;

            if (ReadVal() != 't'
                || ReadVal() != 'r'
                || ReadVal() != 'u'
                || ReadVal() != 'e')
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }
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
            ConsumeStringUtf8MultiSegment();

            SkipWhiteSpaceMultiSegment();

            // The next character must be a key / value seperator. Validate and skip.
            if (ReadVal() != JsonConstants.KeyValueSeperator)
                JsonThrowHelper.ThrowJsonReaderException();

            TokenType = JsonTokenType.PropertyName;
        }

        private void ConsumePropertyNameUtf8(ref ReadOnlySpan<byte> buffer)
        {
            int i = ConsumeStringUtf8(ref buffer);

            SkipWhiteSpaceUtf8(ref buffer, i);

            // The next character must be a key / value seperator. Validate and skip.
            if (buffer.Length < 1 || buffer[0] != JsonConstants.KeyValueSeperator)
                JsonThrowHelper.ThrowJsonReaderException();

            TokenType = JsonTokenType.PropertyName;
            buffer = buffer.Slice(1);
        }

        public bool TryReadUntil(out ReadOnlySpan<byte> span, byte delimiter)
        {
            ReadOnlySpan<byte> remaining = _buffer;

            //TODO: Optimize looking for nested quotes
            int i = 0;
            while (true)
            {
                int counter = 0;
                i += remaining.Slice(i).IndexOf(delimiter);
                if (i == -1)
                    break;
                if (i == 0)
                {
                    goto Done;
                }
                for (int j = i - 1; j >= 0; j--)
                {
                    if (remaining[j] != JsonConstants.ReverseSolidus)
                    {
                        if (counter % 2 == 0)
                            goto Done;
                        break;
                    }
                    else
                        counter++;
                }
                i++;
            }
            return TryReadUntilSlow(out span, delimiter, remaining.Length);
        Done:
            span = remaining.Slice(0, i);
            Advance(i + 1);
            return true;
        }

        private bool TryReadUntilSlow(out ReadOnlySpan<byte> span, byte delimiter, int skip)
        {
            ReadOnlySequence<byte> copy = _sequence;
            if (skip > 0)
                Advance(skip);
            
            while (_buffer.Length > 0)
            {
                ReadOnlySpan<byte> remaining = _buffer;
                int counter = 0;
                int index = remaining.IndexOf(delimiter);
                if (index != -1)
                {
                    // Found the delimiter. Move to it, slice, then move past it.
                    if (index > 0)
                    {
                        for (int j = index - 1; j >= 0; j--)
                        {
                            if (remaining[j] != JsonConstants.ReverseSolidus)
                            {
                                if (counter % 2 == 0)
                                {
                                    Advance(index);
                                    goto Done;
                                }
                                goto KeepLooking;
                            }
                            else
                                counter++;
                        }
                    }

                    Done:
                    ReadOnlySequence<byte> sequence = copy.Slice(copy.First.Span.Length - skip, index + skip);
                    span = sequence.IsSingleSegment ? sequence.First.Span : sequence.ToArray();
                    Advance(1);
                    return true;
                }
                KeepLooking:
                Advance(remaining.Length);
            }
            span = default;
            return false;
        }

        private void ConsumeStringUtf8MultiSegment()
        {
            if (!TryReadUntil(out ReadOnlySpan<byte> span, JsonConstants.Quote))
            {
                JsonThrowHelper.ThrowJsonReaderException();
            }

            Value = span;

            ValueType = JsonValueType.String;
        }

        private int ConsumeStringUtf8(ref ReadOnlySpan<byte> buffer)
        {
            //TODO: Optimize looking for nested quotes
            int i = 0;
            while (true)
            {
                int counter = 0;
                i += buffer.Slice(i).IndexOf(JsonConstants.Quote);
                if (i == -1)
                    JsonThrowHelper.ThrowJsonReaderException();
                if (i == 0)
                {
                    break;
                }
                for (int j = i - 1; j >= 0; j--)
                {
                    if (buffer[j] != JsonConstants.ReverseSolidus)
                    {
                        if (counter % 2 == 0)
                            goto Done;
                        break;
                    }
                    else
                        counter++;
                }
                i++;
            }

        Done:
            Value = buffer.Slice(0, i);
            ValueType = JsonValueType.String;
            return i + 1;
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
