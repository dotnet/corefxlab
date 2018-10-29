// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Diagnostics;

using static System.Text.JsonLab.JsonThrowHelper;

namespace System.Text.JsonLab
{
    public ref partial struct Utf8JsonReader
    {
        // We are using a ulong to represent our nested state, so we can only go 64 levels deep.
        internal const int StackFreeMaxDepth = sizeof(ulong) * 8;

        const int MinimumSegmentSize = 4_096;

        private ReadOnlySpan<byte> _buffer;

        private int _consumed;

        private long _totalConsumed;

        public long Consumed => _totalConsumed + _consumed;

        internal int TokenStartIndex { get; private set; }

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
                    _stack = new Stack<JsonTokenType>();
            }
        }

        private int _maxDepth;

        private Stack<JsonTokenType> _stack;

        // Depth tracks the recursive depth of the nested objects / arrays within the JSON data.
        public int CurrentDepth { get; private set; }

        // This mask represents a tiny stack to track the state during nested transitions.
        // The first bit represents the state of the current level (1 == object, 0 == array).
        // Each subsequent bit is the parent / containing type (object or array). Since this
        // reader does a linear scan, we only need to keep a single path as we go through the data.
        private ulong _containerMask;

        // These properties are helpers for determining the current state of the reader
        internal bool InArray => !_inObject;
        private bool _inObject;

        private bool IsLastSpan => _isFinalBlock && (_isSingleSegment || _isLastSegment);

        /// <summary>
        /// Gets the token type of the last processed token in the JSON stream.
        /// </summary>
        public JsonTokenType TokenType { get; private set; }

        public JsonReaderOptions Options
        {
            get
            {
                return _readerOptions;
            }
            set
            {
                _readerOptions = value;
                if (_readerOptions == JsonReaderOptions.AllowComments && _stack == null)
                    _stack = new Stack<JsonTokenType>();
            }
        }

        private JsonReaderOptions _readerOptions;

        public JsonReaderState CurrentState
            => new JsonReaderState
            {
                _containerMask = _containerMask,
                _currentDepth = CurrentDepth,
                _inObject = _inObject,
                _stack = _stack,
                _tokenType = TokenType,
                _lineNumber = _lineNumber,
                _position = _position,
                _isSingleValue = _isSingleValue,
                _sequencePosition = GetPosition(),
                _consumed = _consumed,
            };

        private SequencePosition GetPosition()
        {
            if (_currentPosition.GetObject() == null)
                return default;

            SequencePosition position = _data.GetPosition(Consumed);
            return position;
            // TODO: This fails - return _data.GetPosition(_consumed - _leftOverLength, _currentPosition);
        }

        /// <summary>
        /// Gets the value as a ReadOnlySpan<byte> of the last processed token. The contents of this
        /// is only relevant when <see cref="TokenType" /> is <see cref="JsonTokenType.Value" /> or
        /// <see cref="JsonTokenType.PropertyName" />. Otherwise, this value should be set to
        /// <see cref="ReadOnlySpan{T}.Empty"/>.
        /// </summary>
        public ReadOnlySpan<byte> Value { get; private set; }

        public ReadOnlySequence<byte> ValueSegment { get; private set; }
        private bool _isValueMultiSegment;

        public bool IsValueMultiSegment => _isValueMultiSegment;

        private readonly bool _isSingleSegment;
        private bool _isFinalBlock;
        private bool _isSingleValue;

        internal bool ConsumedEverything => _consumed >= (uint)_buffer.Length && _isLastSegment;

        internal long _lineNumber;
        internal long _position;

        private SequencePosition _nextPosition;
        private SequencePosition _currentPosition;
        private ReadOnlySequence<byte> _data;
        private bool _isLastSegment;

        /// <summary>
        /// Constructs a new JsonReader instance. This is a stack-only type.
        /// </summary>
        /// <param name="jsonData">The <see cref="Span{byte}"/> value to consume. </param>
        /// <param name="encoder">An encoder used for decoding bytes from <paramref name="jsonData"/> into characters.</param>
        public Utf8JsonReader(ReadOnlySpan<byte> jsonData)
        {
            _containerMask = 0;
            CurrentDepth = 0;
            _inObject = false;
            _stack = null;
            TokenType = JsonTokenType.None;
            _lineNumber = 1;
            _position = 0;

            _isFinalBlock = true;

            _buffer = jsonData;
            _totalConsumed = 0;
            _consumed = 0;
            TokenStartIndex = _consumed;
            _maxDepth = StackFreeMaxDepth;
            Value = ReadOnlySpan<byte>.Empty;
            ValueSegment = ReadOnlySequence<byte>.Empty;
            _isSingleValue = true;
            _readerOptions = JsonReaderOptions.Default;

            _nextPosition = default;
            _currentPosition = default;
            _data = default;
            _isLastSegment = true;
            _isSingleSegment = true;

            _isValueMultiSegment = false;
        }

        public Utf8JsonReader(ReadOnlySpan<byte> jsonData, bool isFinalBlock, JsonReaderState state = default)
        {
            if (!state.IsDefault)
            {
                _containerMask = state._containerMask;
                CurrentDepth = state._currentDepth;
                _inObject = state._inObject;
                _stack = state._stack;
                TokenType = state._tokenType;
                _lineNumber = state._lineNumber;
                _position = state._position;
                _isSingleValue = state._isSingleValue;
            }
            else
            {
                _containerMask = 0;
                CurrentDepth = 0;
                _inObject = false;
                _stack = null;
                TokenType = JsonTokenType.None;
                _lineNumber = 1;
                _position = 0;
                _isSingleValue = true;
            }

            _isFinalBlock = isFinalBlock;

            _buffer = jsonData;
            _totalConsumed = 0;
            _consumed = 0;
            TokenStartIndex = _consumed;
            _maxDepth = StackFreeMaxDepth;
            Value = ReadOnlySpan<byte>.Empty;
            ValueSegment = ReadOnlySequence<byte>.Empty;
            _readerOptions = JsonReaderOptions.Default;

            _nextPosition = default;
            _currentPosition = default;
            _data = default;
            _isLastSegment = _isFinalBlock;
            _isSingleSegment = true;

            _isValueMultiSegment = false;
        }

        public Utf8JsonReader(in ReadOnlySequence<byte> jsonData)
        {
            _containerMask = 0;
            CurrentDepth = 0;
            _inObject = false;
            _stack = null;
            TokenType = JsonTokenType.None;
            _lineNumber = 1;
            _position = 0;

            _isFinalBlock = true;

            _buffer = jsonData.First.Span;
            _totalConsumed = 0;
            _consumed = 0;
            TokenStartIndex = _consumed;
            _maxDepth = StackFreeMaxDepth;
            Value = ReadOnlySpan<byte>.Empty;
            ValueSegment = ReadOnlySequence<byte>.Empty;
            _isSingleValue = true;
            _readerOptions = JsonReaderOptions.Default;

            _data = jsonData;

            if (jsonData.IsSingleSegment)
            {
                _isLastSegment = true;
                _nextPosition = default;
                _currentPosition = default;
                _isSingleSegment = true;
            }
            else
            {
                _nextPosition = jsonData.Start;
                if (_buffer.Length == 0)
                {
                    while (jsonData.TryGet(ref _nextPosition, out ReadOnlyMemory<byte> memory, advance: true))
                    {
                        if (memory.Length != 0)
                        {
                            _buffer = memory.Span;
                            break;
                        }
                    }
                }
                _currentPosition = _nextPosition;
                _isLastSegment = !jsonData.TryGet(ref _nextPosition, out _, advance: true);
                _isSingleSegment = false;
            }

            _isValueMultiSegment = false;
        }

        public Utf8JsonReader(in ReadOnlySequence<byte> jsonData, bool isFinalBlock, JsonReaderState state = default)
        {
            if (!state.IsDefault)
            {
                _containerMask = state._containerMask;
                CurrentDepth = state._currentDepth;
                _inObject = state._inObject;
                _stack = state._stack;
                TokenType = state._tokenType;
                _lineNumber = state._lineNumber;
                _position = state._position;
                _isSingleValue = state._isSingleValue;
            }
            else
            {
                _containerMask = 0;
                CurrentDepth = 0;
                _inObject = false;
                _stack = null;
                TokenType = JsonTokenType.None;
                _lineNumber = 1;
                _position = 0;
                _isSingleValue = true;
            }

            _isFinalBlock = isFinalBlock;

            _buffer = jsonData.First.Span;
            _totalConsumed = 0;
            _consumed = 0;
            TokenStartIndex = _consumed;
            _maxDepth = StackFreeMaxDepth;
            Value = ReadOnlySpan<byte>.Empty;
            ValueSegment = ReadOnlySequence<byte>.Empty;
            _readerOptions = JsonReaderOptions.Default;

            _data = jsonData;

            if (jsonData.IsSingleSegment)
            {
                _nextPosition = default;
                _currentPosition = default;
                _isLastSegment = isFinalBlock;
                _isSingleSegment = true;
            }
            else
            {
                _nextPosition = jsonData.Start;
                if (_buffer.Length == 0)
                {
                    while (jsonData.TryGet(ref _nextPosition, out ReadOnlyMemory<byte> memory, advance: true))
                    {
                        if (memory.Length != 0)
                        {
                            _buffer = memory.Span;
                            break;
                        }
                    }
                }

                _currentPosition = _nextPosition;
                _isLastSegment = !jsonData.TryGet(ref _nextPosition, out _, advance: true) && isFinalBlock; // Don't re-order to avoid short-circuiting
                _isSingleSegment = false;
            }

            _isValueMultiSegment = false;
        }

        /// <summary>
        /// Read the next token from the data buffer.
        /// </summary>
        /// <returns>True if the token was read successfully, else false.</returns>
        public bool Read()
        {
            return ReadSingleSegment();
        }

        private bool GetNextSpan()
        {
            if (_isSingleSegment)
                return false;

            ReadOnlyMemory<byte> memory = default;
            while (true)
            {
                SequencePosition copy = _currentPosition;
                _currentPosition = _nextPosition;
                bool noMoreData = !_data.TryGet(ref _nextPosition, out memory, advance: true);
                if (noMoreData)
                {
                    _currentPosition = copy;
                    return false;
                }
                if (memory.Length != 0)
                    break;
            }

            if (_isFinalBlock)
                _isLastSegment = !_data.TryGet(ref _nextPosition, out _, advance: false);

            _buffer = memory.Span;
            _totalConsumed += _consumed;
            _consumed = 0;

            return true;
        }

        public void Dispose()
        {
        }

        public void Skip()
        {
            if (TokenType == JsonTokenType.PropertyName)
            {
                Read();
            }

            if (TokenType == JsonTokenType.StartObject || TokenType == JsonTokenType.StartArray)
            {
                int depth = CurrentDepth;
                while (Read() && depth < CurrentDepth)
                {
                }
            }
        }

        private void StartObject()
        {
            CurrentDepth++;
            if (CurrentDepth > MaxDepth)
                ThrowJsonReaderException(ref this, ExceptionResource.ObjectDepthTooLarge);

            _consumed++;
            _position++;

            if (CurrentDepth <= StackFreeMaxDepth)
                _containerMask = (_containerMask << 1) | 1;
            else
                _stack.Push(JsonTokenType.StartObject);

            TokenType = JsonTokenType.StartObject;
            _inObject = true;
        }

        private void EndObject()
        {
            if (!_inObject || CurrentDepth <= 0)
                ThrowJsonReaderException(ref this, ExceptionResource.ObjectEndWithinArray);

            _consumed++;
            _position++;

            if (CurrentDepth <= StackFreeMaxDepth)
            {
                _containerMask >>= 1;
                _inObject = (_containerMask & 1) != 0;
            }
            else
            {
                _inObject = _stack.Pop() != JsonTokenType.StartArray;
            }

            CurrentDepth--;
            TokenType = JsonTokenType.EndObject;
        }

        private void StartArray()
        {
            CurrentDepth++;
            if (CurrentDepth > MaxDepth)
                ThrowJsonReaderException(ref this, ExceptionResource.ArrayDepthTooLarge);

            _consumed++;
            _position++;

            if (CurrentDepth <= StackFreeMaxDepth)
                _containerMask = _containerMask << 1;
            else
                _stack.Push(JsonTokenType.StartArray);

            TokenType = JsonTokenType.StartArray;
            _inObject = false;
        }

        private void EndArray()
        {
            if (_inObject || CurrentDepth <= 0)
                ThrowJsonReaderException(ref this, ExceptionResource.ArrayEndWithinObject);

            _consumed++;
            _position++;

            if (CurrentDepth <= StackFreeMaxDepth)
            {
                _containerMask >>= 1;
                _inObject = (_containerMask & 1) != 0;
            }
            else
            {
                _inObject = _stack.Pop() != JsonTokenType.StartArray;
            }

            CurrentDepth--;
            TokenType = JsonTokenType.EndArray;
        }

        private bool ReadFirstToken(byte first)
        {
            if (first == JsonConstants.OpenBrace)
            {
                CurrentDepth++;
                _containerMask = 1;
                TokenType = JsonTokenType.StartObject;
                _consumed++;
                _position++;
                _inObject = true;
                _isSingleValue = false;
            }
            else if (first == JsonConstants.OpenBracket)
            {
                CurrentDepth++;
                TokenType = JsonTokenType.StartArray;
                _consumed++;
                _position++;
                _isSingleValue = false;
            }
            else
            {
                if ((uint)(first - '0') <= '9' - '0' || first == '-')
                {
                    if (!TryGetNumber(_buffer.Slice(_consumed), out int consumed))
                        return false;
                    TokenType = JsonTokenType.Number;
                    _consumed += consumed;
                    _position += consumed;
                    goto Done;
                }
                else if (ConsumeValue(first))
                {
                    goto Done;
                }

                return false;

            Done:
                if (_consumed >= (uint)_buffer.Length)
                {
                    return true;
                }

                if (_buffer[_consumed] <= JsonConstants.Space)
                {
                    SkipWhiteSpace();
                    if (_consumed >= (uint)_buffer.Length)
                    {
                        return true;
                    }
                }

                if (_readerOptions != JsonReaderOptions.Default)
                {
                    if (_readerOptions == JsonReaderOptions.AllowComments)
                    {
                        if (TokenType == JsonTokenType.Comment || _buffer[_consumed] == JsonConstants.Solidus)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        // JsonReaderOptions.SkipComments
                        if (_buffer[_consumed] == JsonConstants.Solidus)
                        {
                            return true;
                        }
                    }
                }
                ThrowJsonReaderException(ref this, ExceptionResource.ExpectedEndAfterSingleJson, _buffer[_consumed]);
            }
            return true;
        }


        private bool HasMoreData()
        {
            if (_consumed >= (uint)_buffer.Length)
            {
                if (!_isSingleValue && IsLastSpan)
                {
                    if (TokenType != JsonTokenType.EndArray && TokenType != JsonTokenType.EndObject)
                        ThrowJsonReaderException(ref this, ExceptionResource.InvalidEndOfJson);
                }
                if (!GetNextSpan())
                    return false;
            }
            return true;
        }

        private bool HasMoreData(ExceptionResource resource)
        {
            if (_consumed >= (uint)_buffer.Length)
            {
                if (IsLastSpan)
                {
                    ThrowJsonReaderException(ref this, resource);
                }
                if (!GetNextSpan())
                    return false;
            }
            return true;
        }

        private bool ReadSingleSegment()
        {
            bool retVal = false;
            _isValueMultiSegment = false;

            if (!HasMoreData())
                goto Done;

            byte first = _buffer[_consumed];

            if (first <= JsonConstants.Space)
            {
                SkipWhiteSpace();
                if (!HasMoreData())
                    goto Done;
                first = _buffer[_consumed];
            }

            TokenStartIndex = _consumed;

            if (TokenType == JsonTokenType.None)
            {
                goto ReadFirstToken;
            }

            if (TokenType == JsonTokenType.StartObject)
            {
                if (first == JsonConstants.CloseBrace)
                {
                    EndObject();
                }
                else
                {
                    if (first != JsonConstants.Quote)
                        ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfPropertyNotFound, first);

                    TokenStartIndex++;
                    int prevConsumed = _consumed;
                    long prevPosition = _position;
                    if (ConsumePropertyName())
                    {
                        return true;
                    }
                    _consumed = prevConsumed;
                    TokenType = JsonTokenType.StartObject;
                    _position = prevPosition;
                    return false;
                }
            }
            else if (TokenType == JsonTokenType.StartArray)
            {
                if (first == JsonConstants.CloseBracket)
                {
                    EndArray();
                }
                else
                {
                    return ConsumeValue(first);
                }
            }
            else if (TokenType == JsonTokenType.PropertyName)
            {
                return ConsumeValue(first);
            }
            else
            {
                int prevConsumed = _consumed;
                long prevPosition = _position;
                long prevLineNumber = _lineNumber;
                JsonTokenType prevTokenType = TokenType;
                InternalResult result = ConsumeNextToken(first);
                if (result == InternalResult.Success)
                {
                    return true;
                }
                if (result == InternalResult.FailureRollback)
                {
                    _consumed = prevConsumed;
                    TokenType = prevTokenType;
                    _position = prevPosition;
                    _lineNumber = prevLineNumber;
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
        private InternalResult ConsumeNextToken(byte marker)
        {
            if (_readerOptions != JsonReaderOptions.Default)
            {
                //TODO: Re-evaluate use of InternalResult enum for the common case
                if (_readerOptions == JsonReaderOptions.AllowComments)
                {
                    if (marker == JsonConstants.Solidus)
                    {
                        return ConsumeComment() ? InternalResult.Success : InternalResult.FailureRollback;
                    }
                    if (TokenType == JsonTokenType.Comment)
                    {
                        TokenType = _stack.Pop();
                        if (ReadSingleSegment())
                            return InternalResult.Success;
                        else
                        {
                            _stack.Push(TokenType);
                            return InternalResult.FailureRollback;
                        }
                    }
                }
                else
                {
                    // JsonReaderOptions.SkipComments
                    if (marker == JsonConstants.Solidus)
                    {
                        if (SkipComment())
                        {
                            if (!HasMoreData())
                                return InternalResult.FalseNoRollback;

                            byte first = _buffer[_consumed];

                            if (first <= JsonConstants.Space)
                            {
                                SkipWhiteSpace();
                                if (!HasMoreData())
                                    return InternalResult.FalseNoRollback;
                                first = _buffer[_consumed];
                            }

                            return ConsumeNextToken(first);
                        }
                        return InternalResult.FailureRollback;
                    }
                }
            }

            if (marker == JsonConstants.ListSeperator)
            {
                _consumed++;
                _position++;

                if (_consumed >= (uint)_buffer.Length)
                {
                    if (IsLastSpan)
                    {
                        _consumed--;
                        _position--;
                        ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfPropertyOrValueNotFound);
                    }
                    else
                    {
                        if (!GetNextSpan())
                            return InternalResult.FailureRollback;
                    }
                }
                byte first = _buffer[_consumed];

                if (first <= JsonConstants.Space)
                {
                    SkipWhiteSpace();
                    // The next character must be a start of a property name or value.
                    if (!HasMoreData(ExceptionResource.ExpectedStartOfPropertyOrValueNotFound))
                        return InternalResult.FailureRollback;
                    first = _buffer[_consumed];
                }

                TokenStartIndex = _consumed;
                if (_inObject)
                {
                    if (first != JsonConstants.Quote)
                        ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfPropertyNotFound, first);
                    TokenStartIndex++;
                    return ConsumePropertyName() ? InternalResult.Success : InternalResult.FailureRollback;
                }
                else
                {
                    return ConsumeValue(first) ? InternalResult.Success : InternalResult.FailureRollback;
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
                ThrowJsonReaderException(ref this, ExceptionResource.FoundInvalidCharacter, marker);
            }
            return InternalResult.Success;
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
                StartObject();
            }
            else if (marker == JsonConstants.OpenBracket)
            {
                StartArray();
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
            else
            {
                if (_readerOptions != JsonReaderOptions.Default)
                {
                    if (_readerOptions == JsonReaderOptions.AllowComments)
                    {
                        if (marker == JsonConstants.Solidus)
                        {
                            return ConsumeComment();
                        }
                    }
                    else
                    {
                        // JsonReaderOptions.SkipComments
                        if (marker == JsonConstants.Solidus)
                        {
                            if (SkipComment())
                            {
                                if (_consumed >= (uint)_buffer.Length)
                                {
                                    if (!_isSingleValue && IsLastSpan)
                                    {
                                        if (TokenType != JsonTokenType.EndArray && TokenType != JsonTokenType.EndObject)
                                            ThrowJsonReaderException(ref this, ExceptionResource.InvalidEndOfJson);
                                    }
                                    return false;
                                }

                                byte first = _buffer[_consumed];

                                if (first <= JsonConstants.Space)
                                {
                                    SkipWhiteSpace();
                                    if (!HasMoreData())
                                        return false;
                                    first = _buffer[_consumed];
                                }

                                return ConsumeValue(first);
                            }
                            return false;
                        }
                    }
                }

                ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfValueNotFound, marker);
            }
            return true;
        }

        private bool SkipComment()
        {
            //Create local copy to avoid bounds checks.
            ReadOnlySpan<byte> localCopy = _buffer.Slice(_consumed + 1);

            if (localCopy.Length > 0)
            {
                byte marker = localCopy[0];
                if (marker == JsonConstants.Solidus)
                    return SkipSingleLineComment(localCopy.Slice(1));
                else if (marker == '*')
                    return SkipMultiLineComment(localCopy.Slice(1));
                else
                    ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfValueNotFound, marker);
            }

            if (IsLastSpan)
            {
                ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfValueNotFound, JsonConstants.Solidus);
            }
            else return false;

            return true;
        }

        private bool SkipSingleLineComment(ReadOnlySpan<byte> localCopy)
        {
            //TODO: Match Json.NET's end of comment semantics
            int idx = localCopy.IndexOf(JsonConstants.LineFeed);
            if (idx == -1)
            {
                if (IsLastSpan)
                {
                    idx = localCopy.Length;
                    // Assume everything on this line is a comment and there is no more data.

                    _position += 2 + localCopy.Length;
                    goto Done;
                }
                else return false;
            }

            _consumed++;
            _position = 0;
            _lineNumber++;
        Done:
            _consumed += 2 + idx;
            return true;
        }

        private bool SkipMultiLineComment(ReadOnlySpan<byte> localCopy)
        {
            int idx = 0;
            while (true)
            {
                int foundIdx = localCopy.Slice(idx).IndexOf(JsonConstants.Solidus);
                if (foundIdx == -1)
                {
                    if (IsLastSpan)
                    {
                        ThrowJsonReaderException(ref this, ExceptionResource.EndOfCommentNotFound);
                    }
                    else return false;
                }
                if (foundIdx != 0 && localCopy[foundIdx + idx - 1] == '*')
                {
                    idx += foundIdx;
                    break;
                }
                idx += foundIdx + 1;
            }

            Debug.Assert(idx >= 1);
            _consumed += 3 + idx;

            (int newLines, int newLineIndex) = JsonReaderHelper.CountNewLines(localCopy.Slice(0, idx - 1));
            _lineNumber += newLines;
            if (newLineIndex != -1)
            {
                _position = idx - newLineIndex;
            }
            else
            {
                _position += 4 + idx - 1;
            }
            return true;
        }

        private bool ConsumeComment()
        {
            //Create local copy to avoid bounds checks.
            ReadOnlySpan<byte> localCopy = _buffer.Slice(_consumed + 1);

            if (localCopy.Length > 0)
            {
                byte marker = localCopy[0];
                if (marker == JsonConstants.Solidus)
                    return ConsumeSingleLineComment(localCopy.Slice(1));
                else if (marker == '*')
                    return ConsumeMultiLineComment(localCopy.Slice(1));
                else
                    ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfValueNotFound, marker);
            }

            if (IsLastSpan)
            {
                ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfValueNotFound, JsonConstants.Solidus);
            }
            else return false;

            return true;
        }

        private bool ConsumeSingleLineComment(ReadOnlySpan<byte> localCopy)
        {
            int idx = localCopy.IndexOf(JsonConstants.LineFeed);
            if (idx == -1)
            {
                if (IsLastSpan)
                {
                    // Assume everything on this line is a comment and there is no more data.
                    Value = localCopy;

                    _position += 2 + Value.Length;

                    goto Done;
                }
                else return false;
            }

            Value = localCopy.Slice(0, idx);
            _consumed++;
            _position = 0;
            _lineNumber++;
        Done:
            _stack.Push(TokenType);
            TokenType = JsonTokenType.Comment;
            _consumed += 2 + Value.Length;
            return true;
        }

        private bool ConsumeMultiLineComment(ReadOnlySpan<byte> localCopy)
        {
            int idx = 0;
            while (true)
            {
                int foundIdx = localCopy.Slice(idx).IndexOf(JsonConstants.Solidus);
                if (foundIdx == -1)
                {
                    if (IsLastSpan)
                    {
                        ThrowJsonReaderException(ref this, ExceptionResource.EndOfCommentNotFound);
                    }
                    else return false;
                }
                if (foundIdx != 0 && localCopy[foundIdx + idx - 1] == '*')
                {
                    idx += foundIdx;
                    break;
                }
                idx += foundIdx + 1;
            }

            Debug.Assert(idx >= 1);
            _stack.Push(TokenType);
            Value = localCopy.Slice(0, idx - 1);
            TokenType = JsonTokenType.Comment;
            _consumed += 4 + Value.Length;

            (int newLines, int newLineIndex) = JsonReaderHelper.CountNewLines(Value);
            _lineNumber += newLines;
            if (newLineIndex != -1)
            {
                _position = Value.Length - newLineIndex + 1;
            }
            else
            {
                _position += 4 + Value.Length;
            }
            return true;
        }

        private bool ConsumeNumber()
        {
            if (!TryGetNumber(_buffer.Slice(_consumed), out int consumed))
                return false;

            TokenType = JsonTokenType.Number;
            _consumed += consumed;
            _position += consumed;

            if (_consumed >= (uint)_buffer.Length)
            {
                if (IsLastSpan || !GetNextSpan())
                {
                    ThrowJsonReaderException(ref this, ExceptionResource.ExpectedEndOfDigitNotFound, _buffer[_consumed - 1]);
                }
            }

            if (JsonConstants.Delimiters.IndexOf(_buffer[_consumed]) >= 0)
                return true;

            ThrowJsonReaderException(ref this, ExceptionResource.ExpectedEndOfDigitNotFound, _buffer[_consumed]);
            return false;
        }

        private bool ConsumeLiteralMultiSegment(ReadOnlySpan<byte> span, ReadOnlySpan<byte> literalValue, ExceptionResource resource)
        {
            if (span.Length >= literalValue.Length || IsLastSpan)
            {
                goto Throw;
            }
            else
            {
                if (!literalValue.StartsWith(span))
                    goto Throw;

                ReadOnlySpan<byte> leftToMatch = literalValue.Slice(span.Length);

                SequencePosition startPosition = _currentPosition;
                int startConsumed = _consumed;

                while (true)
                {
                    if (!GetNextSpan())
                        return false;

                    span = _buffer;
                    if (span.StartsWith(leftToMatch))
                    {
                        _isValueMultiSegment = true;
                        SequencePosition start = new SequencePosition(startPosition.GetObject(), startPosition.GetInteger() + _consumed);
                        SequencePosition end = new SequencePosition(_currentPosition.GetObject(), _currentPosition.GetInteger() + leftToMatch.Length);
                        ValueSegment = _data.Slice(start, end);
                        return true;
                    }

                    if (!leftToMatch.StartsWith(span))
                        goto Throw;

                    leftToMatch = leftToMatch.Slice(span.Length);
                }
            }
        Throw:
            ThrowJsonReaderException(ref this, resource, bytes: span);
            return false;
        }

        private bool ConsumeNull()
        {
            ReadOnlySpan<byte> span = _buffer.Slice(_consumed);

            Debug.Assert(span.Length > 0 && span[0] == JsonConstants.NullValue[0]);

            if (!span.StartsWith(JsonConstants.NullValue))
            {
                if (ConsumeLiteralMultiSegment(span, JsonConstants.NullValue, ExceptionResource.ExpectedNull))
                {
                    goto Done;
                }
                return false;
            }

            Value = span.Slice(0, 4);
            _isValueMultiSegment = false;
        Done:
            TokenType = JsonTokenType.Null;
            _consumed += 4;
            _position += 4;
            return true;
        }

        private bool ConsumeFalse()
        {
            ReadOnlySpan<byte> span = _buffer.Slice(_consumed);

            Debug.Assert(span.Length > 0 && span[0] == JsonConstants.FalseValue[0]);

            if (!span.StartsWith(JsonConstants.FalseValue))
            {
                if (ConsumeLiteralMultiSegment(span, JsonConstants.FalseValue, ExceptionResource.ExpectedFalse))
                {
                    goto Done;
                }
                return false;
            }

            Value = span.Slice(0, 5);
            _isValueMultiSegment = false;
        Done:
            TokenType = JsonTokenType.False;
            _consumed += 5;
            _position += 5;
            return true;
        }

        private bool ConsumeTrue()
        {
            ReadOnlySpan<byte> span = _buffer.Slice(_consumed);

            Debug.Assert(span.Length > 0 && span[0] == JsonConstants.TrueValue[0]);

            if (!span.StartsWith(JsonConstants.TrueValue))
            {
                if (ConsumeLiteralMultiSegment(span, JsonConstants.TrueValue, ExceptionResource.ExpectedTrue))
                {
                    goto Done;
                }
                return false;
            }

            Value = span.Slice(0, 4);
            _isValueMultiSegment = false;
        Done:
            TokenType = JsonTokenType.True;
            _consumed += 4;
            _position += 4;
            return true;
        }

        private bool ConsumePropertyName()
        {
            if (!ConsumeString())
                return false;

            if (!HasMoreData(ExceptionResource.ExpectedValueAfterPropertyNameNotFound))
                return false;

            byte first = _buffer[_consumed];

            if (first <= JsonConstants.Space)
            {
                SkipWhiteSpace();
                if (!HasMoreData(ExceptionResource.ExpectedValueAfterPropertyNameNotFound))
                    return false;
                first = _buffer[_consumed];
            }

            // The next character must be a key / value seperator. Validate and skip.
            if (first != JsonConstants.KeyValueSeperator)
            {
                ThrowJsonReaderException(ref this, ExceptionResource.ExpectedSeparaterAfterPropertyNameNotFound, first);
            }

            _consumed++;
            _position++;
            TokenType = JsonTokenType.PropertyName;
            return true;
        }

        private bool ConsumeString()
        {
            Debug.Assert(_buffer.Length >= _consumed + 1);
            Debug.Assert(_buffer[_consumed] == JsonConstants.Quote);

            //Create local copy to avoid bounds checks.
            ReadOnlySpan<byte> localCopy = _buffer;

            int idx = localCopy.Slice(_consumed + 1).IndexOf(JsonConstants.Quote);
            if (idx < 0)
            {
                if (IsLastSpan)
                {
                    ThrowJsonReaderException(ref this, ExceptionResource.EndOfStringNotFound);
                }
                else
                {
                    return ConsumeStringMultiSegment();
                }
            }

            if (localCopy[idx + _consumed] != JsonConstants.ReverseSolidus)
            {
                localCopy = localCopy.Slice(_consumed + 1, idx);

                if (localCopy.IndexOfAnyControlOrEscape() != -1)
                {
                    _position++;
                    ValidateEscaping_AndHex(localCopy);
                    goto Done;
                }

                _position += idx + 1;

            Done:
                _position++;
                Value = localCopy;
                _isValueMultiSegment = false;
                TokenType = JsonTokenType.String;
                _consumed += idx + 2;
                return true;
            }
            else
            {
                return ConsumeStringWithNestedQuotes(idx);
            }
        }

        private bool ConsumeStringMultiSegment(int counter = 0)
        {
            SequencePosition startPosition = _currentPosition;
            SequencePosition end = default;
            int startConsumed = _consumed + 1;
            _isValueMultiSegment = true;
            int leftOver = _buffer.Length - _consumed;

            //TODO: Worth vectorizing?
            while (true)
            {
                if (!GetNextSpan())
                    return false;

                //Create local copy to avoid bounds checks.
                ReadOnlySpan<byte> localCopy = _buffer;
                int i = 0;
                for (; i < localCopy.Length; i++)
                {
                    byte nextByte = localCopy[i];
                    if (nextByte == JsonConstants.ReverseSolidus)
                        counter++;
                    else if (nextByte == JsonConstants.Quote)
                    {
                        if ((counter & 1) == 0)
                        {
                            end = new SequencePosition(_currentPosition.GetObject(), _currentPosition.GetInteger() + i);
                            if (i >= localCopy.Length - 1)
                            {
                                if (!GetNextSpan())
                                    return false;
                                i = -1; // _consumed will equal 0 below if we move to the next segment
                            }
                            _totalConsumed += leftOver;
                            _consumed = i + 1;
                            goto Done;
                        }
                        counter = 0;
                    }
                    else
                    {
                        counter = 0;
                    }
                }
            }

        Done:
            SequencePosition start = new SequencePosition(startPosition.GetObject(), startPosition.GetInteger() + startConsumed);
            ValueSegment = _data.Slice(start, end);
            ValidateEscaping_AndHex(ValueSegment);  //TODO: Can this be done while searching for end quote?
            _position++;    // TODO: Fix
            TokenType = JsonTokenType.String;
            return true;
        }

        // https://tools.ietf.org/html/rfc8259#section-7
        private void ValidateEscaping_AndHex(ReadOnlySpan<byte> data)
        {
            if (data[data.Length - 1] != JsonConstants.Quote)
            {
                ThrowJsonReaderException(ref this, ExceptionResource.EndOfStringNotFound);
            }
            bool nextCharEscaped = false;
            for (int i = 0; i < data.Length; i++)
            {
                byte currentByte = data[i];
                if (currentByte == JsonConstants.ReverseSolidus)
                {
                    nextCharEscaped = !nextCharEscaped;
                }
                else if (nextCharEscaped)
                {
                    int index = JsonConstants.EscapableChars.IndexOf(currentByte);
                    if (index == -1)
                        ThrowJsonReaderException(ref this, ExceptionResource.InvalidCharacterWithinString, currentByte);

                    if (currentByte == 'n')
                    {
                        _position = -1; // Should be 0, but we increment _position below already
                        _lineNumber++;
                    }
                    else if (currentByte == 'u')
                    {
                        _position++;
                        int startIndex = i + 1;
                        for (int j = startIndex; j < data.Length; j++)
                        {
                            byte nextByte = data[j];
                            if ((uint)(nextByte - '0') > '9' - '0' && (uint)(nextByte - 'A') > 'F' - 'A' && (uint)(nextByte - 'a') > 'f' - 'a')
                            {
                                ThrowJsonReaderException(ref this, ExceptionResource.InvalidCharacterWithinString, nextByte);
                            }
                            if (j - startIndex >= 4)
                                break;
                            _position++;
                        }
                        i += 4;
                    }
                    nextCharEscaped = false;
                }
                else if (currentByte < JsonConstants.Space)
                {
                    ThrowJsonReaderException(ref this, ExceptionResource.InvalidCharacterWithinString, currentByte);
                }

                _position++;
            }
        }

        // https://tools.ietf.org/html/rfc8259#section-7
        private void ValidateEscaping_AndHex(ReadOnlySequence<byte> sequence)
        {
            bool nextCharEscaped = false;
            ReadOnlySequence<byte>.Enumerator enumerator = sequence.GetEnumerator();
            while (enumerator.MoveNext())
            {
                ReadOnlySpan<byte> data = enumerator.Current.Span;
                int i = 0;
                while (true)
                {
                    if (i >= data.Length)
                        break;
                    byte currentByte = data[i];
                    if (currentByte == JsonConstants.ReverseSolidus)
                    {
                        nextCharEscaped = !nextCharEscaped;
                    }
                    else if (nextCharEscaped)
                    {
                        int index = JsonConstants.EscapableChars.IndexOf(currentByte);
                        if (index == -1)
                            ThrowJsonReaderException(ref this, ExceptionResource.InvalidCharacterWithinString, currentByte);

                        if (currentByte == 'n')
                        {
                            _position = -1; // Should be 0, but we increment _position below already
                            _lineNumber++;
                        }
                        else if (currentByte == 'u')
                        {
                            _position++;
                            int j = i + 1;
                            int count = 0;
                            while (true)
                            {
                                if (j >= data.Length)
                                {
                                    if (!enumerator.MoveNext())
                                        ThrowJsonReaderException(ref this, ExceptionResource.EndOfStringNotFound);
                                    data = enumerator.Current.Span;
                                    j = 0;
                                    i = 0;
                                }
                                byte nextByte = data[j];
                                if ((uint)(nextByte - '0') > '9' - '0' && (uint)(nextByte - 'A') > 'F' - 'A' && (uint)(nextByte - 'a') > 'f' - 'a')
                                {
                                    ThrowJsonReaderException(ref this, ExceptionResource.InvalidCharacterWithinString, nextByte);
                                }
                                count++;
                                if (count >= 4)
                                {
                                    i--;     // We increment i below already
                                    break;
                                }
                                _position++;
                                i++;
                                j++;
                            }
                        }
                        nextCharEscaped = false;
                    }
                    else if (currentByte < JsonConstants.Space)
                    {
                        ThrowJsonReaderException(ref this, ExceptionResource.InvalidCharacterWithinString, currentByte);
                    }

                    _position++;
                    i++;
                }
            }
        }

        private bool ConsumeStringWithNestedQuotes(int i)
        {
            //TODO: Optimize looking for nested quotes
            i += 1;
            while (true)
            {
                int counter = 0;
                int foundIdx = _buffer.Slice(i).IndexOf(JsonConstants.Quote);
                if (foundIdx == -1)
                {
                    if (IsLastSpan)
                    {
                        ThrowJsonReaderException(ref this, ExceptionResource.EndOfStringNotFound);
                    }
                    else
                    {
                        // No unescaped quote found, count how many backslashes exist at the end of the buffer.
                        counter = 0;
                        for (int j = _buffer.Length - 1; j >= 0; j--)
                        {
                            if (_buffer[j] == JsonConstants.ReverseSolidus)
                                counter++;
                            else
                                break;
                        }
                        return ConsumeStringMultiSegment(counter);
                    }
                }
                if (foundIdx == 0)
                    break;
                for (int j = i + foundIdx - 1; j >= i; j--)
                {
                    if (_buffer[j] != JsonConstants.ReverseSolidus)
                    {
                        if ((counter & 1) == 0)
                        {
                            i += foundIdx;
                            goto FoundEndOfString;
                        }
                        break;
                    }
                    else
                        counter++;
                }
                i += foundIdx + 1;
            }

        FoundEndOfString:
            int startIndex = _consumed + 1;
            ReadOnlySpan<byte> localCopy = _buffer.Slice(startIndex, i - startIndex);

            if (localCopy.IndexOfAnyControlOrEscape() != -1)
            {
                _position++;
                ValidateEscaping_AndHex(localCopy);
                goto Done;
            }

            _position = i;

        Done:
            _position++;
            Value = localCopy;
            _isValueMultiSegment = false;
            TokenType = JsonTokenType.String;
            _consumed = i + 1;
            return true;
        }

        private void SkipWhiteSpace()
        {
            //Create local copy to avoid bounds checks.
            ReadOnlySpan<byte> localCopy = _buffer;
            for (; _consumed < localCopy.Length; _consumed++)
            {
                byte val = localCopy[_consumed];
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
            if (_consumed >= localCopy.Length)
            {
                if (!GetNextSpan())
                    return;
                SkipWhiteSpace();
            }
        }

        private InternalNumberResult ConsumeNegativeSign(ref ReadOnlySpan<byte> data, ref int i)
        {
            byte nextByte = data[i];

            if (nextByte == '-')
            {
                i++;
                if (i >= data.Length)
                {
                    if (IsLastSpan)
                    {
                        ThrowJsonReaderException(ref this, ExceptionResource.ExpectedDigitNotFoundEndOfData, nextByte);
                    }
                    else
                    {
                        if (!GetNextSpan())
                        {
                            return InternalNumberResult.NeedMoreData;
                        }
                        _isValueMultiSegment = true;
                        i = 0;
                        data = _buffer;
                    }
                }

                nextByte = data[i];
                if ((uint)(nextByte - '0') > '9' - '0')
                    ThrowJsonReaderException(ref this, ExceptionResource.ExpectedDigitNotFound, nextByte);
            }
            return InternalNumberResult.OperationIncomplete;
        }

        private InternalNumberResult ConsumeZero(ref ReadOnlySpan<byte> data, ref int i, ReadOnlySpan<byte> delimiters)
        {
            i++;
            byte nextByte = data[i];
            if (i < data.Length)
            {
                if (delimiters.IndexOf(nextByte) >= 0)
                    return InternalNumberResult.Success;
            }
            else
            {
                if (IsLastSpan)
                    return InternalNumberResult.Success;
                else
                {
                    if (!GetNextSpan())
                    {
                        return InternalNumberResult.NeedMoreData;
                    }
                    _isValueMultiSegment = true;
                    i = 0;
                    data = _buffer;
                    nextByte = data[i];
                }
            }

            if (nextByte != '.' && nextByte != 'E' && nextByte != 'e')
                ThrowJsonReaderException(ref this, ExceptionResource.ExpectedNextDigitComponentNotFound, nextByte);

            return InternalNumberResult.OperationIncomplete;
        }

        private InternalNumberResult ConsumeIntegerDigits(ref ReadOnlySpan<byte> data, ref int i, ReadOnlySpan<byte> delimiters)
        {
            byte nextByte = default;
            for (; i < data.Length; i++)
            {
                nextByte = data[i];
                if ((uint)(nextByte - '0') > '9' - '0')
                    break;
            }
            if (i >= data.Length)
            {
                if (IsLastSpan)
                    return InternalNumberResult.Success;
                else
                {
                    if (!GetNextSpan())
                    {
                        return InternalNumberResult.NeedMoreData;
                    }
                    _isValueMultiSegment = true;
                    i = 0;
                    data = _buffer;
                    nextByte = data[i];
                    return ConsumeIntegerDigits(ref data, ref i, delimiters);   // TODO: Go to iterative solution instead of recursive
                }
            }
            if (delimiters.IndexOf(nextByte) >= 0)
                return InternalNumberResult.Success;

            return InternalNumberResult.OperationIncomplete;
        }

        private InternalNumberResult ConsumeDecimalDigits(ref ReadOnlySpan<byte> data, ref int i, ReadOnlySpan<byte> delimiters, byte nextByte)
        {
            if (i >= data.Length)
            {
                if (IsLastSpan)
                {
                    ThrowJsonReaderException(ref this, ExceptionResource.ExpectedDigitNotFoundEndOfData, nextByte);
                }
                else
                {
                    if (!GetNextSpan())
                    {
                        return InternalNumberResult.NeedMoreData;
                    }
                    _isValueMultiSegment = true;
                    i = 0;
                    data = _buffer;
                }
            }
            nextByte = data[i];
            if ((uint)(nextByte - '0') > '9' - '0')
                ThrowJsonReaderException(ref this, ExceptionResource.ExpectedDigitNotFound, nextByte);
            i++;

            return ConsumeIntegerDigits(ref data, ref i, delimiters);
        }


        private InternalNumberResult ConsumeSign(ref ReadOnlySpan<byte> data, ref int i, byte nextByte)
        {
            if (i >= data.Length)
            {
                if (IsLastSpan)
                {
                    ThrowJsonReaderException(ref this, ExceptionResource.ExpectedDigitNotFoundEndOfData, nextByte);
                }
                else
                {
                    if (!GetNextSpan())
                    {
                        return InternalNumberResult.NeedMoreData;
                    }
                    _isValueMultiSegment = true;
                    i = 0;
                    data = _buffer;
                }
            }

            nextByte = data[i];
            if (nextByte == '+' || nextByte == '-')
            {
                i++;
                if (i >= data.Length)
                {
                    if (IsLastSpan)
                    {
                        ThrowJsonReaderException(ref this, ExceptionResource.ExpectedDigitNotFoundEndOfData, nextByte);
                    }
                    else
                    {
                        if (!GetNextSpan())
                        {
                            return InternalNumberResult.NeedMoreData;
                        }
                        _isValueMultiSegment = true;
                        i = 0;
                        data = _buffer;
                    }
                }
                nextByte = data[i];
            }

            if ((uint)(nextByte - '0') > '9' - '0')
                ThrowJsonReaderException(ref this, ExceptionResource.ExpectedDigitNotFound, nextByte);

            return InternalNumberResult.OperationIncomplete;
        }

        // https://tools.ietf.org/html/rfc7159#section-6
        private bool TryGetNumber(ReadOnlySpan<byte> data, out int consumed)
        {
            Debug.Assert(data.Length > 0);

            ReadOnlySpan<byte> delimiters = JsonConstants.Delimiters;
            SequencePosition startPosition = _currentPosition;
            int startConsumed = _consumed;
            consumed = 0;

            int i = 0;

            InternalNumberResult signResult = ConsumeNegativeSign(ref data, ref i);
            if (signResult == InternalNumberResult.NeedMoreData)
                return false;

            Debug.Assert(signResult == InternalNumberResult.OperationIncomplete);

            byte nextByte = data[i];
            Debug.Assert(nextByte >= '0' && nextByte <= '9');

            if (nextByte == '0')
            {
                InternalNumberResult result = ConsumeZero(ref data, ref i, delimiters);
                if (result == InternalNumberResult.NeedMoreData)
                    return false;
                if (result == InternalNumberResult.Success)
                    goto Done;

                nextByte = data[i];
            }
            else
            {
                i++;
                InternalNumberResult result = ConsumeIntegerDigits(ref data, ref i, delimiters);
                if (result == InternalNumberResult.NeedMoreData)
                    return false;
                if (result == InternalNumberResult.Success)
                    goto Done;

                nextByte = data[i];
                if (nextByte != '.' && nextByte != 'E' && nextByte != 'e')
                    ThrowJsonReaderException(ref this, ExceptionResource.ExpectedNextDigitComponentNotFound, nextByte);
            }

            Debug.Assert(nextByte == '.' || nextByte == 'E' || nextByte == 'e');

            if (nextByte == '.')
            {
                i++;
                InternalNumberResult result = ConsumeDecimalDigits(ref data, ref i, delimiters, nextByte);
                if (result == InternalNumberResult.NeedMoreData)
                    return false;
                if (result == InternalNumberResult.Success)
                    goto Done;

                nextByte = data[i];
                if (nextByte != 'E' && nextByte != 'e')
                    ThrowJsonReaderException(ref this, ExceptionResource.ExpectedNextDigitEValueNotFound, nextByte);
            }

            Debug.Assert(nextByte == 'E' || nextByte == 'e');
            i++;

            signResult = ConsumeSign(ref data, ref i, nextByte);
            if (signResult == InternalNumberResult.NeedMoreData)
                return false;

            Debug.Assert(signResult == InternalNumberResult.OperationIncomplete);

            i++;
            InternalNumberResult resultExponent = ConsumeIntegerDigits(ref data, ref i, delimiters);
            if (resultExponent == InternalNumberResult.NeedMoreData)
                return false;
            if (resultExponent == InternalNumberResult.Success)
                goto Done;
            if (resultExponent == InternalNumberResult.OperationIncomplete)
                ThrowJsonReaderException(ref this, ExceptionResource.ExpectedEndOfDigitNotFound, nextByte);

            Done:
            if (_isValueMultiSegment)
            {
                SequencePosition start = new SequencePosition(startPosition.GetObject(), startPosition.GetInteger() + startConsumed);
                SequencePosition end = new SequencePosition(_currentPosition.GetObject(), _currentPosition.GetInteger() + i);
                ValueSegment = _data.Slice(start, end);
                consumed = i;
            }
            else
            {
                Value = data.Slice(0, i);
                _isValueMultiSegment = false;
                consumed = i;
            }
            return true;
        }

        public string GetValueAsString()
        {
            //TODO: Perform additional validation and unescaping if necessary
            return Encodings.Utf8.ToString(Value);
        }

        public int GetValueAsInt32()
        {
            if (Utf8Parser.TryParse(Value, out int value, out int bytesConsumed))
            {
                if (Value.Length == bytesConsumed)
                {
                    return value;
                }
            }
            //TODO: Proper error message
            ThrowInvalidCastException();
            return default;
        }

        public long GetValueAsInt64()
        {
            if (Utf8Parser.TryParse(Value, out long value, out int bytesConsumed))
            {
                if (Value.Length == bytesConsumed)
                {
                    return value;
                }
            }
            ThrowInvalidCastException();
            return default;
        }

        public float GetValueAsSingle()
        {
            //TODO: We know whether this is true or not ahead of time
            if (Value.IndexOfAny((byte)'e', (byte)'E') == -1)
            {
                if (Utf8Parser.TryParse(Value, out float value, out int bytesConsumed))
                {
                    if (Value.Length == bytesConsumed)
                    {
                        return value;
                    }
                }
            }
            else
            {
                if (Utf8Parser.TryParse(Value, out float value, out int bytesConsumed, 'e'))
                {
                    if (Value.Length == bytesConsumed)
                    {
                        return value;
                    }
                }
            }
            ThrowInvalidCastException();
            return default;
        }

        public double GetValueAsDouble()
        {
            if (Value.IndexOfAny((byte)'e', (byte)'E') == -1)
            {
                if (Utf8Parser.TryParse(Value, out double value, out int bytesConsumed))
                {
                    if (Value.Length == bytesConsumed)
                    {
                        return value;
                    }
                }
            }
            else
            {
                if (Utf8Parser.TryParse(Value, out double value, out int bytesConsumed, standardFormat: 'e'))
                {
                    if (Value.Length == bytesConsumed)
                    {
                        return value;
                    }
                }
            }
            ThrowInvalidCastException();
            return default;
        }

        public decimal GetValueAsDecimal()
        {
            if (Value.IndexOfAny((byte)'e', (byte)'E') == -1)
            {
                if (Utf8Parser.TryParse(Value, out decimal value, out int bytesConsumed))
                {
                    if (Value.Length == bytesConsumed)
                    {
                        return value;
                    }
                }
            }
            else
            {
                if (Utf8Parser.TryParse(Value, out decimal value, out int bytesConsumed, standardFormat: 'e'))
                {
                    if (Value.Length == bytesConsumed)
                    {
                        return value;
                    }
                }
            }
            ThrowInvalidCastException();
            return default;
        }

        public object GetValueAsNumber()
        {
            if (Utf8Parser.TryParse(Value, out int intVal, out int bytesConsumed))
            {
                if (Value.Length == bytesConsumed)
                {
                    return intVal;
                }
            }

            if (Utf8Parser.TryParse(Value, out long longVal, out bytesConsumed))
            {
                if (Value.Length == bytesConsumed)
                {
                    return longVal;
                }
            }

            if (Value.IndexOfAny((byte)'e', (byte)'E') == -1)
            {
                return NumberAsObject(Value);
            }
            else
            {
                return NumberAsObject(Value, standardFormat: 'e');
            }
        }

        private static object NumberAsObject(ReadOnlySpan<byte> value, char standardFormat = default)
        {
            if (Utf8Parser.TryParse(value, out decimal valueDecimal, out int bytesConsumed, standardFormat))
            {
                if (value.Length == bytesConsumed)
                {
                    return TryToChangeToInt32_64(valueDecimal);
                }
            }
            else if (Utf8Parser.TryParse(value, out double valueDouble, out bytesConsumed, standardFormat))
            {
                if (value.Length == bytesConsumed)
                {
                    return TryToChangeToInt32_64(valueDouble);
                }
            }
            else if (Utf8Parser.TryParse(value, out float valueFloat, out bytesConsumed, standardFormat))
            {
                if (value.Length == bytesConsumed)
                {
                    return TryToChangeToInt32_64(valueFloat);
                }
            }

            // Number too large for .NET
            ThrowInvalidCastException();
            return default;
        }

        private static object TryToChangeToInt32_64(float valueFloat)
        {
            float rounded = (float)Math.Floor(valueFloat);
            if (rounded != valueFloat)
            {
                return valueFloat;
            }
            if (rounded <= int.MaxValue && rounded >= int.MinValue)
                return Convert.ToInt32(rounded);
            else if (rounded <= long.MaxValue && rounded >= long.MinValue)
                return Convert.ToInt64(rounded);
            else
                return valueFloat;
        }

        private static object TryToChangeToInt32_64(double valueDouble)
        {
            double rounded = Math.Floor(valueDouble);
            if (rounded != valueDouble)
            {
                return valueDouble;
            }
            if (rounded <= int.MaxValue && rounded >= int.MinValue)
                return Convert.ToInt32(rounded);
            else if (rounded <= long.MaxValue && rounded >= long.MinValue)
                return Convert.ToInt64(rounded);
            else
                return valueDouble;
        }

        private static object TryToChangeToInt32_64(decimal valueDecimal)
        {
            decimal rounded = Math.Floor(valueDecimal);
            if (rounded != valueDecimal)
            {
                return valueDecimal;
            }
            if (rounded <= int.MaxValue && rounded >= int.MinValue)
                return Convert.ToInt32(rounded);
            else if (rounded <= long.MaxValue && rounded >= long.MinValue)
                return Convert.ToInt64(rounded);
            else
                return valueDecimal;
        }
    }
}
