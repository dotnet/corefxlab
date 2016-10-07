// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;
using System.Text.Utf8;

namespace System.Text.Json
{
    internal struct DbRow
    {
        public int ValueIndex;                // index in JSON payload
        public int LengthOrNumberOfRows;      // length of text in JSON payload
        public JsonObject.JsonValueType Type; // type of JSON construct (e.g. Object, Array, Number)

        public const int UnknownNumberOfRows = -1;

        public DbRow(JsonObject.JsonValueType type, int valueIndex, int lengthOrNumberOfRows = UnknownNumberOfRows)
        {
            ValueIndex = valueIndex;
            LengthOrNumberOfRows = lengthOrNumberOfRows;
            Type = type;
        }
    }

    internal struct TwoStacks
    {
        Span<byte> _db;
        int topOfStackObj;
        int topOfStackArr;

        int objectStackCount;
        int arrayStackCount;

        public int ObjectStackCount => objectStackCount;

        public int ArrayStackCount => arrayStackCount;

        public TwoStacks(Span<byte> db)
        {
            _db = db;
            topOfStackObj = _db.Length - 1;
            topOfStackArr = _db.Length - 1;
            objectStackCount = 0;
            arrayStackCount = 0;
        }

        public void PushObject(int value)
        {
            _db.Slice(topOfStackObj - 8).Write<int>(value);
            topOfStackObj -= 8;
            objectStackCount++;
        }

        public void PushArray(int value)
        {
            _db.Slice(topOfStackArr - 4).Write<int>(value);
            topOfStackArr -= 8;
            arrayStackCount++;
        }

        public int PopObject()
        {
            objectStackCount--;
            var value = _db.Slice(topOfStackObj).Read<int>();
            topOfStackObj += 8;
            return value;
        }

        public int PopArray()
        {
            arrayStackCount--;
            var value = _db.Slice(topOfStackArr + 4).Read<int>();
            topOfStackArr += 8;
            return value;
        }
    }

    internal struct JsonParser
    {
        private Span<byte> _db;
        private ReadOnlySpan<byte> _values;

        private int _valuesIndex;
        private int _dbIndex;

        private int _insideObject;
        private int _insideArray;
        private JsonTokenType _tokenType;
        private bool _jsonStartIsObject;

        private enum JsonTokenType
        {
            // Start = 0 state reserved for internal use
            ObjectStart = 1,
            ObjectEnd = 2,
            ArrayStart = 3,
            ArrayEnd = 4,
            Property = 5,
            Value = 6
        };

        private static ReadOnlySpan<byte> s_false = new Utf8String("false").Bytes;
        private static ReadOnlySpan<byte> s_true = new Utf8String("true").Bytes;
        private static ReadOnlySpan<byte> s_null = new Utf8String("null").Bytes;

        public JsonObject Parse(ReadOnlySpan<byte> utf8Json)
        {
            var db = new byte[utf8Json.Length * 4];
            return Parse(utf8Json, db);
        }

        public JsonObject Parse(ReadOnlySpan<byte> utf8Json, Span<byte> db)
        {
            _values = utf8Json;
            _db = db;

            _insideObject = 0;
            _insideArray = 0;
            _tokenType = 0;
            _valuesIndex = 0;
            _dbIndex = 0;
            _jsonStartIsObject = false;

            SkipWhitespace();

            _jsonStartIsObject = _values[_valuesIndex] == '{';

            int arrayItemsCount = 0;
            int numberOfRowsForMembers = 0;

            TwoStacks stack = new TwoStacks(_db);

            while (Read()) {
                var tokenType = _tokenType;
                switch (tokenType) {
                    case JsonTokenType.ObjectStart:
                        AppendDbRow(JsonObject.JsonValueType.Object, _valuesIndex);
                        stack.PushObject(numberOfRowsForMembers);
                        numberOfRowsForMembers = 0;
                        break;
                    case JsonTokenType.ObjectEnd:
                        _db.Slice(FindLocation(stack.ObjectStackCount - 1, true)).Write<int>(numberOfRowsForMembers);
                        numberOfRowsForMembers += stack.PopObject();
                        break;
                    case JsonTokenType.ArrayStart:
                        AppendDbRow(JsonObject.JsonValueType.Array, _valuesIndex);
                        stack.PushArray(arrayItemsCount);
                        arrayItemsCount = 0;
                        break;
                    case JsonTokenType.ArrayEnd:
                        _db.Slice(FindLocation(stack.ArrayStackCount - 1, false)).Write<int>(arrayItemsCount);
                        arrayItemsCount += stack.PopArray();
                        break;
                    case JsonTokenType.Property:
                        ParsePropertyName();
                        ParseValue();
                        numberOfRowsForMembers++;
                        numberOfRowsForMembers++;
                        break;
                    case JsonTokenType.Value:
                        ParseValue();
                        arrayItemsCount++;
                        numberOfRowsForMembers++;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return new JsonObject(_values, _db.Slice(0, _dbIndex));
        }

        private int FindLocation(int index, bool lookingForObject)
        {
            int rowNumber = 0;
            int numFound = 0;

            while (true) {
                int rowStartOffset = rowNumber * JsonObject.RowSize;
                var row = _db.Slice(rowStartOffset).Read<DbRow>();

                int lengthOffset = rowStartOffset + 4;
                
                if (row.LengthOrNumberOfRows == -1 && (lookingForObject ? row.Type == JsonObject.JsonValueType.Object : row.Type == JsonObject.JsonValueType.Array)) {
                    numFound++;
                }

                if (index == numFound - 1) {
                    return lengthOffset;
                } else {
                    if (row.LengthOrNumberOfRows > 0 && (row.Type == JsonObject.JsonValueType.Object || row.Type == JsonObject.JsonValueType.Array)) {
                        rowNumber += row.LengthOrNumberOfRows;
                    }
                    rowNumber++;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool Read()
        {
            var canRead = _valuesIndex < _values.Length;
            if (canRead) MoveToNextTokenType();
            return canRead;
        }

        private JsonObject.JsonValueType PeekType()
        {
            SkipWhitespace();

            var nextByte = _values[_valuesIndex];

            if (nextByte == '"') {
                return JsonObject.JsonValueType.String;
            }

            if (nextByte == '{') {
                return JsonObject.JsonValueType.Object;
            }

            if (nextByte == '[') {
                return JsonObject.JsonValueType.Array;
            }

            if (nextByte == 't') {
                return JsonObject.JsonValueType.True;
            }

            if (nextByte == 'f') {
                return JsonObject.JsonValueType.False;
            }

            if (nextByte == 'n') {
                return JsonObject.JsonValueType.Null;
            }

            if (nextByte == '-' || (nextByte >= '0' && nextByte <= '9')) {
                return JsonObject.JsonValueType.Number;
            }

            throw new FormatException("Invalid json, tried to read char '" + nextByte + " at " + _valuesIndex + "'.");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ParsePropertyName()
        {
            SkipWhitespace();
            ParseString();
            _valuesIndex++;
        }

        private void ParseValue()
        {
            var type = PeekType();
            switch (type) {
                case JsonObject.JsonValueType.String:
                    ParseString();
                    break;
                case JsonObject.JsonValueType.Number:
                    ParseNumber();
                    break;
                case JsonObject.JsonValueType.True:
                    ParseLiteral(JsonObject.JsonValueType.True, s_true);
                    return;
                case JsonObject.JsonValueType.False:
                    ParseLiteral(JsonObject.JsonValueType.False, s_false);
                    break;
                case JsonObject.JsonValueType.Null:
                    ParseLiteral(JsonObject.JsonValueType.Null, s_null);
                    break;
                case JsonObject.JsonValueType.Object:
                case JsonObject.JsonValueType.Array:
                    break;
                default:
                    throw new ArgumentException("Invalid json value type '" + type + "'.");
            }
        }

        private void ParseString()
        {
            _valuesIndex++; // eat quote

            var indexOfClosingQuote = _valuesIndex;
            do {
                indexOfClosingQuote = _values.Slice(indexOfClosingQuote).IndexOf((byte)'"');
            } while (AreNumOfBackSlashesAtEndOfStringOdd(_valuesIndex + indexOfClosingQuote - 2));

            AppendDbRow(JsonObject.JsonValueType.String, _valuesIndex, indexOfClosingQuote);

            _valuesIndex += indexOfClosingQuote + 1;
            SkipWhitespace();
        }

        private void ParseNumber()
        {
            var nextIndex = _valuesIndex;

            var nextByte = _values[nextIndex];
            if (nextByte == '-') {
                nextIndex++;
            }

            nextByte = _values[nextIndex];
            while (nextByte >= '0' && nextByte <= '9') {
                nextIndex++;
                nextByte = _values[nextIndex];
            }

            if (nextByte == '.') {
                nextIndex++;
            }

            nextByte = _values[nextIndex];
            while (nextByte >= '0' && nextByte <= '9') {
                nextIndex++;
                nextByte = _values[nextIndex];
            }

            if (nextByte == 'e' || nextByte == 'E') {
                nextIndex++;
                nextByte = _values[nextIndex];
                if (nextByte == '-' || nextByte == '+') {
                    nextIndex++;
                }
                nextByte = _values[nextIndex];
                while (nextByte >= '0' && nextByte <= '9') {
                    nextIndex++;
                    nextByte = _values[nextIndex];
                }
            }

            var length = nextIndex - _valuesIndex;

            AppendDbRow(JsonObject.JsonValueType.Number, _valuesIndex, length);

            _valuesIndex += length;
            SkipWhitespace();
        }

        private void ParseLiteral(JsonObject.JsonValueType literal, ReadOnlySpan<byte> expected)
        {
            if (!_values.Slice(_valuesIndex).StartsWith(expected)) {
                throw new FormatException("Invalid json, tried to read " + literal.ToString());
            }
            AppendDbRow(literal, _valuesIndex, expected.Length);
            _valuesIndex += expected.Length;
            SkipWhitespace();
        }

        private void AppendDbRow(JsonObject.JsonValueType type, int valueIndex, int LengthOrNumberOfRows = DbRow.UnknownNumberOfRows)
        {
            var dbRow = new DbRow(type, valueIndex, LengthOrNumberOfRows);
            _db.Slice(_dbIndex).Write(dbRow);
            _dbIndex += JsonObject.RowSize;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SkipWhitespace()
        {
            while (Utf8String.IsWhiteSpace(_values[_valuesIndex])) {
                _valuesIndex++;
            }
        }

        private void MoveToNextTokenType()
        {
            SkipWhitespace();

            var nextByte = _values[_valuesIndex];

            switch (_tokenType) {
                case JsonTokenType.ObjectStart:
                    if (nextByte != '}') {
                        _tokenType = JsonTokenType.Property;
                        return;
                    }
                    break;
                case JsonTokenType.ObjectEnd:
                    if (nextByte == ',') {
                        _valuesIndex++;
                        if (_insideObject == _insideArray) {
                            _tokenType = !_jsonStartIsObject ? JsonTokenType.Property : JsonTokenType.Value;
                            return;
                        }
                        _tokenType = _insideObject > _insideArray ? JsonTokenType.Property : JsonTokenType.Value;
                        return;
                    }
                    break;
                case JsonTokenType.ArrayStart:
                    if (nextByte != ']') {
                        _tokenType = JsonTokenType.Value;
                        return;
                    }
                    break;
                case JsonTokenType.ArrayEnd:
                    if (nextByte == ',') {
                        _valuesIndex++;
                        if (_insideObject == _insideArray) {
                            _tokenType = !_jsonStartIsObject ? JsonTokenType.Property : JsonTokenType.Value;
                            return;
                        }
                        _tokenType = _insideObject > _insideArray ? JsonTokenType.Property : JsonTokenType.Value;
                        return;
                    }
                    break;
                case JsonTokenType.Property:
                    if (nextByte == ',') {
                        _valuesIndex++;
                        return;
                    }
                    break;
                case JsonTokenType.Value:
                    if (nextByte == ',') {
                        _valuesIndex++;
                        return;
                    }
                    break;
            }

            _valuesIndex++;
            switch (nextByte) {
                case (byte)'{':
                    _insideObject++;
                    _tokenType = JsonTokenType.ObjectStart;
                    return;
                case (byte)'}':
                    _insideObject--;
                    _tokenType = JsonTokenType.ObjectEnd;
                    return;
                case (byte)'[':
                    _insideArray++;
                    _tokenType = JsonTokenType.ArrayStart;
                    return;
                case (byte)']':
                    _insideArray--;
                    _tokenType = JsonTokenType.ArrayEnd;
                    return;
                default:
                    throw new FormatException("Unable to get next token type. Check json format.");
            }
        }

        private bool AreNumOfBackSlashesAtEndOfStringOdd(int count)
        {
            var length = count - _valuesIndex;
            if (length < 0) return false;
            var nextByte = _values[count];
            if (nextByte != '\\') return false;
            var numOfBackSlashes = 0;
            while (nextByte == '\\') {
                numOfBackSlashes++;
                if ((length - numOfBackSlashes) < 0) return numOfBackSlashes % 2 != 0;
                nextByte = _values[count - numOfBackSlashes];
            }
            return numOfBackSlashes % 2 != 0;
        }
    }
}
