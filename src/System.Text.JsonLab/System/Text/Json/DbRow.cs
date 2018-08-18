// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
}
