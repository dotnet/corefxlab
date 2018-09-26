// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Reader;

namespace System.Text.JsonLab
{
#if UseInstrumented
    internal ref partial struct Utf8JsonReaderInstrumented
#else
    public ref partial struct Utf8JsonReader
#endif
    {
#if UseInstrumented
        public Utf8JsonReaderInstrumented(in ReadOnlySequence<byte> data)
#else
        public Utf8JsonReader(in ReadOnlySequence<byte> data)
#endif
        {
            _reader = new BufferReader<byte>(data);
            _isSingleSegment = data.IsSingleSegment; //true;
            _buffer = _reader.CurrentSpan;  //data.ToArray();
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

#if !UseInstrumented
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
                    Value = GetNumber(sequence.IsSingleSegment ? sequence.First.Span : sequence.ToArray());
                    ValueType = JsonValueType.Number;
                    reader.Advance(Value.Length);
                    CurrentIndex += Value.Length;
                }
                else if (first == '-')
                {
                    //TODO: Is this a valid check?
                    if (reader.End) JsonThrowHelper.ThrowJsonReaderException(ref this);
                    ReadOnlySequence<byte> sequence = reader.Sequence.Slice(reader.Position);
                    Value = GetNumber(sequence.IsSingleSegment ? sequence.First.Span : sequence.ToArray());
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
            GetNumber(Value);
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
                CurrentIndex += idx;
                return;
            }
            ConsumeStringWithNestedQuotes(ref reader);
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
                CurrentIndex += buffer.Length;
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
                    CurrentIndex += index;
                    ReadOnlySequence<byte> sequence = reader.Sequence.Slice(copy.Position, reader.Position);
                    reader.Advance(1);
                    CurrentIndex++;
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
                CurrentIndex += buffer.Length;
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
                        CurrentIndex += index;
                        ReadOnlySequence<byte> sequence = reader.Sequence.Slice(copy.Position, reader.Position);
                        reader.Advance(1);
                        CurrentIndex++;
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

        private void SkipWhiteSpace(ref BufferReader<byte> reader)
        {
            CurrentIndex += (int)reader.SkipPastAny(JsonConstants.Space, JsonConstants.CarriageReturn, JsonConstants.LineFeed, JsonConstants.Tab);
        }
#endif
    }
}
