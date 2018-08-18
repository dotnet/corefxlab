// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace System.Text.JsonLab
{
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
}
