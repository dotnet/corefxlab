// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Text.Utf8;

namespace System.Text.Json
{
    public struct JsonParseObject
    {
        private byte[] _buffer;
        private int _start;
        private int _end;

        private const int RowSize = 9;

        public JsonParseObject(byte[] buffer, int start, int end)
        {
            _buffer = buffer;
            _start = start;
            _end = end;
        }

        public bool HasValue()
        {
            var typeCode = _buffer[_start + 8];

            if (typeCode == (byte)JsonParser.JsonValueType.Object || typeCode == (byte)JsonParser.JsonValueType.Array)
            {
                int length = BitConverter.ToInt32(_buffer, _start + 4);
                if (length == 0) return false;
                return true;
            }
            else
            {
                int location = BitConverter.ToInt32(_buffer, _start);
                if (_buffer[location - 1] == '"' && _buffer[location + 4] == '"')
                {
                    return true;
                }
                return (_buffer[location] != 'n' || _buffer[location + 1] != 'u' || _buffer[location + 2] != 'l' || _buffer[location + 3] != 'l');
            }
        }

        public JsonParseObject this[string index]
        {
            get
            {
                int length = BitConverter.ToInt32(_buffer, _start + 4);
                var typeCode = _buffer[_start + 8];

                if (length == 0)
                {
                    throw new KeyNotFoundException();
                }

                if (typeCode != (byte)JsonParser.JsonValueType.Object)
                {
                    throw new NullReferenceException();
                }

                for (int i = _start + RowSize; i <= _end; i += RowSize)
                {
                    length = BitConverter.ToInt32(_buffer, i + 4);
                    typeCode = _buffer[i + 8];

                    if (typeCode == (byte)JsonParser.JsonValueType.Object || typeCode == (byte)JsonParser.JsonValueType.Array)
                    {
                        i += length * RowSize;
                        continue;
                    }

                    int location = BitConverter.ToInt32(_buffer, i);
                    if (isEqual(index, _buffer, location, length))
                    {
                        int newStart = i + RowSize;
                        int newEnd = newStart + RowSize;

                        typeCode = _buffer[newStart + 8];

                        if (typeCode == (byte)JsonParser.JsonValueType.Object || typeCode == (byte)JsonParser.JsonValueType.Array)
                        {
                            length = BitConverter.ToInt32(_buffer, newStart + 4);
                            newEnd = newEnd + RowSize * length;
                        }

                        return new JsonParseObject(_buffer, newStart, newEnd);
                    }

                    typeCode = _buffer[i + RowSize + 8];

                    if (typeCode != (byte)JsonParser.JsonValueType.Object && typeCode != (byte)JsonParser.JsonValueType.Array)
                    {
                        i += RowSize;
                    }
                }

                throw new KeyNotFoundException();

            }
        }

        private bool isEqual(string str, byte[] buffer, int location, int length)
        {
            if (str.Length != length) return false;
            for (int i = 0; i < length; i++)
            {
                if (str[i] != buffer[location + i])
                {
                    return false;
                }
            }
            return true;
        }

        public JsonParseObject this[int index]
        {
            get
            {
                int length = BitConverter.ToInt32(_buffer, _start + 4);
                var typeCode = _buffer[_start + 8];

                if (index < 0 || index >= length)
                {
                    throw new IndexOutOfRangeException();
                }

                if (typeCode != (byte)JsonParser.JsonValueType.Array)
                {
                    throw new NullReferenceException();
                }

                int counter = 0;
                for (int i = _start + RowSize; i <= _end; i += RowSize)
                {
                    typeCode = _buffer[i + 8];

                    if (index == counter)
                    {
                        int newStart = i;
                        int newEnd = i + RowSize;

                        if (typeCode == (byte)JsonParser.JsonValueType.Object || typeCode == (byte)JsonParser.JsonValueType.Array)
                        {
                            length = BitConverter.ToInt32(_buffer, i + 4);
                            newEnd = newEnd + RowSize * length;
                        }
                        return new JsonParseObject(_buffer, newStart, newEnd);
                    }

                    if (typeCode == (byte)JsonParser.JsonValueType.Object || typeCode == (byte)JsonParser.JsonValueType.Array)
                    {
                        length = BitConverter.ToInt32(_buffer, i + 4);
                        i += length * RowSize;
                    }

                    counter++;
                }

                throw new IndexOutOfRangeException();
            }
        }

        public static explicit operator string (JsonParseObject json)
        {
            return GetUtf8String(json).ToString();
        }

        public static explicit operator Utf8String(JsonParseObject json)
        {
            return GetUtf8String(json);
        }

        private static Utf8String GetUtf8String(JsonParseObject json)
        {
            var typeCode = json._buffer[json._start + 8];

            if (typeCode == (byte)JsonParser.JsonValueType.Object || typeCode == (byte)JsonParser.JsonValueType.Array)
            {
                throw new InvalidCastException();
            }

            int location = BitConverter.ToInt32(json._buffer, json._start);
            int length = BitConverter.ToInt32(json._buffer, json._start + 4);
            return new Utf8String(json._buffer, location, length);
        }

        public static explicit operator bool (JsonParseObject json)
        {
            var typeCode = json._buffer[json._start + 8];

            if (typeCode == (byte)JsonParser.JsonValueType.Object || typeCode == (byte)JsonParser.JsonValueType.Array)
            {
                throw new InvalidCastException();
            }

            int length = BitConverter.ToInt32(json._buffer, json._start + 4);
            if (length < 4 || length > 5)
            {
                throw new InvalidCastException();
            }

            int location = BitConverter.ToInt32(json._buffer, json._start);
            bool isTrue = json._buffer[location] == 't' && json._buffer[location + 1] == 'r' && json._buffer[location + 2] == 'u' && json._buffer[location + 3] == 'e';
            bool isFalse = json._buffer[location] == 'f' && json._buffer[location + 1] == 'a' && json._buffer[location + 2] == 'l' && json._buffer[location + 3] == 's' && json._buffer[location + 4] == 'e';

            if (isTrue)
            {
                return true;
            }
            else if (isFalse)
            {
                return false;
            }
            else
            {
                throw new InvalidCastException();
            }
        }

        public static explicit operator int (JsonParseObject json)
        {
            var typeCode = json._buffer[json._start + 8];

            if (typeCode == (byte)JsonParser.JsonValueType.Object || typeCode == (byte)JsonParser.JsonValueType.Array)
            {
                throw new InvalidCastException();
            }

            int location = BitConverter.ToInt32(json._buffer, json._start);
            int length = BitConverter.ToInt32(json._buffer, json._start + 4);

            int count = location;
            bool isNegative = false;
            var nextByte = json._buffer[count];
            if (nextByte == '-')
            {
                isNegative = true;
                count++;
            }

            int result = 0;
            while (count - location < length)
            {
                nextByte = json._buffer[count];
                if (nextByte < '0' || nextByte > '9')
                {
                    throw new InvalidCastException(); // return isNegative ? result * -1 : result;
                }
                int digit = nextByte - '0';
                result = result * 10 + digit;
                count++;
            }

            return isNegative ? result * -1 : result;
        }

        public static explicit operator double (JsonParseObject json)
        {
            var typeCode = json._buffer[json._start + 8];

            if (typeCode == (byte)JsonParser.JsonValueType.Object || typeCode == (byte)JsonParser.JsonValueType.Array)
            {
                throw new InvalidCastException();
            }

            int location = BitConverter.ToInt32(json._buffer, json._start);
            int length = BitConverter.ToInt32(json._buffer, json._start + 4);

            int count = location;
            bool isNegative = false;
            var nextByte = json._buffer[count];
            if (nextByte == '-')
            {
                isNegative = true;
                count++;
                nextByte = json._buffer[count];
            }

            if (nextByte < '0' || nextByte > '9' || count - location >= length)
            {
                throw new InvalidCastException();
            }

            int integerPart = 0;
            while (nextByte >= '0' && nextByte <= '9' && count - location < length)
            {
                int digit = nextByte - '0';
                integerPart = integerPart * 10 + digit;
                count++;
                nextByte = json._buffer[count];
            }

            double result = integerPart;

            int decimalPart = 0;
            if (nextByte == '.')
            {
                count++;
                int numberOfDigits = count;
                nextByte = json._buffer[count];
                while (nextByte >= '0' && nextByte <= '9' && count - location < length)
                {
                    int digit = nextByte - '0';
                    decimalPart = decimalPart * 10 + digit;
                    count++;
                    nextByte = json._buffer[count];
                }
                numberOfDigits = count - numberOfDigits;
                double divisor = Math.Pow(10, numberOfDigits);
                result += decimalPart / divisor;
            }

            int exponentPart = 0;
            bool isExpNegative = false;
            if (nextByte == 'e' || nextByte == 'E')
            {
                count++;
                nextByte = json._buffer[count];
                if (nextByte == '-' || nextByte == '+')
                {
                    if (nextByte == '-')
                    {
                        isExpNegative = true;
                    }
                    count++;
                }
                nextByte = json._buffer[count];
                while (nextByte >= '0' && nextByte <= '9' && count - location < length)
                {
                    int digit = nextByte - '0';
                    exponentPart = exponentPart * 10 + digit;
                    count++;
                    nextByte = json._buffer[count];
                }

                result *= (Math.Pow(10, isExpNegative ? exponentPart * -1 : exponentPart));
            }

            if (count - location > length)
            {
                throw new InvalidCastException();
            }

            return isNegative ? result * -1 : result;

        }

    }
}
