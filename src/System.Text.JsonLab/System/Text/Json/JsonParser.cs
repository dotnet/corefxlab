// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Buffers.Text;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Utf8;

using static System.Runtime.InteropServices.MemoryMarshal;

namespace System.Text.JsonLab
{
    // Location - offset - 0 - size - 4
    // Length - offset - 4 - size - 4
    // Type - offset - 8 - size - 1
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct DbRow
    {
        public static int Size;

        public int Location;                // index in JSON payload
        public int Length;      // length of text in JSON payload
        public JsonValueType Type; // type of JSON construct (e.g. Object, Array, Number)

        public const int UnknownNumberOfRows = -1;

        public DbRow(JsonValueType type, int valueIndex, int lengthOrNumberOfRows = UnknownNumberOfRows)
        {
            Location = valueIndex;
            Length = lengthOrNumberOfRows;
            Type = type;
        }

        public bool IsSimpleValue => Type > JsonValueType.Array;

        unsafe static DbRow()
        {
            Size = sizeof(DbRow);
        }
    }

    // Length - offset - 0 - size - 4
    // Type - offset - 4 - size - 1
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct StackRow
    {
        public static int Size;

        public int Length;
        public ObjectOrArray Type;

        public const int UnknownNumberOfRows = -1;

        public StackRow(ObjectOrArray type, int lengthOrNumberOfRows = UnknownNumberOfRows)
        {
            Length = lengthOrNumberOfRows;
            Type = type;
        }

        unsafe static StackRow()
        {
            Size = sizeof(StackRow);
        }
    }

    internal enum ObjectOrArray : byte
    {
        Object = 0,
        Array = 1,
    }

    internal ref struct CustomStack
    {
        private Span<byte> _span;
        private int _topOfStack;

        public int ObjectStackCount { get; private set; }
        public int ArrayStackCount { get; private set; }

        public CustomStack(Span<byte> db)
        {
            _span = db;
            _span.Fill(255);
            _topOfStack = _span.Length;
            ObjectStackCount = 0;
            ArrayStackCount = 0;
        }

        public bool TryPush(StackRow row)
        {
            if (_topOfStack > StackRow.Size)
            {
                Write(_span.Slice(_topOfStack - StackRow.Size), ref row);
                _topOfStack -= StackRow.Size;

                if (row.Type == ObjectOrArray.Object)
                    ObjectStackCount++;
                else
                    ArrayStackCount++;

                return true;
            }
            return false;
        }

        public StackRow Pop()
        {
            StackRow row = Read<StackRow>(_span.Slice(_topOfStack));
            _topOfStack += StackRow.Size;

            if (row.Type == ObjectOrArray.Object)
                ObjectStackCount--;
            else
                ArrayStackCount--;

            return row;
        }

        public bool TryGetTopType(out ObjectOrArray type)
        {
            if (_topOfStack + StackRow.Size <= _span.Length)
            {
                type = Read<ObjectOrArray>(_span.Slice(_topOfStack + 4));
                return true;
            }
            type = default;
            return false;
        }

        private void Reset()
        {
            _span.Slice(_topOfStack - StackRow.Size, StackRow.Size).Fill(255);
        }

        internal void Resize(Span<byte> newStackMemory)
        {
            Debug.Assert(newStackMemory.Length > _span.Length);

            newStackMemory.Slice(0, newStackMemory.Length - _span.Length).Fill(255);
            _span.CopyTo(newStackMemory.Slice(newStackMemory.Length - _span.Length));
            _span = newStackMemory;
        }
        
        internal string PrintStacks()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Length" + "\t" + "Type" + "\r\n");
            for (int i = _span.Length - StackRow.Size; i >= StackRow.Size; i -= StackRow.Size)
            {
                StackRow row = Read<StackRow>(_span.Slice(i));
                sb.Append(row.Length + "\t" + row.Type + "\r\n");
            }
            return sb.ToString();
        }
    }

    internal ref struct JsonParser
    {
        private Span<byte> _db;
        private ReadOnlySpan<byte> _values; // TODO: this should be ReadOnlyMemory<byte>
        private Span<byte> _scratchSpan;
        private IMemoryOwner<byte> _scratchManager;
        MemoryPool<byte> _pool;
        CustomStack _stack;

        private int _dbIndex;

        private Utf8JsonReader _reader;

        private static readonly byte[] s_false = new Utf8Span("false").Bytes.ToArray();
        private static readonly byte[] s_true = new Utf8Span("true").Bytes.ToArray();
        private static readonly byte[] s_null = new Utf8Span("null").Bytes.ToArray();

        public JsonParser(ReadOnlySpan<byte> utf8Json, MemoryPool<byte> pool = null)
        {
            _pool = pool ?? MemoryPool<byte>.Shared;
            _scratchManager = _pool.Rent(utf8Json.Length * 4);
            _scratchSpan = _scratchManager.Memory.Span;

            int dbLength = _scratchSpan.Length / 2;
            _db = _scratchSpan.Slice(0, dbLength);
            _stack = new CustomStack(_scratchSpan.Slice(dbLength));

            _values = utf8Json;
            _dbIndex = 0;
            _reader = new Utf8JsonReader(utf8Json);
        }

        public JsonObject Parse()
        {
            int arrayItemsCount = 0;
            int numberOfRowsForMembers = 0;
            bool setStart = false;
            bool success = false;

            ObjectOrArray type = default;

            while (_reader.Read())
            {
                JsonTokenType tokenType = _reader.TokenType;

                if (!setStart)
                {
                    if (tokenType == JsonTokenType.StartObject)
                    {
                        type = ObjectOrArray.Object;
                    }
                    else if (tokenType == JsonTokenType.StartArray)
                    {
                        type = ObjectOrArray.Array;
                    }
                    setStart = true;
                }

                switch (tokenType)
                {
                    case JsonTokenType.StartObject:
                        if (success && type == ObjectOrArray.Array) arrayItemsCount++;
                        if (success) numberOfRowsForMembers++;
                        AppendDbRow(JsonValueType.Object, _reader.StartLocation);
                        StackRow row = new StackRow(ObjectOrArray.Object, numberOfRowsForMembers);
                        while (!_stack.TryPush(row))
                        {
                            ResizeDb();
                        }
                        numberOfRowsForMembers = 0;
                        break;
                    case JsonTokenType.EndObject:
                        Write(_db.Slice(FindLocation(_stack.ObjectStackCount - 1, true)), ref numberOfRowsForMembers);
                        row = _stack.Pop();
                        numberOfRowsForMembers += row.Length;
                        break;
                    case JsonTokenType.StartArray:
                        if (success) numberOfRowsForMembers++;
                        if (success && type == ObjectOrArray.Array) arrayItemsCount++;
                        AppendDbRow(JsonValueType.Array, _reader.StartLocation);
                        row = new StackRow(ObjectOrArray.Array, arrayItemsCount);
                        while (!_stack.TryPush(row))
                        {
                            ResizeDb();
                        }
                        arrayItemsCount = 0;
                        break;
                    case JsonTokenType.EndArray:
                        Write(_db.Slice(FindLocation(_stack.ArrayStackCount - 1, false)), ref arrayItemsCount);
                        row = _stack.Pop();
                        arrayItemsCount = row.Length;
                        break;
                    case JsonTokenType.PropertyName:
                        AppendDbRow(JsonValueType.String, _reader.StartLocation, _reader.Value.Length);
                        numberOfRowsForMembers++;
                        break;
                    case JsonTokenType.Value:
                        ParseValue(_reader.ValueType);
                        if (success && type == ObjectOrArray.Array) arrayItemsCount++;
                        numberOfRowsForMembers++;
                        break;
                    case JsonTokenType.Comment:
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                success = _stack.TryGetTopType(out type);
            }

            var result = new JsonObject(_values, _db.Slice(0, _dbIndex));
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
            _db.Slice(0, _reader.Index).CopyTo(sliceStart);
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

            while (true)
            {
                int rowStartOffset = rowNumber * DbRow.Size;
                DbRow row = Read<DbRow>(_db.Slice(rowStartOffset));

                int lengthOffset = rowStartOffset + 4;

                if (row.Length == -1 && (lookingForObject ? row.Type == JsonValueType.Object : row.Type == JsonValueType.Array))
                {
                    numFound++;
                }

                if (index == numFound - 1)
                {
                    return lengthOffset;
                }
                else
                {
                    if (row.Length > 0 && (row.Type == JsonValueType.Object || row.Type == JsonValueType.Array))
                    {
                        rowNumber += row.Length;
                    }
                    rowNumber++;
                }
            }
        }

        private void ParseValue(JsonValueType type)
        {
            switch (type)
            {
                case JsonValueType.String:
                    AppendDbRow(JsonValueType.String, _reader.StartLocation, _reader.Value.Length);
                    break;
                case JsonValueType.Number:
                    AppendDbRow(JsonValueType.Number, _reader.StartLocation, _reader.Value.Length);
                    break;
                case JsonValueType.True:
                    AppendDbRow(JsonValueType.True, _reader.StartLocation, _reader.Value.Length);
                    return;
                case JsonValueType.False:
                    AppendDbRow(JsonValueType.False, _reader.StartLocation, _reader.Value.Length);
                    break;
                case JsonValueType.Null:
                    AppendDbRow(JsonValueType.Null, _reader.StartLocation, _reader.Value.Length);
                    break;
                case JsonValueType.Object:
                case JsonValueType.Array:
                    break;
                default:
                    throw new ArgumentException("Invalid json value type '" + type + "'.");
            }
        }

        private bool AppendDbRow(JsonValueType type, int valueIndex, int LengthOrNumberOfRows = DbRow.UnknownNumberOfRows)
        {
            var newIndex = _dbIndex + DbRow.Size;
            if (newIndex >= _db.Length)
            {
                ResizeDb();
            }

            var dbRow = new DbRow(type, valueIndex, LengthOrNumberOfRows);
            Write(_db.Slice(_dbIndex), ref dbRow);
            _dbIndex = newIndex;
            return true;
        }
    }
}
