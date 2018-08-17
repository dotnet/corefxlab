// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Diagnostics;
using System.Runtime.InteropServices;

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

    // IsArray - offset - 0 - size - 1
    // Length - offset - 1 - size - 4
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct StackRow
    {
        public static int Size;

        public bool IsArray;
        public int Length;

        public StackRow(bool isArray, int lengthOrNumberOfRows)
        {
            IsArray = isArray;
            Length = lengthOrNumberOfRows;
        }

        unsafe static StackRow()
        {
            Size = sizeof(StackRow);
        }
    }

    internal ref struct CustomStack
    {
        private Span<byte> _stackSpace;
        private int _topOfStack;

        public CustomStack(Span<byte> stackSpace)
        {
            _stackSpace = stackSpace;
            _topOfStack = stackSpace.Length;
        }

        public bool TryPush(StackRow row)
        {
            if (_topOfStack >= StackRow.Size)
            {
                MemoryMarshal.Write(_stackSpace.Slice(_topOfStack - StackRow.Size), ref row);
                _topOfStack -= StackRow.Size;
                return true;
            }
            return false;
        }

        public StackRow Pop()
        {
            StackRow row = Peek();
            _topOfStack += StackRow.Size;
            return row;
        }

        public StackRow Peek()
        {
            if (_topOfStack > _stackSpace.Length - StackRow.Size)
            {
                JsonThrowHelper.ThrowInvalidOperationException();
            }

            return MemoryMarshal.Read<StackRow>(_stackSpace.Slice(_topOfStack));
        }

        public bool IsTopArray()
        {
            if (_topOfStack > _stackSpace.Length - StackRow.Size)
            {
                return false;
            }
            return _stackSpace[_topOfStack] == 1;
        }

        private void Reset()
        {
            _stackSpace.Slice(_topOfStack - StackRow.Size, StackRow.Size).Fill(255);
        }

        public void Resize(Span<byte> newStackMemory)
        {
            Debug.Assert(newStackMemory.Length > _stackSpace.Length);
            int stackGrowth = newStackMemory.Length - _stackSpace.Length;
            _stackSpace.CopyTo(newStackMemory.Slice(stackGrowth));
            _topOfStack += stackGrowth;
            _stackSpace = newStackMemory;
        }

        public string PrintStacks()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("IsArray" + "\t" + "Length" + "\r\n");
            for (int i = _stackSpace.Length - StackRow.Size; i >= StackRow.Size; i -= StackRow.Size)
            {
                StackRow row = MemoryMarshal.Read<StackRow>(_stackSpace.Slice(i));
                sb.Append(row.IsArray + "\t" + row.Length + "\r\n");
            }
            return sb.ToString();
        }
    }

    public ref struct JsonParser
    {
        private readonly ReadOnlySpan<byte> _utf8Json; // TODO: this should be ReadOnlyMemory<byte>
        private ArrayPool<byte> _pool;
        private byte[] _rentedBuffer;
        private Utf8JsonReader _reader;
        private Span<byte> _db;
        private CustomStack _stack;
        private int _dbIndex;

        // Tunable paramters to minimize allocations but avoid resizing for the common cases.
        private const double DatabaseStackSpaceRatio = 0.8;
        private const double AllocationMultiplier = 1.5;

        public JsonParser(ReadOnlySpan<byte> utf8Json, ArrayPool<byte> pool = null)
        {
            _utf8Json = utf8Json;
            _pool = pool ?? ArrayPool<byte>.Shared;
            _rentedBuffer = _pool.Rent((int)(utf8Json.Length * AllocationMultiplier));
            _reader = new Utf8JsonReader(utf8Json);

            Span<byte> scratchSpan = _rentedBuffer;
            int dbLength = (int)(scratchSpan.Length * DatabaseStackSpaceRatio);
            _db = scratchSpan.Slice(0, dbLength);
            _stack = new CustomStack(scratchSpan.Slice(dbLength));
            _dbIndex = 0;
        }

        public JsonObject Parse()
        {
            bool isArray = false;
            JsonTokenType tokenType = default;

            if (_reader.Read())
            {
                tokenType = _reader.TokenType;

                if (tokenType == JsonTokenType.StartObject)
                {
                    AppendDbRow(JsonValueType.Object);
                    while (!_stack.TryPush(new StackRow(false, 0)))
                    {
                        ResizeDb();
                    }
                }
                else if (tokenType == JsonTokenType.StartArray)
                {
                    isArray = true;
                    AppendDbRow(JsonValueType.Array);
                    while (!_stack.TryPush(new StackRow(true, 0)))
                    {
                        ResizeDb();
                    }
                }
                else
                {
                    AppendDbRow(_reader.ValueType, _reader.Value.Length);
                }
            }

            int arrayItemsCount = 0;
            int numberOfRowsForMembers = 0;

            while (_reader.Read())
            {
                tokenType = _reader.TokenType;

                switch (tokenType)
                {
                    case JsonTokenType.StartObject:
                        if (isArray) arrayItemsCount++;
                        AppendDbRow(JsonValueType.Object);
                        StackRow row = new StackRow(false, numberOfRowsForMembers + 1);
                        while (!_stack.TryPush(row))
                        {
                            ResizeDb();
                        }
                        numberOfRowsForMembers = 0;
                        break;
                    case JsonTokenType.EndObject:
                        MemoryMarshal.Write(_db.Slice(FindLocation(JsonValueType.Object)), ref numberOfRowsForMembers);
                        row = _stack.Pop();
                        numberOfRowsForMembers += row.Length;
                        break;
                    case JsonTokenType.StartArray:
                        if (isArray) arrayItemsCount++;
                        numberOfRowsForMembers++;
                        AppendDbRow(JsonValueType.Array);
                        row = new StackRow(true, arrayItemsCount);
                        while (!_stack.TryPush(row))
                        {
                            ResizeDb();
                        }
                        arrayItemsCount = 0;
                        break;
                    case JsonTokenType.EndArray:
                        MemoryMarshal.Write(_db.Slice(FindLocation(JsonValueType.Array)), ref arrayItemsCount);
                        row = _stack.Pop();
                        arrayItemsCount = row.Length;
                        break;
                    case JsonTokenType.PropertyName:
                        numberOfRowsForMembers++;
                        AppendDbRow(JsonValueType.String, _reader.Value.Length);
                        break;
                    case JsonTokenType.Value:
                        if (isArray) arrayItemsCount++;
                        numberOfRowsForMembers++;
                        AppendDbRow(_reader.ValueType, _reader.Value.Length);
                        break;
                }
                isArray = _stack.IsTopArray();
            }
            _pool.Return(_rentedBuffer);
            return new JsonObject(_utf8Json, _db.Slice(0, _dbIndex));
        }

        private void ResizeDb()
        {
            int newSize = _rentedBuffer.Length * 2;
            byte[] newArray = _pool.Rent(newSize);

            Span<byte> scratchSpan = newArray;

            int dbLength = (int)(scratchSpan.Length * DatabaseStackSpaceRatio);
            Span<byte> sliceStart = scratchSpan.Slice(0, dbLength);

            _db.Slice(0, _dbIndex).CopyTo(sliceStart);
            _db = sliceStart;

            Span<byte> newStackMemory = scratchSpan.Slice(dbLength);
            _stack.Resize(newStackMemory);

            _pool.Return(_rentedBuffer);
            _rentedBuffer = newArray;
        }

        private int ForwardPass(JsonValueType lookupType)
        {
            int rowStartOffset = 0;
            while (true)
            {
                DbRow row = MemoryMarshal.Read<DbRow>(_db.Slice(rowStartOffset));

                if (row.Length == -1 && row.Type == lookupType)
                {
                    return rowStartOffset + 4;
                }

                if (!row.IsSimpleValue && row.Length > 0)
                {
                    rowStartOffset += row.Length * DbRow.Size;
                }
                rowStartOffset += DbRow.Size;
            }
        }

        private int BackwardPass(JsonValueType lookupType)
        {
            int rowStartOffset = _dbIndex - DbRow.Size;
            while (true)
            {
                DbRow row = MemoryMarshal.Read<DbRow>(_db.Slice(rowStartOffset));

                if (row.Length == -1 && row.Type == lookupType)
                {
                    return rowStartOffset + 4;
                }

                rowStartOffset -= DbRow.Size;
            }
        }

        private int FindLocation(JsonValueType lookupType)
        {
            if (_reader.Index == _utf8Json.Length)
            {
                return ForwardPass(lookupType);
            }
            else
            {
                return BackwardPass(lookupType);
            }
        }

        private void AppendDbRow(JsonValueType type, int LengthOrNumberOfRows = DbRow.UnknownNumberOfRows)
        {
            while (_dbIndex >= _db.Length - DbRow.Size)
            {
                ResizeDb();
            }

            var dbRow = new DbRow(type, _reader.StartLocation, LengthOrNumberOfRows);
            MemoryMarshal.Write(_db.Slice(_dbIndex), ref dbRow);
            _dbIndex += DbRow.Size;
        }
    }
}
