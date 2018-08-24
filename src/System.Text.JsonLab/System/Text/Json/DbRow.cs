// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace System.Text.JsonLab
{
    // Location - offset - 0 - size - 4
    // Length - offset - 4 - size - 4
    // Type - offset - 8 - size - 1
    // HasChildren - offset - 9 - size - 1
    // ArrayLength - offset - 10 - size - 4
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct DbRow
    {
        public static int Size;

        public int Location;                // index in JSON payload
        public int Length;      // length of text in JSON payload
        public JsonType Type; // type of JSON construct (e.g. Object, Array, Number)
        public bool HasChildren;
        public int ArrayLength;

        public const int UnknownNumberOfRows = -1;

        public DbRow(JsonType type, int valueIndex, int lengthOrNumberOfRows = UnknownNumberOfRows, int arrayNumberOfRows = UnknownNumberOfRows, bool hasChildren = false)
        {
            Location = valueIndex;
            Length = lengthOrNumberOfRows;
            Type = type;
            ArrayLength = arrayNumberOfRows;
            HasChildren = hasChildren;
        }

        public bool IsSimpleValue => Type > JsonType.Array; // Type >= 0b0010_00000...<28 times>

        unsafe static DbRow()
        {
            Size = sizeof(DbRow);
        }
    }

    // Do not change the order of the enum values, since IsSimpleValue relies on it.
    public enum JsonType : byte
    {
        Object = 0,
        Array = 1,
        String = 2,
        Number = 3,
        True = 4,
        False = 5,
        Null = 6,
        Unknown = 7,
        PropertyName = 8
    }
}
