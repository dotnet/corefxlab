// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Text.Utf8;

using static System.Buffers.Binary.BinaryPrimitives;

namespace System.Text.Json
{
    public ref struct JsonObject
    {
        private BufferPool _pool;
        private OwnedMemory<byte> _dbMemory;
        private ReadOnlySpan<byte> _db; 
        private ReadOnlySpan<byte> _values;

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

        internal JsonObject(ReadOnlySpan<byte> values, ReadOnlySpan<byte> db, BufferPool pool = null, OwnedMemory<byte> dbMemory = null)
        {
            _db = db;
            _values = values;
            _pool = pool;
            _dbMemory = dbMemory;
        }

        public bool TryGetValue(Utf8Span propertyName, out JsonObject value)
        {
            var record = Record;

            if (record.Length == 0) {
                throw new KeyNotFoundException();
            }
            if (record.Type != JsonValueType.Object) {
                throw new InvalidOperationException();
            }

            for (int i = DbRow.Size; i <= _db.Length; i += DbRow.Size) {
                record = ReadMachineEndian<DbRow>(_db.Slice(i));

                if (!record.IsSimpleValue) {
                    i += record.Length * DbRow.Size;
                    continue;
                }

                if (new Utf8Span(_values.Slice(record.Location, record.Length)) == propertyName) {
                    int newStart = i + DbRow.Size;
                    int newEnd = newStart + DbRow.Size;

                    record = ReadMachineEndian<DbRow>(_db.Slice(newStart));

                    if (!record.IsSimpleValue) {
                        newEnd = newEnd + DbRow.Size * record.Length;
                    }

                    value = new JsonObject(_values, _db.Slice(newStart, newEnd - newStart));
                    return true;
                }

                var valueType = ReadMachineEndian<JsonValueType>(_db.Slice(i + DbRow.Size + 8));
                if (valueType != JsonValueType.Object && valueType != JsonValueType.Array) {
                    i += DbRow.Size;
                }
            }

            value = default;
            return false;
        }

        public bool TryGetValue(string propertyName, out JsonObject value)
        {
            var record = Record;
            
            if (record.Length == 0) {
                throw new KeyNotFoundException();
            }

            if (record.Type != JsonValueType.Object) {
                throw new InvalidOperationException();
            }

            for (int i = DbRow.Size; i <= _db.Length; i += DbRow.Size) {
                record = ReadMachineEndian<DbRow>(_db.Slice(i));

                if (!record.IsSimpleValue) {
                    i += record.Length * DbRow.Size;
                    continue;
                }

                if (new Utf8Span(_values.Slice(record.Location, record.Length)) == propertyName) {
                    int newStart = i + DbRow.Size;
                    int newEnd = newStart + DbRow.Size;

                    record = ReadMachineEndian<DbRow>(_db.Slice(newStart));

                    if (!record.IsSimpleValue) {
                        newEnd = newEnd + DbRow.Size * record.Length;
                    }

                    value = new JsonObject(_values, _db.Slice(newStart, newEnd - newStart));
                    return true;
                }

                var valueType = ReadMachineEndian<JsonValueType>(_db.Slice(i + DbRow.Size + 8));
                if (valueType != JsonValueType.Object && valueType != JsonValueType.Array) {
                    i += DbRow.Size;
                }
            }

            value = default;
            return false;
        }

        public JsonObject this[Utf8Span name]
        {
            get {
                if (TryGetValue(name, out JsonObject value))
                {
                    return value;
                }
                throw new KeyNotFoundException();
            }
        }

        public JsonObject this[string name] {
            get {
                if (TryGetValue(name, out JsonObject value))
                {
                    return value;
                }
                throw new KeyNotFoundException();
            }
        }

        public JsonObject this[int index] {
            get {
                var record = Record;

                if (index < 0 || index >= record.Length) {
                    throw new IndexOutOfRangeException();
                }

                if (record.Type != JsonValueType.Array) {
                    throw new InvalidOperationException();
                }

                int counter = 0;
                for (int i = DbRow.Size; i <= _db.Length; i += DbRow.Size) {
                    record = ReadMachineEndian<DbRow>(_db.Slice(i));

                    if (index == counter) {
                        int newStart = i;
                        int newEnd = i + DbRow.Size;

                        if (!record.IsSimpleValue) {
                            newEnd = newEnd + DbRow.Size * record.Length;
                        }
                        return new JsonObject(_values, _db.Slice(newStart, newEnd - newStart));
                    }

                    if (!record.IsSimpleValue) {
                        i += record.Length * DbRow.Size;
                    }

                    counter++;
                }

                throw new IndexOutOfRangeException();
            }
        }

        public int ArrayLength
        {
            get
            {
                var record = Record;
                if (record.Type != JsonValueType.Array)
                {
                    throw new InvalidOperationException();
                }
                return record.Length; 
            }
        }

        public static explicit operator string(JsonObject json)
        {
            var utf8 = (Utf8Span)json;
            return utf8.ToString();
        }

        public static explicit operator Utf8Span(JsonObject json)
        {
            var record = json.Record;
            if (!record.IsSimpleValue) {
                throw new InvalidCastException();
            }

            return new Utf8Span(json._values.Slice(record.Location, record.Length));
        }

        public static explicit operator bool(JsonObject json)
        {
            var record = json.Record;
            if (!record.IsSimpleValue) {
                throw new InvalidCastException();
            }

            if (record.Length < 4 || record.Length > 5) {
                throw new InvalidCastException();
            }

            var slice = json._values.Slice(record.Location);

            if (!Utf8Parser.TryParse(slice, out bool result, out _))
            {
                throw new InvalidCastException();
            }
            return result;
        }

        public static explicit operator int(JsonObject json)
        {
            var record = json.Record;
            if (!record.IsSimpleValue) {
                throw new InvalidCastException();
            }

            var slice = json._values.Slice(record.Location);

            if (!Utf8Parser.TryParse(slice, out int result, out _))
            {
                throw new InvalidCastException();
            }
            return result;
        }

        public static explicit operator double(JsonObject json)
        {
            var record = json.Record;
            if (!record.IsSimpleValue) {
                throw new InvalidCastException();
            }

            int count = record.Location;
            bool isNegative = false;
            var nextByte = json._values[count];
            if (nextByte == '-') {
                isNegative = true;
                count++;
                nextByte = json._values[count];
            }

            if (nextByte < '0' || nextByte > '9' || count - record.Location >= record.Length) {
                throw new InvalidCastException();
            }

            int integerPart = 0;
            while (nextByte >= '0' && nextByte <= '9' && count - record.Location < record.Length) {
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
                while (nextByte >= '0' && nextByte <= '9' && count - record.Location < record.Length) {
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
                while (nextByte >= '0' && nextByte <= '9' && count - record.Location < record.Location) {
                    int digit = nextByte - '0';
                    exponentPart = exponentPart * 10 + digit;
                    count++;
                    nextByte = json._values[count];
                }

                result *= (Math.Pow(10, isExpNegative ? exponentPart * -1 : exponentPart));
            }

            if (count - record.Location > record.Length) {
                throw new InvalidCastException();
            }

            return isNegative ? result * -1 : result;

        }

        internal DbRow Record => ReadMachineEndian<DbRow>(_db);
        public JsonValueType Type => ReadMachineEndian<JsonValueType>(_db.Slice(8));

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
            var record = Record;

            if (!record.IsSimpleValue) {
                if (record.Length == 0) return false;
                return true;
            } else {
                if (_values[record.Location - 1] == '"' && _values[record.Location + 4] == '"') {
                    return true;
                }
                return (_values[record.Location] != 'n' || _values[record.Location + 1] != 'u' || _values[record.Location + 2] != 'l' || _values[record.Location + 3] != 'l');
            }
        }

        public void Dispose()
        {
            if (_pool == null) throw new InvalidOperationException("only root object can (and should) be disposed.");
            _db = ReadOnlySpan<byte>.Empty;
            _values = ReadOnlySpan<byte>.Empty;
            _dbMemory.Dispose();
            _dbMemory = null;
        }
    }
}
