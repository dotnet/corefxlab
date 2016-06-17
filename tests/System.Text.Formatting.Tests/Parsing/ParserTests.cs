using System.Runtime.InteropServices;
using System.Text.Parsing;
using Xunit;

namespace System.Text.Formatting.Tests
{
    public class ParserTests
    {
        #region ulong
        [Theory]
        [InlineData(new byte[] { 0x31, 0x37, 0x32, 0x38 } /* "1728" */, true, 0, 1728, 4)]
        [InlineData(new byte[] { 0xCF, 0xA0, 0x20, 0xDF, 0xB7, 0xDE, 0x96, 0xDD, 0xAE, 0x31, 0x37, 0x35, 0x31, 0x31, 0x31, 0x30 }, // stuff + "1751110"
            true, 9, 1751110, 7)]
        [InlineData(new byte[] { 0x39, 0x38, 0x37, 0x66, 0x61, 0x64, 0x66, 0x61, 0x6B, 0x6A, 0x6C, 0x66, 0x68, 0x6B }, // "987" + trailing text
            true, 0, 987, 3)]
        // "The biggest ulong is 9223372036854775808."
        [InlineData(new byte[] { 0x54, 0x68, 0x65, 0x20, 0x62, 0x69, 0x67, 0x67, 0x65, 0x73, 0x74, 0x20, 0x75, 0x6C, 0x6F, 0x6E, 0x67, 0x20, 0x69, 0x73, 0x20, 0x39, 0x32, 0x32, 0x33, 0x33, 0x37, 0x32, 0x30, 0x33, 0x36, 0x38, 0x35, 0x34, 0x37, 0x37, 0x35, 0x38, 0x30, 0x38, 0x2E },
            true, 21, 9223372036854775808, 19)]
        [InlineData(new byte[] { 0x49, 0x20, 0x61, 0x6D, 0x20, 0x31 } /* I am 1 */, false, 0, 0, 0)]
        public void ParseUtf8ByteArrayToUlong(byte[] text, bool expectSuccess, int index, ulong expectedValue, int expectedBytesConsumed)
        {
            ulong parsedValue;
            int bytesConsumed;
            bool result = InvariantParser.TryParse(text, index, out parsedValue, out bytesConsumed);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedBytesConsumed, bytesConsumed);
        }

        [Theory]
        [InlineData(new byte[] { 0x31, 0x37, 0x32, 0x38 } /* "1728" */, true, 0, 1728, 4)]
        [InlineData(new byte[] { 0xCF, 0xA0, 0x20, 0xDF, 0xB7, 0xDE, 0x96, 0xDD, 0xAE, 0x31, 0x37, 0x35, 0x31, 0x31, 0x31, 0x30 }, // stuff + "1751110"
            true, 9, 1751110, 7)]
        [InlineData(new byte[] { 0x39, 0x38, 0x37, 0x66, 0x61, 0x64, 0x66, 0x61, 0x6B, 0x6A, 0x6C, 0x66, 0x68, 0x6B }, // "987" + trailing text
            true, 0, 987, 3)]
        // "The biggest ulong is 9223372036854775808."
        [InlineData(new byte[] { 0x54, 0x68, 0x65, 0x20, 0x62, 0x69, 0x67, 0x67, 0x65, 0x73, 0x74, 0x20, 0x75, 0x6C, 0x6F, 0x6E, 0x67, 0x20, 0x69, 0x73, 0x20, 0x39, 0x32, 0x32, 0x33, 0x33, 0x37, 0x32, 0x30, 0x33, 0x36, 0x38, 0x35, 0x34, 0x37, 0x37, 0x35, 0x38, 0x30, 0x38, 0x2E },
            true, 21, 9223372036854775808, 19)]
        [InlineData(new byte[] { 0x49, 0x20, 0x61, 0x6D, 0x20, 0x31 } /* I am 1 */, false, 0, 0, 0)]
        public unsafe void ParseUtf8ByteStarToUlong(byte[] text, bool expectSuccess, int index, ulong expectedValue, int expectedBytesConsumed)
        {
            ulong parsedValue;
            int bytesConsumed;

            GCHandle arrayPinned = GCHandle.Alloc(text, GCHandleType.Pinned);
            byte* arrayPointer = (byte*)arrayPinned.AddrOfPinnedObject();

            bool result = InvariantParser.TryParse(arrayPointer, index, out parsedValue, out bytesConsumed);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedBytesConsumed, bytesConsumed);
        }
        #endregion

        #region uint
        [Theory]
        [InlineData(new byte[] { 0x31, 0x37, 0x32, 0x38 } /* "1728" */, true, 0, 1728, 4)]
        [InlineData(new byte[] { 0xCF, 0xA0, 0x20, 0xDF, 0xB7, 0xDE, 0x96, 0xDD, 0xAE, 0x31, 0x37, 0x35, 0x31, 0x31, 0x31, 0x30 }, // stuff + "1751110"
            true, 9, 1751110, 7)]
        [InlineData(new byte[] { 0x39, 0x38, 0x37, 0x66, 0x61, 0x64, 0x66, 0x61, 0x6B, 0x6A, 0x6C, 0x66, 0x68, 0x6B }, // "987" + trailing text
            true, 0, 987, 3)]
        [InlineData(new byte[] { 0x49, 0x20, 0x61, 0x6D, 0x20, 0x31 } /* I am 1 */, false, 0, 0, 0)]
        public void ParseUtf8ByteArrayToUint(byte[] text, bool expectSuccess, int index, uint expectedValue, int expectedBytesConsumed)
        {
            uint parsedValue;
            int bytesConsumed;
            bool result = InvariantParser.TryParse(text, index, out parsedValue, out bytesConsumed);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedBytesConsumed, bytesConsumed);
        }

        [Theory]
        [InlineData(new byte[] { 0x31, 0x37, 0x32, 0x38 } /* "1728" */, true, 0, 1728, 4)]
        [InlineData(new byte[] { 0xCF, 0xA0, 0x20, 0xDF, 0xB7, 0xDE, 0x96, 0xDD, 0xAE, 0x31, 0x37, 0x35, 0x31, 0x31, 0x31, 0x30 }, // stuff + "1751110"
            true, 9, 1751110, 7)]
        [InlineData(new byte[] { 0x39, 0x38, 0x37, 0x66, 0x61, 0x64, 0x66, 0x61, 0x6B, 0x6A, 0x6C, 0x66, 0x68, 0x6B }, // "987" + trailing text
            true, 0, 987, 3)]
        [InlineData(new byte[] { 0x49, 0x20, 0x61, 0x6D, 0x20, 0x31 } /* I am 1 */, false, 0, 0, 0)]
        public unsafe void ParseUtf8ByteStarToUint(byte[] text, bool expectSuccess, int index, uint expectedValue, int expectedBytesConsumed)
        {
            uint parsedValue;
            int bytesConsumed;

            GCHandle arrayPinned = GCHandle.Alloc(text, GCHandleType.Pinned);
            byte* arrayPointer = (byte*)arrayPinned.AddrOfPinnedObject();

            bool result = InvariantParser.TryParse(arrayPointer, index, out parsedValue, out bytesConsumed);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedBytesConsumed, bytesConsumed);
        }
        #endregion

        #region ushort
        [Theory]
        [InlineData(new byte[] { 0x31, 0x37, 0x32, 0x38 } /* "1728" */, true, 0, 1728, 4)]
        [InlineData(new byte[] { 0xCF, 0xA0, 0x20, 0xDF, 0xB7, 0xDE, 0x96, 0xDD, 0xAE, 0x33, 0x37, 0x35, 0x31, 0x31 }, // stuff + "37511"
            true, 9, 37511, 5)]
        [InlineData(new byte[] { 0x39, 0x38, 0x37, 0x66, 0x61, 0x64, 0x66, 0x61, 0x6B, 0x6A, 0x6C, 0x66, 0x68, 0x6B }, // "987" + trailing text
            true, 0, 987, 3)]
        [InlineData(new byte[] { 0x49, 0x20, 0x61, 0x6D, 0x20, 0x31 } /* I am 1 */, false, 0, 0, 0)]
        public void ParseUtf8ByteArrayToUshort(byte[] text, bool expectSuccess, int index, ushort expectedValue, int expectedBytesConsumed)
        {
            ushort parsedValue;
            int bytesConsumed;
            bool result = InvariantParser.TryParse(text, index, out parsedValue, out bytesConsumed);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedBytesConsumed, bytesConsumed);
        }

        [Theory]
        [InlineData(new byte[] { 0x31, 0x37, 0x32, 0x38 } /* "1728" */, true, 0, 1728, 4)]
        [InlineData(new byte[] { 0xCF, 0xA0, 0x20, 0xDF, 0xB7, 0xDE, 0x96, 0xDD, 0xAE, 0x33, 0x37, 0x35, 0x31, 0x31 }, // stuff + "37511"
            true, 9, 37511, 5)]
        [InlineData(new byte[] { 0x39, 0x38, 0x37, 0x66, 0x61, 0x64, 0x66, 0x61, 0x6B, 0x6A, 0x6C, 0x66, 0x68, 0x6B }, // "987" + trailing text
            true, 0, 987, 3)]
        [InlineData(new byte[] { 0x49, 0x20, 0x61, 0x6D, 0x20, 0x31 } /* I am 1 */, false, 0, 0, 0)]
        public unsafe void ParseUtf8ByteStarToUshort(byte[] text, bool expectSuccess, int index, ushort expectedValue, int expectedBytesConsumed)
        {
            ushort parsedValue;
            int bytesConsumed;

            GCHandle arrayPinned = GCHandle.Alloc(text, GCHandleType.Pinned);
            byte* arrayPointer = (byte*)arrayPinned.AddrOfPinnedObject();

            bool result = InvariantParser.TryParse(arrayPointer, index, out parsedValue, out bytesConsumed);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedBytesConsumed, bytesConsumed);
        }
        #endregion

        #region byte
        [Theory]
        [InlineData(new byte[] { 0x31, 0x37, 0x32 } /* "178" */, true, 0, 172, 3)]
        [InlineData(new byte[] { 0xCF, 0xA0, 0x20, 0xDF, 0xB7, 0xDE, 0x96, 0xDD, 0xAE, 0x33, 0x37 }, // stuff + "37"
            true, 9, 37, 2)]
        [InlineData(new byte[] { 0x31, 0x38, 0x37, 0x66, 0x61, 0x64, 0x66, 0x61, 0x6B, 0x6A, 0x6C, 0x66, 0x68, 0x6B }, // "187" + trailing text
            true, 0, 187, 3)]
        [InlineData(new byte[] { 0x49, 0x20, 0x61, 0x6D, 0x20, 0x31 } /* I am 1 */, false, 0, 0, 0)]
        public unsafe void ParseUtf8ByteArrayToByte(byte[] text, bool expectSuccess, int index, byte expectedValue, int expectedBytesConsumed)
        {
            byte parsedValue;
            int bytesConsumed;
            bool result = InvariantParser.TryParse(text, index, out parsedValue, out bytesConsumed);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedBytesConsumed, bytesConsumed);
        }

        [Theory]
        [InlineData(new byte[] { 0x31, 0x37, 0x32 } /* "172" */, true, 0, 172, 3)]
        [InlineData(new byte[] { 0xCF, 0xA0, 0x20, 0xDF, 0xB7, 0xDE, 0x96, 0xDD, 0xAE, 0x33, 0x37 }, // stuff + "37"
            true, 9, 37, 2)]
        [InlineData(new byte[] { 0x31, 0x38, 0x37, 0x66, 0x61, 0x64, 0x66, 0x61, 0x6B, 0x6A, 0x6C, 0x66, 0x68, 0x6B }, // "187" + trailing text
            true, 0, 187, 3)]
        [InlineData(new byte[] { 0x49, 0x20, 0x61, 0x6D, 0x20, 0x31 } /* I am 1 */, false, 0, 0, 0)]
        public unsafe void ParseUtf8ByteStarToByte(byte[] text, bool expectSuccess, int index, byte expectedValue, int expectedBytesConsumed)
        {
            byte parsedValue;
            int bytesConsumed;

            GCHandle arrayPinned = GCHandle.Alloc(text, GCHandleType.Pinned);
            byte* arrayPointer = (byte*)arrayPinned.AddrOfPinnedObject();

            bool result = InvariantParser.TryParse(arrayPointer, index, out parsedValue, out bytesConsumed);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedBytesConsumed, bytesConsumed);
        }
        #endregion

        #region int
        [Theory]
        [InlineData(new byte[] { 0x31, 0x37, 0x32, 0x38 } /* "1728" */, true, 0, 1728, 4)]
        [InlineData(new byte[] { 0xCF, 0xA0, 0x20, 0xDF, 0xB7, 0xDE, 0x96, 0xDD, 0xAE, 0x31, 0x37, 0x35, 0x31, 0x31, 0x31, 0x30 }, // stuff + "1751110"
            true, 9, 1751110, 7)]
        [InlineData(new byte[] { 0x39, 0x38, 0x37, 0x66, 0x61, 0x64, 0x66, 0x61, 0x6B, 0x6A, 0x6C, 0x66, 0x68, 0x6B }, // "987" + trailing text
            true, 0, 987, 3)]
        // "Letthem-32984eatcake"
        [InlineData(new byte[] { 0x4C, 0x65, 0x74, 0x74, 0x68, 0x65, 0x6D, 0x2D, 0x33, 0x32, 0x39, 0x38, 0x34, 0x65, 0x61, 0x74, 0x63, 0x61, 0x6B, 0x65 },
            true, 7, -32984, 6)]
        [InlineData(new byte[] { 0x49, 0x20, 0x61, 0x6D, 0x20, 0x31 } /* I am 1 */, false, 0, 0, 0)]
        public void ParseUtf8ByteArrayToInt(byte[] text, bool expectSuccess, int index, int expectedValue, int expectedBytesConsumed)
        {
            int parsedValue;
            int bytesConsumed;
            bool result = InvariantParser.TryParse(text, index, out parsedValue, out bytesConsumed);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedBytesConsumed, bytesConsumed);
        }

        [Theory]
        [InlineData(new byte[] { 0x31, 0x37, 0x32, 0x38 } /* "1728" */, true, 0, 1728, 4)]
        [InlineData(new byte[] { 0xCF, 0xA0, 0x20, 0xDF, 0xB7, 0xDE, 0x96, 0xDD, 0xAE, 0x31, 0x37, 0x35, 0x31, 0x31, 0x31, 0x30 }, // stuff + "1751110"
            true, 9, 1751110, 7)]
        [InlineData(new byte[] { 0x39, 0x38, 0x37, 0x66, 0x61, 0x64, 0x66, 0x61, 0x6B, 0x6A, 0x6C, 0x66, 0x68, 0x6B }, // "987" + trailing text
            true, 0, 987, 3)]
        // "Letthem-32984eatcake"
        [InlineData(new byte[] { 0x4C, 0x65, 0x74, 0x74, 0x68, 0x65, 0x6D, 0x2D, 0x33, 0x32, 0x39, 0x38, 0x34, 0x65, 0x61, 0x74, 0x63, 0x61, 0x6B, 0x65 },
            true, 7, -32984, 6)]
        [InlineData(new byte[] { 0x49, 0x20, 0x61, 0x6D, 0x20, 0x31 } /* I am 1 */, false, 0, 0, 0)]
        public unsafe void ParseUtf8ByteStarToInt(byte[] text, bool expectSuccess, int index, int expectedValue, int expectedBytesConsumed)
        {
            int parsedValue;
            int bytesConsumed;

            GCHandle arrayPinned = GCHandle.Alloc(text, GCHandleType.Pinned);
            byte* arrayPointer = (byte*)arrayPinned.AddrOfPinnedObject();

            bool result = InvariantParser.TryParse(arrayPointer, index, out parsedValue, out bytesConsumed);

            Assert.Equal(expectSuccess, result);
            Assert.Equal(expectedValue, parsedValue);
            Assert.Equal(expectedBytesConsumed, bytesConsumed);
        }
        #endregion
    }
}
