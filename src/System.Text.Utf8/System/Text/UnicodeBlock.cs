// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text
{
    /// <summary>
    /// A named, contiguous range of Unicode code points.
    /// </summary>
    public struct UnicodeBlock
    {
        public UnicodeBlock(int firstCodePoint, int length) => throw null;

        public int FirstCodePoint => throw null;

        public int Length => throw null;

        public int Plane => throw null;

        public bool Contains(char value) => throw null;

        public bool Contains(int codePoint) => throw null;

        public bool Contains(UnicodeScalar value) => throw null;

        public string GetName() => throw null;

        public static bool TryGetBlockForCharacter(char value, out UnicodeBlock block) => throw null;

        public static bool TryGetBlockForCodePoint(int codePoint, out UnicodeBlock block) => throw null;

        public static bool TryGetBlockForScalar(UnicodeScalar value, out UnicodeBlock block) => throw null;
    }
}
