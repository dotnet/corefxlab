// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.Primitives.Tests.Encoding
{
    public static class TextEncoderConstants
    {
        public const ushort Utf16HighSurrogateFirstCodePoint = 0xD800;
        public const ushort Utf16HighSurrogateLastCodePoint = 0xDBFF;
        public const ushort Utf16LowSurrogateFirstCodePoint = 0xDC00;
        public const ushort Utf16LowSurrogateLastCodePoint = 0xDFFF;

        public const uint LastValidCodePoint = 0x10FFFF;

        public const byte Utf8OneByteLastCodePoint = 0x7F;
        public const ushort Utf8TwoBytesLastCodePoint = 0x7FF;
        public const ushort Utf8ThreeBytesLastCodePoint = 0xFFFF;

        public const int DataLength = 999;  // Used as length of input string generated for encoding tests

        public const int RandomSeed = 42;
    }
}
