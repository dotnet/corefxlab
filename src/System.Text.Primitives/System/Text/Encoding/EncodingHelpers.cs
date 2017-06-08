// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Text
{
    static class EncodingHelper
    {
        public const char HighSurrogateStart = '\ud800';
        public const char HighSurrogateEnd = '\udbff';
        public const char LowSurrogateStart = '\udc00';
        public const char LowSurrogateEnd = '\udfff';

        public unsafe static int PtrDiff(char* a, char* b)
        {
            return (int)(((uint)((byte*)a - (byte*)b)) >> 1);
        }

        // byte* flavor just for parity
        public unsafe static int PtrDiff(byte* a, byte* b)
        {
            return (int)(a - b);
        }

        public static bool InRange(int ch, int start, int end)
        {
            return (uint)(ch - start) <= (uint)(end - start);
        }
    }
}
