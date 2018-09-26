// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Reader;
using System.Collections.Generic;
using System.Diagnostics;

#if UseInstrumented
namespace System.Text.JsonLab.Instrumented
#else
namespace System.Text.JsonLab
#endif
{
    public ref partial struct Utf8JsonReader
    {
        // We are using a ulong to represent our nested state, so we can only go 64 levels deep.
        internal const int StackFreeMaxDepth = sizeof(ulong) * 8;

        internal readonly ReadOnlySpan<byte> _buffer;

        public int CurrentIndex { get; private set; }

        public int TokenStartIndex { get; private set; }

        public int MaxDepth
        {
            get
            {
                return _maxDepth;
            }
            set
            {
                if (value <= 0)
                    JsonThrowHelper.ThrowArgumentException("Max depth must be positive.");
                _maxDepth = value;
                if (_maxDepth > StackFreeMaxDepth)
                    _stack = new Stack<bool>();
            }
        }

        private int _maxDepth;

        private BufferReader<byte> _reader;

        private Stack<bool> _stack;

        // Depth tracks the recursive depth of the nested objects / arrays within the JSON data.
        public int Depth { get; private set; }

        // This mask represents a tiny stack to track the state during nested transitions.
        // The first bit represents the state of the current level (1 == object, 0 == array).
        // Each subsequent bit is the parent / containing type (object or array). Since this
        // reader does a linear scan, we only need to keep a single path as we go through the data.
        private ulong _containerMask;

        // These properties are helpers for determining the current state of the reader
        internal bool InArray => !_inObject;
        private bool _inObject;

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

        internal bool NoMoreData => CurrentIndex >= (uint)_buffer.Length;

#if UseInstrumented
        private int _lineNumber;
        private int _position;
#endif

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
            CurrentIndex = 0;
            TokenStartIndex = CurrentIndex;
            _stack = null;
            _maxDepth = StackFreeMaxDepth;

            TokenType = JsonTokenType.None;
            Value = ReadOnlySpan<byte>.Empty;
            ValueType = JsonValueType.Unknown;
            _inObject = false;

#if UseInstrumented
            _lineNumber = 1;
            _position = 0;
#endif
        }

        /// <summary>
        /// Read the next token from the data buffer.
        /// </summary>
        /// <returns>True if the token was read successfully, else false.</returns>
        public bool Read()
        {
#if !UseInstrumented
            return _isSingleSegment ? ReadSingleSegment() : ReadMultiSegment(ref _reader);
#else
            return ReadSingleSegment();
#endif
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
            Depth++;
            if (Depth > MaxDepth)
#if UseInstrumented
                throw new JsonReaderException($"Depth of {Depth} within an object is larger than max depth of {MaxDepth}", _lineNumber, _position);
#else
                JsonThrowHelper.ThrowJsonReaderException(ref this);
#endif
#if UseInstrumented
            _position++;
#endif

            if (Depth <= StackFreeMaxDepth)
                _containerMask = (_containerMask << 1) | 1;
            else
                _stack.Push(true);

            TokenType = JsonTokenType.StartObject;
            _inObject = true;
        }

        private void EndObject()
        {
#if UseInstrumented
            if (!_inObject)
                throw new JsonReaderException($"We are within an array but observed an '{(char)JsonConstants.CloseBrace}'", _lineNumber, _position);

            if (Depth <= 0)
                throw new JsonReaderException($"Mismatched number of start/end objects or arrays. Depth is {Depth} but must be greater than 0", _lineNumber, _position);
#else
            if (!_inObject || Depth <= 0)
                JsonThrowHelper.ThrowJsonReaderException(ref this);
#endif
            if (Depth <= StackFreeMaxDepth)
            {
                _containerMask >>= 1;
                _inObject = (_containerMask & 1) != 0;
            }
            else
            {
                _inObject = _stack.Pop();
            }

            Depth--;
            TokenType = JsonTokenType.EndObject;
        }

        private void StartArray()
        {
            Depth++;
            if (Depth > MaxDepth)
#if UseInstrumented
                throw new JsonReaderException($"Depth of {Depth} within an array is larger than max depth of {MaxDepth}", _lineNumber, _position);
#else
                JsonThrowHelper.ThrowJsonReaderException(ref this);
#endif
#if UseInstrumented
            _position++;
#endif

            if (Depth <= StackFreeMaxDepth)
                _containerMask = _containerMask << 1;
            else
                _stack.Push(false);

            TokenType = JsonTokenType.StartArray;
            _inObject = false;
        }

        private void EndArray()
        {
#if UseInstrumented
            if (_inObject)
                throw new JsonReaderException($"We are within an object but observed an '{(char)JsonConstants.CloseBracket}'", _lineNumber, _position);

            if (Depth <= 0)
                throw new JsonReaderException($"Mismatched number of start/end objects or arrays. Depth is {Depth} but must be greater than 0", _lineNumber, _position);
#else
            if (_inObject || Depth <= 0)
                JsonThrowHelper.ThrowJsonReaderException(ref this);
#endif
            if (Depth <= StackFreeMaxDepth)
            {
                _containerMask >>= 1;
                _inObject = (_containerMask & 1) != 0;
            }
            else
            {
                _inObject = _stack.Pop();
            }

            Depth--;
            TokenType = JsonTokenType.EndArray;
        }

        private void ReadFirstToken(byte first)
        {
            if (first == JsonConstants.OpenBrace)
            {
                Depth++;
                _containerMask = 1;
                TokenType = JsonTokenType.StartObject;
                ValueType = JsonValueType.Object;
                CurrentIndex++;
                _inObject = true;
#if UseInstrumented
                _position++;
#endif
            }
            else if (first == JsonConstants.OpenBracket)
            {
                Depth++;
                TokenType = JsonTokenType.StartArray;
                ValueType = JsonValueType.Array;
                CurrentIndex++;
#if UseInstrumented
                _position++;
#endif
            }
            else
            {
                ConsumeValue(first);
            }
        }

        private bool ReadSingleSegment()
        {
            bool retVal = false;

            if (CurrentIndex >= (uint)_buffer.Length)
                goto Done;

            byte first = _buffer[CurrentIndex];

            if (first <= JsonConstants.Space)
            {
                SkipWhiteSpace();
                if (CurrentIndex >= (uint)_buffer.Length)
                    goto Done;
                first = _buffer[CurrentIndex];
            }

            TokenStartIndex = CurrentIndex;

            if (TokenType == JsonTokenType.None)
            {
                goto ReadFirstToken;
            }

            if (TokenType == JsonTokenType.StartObject)
            {
                CurrentIndex++;
                if (first == JsonConstants.CloseBrace)
                {
#if UseInstrumented
                    _position++;
#endif
                    EndObject();
                }
                else
                {
                    if (first != JsonConstants.Quote)
#if UseInstrumented
                        throw new JsonReaderException($"Expected: {(char)JsonConstants.Quote} for start of property name. Instead reached '{(char)first}'.", _lineNumber, _position);
#else
                        JsonThrowHelper.ThrowJsonReaderException(ref this);
#endif

                    TokenStartIndex++;
#if UseInstrumented
                    _position++;
#endif
                    ConsumePropertyName();
                }
            }
            else if (TokenType == JsonTokenType.StartArray)
            {
                if (first == JsonConstants.CloseBracket)
                {
                    CurrentIndex++;
#if UseInstrumented
                    _position++;
#endif
                    EndArray();
                }
                else
                {
                    ConsumeValue(first);
                }
            }
            else if (TokenType == JsonTokenType.PropertyName)
            {
                ConsumeValue(first);
            }
            else
            {
                ConsumeNextToken(first);
            }

            retVal = true;

        Done:
            return retVal;

        ReadFirstToken:
            ReadFirstToken(first);
            retVal = true;
            goto Done;
        }

        /// <summary>
        /// This method consumes the next token regardless of whether we are inside an object or an array.
        /// For an object, it reads the next property name token. For an array, it just reads the next value.
        /// </summary>
        private void ConsumeNextToken(byte marker)
        {
            CurrentIndex++;
#if UseInstrumented
            _position++;
#endif
            if (marker == JsonConstants.ListSeperator)
            {
                if (CurrentIndex >= (uint)_buffer.Length)
#if UseInstrumented
                    throw new JsonReaderException($"Expected a start of a property name or value after '{(char)JsonConstants.ListSeperator}', but reached end of data instead.", _lineNumber, _position);
#else
                    JsonThrowHelper.ThrowJsonReaderException(ref this);
#endif
                byte first = _buffer[CurrentIndex];

                if (first <= JsonConstants.Space)
                {
                    SkipWhiteSpace();
                    // The next character must be a start of a property name or value.
                    if (CurrentIndex >= (uint)_buffer.Length)
#if UseInstrumented
                        throw new JsonReaderException($"Expected a start of a property name or value after '{(char)JsonConstants.ListSeperator}', but reached end of data instead.", _lineNumber, _position);
#else
                        JsonThrowHelper.ThrowJsonReaderException(ref this);
#endif
                    first = _buffer[CurrentIndex];
                }

                TokenStartIndex = CurrentIndex;
                if (_inObject)
                {
                    if (first != JsonConstants.Quote)
#if UseInstrumented
                        throw new JsonReaderException($"Expected a start of a string property name with '{JsonConstants.Quote}', instead we got '{(char)first}'.", _lineNumber, _position);
#else
                        JsonThrowHelper.ThrowJsonReaderException(ref this);
#endif
                    CurrentIndex++;
                    TokenStartIndex++;
#if UseInstrumented
                    _position++;
#endif
                    ConsumePropertyName();
                }
                else
                {
                    ConsumeValue(first);
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
#if UseInstrumented
                throw new JsonReaderException($"Expected either '{(char)JsonConstants.ListSeperator}', '{(char)JsonConstants.CloseBrace}', or '{(char)JsonConstants.CloseBracket}', instead we got '{(char)marker}'.", _lineNumber, _position);
#else
                JsonThrowHelper.ThrowJsonReaderException(ref this);
#endif
            }
        }

        /// <summary>
        /// This method contains the logic for processing the next value token and determining
        /// what type of data it is.
        /// </summary>
        private void ConsumeValue(byte marker)
        {
            TokenType = JsonTokenType.Value;

            if (marker == JsonConstants.Quote)
            {
                CurrentIndex++;
                TokenStartIndex++;
#if UseInstrumented
                _position++;
#endif
                ConsumeString();
            }
            else if (marker == JsonConstants.OpenBrace)
            {
                CurrentIndex++;
                StartObject();
                ValueType = JsonValueType.Object;
            }
            else if (marker == JsonConstants.OpenBracket)
            {
                CurrentIndex++;
                StartArray();
                ValueType = JsonValueType.Array;
            }
            else if ((uint)(marker - '0') <= '9' - '0' || marker == '-')
            {
                ConsumeNumber();
            }
            else if (marker == 'f')
            {
                ConsumeFalse();
            }
            else if (marker == 't')
            {
                ConsumeTrue();
            }
            else if (marker == 'n')
            {
                ConsumeNull();
            }
            else if (marker == '/')
            {
                // TODO: Comments?
                JsonThrowHelper.ThrowNotImplementedException();
            }
            else
            {
#if UseInstrumented
                throw new JsonReaderException($"Expected start of a value, instead we got '{(char)marker}'.", _lineNumber, _position);
#else
                JsonThrowHelper.ThrowJsonReaderException(ref this);
#endif
            }
        }

        private void ConsumeNumber()
        {
            ValueType = JsonValueType.Number;
            Value = GetNumber(_buffer.Slice(CurrentIndex));
            CurrentIndex += Value.Length;
#if UseInstrumented
            _position += Value.Length;
#endif
        }

        private void ConsumeNull()
        {
            Value = JsonConstants.NullValue;
            ValueType = JsonValueType.Null;

            if (!_buffer.Slice(CurrentIndex).StartsWith(Value))
            {
#if UseInstrumented
                ReadOnlySpan<byte> span = _buffer.Slice(CurrentIndex);
                int length = Math.Min(JsonConstants.NullValue.Length, span.Length);
                string message = "Expected a 'null' value, instead we get '";
                for (int i = 0; i < length; i++)
                {
                    message += (char)span[i];
                }
                message += "'";
                throw new JsonReaderException(message, _lineNumber, _position);
#else
                JsonThrowHelper.ThrowJsonReaderException(ref this);
#endif
            }
            CurrentIndex += 4;
#if UseInstrumented
            _position += 4;
#endif
        }

        private void ConsumeFalse()
        {
            Value = JsonConstants.FalseValue;
            ValueType = JsonValueType.False;

            if (!_buffer.Slice(CurrentIndex).StartsWith(Value))
            {
#if UseInstrumented
                ReadOnlySpan<byte> span = _buffer.Slice(CurrentIndex);
                int length = Math.Min(JsonConstants.FalseValue.Length, span.Length);
                string message = "Expected a 'false' value, instead we get '";
                for (int i = 0; i < length; i++)
                {
                    message += (char)span[i];
                }
                message += "'";
                throw new JsonReaderException(message, _lineNumber, _position);
#else
                JsonThrowHelper.ThrowJsonReaderException(ref this);
#endif
            }
            CurrentIndex += 5;
#if UseInstrumented
            _position += 5;
#endif
        }

        private void ConsumeTrue()
        {
            Value = JsonConstants.TrueValue;
            ValueType = JsonValueType.True;

            if (!_buffer.Slice(CurrentIndex).StartsWith(Value))
            {
#if UseInstrumented
                ReadOnlySpan<byte> span = _buffer.Slice(CurrentIndex);
                int length = Math.Min(JsonConstants.TrueValue.Length, span.Length);
                string message = "Expected a 'true' value, instead we get '";
                for (int i = 0; i < length; i++)
                {
                    message += (char)span[i];
                }
                message += "'";
                throw new JsonReaderException(message, _lineNumber, _position);
#else
                JsonThrowHelper.ThrowJsonReaderException(ref this);
#endif
            }
            CurrentIndex += 4;
#if UseInstrumented
            _position += 4;
#endif
        }

        private void ConsumePropertyName()
        {
            ConsumeString();

            //Create local copy to avoid bounds checks.
            ReadOnlySpan<byte> localCopy = _buffer;
            if (CurrentIndex >= (uint)localCopy.Length)
#if UseInstrumented
                throw new JsonReaderException("Expected a value following the property, but instead reached end of data.", _lineNumber, _position);
#else
                JsonThrowHelper.ThrowJsonReaderException(ref this);
#endif

            byte first = localCopy[CurrentIndex];

            if (first <= JsonConstants.Space)
            {
                SkipWhiteSpace();
                if (CurrentIndex >= (uint)localCopy.Length)
#if UseInstrumented
                    throw new JsonReaderException("Expected a value following the property, but instead reached end of data.", _lineNumber, _position);
#else
                    JsonThrowHelper.ThrowJsonReaderException(ref this);
#endif
                first = localCopy[CurrentIndex];
            }

            // The next character must be a key / value seperator. Validate and skip.
            if (first != JsonConstants.KeyValueSeperator)
            {
#if UseInstrumented
                throw new JsonReaderException($"Expected a '{(char)JsonConstants.KeyValueSeperator}' following the property string, but instead saw '{(char)first}'.", _lineNumber, _position);
#else
                JsonThrowHelper.ThrowJsonReaderException(ref this);
#endif
            }

            TokenType = JsonTokenType.PropertyName;
            CurrentIndex++;
#if UseInstrumented
            _position++;
#endif
        }

        private void ConsumeString()
        {
            //Create local copy to avoid bounds checks.
            ReadOnlySpan<byte> localCopy = _buffer;

            int idx = localCopy.Slice(CurrentIndex).IndexOf(JsonConstants.Quote);
            if (idx < 0)
#if UseInstrumented
                throw new JsonReaderException($"Expected a '{(char)JsonConstants.Quote}' to indicate end of string, but instead reached end of data.", _lineNumber, _position);
#else
                JsonThrowHelper.ThrowJsonReaderException(ref this);
#endif

            Debug.Assert(CurrentIndex >= 1);

            if (localCopy[idx + CurrentIndex - 1] != JsonConstants.ReverseSolidus)
            {
                Value = localCopy.Slice(CurrentIndex, idx);
                ValueType = JsonValueType.String;
                CurrentIndex += idx + 1;
#if UseInstrumented
                _position += idx + 1;
                if (Value.IndexOf((byte)'\n') != -1)
                    AdjustLineNumber(Value);
#endif
            }
            else
            {
                ConsumeStringWithNestedQuotes();
            }
        }

        private void ConsumeStringWithNestedQuotes()
        {
            //TODO: Optimize looking for nested quotes
            //TODO: Avoid redoing first IndexOf search
            int i = CurrentIndex;
            while (true)
            {
                int counter = 0;
                int foundIdx = _buffer.Slice(i).IndexOf(JsonConstants.Quote);
                if (foundIdx == -1)
#if UseInstrumented
                    throw new JsonReaderException($"Expected a '{(char)JsonConstants.Quote}' to indicate end of string, but instead reached end of data.", _lineNumber, _position);
#else
                    JsonThrowHelper.ThrowJsonReaderException(ref this);
#endif
                if (foundIdx == 0)
                    break;
                for (int j = i + foundIdx - 1; j >= i; j--)
                {
                    if (_buffer[j] != JsonConstants.ReverseSolidus)
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
            Value = _buffer.Slice(CurrentIndex, i - CurrentIndex);
            ValueType = JsonValueType.String;

            i++;
#if UseInstrumented
            if (Value.IndexOf((byte)'\n') != -1)
                AdjustLineNumber(Value);
            else
                _position += i - CurrentIndex;
#endif

            CurrentIndex = i;
        }

        private void SkipWhiteSpace()
        {
            //Create local copy to avoid bounds checks.
            ReadOnlySpan<byte> localCopy = _buffer;
            for (; CurrentIndex < localCopy.Length; CurrentIndex++)
            {
                byte val = localCopy[CurrentIndex];
                if (val != JsonConstants.Space &&
                    val != JsonConstants.CarriageReturn &&
                    val != JsonConstants.LineFeed &&
                    val != JsonConstants.Tab)
                {
                    break;
                }
#if UseInstrumented
                if (val == JsonConstants.LineFeed)
                {
                    _lineNumber++;
                    _position = 0;
                }
                else
                {
                    _position++;
                }
#endif
            }
        }

#if UseInstrumented
        private void AdjustLineNumber(ReadOnlySpan<byte> span)
        {
            //TODO: Avoid redoing first IndexOf search
            int index = span.IndexOf(JsonConstants.LineFeed);
            while (index != -1)
            {
                _lineNumber++;
                _position = 0;
                span = span.Slice(index + 1);
                index = span.IndexOf(JsonConstants.LineFeed);
            }
            _position = span.Length + 1;
        }
#endif

        // https://tools.ietf.org/html/rfc7159#section-6
        private ReadOnlySpan<byte> GetNumber(ReadOnlySpan<byte> data)
        {
            Debug.Assert(data.Length > 0);

            ReadOnlySpan<byte> delimiters = JsonConstants.Delimiters;

            int i = 0;
            byte nextByte = data[i];

            if (nextByte == '-')
            {
                i++;
                if (i >= data.Length)
#if UseInstrumented
                    throw new JsonReaderException($"Invalid number. Last character read: '{(char)nextByte}'. Expected a digit.", _lineNumber, _position);
#else
                    JsonThrowHelper.ThrowJsonReaderException(ref this);
#endif

                nextByte = data[i];
                if ((uint)(nextByte - '0') > '9' - '0')
#if UseInstrumented
                    throw new JsonReaderException($"Invalid number. Last character read: '{(char)nextByte}'. Expected a digit.", _lineNumber, _position);
#else
                    JsonThrowHelper.ThrowJsonReaderException(ref this);
#endif
            }

            Debug.Assert(nextByte >= '0' && nextByte <= '9');

            if (nextByte == '0')
            {
                i++;
                if (i < data.Length)
                {
                    nextByte = data[i];
                    if (delimiters.IndexOf(nextByte) != -1)
                        goto Done;

                    if (nextByte != '.' && nextByte != 'E' && nextByte != 'e')
#if UseInstrumented
                        throw new JsonReaderException($"Invalid number. Last character read: '{(char)nextByte}'. Expected '.' or 'E' or 'e'.", _lineNumber, _position);
#else
                        JsonThrowHelper.ThrowJsonReaderException(ref this);
#endif
                }
                else
                    goto Done;
            }
            else
            {
                i++;
                for (; i < data.Length; i++)
                {
                    nextByte = data[i];
                    if ((uint)(nextByte - '0') > '9' - '0')
                        break;
                }
                if (i >= data.Length || delimiters.IndexOf(nextByte) != -1)
                    goto Done;
                if (nextByte != '.' && nextByte != 'E' && nextByte != 'e')
#if UseInstrumented
                    throw new JsonReaderException($"Invalid number. Last character read: '{(char)nextByte}'. Expected '.' or 'E' or 'e'.", _lineNumber, _position);
#else
                    JsonThrowHelper.ThrowJsonReaderException(ref this);
#endif
            }

            Debug.Assert(nextByte == '.' || nextByte == 'E' || nextByte == 'e');

            if (nextByte == '.')
            {
                i++;
                if (i >= data.Length)
#if UseInstrumented
                    throw new JsonReaderException($"Invalid number. Last character read: '{(char)nextByte}'. Expected a digit.", _lineNumber, _position);
#else
                    JsonThrowHelper.ThrowJsonReaderException(ref this);
#endif
                nextByte = data[i];
                if ((uint)(nextByte - '0') > '9' - '0')
#if UseInstrumented
                    throw new JsonReaderException($"Invalid number. Last character read: '{(char)nextByte}'. Expected a digit.", _lineNumber, _position);
#else
                    JsonThrowHelper.ThrowJsonReaderException(ref this);
#endif
                i++;
                for (; i < data.Length; i++)
                {
                    nextByte = data[i];
                    if ((uint)(nextByte - '0') > '9' - '0')
                        break;
                }
                if (i >= data.Length || delimiters.IndexOf(nextByte) != -1)
                    goto Done;
                if (nextByte != 'E' && nextByte != 'e')
#if UseInstrumented
                    throw new JsonReaderException($"Invalid number. Last character read: '{(char)nextByte}'. Expected 'E' or 'e'.", _lineNumber, _position);
#else
                    JsonThrowHelper.ThrowJsonReaderException(ref this);
#endif
            }

            Debug.Assert(nextByte == 'E' || nextByte == 'e');
            i++;

            if (i >= data.Length)
#if UseInstrumented
                throw new JsonReaderException($"Invalid number. Last character read: '{(char)nextByte}'. Expected a digit.", _lineNumber, _position);
#else
                JsonThrowHelper.ThrowJsonReaderException(ref this);
#endif

            nextByte = data[i];
            if (nextByte == '+' || nextByte == '-')
            {
                i++;
                if (i >= data.Length)
#if UseInstrumented
                    throw new JsonReaderException($"Invalid number. Last character read: '{(char)nextByte}'. Expected a digit.", _lineNumber, _position);
#else
                    JsonThrowHelper.ThrowJsonReaderException(ref this);
#endif
                nextByte = data[i];
            }

            if ((uint)(nextByte - '0') > '9' - '0')
#if UseInstrumented
                throw new JsonReaderException($"Invalid number. Last character read: '{(char)nextByte}'. Expected a digit.", _lineNumber, _position);
#else
                JsonThrowHelper.ThrowJsonReaderException(ref this);
#endif
            i++;
            for (; i < data.Length; i++)
            {
                nextByte = data[i];
                if ((uint)(nextByte - '0') > '9' - '0')
                    break;
            }

            if (i < data.Length && delimiters.IndexOf(nextByte) == -1)
#if UseInstrumented
                throw new JsonReaderException($"Invalid end of number. Last character read: '{(char)nextByte}'. Expected a delimiter.", _lineNumber, _position);
#else
                JsonThrowHelper.ThrowJsonReaderException(ref this);
#endif

            Done:
            return data.Slice(0, i);
        }
    }
}
