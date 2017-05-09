// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System.Text.Primitives.Tests.Encoding
{
    public class EncodeIntoUtf8Tests : ITextEncoderTest
    {
        private static TextEncoder utf8 = TextEncoder.Utf8;
        private static Text.Encoding testEncoder = Text.Encoding.UTF8;
        private static Text.Encoding testEncoderUnicode = Text.Encoding.Unicode;
        private static Text.Encoding testEncoderUtf32 = Text.Encoding.UTF32;

        public static object[] SupportedEncodingTestData =
        {
            new object[] { TextEncoderTestHelper.SupportedEncoding.FromUtf8 },
            new object[] { TextEncoderTestHelper.SupportedEncoding.FromUtf16 },
            new object[] { TextEncoderTestHelper.SupportedEncoding.FromString },
            new object[] { TextEncoderTestHelper.SupportedEncoding.FromUtf32 },
        };

        [Theory]
        //[MemberData(nameof(SupportedEncodingTestData))]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf8)]
        //[InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf16)]  // Open issue: https://github.com/dotnet/corefxlab/issues/1524
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromString)]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf32)]
        public void InputBufferEmpty(TextEncoderTestHelper.SupportedEncoding from)
        {
            string inputString = TextEncoderTestHelper.GenerateValidString(0, 0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint);
            
            byte[] expectedBytes;
            Span<byte> encodedBytes;

            int bytesWritten;

            switch (from)
            {
                case TextEncoderTestHelper.SupportedEncoding.FromUtf8:
                    byte[] temp = testEncoder.GetBytes(inputString);
                    expectedBytes = Text.Encoding.Convert(testEncoder, testEncoder, temp);
                    encodedBytes = Span<byte>.Empty;    // Output buffer is size 0
                    Assert.True(utf8.TryEncode(ReadOnlySpan<byte>.Empty, encodedBytes, out int charactersConsumed, out bytesWritten));
                    Assert.Equal(0, charactersConsumed);
                    encodedBytes = new Span<byte>(new byte[1]);    // Output buffer is not size 0
                    Assert.True(utf8.TryEncode(ReadOnlySpan<byte>.Empty, encodedBytes, out charactersConsumed, out bytesWritten));
                    Assert.Equal(0, charactersConsumed);
                    break;

                case TextEncoderTestHelper.SupportedEncoding.FromUtf16:
                    temp = testEncoderUnicode.GetBytes(inputString);
                    expectedBytes = Text.Encoding.Convert(testEncoderUnicode, testEncoder, temp);
                    encodedBytes = Span<byte>.Empty;
                    Assert.True(utf8.TryEncode(ReadOnlySpan<char>.Empty, encodedBytes, out charactersConsumed, out bytesWritten));
                    Assert.Equal(0, charactersConsumed);
                    encodedBytes = new Span<byte>(new byte[1]);
                    Assert.True(utf8.TryEncode(ReadOnlySpan<char>.Empty, encodedBytes, out charactersConsumed, out bytesWritten));
                    Assert.Equal(0, charactersConsumed);
                    break;

                case TextEncoderTestHelper.SupportedEncoding.FromString:
                    temp = testEncoderUnicode.GetBytes(inputString);
                    expectedBytes = Text.Encoding.Convert(testEncoderUnicode, testEncoder, temp);
                    encodedBytes = Span<byte>.Empty;
                    Assert.True(utf8.TryEncode("", encodedBytes, out bytesWritten));
                    encodedBytes = new Span<byte>(new byte[1]);
                    Assert.True(utf8.TryEncode("", encodedBytes, out bytesWritten));
                    break;

                case TextEncoderTestHelper.SupportedEncoding.FromUtf32:
                default:
                    temp = testEncoderUtf32.GetBytes(inputString);
                    expectedBytes = Text.Encoding.Convert(testEncoderUtf32, testEncoder, temp);
                    encodedBytes = Span<byte>.Empty;
                    Assert.True(utf8.TryEncode(ReadOnlySpan<uint>.Empty, encodedBytes, out charactersConsumed, out bytesWritten));
                    Assert.Equal(0, charactersConsumed);
                    encodedBytes = new Span<byte>(new byte[1]);
                    Assert.True(utf8.TryEncode(ReadOnlySpan<uint>.Empty, encodedBytes, out charactersConsumed, out bytesWritten));
                    Assert.Equal(0, charactersConsumed);
                    break;
            }

            Assert.Equal(expectedBytes.Length, bytesWritten);
            Assert.True(expectedBytes.AsSpan().SequenceEqual(encodedBytes.Slice(0, bytesWritten)));
        }

        [Theory]
        //[MemberData(nameof(SupportedEncodingTestData))]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf8)]
        //[InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf16)]  // Open issue: https://github.com/dotnet/corefxlab/issues/1525
        //[InlineData(TextEncoderTestHelper.SupportedEncoding.FromString)]  // Open issue: https://github.com/dotnet/corefxlab/issues/1525
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf32)]
        public void OutputBufferEmpty(TextEncoderTestHelper.SupportedEncoding from)
        {
            string inputString = TextEncoderTestHelper.GenerateValidString(TextEncoderConstants.DataLength, 0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint);

            byte[] expectedBytes;
            Span<byte> encodedBytes = Span<byte>.Empty;

            int bytesWritten;
            int charactersConsumed;

            switch (from)
            {
                case TextEncoderTestHelper.SupportedEncoding.FromUtf8:
                    byte[] temp = testEncoder.GetBytes(inputString);
                    expectedBytes = Text.Encoding.Convert(testEncoder, testEncoder, temp);
                    ReadOnlySpan<byte> inputUtf8 = temp;
                    Assert.False(utf8.TryEncode(inputUtf8, encodedBytes, out charactersConsumed, out bytesWritten));
                    break;

                case TextEncoderTestHelper.SupportedEncoding.FromUtf16:
                    temp = testEncoderUnicode.GetBytes(inputString);
                    expectedBytes = Text.Encoding.Convert(testEncoderUnicode, testEncoder, temp);
                    ReadOnlySpan<char> inputUtf16 = temp.AsSpan().NonPortableCast<byte, char>();
                    Assert.False(utf8.TryEncode(inputUtf16, encodedBytes, out charactersConsumed, out bytesWritten));
                    break;

                case TextEncoderTestHelper.SupportedEncoding.FromString:
                    temp = testEncoderUnicode.GetBytes(inputString);
                    expectedBytes = Text.Encoding.Convert(testEncoderUnicode, testEncoder, temp);
                    string inputStr = inputString;
                    Assert.False(utf8.TryEncode(inputStr, encodedBytes, out bytesWritten));
                    charactersConsumed = 0;
                    break;

                case TextEncoderTestHelper.SupportedEncoding.FromUtf32:
                default:
                    temp = testEncoderUtf32.GetBytes(inputString);
                    expectedBytes = Text.Encoding.Convert(testEncoderUtf32, testEncoder, temp);
                    ReadOnlySpan<uint> input = temp.AsSpan().NonPortableCast<byte, uint>();
                    Assert.False(utf8.TryEncode(input, encodedBytes, out charactersConsumed, out bytesWritten));
                    break;
            }

            Assert.Equal(0, charactersConsumed);
            Assert.Equal(0, bytesWritten);
            Assert.True(Span<byte>.Empty.SequenceEqual(encodedBytes));
        }

        [Theory]
        //[MemberData(nameof(SupportedEncodingTestData))]
        // [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf8)] // Open issue: https://github.com/dotnet/corefxlab/issues/1514
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf16)]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromString)]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf32)]
        public void InputBufferLargerThanOutputBuffer(TextEncoderTestHelper.SupportedEncoding from)
        {
            string inputString = TextEncoderTestHelper.GenerateValidString(TextEncoderConstants.DataLength, 0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint);

            byte[] expectedBytes;
            Span<byte> encodedBytes1;
            Span<byte> encodedBytes2;

            int expectedBytesWritten;

            int bytesWritten1;
            int bytesWritten2;

            switch (from)
            {
                case TextEncoderTestHelper.SupportedEncoding.FromUtf8:
                    byte[] temp = testEncoder.GetBytes(inputString);
                    expectedBytes = Text.Encoding.Convert(testEncoder, testEncoder, temp);
                    expectedBytesWritten = expectedBytes.Length;
                    encodedBytes1 = new Span<byte>(new byte[expectedBytesWritten / 2]);
                    ReadOnlySpan<byte> inputUtf8 = temp;
                    Assert.False(utf8.TryEncode(inputUtf8, encodedBytes1, out int charactersConsumed1, out bytesWritten1));
                    encodedBytes2 = new Span<byte>(new byte[expectedBytesWritten - bytesWritten1]);
                    Assert.True(utf8.TryEncode(inputUtf8.Slice(charactersConsumed1), encodedBytes2, out int charactersConsumed2, out bytesWritten2));
                    Assert.Equal(inputUtf8.Length, charactersConsumed1 + charactersConsumed2);
                    break;

                case TextEncoderTestHelper.SupportedEncoding.FromUtf16:
                    temp = testEncoderUnicode.GetBytes(inputString);
                    expectedBytes = Text.Encoding.Convert(testEncoderUnicode, testEncoder, temp);
                    expectedBytesWritten = expectedBytes.Length;
                    encodedBytes1 = new Span<byte>(new byte[expectedBytesWritten / 2]);
                    ReadOnlySpan<char> inputUtf16 = temp.AsSpan().NonPortableCast<byte, char>();
                    Assert.False(utf8.TryEncode(inputUtf16, encodedBytes1, out charactersConsumed1, out bytesWritten1));
                    encodedBytes2 = new Span<byte>(new byte[expectedBytesWritten - bytesWritten1]);
                    Assert.True(utf8.TryEncode(inputUtf16.Slice(charactersConsumed1), encodedBytes2, out charactersConsumed2, out bytesWritten2));
                    Assert.Equal(inputUtf16.Length, charactersConsumed1 + charactersConsumed2);
                    break;

                case TextEncoderTestHelper.SupportedEncoding.FromString:    // Open issue: https://github.com/dotnet/corefxlab/issues/1515
                    /*temp = testEncoderUnicode.GetBytes(inputString);
                    expectedBytes = Text.Encoding.Convert(testEncoderUnicode, testEncoder, temp);
                    expectedBytesWritten = expectedBytes.Length;
                    encodedBytes1 = new Span<byte>(new byte[expectedBytesWritten / 2]);
                    string inputStr = inputString;
                    Assert.False(utf8.TryEncode(inputStr, encodedBytes1, out bytesWritten1));
                    encodedBytes2 = new Span<byte>(new byte[expectedBytesWritten - bytesWritten1]);
                    Assert.True(utf8.TryEncode(inputStr.Substring(charactersConsumed1), encodedBytes2, out bytesWritten2));*/
                    return;

                case TextEncoderTestHelper.SupportedEncoding.FromUtf32:
                default:
                    temp = testEncoderUtf32.GetBytes(inputString);
                    expectedBytes = Text.Encoding.Convert(testEncoderUtf32, testEncoder, temp);
                    expectedBytesWritten = expectedBytes.Length;
                    encodedBytes1 = new Span<byte>(new byte[expectedBytesWritten / 2]);
                    ReadOnlySpan<uint> input = temp.AsSpan().NonPortableCast<byte, uint>();
                    Assert.False(utf8.TryEncode(input, encodedBytes1, out charactersConsumed1, out bytesWritten1));
                    encodedBytes2 = new Span<byte>(new byte[expectedBytesWritten - bytesWritten1]);
                    Assert.True(utf8.TryEncode(input.Slice(charactersConsumed1), encodedBytes2, out charactersConsumed2, out bytesWritten2));
                    Assert.Equal(input.Length, charactersConsumed1 + charactersConsumed2);
                    break;
            }

            Assert.Equal(expectedBytesWritten, bytesWritten1 + bytesWritten2);

            var encodedBytes = new Span<byte>(new byte[expectedBytesWritten]);
            encodedBytes1.CopyTo(encodedBytes);
            encodedBytes2.CopyTo(encodedBytes.Slice(encodedBytes1.Length));

            Assert.True(expectedBytes.AsSpan().SequenceEqual(encodedBytes));
        }

        [Theory]
        [MemberData(nameof(SupportedEncodingTestData))]
        public void OutputBufferLargerThanInputBuffer(TextEncoderTestHelper.SupportedEncoding from)
        {
            string inputString1 = TextEncoderTestHelper.GenerateValidString(TextEncoderConstants.DataLength, 0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint);
            string inputString2 = TextEncoderTestHelper.GenerateValidString(TextEncoderConstants.DataLength, 0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint);

            byte[] expectedBytes1;
            byte[] expectedBytes2;
            Span<byte> encodedBytes;

            int expectedBytesWritten;

            int bytesWritten;

            switch (from)
            {
                case TextEncoderTestHelper.SupportedEncoding.FromUtf8:
                    byte[] temp1 = testEncoder.GetBytes(inputString1);
                    byte[] temp2 = testEncoder.GetBytes(inputString2);
                    expectedBytes1 = Text.Encoding.Convert(testEncoder, testEncoder, temp1);
                    expectedBytes2 = Text.Encoding.Convert(testEncoder, testEncoder, temp2);
                    expectedBytesWritten = expectedBytes1.Length + expectedBytes2.Length;
                    encodedBytes = new Span<byte>(new byte[expectedBytesWritten]);
                    ReadOnlySpan<byte> firstUtf8 = temp1;
                    ReadOnlySpan<byte> secondUtf8 = temp2;

                    Assert.True(utf8.TryEncode(firstUtf8, encodedBytes, out int charactersConsumed, out bytesWritten));
                    Assert.Equal(firstUtf8.Length, charactersConsumed);
                    Assert.Equal(expectedBytes1.Length, bytesWritten);

                    Assert.True(utf8.TryEncode(secondUtf8, encodedBytes.Slice(bytesWritten), out charactersConsumed, out bytesWritten));
                    Assert.Equal(secondUtf8.Length, charactersConsumed);
                    Assert.Equal(expectedBytes2.Length, bytesWritten);
                    break;

                case TextEncoderTestHelper.SupportedEncoding.FromUtf16:
                    temp1 = testEncoderUnicode.GetBytes(inputString1);
                    temp2 = testEncoderUnicode.GetBytes(inputString2);
                    expectedBytes1 = Text.Encoding.Convert(testEncoderUnicode, testEncoder, temp1);
                    expectedBytes2 = Text.Encoding.Convert(testEncoderUnicode, testEncoder, temp2);
                    expectedBytesWritten = expectedBytes1.Length + expectedBytes2.Length;
                    encodedBytes = new Span<byte>(new byte[expectedBytesWritten]);
                    string firstInputStr = inputString1;
                    string secondInputStr = inputString2;

                    Assert.True(utf8.TryEncode(firstInputStr, encodedBytes, out bytesWritten));
                    Assert.Equal(expectedBytes1.Length, bytesWritten);

                    Assert.True(utf8.TryEncode(secondInputStr, encodedBytes.Slice(bytesWritten), out bytesWritten));
                    Assert.Equal(expectedBytes2.Length, bytesWritten);
                    break;

                case TextEncoderTestHelper.SupportedEncoding.FromString:
                    temp1 = testEncoderUnicode.GetBytes(inputString1);
                    temp2 = testEncoderUnicode.GetBytes(inputString2);
                    expectedBytes1 = Text.Encoding.Convert(testEncoderUnicode, testEncoder, temp1);
                    expectedBytes2 = Text.Encoding.Convert(testEncoderUnicode, testEncoder, temp2);
                    expectedBytesWritten = expectedBytes1.Length + expectedBytes2.Length;
                    encodedBytes = new Span<byte>(new byte[expectedBytesWritten]);
                    ReadOnlySpan<char> firstUtf16 = temp1.AsSpan().NonPortableCast<byte, char>();
                    ReadOnlySpan<char> secondUtf16 = temp2.AsSpan().NonPortableCast<byte, char>();

                    Assert.True(utf8.TryEncode(firstUtf16, encodedBytes, out charactersConsumed, out bytesWritten));
                    Assert.Equal(firstUtf16.Length, charactersConsumed);
                    Assert.Equal(expectedBytes1.Length, bytesWritten);

                    Assert.True(utf8.TryEncode(secondUtf16, encodedBytes.Slice(bytesWritten), out charactersConsumed, out bytesWritten));
                    Assert.Equal(secondUtf16.Length, charactersConsumed);
                    Assert.Equal(expectedBytes2.Length, bytesWritten);
                    break;

                case TextEncoderTestHelper.SupportedEncoding.FromUtf32:
                default:
                    temp1 = testEncoderUtf32.GetBytes(inputString1);
                    temp2 = testEncoderUtf32.GetBytes(inputString2);
                    expectedBytes1 = Text.Encoding.Convert(testEncoderUtf32, testEncoder, temp1);
                    expectedBytes2 = Text.Encoding.Convert(testEncoderUtf32, testEncoder, temp2);
                    expectedBytesWritten = expectedBytes1.Length + expectedBytes2.Length;
                    encodedBytes = new Span<byte>(new byte[expectedBytesWritten]);
                    ReadOnlySpan<uint> firstInput = temp1.AsSpan().NonPortableCast<byte, uint>();
                    ReadOnlySpan<uint> secondInput = temp2.AsSpan().NonPortableCast<byte, uint>();

                    Assert.True(utf8.TryEncode(firstInput, encodedBytes, out charactersConsumed, out bytesWritten));
                    Assert.Equal(firstInput.Length, charactersConsumed);
                    Assert.Equal(expectedBytes1.Length, bytesWritten);

                    Assert.True(utf8.TryEncode(secondInput, encodedBytes.Slice(bytesWritten), out charactersConsumed, out bytesWritten));
                    Assert.Equal(secondInput.Length, charactersConsumed);
                    Assert.Equal(expectedBytes2.Length, bytesWritten);
                    break;
            }

            var expectedBytes = new byte[expectedBytesWritten];
            Array.Copy(expectedBytes1, expectedBytes, expectedBytes1.Length);
            Array.Copy(expectedBytes2, 0, expectedBytes, expectedBytes1.Length, expectedBytes2.Length);

            Assert.True(expectedBytes.AsSpan().SequenceEqual(encodedBytes));
        }

        [Theory]
        //[MemberData(nameof(SupportedEncodingTestData))]
        //[InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf8)] // Open issue: https://github.com/dotnet/corefxlab/issues/1514
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf16)]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromString)]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf32)] // Open issue: https://github.com/dotnet/corefxlab/issues/1513
        public void InputBufferContainsOnlyInvalidData(TextEncoderTestHelper.SupportedEncoding from)
        {
            string inputString = TextEncoderTestHelper.GenerateOnlyInvalidString(TextEncoderConstants.DataLength);
            byte[] inputUtf8Bytes = TextEncoderTestHelper.GenerateOnlyInvalidUtf8Bytes(TextEncoderConstants.DataLength);

            int bytesWritten;

            switch (from)
            {
                case TextEncoderTestHelper.SupportedEncoding.FromUtf8:
                    byte[] expectedBytes = Text.Encoding.Convert(testEncoder, testEncoder, inputUtf8Bytes);
                    Span<byte> encodedBytes = new Span<byte>(new byte[expectedBytes.Length]);
                    ReadOnlySpan<byte> inputUtf8 = inputUtf8Bytes;
                    Assert.False(utf8.TryEncode(inputUtf8, encodedBytes, out int charactersConsumed, out bytesWritten));
                    Assert.Equal(0, charactersConsumed);
                    break;

                case TextEncoderTestHelper.SupportedEncoding.FromUtf16:
                    byte[] temp = testEncoderUnicode.GetBytes(inputString);
                    expectedBytes = Text.Encoding.Convert(testEncoderUnicode, testEncoder, temp);
                    encodedBytes = new Span<byte>(new byte[expectedBytes.Length]);
                    ReadOnlySpan<char> inputUtf16 = inputString.AsSpan();
                    Assert.False(utf8.TryEncode(inputUtf16, encodedBytes, out charactersConsumed, out bytesWritten));
                    Assert.Equal(0, charactersConsumed);
                    break;

                case TextEncoderTestHelper.SupportedEncoding.FromString:
                    temp = testEncoderUnicode.GetBytes(inputString);
                    expectedBytes = Text.Encoding.Convert(testEncoderUnicode, testEncoder, temp);
                    encodedBytes = new Span<byte>(new byte[expectedBytes.Length]);
                    string inputStr = inputString;
                    Assert.False(utf8.TryEncode(inputStr, encodedBytes, out bytesWritten));
                    break;

                case TextEncoderTestHelper.SupportedEncoding.FromUtf32: // Invalid if codePoint > 0x10FFFF
                default:
                    expectedBytes = Text.Encoding.Convert(testEncoderUtf32, testEncoder, inputUtf8Bytes);
                    encodedBytes = new Span<byte>(new byte[expectedBytes.Length]);
                    ReadOnlySpan<uint> input = inputUtf8Bytes.AsSpan().NonPortableCast<byte, uint>();
                    Assert.False(utf8.TryEncode(input, encodedBytes, out charactersConsumed, out bytesWritten));
                    Assert.Equal(0, charactersConsumed);
                    break;
            }

            Assert.Equal(0, bytesWritten);
        }

        [Theory]
        //[MemberData(nameof(SupportedEncodingTestData))]
        //[InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf8)] // Open issue: https://github.com/dotnet/corefxlab/issues/1514
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf16)]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromString)]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf32)] // Open issue: https://github.com/dotnet/corefxlab/issues/1513
        public void InputBufferContainsSomeInvalidData(TextEncoderTestHelper.SupportedEncoding from)
        {
            string inputString = TextEncoderTestHelper.GenerateStringWithInvalidChars(TextEncoderConstants.DataLength);
            byte[] inputUtf8Bytes = TextEncoderTestHelper.GenerateUtf8BytesWithInvalidBytes(TextEncoderConstants.DataLength);

            byte[] expectedBytes;
            Span<byte> encodedBytes;

            int expectedBytesWritten;
            int bytesWritten;

            switch (from)
            {
                case TextEncoderTestHelper.SupportedEncoding.FromUtf8:
                    expectedBytes = Text.Encoding.Convert(testEncoder, testEncoder, inputUtf8Bytes);
                    ReadOnlySpan<byte> inputUtf8 = inputUtf8Bytes;
                    expectedBytesWritten = TextEncoderTestHelper.GetByteCount(inputUtf8);
                    encodedBytes = new Span<byte>(new byte[expectedBytesWritten]);
                    Assert.False(utf8.TryEncode(inputUtf8, encodedBytes, out int charactersConsumed, out bytesWritten));
                    Assert.True(charactersConsumed < inputString.Length);
                    break;

                case TextEncoderTestHelper.SupportedEncoding.FromUtf16:
                    byte[] temp = testEncoderUnicode.GetBytes(inputString);
                    expectedBytes = Text.Encoding.Convert(testEncoderUnicode, testEncoder, temp);
                    ReadOnlySpan<char> inputUtf16 = inputString.AsSpan();
                    expectedBytesWritten = TextEncoderTestHelper.GetByteCount(inputUtf16);
                    encodedBytes = new Span<byte>(new byte[expectedBytesWritten]);
                    Assert.False(utf8.TryEncode(inputUtf16, encodedBytes, out charactersConsumed, out bytesWritten));
                    Assert.True(charactersConsumed < inputString.Length);
                    break;

                case TextEncoderTestHelper.SupportedEncoding.FromString:
                    temp = testEncoderUnicode.GetBytes(inputString);
                    expectedBytes = Text.Encoding.Convert(testEncoderUnicode, testEncoder, temp);
                    string inputStr = inputString;
                    expectedBytesWritten = TextEncoderTestHelper.GetByteCount(inputStr);
                    encodedBytes = new Span<byte>(new byte[expectedBytesWritten]);
                    Assert.False(utf8.TryEncode(inputStr, encodedBytes, out bytesWritten));
                    break;

                case TextEncoderTestHelper.SupportedEncoding.FromUtf32:  // Invalid if codePoint > 0x10FFFF
                default:
                    expectedBytes = Text.Encoding.Convert(testEncoderUtf32, testEncoder, inputUtf8Bytes);
                    ReadOnlySpan<uint> input = inputUtf8Bytes.AsSpan().NonPortableCast<byte, uint>();
                    expectedBytesWritten = TextEncoderTestHelper.GetByteCount(input);
                    encodedBytes = new Span<byte>(new byte[expectedBytesWritten]);
                    Assert.False(utf8.TryEncode(input, encodedBytes, out charactersConsumed, out bytesWritten));
                    Assert.Equal(0, charactersConsumed);
                    break;
            }

            Assert.Equal(expectedBytesWritten, bytesWritten);
            Assert.True(expectedBytes.AsSpan().Slice(0, expectedBytesWritten).SequenceEqual(encodedBytes));
        }

        [Theory]
        //[InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf8)] // Open issue: https://github.com/dotnet/corefxlab/issues/1514
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf16)]
        [InlineData(TextEncoderTestHelper.SupportedEncoding.FromString)]
        //[InlineData(TextEncoderTestHelper.SupportedEncoding.FromUtf32)] // Open issue: https://github.com/dotnet/corefxlab/issues/1513
        public void InputBufferEndsTooEarlyAndRestart(TextEncoderTestHelper.SupportedEncoding from)
        {
            string inputString1 = TextEncoderTestHelper.GenerateValidStringEndsWithHighStartsWithLow(TextEncoderConstants.DataLength, false);
            string inputString2 = inputString1 + TextEncoderTestHelper.GenerateValidStringEndsWithHighStartsWithLow(TextEncoderConstants.DataLength, true);

            byte[] inputUtf8Bytes1 = TextEncoderTestHelper.GenerateValidUtf8BytesEndsWithHighStartsWithLow(TextEncoderConstants.DataLength, false);
            byte[] tempForUtf8Bytes2 = TextEncoderTestHelper.GenerateValidUtf8BytesEndsWithHighStartsWithLow(TextEncoderConstants.DataLength, true);
            byte[] inputUtf8Bytes2 = new byte[inputUtf8Bytes1.Length + tempForUtf8Bytes2.Length];
            Array.Copy(inputUtf8Bytes1, inputUtf8Bytes2, inputUtf8Bytes1.Length);
            Array.Copy(tempForUtf8Bytes2, 0, inputUtf8Bytes2, inputUtf8Bytes1.Length, tempForUtf8Bytes2.Length);

            uint[] inputUtf32Bytes1 = TextEncoderTestHelper.GenerateValidUtf32EndsWithHighStartsWithLow(TextEncoderConstants.DataLength, false);
            uint[] tempForUtf32Bytes2 = TextEncoderTestHelper.GenerateValidUtf32EndsWithHighStartsWithLow(TextEncoderConstants.DataLength, true);
            uint[] inputUtf32Bytes2 = new uint[inputUtf32Bytes1.Length + tempForUtf32Bytes2.Length];
            Array.Copy(inputUtf32Bytes1, inputUtf32Bytes2, inputUtf32Bytes1.Length);
            Array.Copy(tempForUtf32Bytes2, 0, inputUtf32Bytes2, inputUtf32Bytes1.Length, tempForUtf32Bytes2.Length);

            byte[] uint32Bytes1 = TextEncoderTestHelper.GenerateValidBytesUtf32EndsWithHighStartsWithLow(TextEncoderConstants.DataLength, false);
            byte[] tempUint32Bytes = TextEncoderTestHelper.GenerateValidBytesUtf32EndsWithHighStartsWithLow(TextEncoderConstants.DataLength, true);
            byte[] uint32Bytes2 = new byte[uint32Bytes1.Length + tempUint32Bytes.Length];
            Array.Copy(uint32Bytes1, uint32Bytes2, uint32Bytes1.Length);
            Array.Copy(tempUint32Bytes, 0, uint32Bytes2, uint32Bytes1.Length, tempUint32Bytes.Length);

            byte[] expectedBytes;
            Span<byte> encodedBytes;

            int charactersConsumed1;
            int charactersConsumed2;

            int bytesWritten1;
            int bytesWritten2;

            switch (from)
            {
                case TextEncoderTestHelper.SupportedEncoding.FromUtf8:
                    expectedBytes = Text.Encoding.Convert(testEncoder, testEncoder, inputUtf8Bytes2);
                    ReadOnlySpan<byte> firstUtf8 = inputUtf8Bytes1;
                    ReadOnlySpan<byte> secondUtf8 = inputUtf8Bytes2;
                    encodedBytes = new Span<byte>(new byte[expectedBytes.Length]);
                    Assert.False(utf8.TryEncode(firstUtf8, encodedBytes, out charactersConsumed1, out bytesWritten1));
                    Assert.True(utf8.TryEncode(secondUtf8.Slice(charactersConsumed1), encodedBytes.Slice(bytesWritten1), out charactersConsumed2, out bytesWritten2));
                    break;

                case TextEncoderTestHelper.SupportedEncoding.FromUtf16:
                    byte[] temp = testEncoderUnicode.GetBytes(inputString2);
                    expectedBytes = Text.Encoding.Convert(testEncoderUnicode, testEncoder, temp);
                    ReadOnlySpan<char> firstUtf16 = inputString1.AsSpan();
                    ReadOnlySpan<char> secondUtf16 = inputString2.AsSpan();
                    encodedBytes = new Span<byte>(new byte[expectedBytes.Length]);
                    Assert.False(utf8.TryEncode(firstUtf16, encodedBytes, out charactersConsumed1, out bytesWritten1));
                    Assert.True(utf8.TryEncode(secondUtf16.Slice(charactersConsumed1), encodedBytes.Slice(bytesWritten1), out charactersConsumed2, out bytesWritten2));
                    break;

                case TextEncoderTestHelper.SupportedEncoding.FromString:     // Open issue: https://github.com/dotnet/corefxlab/issues/1515
                    /*temp = testEncoderUnicode.GetBytes(inputString2);
                    expectedBytes = Text.Encoding.Convert(testEncoderUnicode, testEncoder, temp);
                    string firstInputStr = inputString1;
                    string secondInputStr = inputString2;
                    encodedBytes = new Span<byte>(new byte[expectedBytes.Length]);
                    Assert.False(utf8.TryEncode(firstInputStr, encodedBytes, out bytesWritten1));
                    Assert.True(utf8.TryEncode(secondInputStr.Substring(charactersConsumed1), encodedBytes.Slice(bytesWritten1), out bytesWritten1));*/
                    return;

                case TextEncoderTestHelper.SupportedEncoding.FromUtf32:
                default:
                    expectedBytes = Text.Encoding.Convert(testEncoderUtf32, testEncoder, uint32Bytes2);
                    ReadOnlySpan<uint> firstInput = inputUtf32Bytes1.AsSpan();
                    ReadOnlySpan<uint> secondInput = inputUtf32Bytes2.AsSpan();
                    encodedBytes = new Span<byte>(new byte[TextEncoderTestHelper.GetByteCount(inputUtf32Bytes2)]);
                    Assert.False(utf8.TryEncode(firstInput, encodedBytes, out charactersConsumed1, out bytesWritten1));
                    Assert.True(utf8.TryEncode(secondInput.Slice(charactersConsumed1), encodedBytes.Slice(bytesWritten1), out charactersConsumed2, out bytesWritten2));
                    break;
            }

            Assert.Equal(TextEncoderConstants.DataLength * 2, charactersConsumed1 + charactersConsumed2);
            Assert.Equal(expectedBytes.Length, bytesWritten1 + bytesWritten2);
            Assert.True(expectedBytes.AsSpan().SequenceEqual(encodedBytes));
        }

        [Theory]
        [MemberData(nameof(SupportedEncodingTestData))]
        public void InputBufferContainsOnlyASCII(TextEncoderTestHelper.SupportedEncoding from)
        {
            Assert.True(TextEncoderTestHelper.Validate(from, utf8, testEncoder, TextEncoderTestHelper.CodePointSubset.ASCII));  // 1 byte
        }

        [Theory]
        [MemberData(nameof(SupportedEncodingTestData))]
        public void InputBufferContainsNonASCII(TextEncoderTestHelper.SupportedEncoding from)
        {
            Assert.True(TextEncoderTestHelper.Validate(from, utf8, testEncoder, TextEncoderTestHelper.CodePointSubset.TwoBytes));  // 2 bytes
            Assert.True(TextEncoderTestHelper.Validate(from, utf8, testEncoder, TextEncoderTestHelper.CodePointSubset.ThreeBytes));  // 3 bytes
            Assert.True(TextEncoderTestHelper.Validate(from, utf8, testEncoder, TextEncoderTestHelper.CodePointSubset.Surrogates));  // 4 bytes (high and low surrogates)
            Assert.True(TextEncoderTestHelper.Validate(from, utf8, testEncoder, TextEncoderTestHelper.CodePointSubset.Mixed));  // mixed
        }

        [Theory]
        [MemberData(nameof(SupportedEncodingTestData))]
        public void InputBufferContainsAllCodePoints(TextEncoderTestHelper.SupportedEncoding from)
        {
            string inputString = TextEncoderTestHelper.GenerateAllCharacters();

            byte[] expectedBytes;
            Span<byte> encodedBytes;
            
            int bytesWritten;

            switch (from)
            {
                case TextEncoderTestHelper.SupportedEncoding.FromUtf8:
                    byte[] temp = testEncoder.GetBytes(inputString);
                    expectedBytes = Text.Encoding.Convert(testEncoder, testEncoder, temp);
                    encodedBytes = new Span<byte>(new byte[expectedBytes.Length]);
                    ReadOnlySpan<byte> inputUtf8 = temp;
                    Assert.True(utf8.TryEncode(inputUtf8, encodedBytes, out int charactersConsumed, out bytesWritten));
                    Assert.Equal(inputUtf8.Length, charactersConsumed);
                    break;

                case TextEncoderTestHelper.SupportedEncoding.FromUtf16:
                    temp = testEncoderUnicode.GetBytes(inputString);
                    expectedBytes = Text.Encoding.Convert(testEncoderUnicode, testEncoder, temp);
                    encodedBytes = new Span<byte>(new byte[expectedBytes.Length]);
                    ReadOnlySpan<char> inputUtf16 = temp.AsSpan().NonPortableCast<byte, char>();
                    Assert.True(utf8.TryEncode(inputUtf16, encodedBytes, out charactersConsumed, out bytesWritten));
                    Assert.Equal(inputUtf16.Length, charactersConsumed);
                    break;

                case TextEncoderTestHelper.SupportedEncoding.FromString:
                    temp = testEncoderUnicode.GetBytes(inputString);
                    expectedBytes = Text.Encoding.Convert(testEncoderUnicode, testEncoder, temp);
                    encodedBytes = new Span<byte>(new byte[expectedBytes.Length]);
                    string inputStr = inputString;
                    Assert.True(utf8.TryEncode(inputStr, encodedBytes, out bytesWritten));
                    break;

                case TextEncoderTestHelper.SupportedEncoding.FromUtf32:
                default:
                    temp = testEncoderUtf32.GetBytes(inputString);
                    expectedBytes = Text.Encoding.Convert(testEncoderUtf32, testEncoder, temp);
                    encodedBytes = new Span<byte>(new byte[expectedBytes.Length]);
                    ReadOnlySpan<uint> input = temp.AsSpan().NonPortableCast<byte, uint>();
                    Assert.True(utf8.TryEncode(input, encodedBytes, out charactersConsumed, out bytesWritten));
                    Assert.Equal(input.Length, charactersConsumed);
                    break;
            }

            Assert.Equal(expectedBytes.Length, bytesWritten);
            Assert.True(expectedBytes.AsSpan().SequenceEqual(encodedBytes));
        }
    }
}
