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
            Instrument = false;
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
            Instrument = false;
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
                        //TODO: Is this a valid check?
                        if (reader.End)
                            ThrowJsonReaderException(ref this);
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
                    BufferReader<byte> copy = reader;
                    reader.Advance(1);
                    CurrentIndex++;
                    _position++;
                    if (ConsumePropertyName(ref reader))
                    {
                        return true;
                    }
                    CurrentIndex = prevCurrentIndex;
                    TokenType = JsonTokenType.StartObject;
                    _position = prevPosition;
                    reader = copy;
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
                BufferReader<byte> copy = reader;
                if (ConsumeNextToken(ref reader, first))
                {
                    return true;
                }
                CurrentIndex = prevCurrentIndex;
                TokenType = prevTokenType;
                _position = prevPosition;
                reader = copy;
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
                    _position++;
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
                BufferReader<byte> copy = reader;
                reader.Advance(1);
                CurrentIndex++;
                _position++;
                TokenStartIndex++;
                if (!ConsumeString(ref reader))
                {
                    CurrentIndex = prevCurrentIndex;
                    _position = prevPosition;
                    reader = copy;
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
                //TODO: Is this a valid check or do we need to do an Advance to be sure?
                if (reader.End) ThrowJsonReaderException(ref this);
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
            else if (marker == '/')
            {
                // TODO: Comments?
                ThrowNotImplementedException();
            }
            else
            {
                ThrowJsonReaderException(ref this);
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
            CurrentIndex += (int)(reader.Consumed - consumedBefore);
            _position += (int)(reader.Consumed - consumedBefore);
            Value = span;
            ValueType = JsonValueType.Number;
            TokenType = JsonTokenType.Value;
            return TryGetNumber(Value, out _);
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
                CurrentIndex += (int)(reader.Consumed - consumedBefore);
                Value = value;
                ValueType = JsonValueType.String;
                TokenType = JsonTokenType.Value;
                if (Instrument)
                {
                    _position += (int)(reader.Consumed - consumedBefore);
                    if (Value.IndexOf((byte)'\n') != -1)
                        AdjustLineNumber(Value);
                }
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
                if (Instrument)
                {
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
}
