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
            _str = str;
            _index = 0;
            _insideObject = 0;
            _insideArray = 0;
            TokenType = 0;
        }

        public JsonReader(string str)
        {
            _str = new Utf8String(str);
            _index = 0;
            _insideObject = 0;
            _insideArray = 0;
            TokenType = 0;
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
            SkipEmpty();
            var nextByte = (byte) _str[_index];

            if (nextByte == '{')
            {
                return JsonValueType.Object;
            }

            if (nextByte == '[')
            {
                return JsonValueType.Array;
            }

            if (nextByte == '-' || (nextByte >= '0' && nextByte <= '9'))
            {
                return JsonValueType.Number;
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

            if (nextByte == '"')
            {
                return JsonValueType.String;
            }

            throw new ArgumentOutOfRangeException();
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
                    return _str.Substring(0, 0);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Utf8String ReadStringValue()
        {
            if (_index >= _str.Length)
                throw new IndexOutOfRangeException("Json length is " + _str.Length + " and reading index " + _index +
                                                   ".");
            _index++;

            var count = _index;
            Utf8String outString;
            do
            {
                var nextByte = (byte) _str[count];
                while (nextByte != '"')
                {
                    count++;
                    nextByte = (byte) _str[count];
                }
                count++;
                outString = _str.Substring(_index, count - _index - 1);
            } while (GetNumOfBackSlashesAtEndOfString(outString)%2 != 0);

            var strLength = count - _index;
            var resultString = _str.Substring(_index, strLength - 1);
            _index += strLength;

            SkipEmpty();
            return resultString;
        }

        private static int GetNumOfBackSlashesAtEndOfString(Utf8String str)
        {
            var numOfBackSlashes = 0;
            if (str.Length - 1 < 0) return numOfBackSlashes;
            var nextByte = (byte) str[str.Length - 1];
            while (nextByte == '\\')
            {
                numOfBackSlashes++;
                if (str.Length - (numOfBackSlashes + 1) < 0) return numOfBackSlashes;
                nextByte = (byte) str[str.Length - (numOfBackSlashes + 1)];
            }
            return numOfBackSlashes;
        }

        private Utf8String ReadNumberValue()
        {
            if (_index >= _str.Length)
                throw new IndexOutOfRangeException("Json length is " + _str.Length + " and reading index " + _index +
                                                   ".");

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
            if (_index >= _str.Length)
                throw new IndexOutOfRangeException("Json length is " + _str.Length + " and reading index " + _index +
                                                   ".");

            var trueString = _str.Substring(_index, 4);
            if ((byte) trueString[0] != 't' || (byte) trueString[1] != 'r' || (byte) trueString[2] != 'u' ||
                (byte) trueString[3] != 'e')
            {
                throw new FormatException("Invalid json, tried to read 'true'.");
            }

            _index += 4;

            SkipEmpty();
            return trueString;
        }

        private Utf8String ReadFalseValue()
        {
            if (_index >= _str.Length)
                throw new IndexOutOfRangeException("Json length is " + _str.Length + " and reading index " + _index +
                                                   ".");

            var falseString = _str.Substring(_index, 5);
            if ((byte) falseString[0] != 'f' || (byte) falseString[1] != 'a' || (byte) falseString[2] != 'l' ||
                (byte) falseString[3] != 's' | (byte) falseString[4] != 'e')
            {
                throw new FormatException("Invalid json, tried to read 'false'.");
            }

            _index += 5;

            SkipEmpty();
            return falseString;
        }

        private Utf8String ReadNullValue()
        {
            if (_index >= _str.Length)
                throw new IndexOutOfRangeException("Json length is " + _str.Length + " and reading index " + _index +
                                                   ".");

            var nullString = _str.Substring(_index, 4);
            if ((byte) nullString[0] != 'n' || (byte) nullString[1] != 'u' || (byte) nullString[2] != 'l' ||
                (byte) nullString[3] != 'l')
            {
                throw new FormatException("Invalid json, tried to read 'null'.");
            }

            _index += 4;

            SkipEmpty();
            return nullString;
        }

        private void SkipEmpty()
        {
            if (_index >= _str.Length)
                throw new IndexOutOfRangeException("Json length is " + _str.Length + " and reading index " + _index +
                                                   ".");
            var nextByte = (byte) _str[_index];
            while (nextByte == ' ' || nextByte == '\n' || nextByte == '\r' || nextByte == '\t')
            {
                _index++;
                nextByte = (byte) _str[_index];
            }
        }

        private JsonTokenType GetNextTokenType()
        {
            SkipEmpty();

            var nextByte = (byte) _str[_index];

            if (TokenType == JsonTokenType.ArrayStart && nextByte != ']')
            {
                return JsonTokenType.Value;
            }

            if (TokenType == JsonTokenType.ObjectStart && nextByte != '}')
            {
                return JsonTokenType.Property;
            }

            if (nextByte == '{')
            {
                _index++;
                _insideObject++;
                return JsonTokenType.ObjectStart;
            }

            if (nextByte == '}')
            {
                _index++;
                _insideObject--;
                return JsonTokenType.ObjectEnd;
            }

            if (nextByte == '[')
            {
                _index++;
                _insideArray++;
                return JsonTokenType.ArrayStart;
            }

            if (nextByte == ']')
            {
                _index++;
                _insideArray--;
                return JsonTokenType.ArrayEnd;
            }

            if (TokenType == JsonTokenType.Property && nextByte == ',')
            {
                _index++;
                return JsonTokenType.Property;
            }

            if (TokenType == JsonTokenType.Value && nextByte == ',')
            {
                _index++;
                return JsonTokenType.Value;
            }

            if (TokenType == JsonTokenType.ArrayEnd && nextByte == ',')
            {
                _index++;
                return _insideObject > _insideArray ? JsonTokenType.Property : JsonTokenType.Value;
            }

            if (TokenType == JsonTokenType.ObjectEnd && nextByte == ',')
            {
                _index++;
                return _insideObject > _insideArray ? JsonTokenType.Property : JsonTokenType.Value;
            }

            throw new FormatException("Unable to get next token type. Check json format.");
        }

        private void MoveToNextTokenType()
        {
            TokenType = GetNextTokenType();
        }
    }
}