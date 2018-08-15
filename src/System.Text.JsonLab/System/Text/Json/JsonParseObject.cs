// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Text.Utf8;

using static System.Runtime.InteropServices.MemoryMarshal;

namespace System.Text.JsonLab
{
    public ref struct JsonObject
    {
        private Span<byte> _db;
        private ReadOnlySpan<byte> _values;

        public string PrintDatabase()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(nameof(Record.Location) + "\t" + nameof(Record.Length) + "\t" + nameof(Record.Type) + "\r\n");
            for (int i = 0; i < _db.Length; i += DbRow.Size)
            {
                DbRow record = Read<DbRow>(_db.Slice(i));
                sb.Append(record.Location + "\t" + record.Length + "\t" + record.Type + "\r\n");
            }
            return sb.ToString();
        }

        public string PrintJson()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _db.Length; i += DbRow.Size)
            {
                DbRow record = Read<DbRow>(_db.Slice(i));
                if (record.IsSimpleValue)
                {
                    ReadOnlySpan<byte> value = _values.Slice(record.Location, record.Length);
                    sb.Append(Encoding.UTF8.GetString(value.ToArray())).Append(", ");
                }
            }
            return sb.ToString();
        }

        public static JsonObject Parse(ReadOnlySpan<byte> utf8Json, MemoryPool<byte> pool = null)
        {
            var parser = new JsonParser();
            var result = parser.Parse(utf8Json, pool);
            return result;
        }

        internal JsonObject(ReadOnlySpan<byte> values, Span<byte> db)
        {
            _db = db;
            _values = values;
        }

        public bool TryGetChild(out JsonObject value)
        {
            DbRow record = Record;

            if (record.Length == 0)
            {
                value = default;
                return false;
            }
            if (record.Type != JsonValueType.Object)
            {
                JsonThrowHelper.ThrowInvalidOperationException();
            }

            record = Read<DbRow>(_db.Slice(DbRow.Size));

            int newStart = DbRow.Size + DbRow.Size;
            int newEnd = newStart + DbRow.Size;

            record = Read<DbRow>(_db.Slice(newStart));

            if (!record.IsSimpleValue)
            {
                newEnd = newEnd + DbRow.Size * record.Length;
            }

            value = new JsonObject(_values, _db.Slice(newStart, newEnd - newStart));
            return true;
        }

        public void Remove(JsonObject obj)
        {
            ReadOnlySpan<byte> values = obj._values;
            Span<byte> db = obj._db;

            DbRow objRecord = Read<DbRow>(db);
            int reduce = objRecord.Length;

            int idx = _db.IndexOf(db);

            int sliceVal = idx + db.Length;

            if (idx >= DbRow.Size)
            {
                reduce++;
                reduce++;
                idx -= DbRow.Size;
            }

            Span<byte> temp = _db.Slice(0, idx);

            for (int i = 0; i < temp.Length; i += DbRow.Size)
            {
                DbRow record = Read<DbRow>(temp.Slice(i));
                if (!record.IsSimpleValue)
                {
                    DbRow myObj = Read<DbRow>(_db.Slice(i + record.Length * DbRow.Size));
                    if (myObj.Location > objRecord.Location)
                    {
                        DbRow fixup = new DbRow(record.Type, record.Location, record.Length - reduce);
                        Write(temp.Slice(i), ref fixup);
                    }
                }
            }

            _db.Slice(sliceVal).CopyTo(_db.Slice(idx));
            _db = _db.Slice(0, _db.Length - db.Length - DbRow.Size);
        }

        public bool TryGetValue(Utf8Span propertyName, out JsonObject value)
        {
            DbRow record = Record;

            if (record.Length == 0)
            {
                JsonThrowHelper.ThrowKeyNotFoundException();
            }
            if (record.Type != JsonValueType.Object)
            {
                JsonThrowHelper.ThrowInvalidOperationException();
            }

            for (int i = DbRow.Size; i <= _db.Length; i += DbRow.Size)
            {
                record = Read<DbRow>(_db.Slice(i));

                if (!record.IsSimpleValue)
                {
                    i += record.Length * DbRow.Size;
                    continue;
                }

                if (new Utf8Span(_values.Slice(record.Location, record.Length)) == propertyName)
                {
                    int newStart = i + DbRow.Size;

                    record = Read<DbRow>(_db.Slice(newStart));

                    int newEnd = record.IsSimpleValue ? i + 2 * DbRow.Size : i + (record.Length + 2) * DbRow.Size;

                    value = new JsonObject(_values, _db.Slice(newStart, newEnd - newStart));
                    return true;
                }

                JsonValueType valueType = Read<JsonValueType>(_db.Slice(i + DbRow.Size + 8));
                if (valueType != JsonValueType.Object && valueType != JsonValueType.Array)
                {
                    i += DbRow.Size;
                }
            }

            value = default;
            return false;
        }

        public bool TryGetValue(string propertyName, out JsonObject value)
        {
            var record = Record;

            if (record.Length == 0)
            {
                JsonThrowHelper.ThrowKeyNotFoundException();
            }

            if (record.Type != JsonValueType.Object)
            {
                JsonThrowHelper.ThrowInvalidOperationException();
            }

            for (int i = DbRow.Size; i < _db.Length; i += DbRow.Size)
            {
                record = Read<DbRow>(_db.Slice(i));

                if (!record.IsSimpleValue)
                {
                    i += record.Length * DbRow.Size;
                    continue;
                }

                if (new Utf8Span(_values.Slice(record.Location, record.Length)) == propertyName)
                {
                    int newStart = i + DbRow.Size;
                    int newEnd = newStart + DbRow.Size;

                    record = Read<DbRow>(_db.Slice(newStart));

                    if (!record.IsSimpleValue)
                    {
                        newEnd = newEnd + DbRow.Size * record.Length;
                    }

                    value = new JsonObject(_values, _db.Slice(newStart, newEnd - newStart));
                    return true;
                }

                var valueType = Read<JsonValueType>(_db.Slice(i + DbRow.Size + 8));
                if (valueType != JsonValueType.Object && valueType != JsonValueType.Array)
                {
                    i += DbRow.Size;
                }
            }

            value = default;
            return false;
        }

        public JsonObject this[Utf8Span name]
        {
            get
            {
                if (TryGetValue(name, out JsonObject value))
                {
                    return value;
                }
                JsonThrowHelper.ThrowKeyNotFoundException();
                return default;
            }
        }

        public JsonObject this[string name]
        {
            get
            {
                if (TryGetValue(name, out JsonObject value))
                {
                    return value;
                }
                JsonThrowHelper.ThrowKeyNotFoundException();
                return default;
            }
        }

        public JsonObject this[int index]
        {
            get
            {
                DbRow record = Record;

                if ((uint)index >= (uint)record.Length)
                {
                    JsonThrowHelper.ThrowIndexOutOfRangeException();
                    return default;
                }

                if (record.Type != JsonValueType.Array)
                {
                    JsonThrowHelper.ThrowInvalidOperationException();
                    return default;
                }

                int counter = 0;
                for (int i = DbRow.Size; i <= _db.Length; i += DbRow.Size)
                {
                    record = Read<DbRow>(_db.Slice(i));

                    if (index == counter)
                    {
                        int newStart = i;
                        int newEnd = i + DbRow.Size;

                        if (!record.IsSimpleValue)
                        {
                            newEnd = newEnd + DbRow.Size * record.Length;
                        }
                        return new JsonObject(_values, _db.Slice(newStart, newEnd - newStart));
                    }

                    if (!record.IsSimpleValue)
                    {
                        i += record.Length * DbRow.Size;
                    }

                    counter++;
                }

                JsonThrowHelper.ThrowIndexOutOfRangeException();
                return default;
            }
        }

        public int ArrayLength
        {
            get
            {
                var record = Record;
                if (record.Type != JsonValueType.Array)
                {
                    JsonThrowHelper.ThrowInvalidOperationException();
                }
                return record.Length;
            }
        }

        public static explicit operator string(JsonObject json)
        {
            return ((Utf8Span)json).ToString();
        }

        public static explicit operator Utf8Span(JsonObject json)
        {
            DbRow record = json.Record;
            if (!record.IsSimpleValue)
            {
                JsonThrowHelper.ThrowInvalidCastException();
            }

            return new Utf8Span(json._values.Slice(record.Location, record.Length));
        }

        public static explicit operator Utf8String(JsonObject json)
        {
            DbRow record = json.Record;
            if (!record.IsSimpleValue)
            {
                JsonThrowHelper.ThrowInvalidCastException();
            }

            return new Utf8String(json._values.Slice(record.Location, record.Length));
        }

        public static explicit operator bool(JsonObject json)
        {
            var record = json.Record;
            if (!record.IsSimpleValue)
            {
                JsonThrowHelper.ThrowInvalidCastException();
            }

            if (record.Length < 4 || record.Length > 5)
            {
                JsonThrowHelper.ThrowInvalidCastException();
            }

            var slice = json._values.Slice(record.Location);

            if (!Utf8Parser.TryParse(slice, out bool result, out _))
            {
                JsonThrowHelper.ThrowInvalidCastException();
            }
            return result;
        }

        public static explicit operator int(JsonObject json)
        {
            var record = json.Record;
            if (!record.IsSimpleValue)
            {
                JsonThrowHelper.ThrowInvalidCastException();
            }

            var slice = json._values.Slice(record.Location);

            if (!Utf8Parser.TryParse(slice, out int result, out _))
            {
                JsonThrowHelper.ThrowInvalidCastException();
            }
            return result;
        }

        public static explicit operator double(JsonObject json)
        {
            var record = json.Record;
            if (!record.IsSimpleValue)
            {
                JsonThrowHelper.ThrowInvalidCastException();
            }

            int count = record.Location;
            bool isNegative = false;
            var nextByte = json._values[count];
            if (nextByte == '-')
            {
                isNegative = true;
                count++;
                nextByte = json._values[count];
            }

            if (nextByte < '0' || nextByte > '9' || count - record.Location >= record.Length)
            {
                JsonThrowHelper.ThrowInvalidCastException();
            }

            int integerPart = 0;
            while (nextByte >= '0' && nextByte <= '9' && count - record.Location < record.Length)
            {
                int digit = nextByte - '0';
                integerPart = integerPart * 10 + digit;
                count++;
                nextByte = json._values[count];
            }

            double result = integerPart;

            int decimalPart = 0;
            if (nextByte == '.')
            {
                count++;
                int numberOfDigits = count;
                nextByte = json._values[count];
                while (nextByte >= '0' && nextByte <= '9' && count - record.Location < record.Length)
                {
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
            if (nextByte == 'e' || nextByte == 'E')
            {
                count++;
                nextByte = json._values[count];
                if (nextByte == '-' || nextByte == '+')
                {
                    if (nextByte == '-')
                    {
                        isExpNegative = true;
                    }
                    count++;
                }
                nextByte = json._values[count];
                while (nextByte >= '0' && nextByte <= '9' && count - record.Location < record.Location)
                {
                    int digit = nextByte - '0';
                    exponentPart = exponentPart * 10 + digit;
                    count++;
                    nextByte = json._values[count];
                }

                result *= (Math.Pow(10, isExpNegative ? exponentPart * -1 : exponentPart));
            }

            if (count - record.Location > record.Length)
            {
                JsonThrowHelper.ThrowInvalidCastException();
            }

            return isNegative ? result * -1 : result;

        }

        private DbRow Record => GetRecord(0);
        private int Location => GetLocation(0);
        private int Length => GetLength(0);

        private DbRow GetRecord(int index) => index == 0 ? Read<DbRow>(_db) : Read<DbRow>(_db.Slice(index));
        private int GetLocation(int index) => index == 0 ? Read<int>(_db) : Read<int>(_db.Slice(index));
        private int GetLength(int index) => index == 0 ? Read<int>(_db.Slice(4)) : Read<int>(_db.Slice(index + 4));
        private JsonValueType GetType(int index) => index == 0 ? Read<JsonValueType>(_db.Slice(8)) : Read<JsonValueType>(_db.Slice(index + 8));

        public JsonValueType Type => GetType(0);

        // Do not change the order of the enum values, since IsSimpleValue relies on it.
        public enum JsonValueType : byte
        {
            Object = 0,
            Array = 1,
            String = 2,
            Number = 3,
            True = 4,
            False = 5,
            Null = 6
        }

        public bool HasValue()
        {
            DbRow record = Record;

            if (!record.IsSimpleValue)
            {
                return record.Length != 0;
            }
            else
            {
                if (_values[record.Location - 1] == '"' && _values[record.Location + 4] == '"')
                {
                    return true;
                }
                return (_values[record.Location] != 'n' || _values[record.Location + 1] != 'u' || _values[record.Location + 2] != 'l' || _values[record.Location + 3] != 'l');
            }
        }

        public void Dispose()
        {
            //if (_pool == null) throw new InvalidOperationException("only root object can (and should) be disposed.");
            _db = Span<byte>.Empty;
            _values = ReadOnlySpan<byte>.Empty;
        }
    }
}
