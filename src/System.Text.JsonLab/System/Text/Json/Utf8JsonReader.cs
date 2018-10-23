// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using static System.Text.JsonLab.JsonThrowHelper;

namespace System.Text.JsonLab
{
    public ref partial struct Utf8JsonReader
    {
        // We are using a ulong to represent our nested state, so we can only go 64 levels deep.
        internal const int StackFreeMaxDepth = sizeof(ulong) * 8;

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
                    _stack = new Stack<InternalJsonTokenType>();
            }
        }

        private int _maxDepth;

        private Stack<InternalJsonTokenType> _stack;

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
                    _stack = new Stack<InternalJsonTokenType>();
            }
        }

        private JsonReaderOptions _readerOptions;

        public JsonReaderState State
            => new JsonReaderState
            {
                _containerMask = _containerMask,
                _depth = Depth,
                _inObject = _inObject,
                _stack = _stack,
                _tokenType = TokenType,
                _lineNumber = _lineNumber,
                _position = _position,
                _isSingleValue = _isSingleValue
            };

        /// <summary>
        /// Gets the value as a ReadOnlySpan<byte> of the last processed token. The contents of this
        /// is only relevant when <see cref="TokenType" /> is <see cref="JsonTokenType.Value" /> or
        /// <see cref="JsonTokenType.PropertyName" />. Otherwise, this value should be set to
        /// <see cref="ReadOnlySpan{T}.Empty"/>.
        /// </summary>
        public ReadOnlySpan<byte> Value { get; private set; }

        private readonly bool _isSingleSegment;
        private bool _isFinalBlock;
        private bool _isSingleValue;

        internal bool ConsumedEverything => _consumed >= (uint)_buffer.Length && _isLastSegment;

        internal long _lineNumber;
        internal long _position;

        private byte[] _pooledArray;
        private SequencePosition _nextPosition;
        private ReadOnlySequence<byte> _data;
        private bool _isLastSegment;
        private Stream _stream;

        const int FirstSegmentSize = 1_024; //TODO: Is this necessary?
        const int StreamSegmentSize = 4_096;

        /// <summary>
        /// Constructs a new JsonReader instance. This is a stack-only type.
        /// </summary>
        /// <param name="jsonData">The <see cref="Span{byte}"/> value to consume. </param>
        /// <param name="encoder">An encoder used for decoding bytes from <paramref name="jsonData"/> into characters.</param>
        public Utf8JsonReader(ReadOnlySpan<byte> jsonData)
        {
            _containerMask = 0;
            Depth = 0;
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
            _isSingleValue = true;
            _readerOptions = JsonReaderOptions.Default;

            _pooledArray = null;
            _nextPosition = default;
            _data = default;
            _isLastSegment = true;
            _isSingleSegment = true;
            _stream = null;
        }

        public Utf8JsonReader(Stream jsonStream)
        {
            if (!jsonStream.CanRead)
                ThrowArgumentException("Stream must be readable");

            _containerMask = 0;
            Depth = 0;
            _inObject = false;
            _stack = null;
            TokenType = JsonTokenType.None;
            _lineNumber = 1;
            _position = 0;

            _pooledArray = ArrayPool<byte>.Shared.Rent(FirstSegmentSize);
            int numberOfBytes = jsonStream.Read(_pooledArray, 0, FirstSegmentSize);
            _isFinalBlock = numberOfBytes < FirstSegmentSize;

            _buffer = _pooledArray.AsSpan(0, numberOfBytes);
            _totalConsumed = 0;
            _consumed = 0;
            TokenStartIndex = _consumed;
            _maxDepth = StackFreeMaxDepth;
            Value = ReadOnlySpan<byte>.Empty;
            _isSingleValue = true;
            _readerOptions = JsonReaderOptions.Default;

            _nextPosition = default;
            _data = default;
            _isLastSegment = _isFinalBlock;
            _isSingleSegment = true;
            _stream = jsonStream;
        }

        public Utf8JsonReader(ReadOnlySpan<byte> jsonData, bool isFinalBlock, JsonReaderState state = default)
        {
            if (!state.IsDefault)
            {
                _containerMask = state._containerMask;
                Depth = state._depth;
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
                Depth = 0;
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
            _readerOptions = JsonReaderOptions.Default;

            _pooledArray = null;
            _nextPosition = default;
            _data = default;
            _isLastSegment = _isFinalBlock;
            _isSingleSegment = true;
            _stream = null;
        }

        private void ResizeBuffer(int minimumSize)
        {
            Debug.Assert(minimumSize > 0);
            Debug.Assert(_pooledArray != null);
            ArrayPool<byte>.Shared.Return(_pooledArray);
            _pooledArray = ArrayPool<byte>.Shared.Rent(minimumSize);
        }

        public void Dispose()
        {
            if (_pooledArray != null)
            {
                ArrayPool<byte>.Shared.Return(_pooledArray);
                _pooledArray = null;
            }
        }

        public Utf8JsonReader(in ReadOnlySequence<byte> jsonData)
        {
            _containerMask = 0;
            Depth = 0;
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
            _isSingleValue = true;
            _readerOptions = JsonReaderOptions.Default;

            _data = jsonData;

            if (jsonData.IsSingleSegment)
            {
                _isLastSegment = true;
                _nextPosition = default;
                _pooledArray = null;
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
                _isLastSegment = !jsonData.TryGet(ref _nextPosition, out _, advance: true);
                _pooledArray = ArrayPool<byte>.Shared.Rent(_buffer.Length * 2);
                _isSingleSegment = false;
            }
            _stream = null;
        }

        public Utf8JsonReader(in ReadOnlySequence<byte> jsonData, bool isFinalBlock, JsonReaderState state = default)
        {
            if (!state.IsDefault)
            {
                _containerMask = state._containerMask;
                Depth = state._depth;
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
                Depth = 0;
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
            _readerOptions = JsonReaderOptions.Default;

            _data = jsonData;

            if (jsonData.IsSingleSegment)
            {
                _nextPosition = default;
                _isLastSegment = isFinalBlock;
                _pooledArray = null;
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

                _isLastSegment = !jsonData.TryGet(ref _nextPosition, out _, advance: true) && isFinalBlock; // Don't re-order to avoid short-circuiting
                _pooledArray = ArrayPool<byte>.Shared.Rent(_buffer.Length * 2);
                _isSingleSegment = false;
            }
            _stream = null;
        }

        /// <summary>
        /// Read the next token from the data buffer.
        /// </summary>
        /// <returns>True if the token was read successfully, else false.</returns>
        public bool Read()
        {
            bool result = ReadSingleSegment();
            if (result || _isLastSegment)
                return result;
            else
                return ReadNextSegment();
        }

        private bool ReadNextSegment()
        {
            bool result = false;

            Debug.Assert(!_isLastSegment);

            if (_stream != null)
                return ReadNextStreamSegment();

            do
            {
                ReadOnlyMemory<byte> memory = default;
                while (true)
                {
                    bool noMoreData = !_data.TryGet(ref _nextPosition, out memory, advance: true);
                    if (noMoreData)
                        return false;
                    if (memory.Length != 0)
                        break;
                }

                if (_isFinalBlock)
                    _isLastSegment = !_data.TryGet(ref _nextPosition, out _, advance: false);

                ReadOnlySpan<byte> leftOver = default;
                if (_consumed < _buffer.Length)
                {
                    leftOver = _buffer.Slice(_consumed);
                }

                // TODO: Should this be a settable property?
                if (leftOver.Length >= 1_000_000)
                {
                    // A single JSON token exceeds 1 MB in size . In such a rare case, allocate.
                    // TODO: Slice based on current SequencePosition
                    _buffer = _data.Slice(Consumed).ToArray();
                    if (_isFinalBlock)
                        _isLastSegment = true;
                }
                else
                {
                    if (leftOver.Length > _buffer.Length - memory.Length)
                    {
                        if (leftOver.Length > int.MaxValue - memory.Length)
                            ThrowArgumentException("Current sequence segment size is too large to fit left over data from the previous segment into a 2 GB buffer.");

                        ResizeBuffer(leftOver.Length + memory.Length);
                    }

                    Span<byte> bufferSpan = _pooledArray;
                    leftOver.CopyTo(bufferSpan);

                    memory.Span.CopyTo(bufferSpan.Slice(leftOver.Length));
                    bufferSpan = bufferSpan.Slice(0, leftOver.Length + memory.Length);   // This is gauranteed to not overflow

                    _totalConsumed += _consumed;
                    _consumed = 0;

                    _buffer = bufferSpan;
                }

                result = ReadSingleSegment();
            } while (!result && !_isLastSegment);
            return result;
        }

        private bool ReadNextStreamSegment()
        {
            bool result = false;

            Debug.Assert(!_isLastSegment);
            Debug.Assert(_stream != null);

            do
            {
                ReadOnlySpan<byte> leftOver = default;
                if (_consumed < _buffer.Length)
                {
                    leftOver = _buffer.Slice(_consumed);
                }

                int amountToRead = StreamSegmentSize;
                if (leftOver.Length > 0)
                {
                    _stream.Position -= leftOver.Length;

                    // TODO: Should this be a settable property?
                    if (leftOver.Length >= 1_000_000)
                    {
                        // A single JSON token exceeds 1 MB in size . In such a rare case, allocate.
                        byte[] maxBuffer = new byte[2_000_000_000];
                        int maxBytes = _stream.Read(maxBuffer, 0, maxBuffer.Length);
                        _isFinalBlock = maxBytes < amountToRead;
                        _buffer = maxBuffer.AsSpan(0, maxBytes);
                        goto ReadNext;
                    }
                    else
                    {
                        if (Consumed == 0)
                        {
                            if (leftOver.Length > int.MaxValue - amountToRead)
                                ThrowArgumentException("Cannot fit left over data from the previous chunk and the next chunk of data into a 2 GB buffer.");

                            amountToRead += leftOver.Length;   // This is gauranteed to not overflow
                            ResizeBuffer(amountToRead);
                        }
                    }
                }

                if (_pooledArray.Length < amountToRead)
                    ResizeBuffer(amountToRead);

                Span<byte> bufferSpan = _pooledArray;
                leftOver.CopyTo(bufferSpan);

                int numberOfBytes = _stream.Read(_pooledArray, 0, amountToRead);

                _isLastSegment = numberOfBytes < amountToRead;

                _buffer = _pooledArray.AsSpan(0, numberOfBytes);   // This is gauranteed to not overflow

            ReadNext:
                _totalConsumed += _consumed;
                _consumed = 0;

                result = ReadSingleSegment();
            } while (!result && !_isLastSegment);
            return result;
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
                _stack.Push(InternalJsonTokenType.StartObject);

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
                _inObject = _stack.Pop() != InternalJsonTokenType.StartArray;
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
                _stack.Push(InternalJsonTokenType.StartArray);

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
                _inObject = _stack.Pop() != InternalJsonTokenType.StartArray;
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
                _consumed++;
                _position++;
                _inObject = true;
                _isSingleValue = false;
            }
            else if (first == JsonConstants.OpenBracket)
            {
                Depth++;
                TokenType = JsonTokenType.StartArray;
                _consumed++;
                _position++;
                _isSingleValue = false;
            }
            else
            {
                if ((uint)(first - '0') <= '9' - '0' || first == '-')
                {
                    if (!TryGetNumber(_buffer.Slice(_consumed), out ReadOnlySpan<byte> number))
                        return false;
                    Value = number;
                    TokenType = JsonTokenType.Number;
                    _consumed += Value.Length;
                    _position += Value.Length;
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

        private bool ReadSingleSegment()
        {
            bool retVal = false;

            if (_consumed >= (uint)_buffer.Length)
            {
                if (!_isSingleValue && IsLastSpan)
                {
                    if (TokenType != JsonTokenType.EndArray && TokenType != JsonTokenType.EndObject)
                        ThrowJsonReaderException(ref this, ExceptionResource.InvalidEndOfJson);
                }
                goto Done;
            }

            byte first = _buffer[_consumed];

            if (first <= JsonConstants.Space)
            {
                SkipWhiteSpace();
                if (_consumed >= (uint)_buffer.Length)
                {
                    if (!_isSingleValue && IsLastSpan)
                    {
                        if (TokenType != JsonTokenType.EndArray && TokenType != JsonTokenType.EndObject)
                            ThrowJsonReaderException(ref this, ExceptionResource.InvalidEndOfJson);
                    }
                    goto Done;
                }
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
                    _consumed++;
                    _position++;
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
                    _consumed++;
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
            _consumed++;
            _position++;

            if (_readerOptions != JsonReaderOptions.Default)
            {
                //TODO: Re-evaluate use of InternalResult enum for the common case
                if (_readerOptions == JsonReaderOptions.AllowComments)
                {
                    if (marker == JsonConstants.Solidus)
                    {
                        _consumed--;
                        _position--;
                        return ConsumeComment() ? InternalResult.Success : InternalResult.FailureRollback;
                    }
                    if (TokenType == JsonTokenType.Comment)
                    {
                        _consumed--;
                        _position--;
                        TokenType = (JsonTokenType)_stack.Pop();
                        if (ReadSingleSegment())
                            return InternalResult.Success;
                        else
                        {
                            _stack.Push((InternalJsonTokenType)TokenType);
                            return InternalResult.FailureRollback;
                        }
                    }
                }
                else
                {
                    // JsonReaderOptions.SkipComments
                    if (marker == JsonConstants.Solidus)
                    {
                        _consumed--;
                        _position--;
                        if (SkipComment())
                        {
                            if (_consumed >= (uint)_buffer.Length)
                            {
                                if (!_isSingleValue && IsLastSpan)
                                {
                                    if (TokenType != JsonTokenType.EndArray && TokenType != JsonTokenType.EndObject)
                                        ThrowJsonReaderException(ref this, ExceptionResource.InvalidEndOfJson);
                                }
                                return InternalResult.FalseNoRollback;
                            }

                            byte first = _buffer[_consumed];

                            if (first <= JsonConstants.Space)
                            {
                                SkipWhiteSpace();
                                if (_consumed >= (uint)_buffer.Length)
                                {
                                    if (!_isSingleValue && IsLastSpan)
                                    {
                                        if (TokenType != JsonTokenType.EndArray && TokenType != JsonTokenType.EndObject)
                                            ThrowJsonReaderException(ref this, ExceptionResource.InvalidEndOfJson);
                                    }
                                    return InternalResult.FalseNoRollback;
                                }
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
                if (_consumed >= (uint)_buffer.Length)
                {
                    if (IsLastSpan)
                    {
                        _consumed--;
                        _position--;
                        ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfPropertyOrValueNotFound);
                    }
                    else return InternalResult.FailureRollback;
                }
                byte first = _buffer[_consumed];

                if (first <= JsonConstants.Space)
                {
                    SkipWhiteSpace();
                    // The next character must be a start of a property name or value.
                    if (_consumed >= (uint)_buffer.Length)
                    {
                        if (IsLastSpan)
                        {
                            ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfPropertyOrValueNotFound);
                        }
                        else return InternalResult.FailureRollback;
                    }
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
                _consumed--;
                _position--;
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
                _consumed++;
                StartObject();
            }
            else if (marker == JsonConstants.OpenBracket)
            {
                _consumed++;
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
                                    if (_consumed >= (uint)_buffer.Length)
                                    {
                                        if (!_isSingleValue && IsLastSpan)
                                        {
                                            if (TokenType != JsonTokenType.EndArray && TokenType != JsonTokenType.EndObject)
                                                ThrowJsonReaderException(ref this, ExceptionResource.InvalidEndOfJson);
                                        }
                                        return false;
                                    }
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

                    if (_readerOptions != JsonReaderOptions.TrackPositionAsCodePoints)
                        _position += 2 + localCopy.Length;
                    else
                    {
                        OperationStatus status = Encodings.Utf8.ToUtf16Length(localCopy, out int bytesNeeded);
                        Debug.Assert(status == OperationStatus.Done);
                        _position += 2 + (bytesNeeded / 2);
                    }
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

            var span = localCopy.Slice(0, idx - 1);

            (int newLines, int newLineIndex) = JsonReaderHelper.CountNewLines(span);
            _lineNumber += newLines;
            if (newLineIndex != -1)
            {
                if (_readerOptions != JsonReaderOptions.TrackPositionAsCodePoints)
                    _position = idx - newLineIndex;
                else
                {
                    OperationStatus status = Encodings.Utf8.ToUtf16Length(span.Slice(newLineIndex), out int bytesNeeded);
                    Debug.Assert(status == OperationStatus.Done);
                    _position += 2 + (bytesNeeded / 2);
                }
            }
            else
            {
                if (_readerOptions != JsonReaderOptions.TrackPositionAsCodePoints)
                    _position += 4 + idx - 1;
                else
                {
                    OperationStatus status = Encodings.Utf8.ToUtf16Length(span, out int bytesNeeded);
                    Debug.Assert(status == OperationStatus.Done);
                    _position += 4 + (bytesNeeded / 2) - 1;
                }
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

                    if (_readerOptions != JsonReaderOptions.TrackPositionAsCodePoints)
                        _position += 2 + Value.Length;
                    else
                    {
                        OperationStatus status = Encodings.Utf8.ToUtf16Length(Value, out int bytesNeeded);
                        Debug.Assert(status == OperationStatus.Done);
                        _position += 2 + (bytesNeeded / 2);
                    }

                    goto Done;
                }
                else return false;
            }

            Value = localCopy.Slice(0, idx);
            _consumed++;
            _position = 0;
            _lineNumber++;
        Done:
            _stack.Push((InternalJsonTokenType)TokenType);
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
            _stack.Push((InternalJsonTokenType)TokenType);
            Value = localCopy.Slice(0, idx - 1);
            TokenType = JsonTokenType.Comment;
            _consumed += 4 + Value.Length;

            (int newLines, int newLineIndex) = JsonReaderHelper.CountNewLines(Value);
            _lineNumber += newLines;
            if (newLineIndex != -1)
            {
                if (_readerOptions != JsonReaderOptions.TrackPositionAsCodePoints)
                    _position = Value.Length - newLineIndex + 1;
                else
                {
                    OperationStatus status = Encodings.Utf8.ToUtf16Length(Value.Slice(newLineIndex), out int bytesNeeded);
                    Debug.Assert(status == OperationStatus.Done);
                    _position += 2 + (bytesNeeded / 2);
                }
            }
            else
            {
                if (_readerOptions != JsonReaderOptions.TrackPositionAsCodePoints)
                    _position += 4 + Value.Length;
                else
                {
                    OperationStatus status = Encodings.Utf8.ToUtf16Length(Value, out int bytesNeeded);
                    Debug.Assert(status == OperationStatus.Done);
                    _position += 4 + (bytesNeeded / 2);
                }
            }
            return true;
        }

        private bool ConsumeNumber()
        {
            if (!TryGetNumberLookForEnd(_buffer.Slice(_consumed), out ReadOnlySpan<byte> number))
                return false;
            Value = number;
            TokenType = JsonTokenType.Number;
            _consumed += Value.Length;
            _position += Value.Length;
            return true;
        }

        private bool ConsumeNull()
        {
            Value = JsonConstants.NullValue;

            ReadOnlySpan<byte> span = _buffer.Slice(_consumed);

            Debug.Assert(span.Length > 0 && span[0] == Value[0]);

            if (!span.StartsWith(Value))
            {
                if (IsLastSpan)
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
            TokenType = JsonTokenType.Null;
            _consumed += 4;
            _position += 4;
            return true;
        }

        private bool ConsumeFalse()
        {
            Value = JsonConstants.FalseValue;

            ReadOnlySpan<byte> span = _buffer.Slice(_consumed);

            Debug.Assert(span.Length > 0 && span[0] == Value[0]);

            if (!span.StartsWith(Value))
            {
                if (IsLastSpan)
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
            TokenType = JsonTokenType.False;
            _consumed += 5;
            _position += 5;
            return true;
        }

        private bool ConsumeTrue()
        {
            Value = JsonConstants.TrueValue;

            ReadOnlySpan<byte> span = _buffer.Slice(_consumed);

            Debug.Assert(span.Length > 0 && span[0] == Value[0]);

            if (!span.StartsWith(Value))
            {
                if (IsLastSpan)
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
            TokenType = JsonTokenType.True;
            _consumed += 4;
            _position += 4;
            return true;
        }

        private bool ConsumePropertyName()
        {
            if (!ConsumeString())
                return false;

            //Create local copy to avoid bounds checks.
            ReadOnlySpan<byte> localCopy = _buffer;
            if (_consumed >= (uint)localCopy.Length)
            {
                if (IsLastSpan)
                {
                    ThrowJsonReaderException(ref this, ExceptionResource.ExpectedValueAfterPropertyNameNotFound);
                }
                else return false;
            }

            byte first = localCopy[_consumed];

            if (first <= JsonConstants.Space)
            {
                SkipWhiteSpace();
                if (_consumed >= (uint)localCopy.Length)
                {
                    if (IsLastSpan)
                    {
                        ThrowJsonReaderException(ref this, ExceptionResource.ExpectedValueAfterPropertyNameNotFound);
                    }
                    else return false;
                }
                first = localCopy[_consumed];
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

            return ConsumeStringVectorized();
        }

        private bool ConsumeStringVectorized()
        {
            //Create local copy to avoid bounds checks.
            ReadOnlySpan<byte> localCopy = _buffer;

            int idx = localCopy.Slice(_consumed + 1).IndexOf(JsonConstants.Quote);
            if (idx < 0)
            {
                if (IsLastSpan)
                {
                    ThrowJsonReaderException(ref this, ExceptionResource.EndOfStringNotFound);
                }
                else return false;
            }

            if (localCopy[idx + _consumed] != JsonConstants.ReverseSolidus)
            {
                localCopy = localCopy.Slice(_consumed + 1, idx);

                if (localCopy.IndexOfAnyControlOrEscape() != -1)
                {
                    _position++;
                    if (ValidateEscaping_AndHex(localCopy))
                        goto Done;
                    return false;
                }

                if (_readerOptions != JsonReaderOptions.TrackPositionAsCodePoints)
                    _position += idx + 1;
                else
                {
                    OperationStatus status = Encodings.Utf8.ToUtf16Length(localCopy, out int bytesNeeded);
                    Debug.Assert(status == OperationStatus.Done);
                    _position += 1 + (bytesNeeded / 2);
                }

            Done:
                _position++;
                Value = localCopy;
                TokenType = JsonTokenType.String;
                _consumed += idx + 2;
                return true;
            }
            else
            {
                return ConsumeStringWithNestedQuotes();
            }
        }

        // https://tools.ietf.org/html/rfc8259#section-7
        private bool ValidateEscaping_AndHex(ReadOnlySpan<byte> data)
        {
            bool nextCharEscaped = false;
            int incrementBy = 1;
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
                        if (i >= data.Length)
                        {
                            if (IsLastSpan)
                                ThrowJsonReaderException(ref this, ExceptionResource.EndOfStringNotFound);
                            else
                                goto False;
                        }
                    }
                    nextCharEscaped = false;
                }
                else if (currentByte < JsonConstants.Space)
                {
                    ThrowJsonReaderException(ref this, ExceptionResource.InvalidCharacterWithinString, currentByte);
                }

                if (_readerOptions != JsonReaderOptions.TrackPositionAsCodePoints)
                    _position++;
                else
                {
                    if (currentByte >= 0x0 && currentByte <= 0x7F)
                    {
                        incrementBy = 1;
                    }
                    else if (currentByte >= 0xC0 && currentByte <= 0xDF)
                    {
                        incrementBy = 2;
                    }
                    else if (currentByte >= 0xE0 && currentByte <= 0xEF)
                    {
                        incrementBy = 3;
                    }
                    else if (currentByte >= 0xF0 && currentByte <= 0xF7)
                    {
                        incrementBy = 4;
                    }
                    else
                        incrementBy--;

                    if (incrementBy == 1)
                        _position += 1;
                }
            }
            return true;

        False:
            return false;
        }

        private bool ConsumeStringWithNestedQuotes()
        {
            //TODO: Optimize looking for nested quotes
            //TODO: Avoid redoing first IndexOf search
            int i = _consumed + 1;
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
                if (ValidateEscaping_AndHex(localCopy))
                    goto Done;
                return false;
            }

            if (_readerOptions != JsonReaderOptions.TrackPositionAsCodePoints)
                _position = i;
            else
            {
                OperationStatus status = Encodings.Utf8.ToUtf16Length(localCopy, out int bytesNeeded);
                Debug.Assert(status == OperationStatus.Done);
                _position += bytesNeeded / 2;
            }

        Done:
            _position++;
            Value = localCopy;
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
                    if (IsLastSpan)
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
                    if (IsLastSpan)
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
                    if (IsLastSpan)
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
                    if (IsLastSpan)
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
                    if (IsLastSpan)
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
                if (IsLastSpan)
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
                    if (IsLastSpan)
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
            else if (!IsLastSpan)
            {
                return false;
            }

        Done:
            number = data.Slice(0, i);
            return true;
        }

        // https://tools.ietf.org/html/rfc7159#section-6
        private bool TryGetNumberLookForEnd(ReadOnlySpan<byte> data, out ReadOnlySpan<byte> number)
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
                    if (IsLastSpan)
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
                    if (IsLastSpan)
                        ThrowJsonReaderException(ref this, ExceptionResource.ExpectedEndOfDigitNotFound, nextByte);
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
                    if (IsLastSpan)
                        ThrowJsonReaderException(ref this, ExceptionResource.ExpectedEndOfDigitNotFound, nextByte);
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
                    if (IsLastSpan)
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
                    if (IsLastSpan)
                        ThrowJsonReaderException(ref this, ExceptionResource.ExpectedEndOfDigitNotFound, nextByte);
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
                if (IsLastSpan)
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
                    if (IsLastSpan)
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
            else if (!IsLastSpan)
            {
                return false;
            }

        Done:
            number = data.Slice(0, i);
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
