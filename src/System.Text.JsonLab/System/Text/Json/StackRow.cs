// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace System.Text.JsonLab
{
    // IsArray - offset - 0 - size - 1
    // Length - offset - 1 - size - 4
    // HasChildren - offset - 5 - size - 1
    // ArrayLength - offset - 6 - size - 4
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct StackRow
    {
        public static int Size;

        public bool IsArray;
        public int Length;
        public bool HasChildren;
        public int ArrayLength;

        public StackRow(bool isArray, int lengthOrNumberOfRows, bool hasChildren, int arrayLength)
        {
            IsArray = isArray;
            Length = lengthOrNumberOfRows;
            HasChildren = hasChildren;
            ArrayLength = arrayLength;
        }

        unsafe static StackRow()
        {
            Size = sizeof(StackRow);
        }
    }
}
