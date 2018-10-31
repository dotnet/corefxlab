// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
            var stack = new CustomStack(JsonUtf8Reader.StackFreeMaxDepth * StackRow.Size);
            var json = new Utf8Json();
            var reader = json.GetReader(_utf8Json);

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
                    database.Append(JsonType.StartObject, reader.TokenStartIndex);
                    var row = new StackRow(numberOfRowsForMembers + 1);
                    stack.Push(row);
                    numberOfRowsForMembers = 0;
                }
                else if (tokenType == JsonTokenType.EndObject)
                {
                    parentLocation = -1;
                    int rowIndex = reader.ConsumedEverything ? 0 : database.FindIndexOfFirstUnsetSizeOrLength(JsonType.StartObject);
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
                    database.Append(JsonType.StartArray, reader.TokenStartIndex);
                    var row = new StackRow(arrayItemsCount, numberOfRowsForValues + 1);
                    stack.Push(row);
                    arrayItemsCount = 0;
                    numberOfRowsForValues = 0;
                }
                else if (tokenType == JsonTokenType.EndArray)
                {
                    parentLocation = -1;
                    int rowIndex = reader.ConsumedEverything ? 0 : database.FindIndexOfFirstUnsetSizeOrLength(JsonType.StartArray);
                    database.SetLength(rowIndex, arrayItemsCount);
                    if (numberOfRowsForValues != 0)
                        database.SetNumberOfRows(rowIndex, numberOfRowsForValues);
                    StackRow row = stack.Pop();
                    arrayItemsCount = row.SizeOrLength;
                    numberOfRowsForValues += row.NumberOfRows;
                }
                else if (tokenType == JsonTokenType.PropertyName)
                {
                    numberOfRowsForValues++;
                    numberOfRowsForMembers++;
                    database.Append(JsonType.String, reader.TokenStartIndex, reader.Value.Length);
                    if (inArray)
                        arrayItemsCount++;
                }
                else
                {
                    Debug.Assert(tokenType >= JsonTokenType.String && tokenType <= JsonTokenType.Null);
                    numberOfRowsForValues++;
                    numberOfRowsForMembers++;
                    database.Append((JsonType)(tokenType - 4), reader.TokenStartIndex, reader.Value.Length);
                    if (inArray)
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
