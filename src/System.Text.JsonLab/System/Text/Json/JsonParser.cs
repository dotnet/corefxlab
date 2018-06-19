﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Buffers.Text;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Utf8;

using static System.Buffers.Binary.BinaryPrimitives;
using static System.Runtime.InteropServices.MemoryMarshal;

namespace System.Text.JsonLab
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct DbRow
    {
        public static int Size;

        public int Location;                // index in JSON payload
        public int Length;      // length of text in JSON payload
        public JsonObject.JsonValueType Type; // type of JSON construct (e.g. Object, Array, Number)

        public const int UnknownNumberOfRows = -1;

        public DbRow(JsonObject.JsonValueType type, int valueIndex, int lengthOrNumberOfRows = UnknownNumberOfRows)
        {
            Location = valueIndex;
            Length = lengthOrNumberOfRows;
            Type = type;
        }

        public bool IsSimpleValue => Type != JsonObject.JsonValueType.Object && Type != JsonObject.JsonValueType.Array;

        static DbRow()
        {
            unsafe { Size = sizeof(DbRow); }
        }
    }

    [StructLayout(LayoutKind.Auto)]
    internal ref struct TwoStacks
    {
        Span<byte> _span;
        int topOfStackObj;
        int topOfStackArr;
        int capacity;
        int objectStackCount;
        int arrayStackCount;

        public int ObjectStackCount => objectStackCount;

        public int ArrayStackCount => arrayStackCount;

        public bool IsFull {
            get {
                return objectStackCount >= capacity || arrayStackCount >= capacity;
            }
        }

        public TwoStacks(Span<byte> db)
        {
            _span = db;
            topOfStackObj = _span.Length;
            topOfStackArr = _span.Length;
            objectStackCount = 0;
            arrayStackCount = 0;
            capacity = _span.Length/8;
        }

        public bool TryPushObject(int value)
        {
            if (!IsFull) {
                Write(_span.Slice(topOfStackObj - 8), ref value);
                topOfStackObj -= 8;
                objectStackCount++;
                return true;
            }
            return false;
        }

        public bool TryPushArray(int value)
        {
            if (!IsFull) {
                Write(_span.Slice(topOfStackArr - 4), ref value);
                topOfStackArr -= 8;
                arrayStackCount++;
                return true;
            }
            return false;
        }

        public int PopObject()
        {
            objectStackCount--;
            int value = Read<int>(_span.Slice(topOfStackObj));
            topOfStackObj += 8;
            return value;
        }

        public int PopArray()
        {
            arrayStackCount--;
            int value = Read<int>(_span.Slice(topOfStackArr + 4));
            topOfStackArr += 8;
            return value;
        }

        internal void Resize(Span<byte> newStackMemory)
        {
            _span.Slice(0, Math.Max(objectStackCount, arrayStackCount) * 8).CopyTo(newStackMemory);
            _span = newStackMemory;
        }
    }

    [StructLayout(LayoutKind.Auto)]
    internal ref struct JsonParser
    {
        private Span<byte> _db;
        private ReadOnlySpan<byte> _values; // TODO: this should be ReadOnlyMemory<byte>
        private Span<byte> _scratchSpan;
        private IMemoryOwner<byte> _scratchManager;
        MemoryPool<byte> _pool;
        TwoStacks _stack;

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

        private static readonly byte[] s_false = new Utf8Span("false").Bytes.ToArray();
        private static readonly byte[] s_true = new Utf8Span("true").Bytes.ToArray();
        private static readonly byte[] s_null = new Utf8Span("null").Bytes.ToArray();

        public JsonObject Parse(ReadOnlySpan<byte> utf8Json, MemoryPool<byte> pool = null)
        {
            _pool = pool;
            if (_pool == null) _pool = MemoryPool<byte>.Shared;
            _scratchManager = _pool.Rent(utf8Json.Length * 4);
            _scratchSpan = _scratchManager.Memory.Span;

            int dbLength = _scratchSpan.Length / 2;
            _db = _scratchSpan.Slice(0, dbLength);
            _stack = new TwoStacks(_scratchSpan.Slice(dbLength));

            _values = utf8Json;
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

            while (Read()) {
                var tokenType = _tokenType;
                switch (tokenType) {
                    case JsonTokenType.ObjectStart:
                        AppendDbRow(JsonObject.JsonValueType.Object, _valuesIndex);
                        while(!_stack.TryPushObject(numberOfRowsForMembers)) {
                            ResizeDb();
                        }
                        numberOfRowsForMembers = 0;
                        break;
                    case JsonTokenType.ObjectEnd:
                        Write(_db.Slice(FindLocation(_stack.ObjectStackCount - 1, true)), ref numberOfRowsForMembers);
                        numberOfRowsForMembers += _stack.PopObject();
                        break;
                    case JsonTokenType.ArrayStart:
                        AppendDbRow(JsonObject.JsonValueType.Array, _valuesIndex);
                        while (!_stack.TryPushArray(arrayItemsCount)) {
                            ResizeDb();
                        }
                        arrayItemsCount = 0;
                        break;
                    case JsonTokenType.ArrayEnd:
                        Write(_db.Slice(FindLocation(_stack.ArrayStackCount - 1, false)), ref arrayItemsCount);
                        arrayItemsCount = _stack.PopArray();
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

            var result =  new JsonObject(_values, _db.Slice(0, _dbIndex), _pool, _scratchManager);
            _scratchManager.Dispose();
            _scratchManager = null;
            return result;
        }

        private void ResizeDb()
        {
            IMemoryOwner<byte> newScratch = _pool.Rent(_scratchSpan.Length * 2);
            int dbLength = newScratch.Memory.Length / 2;

            Span<byte> span = newScratch.Memory.Span;
            Span<byte> sliceStart = span.Slice(0, dbLength);
            _db.Slice(0, _valuesIndex).CopyTo(sliceStart);
            _db = sliceStart;

            Span<byte> newStackMemory = span.Slice(dbLength);
            _stack.Resize(newStackMemory);
            _scratchManager.Dispose();
            _scratchManager = newScratch;
        }

        private int FindLocation(int index, bool lookingForObject)
        {
            int rowNumber = 0;
            int numFound = 0;

            while (true) {
                int rowStartOffset = rowNumber * DbRow.Size;
                DbRow row = Read<DbRow>(_db.Slice(rowStartOffset));

                int lengthOffset = rowStartOffset + 4;
                
                if (row.Length == -1 && (lookingForObject ? row.Type == JsonObject.JsonValueType.Object : row.Type == JsonObject.JsonValueType.Array)) {
                    numFound++;
                }

                if (index == numFound - 1) {
                    return lengthOffset;
                } else {
                    if (row.Length > 0 && (row.Type == JsonObject.JsonValueType.Object || row.Type == JsonObject.JsonValueType.Array)) {
                        rowNumber += row.Length;
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

        private bool AppendDbRow(JsonObject.JsonValueType type, int valueIndex, int LengthOrNumberOfRows = DbRow.UnknownNumberOfRows)
        {
            var newIndex = _dbIndex + DbRow.Size;
            if (newIndex >= _db.Length) {
                ResizeDb();
            }

            var dbRow = new DbRow(type, valueIndex, LengthOrNumberOfRows);
            Write(_db.Slice(_dbIndex), ref dbRow);
            _dbIndex = newIndex;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SkipWhitespace()
        {
            while (Unicode.IsWhitespace(_values[_valuesIndex])) {
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
