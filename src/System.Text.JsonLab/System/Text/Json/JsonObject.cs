// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Buffers.Text;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Utf8;

namespace System.Text.JsonLab
{
    public ref struct JsonObject
    {
        private CustomDb _database;
        private ReadOnlySpan<byte> _jsonData;

        public string PrintDatabase() => _database.PrintDatabase();

        public string PrintJson()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _database.Length; i += DbRow.Size)
            {
                DbRow record = _database.Get(i);
                // Special casing Null so that it matches what JSON.NET does
                if (record.IsSimpleValue && record.JsonType != JsonValueType.Null)
                {
                    ReadOnlySpan<byte> value = _jsonData.Slice(record.Location, record.SizeOrLength);
                    sb.Append(Encoding.UTF8.GetString(value.ToArray())).Append(", ");
                }
            }
            return sb.ToString();
        }

        public static JsonObject Parse(ReadOnlySpan<byte> utf8Json, ArrayPool<byte> pool = null)
        {
            var parser = new JsonParser(utf8Json, pool);
            return parser.Parse();
        }

        internal JsonObject(ReadOnlySpan<byte> jsonData, CustomDb database)
        {
            _database = database;
            _jsonData = jsonData;
        }

        public bool TryGetChild(out JsonObject value)
        {
            DbRow record = _database.Get();

            if (record.SizeOrLength == 0)
            {
                value = default;
                return false;
            }
            if (record.JsonType != JsonValueType.Object)
            {
                JsonThrowHelper.ThrowInvalidOperationException();
            }

            record = _database.Get(DbRow.Size);

            int newStart = DbRow.Size + DbRow.Size;
            int newEnd = newStart + DbRow.Size;

            record = _database.Get(newStart);

            if (!record.IsSimpleValue)
            {
                newEnd = newEnd + DbRow.Size * record.SizeOrLength;
            }

            CustomDb copy = _database;
            copy.Span = _database.Slice(newStart, newEnd - newStart);
            value = new JsonObject(_jsonData, copy);
            return true;
        }

        public void Remove(JsonObject obj)
        {
            ReadOnlySpan<byte> values = obj._jsonData;
            CustomDb db = obj._database;

            DbRow objRecord = db.Get();
            int reduce = objRecord.SizeOrLength;

            int idx = _database.Span.IndexOf(db.Span);

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
                    DbRow myObj = _database.Get(i + record.SizeOrLength * DbRow.Size);
                    if (myObj.Location > objRecord.Location)
                    {
                        DbRow fixup = new DbRow(record.JsonType, record.Location, record.SizeOrLength - reduce);
                        MemoryMarshal.Write(temp.Slice(i), ref fixup);
                    }
                }
            }

            _database.Slice(sliceVal).CopyTo(_database.Slice(idx));
            _database.Span = _database.Slice(0, _database.Length - db.Length - DbRow.Size);
        }

        public bool TryGetValue(Utf8Span propertyName, out JsonObject value)
        {
            DbRow record = _database.Get();

            if (record.JsonType != JsonValueType.Object)
                JsonThrowHelper.ThrowInvalidOperationException();

            for (int i = DbRow.Size; i < _database.Length; i += DbRow.Size)
            {
                record = _database.Get(i);

                if (!record.IsSimpleValue)
                {
                    if (record.SizeOrLength != 0)
                        i += DbRow.Size * record.NumberOfRows;
                    continue;
                }

                Debug.Assert(record.JsonType == JsonValueType.String);

                int startIndex = i + DbRow.Size;
                DbRow nextRecord = _database.Get(startIndex);

                if (_jsonData.Slice(record.Location, record.SizeOrLength).SequenceEqual(propertyName.Bytes))
                {
                    int length = DbRow.Size;
                    if (!nextRecord.IsSimpleValue)
                        length += DbRow.Size * nextRecord.NumberOfRows;

                    value = CreateJsonObject(startIndex, length);
                    return true;
                }

                // Skip primitive value since we are looking for keys
                if (nextRecord.IsSimpleValue)
                    i += DbRow.Size;
            }

            value = default;
            return false;
        }

        public bool TryGetValue(string propertyName, out JsonObject value)
            => TryGetValue((Utf8Span)propertyName, out value);
        
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

        public JsonObject this[string name] => this[(Utf8Span)name];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private JsonObject CreateJsonObject(int startIndex, int length)
        {
            CustomDb copy = _database;
            copy.Span = _database.Slice(startIndex, length);
            return new JsonObject(_jsonData, copy);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private JsonObject SequentialSearch(int startIndex, int length, int index, int uniformCount, int uniformValue, DbRow record)
        {
            bool uniqueFound = false;
            int counter = 0;
            for (int i = startIndex; i <= _database.Length; i += DbRow.Size)
            {
                DbRow nextRecord = _database.Get(i);

                if (!uniqueFound && uniformValue == nextRecord.NumberOfRows)
                    uniformCount++;
                else
                    uniqueFound = true;

                if (index == counter)
                {
                    if (!nextRecord.IsSimpleValue)
                    {
                        length += DbRow.Size * nextRecord.NumberOfRows;
                    }
                    /*if (uniformCount > record.SizeOrLength)
                        uniformCount = record.SizeOrLength;*/
                    _database.SetNumberOfRows(0, uniformCount);
                    return CreateJsonObject(i, length);
                }

                if (!nextRecord.IsSimpleValue)
                {
                    i += nextRecord.NumberOfRows * DbRow.Size;
                }
                counter++;
            }
            JsonThrowHelper.ThrowIndexOutOfRangeException();
            return default;
        }

        /*public JsonObject this[int index]
        {
            get
            {
                DbRow record = _database.Get();

                string temp = _database.PrintDatabase();

                if (record.JsonType != JsonValueType.Array)
                    JsonThrowHelper.ThrowInvalidOperationException();

                if ((uint)index >= (uint)record.SizeOrLength)
                    JsonThrowHelper.ThrowIndexOutOfRangeException();

                if (!record.HasChildren)
                    return CreateJsonObject(DbRow.Size * (index + 1), DbRow.Size);

                int length = DbRow.Size;
                DbRow nextRecord = _database.Get(DbRow.Size);

                if (index == 0)
                {
                    if (!nextRecord.IsSimpleValue)
                    {
                        length += DbRow.Size * nextRecord.NumberOfRows;
                    }
                    return CreateJsonObject(DbRow.Size, length);
                }

                int uniformCount = 0;
                int uniformValue = nextRecord.SizeOrLength;
                int startIndex = DbRow.Size;

                if (record.NumberOfRows <= record.SizeOrLength)
                {
                    if (index < record.NumberOfRows)
                    {
                        length = DbRow.Size * (1 + uniformValue);
                        // startIndex is equivalent to DbRow.Size * (index + 1) + (index * DbRow.Size * homogenousValue);
                        return CreateJsonObject(index * length + DbRow.Size, length);
                    }

                    startIndex = DbRow.Size * (record.NumberOfRows + record.NumberOfRows * uniformValue + 1);
                    index -= record.NumberOfRows;
                    uniformCount = record.NumberOfRows;
                }
                return SequentialSearch(startIndex, length, index, uniformCount, uniformValue, record);
            }
        }*/

        [MethodImpl(MethodImplOptions.NoInlining)]
        private JsonObject SequentialSearch(int index)
        {
            int counter = 0;
            for (int i = DbRow.Size; i <= _database.Length; i += DbRow.Size)
            {
                DbRow nextRecord = _database.Get(i);

                if (index == counter)
                {
                    int length = DbRow.Size;
                    if (!nextRecord.IsSimpleValue)
                    {
                        length += DbRow.Size * nextRecord.NumberOfRows;
                    }
                    return CreateJsonObject(i, length);
                }

                if (!nextRecord.IsSimpleValue)
                    i += nextRecord.NumberOfRows * DbRow.Size;

                counter++;
            }
            JsonThrowHelper.ThrowIndexOutOfRangeException();
            return default;
        }

        public JsonObject this[int index]
        {
            get
            {
                DbRow record = _database.Get();

                if (record.JsonType != JsonValueType.Array)
                    JsonThrowHelper.ThrowInvalidOperationException();

                if ((uint)index >= (uint)record.SizeOrLength)
                    JsonThrowHelper.ThrowIndexOutOfRangeException();

                if (!record.HasChildren)
                    return CreateJsonObject(DbRow.Size * (index + 1), DbRow.Size);

                return SequentialSearch(index);
            }
        }

        public int ArrayLength
        {
            get
            {
                DbRow record = _database.Get();
                if (record.JsonType != JsonValueType.Array)
                {
                    JsonThrowHelper.ThrowInvalidOperationException();
                }
                return record.SizeOrLength;
            }
        }

        public static explicit operator string(JsonObject json)
        {
            return ((Utf8Span)json).ToString();
        }

        public static explicit operator Utf8Span(JsonObject json)
        {
            DbRow record = json._database.Get();
            if (!record.IsSimpleValue)
            {
                JsonThrowHelper.ThrowInvalidCastException();
            }

            return new Utf8Span(json._jsonData.Slice(record.Location, record.SizeOrLength));
        }

        public static explicit operator Utf8String(JsonObject json)
        {
            DbRow record = json._database.Get();
            if (!record.IsSimpleValue)
            {
                JsonThrowHelper.ThrowInvalidCastException();
            }

            return new Utf8String(json._jsonData.Slice(record.Location, record.SizeOrLength));
        }

        public static explicit operator bool(JsonObject json)
        {
            DbRow record = json._database.Get();
            if (!record.IsSimpleValue)
            {
                JsonThrowHelper.ThrowInvalidCastException();
            }

            if (record.SizeOrLength < 4 || record.SizeOrLength > 5)
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
            DbRow record = json._database.Get();
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
            DbRow record = json._database.Get();
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

            if (nextByte < '0' || nextByte > '9' || count - record.Location >= record.SizeOrLength)
            {
                JsonThrowHelper.ThrowInvalidCastException();
            }

            int integerPart = 0;
            while (nextByte >= '0' && nextByte <= '9' && count - record.Location < record.SizeOrLength)
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
                while (nextByte >= '0' && nextByte <= '9' && count - record.Location < record.SizeOrLength)
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

            if (count - record.Location > record.SizeOrLength)
            {
                JsonThrowHelper.ThrowInvalidCastException();
            }

            return isNegative ? result * -1 : result;

        }

        public JsonValueType Type => _database.GetJsonType();

        public bool HasValue()
        {
            DbRow record = _database.Get();

            if (!record.IsSimpleValue)
            {
                return record.SizeOrLength != 0;
            }
            else
            {
                if (record.SizeOrLength != 4)
                    return true;
                return (_jsonData[record.Location] != 'n' || _jsonData[record.Location + 1] != 'u' || _jsonData[record.Location + 2] != 'l' || _jsonData[record.Location + 3] != 'l');
            }
        }

        public void Dispose()
        {
            _database.Dispose();
            _jsonData = ReadOnlySpan<byte>.Empty;
        }
    }
}
