// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace System.Text.JsonLab
{
    // SizeOrLength - offset - 0 - size - 4
    // NumberOfRows - offset - 4 - size - 4
    [StructLayout(LayoutKind.Sequential)]
    internal struct StackRow
    {
        public const int Size = 8;

        public int SizeOrLength;
        public int NumberOfRows;

        public StackRow(int sizeOrLength = 0, int numberOfRows = -1)
        {
            Debug.Assert(sizeOrLength >= 0);
            Debug.Assert(numberOfRows >= -1);

            SizeOrLength = sizeOrLength;
            NumberOfRows = numberOfRows;
        }
    }
}
