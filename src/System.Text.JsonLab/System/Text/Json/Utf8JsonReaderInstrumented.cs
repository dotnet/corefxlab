// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Diagnostics;

namespace System.Text.JsonLab
{
    //TODO: Add multi-segment support
    internal ref struct Utf8JsonReaderInstrumented
    {
        // We are using a ulong to represent our nested state, so we can only go 64 levels deep.
        private const int StackFreeMaxDepth = sizeof(ulong) * 8;
        private readonly ReadOnlySpan<byte> _buffer;

        private readonly int _maxDepth;

        private int _lineNumber;
        private int _position;

        private Stack<bool> _stack;

        // Depth tracks the recursive depth of the nested objects / arrays within the JSON data.
        private int _depth;

        private int _index;

        public JsonReaderException Exception { get; private set; }

        // This mask represents a tiny stack to track the state during nested transitions.
        // The first bit represents the state of the current level (1 == object, 0 == array).
        // Each subsequent bit is the parent / containing type (object or array). Since this
        // reader does a linear scan, we only need to keep a single path as we go through the data.
        private ulong _containerMask;

        private bool _inObject;

        /// <summary>
        /// Gets the token type of the last processed token in the JSON stream.
        /// </summary>
        private JsonTokenType _tokenType;

        /// <summary>
        /// Constructs a new JsonReader instance. This is a stack-only type.
        /// </summary>
        /// <param name="data">The <see cref="Span{byte}"/> value to consume. </param>
        /// <param name="encoder">An encoder used for decoding bytes from <paramref name="data"/> into characters.</param>
        public Utf8JsonReaderInstrumented(ReadOnlySpan<byte> data, int maxDepth)
        {
            _buffer = data;
            _depth = 0;
            _containerMask = 0;
            _stack = maxDepth > StackFreeMaxDepth ? new Stack<bool>() : null;
            _maxDepth = maxDepth;
            _index = 0;

            _tokenType = JsonTokenType.None;

            Exception = null;

            _lineNumber = 1;
            _position = 0;

            _inObject = false;
        }

        public bool Read()
        {
            return ReadSingleSegment();
        }

        private bool ReadFirstToken(byte first)
        {
            if (first == JsonConstants.OpenBrace)
            {
                _depth++;
                _containerMask = 1;
                _tokenType = JsonTokenType.StartObject;
                _index++;
                _position++;
                _inObject = true;
            }
            else if (first == JsonConstants.OpenBracket)
            {
                _depth++;
                _tokenType = JsonTokenType.StartArray;
                _index++;
                _position++;
            }
            else
            {
                return ConsumeSingleValue(first);
            }
            return true;
        }

        private bool ReadSingleSegment()
        {
            bool retVal = false;

            if (_index >= (uint)_buffer.Length)
                goto Done;

            byte first = _buffer[_index];

            if (first <= JsonConstants.Space)
            {
                SkipWhiteSpaceUtf8();
                if (_index >= (uint)_buffer.Length)
                    goto Done;
                first = _buffer[_index];
            }

            if (_tokenType == JsonTokenType.None)
            {
                goto ReadFirstToken;
            }

            if (_tokenType == JsonTokenType.StartObject)
            {
                _index++;
                if (first == JsonConstants.CloseBrace)
                {
                    _position++;
                    if (!EndObject())
                        return false;
                }
                else
                {
                    if (first != JsonConstants.Quote)
                    {
                        Exception = new JsonReaderException($"Expected: {(char)JsonConstants.Quote} for start of property name. Instead reached '{(char)first}'.", _lineNumber, _position);
                        return false;
                    }
                    _position++;
                    if (!ConsumePropertyNameUtf8())
                        return false;
                }
            }
            else if (_tokenType == JsonTokenType.StartArray)
            {
                if (first == JsonConstants.CloseBracket)
                {
                    _index++;
                    _position++;
                    if (!EndArray())
                        return false;
                }
                else
                {
                    if (!ConsumeValueUtf8(first))
                        return false;
                }
            }
            else if (_tokenType == JsonTokenType.PropertyName)
            {
                if (!ConsumeValueUtf8(first))
                    return false;
            }
            else
            {
                if (!ConsumeNextUtf8(first))
                    return false;
            }

            retVal = true;

        Done:
            return retVal;

        ReadFirstToken:
            retVal = ReadFirstToken(first);
            goto Done;
        }

        private bool StartObject()
        {
            _depth++;
            if (_depth > _maxDepth)
            {
                Exception = new JsonReaderException($"Depth of {_depth} within an object is larger than max depth of {_maxDepth}", _lineNumber, _position);
                return false;
            }
            _position++;

            if (_depth <= StackFreeMaxDepth)
                _containerMask = (_containerMask << 1) | 1;
            else
                _stack.Push(true);

            _tokenType = JsonTokenType.StartObject;
            _inObject = true;
            return true;
        }

        private bool EndObject()
        {
            if (!_inObject)
            {
                Exception = new JsonReaderException($"We are within an array but observed an '{(char)JsonConstants.CloseBrace}'", _lineNumber, _position);
                return false;
            }

            if (_depth <= 0)
            {
                Exception = new JsonReaderException($"Mismatched number of start/end objects or arrays. Depth is {_depth} but must be greater than 0", _lineNumber, _position);
                return false;
            }

            if (_depth <= StackFreeMaxDepth)
            {
                _containerMask >>= 1;
                _inObject = (_containerMask & 1) != 0;
            }
            else
            {
                _inObject = _stack.Pop();
            }

            _depth--;
            _tokenType = JsonTokenType.EndObject;
            return true;
        }

        private bool StartArray()
        {
            _depth++;
            if (_depth > _maxDepth)
            {
                Exception = new JsonReaderException($"Depth of {_depth} within an array is larger than max depth of {_maxDepth}", _lineNumber, _position);
                return false;
            }
            _position++;

            if (_depth <= StackFreeMaxDepth)
                _containerMask = _containerMask << 1;
            else
                _stack.Push(false);

            _tokenType = JsonTokenType.StartArray;
            _inObject = false;
            return true;
        }

        private bool EndArray()
        {
            if (_inObject)
            {
                Exception = new JsonReaderException($"We are within an object but observed an '{(char)JsonConstants.CloseBracket}'", _lineNumber, _position);
                return false;
            }

            if (_depth <= 0)
            {
                Exception = new JsonReaderException($"Mismatched number of start/end objects or arrays. Depth is {_depth} but must be greater than 0", _lineNumber, _position);
                return false;
            }

            if (_depth <= StackFreeMaxDepth)
            {
                _containerMask >>= 1;
                _inObject = (_containerMask & 1) != 0;
            }
            else
            {
                _inObject = _stack.Pop();
            }

            _depth--;
            _tokenType = JsonTokenType.EndArray;
            return true;
        }

        /// <summary>
        /// This method consumes the next token regardless of whether we are inside an object or an array.
        /// For an object, it reads the next property name token. For an array, it just reads the next value.
        /// </summary>
        private bool ConsumeNextUtf8(byte marker)
        {
            _index++;
            _position++;
            if (marker == JsonConstants.ListSeperator)
            {
                if (_index >= (uint)_buffer.Length)
                {
                    Exception = new JsonReaderException($"Expected a start of a property name or value after '{(char)JsonConstants.ListSeperator}', but reached end of data instead.", _lineNumber, _position);
                    return false;
                }

                byte first = _buffer[_index];

                if (first <= JsonConstants.Space)
                {
                    SkipWhiteSpaceUtf8();
                    // The next character must be a start of a property name or value.
                    if (_index >= (uint)_buffer.Length)
                    {
                        Exception = new JsonReaderException($"Expected a start of a property name or value after '{(char)JsonConstants.ListSeperator}', but reached end of data instead.", _lineNumber, _position);
                        return false;
                    }
                    first = _buffer[_index];
                }
                if (_inObject)
                {
                    if (first != JsonConstants.Quote)
                    {
                        Exception = new JsonReaderException($"Expected a start of a string property name with '{JsonConstants.Quote}', instead we got '{(char)first}'.", _lineNumber, _position);
                        return false;
                    }

                    _index++;
                    _position++;
                    return ConsumePropertyNameUtf8();
                }
                else
                {
                    return ConsumeValueUtf8(first);
                }
            }
            else if (marker == JsonConstants.CloseBrace)
            {
                return EndObject();
            }
            else if (marker == JsonConstants.CloseBracket)
            {
                return EndArray();
            }
            else
            {
                Exception = new JsonReaderException($"Expected either '{(char)JsonConstants.ListSeperator}', '{(char)JsonConstants.CloseBrace}', or '{(char)JsonConstants.CloseBracket}', instead we got '{(char)marker}'.", _lineNumber, _position);
                return false;
            }
        }

        /// <summary>
        /// This method contains the logic for processing the next value token and determining
        /// what type of data it is.
        /// </summary>
        private bool ConsumeValueUtf8(byte marker)
        {
            _tokenType = JsonTokenType.Value;

            if (marker == JsonConstants.Quote)
            {
                _index++;
                _position++;
                return ConsumeStringUtf8();
            }
            else if (marker == JsonConstants.OpenBrace)
            {
                _index++;
                return StartObject();
            }
            else if (marker == JsonConstants.OpenBracket)
            {
                _index++;
                return StartArray();
            }
            else if (marker - '0' <= '9' - '0')
            {
                return ConsumeNumberUtf8();
            }
            else if (marker == '-')
            {
                if (_buffer.Length - 2 > _index)
                {
                    Exception = new JsonReaderException($"Expected a digit following '{(char)marker}', but reached end of data instead.", _lineNumber, _position);
                    return false;
                }
                return ConsumeNumberUtf8();
            }
            else if (marker == 'f')
            {
                return ConsumeFalseUtf8();
            }
            else if (marker == 't')
            {
                return ConsumeTrueUtf8();
            }
            else if (marker == 'n')
            {
                return ConsumeNullUtf8();
            }
            else
            {
                Exception = new JsonReaderException($"Expected start of a value, instead we got '{(char)marker}'.", _lineNumber, _position);
                return false;
            }
        }

        private bool ConsumeSingleValue(byte marker)
        {
            _tokenType = JsonTokenType.Value;

            if (marker == JsonConstants.Quote)
            {
                _index++;
                _position++;
                return ConsumeStringUtf8();
            }
            else if (marker - '0' <= '9' - '0' || marker == '-')
            {
                if (!ValidateNumber(_buffer))
                    return false;
                _index += _buffer.Length;
                _position += _buffer.Length;
            }
            else if (marker == 'f')
            {
                return ConsumeFalseUtf8();
            }
            else if (marker == 't')
            {
                return ConsumeTrueUtf8();
            }
            else if (marker == 'n')
            {
                return ConsumeNullUtf8();
            }
            else
            {
                Exception = new JsonReaderException($"Expected start of a value, instead we got '{(char)marker}'.", _lineNumber, _position);
                return false;
            }

            return true;
        }

        private bool ConsumeNumberUtf8()
        {
            int idx = _buffer.Slice(_index).IndexOfAny(JsonConstants.Delimiters);
            if (idx == -1)
            {
                Exception = new JsonReaderException($"Expected a delimiter that indicates end of number, but didn't find one.", _lineNumber, _position);
                return false;
            }

            if (!ValidateNumber(_buffer.Slice(_index, idx)))
                return false;

            _index += idx;
            _position += idx;
            return true;
        }

        private bool ConsumeNullUtf8()
        {
            if (!_buffer.Slice(_index).StartsWith(JsonConstants.NullValue))
            {
                ReadOnlySpan<byte> span = _buffer.Slice(_index);
                int length = Math.Min(JsonConstants.NullValue.Length, span.Length);
                string message = "Expected a 'null' value, instead we get '";
                for (int i = 0; i < length; i++)
                {
                    message += (char)span[i];
                }
                message += "'";
                Exception = new JsonReaderException(message, _lineNumber, _position);
                return false;
            }
            _index += 4;
            _position += 4;
            return true;
        }

        private bool ConsumeFalseUtf8()
        {
            if (!_buffer.Slice(_index).StartsWith(JsonConstants.FalseValue))
            {
                ReadOnlySpan<byte> span = _buffer.Slice(_index);
                int length = Math.Min(JsonConstants.FalseValue.Length, span.Length);
                string message = "Expected a 'false' value, instead we get '";
                for (int i = 0; i < length; i++)
                {
                    message += (char)span[i];
                }
                message += "'";
                Exception = new JsonReaderException(message, _lineNumber, _position);
                return false;
            }
            _index += 5;
            _position += 5;
            return true;
        }

        private bool ConsumeTrueUtf8()
        {
            if (!_buffer.Slice(_index).StartsWith(JsonConstants.TrueValue))
            {
                ReadOnlySpan<byte> span = _buffer.Slice(_index);
                int length = Math.Min(JsonConstants.TrueValue.Length, span.Length);
                string message = "Expected a 'true' value, instead we get '";
                for (int i = 0; i < length; i++)
                {
                    message += (char)span[i];
                }
                message += "'";
                Exception = new JsonReaderException(message, _lineNumber, _position);
                return false;
            }
            _index += 4;
            _position += 4;
            return true;
        }

        private bool ConsumePropertyNameUtf8()
        {
            if (!ConsumeStringUtf8())
                return false;

            //Create local copy to avoid bounds checks.
            ReadOnlySpan<byte> localCopy = _buffer;
            if (_index >= (uint)localCopy.Length)
            {
                Exception = new JsonReaderException("Expected a value following the property, but instead reached end of data.", _lineNumber, _position);
                return false;
            }

            byte first = localCopy[_index];

            if (first <= JsonConstants.Space)
            {
                SkipWhiteSpaceUtf8();
                if (_index >= (uint)localCopy.Length)
                {
                    Exception = new JsonReaderException("Expected a value following the property, but instead reached end of data.", _lineNumber, _position);
                    return false;
                }
                first = localCopy[_index];
            }

            // The next character must be a key / value seperator. Validate and skip.
            if (first != JsonConstants.KeyValueSeperator)
            {
                Exception = new JsonReaderException($"Expected a '{(char)JsonConstants.KeyValueSeperator}' following the property string, but instead saw '{(char)first}'.", _lineNumber, _position);
                return false;
            }

            _tokenType = JsonTokenType.PropertyName;
            _index++;
            _position++;
            return true;
        }

        private bool ConsumeStringUtf8()
        {
            //Create local copy to avoid bounds checks.
            ReadOnlySpan<byte> localCopy = _buffer;

            int idx = localCopy.Slice(_index).IndexOf(JsonConstants.Quote);
            if (idx < 0)
            {
                Exception = new JsonReaderException($"Expected a '{(char)JsonConstants.Quote}' to indicate end of string, but instead reached end of data.", _lineNumber, _position);
                return false;
            }

            Debug.Assert(_index >= 1);

            if (localCopy[idx + _index - 1] != JsonConstants.ReverseSolidus)
            {
                ReadOnlySpan<byte> value = localCopy.Slice(_index, idx);
                _index += idx + 1;
                _position += idx + 1;
                if (value.IndexOf((byte)'\n') != -1)
                    AdjustLineNumber(value);
            }
            else
            {
                return ConsumeStringWithNestedQuotes();
            }
            return true;
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

        private bool ConsumeStringWithNestedQuotes()
        {
            //TODO: Optimize looking for nested quotes
            //TODO: Avoid redoing first IndexOf search
            int i = _index;
            while (true)
            {
                int counter = 0;
                int foundIdx = _buffer.Slice(i).IndexOf(JsonConstants.Quote);
                if (foundIdx == -1)
                {
                    Exception = new JsonReaderException($"Expected a '{(char)JsonConstants.Quote}' to indicate end of string, but instead reached end of data.", _lineNumber, _position);
                    return false;
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
            ReadOnlySpan<byte> value = _buffer.Slice(_index, i - _index);
            i++;
            if (value.IndexOf((byte)'\n') != -1)
                AdjustLineNumber(value);
            else
                _position += i - _index;
            _index = i;
            return true;
        }

        private void SkipWhiteSpaceUtf8()
        {
            //Create local copy to avoid bounds checks.
            ReadOnlySpan<byte> localCopy = _buffer;
            for (; _index < localCopy.Length; _index++)
            {
                byte val = localCopy[_index];
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

        private bool ValidateNumber(ReadOnlySpan<byte> data)
        {
            Debug.Assert(data.Length > 0);

            int i = 0;
            byte nextByte = data[i++];

            if (nextByte == '-')
            {
                if (i >= data.Length)
                {
                    Exception = new JsonReaderException($"Invalid number. Last character read: '{(char)nextByte}'. Expected a digit.", _lineNumber, _position);
                    return false;
                }
                nextByte = data[i++];
            }

            Debug.Assert(nextByte >= '0' && nextByte <= '9');

            if (nextByte == '0')
            {
                if (i < data.Length)
                {
                    nextByte = data[i];
                    if (nextByte != '.' && nextByte != 'E' && nextByte != 'e')
                    {
                        Exception = new JsonReaderException($"Invalid number. Last character read: '{(char)nextByte}'. Expected '.' or 'E' or 'e'.", _lineNumber, _position);
                        return false;
                    }
                    i++;
                }
                else
                    return true;
            }
            else
            {
                for (; i < data.Length; i++)
                {
                    nextByte = data[i];
                    if (nextByte < '0' || nextByte > '9')
                        break;
                }
                if (i >= data.Length)
                    return true;
                if (nextByte != '.' && nextByte != 'E' && nextByte != 'e')
                {
                    Exception = new JsonReaderException($"Invalid number. Last character read: '{(char)nextByte}'. Expected '.' or 'E' or 'e'.", _lineNumber, _position);
                    return false;
                }
                i++;
            }

            Debug.Assert(nextByte == '.' || nextByte == 'E' || nextByte == 'e');

            if (nextByte == '.')
            {
                if (i >= data.Length)
                {
                    Exception = new JsonReaderException($"Invalid number. Last character read: '{(char)nextByte}'. Expected a digit.", _lineNumber, _position);
                    return false;
                }

                for (; i < data.Length; i++)
                {
                    nextByte = data[i];
                    if (nextByte < '0' || nextByte > '9')
                        break;
                }
                if (i >= data.Length)
                    return true;
                if (nextByte != 'E' && nextByte != 'e')
                {
                    Exception = new JsonReaderException($"Invalid number. Last character read: '{(char)nextByte}'. Expected 'E' or 'e'.", _lineNumber, _position);
                    return false;
                }
                i++;
            }

            Debug.Assert(nextByte == 'E' || nextByte == 'e');

            if (i >= data.Length)
            {
                Exception = new JsonReaderException($"Invalid number. Last character read: '{(char)nextByte}'. Expected a digit.", _lineNumber, _position);
                return false;
            }
            nextByte = data[i];
            if (nextByte == '+' || nextByte == '-')
            {
                i++;
                if (i >= data.Length)
                {
                    Exception = new JsonReaderException($"Invalid number. Last character read: '{(char)nextByte}'. Expected a digit.", _lineNumber, _position);
                    return false;
                }
            }

            for (; i < data.Length; i++)
            {
                nextByte = data[i];
                if (nextByte < '0' || nextByte > '9')
                    break;
            }

            if (i < data.Length)
            {
                Exception = new JsonReaderException($"Invalid number. Last character read: '{(char)nextByte}'. Expected a digit.", _lineNumber, _position);
                return false;
            }

            return true;
        }
    }
}
