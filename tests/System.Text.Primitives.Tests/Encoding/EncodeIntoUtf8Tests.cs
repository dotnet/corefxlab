// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using Xunit;

namespace System.Text.Primitives.Tests.Encoding
{
    public class EncodeIntoUtf8Tests
    {
        const int BufferSizeRange = 30;

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

        [Fact]
        public void InputEmptyFromUtf16()
        {
            // Destination has zero storage
            Assert.Equal(TransformationStatus.Done, Encoders.Utf8.ConvertFromUtf16(ReadOnlySpan<byte>.Empty, Span<byte>.Empty, out int consumed, out int written));
            Assert.Equal(0, consumed);
            Assert.Equal(0, written);

            // Destination has non-zero storage
            Assert.Equal(TransformationStatus.Done, Encoders.Utf8.ConvertFromUtf16(ReadOnlySpan<byte>.Empty, new byte[1], out consumed, out written));
            Assert.Equal(0, consumed);
            Assert.Equal(0, written);
        }

        [Fact]
        public void InputEmptyFromUtf32()
        {
            // Destination has zero storage
            Assert.Equal(TransformationStatus.Done, Encoders.Utf8.ConvertFromUtf32(ReadOnlySpan<byte>.Empty, Span<byte>.Empty, out int consumed, out int written));
            Assert.Equal(0, consumed);
            Assert.Equal(0, written);

            // Destination has non-zero storage
            Assert.Equal(TransformationStatus.Done, Encoders.Utf8.ConvertFromUtf32(ReadOnlySpan<byte>.Empty, new byte[1], out consumed, out written));
            Assert.Equal(0, consumed);
            Assert.Equal(0, written);
        }

        [Fact]
        public void OutputEmptyFromUtf16()
        {
            string inputString = TextEncoderTestHelper.GenerateValidString(TextEncoderConstants.DataLength, 0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint);
            ReadOnlySpan<byte> input = Text.Encoding.Unicode.GetBytes(inputString);

            Assert.Equal(TransformationStatus.DestinationTooSmall, Encoders.Utf8.ConvertFromUtf16(input, Span<byte>.Empty, out int consumed, out int written));
            Assert.Equal(0, consumed);
            Assert.Equal(0, written);
        }

        [Fact]
        public void OutputEmptyFromUtf32()
        {
            string inputString = TextEncoderTestHelper.GenerateValidString(TextEncoderConstants.DataLength, 0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint);
            ReadOnlySpan<byte> input = Text.Encoding.UTF32.GetBytes(inputString);

            Assert.Equal(TransformationStatus.DestinationTooSmall, Encoders.Utf8.ConvertFromUtf32(input, Span<byte>.Empty, out int consumed, out int written));
            Assert.Equal(0, consumed);
            Assert.Equal(0, written);
        }

        [Fact]
        public void InputOutputSizeCombosFromUtf16()
        {

            for (int i = 0; i < BufferSizeRange; i++)
            {
                string inputString = TextEncoderTestHelper.GenerateValidString(i, 0, TextEncoderConstants.Utf8TwoBytesLastCodePoint);
                ReadOnlySpan<byte> input = Text.Encoding.Unicode.GetBytes(inputString);
                Span<byte> expected = Text.Encoding.UTF8.GetBytes(inputString);

                for (int j = 0; j < BufferSizeRange * 4; j++)
                {
                    Span<byte> output = new byte[j];
                    var status = Encoders.Utf8.ConvertFromUtf16(input, output, out int consumed, out int written);
                    if (status == TransformationStatus.DestinationTooSmall)
                    {
                        Assert.True(consumed < input.Length, "consumed is too large");
                        Assert.True(written < expected.Length, "written is too large");
                    }
                    else
                    {
                        Assert.Equal(TransformationStatus.Done, status);
                        Assert.Equal(input.Length, consumed);
                        Assert.Equal(expected.Length, written);
                    }

                    Assert.True(expected.Slice(0, written).SequenceEqual(output.Slice(0, written)), "Incorrect byte sequence");
                }
            }
        }

        [Fact]
        public void InputOutputSizeCombosFromUtf32()
        {

            for (int i = 0; i < BufferSizeRange; i++)
            {
                string inputString = TextEncoderTestHelper.GenerateValidString(i, 0, TextEncoderConstants.Utf8TwoBytesLastCodePoint);
                ReadOnlySpan<byte> input = Text.Encoding.UTF32.GetBytes(inputString);
                Span<byte> expected = Text.Encoding.UTF8.GetBytes(inputString);

                for (int j = 0; j < BufferSizeRange * 4; j++)
                {
                    Span<byte> output = new byte[j];
                    var status = Encoders.Utf8.ConvertFromUtf32(input, output, out int consumed, out int written);
                    if (status == TransformationStatus.DestinationTooSmall)
                    {
                        Assert.True(consumed < input.Length, "consumed is too large");
                        Assert.True(written < expected.Length, "written is too large");
                    }
                    else
                    {
                        Assert.Equal(TransformationStatus.Done, status);
                        Assert.Equal(input.Length, consumed);
                        Assert.Equal(expected.Length, written);
                    }

                    Assert.True(expected.Slice(0, written).SequenceEqual(output.Slice(0, written)), "Incorrect byte sequence");
                }
            }
        }

        [Fact]
        public void InputLargerThanOutputFromUtf16()
        {
            string inputString = TextEncoderTestHelper.GenerateValidString(TextEncoderConstants.DataLength, 0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint);
            ReadOnlySpan<byte> input = Text.Encoding.Unicode.GetBytes(inputString);
            Span<byte> expected = Text.Encoding.UTF8.GetBytes(inputString);

            Span<byte> output = new byte[expected.Length / 2];
            Assert.Equal(TransformationStatus.DestinationTooSmall, Encoders.Utf8.ConvertFromUtf16(input, output, out int consumed, out int written));
            Assert.True(consumed < input.Length, "Unexpectedly consumed entire input");
            Assert.True(written < expected.Length, "Unexpectedly wrote entire output");
            Assert.True(expected.Slice(0, written).SequenceEqual(output.Slice(0, written)), "Incorrect byte sequence");

            input = input.Slice(consumed);
            expected = expected.Slice(written);
            output = new byte[expected.Length];
            Assert.Equal(TransformationStatus.Done, Encoders.Utf8.ConvertFromUtf16(input, output, out consumed, out written));
            Assert.Equal(input.Length, consumed);
            Assert.Equal(expected.Length, written);
            Assert.True(expected.SequenceEqual(output), "Incorrect byte sequence");
        }

        [Fact]
        public void InputLargerThanOutputFromUtf32()
        {
            string inputString = TextEncoderTestHelper.GenerateValidString(TextEncoderConstants.DataLength, 0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint);
            ReadOnlySpan<byte> input = Text.Encoding.UTF32.GetBytes(inputString);
            Span<byte> expected = Text.Encoding.UTF8.GetBytes(inputString);

            Span<byte> output = new byte[expected.Length / 2];
            Assert.Equal(TransformationStatus.DestinationTooSmall, Encoders.Utf8.ConvertFromUtf32(input, output, out int consumed, out int written));
            Assert.True(consumed < input.Length, "Unexpectedly consumed entire input");
            Assert.True(written < expected.Length, "Unexpectedly wrote entire output");
            Assert.True(expected.Slice(0, written).SequenceEqual(output.Slice(0, written)), "Incorrect byte sequence");

            input = input.Slice(consumed);
            expected = expected.Slice(written);
            output = new byte[expected.Length];
            Assert.Equal(TransformationStatus.Done, Encoders.Utf8.ConvertFromUtf32(input, output, out consumed, out written));
            Assert.Equal(input.Length, consumed);
            Assert.Equal(expected.Length, written);
            Assert.True(expected.SequenceEqual(output), "Incorrect byte sequence");
        }

        [Fact]
        public void OutputLargerThatInputFromUtf16()
        {
            string inputString1 = TextEncoderTestHelper.GenerateValidString(TextEncoderConstants.DataLength, 0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint);
            string inputString2 = TextEncoderTestHelper.GenerateValidString(TextEncoderConstants.DataLength, 0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint);
            ReadOnlySpan<byte> input1 = Text.Encoding.Unicode.GetBytes(inputString1);
            ReadOnlySpan<byte> input2 = Text.Encoding.Unicode.GetBytes(inputString1);
            Span<byte> expected1 = Text.Encoding.UTF8.GetBytes(inputString1);
            Span<byte> expected2 = Text.Encoding.UTF8.GetBytes(inputString2);

            Span<byte> output = new byte[expected1.Length + expected2.Length];
            Assert.Equal(TransformationStatus.Done, Encoders.Utf8.ConvertFromUtf16(input1, output, out int consumed1, out int written1));
            Assert.Equal(input1.Length, consumed1);
            Assert.Equal(expected1.Length, written1);

            Assert.Equal(TransformationStatus.Done, Encoders.Utf8.ConvertFromUtf16(input2, output.Slice(written1), out int consumed2, out int written2));
            Assert.Equal(input2.Length, consumed2);
            Assert.Equal(expected2.Length, written2);

            Assert.True(expected1.SequenceEqual(output.Slice(0, written1)));
            Assert.True(expected2.SequenceEqual(output.Slice(written1)));
        }

        [Fact]
        public void OutputLargerThatInputFromUtf32()
        {
            string inputString1 = TextEncoderTestHelper.GenerateValidString(TextEncoderConstants.DataLength, 0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint);
            string inputString2 = TextEncoderTestHelper.GenerateValidString(TextEncoderConstants.DataLength, 0, TextEncoderConstants.Utf8ThreeBytesLastCodePoint);
            ReadOnlySpan<byte> input1 = Text.Encoding.UTF32.GetBytes(inputString1);
            ReadOnlySpan<byte> input2 = Text.Encoding.UTF32.GetBytes(inputString1);
            Span<byte> expected1 = Text.Encoding.UTF8.GetBytes(inputString1);
            Span<byte> expected2 = Text.Encoding.UTF8.GetBytes(inputString2);

            Span<byte> output = new byte[expected1.Length + expected2.Length];
            Assert.Equal(TransformationStatus.Done, Encoders.Utf8.ConvertFromUtf32(input1, output, out int consumed1, out int written1));
            Assert.Equal(input1.Length, consumed1);
            Assert.Equal(expected1.Length, written1);

            Assert.Equal(TransformationStatus.Done, Encoders.Utf8.ConvertFromUtf32(input2, output.Slice(written1), out int consumed2, out int written2));
            Assert.Equal(input2.Length, consumed2);
            Assert.Equal(expected2.Length, written2);

            Assert.True(expected1.SequenceEqual(output.Slice(0, written1)));
            Assert.True(expected2.SequenceEqual(output.Slice(written1)));
        }

        [Fact]
        public void InputContainsOnlyInvalidDataFromUtf16()
        {
            string inputStringLow = TextEncoderTestHelper.GenerateOnlyInvalidString(TextEncoderConstants.DataLength);
            string inputStringHigh = TextEncoderTestHelper.GenerateOnlyInvalidString(TextEncoderConstants.DataLength, true);
            Span<byte> output = new byte[16];

            ReadOnlySpan<byte> input = inputStringLow.AsSpan().AsBytes();
            Assert.Equal(TransformationStatus.InvalidData, Encoders.Utf8.ConvertFromUtf16(input, output, out int consumed, out int written));
            Assert.Equal(0, consumed);
            Assert.Equal(0, written);

            input = inputStringHigh.AsSpan().AsBytes();
            Assert.Equal(TransformationStatus.InvalidData, Encoders.Utf8.ConvertFromUtf16(input, output, out consumed, out written));
            Assert.Equal(0, consumed);
            Assert.Equal(0, written);
        }

        [Fact]
        public void InputContainsOnlyInvalidDataFromUtf32()
        {
            uint[] codepoints = TextEncoderTestHelper.GenerateOnlyInvalidUtf32CodePoints(TextEncoderConstants.DataLength / sizeof(uint));
            ReadOnlySpan<byte> input = codepoints.AsSpan().AsBytes();
            Span<byte> output = new byte[16];

            Assert.Equal(TransformationStatus.InvalidData, Encoders.Utf8.ConvertFromUtf32(input, output, out int consumed, out int written));
            Assert.Equal(0, consumed);
            Assert.Equal(0, written);
        }

        [Fact]
        public void InputContainsSomeInvalidDataFromUtf16()
        {
            string inputStringEndsWithLow = TextEncoderTestHelper.GenerateInvalidStringEndsWithLow(TextEncoderConstants.DataLength);
            byte[] inputBytes = Text.Encoding.Unicode.GetBytes(inputStringEndsWithLow);
            ReadOnlySpan<byte> input = inputStringEndsWithLow.AsSpan().AsBytes();
            ReadOnlySpan<byte> expected = Text.Encoding.Convert(Text.Encoding.Unicode, Text.Encoding.UTF8, inputBytes);
            int expectedWritten = TextEncoderTestHelper.GetUtf8ByteCount(inputStringEndsWithLow.AsSpan());
            Span<byte> output = new byte[expectedWritten + 10];
            Assert.Equal(TransformationStatus.InvalidData, Encoders.Utf8.ConvertFromUtf16(input, output, out int consumed, out int written));
            Assert.True(consumed < input.Length, "Consumed too many input characters");
            Assert.Equal(expectedWritten, written);
            Assert.True(expected.Slice(0, written).SequenceEqual(output.Slice(0, written)), "Invalid output sequence [ends with low]");

            string inputStringInvalid = TextEncoderTestHelper.GenerateStringWithInvalidChars(TextEncoderConstants.DataLength);
            inputBytes = Text.Encoding.Unicode.GetBytes(inputStringInvalid);
            input = inputStringInvalid.AsSpan().AsBytes();
            expected = Text.Encoding.Convert(Text.Encoding.Unicode, Text.Encoding.UTF8, inputBytes);
            expectedWritten = TextEncoderTestHelper.GetUtf8ByteCount(inputStringInvalid.AsSpan());
            output = new byte[expectedWritten + 10];
            Assert.Equal(TransformationStatus.InvalidData, Encoders.Utf8.ConvertFromUtf16(input, output, out consumed, out written));
            Assert.True(consumed < input.Length, "Consumed more input than expected");
            Assert.Equal(expectedWritten, written);
            Assert.True(expected.Slice(0, written).SequenceEqual(output.Slice(0, written)), "Invalid output sequence [invalid]");
        }

        [Fact]
        public void InputContainsSomeInvalidDataFromUtf32()
        {
            uint[] codepoints = TextEncoderTestHelper.GenerateOnlyInvalidUtf32CodePoints(TextEncoderConstants.DataLength / sizeof(uint));
            ReadOnlySpan<byte> input = codepoints.AsSpan().AsBytes();
            ReadOnlySpan<byte> expected = Text.Encoding.Convert(Text.Encoding.UTF32, Text.Encoding.UTF8, input.ToArray());
            int expectedWritten = TextEncoderTestHelper.GetUtf8ByteCount(codepoints);
            Span<byte> output = new byte[expectedWritten];

            Assert.Equal(TransformationStatus.InvalidData, Encoders.Utf8.ConvertFromUtf32(input, output, out int consumed, out int written));
            Assert.True(consumed < input.Length, "Consumed more input than expected");
            Assert.Equal(expectedWritten, written);
            Assert.True(expected.Slice(0, expectedWritten).SequenceEqual(output));
        }

        [Fact]
        public void InputEndsTooEarlyAndRestartsFromUtf16()
        {
            string inputString1 = TextEncoderTestHelper.GenerateValidStringEndsWithHighStartsWithLow(TextEncoderConstants.DataLength, false);
            string inputString2 = TextEncoderTestHelper.GenerateValidStringEndsWithHighStartsWithLow(TextEncoderConstants.DataLength, true);

            char[] inputCharsAll = new char[inputString1.Length + inputString2.Length];
            inputString1.CopyTo(0, inputCharsAll, 0, inputString1.Length);
            inputString2.CopyTo(0, inputCharsAll, inputString1.Length, inputString2.Length);

            ReadOnlySpan<byte> input1 = inputCharsAll.AsSpan().Slice(0, inputString1.Length).AsBytes();
            ReadOnlySpan<byte> input2 = inputCharsAll.AsSpan().Slice(inputString1.Length - 1).AsBytes();

            ReadOnlySpan<byte> expected = Text.Encoding.UTF8.GetBytes(inputString1 + inputString2);
            Span<byte> output = new byte[expected.Length];

            Assert.Equal(TransformationStatus.NeedMoreSourceData, Encoders.Utf8.ConvertFromUtf16(input1, output, out int consumed, out int written));
            Assert.Equal(input1.Length - 2, consumed);
            Assert.NotEqual(expected.Length, written);
            Assert.True(expected.Slice(0, written).SequenceEqual(output.Slice(0, written)), "Invalid output sequence [first half]");

            expected = expected.Slice(written);
            Assert.Equal(TransformationStatus.Done, Encoders.Utf8.ConvertFromUtf16(input2, output, out consumed, out written));
            Assert.Equal(input2.Length, consumed);
            Assert.Equal(expected.Length, written);
            Assert.True(expected.SequenceEqual(output.Slice(0, written)), "Invalid output sequence [second half]");
        }

        [Fact]
        public void InputEndsTooEarlyAndRestartsFromUtf32()
        {
            uint[] codepoints1 = TextEncoderTestHelper.GenerateValidUtf32CodePoints(TextEncoderConstants.DataLength);
            uint[] codepoints2 = TextEncoderTestHelper.GenerateValidUtf32CodePoints(TextEncoderConstants.DataLength);

            uint[] inputAll = new uint[codepoints1.Length + codepoints2.Length];
            Array.Copy(codepoints1, inputAll, codepoints1.Length);
            Array.Copy(codepoints2, 0, inputAll, codepoints1.Length, codepoints2.Length);

            ReadOnlySpan<byte> expected = Text.Encoding.Convert(Text.Encoding.UTF32, Text.Encoding.UTF8, inputAll.AsSpan().AsBytes().ToArray());
            Span<byte> output = new byte[expected.Length];

            ReadOnlySpan<byte> input = inputAll.AsSpan().Slice(0, codepoints1.Length).AsBytes();
            input = input.Slice(0, input.Length - 2); // Strip a couple bytes from last good code point
            Assert.Equal(TransformationStatus.NeedMoreSourceData, Encoders.Utf8.ConvertFromUtf32(input, output, out int consumed, out int written));
            Assert.True(input.Length > consumed, "Consumed too many bytes [first half]");
            Assert.NotEqual(expected.Length, written);
            Assert.True(expected.Slice(0, written).SequenceEqual(output.Slice(0, written)), "Invalid output sequence [first half]");

            input = inputAll.AsSpan().AsBytes().Slice(consumed);
            expected = expected.Slice(written);
            Assert.Equal(TransformationStatus.Done, Encoders.Utf8.ConvertFromUtf32(input, output, out consumed, out written));
            Assert.Equal(input.Length, consumed);
            Assert.Equal(expected.Length, written);
            Assert.True(expected.SequenceEqual(output.Slice(0, written)), "Invalid output sequence [second half]");
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

        [Fact]
        public void InputContainsAllCodePointsFromUtf16()
        {
            string inputString = TextEncoderTestHelper.GenerateAllCharacters();
            ReadOnlySpan<byte> input = Text.Encoding.Unicode.GetBytes(inputString);
            ReadOnlySpan<byte> expected = Text.Encoding.UTF8.GetBytes(inputString);
            Span<byte> output = new byte[expected.Length];

            Assert.Equal(TransformationStatus.Done, Encoders.Utf8.ConvertFromUtf16(input, output, out int consumed, out int written));
            Assert.Equal(input.Length, consumed);
            Assert.Equal(expected.Length, written);
            Assert.True(expected.SequenceEqual(output), "Invalid output sequence");
        }

        [Fact]
        public void InputContainsAllCodePointsFromUtf32()
        {
            string inputString = TextEncoderTestHelper.GenerateAllCharacters();
            ReadOnlySpan<byte> input = Text.Encoding.UTF32.GetBytes(inputString);
            ReadOnlySpan<byte> expected = Text.Encoding.UTF8.GetBytes(inputString);
            Span<byte> output = new byte[expected.Length];

            Assert.Equal(TransformationStatus.Done, Encoders.Utf8.ConvertFromUtf32(input, output, out int consumed, out int written));
            Assert.Equal(input.Length, consumed);
            Assert.Equal(expected.Length, written);
            Assert.True(expected.SequenceEqual(output), "Invalid output sequence");
        }
    }
}
