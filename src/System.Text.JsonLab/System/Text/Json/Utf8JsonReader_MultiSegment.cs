// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//TODO: Add multi-segment support for tracking line number/position

using System.Buffers;
using System.Buffers.Reader;

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

        private void ReadFirstToken(ref BufferReader<byte> reader, byte first)
        {
            if (first == JsonConstants.OpenBrace)
            {
                Depth++;
                _containerMask = 1;
                TokenType = JsonTokenType.StartObject;
                ValueType = JsonValueType.Object;
                reader.Advance(1);
                CurrentIndex++;
                _inObject = true;
            }
            else if (first == JsonConstants.OpenBracket)
            {
                Depth++;
                TokenType = JsonTokenType.StartArray;
                ValueType = JsonValueType.Array;
                reader.Advance(1);
                CurrentIndex++;
            }
            else
            {
                // Cannot call ConsumeNumber since it looks for the delimiter and we won't find it.
                if ((uint)(first - '0') <= '9' - '0')
                {
                    ReadOnlySequence<byte> sequence = reader.Sequence.Slice(reader.Position);
                    TryGetNumber(sequence.IsSingleSegment ? sequence.First.Span : sequence.ToArray(), out ReadOnlySpan<byte> number);
                    Value = number;
                    ValueType = JsonValueType.Number;
                    reader.Advance(Value.Length);
                    CurrentIndex += Value.Length;
                }
                else if (first == '-')
                {
                    //TODO: Is this a valid check?
                    if (reader.End) JsonThrowHelper.ThrowJsonReaderException(ref this);
                    ReadOnlySequence<byte> sequence = reader.Sequence.Slice(reader.Position);
                    TryGetNumber(sequence.IsSingleSegment ? sequence.First.Span : sequence.ToArray(), out ReadOnlySpan<byte> number);
                    Value = number;
                    ValueType = JsonValueType.Number;
                    reader.Advance(Value.Length);
                    CurrentIndex += Value.Length;
                }
                else
                {
                    ConsumeValue(ref reader, first);
                }
            }
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

            TokenStartIndex = CurrentIndex;

            if (TokenType == JsonTokenType.None)
            {
                goto ReadFirstToken;
            }

            if (TokenType == JsonTokenType.StartObject)
            {
                reader.Advance(1);
                CurrentIndex++;
                if (first == JsonConstants.CloseBrace)
                {
                    EndObject();
                }
                else
                {
                    if (first != JsonConstants.Quote) JsonThrowHelper.ThrowJsonReaderException(ref this);
                    TokenStartIndex++;
                    ConsumePropertyName(ref reader);
                }
            }
            else if (TokenType == JsonTokenType.StartArray)
            {
                if (first == JsonConstants.CloseBracket)
                {
                    reader.Advance(1);
                    CurrentIndex++;
                    EndArray();
                }
                else
                {
                    ConsumeValue(ref reader, first);
                }
            }
            else if (TokenType == JsonTokenType.PropertyName)
            {
                ConsumeValue(ref reader, first);
            }
            else
            {
                ConsumeNextToken(ref reader, first);
            }

            retVal = true;

        Done:
            return retVal;

        ReadFirstToken:
            ReadFirstToken(ref reader, first);
            retVal = true;
            goto Done;
        }

        private void ConsumeNextToken(ref BufferReader<byte> reader, byte marker)
        {
            reader.Advance(1);
            CurrentIndex++;
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

                TokenStartIndex = CurrentIndex;
                if (_inObject)
                {
                    if (first != JsonConstants.Quote)
                        JsonThrowHelper.ThrowJsonReaderException(ref this);

                    reader.Advance(1);
                    CurrentIndex++;
                    TokenStartIndex++;
                    ConsumePropertyName(ref reader);
                }
                else
                {
                    ConsumeValue(ref reader, first);
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

        private void ConsumeValue(ref BufferReader<byte> reader, byte marker)
        {
            TokenType = JsonTokenType.Value;

            if (marker == JsonConstants.Quote)
            {
                reader.Advance(1);
                CurrentIndex++;
                TokenStartIndex++;
                ConsumeString(ref reader);
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
                ConsumeNumber(ref reader);
            }
            else if (marker == '-')
            {
                //TODO: Is this a valid check or do we need to do an Advance to be sure?
                if (reader.End) JsonThrowHelper.ThrowJsonReaderException(ref this);
                ConsumeNumber(ref reader);
            }
            else if (marker == 'f')
            {
                ConsumeFalse(ref reader);
            }
            else if (marker == 't')
            {
                ConsumeTrue(ref reader);
            }
            else if (marker == 'n')
            {
                ConsumeNull(ref reader);
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

        private void ConsumeNumber(ref BufferReader<byte> reader)
        {
            //TODO: Increment Index by number of bytes advanced
            if (!reader.TryReadToAny(out ReadOnlySpan<byte> span, JsonConstants.Delimiters, advancePastDelimiter: false))
            {
                JsonThrowHelper.ThrowJsonReaderException(ref this);
            }

            Value = span;
            ValueType = JsonValueType.Number;
            TryGetNumber(Value, out _);
        }

        private void ConsumeNull(ref BufferReader<byte> reader)
        {
            Value = JsonConstants.NullValue;
            ValueType = JsonValueType.Null;

            if (!reader.IsNext(JsonConstants.NullValue, advancePast: true))
            {
                JsonThrowHelper.ThrowJsonReaderException(ref this);
            }
            CurrentIndex += 4;
        }

        private void ConsumeFalse(ref BufferReader<byte> reader)
        {
            Value = JsonConstants.FalseValue;
            ValueType = JsonValueType.False;

            if (!reader.IsNext(JsonConstants.FalseValue, advancePast: true))
            {
                JsonThrowHelper.ThrowJsonReaderException(ref this);
            }
            CurrentIndex += 5;
        }

        private void ConsumeTrue(ref BufferReader<byte> reader)
        {
            Value = JsonConstants.TrueValue;
            ValueType = JsonValueType.True;

            if (!reader.IsNext(JsonConstants.TrueValue, advancePast: true))
            {
                JsonThrowHelper.ThrowJsonReaderException(ref this);
            }
            CurrentIndex += 4;
        }

        private void ConsumePropertyName(ref BufferReader<byte> reader)
        {
            ConsumeString(ref reader);

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
            CurrentIndex++;
        }

        private void ConsumeString(ref BufferReader<byte> reader)
        {
            if (reader.TryReadTo(out ReadOnlySpan<byte> value, JsonConstants.Quote, (byte)'\\', advancePastDelimiter: true))
            {
                Value = value;
                ValueType = JsonValueType.String;
                return;
            }
            JsonThrowHelper.ThrowJsonReaderException(ref this);
        }

        private void SkipWhiteSpace(ref BufferReader<byte> reader)
        {
            CurrentIndex += (int)reader.SkipPastAny(JsonConstants.Space, JsonConstants.CarriageReturn, JsonConstants.LineFeed, JsonConstants.Tab);
        }
    }
}
