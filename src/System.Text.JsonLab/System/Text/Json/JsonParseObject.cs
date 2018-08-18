// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers.Text;
using System.Runtime.InteropServices;
using System.Text.Utf8;

namespace System.Text.JsonLab
{
    public ref struct JsonObject
    {
        private Span<byte> _database;
        private ReadOnlySpan<byte> _jsonData;

        internal string PrintDatabase()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(nameof(DbRow.Location) + "\t" + nameof(DbRow.Length) + "\t" + nameof(DbRow.Type) + "\r\n");
            for (int i = 0; i < _database.Length; i += DbRow.Size)
            {
                DbRow record = MemoryMarshal.Read<DbRow>(_database.Slice(i));
                sb.Append(record.Location + "\t" + record.Length + "\t" + record.Type + "\r\n");
            }
            return sb.ToString();
        }

        public string PrintJson()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _database.Length; i += DbRow.Size)
            {
                DbRow record = MemoryMarshal.Read<DbRow>(_database.Slice(i));
                // Special casing Null so that it matches what JSON.NET does
                if (record.IsSimpleValue && record.Type != JsonValueType.Null)
                {
                    ReadOnlySpan<byte> value = _jsonData.Slice(record.Location, record.Length);
                    sb.Append(Encoding.UTF8.GetString(value.ToArray())).Append(", ");
                }
            }
            return sb.ToString();
        }

        internal JsonObject(ReadOnlySpan<byte> jsonData, Span<byte> database)
        {
            _database = database;
            _jsonData = jsonData;
        }

        public bool TryGetChild(out JsonObject value)
        {
            DbRow record = GetRecord(0);

            if (record.Length == 0)
            {
                value = default;
                return false;
            }
            if (record.Type != JsonValueType.Object)
            {
                JsonThrowHelper.ThrowInvalidOperationException();
            }

            record = MemoryMarshal.Read<DbRow>(_database.Slice(DbRow.Size));

            int newStart = DbRow.Size + DbRow.Size;
            int newEnd = newStart + DbRow.Size;

            record = MemoryMarshal.Read<DbRow>(_database.Slice(newStart));

            if (!record.IsSimpleValue)
            {
                newEnd = newEnd + DbRow.Size * record.Length;
            }

            value = new JsonObject(_jsonData, _database.Slice(newStart, newEnd - newStart));
            return true;
        }

        public void Remove(JsonObject obj)
        {
            ReadOnlySpan<byte> values = obj._jsonData;
            Span<byte> db = obj._database;

            DbRow objRecord = MemoryMarshal.Read<DbRow>(db);
            int reduce = objRecord.Length;

            int idx = _database.IndexOf(db);

            int sliceVal = idx + db.Length;

            if (idx >= DbRow.Size)
            {
                reduce++;
                reduce++;
                idx -= DbRow.Size;
            }

            Span<byte> temp = _database.Slice(0, idx);

            for (int i = 0; i < temp.Length; i += DbRow.Size)
            {
                DbRow record = MemoryMarshal.Read<DbRow>(temp.Slice(i));
                if (!record.IsSimpleValue)
                {
                    DbRow myObj = MemoryMarshal.Read<DbRow>(_database.Slice(i + record.Length * DbRow.Size));
                    if (myObj.Location > objRecord.Location)
                    {
                        DbRow fixup = new DbRow(record.Type, record.Location, record.Length - reduce);
                        MemoryMarshal.Write(temp.Slice(i), ref fixup);
                    }
                }
            }

            _database.Slice(sliceVal).CopyTo(_database.Slice(idx));
            _database = _database.Slice(0, _database.Length - db.Length - DbRow.Size);
        }

        public bool TryGetValue(Utf8Span propertyName, out JsonObject value)
        {
            DbRow record = GetRecord(0);

            if (record.Length == 0)
            {
                JsonThrowHelper.ThrowKeyNotFoundException();
            }
            if (record.Type != JsonValueType.Object)
            {
                JsonThrowHelper.ThrowInvalidOperationException();
            }

            for (int i = DbRow.Size; i <= _database.Length; i += DbRow.Size)
            {
                record = GetRecord(i);

                if (!record.IsSimpleValue)
                {
                    i += record.Length * DbRow.Size;
                    continue;
                }

                if (_jsonData.Slice(record.Location, record.Length).SequenceEqual(propertyName.Bytes))
                {
                    int newStart = i + DbRow.Size;

                    record = GetRecord(newStart);

                    int newEnd = record.IsSimpleValue ? i + 2 * DbRow.Size : i + (record.Length + 2) * DbRow.Size;

                    value = new JsonObject(_jsonData, _database.Slice(newStart, newEnd - newStart));
                    return true;
                }

                JsonValueType valueType = GetType(i + DbRow.Size);
                if (valueType > JsonValueType.Array)
                {
                    i += DbRow.Size;
                }
            }

            value = default;
            return false;
        }

        public bool TryGetValue(string propertyName, out JsonObject value)
        {
            var record = GetRecord(0);

            if (record.Length == 0)
            {
                JsonThrowHelper.ThrowKeyNotFoundException();
            }

            if (record.Type != JsonValueType.Object)
            {
                JsonThrowHelper.ThrowInvalidOperationException();
            }

            for (int i = DbRow.Size; i < _database.Length; i += DbRow.Size)
            {
                record = MemoryMarshal.Read<DbRow>(_database.Slice(i));

                if (!record.IsSimpleValue)
                {
                    i += record.Length * DbRow.Size;
                    continue;
                }

                if (new Utf8Span(_jsonData.Slice(record.Location, record.Length)) == propertyName)
                {
                    int newStart = i + DbRow.Size;
                    int newEnd = newStart + DbRow.Size;

                    record = MemoryMarshal.Read<DbRow>(_database.Slice(newStart));

                    if (!record.IsSimpleValue)
                    {
                        newEnd = newEnd + DbRow.Size * record.Length;
                    }

                    value = new JsonObject(_jsonData, _database.Slice(newStart, newEnd - newStart));
                    return true;
                }

                var valueType = MemoryMarshal.Read<JsonValueType>(_database.Slice(i + DbRow.Size + 8));
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
                DbRow record = GetRecord(0);

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
                for (int i = DbRow.Size; i <= _database.Length; i += DbRow.Size)
                {
                    record = MemoryMarshal.Read<DbRow>(_database.Slice(i));

                    if (index == counter)
                    {
                        int newStart = i;
                        int newEnd = i + DbRow.Size;

                        if (!record.IsSimpleValue)
                        {
                            newEnd = newEnd + DbRow.Size * record.Length;
                        }
                        return new JsonObject(_jsonData, _database.Slice(newStart, newEnd - newStart));
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
                var record = GetRecord(0);
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
            DbRow record = json.GetRecord(0);
            if (!record.IsSimpleValue)
            {
                JsonThrowHelper.ThrowInvalidCastException();
            }

            return new Utf8Span(json._jsonData.Slice(record.Location, record.Length));
        }

        public static explicit operator Utf8String(JsonObject json)
        {
            DbRow record = json.GetRecord(0);
            if (!record.IsSimpleValue)
            {
                JsonThrowHelper.ThrowInvalidCastException();
            }

            return new Utf8String(json._jsonData.Slice(record.Location, record.Length));
        }

        public static explicit operator bool(JsonObject json)
        {
            var record = json.GetRecord(0);
            if (!record.IsSimpleValue)
            {
                JsonThrowHelper.ThrowInvalidCastException();
            }

            if (record.Length < 4 || record.Length > 5)
            {
                JsonThrowHelper.ThrowInvalidCastException();
            }

            var slice = json._jsonData.Slice(record.Location);

            if (!Utf8Parser.TryParse(slice, out bool result, out _))
            {
                JsonThrowHelper.ThrowInvalidCastException();
            }
            return result;
        }

        public static explicit operator int(JsonObject json)
        {
            var record = json.GetRecord(0);
            if (!record.IsSimpleValue)
            {
                JsonThrowHelper.ThrowInvalidCastException();
            }

            var slice = json._jsonData.Slice(record.Location);

            if (!Utf8Parser.TryParse(slice, out int result, out _))
            {
                JsonThrowHelper.ThrowInvalidCastException();
            }
            return result;
        }

        public static explicit operator double(JsonObject json)
        {
            var record = json.GetRecord(0);
            if (!record.IsSimpleValue)
            {
                JsonThrowHelper.ThrowInvalidCastException();
            }

            int count = record.Location;
            bool isNegative = false;
            var nextByte = json._jsonData[count];
            if (nextByte == '-')
            {
                isNegative = true;
                count++;
                nextByte = json._jsonData[count];
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
                nextByte = json._jsonData[count];
            }

            double result = integerPart;

            int decimalPart = 0;
            if (nextByte == '.')
            {
                count++;
                int numberOfDigits = count;
                nextByte = json._jsonData[count];
                while (nextByte >= '0' && nextByte <= '9' && count - record.Location < record.Length)
                {
                    int digit = nextByte - '0';
                    decimalPart = decimalPart * 10 + digit;
                    count++;
                    nextByte = json._jsonData[count];
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
                nextByte = json._jsonData[count];
                if (nextByte == '-' || nextByte == '+')
                {
                    if (nextByte == '-')
                    {
                        isExpNegative = true;
                    }
                    count++;
                }
                nextByte = json._jsonData[count];
                while (nextByte >= '0' && nextByte <= '9' && count - record.Location < record.Location)
                {
                    int digit = nextByte - '0';
                    exponentPart = exponentPart * 10 + digit;
                    count++;
                    nextByte = json._jsonData[count];
                }

                result *= (Math.Pow(10, isExpNegative ? exponentPart * -1 : exponentPart));
            }

            if (count - record.Location > record.Length)
            {
                JsonThrowHelper.ThrowInvalidCastException();
            }

            return isNegative ? result * -1 : result;

        }

        private DbRow GetRecord(int index) => index == 0
            ? MemoryMarshal.Read<DbRow>(_database)
            : MemoryMarshal.Read<DbRow>(_database.Slice(index));

        private int GetLocation(int index) => index == 0
            ? MemoryMarshal.Read<int>(_database)
            : MemoryMarshal.Read<int>(_database.Slice(index));

        private int GetLength(int index) => index == 0
            ? MemoryMarshal.Read<int>(_database.Slice(4))
            : MemoryMarshal.Read<int>(_database.Slice(index + 4));

        private JsonValueType GetType(int index) => index == 0
            ? MemoryMarshal.Read<JsonValueType>(_database.Slice(8))
            : MemoryMarshal.Read<JsonValueType>(_database.Slice(index + 8));

        public JsonValueType Type => GetType(0);

        public bool HasValue()
        {
            DbRow record = GetRecord(0);

            if (!record.IsSimpleValue)
            {
                return record.Length != 0;
            }
            else
            {
                if (_jsonData[record.Location - 1] == '"' && _jsonData[record.Location + 4] == '"')
                {
                    return true;
                }
                return (_jsonData[record.Location] != 'n' || _jsonData[record.Location + 1] != 'u' || _jsonData[record.Location + 2] != 'l' || _jsonData[record.Location + 3] != 'l');
            }
        }

        public void Dispose()
        {
            //if (_pool == null) throw new InvalidOperationException("only root object can (and should) be disposed.");
            _database = Span<byte>.Empty;
            _jsonData = ReadOnlySpan<byte>.Empty;
        }
    }
}
