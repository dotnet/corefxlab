﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace System.Text.JsonLab
{
    // Location - offset - 0 - size - 4
    // SizeOrLength - offset - 4 - size - 4
    // _union - HasChildren | JsonType | NumberOfRows - offset - 8 - size - 4
    [StructLayout(LayoutKind.Sequential)]
    internal struct DbRow
    {
        public const int Size = 16;

        public int Location;                // index in JSON payload
        public int SizeOrLength;      // length of text in JSON payload (or number of elements if its a JSON array)

        // The highest order bit indicates if the JSON element has children
        // The next 3 bits indicate the json type (there are 2^3 = 8 such types)
        // The last 28 bits indicate the number of rows.
        // Since each row of the database is 12 bytes long, we can't exceed 2^31/12 = 178956971 total number of rows.
        // Hence, we only need 28 bits (maximum value of 268435455).
        private readonly int _union;

        public int ArrayUniformLength;

        public bool HasChildren => _union < 0;  // True only if there are nested objects or arrays within the current JSON element
        public JsonValueType JsonType => (JsonValueType)((_union & 0x70000000) >> 28); // type of JSON construct (e.g. Object, Array, Number)
        public int NumberOfRows => _union & 0x0FFFFFFF; // Number of rows that the current JSON element occupies within the database

        internal const int UnknownSize = -1;

        public DbRow(JsonValueType jsonType, int location, int sizeOrLength = UnknownSize, int numberOfRows = 1)
        {
            Debug.Assert(jsonType >= JsonValueType.Object && jsonType <= JsonValueType.Unknown);
            Debug.Assert(location >= 0);
            Debug.Assert(sizeOrLength >= UnknownSize);
            Debug.Assert(numberOfRows >= 1 && numberOfRows <= 0x0FFFFFFF);

            Location = location;
            SizeOrLength = sizeOrLength;
            _union = numberOfRows | (int)jsonType << 28; // HasChildren is set to false by default
            ArrayUniformLength = 1;
        }

        public bool IsSimpleValue => JsonType > JsonValueType.Array;
    }
}
