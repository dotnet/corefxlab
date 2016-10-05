// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Utf8;

namespace System.Text.Json
{
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

        private const int RowSize = 9;  // Do not change, unless you also change FindLocation

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

        public JsonObject Parse(ReadOnlySpan<byte> utf8Json)
        {
            var db = new byte[utf8Json.Length * 2];
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

            int numValues = 0;
            int numPairs = 0;
            int numObj = 0;
            int numArr = 0;

            int topOfStackObj = _db.Length - 1;
            int topOfStackArr = _db.Length - 1;

            while (Read()) {
                var tokenType = _tokenType;
                switch (tokenType) {
                    case JsonTokenType.ObjectStart:
                        _db.Slice(_dbIndex).Write<int>(_valuesIndex); _dbIndex += 4;
                        _db.Slice(_dbIndex).Write<int>(-1); _dbIndex += 4;
                        _db.Slice(_dbIndex).Write(JsonObject.JsonValueType.Object); _dbIndex += 1;
                        _db.Slice(topOfStackObj - 8).Write<int>(numPairs); topOfStackObj -= 8;
                        numPairs = 0;
                        numObj++;
                        break;
                    case JsonTokenType.ObjectEnd:
                        _db.Slice(FindLocation(numObj - 1, true)).Write<int>(numPairs);
                        numObj--;
                        numPairs += _db.Slice(topOfStackObj).Read<int>();
                        topOfStackObj += 8;
                        break;
                    case JsonTokenType.ArrayStart:
                        _db.Slice(_dbIndex).Write<int>(_valuesIndex); _dbIndex += 4;
                        _db.Slice(_dbIndex).Write<int>(-1); _dbIndex += 4;
                        _db.Slice(_dbIndex).Write(JsonObject.JsonValueType.Array); _dbIndex += 1;
                        _db.Slice(topOfStackArr - 4).Write<int>(numValues); topOfStackArr -= 8;
                        numValues = 0;
                        numArr++;
                        break;
                    case JsonTokenType.ArrayEnd:
                        _db.Slice(FindLocation(numArr - 1, false)).Write<int>(numValues);
                        numArr--;
                        numValues += _db.Slice(topOfStackArr + 4).Read<int>();
                        topOfStackArr += 8;
                        break;
                    case JsonTokenType.Property:
                        ParseName();
                        numPairs++;
                        ParseValue();
                        numPairs++;
                        numValues++;
                        numValues++;
                        break;
                    case JsonTokenType.Value:
                        ParseValue();
                        numValues++;
                        numPairs++;
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
                int rowStartOffset = (rowNumber << 3) + rowNumber; // multiply by RowSize which is 9
                int lengthOffset = rowStartOffset + 4;
                var typeCode = _db.Slice(rowStartOffset + 8).Read<JsonObject.JsonValueType>();
                var length = _db.Slice(rowStartOffset + 4).Read<int>();

                if (length == -1 && (lookingForObject ? typeCode == JsonObject.JsonValueType.Object : typeCode == JsonObject.JsonValueType.Array)) {
                    numFound++;
                }

                if (index == numFound - 1) {
                    return lengthOffset;
                } else {
                    if (length > 0 && (typeCode == JsonObject.JsonValueType.Object || typeCode == JsonObject.JsonValueType.Array)) {
                        rowNumber += length;
                    }
                    rowNumber++;
                }
            }
        }

        private bool Read()
        {
            var canRead = _valuesIndex < _values.Length;
            if (canRead) MoveToNextTokenType();
            return canRead;
        }

        private void ParseName()
        {
            SkipWhitespace();
            ParseStringValue();
            _valuesIndex++;
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

            throw new FormatException("Invalid json, tried to read char '" + nextByte + "'.");
        }

        private void ParseValue()
        {
            var type = PeekType();
            switch (type) {
                case JsonObject.JsonValueType.String:
                    ParseStringValue();
                    return;
                case JsonObject.JsonValueType.Number:
                    ParseNumberValue();
                    return;
                case JsonObject.JsonValueType.True:
                    ParseTrueValue();
                    return;
                case JsonObject.JsonValueType.False:
                    ParseFalseValue();
                    return;
                case JsonObject.JsonValueType.Null:
                    ParseNullValue();
                    return;
                case JsonObject.JsonValueType.Object:
                case JsonObject.JsonValueType.Array:
                    return;
                default:
                    throw new ArgumentException("Invalid json value type '" + type + "'.");
            }
        }

        private void ParseStringValue()
        {
            _valuesIndex++;
            var count = _valuesIndex;
            do {
                while (_values[count] != '"') {
                    count++;
                }
                count++;
            } while (AreNumOfBackSlashesAtEndOfStringOdd(count - 2));

            var strLength = count - _valuesIndex;

            _db.Slice(_dbIndex).Write<int>(_valuesIndex); _dbIndex += 4;
            _db.Slice(_dbIndex).Write<int>(strLength - 1); _dbIndex += 4;
            _dbIndex += 1;

            _valuesIndex += strLength;

            SkipWhitespace();
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

        private void ParseNumberValue()
        {
            var count = _valuesIndex;

            var nextByte = _values[count];
            if (nextByte == '-') {
                count++;
            }

            nextByte = _values[count];
            while (nextByte >= '0' && nextByte <= '9') {
                count++;
                nextByte = _values[count];
            }

            if (nextByte == '.') {
                count++;
            }

            nextByte = _values[count];
            while (nextByte >= '0' && nextByte <= '9') {
                count++;
                nextByte = _values[count];
            }

            if (nextByte == 'e' || nextByte == 'E') {
                count++;
                nextByte = _values[count];
                if (nextByte == '-' || nextByte == '+') {
                    count++;
                }
                nextByte = _values[count];
                while (nextByte >= '0' && nextByte <= '9') {
                    count++;
                    nextByte = _values[count];
                }
            }

            var length = count - _valuesIndex;

            _db.Slice(_dbIndex).Write<int>(_valuesIndex); _dbIndex += 4;
            _db.Slice(_dbIndex).Write<int>(count - _valuesIndex); _dbIndex += 4;
            _dbIndex += 1;

            _valuesIndex += length;
            SkipWhitespace();
        }

        private void ParseTrueValue()
        {
            _db.Slice(_dbIndex).Write<int>(_valuesIndex); _dbIndex += 4;
            _db.Slice(_dbIndex).Write<int>(4); _dbIndex += 4;
            _dbIndex += 1;

            if (_values[_valuesIndex + 1] != 'r' || _values[_valuesIndex + 2] != 'u' || _values[_valuesIndex + 3] != 'e') {
                throw new FormatException("Invalid json, tried to read 'true'.");
            }

            _valuesIndex += 4;

            SkipWhitespace();
        }

        private void ParseFalseValue()
        {
            _db.Slice(_dbIndex).Write<int>(_valuesIndex); _dbIndex += 4;
            _db.Slice(_dbIndex).Write(JsonObject.JsonValueType.False); _dbIndex += 4; // TODO: can this advance by 1?
            _dbIndex += 1;

            if (_values[_valuesIndex + 1] != 'a' || _values[_valuesIndex + 2] != 'l' || _values[_valuesIndex + 3] != 's' || _values[_valuesIndex + 4] != 'e') {
                throw new FormatException("Invalid json, tried to read 'false'.");
            }

            _valuesIndex += 5;

            SkipWhitespace();
        }

        private void ParseNullValue()
        {
            _db.Slice(_dbIndex).Write<int>(_valuesIndex); _dbIndex += 4;
            _db.Slice(_dbIndex).Write(JsonObject.JsonValueType.True); _dbIndex += 4;
            _dbIndex += 1;

            if (_values[_valuesIndex + 1] != 'u' || _values[_valuesIndex + 2] != 'l' || _values[_valuesIndex + 3] != 'l') {
                throw new FormatException("Invalid json, tried to read 'null'.");
            }

            _valuesIndex += 4;

            SkipWhitespace();
        }

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
    }
}
