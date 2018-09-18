// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Reader;
using System.Collections.Generic;

namespace System.Text.JsonLab
{
    public ref struct Utf8JsonReader
    {
        // We are using a ulong to represent our nested state, so we can only go 64 levels deep.
        internal const int StackFreeMaxDepth = sizeof(ulong) * 8;

        private ReadOnlySpan<byte> _buffer;

        public int Index { get; private set; }

        public int StartLocation { get; private set; }

        public int MaxDepth
        {
            get
            {
                return _maxDepth;
            }
            set
            {
                if (value <= 0)
                    JsonThrowHelper.ThrowArgumentException(ref this, "Max depth must be positive.");
                _maxDepth = value;
                if (_maxDepth > StackFreeMaxDepth)
                    _stack = new Stack<int>();
            }
        }

        private int _maxDepth;

        internal int _lineNumber;
        internal int _position;

        private BufferReader<byte> _reader;

        private Stack<int> _stack;

        internal Stack<string> _path;

        // Depth tracks the recursive depth of the nested objects / arrays within the JSON data.
        public int Depth { get; private set; }

        // This mask represents a tiny stack to track the state during nested transitions.
        // The first bit represents the state of the current level (1 == object, 0 == array).
        // Each subsequent bit is the parent / containing type (object or array). Since this
        // reader does a linear scan, we only need to keep a single path as we go through the data.
        private ulong _containerMask;

        // These properties are helpers for determining the current state of the reader
        internal bool InArray => !InObject;
        internal bool InObject => Depth <= StackFreeMaxDepth ? (_containerMask & 1) != 0 : _stack.Peek() != 0;

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

        private readonly bool _isSingleSegment;

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
            Depth = 0;
            _containerMask = 0;
            Index = 0;
            StartLocation = Index;
            _stack = null;
            _maxDepth = StackFreeMaxDepth;

            TokenType = JsonTokenType.None;
            Value = ReadOnlySpan<byte>.Empty;
            ValueType = JsonValueType.Unknown;
            _lineNumber = 1;
            _position = 0;

            _path = new Stack<string>();
        }

        public Utf8JsonReader(in ReadOnlySequence<byte> data)
        {
            _reader = new BufferReader<byte>(data);
            _isSingleSegment = data.IsSingleSegment; //true;
            _buffer = _reader.CurrentSpan;  //data.ToArray();
            Depth = 0;
            _containerMask = 0;
            Index = 0;
            StartLocation = Index;
            _stack = null;
            _maxDepth = StackFreeMaxDepth;

            TokenType = JsonTokenType.None;
            Value = ReadOnlySpan<byte>.Empty;
            ValueType = JsonValueType.Unknown;
            _lineNumber = 1;
            _position = 0;

            _path = new Stack<string>();
        }

        /// <summary>
        /// Read the next token from the data buffer.
        /// </summary>
        /// <returns>True if the token was read successfully, else false.</returns>
        public bool Read()
        {
            return _isSingleSegment ? ReadSingleSegment(ref _buffer) : ReadMultiSegment(ref _reader);
        }

        private void SkipWhiteSpace(ref BufferReader<byte> reader)
        {
            Index += (int)reader.SkipPastAny(JsonConstants.Space, JsonConstants.CarriageReturn, JsonConstants.LineFeed, JsonConstants.Tab);
        }

        private bool ReadFirstToken(ref BufferReader<byte> reader, byte first)
        {
            if (first == JsonConstants.OpenBrace)
            {
                Depth++;
                _containerMask = 1;
                TokenType = JsonTokenType.StartObject;
                reader.Advance(1);
                Index++;
            }
            else if (first == JsonConstants.OpenBracket)
            {
                Depth++;
                TokenType = JsonTokenType.StartArray;
                reader.Advance(1);
                Index++;
            }
            else
            {
                ConsumeSingleValue(ref reader, first);
            }
            return true;
        }

        private bool ReadMultiSegment(ref BufferReader<byte> reader)
        {
            bool retVal = false;

            if (!reader.TryPeek(out byte first))
                goto Done;

            if (first <= JsonConstants.Space)
            {
                SkipWhiteSpace(ref reader);
                if (!reader.TryPeek(out first))
                    goto Done;
            }

            StartLocation = Index;

            if (TokenType == JsonTokenType.None)
                goto ReadFirstToken;

            if (TokenType == JsonTokenType.StartObject)
            {
                reader.Advance(1);
                Index++;
                if (first == JsonConstants.CloseBrace)
                    EndObject();
                else
                {
                    if (first != JsonConstants.Quote) JsonThrowHelper.ThrowJsonReaderException(ref this);
                    StartLocation++;
                    ConsumePropertyNameUtf8MultiSegment(ref reader);
                }
            }
            else if (TokenType == JsonTokenType.StartArray)
            {
                if (first == JsonConstants.CloseBracket)
                {
                    reader.Advance(1);
                    Index++;
                    EndArray();
                }
                else
                    ConsumeValueUtf8MultiSegment(ref reader, first);
            }
            else if (TokenType == JsonTokenType.PropertyName)
            {
                ConsumeValueUtf8MultiSegment(ref reader, first);
            }
            else
            {
                ConsumeNextUtf8MultiSegment(ref reader, first);
            }

            retVal = true;

        Done:
            return retVal;

        ReadFirstToken:
            retVal = ReadFirstToken(ref reader, first);
            goto Done;
        }

        private bool ReadFirstToken(ref ReadOnlySpan<byte> buffer, byte first)
        {
            if (first == JsonConstants.OpenBrace)
            {
                Depth++;
                _containerMask = 1;
                TokenType = JsonTokenType.StartObject;
                buffer = buffer.Slice(1);
                Index++;
                _position++;
            }
            else if (first == JsonConstants.OpenBracket)
            {
                Depth++;
                TokenType = JsonTokenType.StartArray;
                buffer = buffer.Slice(1);
                Index++;
                _position++;
            }
            else
            {
                ConsumeSingleValue(ref buffer, first);
            }
            return true;
        }

        internal bool NoMoreData => (0 >= (uint)_buffer.Length);

        private bool ReadSingleSegment(ref ReadOnlySpan<byte> buffer)
        {
            bool retVal = false;

            if (0 >= (uint)buffer.Length)
                goto Done;

            byte first = buffer[0];

            if (first <= JsonConstants.Space)
            {
                SkipWhiteSpaceUtf8(ref buffer);
                if (0 >= (uint)buffer.Length)
                    goto Done;
                first = buffer[0];
            }

            StartLocation = Index;

            if (TokenType == JsonTokenType.None)
            {
                goto ReadFirstToken;
            }

            if (TokenType == JsonTokenType.StartObject)
            {
                buffer = buffer.Slice(1);
                Index++;
                _position++;
                if (first == JsonConstants.CloseBrace)
                    EndObject();
                else
                {
                    if (first != JsonConstants.Quote) JsonThrowHelper.ThrowJsonReaderException(ref this);
                    StartLocation++;
                    ConsumePropertyNameUtf8(ref buffer);
                }
            }
            else if (TokenType == JsonTokenType.StartArray)
            {
                if (first == JsonConstants.CloseBracket)
                {
                    buffer = buffer.Slice(1);
                    Index++;
                    _position++;
                    EndArray();
                }
                else
                {
                    var str = "[Z]";
                    _path.Push(str);
                    ConsumeValueUtf8(ref buffer, first);
                }
            }
            else if (TokenType == JsonTokenType.PropertyName)
            {
                ConsumeValueUtf8(ref buffer, first);
            }
            else
            {
                ConsumeNextUtf8(ref buffer, first);
            }

            retVal = true;

        Done:
            return retVal;

        ReadFirstToken:
            retVal = ReadFirstToken(ref buffer, first);
            goto Done;
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
            //var str = Encoding.UTF8.GetString(Value.ToArray());
            //_path.Push(str);

            if (Depth >= MaxDepth)
                JsonThrowHelper.ThrowJsonReaderException(ref this);

            Depth++;
            if (Depth <= StackFreeMaxDepth)
            {
                _containerMask = (_containerMask << 1) | 1;
            }
            else
            {
                _stack.Push(1);
            }
            TokenType = JsonTokenType.StartObject;
        }

        private void EndObject()
        {
            if (_path.Count > 0)
                _path.Pop();
            if (Depth <= StackFreeMaxDepth)
            {
                if ((_containerMask & 1) == 0 || Depth <= 0)
                    JsonThrowHelper.ThrowJsonReaderException(ref this);
                _containerMask >>= 1;
            }
            else
            {
                if (_stack.Peek() == 0 || Depth <= 0)
                    JsonThrowHelper.ThrowJsonReaderException(ref this);
                _stack.Pop();
            }

            Depth--;
            TokenType = JsonTokenType.EndObject;
        }

        private void StartArray()
        {
            //var str = Encoding.UTF8.GetString(Value.ToArray());
            //_path.Push(str);

            if (Depth >= MaxDepth)
                JsonThrowHelper.ThrowJsonReaderException(ref this);

            Depth++;
            if (Depth <= StackFreeMaxDepth)
            {
                _containerMask = (_containerMask << 1);
            }
            else
            {
                _stack.Push(0);
            }
            TokenType = JsonTokenType.StartArray;
        }

        private void EndArray()
        {
            if (_path.Count > 0)
                _path.Pop();
            if (Depth <= StackFreeMaxDepth)
            {
                if ((_containerMask & 1) != 0 || Depth <= 0)
                    JsonThrowHelper.ThrowJsonReaderException(ref this);
                _containerMask >>= 1;
            }
            else
            {
                if (_stack.Peek() != 0 || Depth <= 0)
                    JsonThrowHelper.ThrowJsonReaderException(ref this);
                _stack.Pop();
            }

            Depth--;
            TokenType = JsonTokenType.EndArray;
        }

        private void ConsumeNextUtf8MultiSegment(ref BufferReader<byte> reader, byte marker)
        {
            reader.Advance(1);
            Index++;
            if (marker == JsonConstants.ListSeperator)
            {
                if (!reader.TryPeek(out byte first))
                    JsonThrowHelper.ThrowJsonReaderException(ref this);

                if (first <= JsonConstants.Space)
                {
                    SkipWhiteSpace(ref reader);
                    // The next character must be a start of a property name or value.
                    if (!reader.TryPeek(out first))
                        JsonThrowHelper.ThrowJsonReaderException(ref this);
                }

                StartLocation = Index;
                if (InObject)
                {
                    if (first != JsonConstants.Quote)
                        JsonThrowHelper.ThrowJsonReaderException(ref this);

                    reader.Advance(1);
                    Index++;
                    StartLocation++;
                    ConsumePropertyNameUtf8MultiSegment(ref reader);
                }
                else
                {
                    ConsumeValueUtf8MultiSegment(ref reader, first);
                }
            }
            else if (marker == JsonConstants.CloseBrace)
            {
                EndObject();
            }
            else if (marker == JsonConstants.CloseBracket)
            {
                EndArray();
            }
            else
            {
                JsonThrowHelper.ThrowJsonReaderException(ref this);
            }
        }

        /// <summary>
        /// This method consumes the next token regardless of whether we are inside an object or an array.
        /// For an object, it reads the next property name token. For an array, it just reads the next value.
        /// </summary>
        private void ConsumeNextUtf8(ref ReadOnlySpan<byte> buffer, byte marker)
        {
            buffer = buffer.Slice(1);
            Index++;
            _position++;
            if (marker == JsonConstants.ListSeperator)
            {
                if (0 >= (uint)buffer.Length)
                    JsonThrowHelper.ThrowJsonReaderException(ref this);

                byte first = buffer[0];

                if (first <= JsonConstants.Space)
                {
                    SkipWhiteSpaceUtf8(ref buffer);
                    // The next character must be a start of a property name or value.
                    if (0 >= (uint)buffer.Length)
                        JsonThrowHelper.ThrowJsonReaderException(ref this);
                    first = buffer[0];
                }

                StartLocation = Index;
                if (InObject)
                {
                    if (first != JsonConstants.Quote)
                        JsonThrowHelper.ThrowJsonReaderException(ref this);

                    buffer = buffer.Slice(1);
                    Index++;
                    _position++;
                    StartLocation++;
                    ConsumePropertyNameUtf8(ref buffer);
                }
                else
                {
                    var str = "[Z]";
                    _path.Push(str);
                    ConsumeValueUtf8(ref buffer, first);
                }
            }
            else if (marker == JsonConstants.CloseBrace)
            {
                EndObject();
            }
            else if (marker == JsonConstants.CloseBracket)
            {
                EndArray();
            }
            else
            {
                JsonThrowHelper.ThrowJsonReaderException(ref this);
            }
        }

        private void ConsumeValueUtf8MultiSegment(ref BufferReader<byte> reader, byte marker)
        {
            TokenType = JsonTokenType.Value;

            if (marker == JsonConstants.Quote)
            {
                reader.Advance(1);
                Index++;
                StartLocation++;
                ConsumeStringUtf8MultiSegment(ref reader);
            }
            else if (marker == JsonConstants.OpenBrace)
            {
                reader.Advance(1);
                Index++;
                StartObject();
                ValueType = JsonValueType.Object;
            }
            else if (marker == JsonConstants.OpenBracket)
            {
                reader.Advance(1);
                Index++;
                StartArray();
                ValueType = JsonValueType.Array;
            }
            else if (marker - '0' <= '9' - '0')
            {
                ConsumeNumberUtf8MultiSegment(ref reader);
            }
            else if (marker == '-')
            {
                //TODO: Is this a valid check or do we need to do an Advance to be sure?
                if (reader.End) JsonThrowHelper.ThrowJsonReaderException(ref this);
                ConsumeNumberUtf8MultiSegment(ref reader);
            }
            else if (marker == 'f')
            {
                ConsumeFalseUtf8MultiSegment(ref reader);
            }
            else if (marker == 't')
            {
                ConsumeTrueUtf8MultiSegment(ref reader);
            }
            else if (marker == 'n')
            {
                ConsumeNullUtf8MultiSegment(ref reader);
            }
            else if (marker == '/')
            {
                // TODO: Comments?
                JsonThrowHelper.ThrowNotImplementedException();
            }
            else
            {
                JsonThrowHelper.ThrowJsonReaderException(ref this);
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
                Index++;
                _position++;
                StartLocation++;
                ConsumeStringUtf8(ref buffer);
            }
            else if (marker == JsonConstants.OpenBrace)
            {
                buffer = buffer.Slice(1);
                Index++;
                _position++;
                StartObject();
                ValueType = JsonValueType.Object;
            }
            else if (marker == JsonConstants.OpenBracket)
            {
                buffer = buffer.Slice(1);
                Index++;
                _position++;
                StartArray();
                ValueType = JsonValueType.Array;
            }
            else if (marker - '0' <= '9' - '0')
            {
                ConsumeNumberUtf8(ref buffer);
            }
            else if (marker == '-')
            {
                if (buffer.Length < 2) JsonThrowHelper.ThrowJsonReaderException(ref this);
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
                JsonThrowHelper.ThrowJsonReaderException(ref this);
            }
        }

        private void ConsumeSingleValue(ref BufferReader<byte> reader, byte marker)
        {
            TokenType = JsonTokenType.Value;

            if (marker == JsonConstants.Quote)
            {
                reader.Advance(1);
                Index++;
                _position++;
                StartLocation++;
                ConsumeStringUtf8MultiSegment(ref reader);
            }
            else if (marker - '0' <= '9' - '0')
            {
                //TODO: Validate number
                ReadOnlySequence<byte> sequence = reader.Sequence.Slice(reader.Position);
                Value = sequence.IsSingleSegment ? sequence.First.Span : sequence.ToArray();
                ValueType = JsonValueType.Number;
                int length = reader.UnreadSpan.Length;
                reader.Advance(length);
                Index += length;
                _position += length;
            }
            else if (marker == '-')
            {
                //TODO: Is this a valid check?
                if (reader.End) JsonThrowHelper.ThrowJsonReaderException(ref this);
                ReadOnlySequence<byte> sequence = reader.Sequence.Slice(reader.Position);
                Value = sequence.IsSingleSegment ? sequence.First.Span : sequence.ToArray();
                ValueType = JsonValueType.Number;
                int length = reader.UnreadSpan.Length;
                reader.Advance(length);
                Index += length;
                _position += length;
            }
            else if (marker == 'f')
            {
                ConsumeFalseUtf8MultiSegment(ref reader);
            }
            else if (marker == 't')
            {
                ConsumeTrueUtf8MultiSegment(ref reader);
            }
            else if (marker == 'n')
            {
                ConsumeNullUtf8MultiSegment(ref reader);
            }
            else if (marker == '/')
            {
                // TODO: Comments?
                JsonThrowHelper.ThrowNotImplementedException();
            }
            else
            {
                JsonThrowHelper.ThrowJsonReaderException(ref this);
            }
        }

        private void ConsumeSingleValue(ref ReadOnlySpan<byte> buffer, byte marker)
        {
            TokenType = JsonTokenType.Value;

            if (marker == JsonConstants.Quote)
            {
                buffer = buffer.Slice(1);
                Index++;
                _position++;
                StartLocation++;
                ConsumeStringUtf8(ref buffer);
            }
            else if (marker - '0' <= '9' - '0')
            {
                //TODO: Validate number
                Value = buffer;
                ValueType = JsonValueType.Number;
                Index += buffer.Length;
                _position += buffer.Length;
                buffer = ReadOnlySpan<byte>.Empty;
            }
            else if (marker == '-')
            {
                if (buffer.Length < 2) JsonThrowHelper.ThrowJsonReaderException(ref this);
                Value = buffer;
                ValueType = JsonValueType.Number;
                Index += buffer.Length;
                _position += buffer.Length;
                buffer = ReadOnlySpan<byte>.Empty;
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
                JsonThrowHelper.ThrowJsonReaderException(ref this);
            }
        }

        private void ConsumeNumberUtf8MultiSegment(ref BufferReader<byte> reader)
        {
            //TODO: Increment Index by number of bytes advanced
            if (!reader.TryReadToAny(out ReadOnlySpan<byte> span, JsonConstants.Delimiters, advancePastDelimiter: false))
            {
                JsonThrowHelper.ThrowJsonReaderException(ref this);
            }

            Value = span;
            ValueType = JsonValueType.Number;
        }

        private void ConsumeNumberUtf8(ref ReadOnlySpan<byte> buffer)
        {
            int idx = buffer.IndexOfAny(JsonConstants.Delimiters);
            if (idx == -1)
            {
                JsonThrowHelper.ThrowJsonReaderException(ref this);
            }

            Value = buffer.Slice(0, idx);
            ValueType = JsonValueType.Number;
            buffer = buffer.Slice(idx);
            Index += idx;
            _position += idx;
        }

        private void ConsumeNullUtf8MultiSegment(ref BufferReader<byte> reader)
        {
            Value = JsonConstants.NullValue;
            ValueType = JsonValueType.Null;

            if (!reader.IsNext(JsonConstants.NullValue, advancePast: true))
            {
                JsonThrowHelper.ThrowJsonReaderException(ref this);
            }
            Index += 4;
        }

        private void ConsumeNullUtf8(ref ReadOnlySpan<byte> buffer)
        {
            Value = JsonConstants.NullValue;
            ValueType = JsonValueType.Null;

            if (!buffer.StartsWith(Value))
            {
                JsonThrowHelper.ThrowJsonReaderException(ref this);
            }
            buffer = buffer.Slice(4);
            Index += 4;
            _position += 4;
        }

        private void ConsumeFalseUtf8MultiSegment(ref BufferReader<byte> reader)
        {
            Value = JsonConstants.FalseValue;
            ValueType = JsonValueType.False;

            if (!reader.IsNext(JsonConstants.FalseValue, advancePast: true))
            {
                JsonThrowHelper.ThrowJsonReaderException(ref this);
            }
            Index += 5;
        }

        private void ConsumeFalseUtf8(ref ReadOnlySpan<byte> buffer)
        {
            Value = JsonConstants.FalseValue;
            ValueType = JsonValueType.False;

            if (!buffer.StartsWith(Value))
            {
                JsonThrowHelper.ThrowJsonReaderException(ref this);
            }
            buffer = buffer.Slice(5);
            Index += 5;
            _position += 5;
        }

        private void ConsumeTrueUtf8MultiSegment(ref BufferReader<byte> reader)
        {
            Value = JsonConstants.TrueValue;
            ValueType = JsonValueType.True;

            if (!reader.IsNext(JsonConstants.TrueValue, advancePast: true))
            {
                JsonThrowHelper.ThrowJsonReaderException(ref this);
            }
            Index += 4;
        }

        private void ConsumeTrueUtf8(ref ReadOnlySpan<byte> buffer)
        {
            Value = JsonConstants.TrueValue;
            ValueType = JsonValueType.True;

            if (!buffer.StartsWith(Value))
            {
                JsonThrowHelper.ThrowJsonReaderException(ref this);
            }
            buffer = buffer.Slice(4);
            Index += 4;
            _position += 4;
        }

        private void ConsumePropertyNameUtf8MultiSegment(ref BufferReader<byte> reader)
        {
            ConsumeStringUtf8MultiSegment(ref reader);

            if (!reader.TryPeek(out byte first))
                JsonThrowHelper.ThrowJsonReaderException(ref this);

            if (first <= JsonConstants.Space)
            {
                SkipWhiteSpace(ref reader);
                if (!reader.TryPeek(out first))
                    JsonThrowHelper.ThrowJsonReaderException(ref this);
            }

            // The next character must be a key / value seperator. Validate and skip.
            if (first != JsonConstants.KeyValueSeperator)
                JsonThrowHelper.ThrowJsonReaderException(ref this);

            TokenType = JsonTokenType.PropertyName;
            reader.Advance(1);
            Index++;
        }

        private void ConsumePropertyNameUtf8(ref ReadOnlySpan<byte> buffer)
        {
            ConsumeStringUtf8(ref buffer);

            var str = Encoding.UTF8.GetString(Value.ToArray());
            _path.Push(str);

            if (0 >= (uint)buffer.Length)
                JsonThrowHelper.ThrowJsonReaderException(ref this);

            byte first = buffer[0];

            if (first <= JsonConstants.Space)
            {
                SkipWhiteSpaceUtf8(ref buffer);
                if (0 >= (uint)buffer.Length)
                    JsonThrowHelper.ThrowJsonReaderException(ref this);
                first = buffer[0];
            }

            // The next character must be a key / value seperator. Validate and skip.
            if (first != JsonConstants.KeyValueSeperator)
            {
                JsonThrowHelper.ThrowJsonReaderException(ref this);
            }

            TokenType = JsonTokenType.PropertyName;
            buffer = buffer.Slice(1);
            Index++;
            _position++;
        }

        public bool TryReadUntil(out ReadOnlySpan<byte> span, byte delimiter)
        {
            ReadOnlySpan<byte> remaining = _reader.UnreadSpan;

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
            _reader.Advance(i + 1);
            return true;
        }

        private bool TryReadUntilSlow(out ReadOnlySpan<byte> span, byte delimiter, int skip)
        {
            BufferReader<byte> copy = _reader;
            if (skip > 0)
                _reader.Advance(skip);
            ReadOnlySpan<byte> remaining = _reader.UnreadSpan;

            while (!_reader.End)
            {
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
                                    _reader.Advance(index);
                                    goto Done;
                                }
                                goto KeepLooking;
                            }
                            else
                                counter++;
                        }
                    }

                Done:
                    ReadOnlySequence<byte> sequence = _reader.Sequence.Slice(copy.Position, _reader.Position);
                    _reader.Advance(1);
                    span = sequence.IsSingleSegment ? sequence.First.Span : sequence.ToArray();
                    return true;
                }
            KeepLooking:
                _reader.Advance(remaining.Length);
                remaining = _reader.CurrentSpan;
            }

            // Didn't find anything, reset our original state.
            _reader = copy;
            span = default;
            return false;
        }

        private void ConsumeStringWithNestedQuotes(ref BufferReader<byte> reader)
        {
            //TODO: Optimize looking for nested quotes
            //TODO: Avoid redoing first IndexOf searches

            BufferReader<byte> copy = reader;
            ReadOnlySpan<byte> buffer = reader.UnreadSpan;
            if (0 >= (uint)buffer.Length)
            {
                reader.Advance(buffer.Length);
                Index += buffer.Length;
                buffer = reader.CurrentSpan;
            }

            while (!reader.End)
            {
                int counter = 1;
                int index = buffer.IndexOf(JsonConstants.Quote);
                if (index != -1)
                {
                    // Found the delimiter. Move to it, slice, then move past it.
                    if (index == 0 || buffer[index - 1] != JsonConstants.ReverseSolidus)
                        goto Done;

                    for (int j = index - 2; j >= 0; j--)
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
                    goto KeepLooking;

                Done:
                    reader.Advance(index);
                    Index += index;
                    ReadOnlySequence<byte> sequence = reader.Sequence.Slice(copy.Position, reader.Position);
                    reader.Advance(1);
                    Index++;
                    Value = sequence.IsSingleSegment ? sequence.First.Span : sequence.ToArray();
                    ValueType = JsonValueType.String;
                    return;
                }
            KeepLooking:
                reader.Advance(index + 1);
                buffer = reader.UnreadSpan;
            }

            JsonThrowHelper.ThrowJsonReaderException(ref this);
        }

        private void ConsumeStringUtf8MultiSegmentSlow(ref BufferReader<byte> reader)
        {
            BufferReader<byte> copy = reader;
            ReadOnlySpan<byte> buffer = reader.UnreadSpan;
            if (0 >= (uint)buffer.Length)
            {
                reader.Advance(buffer.Length);
                Index += buffer.Length;
                buffer = reader.CurrentSpan;
            }

            while (!reader.End)
            {
                int index = buffer.IndexOf(JsonConstants.Quote);
                if (index != -1)
                {
                    // Found the delimiter. Move to it, slice, then move past it.
                    if (index == 0 || buffer[index - 1] != JsonConstants.ReverseSolidus)
                    {
                        reader.Advance(index);
                        Index += index;
                        ReadOnlySequence<byte> sequence = reader.Sequence.Slice(copy.Position, reader.Position);
                        reader.Advance(1);
                        Index++;
                        Value = sequence.IsSingleSegment ? sequence.First.Span : sequence.ToArray();
                        ValueType = JsonValueType.String;
                        return;
                    }
                    reader = copy;
                    ConsumeStringWithNestedQuotes(ref reader);
                }
                reader.Advance(buffer.Length);
                buffer = reader.CurrentSpan;
            }

            JsonThrowHelper.ThrowJsonReaderException(ref this);
        }

        private void ConsumeStringUtf8MultiSegment(ref BufferReader<byte> reader)
        {
            ReadOnlySpan<byte> buffer = reader.UnreadSpan;
            int idx = buffer.IndexOf(JsonConstants.Quote);
            if (idx < 0)
            {
                ConsumeStringUtf8MultiSegmentSlow(ref reader);
                return;
            }

            if (idx == 0 || buffer[idx - 1] != JsonConstants.ReverseSolidus)
            {
                Value = buffer.Slice(0, idx);
                ValueType = JsonValueType.String;

                idx++;
                reader.Advance(idx);
                Index += idx;
                return;
            }
            ConsumeStringWithNestedQuotes(ref reader);
        }

        private void ConsumeStringUtf8(ref ReadOnlySpan<byte> buffer)
        {
            int idx = buffer.IndexOf(JsonConstants.Quote);
            if (idx < 0)
                JsonThrowHelper.ThrowJsonReaderException(ref this);
            if (idx == 0 || buffer[idx - 1] != JsonConstants.ReverseSolidus)
            {
                Value = buffer.Slice(0, idx);
                ValueType = JsonValueType.String;

                idx++;
                buffer = buffer.Slice(idx);
                Index += idx;
                _position += idx;   // TODO: update line number for multi-line strings
                return;
            }
            ConsumeStringWithNestedQuotes(ref buffer);
        }

        private void ConsumeStringWithNestedQuotes(ref ReadOnlySpan<byte> buffer)
        {
            //TODO: Optimize looking for nested quotes
            //TODO: Avoid redoing first IndexOf search
            int i = 0;
            while (true)
            {
                int counter = 0;
                int foundIdx = buffer.Slice(i).IndexOf(JsonConstants.Quote);
                if (foundIdx == -1)
                    JsonThrowHelper.ThrowJsonReaderException(ref this);
                if (foundIdx == 0)
                    break;
                for (int j = i + foundIdx - 1; j >= i; j--)
                {
                    if (buffer[j] != JsonConstants.ReverseSolidus)
                    {
                        if (counter % 2 == 0)
                        {
                            i += foundIdx;
                            goto Done;
                        }
                        break;
                    }
                    else
                        counter++;
                }
                i += foundIdx + 1;
            }

        Done:
            Value = buffer.Slice(0, i);
            ValueType = JsonValueType.String;

            i++;
            buffer = buffer.Slice(i);
            Index += i;
            _position += i;
        }

        private void SkipWhiteSpaceUtf8(ref ReadOnlySpan<byte> buffer)
        {
            //Create local copy to avoid bounds checks.
            ReadOnlySpan<byte> bufferCopy = buffer;
            int i = 0;
            for (; i < bufferCopy.Length; i++)
            {
                byte val = bufferCopy[i];
                if (val != JsonConstants.Space &&
                    val != JsonConstants.CarriageReturn &&
                    val != JsonConstants.LineFeed &&
                    val != JsonConstants.Tab)
                {
                    break;
                }

                if (val == JsonConstants.LineFeed)
                {
                    _lineNumber++;
                    _position = 0;
                }
                else
                {
                    _position++;
                }
            }
            buffer = bufferCopy.Slice(i);
            Index += i;
        }
    }
}
