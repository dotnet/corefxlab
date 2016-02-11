// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Utf8;

namespace System.Text.Json
{
    public struct JsonReader : IDisposable
    {
        private Utf8String _str;
        private int _index;
        private int _insideObject;
        private int _insideArray;
        public JsonTokenType TokenType;
        private bool _jsonStartIsObject;

        public enum JsonTokenType
        {
            // Start = 0 state reserved for internal use
            ObjectStart = 1,
            ObjectEnd = 2,
            ArrayStart = 3,
            ArrayEnd = 4,
            Property = 5,
            Value = 6
        };

        public enum JsonValueType
        {
            String,
            Number,
            Object,
            Array,
            True,
            False,
            Null
        }

        public JsonReader(Utf8String str)
        {
            _str = str.Trim();
            _index = 0;
            _insideObject = 0;
            _insideArray = 0;
            TokenType = 0;
            _jsonStartIsObject = (byte)_str[0] == '{';
        }

        public JsonReader(string str)
        {
            _str = new Utf8String(str).Trim();
            _index = 0;
            _insideObject = 0;
            _insideArray = 0;
            TokenType = 0;
            _jsonStartIsObject = (byte)_str[0] == '{';
        }

        public void Dispose()
        {
        }

        public bool Read()
        {
            var canRead = _index < _str.Length;
            if (canRead) MoveToNextTokenType();
            return canRead;
        }

        public Utf8String GetName()
        {
            SkipEmpty();
            var str = ReadStringValue();
            _index++;
            return str;
        }

        public JsonValueType GetJsonValueType()
        {
            var nextByte = (byte) _str[_index];

            while (isWhiteSpace(nextByte))
            {
                _index++;
                nextByte = (byte)_str[_index];
            }

            if (nextByte == '"')
            {
                return JsonValueType.String;
            }

            if (nextByte == '{')
            {
                return JsonValueType.Object;
            }

            if (nextByte == '[')
            {
                return JsonValueType.Array;
            }

            if (nextByte == 't')
            {
                return JsonValueType.True;
            }

            if (nextByte == 'f')
            {
                return JsonValueType.False;
            }

            if (nextByte == 'n')
            {
                return JsonValueType.Null;
            }

            if (nextByte == '-' || (nextByte >= '0' && nextByte <= '9'))
            {
                return JsonValueType.Number;
            }

            throw new FormatException("Invalid json, tried to read char '" + nextByte + "'.");
        }

        public Utf8String GetValue()
        {
            var type = GetJsonValueType();
            SkipEmpty();
            switch (type)
            {
                case JsonValueType.String:
                    return ReadStringValue();
                case JsonValueType.Number:
                    return ReadNumberValue();
                case JsonValueType.True:
                    return ReadTrueValue();
                case JsonValueType.False:
                    return ReadFalseValue();
                case JsonValueType.Null:
                    return ReadNullValue();
                case JsonValueType.Object:
                case JsonValueType.Array:
                    return Utf8String.Empty;
                default:
                    throw new ArgumentException("Invalid json value type '" + type + "'.");
            }
        }

        private Utf8String ReadStringValue()
        {
            _index++;
            var count = _index;
            do
            {
                while ((byte) _str[count] != '"')
                {
                    count++;
                }
                count++;
            } while (AreNumOfBackSlashesAtEndOfStringOdd(count - 2));

            var strLength = count - _index;
            var resultString = _str.Substring(_index, strLength - 1);
            _index += strLength;

            SkipEmpty();
            return resultString;
        }

        private bool AreNumOfBackSlashesAtEndOfStringOdd(int count)
        {
            var length = count - _index;
            if (length < 0) return false;
            var nextByte = (byte) _str[count];
            if (nextByte != '\\') return false;
            var numOfBackSlashes = 0;
            while (nextByte == '\\')
            {
                numOfBackSlashes++;
                if ((length - numOfBackSlashes) < 0) return numOfBackSlashes%2 != 0;
                nextByte = (byte) _str[count - numOfBackSlashes];
            }
            return numOfBackSlashes%2 != 0;
        }

        private Utf8String ReadNumberValue()
        {
            var count = _index;

            var nextByte = (byte) _str[count];
            if (nextByte == '-')
            {
                count++;
            }

            nextByte = (byte) _str[count];
            while (nextByte >= '0' && nextByte <= '9')
            {
                count++;
                nextByte = (byte) _str[count];
            }

            if (nextByte == '.')
            {
                count++;
            }

            nextByte = (byte) _str[count];
            while (nextByte >= '0' && nextByte <= '9')
            {
                count++;
                nextByte = (byte) _str[count];
            }

            if (nextByte == 'e' || nextByte == 'E')
            {
                count++;
                nextByte = (byte) _str[count];
                if (nextByte == '-' || nextByte == '+')
                {
                    count++;
                }
                nextByte = (byte) _str[count];
                while (nextByte >= '0' && nextByte <= '9')
                {
                    count++;
                    nextByte = (byte) _str[count];
                }
            }

            var length = count - _index;
            var resultStr = _str.Substring(_index, count - _index);
            _index += length;
            SkipEmpty();
            return resultStr;
        }

        private Utf8String ReadTrueValue()
        {
            var trueString = _str.Substring(_index, 4);
            if ((byte) trueString[1] != 'r' || (byte) trueString[2] != 'u' || (byte) trueString[3] != 'e')
            {
                throw new FormatException("Invalid json, tried to read 'true'.");
            }

            _index += 4;

            SkipEmpty();
            return trueString;
        }

        private Utf8String ReadFalseValue()
        {
            var falseString = _str.Substring(_index, 5);
            if ( (byte) falseString[1] != 'a' || (byte) falseString[2] != 'l' || (byte) falseString[3] != 's' || (byte) falseString[4] != 'e')
            {
                throw new FormatException("Invalid json, tried to read 'false'.");
            }

            _index += 5;

            SkipEmpty();
            return falseString;
        }

        private Utf8String ReadNullValue()
        {
            var nullString = _str.Substring(_index, 4);
            if ((byte) nullString[1] != 'u' || (byte) nullString[2] != 'l' || (byte) nullString[3] != 'l')
            {
                throw new FormatException("Invalid json, tried to read 'null'.");
            }

            _index += 4;

            SkipEmpty();
            return nullString;
        }

        private void SkipEmpty()
        {
            var nextByte = (byte)_str[_index];

            while (isWhiteSpace(nextByte))
            {
                _index++;
                nextByte = (byte)_str[_index];
            }
        }

        private static bool isWhiteSpace(byte nextByte)
        {
            return nextByte == ' ' || nextByte == '\n' || nextByte == '\r' || nextByte == '\t';
        }

        private void MoveToNextTokenType()
        {
            var nextByte = (byte) _str[_index];
            while (isWhiteSpace(nextByte))
            {
                _index++;
                nextByte = (byte)_str[_index];
            }

            switch (TokenType)
            {
                case JsonTokenType.ObjectStart:
                    if (nextByte != '}')
                    {
                        TokenType = JsonTokenType.Property;
                        return;
                    }
                    break;
                case JsonTokenType.ObjectEnd:
                    if (nextByte == ',')
                    {
                        _index++;
                        if (_insideObject == _insideArray)
                        {
                            TokenType = !_jsonStartIsObject ? JsonTokenType.Property : JsonTokenType.Value;
                            return;
                        }
                        TokenType = _insideObject > _insideArray ? JsonTokenType.Property : JsonTokenType.Value;
                        return;
                    }
                    break;
                case JsonTokenType.ArrayStart:
                    if (nextByte != ']')
                    {
                        TokenType = JsonTokenType.Value;
                        return;
                    }
                    break;
                case JsonTokenType.ArrayEnd:
                    if (nextByte == ',')
                    {
                        _index++;
                        if (_insideObject == _insideArray)
                        {
                            TokenType = !_jsonStartIsObject ? JsonTokenType.Property : JsonTokenType.Value;
                            return;
                        }
                        TokenType = _insideObject > _insideArray ? JsonTokenType.Property : JsonTokenType.Value;
                        return;
                    }
                    break;
                case JsonTokenType.Property:
                    if (nextByte == ',')
                    {
                        _index++;
                        return;
                    }
                    break;
                case JsonTokenType.Value:
                    if (nextByte == ',')
                    {
                        _index++;
                        return;
                    }
                    break;
            }

            _index++;
            switch (nextByte)
            {
                case (byte) '{':
                    _insideObject++;
                    TokenType = JsonTokenType.ObjectStart;
                    return;
                case (byte) '}':
                    _insideObject--;
                    TokenType = JsonTokenType.ObjectEnd;
                    return;
                case (byte) '[':
                    _insideArray++;
                    TokenType = JsonTokenType.ArrayStart;
                    return;
                case (byte) ']':
                    _insideArray--;
                    TokenType = JsonTokenType.ArrayEnd;
                    return;
                default:
                    throw new FormatException("Unable to get next token type. Check json format.");
            }
        }
    }
}