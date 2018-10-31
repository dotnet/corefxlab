// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using static System.Text.JsonLab.JsonThrowHelper;

namespace System.Text.JsonLab
{
    public ref partial struct JsonUtf8Reader
    {
        private bool GetNextSpan()
        {
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

        private bool ReadFirstTokenMultiSegment(byte first)
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
                    if (!TryGetNumberMultiSegment(_buffer.Slice(_consumed), out int consumed))
                        return false;
                    TokenType = JsonTokenType.Number;
                    _consumed += consumed;
                    goto Done;
                }
                else if (ConsumeValueMultiSegment(first))
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
                    SkipWhiteSpaceMultiSegment();
                    if (_consumed >= (uint)_buffer.Length)
                    {
                        return true;
                    }
                }

                if (_readerOptions._commentHandling != JsonReaderOptions.CommentHandling.Default)
                {
                    if (_readerOptions._commentHandling == JsonReaderOptions.CommentHandling.AllowComments)
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool HasMoreDataMultiSegment()
        {
            if (_consumed >= (uint)_buffer.Length)
            {
                if (!_isSingleValue && IsLastSpan)
                {
                    if (TokenType != JsonTokenType.EndArray && TokenType != JsonTokenType.EndObject)
                        ThrowJsonReaderException(ref this, ExceptionResource.InvalidEndOfJson);
                }
                if (!GetNextSpan())
                {
                    if (!_isSingleValue && IsLastSpan)
                    {
                        if (TokenType != JsonTokenType.EndArray && TokenType != JsonTokenType.EndObject)
                            ThrowJsonReaderException(ref this, ExceptionResource.InvalidEndOfJson);
                    }
                    return false;
                }
            }
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool HasMoreDataMultiSegment(ExceptionResource resource)
        {
            if (_consumed >= (uint)_buffer.Length)
            {
                if (IsLastSpan)
                {
                    ThrowJsonReaderException(ref this, resource);
                }
                if (!GetNextSpan())
                {
                    if (IsLastSpan)
                    {
                        ThrowJsonReaderException(ref this, resource);
                    }
                    return false;
                }
            }
            return true;
        }

        private bool ReadMultiSegment()
        {
            bool retVal = false;
            _isValueMultiSegment = false;

            if (!HasMoreDataMultiSegment())
                goto Done;

            byte first = _buffer[_consumed];

            if (first <= JsonConstants.Space)
            {
                SkipWhiteSpaceMultiSegment();
                if (!HasMoreDataMultiSegment())
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
                    long prevTotalConsumed = _totalConsumed;
                    int prevConsumed = _consumed;
                    long prevPosition = _position;
                    if (ConsumePropertyNameMultiSegment())
                    {
                        return true;
                    }
                    _consumed = prevConsumed;
                    TokenType = JsonTokenType.StartObject;
                    _position = prevPosition;
                    _totalConsumed = prevTotalConsumed;
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
                    return ConsumeValueMultiSegment(first);
                }
            }
            else if (TokenType == JsonTokenType.PropertyName)
            {
                return ConsumeValueMultiSegment(first);
            }
            else
            {
                long prevTotalConsumed = _totalConsumed;
                int prevConsumed = _consumed;
                long prevPosition = _position;
                long prevLineNumber = _lineNumber;
                JsonTokenType prevTokenType = TokenType;
                InternalResult result = ConsumeNextTokenMultiSegment(first);
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
                    _totalConsumed = prevTotalConsumed;
                }
                return false;
            }

            retVal = true;

        Done:
            return retVal;

        ReadFirstToken:
            retVal = ReadFirstTokenMultiSegment(first);
            goto Done;
        }

        /// <summary>
        /// This method consumes the next token regardless of whether we are inside an object or an array.
        /// For an object, it reads the next property name token. For an array, it just reads the next value.
        /// </summary>
        private InternalResult ConsumeNextTokenMultiSegment(byte marker)
        {
            if (_readerOptions._commentHandling != JsonReaderOptions.CommentHandling.Default)
            {
                //TODO: Re-evaluate use of InternalResult enum for the common case
                if (_readerOptions._commentHandling == JsonReaderOptions.CommentHandling.AllowComments)
                {
                    if (marker == JsonConstants.Solidus)
                    {
                        return ConsumeCommentMultiSegment() ? InternalResult.Success : InternalResult.FailureRollback;
                    }
                    if (TokenType == JsonTokenType.Comment)
                    {
                        TokenType = (JsonTokenType)_stack.Pop();
                        if (ReadMultiSegment())
                            return InternalResult.Success;
                        else
                        {
                            _stack.Push((byte)TokenType);
                            return InternalResult.FailureRollback;
                        }
                    }
                }
                else
                {
                    // JsonReaderOptions.SkipComments
                    if (marker == JsonConstants.Solidus)
                    {
                        if (SkipCommentMultiSegment())
                        {
                            if (!HasMoreDataMultiSegment())
                                return InternalResult.FalseNoRollback;

                            byte first = _buffer[_consumed];

                            if (first <= JsonConstants.Space)
                            {
                                SkipWhiteSpaceMultiSegment();
                                if (!HasMoreDataMultiSegment())
                                    return InternalResult.FalseNoRollback;
                                first = _buffer[_consumed];
                            }

                            return ConsumeNextTokenMultiSegment(first);
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
                    SkipWhiteSpaceMultiSegment();
                    // The next character must be a start of a property name or value.
                    if (!HasMoreDataMultiSegment(ExceptionResource.ExpectedStartOfPropertyOrValueNotFound))
                        return InternalResult.FailureRollback;
                    first = _buffer[_consumed];
                }

                TokenStartIndex = _consumed;
                if (_inObject)
                {
                    if (first != JsonConstants.Quote)
                        ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfPropertyNotFound, first);
                    TokenStartIndex++;
                    return ConsumePropertyNameMultiSegment() ? InternalResult.Success : InternalResult.FailureRollback;
                }
                else
                {
                    return ConsumeValueMultiSegment(first) ? InternalResult.Success : InternalResult.FailureRollback;
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
        private bool ConsumeValueMultiSegment(byte marker)
        {
            if (marker == JsonConstants.Quote)
            {
                TokenStartIndex++;
                return ConsumeStringMultiSegment();
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
                return ConsumeNumberMultiSegment();
            }
            else if (marker == 'f')
            {
                return ConsumeFalseMultiSegment();
            }
            else if (marker == 't')
            {
                return ConsumeTrueMultiSegment();
            }
            else if (marker == 'n')
            {
                return ConsumeNullMultiSegment();
            }
            else
            {
                if (_readerOptions._commentHandling != JsonReaderOptions.CommentHandling.Default)
                {
                    if (_readerOptions._commentHandling == JsonReaderOptions.CommentHandling.AllowComments)
                    {
                        if (marker == JsonConstants.Solidus)
                        {
                            return ConsumeCommentMultiSegment();
                        }
                    }
                    else
                    {
                        // JsonReaderOptions.SkipComments
                        if (marker == JsonConstants.Solidus)
                        {
                            if (SkipCommentMultiSegment())
                            {
                                if (_consumed >= (uint)_buffer.Length)
                                {
                                    if (!_isSingleValue && IsLastSpan)
                                    {
                                        if (TokenType != JsonTokenType.EndArray && TokenType != JsonTokenType.EndObject)
                                            ThrowJsonReaderException(ref this, ExceptionResource.InvalidEndOfJson);
                                    }
                                    if (!GetNextSpan())
                                    {
                                        return false;
                                    }
                                }

                                byte first = _buffer[_consumed];

                                if (first <= JsonConstants.Space)
                                {
                                    SkipWhiteSpaceMultiSegment();
                                    if (!HasMoreDataMultiSegment())
                                        return false;
                                    first = _buffer[_consumed];
                                }

                                return ConsumeValueMultiSegment(first);
                            }
                            return false;
                        }
                    }
                }

                ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfValueNotFound, marker);
            }
            return true;
        }

        private bool SkipCommentMultiSegment()
        {
            //Create local copy to avoid bounds checks.
            ReadOnlySpan<byte> localCopy = _buffer.Slice(_consumed + 1);

            if (localCopy.Length > 0)
            {
                byte marker = localCopy[0];
                if (marker == JsonConstants.Solidus)
                    return SkipSingleLineCommentMultiSegment(localCopy.Slice(1));
                else if (marker == '*')
                    return SkipMultiLineCommentMultiSegment(localCopy.Slice(1));
                else
                    ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfValueNotFound, marker);
            }

            if (IsLastSpan)
            {
                ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfValueNotFound, JsonConstants.Solidus);
            }
            else
            {
                if (!GetNextSpan())
                    return false;

                _totalConsumed++;
                _position++;

                byte marker = _buffer[0];
                if (marker == JsonConstants.Solidus)
                    return SkipSingleLineCommentMultiSegment(_buffer.Slice(1), leftOver: 1);
                else if (marker == '*')
                    return SkipMultiLineCommentMultiSegment(_buffer.Slice(1), leftOver: 1);
                else
                    ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfValueNotFound, marker);
            }

            return true;
        }

        private bool SkipSingleLineCommentMultiSegment(ReadOnlySpan<byte> localCopy, int leftOver = 2)
        {
            int idx;
            long prevTotalConsumed = _totalConsumed;
            do
            {
                //TODO: Match Json.NET's end of comment semantics
                idx = localCopy.IndexOf(JsonConstants.LineFeed);
                if (idx == -1)
                {
                    if (IsLastSpan)
                    {
                        // Assume everything on this line is a comment and there is no more data.
                        idx = localCopy.Length;
                        _position += 2 + localCopy.Length;
                        goto Done;
                    }
                    else
                    {
                        if (!GetNextSpan())
                        {
                            _totalConsumed = prevTotalConsumed;
                            return false;
                        }
                        _totalConsumed += localCopy.Length + leftOver;
                        leftOver = 0;

                        localCopy = _buffer;
                    }
                }
            } while (idx == -1);

            _consumed++;
            _position = 0;
            _lineNumber++;
        Done:
            _consumed += leftOver + idx;
            return true;
        }

        private bool SkipMultiLineCommentMultiSegment(ReadOnlySpan<byte> localCopy, int leftOver = 2)
        {
            long prevTotalConsumed = _totalConsumed;
            int i;
            while (true)
            {
                i = 0;
                for (; i < localCopy.Length; i++)
                {
                    byte nextByte = localCopy[i];
                    if (nextByte == '*')
                    {
                        i++;
                        if (i < localCopy.Length)
                        {
                            if (localCopy[i] == JsonConstants.Solidus)
                                goto Done;
                        }
                        else
                        {
                            if (!GetNextSpan())
                            {
                                _totalConsumed = prevTotalConsumed;
                                return false;
                            }
                            _totalConsumed += localCopy.Length + leftOver;
                            _position += localCopy.Length + leftOver;
                            leftOver = 0;
                            localCopy = _buffer;
                            i = 0;
                            if (localCopy[i] == JsonConstants.Solidus)
                                goto Done;
                            break;
                        }
                    }
                    else if (nextByte == JsonConstants.LineFeed)
                    {
                        _position = 0;
                        _lineNumber++;
                    }
                }
                if (i == localCopy.Length)
                {
                    if (!GetNextSpan())
                    {
                        _totalConsumed = prevTotalConsumed;
                        return false;
                    }
                    _totalConsumed += localCopy.Length + leftOver;
                    _position += localCopy.Length + leftOver;
                    leftOver = 0;
                    localCopy = _buffer;
                }
            }

        Done:
            _consumed += i + 1 + leftOver;
            _position += i + 1 + leftOver;
            return true;
        }

        private bool ConsumeCommentMultiSegment()
        {
            //Create local copy to avoid bounds checks.
            ReadOnlySpan<byte> localCopy = _buffer.Slice(_consumed + 1);

            SequencePosition start = new SequencePosition(_currentPosition.GetObject(), _currentPosition.GetInteger() + _consumed);
            if (localCopy.Length > 0)
            {
                byte marker = localCopy[0];
                if (marker == JsonConstants.Solidus)
                    return ConsumeSingleLineCommentMultiSegment(localCopy.Slice(1), start);
                else if (marker == '*')
                    return ConsumeMultiLineCommentMultiSegment(localCopy.Slice(1), start);
                else
                    ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfValueNotFound, marker);
            }

            if (IsLastSpan)
            {
                ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfValueNotFound, JsonConstants.Solidus);
            }
            else
            {
                if (!GetNextSpan())
                    return false;

                _totalConsumed++;
                _position++;

                byte marker = _buffer[0];
                if (marker == JsonConstants.Solidus)
                    return ConsumeSingleLineCommentMultiSegment(_buffer.Slice(1), start, leftOver: 1);
                else if (marker == '*')
                    return ConsumeMultiLineCommentMultiSegment(_buffer.Slice(1), start, leftOver: 1);
                else
                    ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfValueNotFound, marker);
            }

            return true;
        }

        private bool ConsumeSingleLineCommentMultiSegment(ReadOnlySpan<byte> localCopy, SequencePosition start, int leftOver = 2)
        {
            int idx;
            long prevTotalConsumed = _totalConsumed;
            do
            {
                //TODO: Match Json.NET's end of comment semantics
                idx = localCopy.IndexOf(JsonConstants.LineFeed);
                if (idx == -1)
                {
                    if (IsLastSpan)
                    {
                        // Assume everything on this line is a comment and there is no more data.
                        idx = localCopy.Length;
                        _position += 2 + localCopy.Length;
                        goto Done;
                    }
                    else
                    {
                        if (!GetNextSpan())
                        {
                            _totalConsumed = prevTotalConsumed;
                            return false;
                        }
                        _isValueMultiSegment = true;
                        _totalConsumed += localCopy.Length + leftOver;
                        leftOver = 0;

                        localCopy = _buffer;
                    }
                }
            } while (idx == -1);

            _consumed++;
            _position = 0;
            _lineNumber++;

        Done:
            if (_isValueMultiSegment)
            {
                SequencePosition end = new SequencePosition(_currentPosition.GetObject(), _currentPosition.GetInteger() + idx);
                ReadOnlySequence<byte> commentSequence = _data.Slice(start, end).Slice(2);  // Slice off '//'
                if (!IsLastSpan)
                    commentSequence = commentSequence.Slice(0, commentSequence.Length - 1); // Slice off new line
                if (commentSequence.IsSingleSegment)
                {
                    ValueSpan = commentSequence.First.Span;
                    _isValueMultiSegment = false;
                }
                else
                {
                    ValueSequence = commentSequence;
                }
            }
            else
            {
                ValueSpan = localCopy.Slice(0, idx);
            }
            _stack.Push((byte)TokenType);
            TokenType = JsonTokenType.Comment;
            _consumed += leftOver + idx;
            return true;
        }

        private bool ConsumeMultiLineCommentMultiSegment(ReadOnlySpan<byte> localCopy, SequencePosition start, int leftOver = 2)
        {
            long prevTotalConsumed = _totalConsumed;
            int i;
            int lastLineFeedIndex;
            while (true)
            {
                i = 0;
                lastLineFeedIndex = -1;
                for (; i < localCopy.Length; i++)
                {
                    byte nextByte = localCopy[i];
                    if (nextByte == '*')
                    {
                        i++;
                        if (i < localCopy.Length)
                        {
                            if (localCopy[i] == JsonConstants.Solidus)
                                goto Done;
                        }
                        else
                        {
                            if (!GetNextSpan())
                            {
                                _totalConsumed = prevTotalConsumed;
                                return false;
                            }
                            _isValueMultiSegment = true;
                            _totalConsumed += localCopy.Length + leftOver;

                            if (lastLineFeedIndex == -1)
                            {
                                _position += localCopy.Length + leftOver;
                            }
                            else
                            {
                                _position += i - lastLineFeedIndex - 1;
                            }
                            lastLineFeedIndex = -1;
                            leftOver = 0;
                            localCopy = _buffer;
                            i = 0;
                            if (localCopy[i] == JsonConstants.Solidus)
                                goto Done;
                            break;
                        }
                    }
                    else if (nextByte == JsonConstants.LineFeed)
                    {
                        lastLineFeedIndex = i;
                        _position = 0;
                        _lineNumber++;
                    }
                }
                if (i == localCopy.Length)
                {
                    if (!GetNextSpan())
                    {
                        _totalConsumed = prevTotalConsumed;
                        return false;
                    }
                    _isValueMultiSegment = true;
                    _totalConsumed += localCopy.Length + leftOver;
                    if (lastLineFeedIndex == -1)
                    {
                        _position += localCopy.Length + leftOver;
                    }
                    else
                    {
                        _position += i - lastLineFeedIndex - 1;
                    }
                    lastLineFeedIndex = -1;
                    leftOver = 0;
                    localCopy = _buffer;
                }
            }

        Done:
            if (_isValueMultiSegment)
            {
                SequencePosition end = new SequencePosition(_currentPosition.GetObject(), _currentPosition.GetInteger() + i);
                ReadOnlySequence<byte> commentSequence = _data.Slice(start, end).Slice(2);    // Slice off '/*'
                commentSequence = commentSequence.Slice(0, commentSequence.Length - 1);  // Slice off '*' before final '/'
                if (commentSequence.IsSingleSegment)
                {
                    ValueSpan = commentSequence.First.Span;
                    _isValueMultiSegment = false;
                }
                else
                {
                    ValueSequence = commentSequence;
                }
            }
            else
            {
                ValueSpan = i <= 1 ? default : localCopy.Slice(0, i - 1);
            }

            _stack.Push((byte)TokenType);
            TokenType = JsonTokenType.Comment;
            _consumed += i + 1 + leftOver;
            if (lastLineFeedIndex == -1)
            {
                _position += i + 1 + leftOver;
            }
            else
            {
                _position += i - lastLineFeedIndex;
            }
            return true;
        }

        private bool ConsumeNumberMultiSegment()
        {
            if (!TryGetNumberMultiSegment(_buffer.Slice(_consumed), out int consumed))
                return false;

            TokenType = JsonTokenType.Number;
            _consumed += consumed;

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

        private int FindMismatch(ReadOnlySpan<byte> span, ReadOnlySpan<byte> literal)
        {
            Debug.Assert(span.Length > 0 && span[0] == literal[0]);

            int indexOfFirstMismatch = 0;

            for (int i = 1; i < literal.Length; i++)
            {
                if (span.Length > i)
                {
                    if (span[i] != literal[i])
                    {
                        indexOfFirstMismatch = i;
                        break;
                    }
                }
                else
                {
                    indexOfFirstMismatch = i;
                    break;
                }
            }

            Debug.Assert(indexOfFirstMismatch > 0 && indexOfFirstMismatch < literal.Length);

            return indexOfFirstMismatch;
        }

        private bool ConsumeLiteralMultiSegment(ReadOnlySpan<byte> span, ReadOnlySpan<byte> literalValue, ExceptionResource resource, out int consumed)
        {
            long prevTotalConsumed = _totalConsumed;
            if (span.Length >= literalValue.Length || IsLastSpan)
            {
                _position += FindMismatch(span, literalValue);
                goto Throw;
            }
            else
            {
                if (!literalValue.StartsWith(span))
                {
                    _position += FindMismatch(span, literalValue);
                    goto Throw;
                }

                ReadOnlySpan<byte> leftToMatch = literalValue.Slice(span.Length);

                SequencePosition startPosition = _currentPosition;
                int startConsumed = _consumed;
                int alreadyMatched = literalValue.Length - leftToMatch.Length;
                while (true)
                {
                    _totalConsumed += alreadyMatched;
                    _position += alreadyMatched;
                    if (!GetNextSpan())
                    {
                        _totalConsumed = prevTotalConsumed;
                        consumed = default;
                        if (IsLastSpan)
                            goto Throw;
                        return false;
                    }

                    span = _buffer;
                    if (span.StartsWith(leftToMatch))
                    {
                        _isValueMultiSegment = true;
                        SequencePosition start = new SequencePosition(startPosition.GetObject(), startPosition.GetInteger() + startConsumed);
                        SequencePosition end = new SequencePosition(_currentPosition.GetObject(), _currentPosition.GetInteger() + leftToMatch.Length);
                        ValueSequence = _data.Slice(start, end);
                        consumed = leftToMatch.Length;
                        return true;
                    }

                    if (!leftToMatch.StartsWith(span))
                    {
                        _position += FindMismatch(span, leftToMatch);
                        goto Throw;
                    }

                    leftToMatch = leftToMatch.Slice(span.Length);
                    alreadyMatched = span.Length;
                }
            }
        Throw:
            _totalConsumed = prevTotalConsumed;
            consumed = default;
            ThrowJsonReaderException(ref this, resource, bytes: span);
            return false;
        }

        private bool ConsumeNullMultiSegment()
        {
            ReadOnlySpan<byte> span = _buffer.Slice(_consumed);

            Debug.Assert(span.Length > 0 && span[0] == JsonConstants.NullValue[0]);

            int consumed = 4;

            if (!span.StartsWith(JsonConstants.NullValue))
            {
                if (ConsumeLiteralMultiSegment(span, JsonConstants.NullValue, ExceptionResource.ExpectedNull, out consumed))
                {
                    goto Done;
                }
                return false;
            }

            ValueSpan = span.Slice(0, 4);
            _isValueMultiSegment = false;
        Done:
            TokenType = JsonTokenType.Null;
            _consumed += consumed;
            _position += consumed;
            return true;
        }

        private bool ConsumeFalseMultiSegment()
        {
            ReadOnlySpan<byte> span = _buffer.Slice(_consumed);

            Debug.Assert(span.Length > 0 && span[0] == JsonConstants.FalseValue[0]);

            int consumed = 5;

            if (!span.StartsWith(JsonConstants.FalseValue))
            {
                if (ConsumeLiteralMultiSegment(span, JsonConstants.FalseValue, ExceptionResource.ExpectedFalse, out consumed))
                {
                    goto Done;
                }
                return false;
            }

            ValueSpan = span.Slice(0, 5);
            _isValueMultiSegment = false;
        Done:
            TokenType = JsonTokenType.False;
            _consumed += consumed;
            _position += consumed;
            return true;
        }

        private bool ConsumeTrueMultiSegment()
        {
            ReadOnlySpan<byte> span = _buffer.Slice(_consumed);

            Debug.Assert(span.Length > 0 && span[0] == JsonConstants.TrueValue[0]);

            int consumed = 4;

            if (!span.StartsWith(JsonConstants.TrueValue))
            {
                if (ConsumeLiteralMultiSegment(span, JsonConstants.TrueValue, ExceptionResource.ExpectedTrue, out consumed))
                {
                    goto Done;
                }
                return false;
            }

            ValueSpan = span.Slice(0, 4);
            _isValueMultiSegment = false;
        Done:
            TokenType = JsonTokenType.True;
            _consumed += consumed;
            _position += consumed;
            return true;
        }

        private bool ConsumePropertyNameMultiSegment()
        {
            if (!ConsumeStringMultiSegment())
                return false;

            if (!HasMoreDataMultiSegment(ExceptionResource.ExpectedValueAfterPropertyNameNotFound))
                return false;

            byte first = _buffer[_consumed];

            if (first <= JsonConstants.Space)
            {
                SkipWhiteSpaceMultiSegment();
                if (!HasMoreDataMultiSegment(ExceptionResource.ExpectedValueAfterPropertyNameNotFound))
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

        private bool ConsumeStringMultiSegment()
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
                    // No quote found, count how many backslashes exist at the end of the buffer.
                    int counter = 0;
                    for (int j = localCopy.Length - 1; j >= _consumed + 1; j--)
                    {
                        if (_buffer[j] == JsonConstants.ReverseSolidus)
                            counter++;
                        else
                            break;
                    }
                    return ConsumeStringNextSegment(counter);
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
                ValueSpan = localCopy;
                _isValueMultiSegment = false;
                TokenType = JsonTokenType.String;
                _consumed += idx + 2;
                return true;
            }
            else
            {
                return ConsumeStringWithNestedQuotesMultiSegment();
            }
        }

        private bool ConsumeStringNextSegment(int counter = 0)
        {
            SequencePosition startPosition = _currentPosition;
            SequencePosition end = default;
            int startConsumed = _consumed + 1;
            _isValueMultiSegment = true;
            int leftOver = _buffer.Length - _consumed;

            long prevTotalConsumed = _totalConsumed;
            long prevPosition = _position;

            //TODO: Worth vectorizing?
            while (true)
            {
                if (!GetNextSpan())
                {
                    _totalConsumed = prevTotalConsumed;
                    _position = prevPosition;
                    if (IsLastSpan)
                        ThrowJsonReaderException(ref this, ExceptionResource.EndOfStringNotFound);
                    return false;
                }

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
                                {
                                    _totalConsumed += leftOver;
                                    _consumed = i + 1;
                                    goto Done;
                                }
                                _totalConsumed += i + 1;
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

                _totalConsumed += i;
            }

        Done:
            SequencePosition start = new SequencePosition(startPosition.GetObject(), startPosition.GetInteger() + startConsumed);
            ValueSequence = _data.Slice(start, end);
            if (leftOver > 0)
                _position++;    // For first quote of the string
            ValidateEscaping_AndHex(ValueSequence);  //TODO: Can this be done while searching for end quote?
            _position += 1;     // For second quote of the string
            TokenType = JsonTokenType.String;
            return true;
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

        private bool ConsumeStringWithNestedQuotesMultiSegment()
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
                        return ConsumeStringNextSegment(counter);
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
            ValueSpan = localCopy;
            _isValueMultiSegment = false;
            TokenType = JsonTokenType.String;
            _consumed = i + 1;
            return true;
        }

        private void SkipWhiteSpaceMultiSegment()
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
                SkipWhiteSpaceMultiSegment();
            }
        }

        private InternalNumberResult ConsumeNegativeSignMultiSegment(ref ReadOnlySpan<byte> data, ref int i)
        {
            byte nextByte = data[i];

            if (nextByte == '-')
            {
                i++;
                _position++;
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
                        _totalConsumed++;
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

        private InternalNumberResult ConsumeZeroMultiSegment(ref ReadOnlySpan<byte> data, ref int i, ReadOnlySpan<byte> delimiters)
        {
            i++;
            _position++;
            byte nextByte = default;
            if (i < data.Length)
            {
                nextByte = data[i];
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
                    _totalConsumed++;
                    _isValueMultiSegment = true;    //TODO: Can this be single segment instead if '0' is the last byte of a segment
                    i = 0;
                    data = _buffer;
                    nextByte = data[i];
                    if (delimiters.IndexOf(nextByte) >= 0)
                        return InternalNumberResult.Success;
                }
            }

            if (nextByte != '.' && nextByte != 'E' && nextByte != 'e')
                ThrowJsonReaderException(ref this, ExceptionResource.ExpectedNextDigitComponentNotFound, nextByte);

            return InternalNumberResult.OperationIncomplete;
        }

        private InternalNumberResult ConsumeIntegerDigitsMultiSegment(ref ReadOnlySpan<byte> data, ref int i, ReadOnlySpan<byte> delimiters)
        {
            byte nextByte = default;
            int counter = 0;
            for (; i < data.Length; i++)
            {
                nextByte = data[i];
                if ((uint)(nextByte - '0') > '9' - '0')
                    break;
                counter++;
            }
            if (i >= data.Length)
            {
                if (IsLastSpan)
                {
                    _position += counter;
                    return InternalNumberResult.Success;
                }
                else
                {
                    while (true)
                    {
                        if (!GetNextSpan())
                        {
                            return InternalNumberResult.NeedMoreData;
                        }

                        _totalConsumed += i;
                        _position += counter;
                        counter = 0;
                        _isValueMultiSegment = true;
                        i = 0;
                        data = _buffer;
                        for (; i < data.Length; i++)
                        {
                            nextByte = data[i];
                            if ((uint)(nextByte - '0') > '9' - '0')
                                break;
                        }
                        if (i >= data.Length)
                        {
                            if (IsLastSpan)
                            {
                                _position += i;
                                return InternalNumberResult.Success;
                            }
                        }
                        else
                        {
                            _position += i;
                            break;
                        }
                    }
                }
            }
            else
            {
                _position += counter;
            }
            if (delimiters.IndexOf(nextByte) >= 0)
                return InternalNumberResult.Success;

            return InternalNumberResult.OperationIncomplete;
        }

        private InternalNumberResult ConsumeDecimalDigitsMultiSegment(ref ReadOnlySpan<byte> data, ref int i, ReadOnlySpan<byte> delimiters, byte nextByte)
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
                    _totalConsumed += i;
                    _isValueMultiSegment = true;
                    i = 0;
                    data = _buffer;
                }
            }
            nextByte = data[i];
            if ((uint)(nextByte - '0') > '9' - '0')
                ThrowJsonReaderException(ref this, ExceptionResource.ExpectedDigitNotFound, nextByte);
            i++;
            _position++;
            return ConsumeIntegerDigitsMultiSegment(ref data, ref i, delimiters);
        }


        private InternalNumberResult ConsumeSignMultiSegment(ref ReadOnlySpan<byte> data, ref int i, byte nextByte)
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
                _position++;
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
                        _totalConsumed++;
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
        private bool TryGetNumberMultiSegment(ReadOnlySpan<byte> data, out int consumed)
        {
            Debug.Assert(data.Length > 0);

            ReadOnlySpan<byte> delimiters = JsonConstants.Delimiters;
            SequencePosition startPosition = _currentPosition;
            int startConsumed = _consumed;
            consumed = 0;
            long prevTotalConsumed = _totalConsumed;
            long prevPosition = _position;

            int i = 0;

            InternalNumberResult signResult = ConsumeNegativeSignMultiSegment(ref data, ref i);
            if (signResult == InternalNumberResult.NeedMoreData)
            {
                _totalConsumed = prevTotalConsumed;
                _position = prevPosition;
                return false;
            }

            Debug.Assert(signResult == InternalNumberResult.OperationIncomplete);

            byte nextByte = data[i];
            Debug.Assert(nextByte >= '0' && nextByte <= '9');

            if (nextByte == '0')
            {
                InternalNumberResult result = ConsumeZeroMultiSegment(ref data, ref i, delimiters);
                if (result == InternalNumberResult.NeedMoreData)
                {
                    _totalConsumed = prevTotalConsumed;
                    _position = prevPosition;
                    return false;
                }
                if (result == InternalNumberResult.Success)
                    goto Done;

                nextByte = data[i];
            }
            else
            {
                Debug.Assert(nextByte >= '1' && nextByte <= '9');

                InternalNumberResult result = ConsumeIntegerDigitsMultiSegment(ref data, ref i, delimiters);
                if (result == InternalNumberResult.NeedMoreData)
                {
                    _totalConsumed = prevTotalConsumed;
                    _position = prevPosition;
                    return false;
                }
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
                _position++;
                InternalNumberResult result = ConsumeDecimalDigitsMultiSegment(ref data, ref i, delimiters, nextByte);
                if (result == InternalNumberResult.NeedMoreData)
                {
                    _totalConsumed = prevTotalConsumed;
                    _position = prevPosition;
                    return false;
                }
                if (result == InternalNumberResult.Success)
                    goto Done;

                nextByte = data[i];
                if (nextByte != 'E' && nextByte != 'e')
                    ThrowJsonReaderException(ref this, ExceptionResource.ExpectedNextDigitEValueNotFound, nextByte);
            }

            Debug.Assert(nextByte == 'E' || nextByte == 'e');
            i++;
            _position++;

            signResult = ConsumeSignMultiSegment(ref data, ref i, nextByte);
            if (signResult == InternalNumberResult.NeedMoreData)
            {
                _totalConsumed = prevTotalConsumed;
                _position = prevPosition;
                return false;
            }

            Debug.Assert(signResult == InternalNumberResult.OperationIncomplete);

            i++;
            _position++;
            InternalNumberResult resultExponent = ConsumeIntegerDigitsMultiSegment(ref data, ref i, delimiters);
            if (resultExponent == InternalNumberResult.NeedMoreData)
            {
                _totalConsumed = prevTotalConsumed;
                _position = prevPosition;
                return false;
            }
            if (resultExponent == InternalNumberResult.Success)
                goto Done;
            if (resultExponent == InternalNumberResult.OperationIncomplete)
                ThrowJsonReaderException(ref this, ExceptionResource.ExpectedEndOfDigitNotFound, nextByte);

            Done:
            if (_isValueMultiSegment)
            {
                SequencePosition start = new SequencePosition(startPosition.GetObject(), startPosition.GetInteger() + startConsumed);
                SequencePosition end = new SequencePosition(_currentPosition.GetObject(), _currentPosition.GetInteger() + i);
                ValueSequence = _data.Slice(start, end);
                consumed = i;
            }
            else
            {
                ValueSpan = data.Slice(0, i);
                _isValueMultiSegment = false;
                consumed = i;
            }
            return true;
        }
    }
}
