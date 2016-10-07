// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Collections.Generic;
using System.Text.Utf8;

namespace System.Text.Json
{
    public struct JsonObject : IDisposable
    {
        private BufferPool _pool;
        private Memory<byte> _dbMemory;
        private ReadOnlySpan<byte> _db; 
        private ReadOnlySpan<byte> _values;
                
        internal const int RowSize = 9; // Do not change, unless you also change FindLocation

        public static JsonObject Parse(ReadOnlySpan<byte> utf8Json)
        {
            var parser = new JsonParser();
            var result = parser.Parse(utf8Json);
            return result;
        }

        public static JsonObject Parse(ReadOnlySpan<byte> utf8Json, BufferPool pool = null)
        {
            var parser = new JsonParser();
            var result = parser.Parse(utf8Json, pool);
            return result;
        }

        internal JsonObject(ReadOnlySpan<byte> values, ReadOnlySpan<byte> db, BufferPool pool = null, Memory<byte> dbMemory = default(Memory<byte>))
        {
            _db = db;
            _values = values;
            _pool = pool;
            _dbMemory = dbMemory;
        }

        public bool TryGetValue(Utf8String propertyName, out JsonObject value)
        {
            int length = Length;
            var valueType = _db.Slice(8).Read<JsonValueType>();

            if (length == 0) {
                throw new KeyNotFoundException();
            }

            if (valueType != JsonValueType.Object) {
                throw new NullReferenceException();
            }

            for (int i = RowSize; i <= _db.Length; i += RowSize) {
                length = _db.Slice(i + 4).Read<int>();
                valueType = _db.Slice(i + 8).Read<JsonValueType>();

                if (valueType == JsonValueType.Object || valueType == JsonValueType.Array) {
                    i += length * RowSize;
                    continue;
                }

                int location = _db.Slice(i).Read<int>();
                if (new Utf8String(_values.Slice(location, length)) == propertyName) {
                    int newStart = i + RowSize;
                    int newEnd = newStart + RowSize;

                    valueType = _db.Slice(newStart + 8).Read<JsonValueType>();

                    if (valueType == JsonValueType.Object || valueType == JsonValueType.Array) {
                        length = _db.Slice(newStart + 4).Read<int>();
                        newEnd = newEnd + RowSize * length;
                    }

                    value = new JsonObject(_values, _db.Slice(newStart, newEnd - newStart), null, Memory<byte>.Empty);
                    return true;
                }

                valueType = _db.Slice(i + RowSize + 8).Read<JsonValueType>();

                if (valueType != JsonValueType.Object && valueType != JsonValueType.Array) {
                    i += RowSize;
                }
            }

            value = default(JsonObject);
            return false;
        }

        public bool TryGetValue(string propertyName, out JsonObject value)
        {
            int length = Length;
            var valueType = _db.Slice(8).Read<JsonValueType>();

            if (length == 0) {
                throw new KeyNotFoundException();
            }

            if (valueType != JsonValueType.Object) {
                throw new NullReferenceException();
            }

            for (int i = RowSize; i <= _db.Length; i += RowSize) {
                length = _db.Slice(i + 4).Read<int>();
                valueType = _db.Slice(i + 8).Read<JsonValueType>();

                if (valueType == JsonValueType.Object || valueType == JsonValueType.Array) {
                    i += length * RowSize;
                    continue;
                }

                int location = _db.Slice(i).Read<int>();
                if (new Utf8String(_values.Slice(location, length)) == propertyName) {
                    int newStart = i + RowSize;
                    int newEnd = newStart + RowSize;

                    valueType = _db.Slice(newStart + 8).Read<JsonValueType>();

                    if (valueType == JsonValueType.Object || valueType == JsonValueType.Array) {
                        length = _db.Slice(newStart + 4).Read<int>();
                        newEnd = newEnd + RowSize * length;
                    }

                    value = new JsonObject(_values, _db.Slice(newStart, newEnd - newStart));
                    return true;
                }

                valueType = _db.Slice(i + RowSize + 8).Read<JsonValueType>();

                if (valueType != JsonValueType.Object && valueType != JsonValueType.Array) {
                    i += RowSize;
                }
            }

            value = default(JsonObject);
            return false;
        }

        public JsonObject this[Utf8String name]
        {
            get {
                JsonObject value;
                if(TryGetValue(name, out value)) {
                    return value;
                }
                throw new KeyNotFoundException();
            }
        }

        public JsonObject this[string name] {
            get {
                JsonObject value;
                if (TryGetValue(name, out value)) {
                    return value;
                }
                throw new KeyNotFoundException();
            }
        }

        public JsonObject this[int index] {
            get {
                int length = Length;
                var valueType = Type;

                if (index < 0 || index >= length) {
                    throw new IndexOutOfRangeException();
                }

                if (valueType != JsonValueType.Array) {
                    throw new NullReferenceException();
                }

                int counter = 0;
                for (int i = RowSize; i <= _db.Length; i += RowSize) {
                    valueType = _db.Slice(i + 8).Read<JsonValueType>();

                    if (index == counter) {
                        int newStart = i;
                        int newEnd = i + RowSize;

                        if (valueType == JsonValueType.Object || valueType == JsonValueType.Array) {
                            length = _db.Slice(i + 4).Read<int>();
                            newEnd = newEnd + RowSize * length;
                        }
                        return new JsonObject(_values, _db.Slice(newStart, newEnd - newStart));
                    }

                    if (valueType == JsonValueType.Object || valueType == JsonValueType.Array) {
                        length = _db.Slice(i + 4).Read<int>();
                        i += length * RowSize;
                    }

                    counter++;
                }

                throw new IndexOutOfRangeException();
            }
        }

        public static explicit operator string(JsonObject json)
        {
            var utf8 = (Utf8String)json;
            return utf8.ToString();
        }

        public static explicit operator Utf8String(JsonObject json)
        {
            if (!json.IsSimpleValue) {
                throw new InvalidCastException();
            }

            int location = json.Location;
            int length = json.Length;
            return new Utf8String(json._values.Slice(location, length));
        }

        public static explicit operator bool(JsonObject json)
        {
            if (!json.IsSimpleValue) {
                throw new InvalidCastException();
            }

            int length = json.Length;
            if (length < 4 || length > 5) {
                throw new InvalidCastException();
            }

            int location = json.Location;
            bool isTrue = json._values[location] == 't' && json._values[location + 1] == 'r' && json._values[location + 2] == 'u' && json._values[location + 3] == 'e';
            bool isFalse = json._values[location] == 'f' && json._values[location + 1] == 'a' && json._values[location + 2] == 'l' && json._values[location + 3] == 's' && json._values[location + 4] == 'e';

            if (isTrue) {
                return true;
            } else if (isFalse) {
                return false;
            } else {
                throw new InvalidCastException();
            }
        }

        public static explicit operator int(JsonObject json)
        {
            var type = json.Type;

            if (type == JsonValueType.Object || type == JsonValueType.Array) {
                throw new InvalidCastException();
            }

            int location = json.Location;
            int length = json.Length;

            int count = location;
            bool isNegative = false;
            var nextByte = json._values[count];
            if (nextByte == '-') {
                isNegative = true;
                count++;
            }

            int result = 0;
            while (count - location < length) {
                nextByte = json._values[count];
                if (nextByte < '0' || nextByte > '9') {
                    throw new InvalidCastException(); // return isNegative ? result * -1 : result;
                }
                int digit = nextByte - '0';
                result = result * 10 + digit;
                count++;
            }

            return isNegative ? result * -1 : result;
        }

        public static explicit operator double(JsonObject json)
        {
            var typeCode = json.Type;

            if (typeCode == JsonValueType.Object || typeCode == JsonValueType.Array) {
                throw new InvalidCastException();
            }

            int location = json.Location;
            int length = json.Length;

            int count = location;
            bool isNegative = false;
            var nextByte = json._values[count];
            if (nextByte == '-') {
                isNegative = true;
                count++;
                nextByte = json._values[count];
            }

            if (nextByte < '0' || nextByte > '9' || count - location >= length) {
                throw new InvalidCastException();
            }

            int integerPart = 0;
            while (nextByte >= '0' && nextByte <= '9' && count - location < length) {
                int digit = nextByte - '0';
                integerPart = integerPart * 10 + digit;
                count++;
                nextByte = json._values[count];
            }

            double result = integerPart;

            int decimalPart = 0;
            if (nextByte == '.') {
                count++;
                int numberOfDigits = count;
                nextByte = json._values[count];
                while (nextByte >= '0' && nextByte <= '9' && count - location < length) {
                    int digit = nextByte - '0';
                    decimalPart = decimalPart * 10 + digit;
                    count++;
                    nextByte = json._values[count];
                }
                numberOfDigits = count - numberOfDigits;
                double divisor = Math.Pow(10, numberOfDigits);
                result += decimalPart / divisor;
            }

            int exponentPart = 0;
            bool isExpNegative = false;
            if (nextByte == 'e' || nextByte == 'E') {
                count++;
                nextByte = json._values[count];
                if (nextByte == '-' || nextByte == '+') {
                    if (nextByte == '-') {
                        isExpNegative = true;
                    }
                    count++;
                }
                nextByte = json._values[count];
                while (nextByte >= '0' && nextByte <= '9' && count - location < length) {
                    int digit = nextByte - '0';
                    exponentPart = exponentPart * 10 + digit;
                    count++;
                    nextByte = json._values[count];
                }

                result *= (Math.Pow(10, isExpNegative ? exponentPart * -1 : exponentPart));
            }

            if (count - location > length) {
                throw new InvalidCastException();
            }

            return isNegative ? result * -1 : result;

        }

        internal int Location => _db.Read<int>();
        internal int Length => _db.Slice(4).Read<int>();
        public JsonValueType Type => _db.Slice(8).Read<JsonValueType>();
        internal bool IsSimpleValue {
            get {
                var type = Type;
                return type != JsonValueType.Object && type != JsonValueType.Array;
            }
        }

        public enum JsonValueType : byte
        {
            String = 0,
            Number = 1,
            Object = 2,
            Array  = 3,
            True   = 4,
            False  = 5,
            Null   = 6
        }

        public bool HasValue()
        {
            var typeCode = Type;

            if (typeCode == JsonValueType.Object || typeCode == JsonValueType.Array) {
                int length = Length;
                if (length == 0) return false;
                return true;
            } else {
                int location = Location;
                if (_values[location - 1] == '"' && _values[location + 4] == '"') {
                    return true;
                }
                return (_values[location] != 'n' || _values[location + 1] != 'u' || _values[location + 2] != 'l' || _values[location + 3] != 'l');
            }
        }

        public void Dispose()
        {
            if (_pool == null) throw new InvalidOperationException("only root object can (and should) be disposed.");
            _db = ReadOnlySpan<byte>.Empty;
            _values = ReadOnlySpan<byte>.Empty;
            _pool.Return(_dbMemory);
            _dbMemory = Memory<byte>.Empty;
        }
    }
}
