// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//TODO: Add multi-segment support for tracking line number/position

using System.Buffers;
using System.Buffers.Reader;
using System.Diagnostics;
using static System.Text.JsonLab.JsonThrowHelper;

namespace System.Text.JsonLab
{
    public ref partial struct Utf8JsonReader
    {
        public Utf8JsonReader(in ReadOnlySequence<byte> data)
        {
            _containerMask = 0;
            Depth = 0;
            _inObject = false;
            _stack = null;
            TokenType = JsonTokenType.None;
            _lineNumber = 1;
            _position = 0;

            _isFinalBlock = true;

            _reader = new BufferReader<byte>(data);
            _isSingleSegment = data.IsSingleSegment; //true;
            _buffer = _reader.CurrentSpan;  //data.ToArray();
            CurrentIndex = 0;
            TokenStartIndex = CurrentIndex;
            _maxDepth = StackFreeMaxDepth;
            Value = ReadOnlySpan<byte>.Empty;
            ValueType = JsonValueType.Unknown;
            _isSingleValue = true;
            _allowComments = false;
        }

        public Utf8JsonReader(in ReadOnlySequence<byte> data, bool isFinalBlock, JsonReaderState state = default)
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
            }

            _isFinalBlock = isFinalBlock;

            _reader = new BufferReader<byte>(data);
            _isSingleSegment = data.IsSingleSegment; //true;
            _buffer = _reader.CurrentSpan;  //data.ToArray();
            CurrentIndex = 0;
            TokenStartIndex = CurrentIndex;
            _maxDepth = StackFreeMaxDepth;
            Value = ReadOnlySpan<byte>.Empty;
            ValueType = JsonValueType.Unknown;
            _isSingleValue = true;
            _allowComments = false;
        }

        private bool ReadFirstToken(ref BufferReader<byte> reader, byte first)
        {
            if (first == JsonConstants.OpenBrace)
            {
                Depth++;
                _containerMask = 1;
                TokenType = JsonTokenType.StartObject;
                ValueType = JsonValueType.Object;
                reader.Advance(1);
                CurrentIndex++;
                _position++;
                _inObject = true;
                _isSingleValue = false;
            }
            else if (first == JsonConstants.OpenBracket)
            {
                Depth++;
                TokenType = JsonTokenType.StartArray;
                ValueType = JsonValueType.Array;
                reader.Advance(1);
                CurrentIndex++;
                _position++;
                _isSingleValue = false;
            }
            else
            {
                // Cannot call ConsumeNumber since it looks for the delimiter and we won't find it.
                if ((uint)(first - '0') <= '9' - '0' || first == '-')
                {
                    if (first == '-')
                    {
                        // Couldn't find anything beyond '-', can't advance past it since we need '-' to be part of the value
                        // TODO: This could silently allocate. Try to avoid it.
                        if (reader.Peek(2).Length == 1)
                        {
                            if (_isFinalBlock)
                            {
                                ThrowJsonReaderException(ref this);
                            }
                            else return false;
                        }
                    }
                    ReadOnlySequence<byte> sequence = reader.Sequence.Slice(reader.Position);
                    TryGetNumber(sequence.IsSingleSegment ? sequence.First.Span : sequence.ToArray(), out ReadOnlySpan<byte> number);
                    if (_isFinalBlock)
                    {
                        Value = number;
                        ValueType = JsonValueType.Number;
                        reader.Advance(Value.Length);
                        CurrentIndex += Value.Length;
                        _position += Value.Length;
                        return true;
                    }
                    else return false;
                }
                else if (ConsumeValue(ref reader, first))
                {
                    if (!reader.TryPeek(out first))
                    {
                        return true;
                    }
                    if (first <= JsonConstants.Space)
                    {
                        SkipWhiteSpace(ref reader);
                        if (reader.End)
                        {
                            return true;
                        }
                    }

                    if (AllowComments)
                    {
                        reader.TryPeek(out first);
                        if (TokenType == JsonTokenType.Comment || first == JsonConstants.Solidus)
                        {
                            return true;
                        }
                    }

                    ThrowJsonReaderException(ref this);
                }
                return false;
            }
            return true;
        }

        private bool ReadMultiSegment(ref BufferReader<byte> reader)
        {
            bool retVal = false;

            if (!reader.TryPeek(out byte first))
            {
                if (!_isSingleValue && _isFinalBlock)
                {
                    if (TokenType != JsonTokenType.EndArray && TokenType != JsonTokenType.EndObject)
                        ThrowJsonReaderException(ref this, ExceptionResource.InvalidEndOfJson);
                }
                goto Done;
            }

            if (first <= JsonConstants.Space)
            {
                SkipWhiteSpace(ref reader);
                if (!reader.TryPeek(out first))
                {
                    if (!_isSingleValue && _isFinalBlock)
                    {
                        if (TokenType != JsonTokenType.EndArray && TokenType != JsonTokenType.EndObject)
                            ThrowJsonReaderException(ref this, ExceptionResource.InvalidEndOfJson);
                    }
                    goto Done;
                }
            }

            TokenStartIndex = CurrentIndex;

            if (TokenType == JsonTokenType.None)
            {
                goto ReadFirstToken;
            }

            if (TokenType == JsonTokenType.StartObject)
            {
                if (first == JsonConstants.CloseBrace)
                {
                    reader.Advance(1);
                    CurrentIndex++;
                    _position++;
                    EndObject();
                }
                else
                {
                    if (first != JsonConstants.Quote)
                        ThrowJsonReaderException(ref this);
                    TokenStartIndex++;
                    int prevCurrentIndex = CurrentIndex;
                    int prevPosition = _position;
                    reader.Advance(1);
                    CurrentIndex++;
                    if (ConsumePropertyName(ref reader))
                    {
                        return true;
                    }
                    CurrentIndex = prevCurrentIndex;
                    TokenType = JsonTokenType.StartObject;
                    _position = prevPosition;
                    reader.Rewind(1);
                    return false;
                }
            }
            else if (TokenType == JsonTokenType.StartArray)
            {
                if (first == JsonConstants.CloseBracket)
                {
                    reader.Advance(1);
                    CurrentIndex++;
                    _position++;
                    EndArray();
                }
                else
                {
                    return ConsumeValue(ref reader, first);
                }
            }
            else if (TokenType == JsonTokenType.PropertyName)
            {
                return ConsumeValue(ref reader, first);
            }
            else
            {
                int prevCurrentIndex = CurrentIndex;
                int prevPosition = _position;
                JsonTokenType prevTokenType = TokenType;
                if (ConsumeNextToken(ref reader, first))
                {
                    return true;
                }
                reader.Rewind(CurrentIndex - prevCurrentIndex);
                CurrentIndex = prevCurrentIndex;
                TokenType = prevTokenType;
                _position = prevPosition;
                return false;
            }

            retVal = true;

        Done:
            return retVal;

        ReadFirstToken:
            retVal = ReadFirstToken(ref reader, first);
            goto Done;
        }

        private bool ConsumeNextToken(ref BufferReader<byte> reader, byte marker)
        {
            reader.Advance(1);
            CurrentIndex++;
            _position++;

            if (AllowComments)
            {
                if (marker == JsonConstants.Solidus)
                {
                    CurrentIndex--;
                    _position--;
                    reader.Rewind(1);
                    return ConsumeComment(ref reader);
                }
                if (TokenType == JsonTokenType.Comment)
                {
                    CurrentIndex--;
                    _position--;
                    reader.Rewind(1);
                    TokenType = (JsonTokenType)_stack.Pop();
                    return ReadMultiSegment(ref reader);
                }
            }

            if (marker == JsonConstants.ListSeperator)
            {
                if (!reader.TryPeek(out byte first))
                {
                    if (_isFinalBlock)
                        ThrowJsonReaderException(ref this);
                    else return false;
                }

                if (first <= JsonConstants.Space)
                {
                    SkipWhiteSpace(ref reader);
                    // The next character must be a start of a property name or value.
                    if (!reader.TryPeek(out first))
                    {
                        if (_isFinalBlock)
                            ThrowJsonReaderException(ref this);
                        else return false;
                    }
                }

                TokenStartIndex = CurrentIndex;
                if (_inObject)
                {
                    if (first != JsonConstants.Quote)
                        ThrowJsonReaderException(ref this);

                    reader.Advance(1);
                    CurrentIndex++;
                    TokenStartIndex++;
                    return ConsumePropertyName(ref reader);
                }
                else
                {
                    return ConsumeValue(ref reader, first);
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
                CurrentIndex--;
                _position--;
                ThrowJsonReaderException(ref this);
            }
            return true;
        }

        private bool ConsumeValue(ref BufferReader<byte> reader, byte marker)
        {
            if (marker == JsonConstants.Quote)
            {
                int prevCurrentIndex = CurrentIndex;
                int prevPosition = _position;
                reader.Advance(1);
                CurrentIndex++;
                TokenStartIndex++;
                if (!ConsumeString(ref reader))
                {
                    CurrentIndex = prevCurrentIndex;
                    _position = prevPosition;
                    reader.Rewind(1);
                    return false;
                }
                return true;
            }
            else if (marker == JsonConstants.OpenBrace)
            {
                reader.Advance(1);
                CurrentIndex++;
                StartObject();
                ValueType = JsonValueType.Object;
            }
            else if (marker == JsonConstants.OpenBracket)
            {
                reader.Advance(1);
                CurrentIndex++;
                StartArray();
                ValueType = JsonValueType.Array;
            }
            else if ((uint)(marker - '0') <= '9' - '0')
            {
                return ConsumeNumber(ref reader);
            }
            else if (marker == '-')
            {
                // Couldn't find anything beyond '-', can't advance past it since we need '-' to be part of the value
                // TODO: This could silently allocate. Try to avoid it.
                if (reader.Peek(2).Length == 1)
                {
                    if (_isFinalBlock)
                    {
                        ThrowJsonReaderException(ref this);
                    }
                    else return false;
                }
                return ConsumeNumber(ref reader);
            }
            else if (marker == 'f')
            {
                return ConsumeFalse(ref reader);
            }
            else if (marker == 't')
            {
                return ConsumeTrue(ref reader);
            }
            else if (marker == 'n')
            {
                return ConsumeNull(ref reader);
            }
            else
            {
                if (AllowComments && marker == JsonConstants.Solidus)
                    return ConsumeComment(ref reader);

                ThrowJsonReaderException(ref this);
            }
            return true;
        }

        private bool ConsumeComment(ref BufferReader<byte> reader)
        {
            //TODO: Re-evaluate recovery mechanism if this fails. Do we need to rewind?
            reader.Advance(1);
            if (reader.TryPeek(out byte marker))
            {
                if (marker == JsonConstants.Solidus)
                {
                    reader.Advance(1);
                    return ConsumeSingleLineComment(ref reader);
                }
                else if (marker == '*')
                {
                    reader.Advance(1);
                    return ConsumeMultiLineComment(ref reader);
                }
                else
                    ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfValueNotFound, marker);
            }

            if (_isFinalBlock)
            {
                ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfValueNotFound, JsonConstants.Solidus);
            }
            else return false;

            return true;
        }

        private bool ConsumeSingleLineComment(ref BufferReader<byte> reader)
        {
            if (!reader.TryReadTo(out ReadOnlySpan<byte> span, JsonConstants.LineFeed, advancePastDelimiter: true))
            {
                if (_isFinalBlock)
                {
                    // Assume everything on this line is a comment and there is no more data.
                    ReadOnlySequence<byte> sequence = reader.Sequence.Slice(reader.Position);
                    Value = sequence.IsSingleSegment ? sequence.First.Span : sequence.ToArray();
                    _position += 2 + Value.Length;
                    reader.Advance(Value.Length);
                    goto Done;
                }
                else return false;
            }

            Value = span;
            CurrentIndex++;
            _position = 0;
            _lineNumber++;
        Done:
            _stack.Push((InternalJsonTokenType)TokenType);
            TokenType = JsonTokenType.Comment;
            CurrentIndex += 2 + Value.Length;
            return true;
        }

        // TODO: Fix allocations
        private bool ConsumeMultiLineComment(ref BufferReader<byte> reader)
        {
            long consumedBefore = reader.Consumed;

            byte[] partialComment = null;
            ReadOnlySpan<byte> span = default;
            while (true)
            {
                if (reader.TryReadTo(out span, JsonConstants.Solidus, advancePastDelimiter: true))
                {
                    if (span.Length > 0 && span[span.Length - 1] == '*')
                    {
                        if (partialComment != null)
                        {
                            if (partialComment.Length == 0)
                                partialComment = new byte[] { JsonConstants.Solidus };
                            byte[] temp = span.Slice(0, span.Length - 1).ToArray();
                            var newArray = new byte[partialComment.Length + temp.Length];
                            Array.Copy(partialComment, newArray, partialComment.Length);
                            Array.Copy(temp, 0, newArray, partialComment.Length, temp.Length);
                            partialComment = newArray;
                        }
                        break;
                    }

                    if (partialComment == null)
                        partialComment = span.ToArray();
                    else
                    {
                        byte[] temp = span.ToArray();
                        var newArray = new byte[partialComment.Length + temp.Length];
                        Array.Copy(partialComment, newArray, partialComment.Length);
                        Array.Copy(temp, 0, newArray, partialComment.Length, temp.Length);
                        partialComment = newArray;
                    }
                }
                else
                {
                    if (_isFinalBlock)
                    {
                        ThrowJsonReaderException(ref this, ExceptionResource.EndOfCommentNotFound);
                    }
                    else return false;
                }
            }

            _stack.Push((InternalJsonTokenType)TokenType);
            Value = partialComment == null ? span.Slice(0, span.Length - 1) : partialComment;
            TokenType = JsonTokenType.Comment;
            CurrentIndex += 4 + Value.Length;

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

        private bool ConsumeNumber(ref BufferReader<byte> reader)
        {
            long consumedBefore = reader.Consumed;
            if (!reader.TryReadToAny(out ReadOnlySpan<byte> span, JsonConstants.Delimiters, advancePastDelimiter: false))
            {
                if (_isFinalBlock)
                {
                    ThrowJsonReaderException(ref this);
                }
                else return false;
            }
            if (!TryGetNumber(span, out _))
                return false;
            CurrentIndex += (int)(reader.Consumed - consumedBefore);
            _position += (int)(reader.Consumed - consumedBefore);
            Value = span;
            ValueType = JsonValueType.Number;
            TokenType = JsonTokenType.Value;
            return true;
        }

        private bool ConsumeNull(ref BufferReader<byte> reader)
        {
            Value = JsonConstants.NullValue;
            ValueType = JsonValueType.Null;

            Debug.Assert(reader.TryPeek(out byte first) && first == Value[0]);

            if (!reader.IsNext(JsonConstants.NullValue, advancePast: true))
            {
                if (_isFinalBlock)
                {
                    goto Throw;
                }
                else
                {
                    // TODO: Throw if next two bytes are not 'u' and 'l'.
                    // We know first byte is 'n', and if fourth byte was 'l', we wouldn't be here.
                    return false;
                }
            Throw:
                ThrowJsonReaderException(ref this);
            }
            TokenType = JsonTokenType.Value;
            CurrentIndex += 4;
            _position += 4;
            return true;
        }

        private bool ConsumeFalse(ref BufferReader<byte> reader)
        {
            Value = JsonConstants.FalseValue;
            ValueType = JsonValueType.False;

            Debug.Assert(reader.TryPeek(out byte first) && first == Value[0]);

            if (!reader.IsNext(JsonConstants.FalseValue, advancePast: true))
            {
                if (_isFinalBlock)
                {
                    goto Throw;
                }
                else
                {
                    // TODO: Throw if next three bytes are not 'a', 'l', and 's'.
                    // We know first byte is 'f', and if fifth byte was 'e', we wouldn't be here.
                    return false;
                }
            Throw:
                ThrowJsonReaderException(ref this);
            }
            TokenType = JsonTokenType.Value;
            CurrentIndex += 5;
            _position += 5;
            return true;
        }

        private bool ConsumeTrue(ref BufferReader<byte> reader)
        {
            Value = JsonConstants.TrueValue;
            ValueType = JsonValueType.True;

            Debug.Assert(reader.TryPeek(out byte first) && first == Value[0]);

            if (!reader.IsNext(JsonConstants.TrueValue, advancePast: true))
            {
                if (_isFinalBlock)
                {
                    goto Throw;
                }
                else
                {
                    // TODO: Throw if next three bytes are not 'r' and 'u'.
                    // We know first byte is 't', and if fourth byte was 'e', we wouldn't be here.
                    return false;
                }
            Throw:
                ThrowJsonReaderException(ref this);
            }
            TokenType = JsonTokenType.Value;
            CurrentIndex += 4;
            _position += 4;
            return true;
        }

        private bool ConsumePropertyName(ref BufferReader<byte> reader)
        {
            if (!ConsumeString(ref reader))
                return false;

            if (!reader.TryPeek(out byte first))
            {
                if (_isFinalBlock)
                {

                    ThrowJsonReaderException(ref this);
                }
                else return false;
            }

            if (first <= JsonConstants.Space)
            {
                SkipWhiteSpace(ref reader);
                if (!reader.TryPeek(out first))
                {
                    if (_isFinalBlock)
                    {

                        ThrowJsonReaderException(ref this);
                    }
                    else return false;
                }
            }

            // The next character must be a key / value seperator. Validate and skip.
            if (first != JsonConstants.KeyValueSeperator)
            {
                ThrowJsonReaderException(ref this);
            }

            TokenType = JsonTokenType.PropertyName;
            reader.Advance(1);
            CurrentIndex++;
            _position++;
            return true;
        }

        private bool ConsumeString(ref BufferReader<byte> reader)
        {
            long consumedBefore = reader.Consumed;

            if (reader.TryReadTo(out ReadOnlySpan<byte> value, JsonConstants.Quote, (byte)'\\', advancePastDelimiter: true))
            {
                if (value.IndexOfAnyControlOrEscape() != -1)
                {
                    _position++;
                    if (ValidateEscaping_AndHex(value))
                        goto Done;
                    return false;
                }

                _position += (int)(reader.Consumed - consumedBefore);
            Done:
                _position++;
                CurrentIndex += (int)(reader.Consumed - consumedBefore);
                Value = value;
                ValueType = JsonValueType.String;
                TokenType = JsonTokenType.Value;
                return true;
            }
            if (_isFinalBlock)
                ThrowJsonReaderException(ref this);
            return false;
        }

        private void SkipWhiteSpace(ref BufferReader<byte> reader)
        {
            while(true)
            {
                reader.TryPeek(out byte val);
                if (val != JsonConstants.Space && val != JsonConstants.CarriageReturn && val != JsonConstants.LineFeed && val != JsonConstants.Tab)
                {
                    break;
                }
                reader.Advance(1);
                CurrentIndex++;
                // TODO: Does this work for Windows and Unix?
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
}
