// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Text
{
    internal static class UnicodeConstants
    {
        // TODO: Some of these members are needed only in Utf16LittleEndianEncoder.
        //       Should we add the usage of them to UnicodeCodePoint class and merge this class with it?
        public const uint HighSurrogateFirstCodePoint = 0xD800;
        public const uint HighSurrogateLastCodePoint = 0xDFFF;
        public const uint LowSurrogateFirstCodePoint = 0xDC00;
        public const uint LowSurrogateLastCodePoint = 0xDFFF;

        public const uint SurrogateRangeStart = HighSurrogateFirstCodePoint;
        public const uint SurrogateRangeEnd = LowSurrogateLastCodePoint;

        public const uint FirstNotSupportedCodePoint = 0x110000; // 17 * 2^16
    }
}
