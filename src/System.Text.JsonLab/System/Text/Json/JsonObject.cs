// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
                if (record.IsSimpleValue && record.JsonType != JsonType.Null)
                {
                    ReadOnlySpan<byte> value = _jsonData.Slice(record.Location, record.SizeOrLength);
                    sb.Append(Encoding.UTF8.GetString(value.ToArray(), 0, value.Length)).Append(", ");
                }
            }
            return sb.ToString();
        }

        public static JsonObject Parse(ReadOnlySpan<byte> utf8Json, ArrayPool<byte> pool = null)
        {
            var parser = new JsonParser(utf8Json, pool);
            return parser.Parse();
        }

        internal JsonObject(ReadOnlySpan<byte> jsonData, CustomDb database, ReadOnlySpan<byte> name = default)
        {
            _database = database;
            _jsonData = jsonData;
            PropertyName = name;
        }

        public bool TryGetChild(out JsonObject value)
        {
            DbRow record = _database.Get();

            if (record.SizeOrLength == 0)
            {
                value = default;
                return false;
            }
            if (record.JsonType != JsonType.StartObject)
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

            value = CreateJsonObject(newStart, newEnd - newStart);
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
                        int sizeOrlength = record.SizeOrLength - reduce;
                        int numberOfRows = sizeOrlength == 0 ? 1 : sizeOrlength; //TODO: fix up for arrays
                        bool hasChildren = record.HasChildren; //TODO: Re-calculate HasChildren since it can become false
                        DbRow fixup = new DbRow(record.JsonType, record.Location, hasChildren, sizeOrlength, numberOfRows);
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

            if (record.JsonType != JsonType.StartObject)
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

                Debug.Assert(record.JsonType == JsonType.String);

                int startIndex = i + DbRow.Size;
                DbRow nextRecord = _database.Get(startIndex);

                if (_jsonData.Slice(record.Location, record.SizeOrLength).SequenceEqual(propertyName.Bytes))
                {
                    int length = DbRow.Size;
                    if (!nextRecord.IsSimpleValue)
                        if (nextRecord.SizeOrLength != 0)
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

        public JsonObject this[ReadOnlySpan<byte> name] => this[new Utf8Span(name)];

        private JsonObject CreateJsonObject(int startIndex, int length)
        {
            CustomDb copy = _database;
            copy.Span = _database.Slice(startIndex, length);
            ReadOnlySpan<byte> name = default;
            if (startIndex >= DbRow.Size)
            {
                DbRow row = GetRowDbIndex(startIndex - DbRow.Size);
                if (row.JsonType == JsonType.String)
                    name = _jsonData.Slice(row.Location, row.SizeOrLength);
            }
            return new JsonObject(_jsonData, copy, name);
        }

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
                        if (nextRecord.SizeOrLength != 0)
                            length += DbRow.Size * nextRecord.NumberOfRows;
                    return CreateJsonObject(i, length);
                }

                if (!nextRecord.IsSimpleValue)
                    if (nextRecord.SizeOrLength != 0)
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

                if (record.JsonType != JsonType.StartArray)
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
                if (record.JsonType != JsonType.StartArray)
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

        public JsonTokenType Type => _database.GetJsonType();

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

        internal DbRow GetRow(int index)
        {
            return _database.Get(index * DbRow.Size);
        }

        internal DbRow GetRow()
        {
            return _database.Get();
        }

        internal DbRow GetRowDbIndex(int index)
        {
            return _database.Get(index);
        }

        internal ReadOnlySpan<byte> GetSpan(int index)
        {
            DbRow row = GetRow(index);
            if (row.IsSimpleValue)
                return _jsonData.Slice(row.Location, row.SizeOrLength);
            return default; // Throw instead?
        }

        internal ReadOnlySpan<byte> GetSpan()
        {
            DbRow row = GetRow();
            if (row.IsSimpleValue)
                return _jsonData.Slice(row.Location, row.SizeOrLength);
            return default; // Throw instead?
        }

        internal ReadOnlySpan<byte> GetSpan(DbRow row)
        {
            Debug.Assert(row.IsSimpleValue);
            return _jsonData.Slice(row.Location, row.SizeOrLength);
        }

        internal int Size
        {
            get
            {
                DbRow record = _database.Get();
                if (record.IsSimpleValue)
                {
                    JsonThrowHelper.ThrowInvalidOperationException();
                }
                return record.SizeOrLength == 0 ? 0 : record.NumberOfRows;
            }
        }

        public void Dispose()
        {
            _database.Dispose();
            _jsonData = ReadOnlySpan<byte>.Empty;
        }

        public Enumerator GetEnumerator() => new Enumerator(this);

        public ReadOnlySpan<byte> PropertyName { get; private set; }

        public ref struct Enumerator
        {
            private readonly JsonObject _obj;
            private int _index;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal Enumerator(JsonObject obj)
            {
                _obj = obj;
                Current = default;
                _index = 0;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                DbRow row = _obj.GetRow();
                if (row.JsonType == JsonType.StartObject)
                {
                    if (_index < _obj.Size - 1)
                    {
                        _index++;
                        row = _obj.GetRow(_index);
                        if (row.JsonType != JsonType.String)
                            JsonThrowHelper.ThrowInvalidOperationException();

                        _index++;

                        DbRow nextRow = _obj.GetRow(_index);

                        if (!nextRow.IsSimpleValue && nextRow.SizeOrLength != 0)
                        {
                            Current = _obj.CreateJsonObject(_index * DbRow.Size, DbRow.Size * (nextRow.NumberOfRows + 1));
                            _index += nextRow.NumberOfRows;
                        }
                        else
                        {
                            Current = _obj.CreateJsonObject(_index * DbRow.Size, DbRow.Size);
                        }
                        return true;
                    }
                }
                else if (row.JsonType == JsonType.StartArray)
                {
                    if (_index < _obj.Size)
                    {
                        _index++;

                        DbRow nextRow = _obj.GetRow(_index);

                        if (!nextRow.IsSimpleValue && nextRow.SizeOrLength != 0)
                        {
                            Current = _obj.CreateJsonObject(_index * DbRow.Size, DbRow.Size * (nextRow.NumberOfRows + 1));
                            _index += nextRow.NumberOfRows;
                        }
                        else
                        {
                            Current = _obj.CreateJsonObject(_index * DbRow.Size, DbRow.Size);
                        }
                        return true;
                    }
                }
                return false;
            }

            public JsonObject Current { get; private set; }
        }
    }
}
