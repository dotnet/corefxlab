// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Reader;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Text.JsonLab.JsonThrowHelper;

namespace System.Text.JsonLab
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
                    ThrowArgumentException("Max depth must be positive.");
                _maxDepth = value;
                if (_maxDepth > StackFreeMaxDepth)
                    _stack = new Stack<bool>();
            }
        }

        public bool Instrument { get; set; }

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

        public JsonReaderState State
            => new JsonReaderState
            {
                _containerMask = _containerMask,
                _depth = Depth,
                _inObject = _inObject,
                _searchedNextLast = _searchedNextLast,
                _stack = _stack,
                _tokenType = TokenType,
                _lineNumber = _lineNumber,
                _position = _position
            };

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
        internal readonly bool _isFinalBlock;
        private bool _isSingleValue;
        internal bool _isRetry;
        private bool _searchedNextLast;

        internal bool NoMoreData => CurrentIndex >= (uint)_buffer.Length;


        internal int _lineNumber;
        internal int _position;

        /// <summary>
        /// Constructs a new JsonReader instance. This is a stack-only type.
        /// </summary>
        /// <param name="data">The <see cref="Span{byte}"/> value to consume. </param>
        /// <param name="encoder">An encoder used for decoding bytes from <paramref name="data"/> into characters.</param>
        public Utf8JsonReader(ReadOnlySpan<byte> data)
        {
            _isRetry = false;
            _containerMask = 0;
            Depth = 0;
            _inObject = false;
            _searchedNextLast = false;
            _stack = null;
            TokenType = JsonTokenType.None;

            _reader = default;
            _isSingleSegment = true;
            _buffer = data;
            CurrentIndex = 0;
            TokenStartIndex = CurrentIndex;
            _maxDepth = StackFreeMaxDepth;
            Value = ReadOnlySpan<byte>.Empty;
            ValueType = JsonValueType.Unknown;
            _isFinalBlock = true;
            _isSingleValue = false;
            Instrument = false;

            _lineNumber = 1;
            _position = 0;
        }

        public Utf8JsonReader(ReadOnlySpan<byte> data, bool isFinalBlock, JsonReaderState state = default)
        {
            _isRetry = state != default;
            _containerMask = state._containerMask;
            Depth = state._depth;
            _inObject = state._inObject;
            _searchedNextLast = state._searchedNextLast;
            _stack = state._stack;
            TokenType = state._tokenType;

            _reader = default;
            _isSingleSegment = true;
            _buffer = data;
            CurrentIndex = 0;
            TokenStartIndex = CurrentIndex;
            _maxDepth = StackFreeMaxDepth;
            Value = ReadOnlySpan<byte>.Empty;
            ValueType = JsonValueType.Unknown;
            _isFinalBlock = isFinalBlock;
            _isSingleValue = false;
            Instrument = false;

            _lineNumber = state != default ? state._lineNumber : 1;
            _position = state._position;
        }

        /// <summary>
        /// Read the next token from the data buffer.
        /// </summary>
        /// <returns>True if the token was read successfully, else false.</returns>
        public bool Read()
        {
            return _isSingleSegment ? ReadSingleSegment() : ReadMultiSegment(ref _reader);
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
                ThrowJsonReaderException(ref this, ExceptionResource.ObjectDepthTooLarge);

            _position++;

            if (Depth <= StackFreeMaxDepth)
                _containerMask = (_containerMask << 1) | 1;
            else
                _stack.Push(true);

            TokenType = JsonTokenType.StartObject;
            _inObject = true;
        }

        private void EndObject()
        {
            if (!_inObject || Depth <= 0)
                ThrowJsonReaderException(ref this, ExceptionResource.ObjectEndWithinArray);

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
                ThrowJsonReaderException(ref this, ExceptionResource.ArrayDepthTooLarge);

            _position++;

            if (Depth <= StackFreeMaxDepth)
                _containerMask = _containerMask << 1;
            else
                _stack.Push(false);

            TokenType = JsonTokenType.StartArray;
            _inObject = false;
        }

        private void EndArray()
        {
            if (_inObject || Depth <= 0)
                ThrowJsonReaderException(ref this, ExceptionResource.ArrayEndWithinObject);

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

        private bool ReadFirstToken(byte first)
        {
            if (first == JsonConstants.OpenBrace)
            {
                Depth++;
                _containerMask = 1;
                TokenType = JsonTokenType.StartObject;
                ValueType = JsonValueType.Object;
                CurrentIndex++;
                _inObject = true;
                _position++;
            }
            else if (first == JsonConstants.OpenBracket)
            {
                Depth++;
                TokenType = JsonTokenType.StartArray;
                ValueType = JsonValueType.Array;
                CurrentIndex++;
                _position++;
            }
            else
            {
                _isSingleValue = true;
                if (ConsumeValue(first))
                {
                    if (CurrentIndex >= (uint)_buffer.Length)
                    {
                        return true;
                    }
                    ThrowJsonReaderException(ref this, ExceptionResource.ExpectedEndAfterSingleJson);
                }
                return false;
            }
            return true;
        }

        private bool ReadSingleSegment()
        {
            bool retVal = false;

            if (CurrentIndex >= (uint)_buffer.Length)
            {
                if (!_isSingleValue && _isFinalBlock)
                {
                    if (TokenType != JsonTokenType.EndArray && TokenType != JsonTokenType.EndObject)
                        ThrowJsonReaderException(ref this, ExceptionResource.InvalidEndOfJson);
                }
                goto Done;
            }

            byte first = _buffer[CurrentIndex];

            if (first <= JsonConstants.Space)
            {
                SkipWhiteSpace();
                if (CurrentIndex >= (uint)_buffer.Length)
                {
                    if (_isFinalBlock)
                    {
                        if (TokenType != JsonTokenType.EndArray && TokenType != JsonTokenType.EndObject)
                            ThrowJsonReaderException(ref this, ExceptionResource.InvalidEndOfJson);
                    }
                    goto Done;
                }
                first = _buffer[CurrentIndex];
            }

            TokenStartIndex = CurrentIndex;

            if (TokenType == JsonTokenType.None)
            {
                goto ReadFirstToken;
            }

            if (TokenType == JsonTokenType.StartObject)
            {
                _searchedNextLast = false;
                if (first == JsonConstants.CloseBrace)
                {
                    CurrentIndex++;
                    _position++;
                    EndObject();
                }
                else
                {
                    if (first != JsonConstants.Quote)
                        ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfPropertyNotFound, first);

                    TokenStartIndex++;
                    return ConsumePropertyName();
                }
            }
            else if (TokenType == JsonTokenType.StartArray)
            {
                _searchedNextLast = false;
                if (first == JsonConstants.CloseBracket)
                {
                    CurrentIndex++;
                    _position++;
                    EndArray();
                }
                else
                {
                    return ConsumeValue(first);
                }
            }
            else if (TokenType == JsonTokenType.PropertyName)
            {
                _searchedNextLast = false;
                return ConsumeValue(first);
            }
            else
            {
                if (ConsumeNextToken(first))
                {
                    _searchedNextLast = true;
                    return true;
                }
                return false;
            }

            retVal = true;

        Done:
            return retVal;

        ReadFirstToken:
            retVal = ReadFirstToken(first);
            goto Done;
        }

        /// <summary>
        /// This method consumes the next token regardless of whether we are inside an object or an array.
        /// For an object, it reads the next property name token. For an array, it just reads the next value.
        /// </summary>
        private bool ConsumeNextToken(byte marker)
        {
            CurrentIndex++;
            _position++;
            if (marker == JsonConstants.ListSeperator)
            {
                if (CurrentIndex >= (uint)_buffer.Length)
                {
                    if (_isFinalBlock)
                    {
                        ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfPropertyOrValueNotFound);
                    }
                    else return false;
                }
                byte first = _buffer[CurrentIndex];

                if (first <= JsonConstants.Space)
                {
                    SkipWhiteSpace();
                    // The next character must be a start of a property name or value.
                    if (CurrentIndex >= (uint)_buffer.Length)
                    {
                        if (_isFinalBlock)
                        {
                            ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfPropertyOrValueNotFound);
                        }
                        else return false;
                    }
                    first = _buffer[CurrentIndex];
                }

                TokenStartIndex = CurrentIndex;
                if (_inObject)
                {
                    if (first != JsonConstants.Quote)
                        ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfPropertyNotFound, first);
                    TokenStartIndex++;
                    return ConsumePropertyName();
                }
                else
                {
                    return ConsumeValue(first);
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
                if (_searchedNextLast)
                {
                    if (TokenType == JsonTokenType.Value)
                    {
                        CurrentIndex--;
                        _position--;
                        TokenStartIndex = CurrentIndex;
                        TokenType = JsonTokenType.Value;
                        return ConsumeValue(marker);
                    }
                }
                else
                {
                    CurrentIndex--;
                    _position--;
                }
                ThrowJsonReaderException(ref this, ExceptionResource.FoundInvalidCharacter, marker);
            }
            return true;
        }

        /// <summary>
        /// This method contains the logic for processing the next value token and determining
        /// what type of data it is.
        /// </summary>
        private bool ConsumeValue(byte marker)
        {
            if (marker == JsonConstants.Quote)
            {
                TokenStartIndex++;
                return ConsumeString();
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
                return ConsumeNumber();
            }
            else if (marker == 'f')
            {
                return ConsumeFalse();
            }
            else if (marker == 't')
            {
                return ConsumeTrue();
            }
            else if (marker == 'n')
            {
                return ConsumeNull();
            }
            else if (marker == '/')
            {
                // TODO: Comments?
                ThrowNotImplementedException();
            }
            else
            {
                ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfValueNotFound, marker);
            }
            return true;
        }

        private bool ConsumeNumber()
        {
            ValueType = JsonValueType.Number;
            if (!TryGetNumber(_buffer.Slice(CurrentIndex), out ReadOnlySpan<byte> number))
                return false;
            Value = number;
            TokenType = JsonTokenType.Value;
            CurrentIndex += Value.Length;
            _position += Value.Length;
            return true;
        }

        private bool ConsumeNull()
        {
            Value = JsonConstants.NullValue;
            ValueType = JsonValueType.Null;

            ReadOnlySpan<byte> span = _buffer.Slice(CurrentIndex);

            Debug.Assert(span.Length > 0 && span[0] == Value[0]);

            if (!span.StartsWith(Value))
            {
                if (_isFinalBlock)
                {
                    goto Throw;
                }
                else
                {
                    if (span.Length > 1 && span[1] != Value[1])
                        goto Throw;
                    if (span.Length > 2 && span[2] != Value[2])
                        goto Throw;
                    if (span.Length >= Value.Length)
                        goto Throw;
                    return false;
                }
            Throw:
                ThrowJsonReaderException(ref this, ExceptionResource.ExpectedNull, bytes: span);
            }
            TokenType = JsonTokenType.Value;
            CurrentIndex += 4;
            _position += 4;
            return true;
        }

        private bool ConsumeFalse()
        {
            Value = JsonConstants.FalseValue;
            ValueType = JsonValueType.False;

            ReadOnlySpan<byte> span = _buffer.Slice(CurrentIndex);

            Debug.Assert(span.Length > 0 && span[0] == Value[0]);

            if (!span.StartsWith(Value))
            {
                if (_isFinalBlock)
                {
                    goto Throw;
                }
                else
                {
                    if (span.Length > 1 && span[1] != Value[1])
                        goto Throw;
                    if (span.Length > 2 && span[2] != Value[2])
                        goto Throw;
                    if (span.Length > 3 && span[3] != Value[3])
                        goto Throw;
                    if (span.Length >= Value.Length)
                        goto Throw;
                    return false;
                }
            Throw:
                ThrowJsonReaderException(ref this, ExceptionResource.ExpectedFalse, bytes: span);
            }
            TokenType = JsonTokenType.Value;
            CurrentIndex += 5;
            _position += 5;
            return true;
        }

        private bool ConsumeTrue()
        {
            Value = JsonConstants.TrueValue;
            ValueType = JsonValueType.True;

            ReadOnlySpan<byte> span = _buffer.Slice(CurrentIndex);

            Debug.Assert(span.Length > 0 && span[0] == Value[0]);

            if (!span.StartsWith(Value))
            {
                if (_isFinalBlock)
                {
                    goto Throw;
                }
                else
                {
                    if (span.Length > 1 && span[1] != Value[1])
                        goto Throw;
                    if (span.Length > 2 && span[2] != Value[2])
                        goto Throw;
                    if (span.Length >= Value.Length)
                        goto Throw;
                    return false;
                }
            Throw:
                ThrowJsonReaderException(ref this, ExceptionResource.ExpectedTrue, bytes: span);
            }
            TokenType = JsonTokenType.Value;
            CurrentIndex += 4;
            _position += 4;
            return true;
        }

        private bool ConsumePropertyName()
        {
            if (!ConsumeString())
                return false;

            //Create local copy to avoid bounds checks.
            ReadOnlySpan<byte> localCopy = _buffer;
            if (CurrentIndex >= (uint)localCopy.Length)
            {
                if (_isFinalBlock)
                {
                    ThrowJsonReaderException(ref this, ExceptionResource.ExpectedValueAfterPropertyNameNotFound);
                }
                else return false;
            }

            byte first = localCopy[CurrentIndex];

            if (first <= JsonConstants.Space)
            {
                SkipWhiteSpace();
                if (CurrentIndex >= (uint)localCopy.Length)
                {
                    if (_isFinalBlock)
                    {
                        ThrowJsonReaderException(ref this, ExceptionResource.ExpectedValueAfterPropertyNameNotFound);
                    }
                    else return false;
                }
                first = localCopy[CurrentIndex];
            }

            // The next character must be a key / value seperator. Validate and skip.
            if (first != JsonConstants.KeyValueSeperator)
            {
                ThrowJsonReaderException(ref this, ExceptionResource.ExpectedSeparaterAfterPropertyNameNotFound, first);
            }

            TokenType = JsonTokenType.PropertyName;
            CurrentIndex++;
            _position++;
            return true;
        }

        private bool ConsumeString()
        {
            //Create local copy to avoid bounds checks.
            ReadOnlySpan<byte> localCopy = _buffer;

            Debug.Assert(localCopy.Length >= CurrentIndex + 1);

            int idx = localCopy.Slice(CurrentIndex + 1).IndexOf(JsonConstants.Quote);
            if (idx < 0)
            {
                if (_isFinalBlock)
                {
                    ThrowJsonReaderException(ref this, ExceptionResource.EndOfStringNotFound);
                }
                else return false;
            }

            if (localCopy[idx + CurrentIndex] != JsonConstants.ReverseSolidus)
            {
                CurrentIndex++;
                Value = localCopy.Slice(CurrentIndex, idx);
                ValueType = JsonValueType.String;
                TokenType = JsonTokenType.Value;
                CurrentIndex += idx + 1;

                if (Instrument)
                {
                    _position++;
                    _position += idx + 1;
                    if (Value.IndexOf((byte)'\n') != -1)
                        AdjustLineNumber(Value);
                }
                return true;
            }
            else
            {
                return ConsumeStringWithNestedQuotes();
            }
        }

        private bool ConsumeStringWithNestedQuotes()
        {
            //TODO: Optimize looking for nested quotes
            //TODO: Avoid redoing first IndexOf search
            int i = CurrentIndex + 1;
            while (true)
            {
                int counter = 0;
                int foundIdx = _buffer.Slice(i).IndexOf(JsonConstants.Quote);
                if (foundIdx == -1)
                {
                    if (_isFinalBlock)
                    {
                        ThrowJsonReaderException(ref this, ExceptionResource.EndOfStringNotFound);
                    }
                    else return false;
                }
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
            CurrentIndex++;
            Value = _buffer.Slice(CurrentIndex, i - CurrentIndex);
            ValueType = JsonValueType.String;
            TokenType = JsonTokenType.Value;

            i++;

            if (Instrument)
            {
                _position++;
                if (Value.IndexOf((byte)'\n') != -1)
                    AdjustLineNumber(Value);
                else
                    _position += i - CurrentIndex;
            }

            CurrentIndex = i;
            return true;
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

                if (Instrument)
                {
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
            }
        }

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

        // https://tools.ietf.org/html/rfc7159#section-6
        private bool TryGetNumber(ReadOnlySpan<byte> data, out ReadOnlySpan<byte> number)
        {
            Debug.Assert(data.Length > 0);

            ReadOnlySpan<byte> delimiters = JsonConstants.Delimiters;

            number = default;

            int i = 0;
            byte nextByte = data[i];

            if (nextByte == '-')
            {
                i++;
                if (i >= data.Length)
                {
                    if (_isFinalBlock)
                    {
                        ThrowJsonReaderException(ref this, ExceptionResource.ExpectedDigitNotFoundEndOfData, nextByte);
                    }
                    else return false;
                }

                nextByte = data[i];
                if ((uint)(nextByte - '0') > '9' - '0')
                    ThrowJsonReaderException(ref this, ExceptionResource.ExpectedDigitNotFound, nextByte);
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
                        ThrowJsonReaderException(ref this, ExceptionResource.ExpectedNextDigitComponentNotFound, nextByte);
                }
                else
                {
                    if (_isFinalBlock)
                        goto Done;
                    else return false;
                }
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
                if (i >= data.Length)
                {
                    if (_isFinalBlock)
                        goto Done;
                    else return false;
                }
                if (delimiters.IndexOf(nextByte) != -1)
                    goto Done;
                if (nextByte != '.' && nextByte != 'E' && nextByte != 'e')
                    ThrowJsonReaderException(ref this, ExceptionResource.ExpectedNextDigitComponentNotFound, nextByte);
            }

            Debug.Assert(nextByte == '.' || nextByte == 'E' || nextByte == 'e');

            if (nextByte == '.')
            {
                i++;
                if (i >= data.Length)
                {
                    if (_isFinalBlock)
                    {
                        ThrowJsonReaderException(ref this, ExceptionResource.ExpectedDigitNotFoundEndOfData, nextByte);
                    }
                    else return false;
                }
                nextByte = data[i];
                if ((uint)(nextByte - '0') > '9' - '0')
                    ThrowJsonReaderException(ref this, ExceptionResource.ExpectedDigitNotFound, nextByte);
                i++;
                for (; i < data.Length; i++)
                {
                    nextByte = data[i];
                    if ((uint)(nextByte - '0') > '9' - '0')
                        break;
                }
                if (i >= data.Length)
                {
                    if (_isFinalBlock)
                        goto Done;
                    else return false;
                }
                if (delimiters.IndexOf(nextByte) != -1)
                    goto Done;
                if (nextByte != 'E' && nextByte != 'e')
                    ThrowJsonReaderException(ref this, ExceptionResource.ExpectedNextDigitEValueNotFound, nextByte);
            }

            Debug.Assert(nextByte == 'E' || nextByte == 'e');
            i++;

            if (i >= data.Length)
            {
                if (_isFinalBlock)
                {
                    ThrowJsonReaderException(ref this, ExceptionResource.ExpectedDigitNotFoundEndOfData, nextByte);
                }
                else return false;
            }

            nextByte = data[i];
            if (nextByte == '+' || nextByte == '-')
            {
                i++;
                if (i >= data.Length)
                {
                    if (_isFinalBlock)
                    {
                        ThrowJsonReaderException(ref this, ExceptionResource.ExpectedDigitNotFoundEndOfData, nextByte);
                    }
                    else return false;
                }
                nextByte = data[i];
            }

            if ((uint)(nextByte - '0') > '9' - '0')
                ThrowJsonReaderException(ref this, ExceptionResource.ExpectedDigitNotFound, nextByte);

            i++;
            for (; i < data.Length; i++)
            {
                nextByte = data[i];
                if ((uint)(nextByte - '0') > '9' - '0')
                    break;
            }

            if (i < data.Length)
            {
                if (delimiters.IndexOf(nextByte) == -1)
                {
                    ThrowJsonReaderException(ref this, ExceptionResource.ExpectedEndOfDigitNotFound, nextByte);
                }
            }
            else if (!_isFinalBlock)
            {
                return false;
            }

        Done:
            number = data.Slice(0, i);
            return true;
        }
    }
}
