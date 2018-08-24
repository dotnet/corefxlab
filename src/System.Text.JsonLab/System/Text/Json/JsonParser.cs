// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Runtime.InteropServices;

namespace System.Text.JsonLab
{
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
                    AppendDbRow(JsonType.Object);
                    while (!_stack.TryPush(new StackRow(false, 0, false, -1)))
                    {
                        ResizeDb();
                    }
                }
                else if (tokenType == JsonTokenType.StartArray)
                {
                    isArray = true;
                    AppendDbRow(JsonType.Array);
                    while (!_stack.TryPush(new StackRow(true, 0, false, -1)))
                    {
                        ResizeDb();
                    }
                }
                else
                {
                    AppendDbRow((JsonType)_reader.ValueType, _reader.Value.Length);
                }
            }

            int arrayItemsCount = 0;
            int numberOfRowsForMembers = 0;
            int numberOfRowsForValues = 0;

            while (_reader.Read())
            {
                tokenType = _reader.TokenType;

                switch (tokenType)
                {
                    case JsonTokenType.StartObject:
                        if (isArray)
                        {
                            arrayItemsCount++;
                        }
                        numberOfRowsForValues++;
                        AppendDbRow(JsonType.Object);
                        StackRow row = new StackRow(false, numberOfRowsForMembers + 1, false, -1);
                        while (!_stack.TryPush(row))
                        {
                            ResizeDb();
                        }
                        numberOfRowsForMembers = 0;
                        break;
                    case JsonTokenType.EndObject:
                        int location = FindLocation(JsonType.Object);
                        MemoryMarshal.Write(_db.Slice(location), ref numberOfRowsForMembers);
                        row = _stack.Pop();
                        MemoryMarshal.Write(_db.Slice(location + 5), ref row.HasChildren);
                        numberOfRowsForMembers += row.Length;
                        break;
                    case JsonTokenType.StartArray:
                        if (isArray)
                        {
                            arrayItemsCount++;
                        }
                        numberOfRowsForMembers++;
                        AppendDbRow(JsonType.Array);
                        row = new StackRow(true, arrayItemsCount, false, numberOfRowsForValues + 1);
                        while (!_stack.TryPush(row))
                        {
                            ResizeDb();
                        }
                        arrayItemsCount = 0;
                        numberOfRowsForValues = 0;
                        break;
                    case JsonTokenType.EndArray:
                        location = FindLocation(JsonType.Array);
                        MemoryMarshal.Write(_db.Slice(location), ref arrayItemsCount);
                        MemoryMarshal.Write(_db.Slice(location + 6), ref numberOfRowsForValues);
                        row = _stack.Pop();
                        MemoryMarshal.Write(_db.Slice(location + 5), ref row.HasChildren);
                        arrayItemsCount = row.Length;
                        numberOfRowsForValues += row.ArrayLength;
                        break;
                    case JsonTokenType.PropertyName:
                        numberOfRowsForValues++;
                        numberOfRowsForMembers++;
                        AppendDbRow(JsonType.PropertyName, _reader.Value.Length);
                        break;
                    case JsonTokenType.Value:
                        if (isArray)
                        {
                            arrayItemsCount++;
                        }
                        numberOfRowsForValues++;
                        numberOfRowsForMembers++;
                        AppendDbRow((JsonType)_reader.ValueType, _reader.Value.Length);
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

        private int ForwardPass(JsonType lookupType)
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

        private int BackwardPass(JsonType lookupType)
        {
            int rowStartOffset = _dbIndex - DbRow.Size;
            while (true)
            {
                DbRow row = MemoryMarshal.Read<DbRow>(_db.Slice(rowStartOffset));

                if (row.Length == -1 && row.Type == lookupType)
                {
                    return rowStartOffset + 4;
                }

                // TODO: Investigate performance impact of adding "skip" logic similar to ForwardPass
                rowStartOffset -= DbRow.Size;
            }
        }

        private int FindLocation(JsonType lookupType)
        {
            // No more JSON left to read, i.e. we are at the root node,
            // so it will always be faster to search going forward
            if (_reader.Index == _utf8Json.Length)
            {
                return ForwardPass(lookupType);
            }
            else
            {
                return BackwardPass(lookupType);
            }
        }

        /*private (int, int) FindLocationArray()
        {
            // No more JSON left to read, i.e. we are at the root node,
            // so it will always be faster to search going forward
            if (_reader.Index == _utf8Json.Length)
            {
                return ForwardPassArray();
            }
            else
            {
                return BackwardPassArray();
            }
        }

        private (int, int) ForwardPassArray()
        {
            int rowStartOffset = 0;
            while (true)
            {
                DbRow row = MemoryMarshal.Read<DbRow>(_db.Slice(rowStartOffset));

                if (row.Length == -1 && row.Type == JsonType.Array)
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

        private (int, int) BackwardPassArray()
        {
            int primitiveLength = 1;
            int rowStartOffset = _dbIndex - DbRow.Size;
            while (true)
            {
                DbRow row = MemoryMarshal.Read<DbRow>(_db.Slice(rowStartOffset));

                if (row.Length == -1 && row.Type == JsonType.Array)
                {
                    return (primitiveLength, rowStartOffset + 4);
                }
                if (row.Type <= JsonType.Array)
                    primitiveLength = -1;

                // TODO: Investigate performance impact of adding "skip" logic similar to ForwardPass
                rowStartOffset -= DbRow.Size;
            }
        }*/

        private void AppendDbRow(JsonType type, int LengthOrNumberOfRows = DbRow.UnknownNumberOfRows)
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
