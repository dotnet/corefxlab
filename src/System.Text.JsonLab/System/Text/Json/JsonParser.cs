// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Diagnostics;

namespace System.Text.JsonLab
{
    internal ref struct JsonParser
    {
        private readonly ReadOnlySpan<byte> _utf8Json; // TODO: this should be ReadOnlyMemory<byte>
        private readonly ArrayPool<byte> _pool;

        public JsonParser(ReadOnlySpan<byte> utf8Json, ArrayPool<byte> pool = null)
        {
            _utf8Json = utf8Json;
            _pool = pool ?? ArrayPool<byte>.Shared;
        }

        public JsonObject Parse()
        {
            var database = new CustomDb(_pool, DbRow.Size + _utf8Json.Length);
            var stack = new CustomStack(Utf8JsonReader.MaxDepth * StackRow.Size);
            var reader = new Utf8JsonReader(_utf8Json);

            bool inArray = false;
            int arrayItemsCount = 0;
            int numberOfRowsForMembers = 0;
            int numberOfRowsForValues = 0;
            int parentLocation = -1;

            while (reader.Read())
            {
                JsonTokenType tokenType = reader.TokenType;

                if (tokenType == JsonTokenType.StartObject)
                {
                    if (parentLocation != -1)
                        database.SetHasChildren(parentLocation);
                    parentLocation = database.Index;
                    if (inArray)
                        arrayItemsCount++;
                    numberOfRowsForValues++;
                    database.Append(JsonValueType.Object, reader.StartLocation);
                    var row = new StackRow(numberOfRowsForMembers + 1);
                    stack.Push(row);
                    numberOfRowsForMembers = 0;
                }
                else if (tokenType == JsonTokenType.EndObject)
                {
                    parentLocation = -1;
                    int rowIndex = reader.NoMoreData ? 0 : database.FindIndexOfFirstUnsetSizeOrLength(JsonValueType.Object);
                    database.SetLength(rowIndex, numberOfRowsForMembers);
                    if (numberOfRowsForMembers != 0)
                        database.SetNumberOfRows(rowIndex, numberOfRowsForMembers);
                    StackRow row = stack.Pop();
                    numberOfRowsForMembers += row.SizeOrLength;
                }
                else if (tokenType == JsonTokenType.StartArray)
                {
                    if (parentLocation != -1)
                        database.SetHasChildren(parentLocation);
                    parentLocation = database.Index;
                    if (inArray)
                        arrayItemsCount++;
                    numberOfRowsForMembers++;
                    database.Append(JsonValueType.Array, reader.StartLocation);
                    var row = new StackRow(arrayItemsCount, numberOfRowsForValues + 1);
                    stack.Push(row);
                    arrayItemsCount = 0;
                    numberOfRowsForValues = 0;
                }
                else if (tokenType == JsonTokenType.EndArray)
                {
                    parentLocation = -1;
                    int rowIndex = reader.NoMoreData ? 0 : database.FindIndexOfFirstUnsetSizeOrLength(JsonValueType.Array);
                    database.SetLength(rowIndex, arrayItemsCount);
                    if (numberOfRowsForValues != 0)
                        database.SetNumberOfRows(rowIndex, numberOfRowsForValues);
                    StackRow row = stack.Pop();
                    arrayItemsCount = row.SizeOrLength;
                    numberOfRowsForValues += row.NumberOfRows;
                }
                else
                {
                    Debug.Assert(tokenType == JsonTokenType.PropertyName || tokenType == JsonTokenType.Value);
                    numberOfRowsForValues++;
                    numberOfRowsForMembers++;
                    database.Append(reader.ValueType, reader.StartLocation, reader.Value.Length);
                    if (tokenType == JsonTokenType.Value && inArray)
                        arrayItemsCount++;
                }

                inArray = reader.InArray;
            }

            stack.Dispose();
            database.Resize();
            return new JsonObject(_utf8Json, database);
        }
    }
}
