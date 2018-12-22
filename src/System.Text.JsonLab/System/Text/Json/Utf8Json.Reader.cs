// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Text;
using System.Diagnostics;
using static System.Text.JsonLab.JsonThrowHelper;

namespace System.Text.JsonLab
{
    public partial class Utf8Json
    {
        public ref struct Reader
        {
            const int MinimumSegmentSize = 4_096;

            private ReadOnlySpan<byte> _buffer;

            private int _consumed;

            private long _totalConsumed;

            public long Consumed => _totalConsumed + _consumed;

            internal int TokenStartIndex { get; private set; }

            // Depth tracks the recursive depth of the nested objects / arrays within the JSON data.
            public int CurrentDepth => _utf8Json._state._currentDepth;

            // These properties are helpers for determining the current state of the reader
            internal bool InArray => !_utf8Json._state._inObject;

            private bool IsLastSpan => _isFinalBlock && (_isSingleSegment || _isLastSegment);

            /// <summary>
            /// Gets the token type of the last processed token in the JSON stream.
            /// </summary>
            public JsonTokenType TokenType => _utf8Json._state._tokenType;

            /// <summary>
            /// Gets the value as a ReadOnlySpan<byte> of the last processed token. The contents of this
            /// is only relevant when <see cref="TokenType" /> is <see cref="JsonTokenType.Value" /> or
            /// <see cref="JsonTokenType.PropertyName" />. Otherwise, this value should be set to
            /// <see cref="ReadOnlySpan{T}.Empty"/>.
            /// </summary>
            public ReadOnlySpan<byte> Value { get; private set; }

            private readonly bool _isSingleSegment;
            private readonly bool _isFinalBlock;

            internal bool ConsumedEverything => _consumed >= (uint)_buffer.Length && _isLastSegment;

            private byte[] _pooledArray;
            private SequencePosition _nextPosition;
            private SequencePosition _currentPosition;
            private ReadOnlySequence<byte> _data;
            private bool _isLastSegment;
            private int _leftOverLength;

            internal Utf8Json _utf8Json;

            internal Reader(Utf8Json utf8Json, ReadOnlySpan<byte> jsonData)
            {
                _utf8Json = utf8Json;

                _isFinalBlock = true;

                _buffer = jsonData;
                _totalConsumed = 0;
                _consumed = 0;
                TokenStartIndex = _consumed;
                Value = ReadOnlySpan<byte>.Empty;

                _pooledArray = null;
                _nextPosition = default;
                _currentPosition = default;
                _data = default;
                _isLastSegment = true;
                _isSingleSegment = true;
                _leftOverLength = 0;
            }

            internal Reader(Utf8Json jsonReader, in ReadOnlySequence<byte> jsonData)
            {
                _utf8Json = jsonReader;

                _isFinalBlock = true;

                _buffer = jsonData.First.Span;
                _totalConsumed = 0;
                _consumed = 0;
                TokenStartIndex = _consumed;
                Value = ReadOnlySpan<byte>.Empty;

                _data = jsonData;

                if (jsonData.IsSingleSegment)
                {
                    _isLastSegment = true;
                    _nextPosition = default;
                    _currentPosition = default;
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
                    _currentPosition = _nextPosition;
                    _isLastSegment = !jsonData.TryGet(ref _nextPosition, out _, advance: true);
                    _pooledArray = ArrayPool<byte>.Shared.Rent(_buffer.Length * 2);
                    _isSingleSegment = false;
                }
                _leftOverLength = 0;
            }

            internal Reader(Utf8Json jsonReader, ReadOnlySpan<byte> jsonData, bool isFinalBlock)
            {
                _utf8Json = jsonReader;

                _isFinalBlock = isFinalBlock;

                _buffer = jsonData;
                _totalConsumed = 0;
                _consumed = 0;
                TokenStartIndex = _consumed;
                Value = ReadOnlySpan<byte>.Empty;

                _pooledArray = null;
                _nextPosition = default;
                _currentPosition = default;
                _data = default;
                _isLastSegment = _isFinalBlock;
                _isSingleSegment = true;
                _leftOverLength = 0;
            }

            internal Reader(Utf8Json jsonReader, in ReadOnlySequence<byte> jsonData, bool isFinalBlock)
            {
                _utf8Json = jsonReader;

                _isFinalBlock = isFinalBlock;

                _buffer = jsonData.First.Span;
                _totalConsumed = 0;
                _consumed = 0;
                TokenStartIndex = _consumed;
                Value = ReadOnlySpan<byte>.Empty;

                _data = jsonData;

                if (jsonData.IsSingleSegment)
                {
                    _nextPosition = default;
                    _currentPosition = default;
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

                    _currentPosition = _nextPosition;
                    _isLastSegment = !jsonData.TryGet(ref _nextPosition, out _, advance: true) && isFinalBlock; // Don't re-order to avoid short-circuiting
                    _pooledArray = ArrayPool<byte>.Shared.Rent(_buffer.Length * 2);
                    _isSingleSegment = false;
                }
                _leftOverLength = 0;
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
                if (_pooledArray == null)
                    return false;

                bool result = false;

                Debug.Assert(!_isLastSegment);

                do
                {
                    if (_consumed == 0)
                    {
                        if (!_data.TryGet(ref _nextPosition, out _, advance: false))
                            return false;
                        CopyNextSequences();
                    }
                    else
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

                        CopyLeftOverAndNext(memory);
                    }

                    result = ReadSingleSegment();
                } while (!result && !_isLastSegment);

                if (!result)
                {
                    _utf8Json._state._sequencePosition = GetPosition();
                }

                return result;
            }

            private SequencePosition GetPosition()
            {
                if (_currentPosition.GetObject() == null)
                    return default;

                SequencePosition position = _data.Slice(_consumed).GetPosition(0);
                return position;
                // TODO: This fails - return _data.GetPosition(_consumed - _leftOverLength, _currentPosition);
            }

            private void CopyLeftOverAndNext(ReadOnlyMemory<byte> memory)
            {
                if (_consumed < _buffer.Length)
                {
                    ReadOnlySpan<byte> leftOver = _buffer.Slice(_consumed);
                    _leftOverLength = leftOver.Length;

                    if (leftOver.Length > _buffer.Length - memory.Length)
                    {
                        if (leftOver.Length > int.MaxValue - memory.Length)
                            ThrowArgumentException("Current sequence segment size is too large to fit left over data from the previous segment into a 2 GB buffer.");

                        // This is guaranteed to not overflow due to the check above.
                        ResizeBuffer(leftOver.Length + memory.Length);
                    }

                    // This is guaranteed to not overflow
                    // -> _buffer.Length <= int.MaxValue, since it needs to fit in a span.
                    // -> leftOver.Length < _buffer.Length, since we leftOver is a slice of _buffer and _consumed != 0
                    // -> if leftOver.Length + memory.Length > _buffer.Length
                    // ->-> we check if leftOver.Length + memory.Length > int.MaxValue and throw above
                    // -> therefore, leftOver.Length + memory.Length < _buffer.Length <= int.MaxValue
                    Span<byte> bufferSpan = _pooledArray.AsSpan(0, leftOver.Length + memory.Length);
                    leftOver.CopyTo(bufferSpan);
                    memory.Span.CopyTo(bufferSpan.Slice(leftOver.Length));
                    _buffer = bufferSpan;
                    bufferSpan = default;
                }
                else
                {
                    _buffer = memory.Span;
                    _leftOverLength = 0;
                }

                _totalConsumed += _consumed;
                _consumed = 0;
            }

            private void CopyNextSequences()
            {
                Debug.Assert(_consumed == 0);

                // TODO: Should we try to support more than 1 GB?
                if (_buffer.Length > 1_000_000_000)
                    ThrowArgumentException("Current sequence segment size is too large to fit left over data from the previous segment into a 2 GB buffer.");

                // TODO: Always double or is it useful to have MinimumSegmentSize?
                // This is guaranteed to not overflow since _buffer.Length <= 1 billion
                int minSize = Math.Max(_buffer.Length * 2, MinimumSegmentSize);
                if (_pooledArray.Length <= minSize)
                    ResizeBuffer(minSize);

                Span<byte> bufferSpan = _pooledArray;
                _buffer.CopyTo(bufferSpan);
                bufferSpan = bufferSpan.Slice(_buffer.Length);

                int copied = 0;
                while (true)
                {
                    _currentPosition = _nextPosition;
                    SequencePosition prevNextPosition = _nextPosition;
                    if (_data.TryGet(ref _nextPosition, out ReadOnlyMemory<byte> memory, advance: true))
                    {
                        if (memory.Length == 0)
                        {
                            _currentPosition = prevNextPosition;
                            prevNextPosition = _nextPosition;
                            continue;
                        }

                        ReadOnlySpan<byte> currentSpan = memory.Span;
                        if (!currentSpan.TryCopyTo(bufferSpan.Slice(copied)))
                        {
                            _nextPosition = prevNextPosition;
                            break;
                        }

                        // This is guaranteed to not overflow:
                        // -> _pooledArray.Length <= int.MaxValue, therefore bufferSpan.Length <= int.MaxValue
                        // -> bufferSpan.Length - copied >= currentSpan.Length, since currentSpan.TryCopyTo succeeded
                        // -> int.MaxValue - copied >= currentSpan.Length, assuming maximum bufferSpan.Length possible
                        // -> int.MaxValue >= currentSpan.Length + copied
                        copied += currentSpan.Length;
                        _currentPosition = prevNextPosition;
                        prevNextPosition = _nextPosition;
                    }
                    else
                    {
                        _nextPosition = prevNextPosition;
                        if (_isFinalBlock)
                            _isLastSegment = true;
                        break;
                    }
                }
                // TODO: Set _leftOverLength?
                bufferSpan = default;
                Debug.Assert(_buffer.Length <= int.MaxValue - copied);
                _buffer = _pooledArray.AsSpan(0, _buffer.Length + copied);  // This is guaranteed to not overflow
            }

            public void Skip()
            {
                if (_utf8Json._state._tokenType == JsonTokenType.PropertyName)
                {
                    Read();
                }

                if (_utf8Json._state._tokenType == JsonTokenType.StartObject || _utf8Json._state._tokenType == JsonTokenType.StartArray)
                {
                    int depth = CurrentDepth;
                    while (Read() && depth < CurrentDepth)
                    {
                    }
                }
            }

            private void StartObject()
            {
                _utf8Json._state._currentDepth++;
                if (CurrentDepth > _utf8Json._maxDepth)
                    ThrowJsonReaderException(ref this, ExceptionResource.ObjectDepthTooLarge);

                _utf8Json._state._position++;

                if (CurrentDepth <= StackFreeMaxDepth)
                    _utf8Json._state._containerMask = (_utf8Json._state._containerMask << 1) | 1;
                else
                    _utf8Json._state._stack.Push((byte)JsonTokenType.StartObject);

                _utf8Json._state._tokenType = JsonTokenType.StartObject;
                _utf8Json._state._inObject = true;
            }

            private void EndObject()
            {
                if (!_utf8Json._state._inObject || CurrentDepth <= 0)
                    ThrowJsonReaderException(ref this, ExceptionResource.ObjectEndWithinArray);

                if (CurrentDepth <= StackFreeMaxDepth)
                {
                    _utf8Json._state._containerMask >>= 1;
                    _utf8Json._state._inObject = (_utf8Json._state._containerMask & 1) != 0;
                }
                else
                {
                    _utf8Json._state._inObject = (JsonTokenType)_utf8Json._state._stack.Pop() != JsonTokenType.StartArray;
                }

                _utf8Json._state._currentDepth--;
                _utf8Json._state._tokenType = JsonTokenType.EndObject;
            }

            private void StartArray()
            {
                _utf8Json._state._currentDepth++;
                if (CurrentDepth > _utf8Json._maxDepth)
                    ThrowJsonReaderException(ref this, ExceptionResource.ArrayDepthTooLarge);

                _utf8Json._state._position++;

                if (CurrentDepth <= StackFreeMaxDepth)
                    _utf8Json._state._containerMask = _utf8Json._state._containerMask << 1;
                else
                    _utf8Json._state._stack.Push((byte)JsonTokenType.StartArray);

                _utf8Json._state._tokenType = JsonTokenType.StartArray;
                _utf8Json._state._inObject = false;
            }

            private void EndArray()
            {
                if (_utf8Json._state._inObject || CurrentDepth <= 0)
                    ThrowJsonReaderException(ref this, ExceptionResource.ArrayEndWithinObject);

                if (CurrentDepth <= StackFreeMaxDepth)
                {
                    _utf8Json._state._containerMask >>= 1;
                    _utf8Json._state._inObject = (_utf8Json._state._containerMask & 1) != 0;
                }
                else
                {
                    _utf8Json._state._inObject = (JsonTokenType)_utf8Json._state._stack.Pop() != JsonTokenType.StartArray;
                }

                _utf8Json._state._currentDepth--;
                _utf8Json._state._tokenType = JsonTokenType.EndArray;
            }

            private bool ReadFirstToken(byte first)
            {
                if (first == JsonConstants.OpenBrace)
                {
                    _utf8Json._state._currentDepth++;
                    _utf8Json._state._containerMask = 1;
                    _utf8Json._state._tokenType = JsonTokenType.StartObject;
                    _consumed++;
                    _utf8Json._state._position++;
                    _utf8Json._state._inObject = true;
                    _utf8Json._state._isSingleValue = false;
                }
                else if (first == JsonConstants.OpenBracket)
                {
                    _utf8Json._state._currentDepth++;
                    _utf8Json._state._tokenType = JsonTokenType.StartArray;
                    _consumed++;
                    _utf8Json._state._position++;
                    _utf8Json._state._isSingleValue = false;
                }
                else
                {
                    if ((uint)(first - '0') <= '9' - '0' || first == '-')
                    {
                        if (!TryGetNumber(_buffer.Slice(_consumed), out ReadOnlySpan<byte> number))
                            return false;
                        Value = number;
                        _utf8Json._state._tokenType = JsonTokenType.Number;
                        _consumed += Value.Length;
                        _utf8Json._state._position += Value.Length;
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

                    if (_utf8Json._readerOptions._commentHandling != JsonReaderOptions.CommentHandling.Default)
                    {
                        if (_utf8Json._readerOptions._commentHandling == JsonReaderOptions.CommentHandling.AllowComments)
                        {
                            if (_utf8Json._state._tokenType == JsonTokenType.Comment || _buffer[_consumed] == JsonConstants.Solidus)
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
                    if (!_utf8Json._state._isSingleValue && IsLastSpan)
                    {
                        if (_utf8Json._state._tokenType != JsonTokenType.EndArray && _utf8Json._state._tokenType != JsonTokenType.EndObject)
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
                        if (!_utf8Json._state._isSingleValue && IsLastSpan)
                        {
                            if (_utf8Json._state._tokenType != JsonTokenType.EndArray && _utf8Json._state._tokenType != JsonTokenType.EndObject)
                                ThrowJsonReaderException(ref this, ExceptionResource.InvalidEndOfJson);
                        }
                        goto Done;
                    }
                    first = _buffer[_consumed];
                }

                TokenStartIndex = _consumed;

                if (_utf8Json._state._tokenType == JsonTokenType.None)
                {
                    goto ReadFirstToken;
                }

                if (_utf8Json._state._tokenType == JsonTokenType.StartObject)
                {
                    if (first == JsonConstants.CloseBrace)
                    {
                        _consumed++;
                        _utf8Json._state._position++;
                        EndObject();
                    }
                    else
                    {
                        if (first != JsonConstants.Quote)
                            ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfPropertyNotFound, first);

                        TokenStartIndex++;
                        int prevConsumed = _consumed;
                        long prevPosition = _utf8Json._state._position;
                        if (ConsumePropertyName())
                        {
                            return true;
                        }
                        _consumed = prevConsumed;
                        _utf8Json._state._tokenType = JsonTokenType.StartObject;
                        _utf8Json._state._position = prevPosition;
                        return false;
                    }
                }
                else if (_utf8Json._state._tokenType == JsonTokenType.StartArray)
                {
                    if (first == JsonConstants.CloseBracket)
                    {
                        _consumed++;
                        _utf8Json._state._position++;
                        EndArray();
                    }
                    else
                    {
                        return ConsumeValue(first);
                    }
                }
                else if (_utf8Json._state._tokenType == JsonTokenType.PropertyName)
                {
                    return ConsumeValue(first);
                }
                else
                {
                    int prevConsumed = _consumed;
                    long prevPosition = _utf8Json._state._position;
                    long prevLineNumber = _utf8Json._state._lineNumber;
                    JsonTokenType prevTokenType = _utf8Json._state._tokenType;
                    InternalResult result = ConsumeNextToken(first);
                    if (result == InternalResult.Success)
                    {
                        return true;
                    }
                    if (result == InternalResult.FailureRollback)
                    {
                        _consumed = prevConsumed;
                        _utf8Json._state._tokenType = prevTokenType;
                        _utf8Json._state._position = prevPosition;
                        _utf8Json._state._lineNumber = prevLineNumber;
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
                _utf8Json._state._position++;

                if (_utf8Json._readerOptions._commentHandling != JsonReaderOptions.CommentHandling.Default)
                {
                    //TODO: Re-evaluate use of InternalResult enum for the common case
                    if (_utf8Json._readerOptions._commentHandling == JsonReaderOptions.CommentHandling.AllowComments)
                    {
                        if (marker == JsonConstants.Solidus)
                        {
                            _consumed--;
                            _utf8Json._state._position--;
                            return ConsumeComment() ? InternalResult.Success : InternalResult.FailureRollback;
                        }
                        if (_utf8Json._state._tokenType == JsonTokenType.Comment)
                        {
                            _consumed--;
                            _utf8Json._state._position--;
                            _utf8Json._state._tokenType = (JsonTokenType)_utf8Json._state._stack.Pop();
                            if (ReadSingleSegment())
                                return InternalResult.Success;
                            else
                            {
                                _utf8Json._state._stack.Push((byte)_utf8Json._state._tokenType);
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
                            _utf8Json._state._position--;
                            if (SkipComment())
                            {
                                if (_consumed >= (uint)_buffer.Length)
                                {
                                    if (!_utf8Json._state._isSingleValue && IsLastSpan)
                                    {
                                        if (_utf8Json._state._tokenType != JsonTokenType.EndArray && _utf8Json._state._tokenType != JsonTokenType.EndObject)
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
                                        if (!_utf8Json._state._isSingleValue && IsLastSpan)
                                        {
                                            if (_utf8Json._state._tokenType != JsonTokenType.EndArray && _utf8Json._state._tokenType != JsonTokenType.EndObject)
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
                            _utf8Json._state._position--;
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
                    if (_utf8Json._state._inObject)
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
                    _utf8Json._state._position--;
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
                    if (_utf8Json._readerOptions._commentHandling != JsonReaderOptions.CommentHandling.Default)
                    {
                        if (_utf8Json._readerOptions._commentHandling == JsonReaderOptions.CommentHandling.AllowComments)
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
                                        if (!_utf8Json._state._isSingleValue && IsLastSpan)
                                        {
                                            if (_utf8Json._state._tokenType != JsonTokenType.EndArray && _utf8Json._state._tokenType != JsonTokenType.EndObject)
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
                                            if (!_utf8Json._state._isSingleValue && IsLastSpan)
                                            {
                                                if (_utf8Json._state._tokenType != JsonTokenType.EndArray && _utf8Json._state._tokenType != JsonTokenType.EndObject)
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

                        _utf8Json._state._position += 2 + localCopy.Length;
                        goto Done;
                    }
                    else return false;
                }

                _consumed++;
                _utf8Json._state._position = 0;
                _utf8Json._state._lineNumber++;
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
                _utf8Json._state._lineNumber += newLines;
                if (newLineIndex != -1)
                {
                    _utf8Json._state._position = idx - newLineIndex;
                }
                else
                {
                    _utf8Json._state._position += 4 + idx - 1;
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

                        _utf8Json._state._position += 2 + Value.Length;

                        goto Done;
                    }
                    else return false;
                }

                Value = localCopy.Slice(0, idx);
                _consumed++;
                _utf8Json._state._position = 0;
                _utf8Json._state._lineNumber++;
            Done:
                _utf8Json._state._stack.Push((byte)_utf8Json._state._tokenType);
                _utf8Json._state._tokenType = JsonTokenType.Comment;
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
                _utf8Json._state._stack.Push((byte)_utf8Json._state._tokenType);
                Value = localCopy.Slice(0, idx - 1);
                _utf8Json._state._tokenType = JsonTokenType.Comment;
                _consumed += 4 + Value.Length;

                (int newLines, int newLineIndex) = JsonReaderHelper.CountNewLines(Value);
                _utf8Json._state._lineNumber += newLines;
                if (newLineIndex != -1)
                {
                    _utf8Json._state._position = Value.Length - newLineIndex + 1;
                }
                else
                {
                    _utf8Json._state._position += 4 + Value.Length;
                }
                return true;
            }

            private bool ConsumeNumber()
            {
                if (!TryGetNumberLookForEnd(_buffer.Slice(_consumed), out ReadOnlySpan<byte> number))
                    return false;
                Value = number;
                _utf8Json._state._tokenType = JsonTokenType.Number;
                _consumed += Value.Length;
                _utf8Json._state._position += Value.Length;
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
                _utf8Json._state._tokenType = JsonTokenType.Null;
                _consumed += 4;
                _utf8Json._state._position += 4;
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
                _utf8Json._state._tokenType = JsonTokenType.False;
                _consumed += 5;
                _utf8Json._state._position += 5;
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
                _utf8Json._state._tokenType = JsonTokenType.True;
                _consumed += 4;
                _utf8Json._state._position += 4;
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
                _utf8Json._state._position++;
                _utf8Json._state._tokenType = JsonTokenType.PropertyName;
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
                        _utf8Json._state._position++;
                        if (ValidateEscaping_AndHex(localCopy))
                            goto Done;
                        return false;
                    }

                    _utf8Json._state._position += idx + 1;

                Done:
                    _utf8Json._state._position++;
                    Value = localCopy;
                    _utf8Json._state._tokenType = JsonTokenType.String;
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
                            _utf8Json._state._position = -1; // Should be 0, but we increment _state._position below already
                            _utf8Json._state._lineNumber++;
                        }
                        else if (currentByte == 'u')
                        {
                            _utf8Json._state._position++;
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
                                _utf8Json._state._position++;
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

                    _utf8Json._state._position++;
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
                    _utf8Json._state._position++;
                    if (ValidateEscaping_AndHex(localCopy))
                        goto Done;
                    return false;
                }

                _utf8Json._state._position = i;

            Done:
                _utf8Json._state._position++;
                Value = localCopy;
                _utf8Json._state._tokenType = JsonTokenType.String;
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
                        _utf8Json._state._lineNumber++;
                        _utf8Json._state._position = 0;
                    }
                    else
                    {
                        _utf8Json._state._position++;
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
                return Buffers.Text.Encodings.Utf8.ToString(Value);
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
}
