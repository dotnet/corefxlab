// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;

namespace System.Text.Json.Tests
{
    public partial class JsonPerfTests
    {
        const int BufferSize = 1024 + (ExtraArraySize * 64);

        public enum EncoderTarget
        {
            InvariantUtf8,
            InvariantUtf16,
            SlowUtf8,
            SlowUtf16
        }

        static TextEncoder GetTargetEncoder(EncoderTarget target)
        {
            switch (target)
            {
                case EncoderTarget.InvariantUtf8:
                    return TextEncoder.Utf8;
                case EncoderTarget.InvariantUtf16:
                    return TextEncoder.Utf16;
                case EncoderTarget.SlowUtf8:
                    return TextEncoder.CreateUtf8Encoder(Utf8DigitsAndSymbols);
                case EncoderTarget.SlowUtf16:
                    return TextEncoder.CreateUtf16Encoder(Utf16DigitsAndSymbols);
                default:
                    Assert.True(false, "Invalid encoder targetted in test");
                    return null;
            }
        }

        // Copy of UTF-8 symbol table for creating a "non-invariant" version of it
        // to force the slow path.
        private static readonly byte[][] Utf8DigitsAndSymbols = new byte[][]
        {
            new byte[] { 48, },
            new byte[] { 49, },
            new byte[] { 50, },
            new byte[] { 51, },
            new byte[] { 52, },
            new byte[] { 53, },
            new byte[] { 54, },
            new byte[] { 55, },
            new byte[] { 56, },
            new byte[] { 57, }, // digit 9
            new byte[] { 46, }, // decimal separator
            new byte[] { 44, }, // group separator
            new byte[] { 73, 110, 102, 105, 110, 105, 116, 121, },
            new byte[] { 45, }, // minus sign
            new byte[] { 43, }, // plus sign
            new byte[] { 78, 97, 78, }, // NaN
            new byte[] { 69, }, // E
            new byte[] { 101, }, // e
        };

        // Copy of UTF-16 symbol table for creating a "non-invariant" version of it
        // to force the slow path.
        private static readonly byte[][] Utf16DigitsAndSymbols = new byte[][]
        {
            new byte[] { 48, 0, }, // digit 0
            new byte[] { 49, 0, },
            new byte[] { 50, 0, },
            new byte[] { 51, 0, },
            new byte[] { 52, 0, },
            new byte[] { 53, 0, },
            new byte[] { 54, 0, },
            new byte[] { 55, 0, },
            new byte[] { 56, 0, },
            new byte[] { 57, 0, }, // digit 9
            new byte[] { 46, 0, }, // decimal separator
            new byte[] { 44, 0, }, // group separator
            new byte[] { 73, 0, 110, 0, 102, 0, 105, 0, 110, 0, 105, 0, 116, 0, 121, 0, }, // Infinity
            new byte[] { 45, 0, }, // minus sign 
            new byte[] { 43, 0, }, // plus sign 
            new byte[] { 78, 0, 97, 0, 78, 0, }, // NaN
            new byte[] { 69, 0, }, // E
            new byte[] { 101, 0, }, // e
        };
    }
}
