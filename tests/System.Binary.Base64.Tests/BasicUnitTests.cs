// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Xunit;
using System.Collections.Generic;
using System.Buffers;
using System.Buffers.Text;

namespace System.Binary.Base64.Tests
{
    public class Base64Tests
    {
        [Fact]
        public void BasicEncodingDecoding()
        {
            var bytes = new byte[byte.MaxValue + 1];
            for (int i = 0; i < byte.MaxValue + 1; i++)
            {
                bytes[i] = (byte)i;
            }

            for (int value = 0; value < 256; value++)
            {
                Span<byte> sourceBytes = bytes.AsSpan().Slice(0, value + 1);
                Span<byte> encodedBytes = new byte[Base64.GetMaxEncodedToUtf8Length(sourceBytes.Length)];
                Assert.Equal(OperationStatus.Done, Base64.EncodeToUtf8(sourceBytes, encodedBytes, out int consumed, out int encodedBytesCount));
                Assert.Equal(encodedBytes.Length, encodedBytesCount);

                string encodedText = Text.Encoding.ASCII.GetString(encodedBytes.ToArray());
                string expectedText = Convert.ToBase64String(bytes, 0, value + 1);
                Assert.Equal(expectedText, encodedText);

                if (encodedBytes.Length % 4 == 0)
                {
                    Span<byte> decodedBytes = new byte[Base64.GetMaxDecodedFromUtf8Length(encodedBytes.Length)];
                    Assert.Equal(OperationStatus.Done, Base64.DecodeFromUtf8(encodedBytes, decodedBytes, out consumed, out int decodedByteCount));
                    Assert.Equal(sourceBytes.Length, decodedByteCount);
                    Assert.True(sourceBytes.SequenceEqual(decodedBytes.Slice(0, decodedByteCount)));
                }
            }
        }

        [Fact]
        public void BasicEncoding()
        {
            var rnd = new Random(42);
            for (int i = 0; i < 10; i++)
            {
                int numBytes = rnd.Next(100, 1000 * 1000);
                Span<byte> source = new byte[numBytes];
                Base64TestHelper.InitalizeBytes(source, numBytes);

                Span<byte> encodedBytes = new byte[Base64.GetMaxEncodedToUtf8Length(source.Length)];
                Assert.Equal(OperationStatus.Done, Base64.EncodeToUtf8(source, encodedBytes, out int consumed, out int encodedBytesCount));
                Assert.Equal(encodedBytes.Length, encodedBytesCount);

                string encodedText = Text.Encoding.ASCII.GetString(encodedBytes.ToArray());
                string expectedText = Convert.ToBase64String(source.ToArray());
                Assert.Equal(expectedText, encodedText);
            }
        }

        [Fact]
        public void BasicEncodingWithFinalBlockFalseRandomInput()
        {
            var rnd = new Random(42);
            for (int i = 0; i < 10; i++)
            {
                int numBytes = rnd.Next(100, 1000 * 1000);
                Span<byte> source = new byte[numBytes];
                Base64TestHelper.InitalizeBytes(source, numBytes);
                Span<byte> encodedBytes = new byte[Base64.GetMaxEncodedToUtf8Length(source.Length)];
                int expectedConsumed = source.Length / 3 * 3; // only consume closest multiple of three since isFinalBlock is false
                int expectedWritten = source.Length / 3 * 4;

                Assert.Equal(OperationStatus.NeedMoreData, Base64.EncodeToUtf8(source, encodedBytes, out int consumed, out int encodedBytesCount, isFinalBlock: false));
                Assert.Equal(expectedConsumed, consumed);
                Assert.Equal(expectedWritten, encodedBytesCount);

                string encodedText = Text.Encoding.ASCII.GetString(encodedBytes.Slice(0, expectedWritten).ToArray());
                string expectedText = Convert.ToBase64String(source.Slice(0, expectedConsumed).ToArray());
                Assert.Equal(expectedText, encodedText);
            }
        }

        [Theory]
        [InlineData(1, "", 0, 0)]
        [InlineData(2, "", 0, 0)]
        [InlineData(3, "AQID", 3, 4)]
        [InlineData(4, "AQID", 3, 4)]
        [InlineData(5, "AQID", 3, 4)]
        [InlineData(6, "AQIDBAUG", 6, 8)]
        [InlineData(7, "AQIDBAUG", 6, 8)]
        public void BasicEncodingWithFinalBlockFalseKnownInput(int numBytes, string expectedText, int expectedConsumed, int expectedWritten)
        {
            Span<byte> source = new byte[numBytes];
            for (int i = 0; i < numBytes; i++)
            {
                source[i] = (byte)(i + 1);
            }
            Span<byte> encodedBytes = new byte[Base64.GetMaxEncodedToUtf8Length(source.Length)];

            Assert.Equal(OperationStatus.NeedMoreData, Base64.EncodeToUtf8(source, encodedBytes, out int consumed, out int encodedBytesCount, isFinalBlock: false));
            Assert.Equal(expectedConsumed, consumed);
            Assert.Equal(expectedWritten, encodedBytesCount);

            string encodedText = Text.Encoding.ASCII.GetString(encodedBytes.Slice(0, expectedWritten).ToArray());
            Assert.Equal(expectedText, encodedText);
        }

        [Fact]
        public void BasicEncodingWithLineBreaksMime()
        {
            var format = new ParsedFormat('M');
            for (int numBytes = 58; numBytes < 1000; numBytes++)
            {
                Span<byte> source = new byte[numBytes];
                Base64TestHelper.InitalizeBytes(source, numBytes);

                char[] charArray = new char[numBytes * 10];
                Span<byte> encodedBytes = new byte[Base64.GetMaxEncodedToUtf8Length(source.Length)];
                Span<byte> encodedBytesWithLineBreaks = new byte[Base64.GetMaxEncodedToUtf8Length(source.Length, format)];
                Assert.Equal(OperationStatus.Done, Base64.EncodeToUtf8(source, encodedBytes, out int consumed, out int encodedBytesCount));
                Assert.Equal(encodedBytes.Length, encodedBytesCount);
                Assert.True(OperationStatus.Done == Base64.EncodeToUtf8(source, encodedBytesWithLineBreaks, out consumed, out encodedBytesCount, format), "At index: " + numBytes);
                Assert.Equal(encodedBytesWithLineBreaks.Length, encodedBytesCount);

                string encodedText = Text.Encoding.ASCII.GetString(encodedBytes.ToArray());
                string encodedTextWithLineBreaks = Text.Encoding.ASCII.GetString(encodedBytesWithLineBreaks.ToArray());
                string expectedText = Convert.ToBase64String(source.ToArray());
                string expectedTextWithLineBreaks = Convert.ToBase64String(source.ToArray(), Base64FormattingOptions.InsertLineBreaks);
                Assert.Equal(expectedText, encodedText);
                Assert.Equal(expectedTextWithLineBreaks, encodedTextWithLineBreaks);
            }
        }

        [Fact]
        public void BasicEncodingWithLineBreaks()
        {
            var format = new ParsedFormat('N', 64);
            for (int numBytes = 0; numBytes < 1000; numBytes++)
            {
                Span<byte> source = new byte[numBytes];
                Base64TestHelper.InitalizeBytes(source, numBytes);

                char[] charArray = new char[numBytes * 10];
                Span<byte> encodedBytes = new byte[Base64.GetMaxEncodedToUtf8Length(source.Length)];
                Span<byte> encodedBytesWithLineBreaks = new byte[Base64.GetMaxEncodedToUtf8Length(source.Length, format)];
                Assert.True(OperationStatus.Done == Base64.EncodeToUtf8(source, encodedBytesWithLineBreaks, out int consumed, out int encodedBytesCount, format), "At index: " + numBytes);
                Assert.Equal(encodedBytesWithLineBreaks.Length, encodedBytesCount);

                string encodedTextWithLineBreaks = Text.Encoding.ASCII.GetString(encodedBytesWithLineBreaks.ToArray());
                Assert.Equal(encodedTextWithLineBreaks, encodedTextWithLineBreaks);
            }
        }

        [Fact]
        public void EncodingOutputTooSmall()
        {
            Span<byte> source = new byte[750];
            Base64TestHelper.InitalizeBytes(source);

            int outputSize = 320;
            int requiredSize = Base64.GetMaxEncodedToUtf8Length(source.Length);

            Span<byte> encodedBytes = new byte[outputSize];
            Assert.Equal(OperationStatus.DestinationTooSmall,
                Base64.EncodeToUtf8(source, encodedBytes, out int consumed, out int written));
            Assert.Equal(encodedBytes.Length, written);
            Assert.Equal(encodedBytes.Length / 4 * 3, consumed);
            encodedBytes = new byte[requiredSize - outputSize];
            Assert.Equal(OperationStatus.Done,
                Base64.EncodeToUtf8(source.Slice(consumed), encodedBytes, out consumed, out written));
            Assert.Equal(encodedBytes.Length, written);
            Assert.Equal(encodedBytes.Length / 4 * 3, consumed);

            string encodedText = Text.Encoding.ASCII.GetString(encodedBytes.ToArray());
            string expectedText = Convert.ToBase64String(source.ToArray()).Substring(outputSize);
            Assert.Equal(expectedText, encodedText);
        }

        [Fact]
        public void EncodingWithLineBreaksOutputTooSmall()
        {
            Span<byte> source = new byte[750];
            Base64TestHelper.InitalizeBytes(source);

            int outputSize = 316;
            int expectedWritten = 312;

            ParsedFormat format = new ParsedFormat('N', 76);
            int requiredSize = Base64.GetMaxEncodedToUtf8Length(source.Length, format);

            Span<byte> encodedBytes = new byte[outputSize];
            Assert.Equal(OperationStatus.DestinationTooSmall,
                Base64.EncodeToUtf8(source, encodedBytes, out int consumed, out int written, format, false));
            Assert.Equal(expectedWritten, written);

            string encodedText = Text.Encoding.ASCII.GetString(encodedBytes.ToArray());

            encodedBytes = new byte[requiredSize - written];
            Assert.Equal(OperationStatus.Done,
                Base64.EncodeToUtf8(source.Slice(consumed), encodedBytes, out consumed, out written, format));
            Assert.Equal(encodedBytes.Length, written);

            encodedText = Text.Encoding.ASCII.GetString(encodedBytes.ToArray());
            string expectedText = Convert.ToBase64String(source.ToArray(), Base64FormattingOptions.InsertLineBreaks).Substring(expectedWritten);
            Assert.Equal(expectedText, encodedText);
        }

        [Fact]
        public void BasicDecoding()
        {
            var rnd = new Random(42);
            for (int i = 0; i < 10; i++)
            {
                int numBytes = rnd.Next(100, 1000 * 1000);
                while (numBytes % 4 != 0)
                {
                    numBytes = rnd.Next(100, 1000 * 1000);
                }
                Span<byte> source = new byte[numBytes];
                Base64TestHelper.InitalizeDecodableBytes(source, numBytes);

                Span<byte> decodedBytes = new byte[Base64.GetMaxDecodedFromUtf8Length(source.Length)];
                Assert.Equal(OperationStatus.Done, 
                    Base64.DecodeFromUtf8(source, decodedBytes, out int consumed, out int decodedByteCount));
                Assert.Equal(decodedBytes.Length, decodedByteCount);

                string expectedStr = Text.Encoding.ASCII.GetString(source.ToArray());
                byte[] expectedText = Convert.FromBase64String(expectedStr);
                Assert.True(expectedText.AsSpan().SequenceEqual(decodedBytes));
            }
        }

        [Theory]
        [InlineData("A", 0, 0)]
        [InlineData("AQ", 0, 0)]
        [InlineData("AQI", 0, 0)]
        [InlineData("AQIDBA", 4, 3)]
        [InlineData("AQIDBAU", 4, 3)]
        public void BasicDecodingWithFinalBlockFalseKnownInputNeedMore(string inputString, int expectedConsumed, int expectedWritten)
        {
            Span<byte> source = Text.Encoding.ASCII.GetBytes(inputString);
            Span<byte> decodedBytes = new byte[Base64.GetMaxDecodedFromUtf8Length(source.Length)];

            Assert.Equal(OperationStatus.NeedMoreData, Base64.DecodeFromUtf8(source, decodedBytes, out int consumed, out int decodedByteCount, isFinalBlock: false));
            Assert.Equal(expectedConsumed, consumed);
            Assert.Equal(expectedWritten, decodedByteCount);

            string expectedStr = Text.Encoding.ASCII.GetString(source.Slice(0, expectedConsumed).ToArray());
            byte[] expectedText = Convert.FromBase64String(expectedStr);
            Assert.True(expectedText.AsSpan().SequenceEqual(decodedBytes));
        }

        [Theory]
        [InlineData("AQ==", 0, 0)]
        [InlineData("AQI=", 0, 0)]
        [InlineData("AQIDBA==", 4, 3)]
        [InlineData("AQIDBAU=", 4, 3)]
        public void BasicDecodingWithFinalBlockFalseKnownInputInvalid(string inputString, int expectedConsumed, int expectedWritten)
        {
            Span<byte> source = Text.Encoding.ASCII.GetBytes(inputString);
            Span<byte> decodedBytes = new byte[Base64.GetMaxDecodedFromUtf8Length(source.Length)];

            Assert.Equal(OperationStatus.InvalidData, Base64.DecodeFromUtf8(source, decodedBytes, out int consumed, out int decodedByteCount, isFinalBlock: false));
            Assert.Equal(expectedConsumed, consumed);
            Assert.Equal(expectedWritten, decodedByteCount);

            string expectedStr = Text.Encoding.ASCII.GetString(source.Slice(0, expectedConsumed).ToArray());
            byte[] expectedText = Convert.FromBase64String(expectedStr);
            Assert.True(expectedText.AsSpan().SequenceEqual(decodedBytes.Slice(0, decodedByteCount)));
        }

        [Theory]
        [InlineData("AQID", 4, 3)]
        [InlineData("AQIDBAUG", 8, 6)]
        public void BasicDecodingWithFinalBlockFalseKnownInputNeedMoreData(string inputString, int expectedConsumed, int expectedWritten)
        {
            Span<byte> source = Text.Encoding.ASCII.GetBytes(inputString);
            Span<byte> decodedBytes = new byte[Base64.GetMaxDecodedFromUtf8Length(source.Length)];

            Assert.Equal(OperationStatus.NeedMoreData, Base64.DecodeFromUtf8(source, decodedBytes, out int consumed, out int decodedByteCount, isFinalBlock: false));
            Assert.Equal(expectedConsumed, consumed);
            Assert.Equal(expectedWritten, decodedByteCount);

            string expectedStr = Text.Encoding.ASCII.GetString(source.Slice(0, expectedConsumed).ToArray());
            byte[] expectedText = Convert.FromBase64String(expectedStr);
            Assert.True(expectedText.AsSpan().SequenceEqual(decodedBytes.Slice(0, decodedByteCount)));
        }

        [Theory]
        [InlineData("A", 0, 0)]
        [InlineData("AQ", 0, 0)]
        [InlineData("AQI", 0, 0)]
        [InlineData("AQIDBA", 4, 3)]
        [InlineData("AQIDBAU", 4, 3)]
        public void BasicDecodingWithFinalBlockTrueKnownInputInvalid(string inputString, int expectedConsumed, int expectedWritten)
        {
            Span<byte> source = Text.Encoding.ASCII.GetBytes(inputString);
            Span<byte> decodedBytes = new byte[Base64.GetMaxDecodedFromUtf8Length(source.Length)];

            Assert.Equal(OperationStatus.InvalidData, Base64.DecodeFromUtf8(source, decodedBytes, out int consumed, out int decodedByteCount));
            Assert.Equal(expectedConsumed, consumed);
            Assert.Equal(expectedWritten, decodedByteCount);

            string expectedStr = Text.Encoding.ASCII.GetString(source.Slice(0, expectedConsumed).ToArray());
            byte[] expectedText = Convert.FromBase64String(expectedStr);
            Assert.True(expectedText.AsSpan().SequenceEqual(decodedBytes.Slice(0, decodedByteCount)));
        }

        [Theory]
        [InlineData("AQ==", 4, 1)]
        [InlineData("AQI=", 4, 2)]
        [InlineData("AQID", 4, 3)]
        [InlineData("AQIDBA==", 8, 4)]
        [InlineData("AQIDBAU=", 8, 5)]
        [InlineData("AQIDBAUG", 8, 6)]
        public void BasicDecodingWithFinalBlockTrueKnownInputDone(string inputString, int expectedConsumed, int expectedWritten)
        {
            Span<byte> source = Text.Encoding.ASCII.GetBytes(inputString);
            Span<byte> decodedBytes = new byte[Base64.GetMaxDecodedFromUtf8Length(source.Length)];

            Assert.Equal(OperationStatus.Done, Base64.DecodeFromUtf8(source, decodedBytes, out int consumed, out int decodedByteCount));
            Assert.Equal(expectedConsumed, consumed);
            Assert.Equal(expectedWritten, decodedByteCount);

            string expectedStr = Text.Encoding.ASCII.GetString(source.Slice(0, expectedConsumed).ToArray());
            byte[] expectedText = Convert.FromBase64String(expectedStr);
            Assert.True(expectedText.AsSpan().SequenceEqual(decodedBytes.Slice(0, decodedByteCount)));
        }

        [Fact]
        public void DecodingInvalidBytes()
        {
            int[] invalidBytes = Base64TestHelper.s_decodingMap.FindAllIndexOf(Base64TestHelper.s_invalidByte);

            for (int j = 0; j < 8; j++)
            {
                Span<byte> source = new byte[] { 50, 50, 50, 50, 80, 80, 80, 80 };
                Span<byte> decodedBytes = new byte[Base64.GetMaxDecodedFromUtf8Length(source.Length)];

                for (int i = 0; i < invalidBytes.Length; i++)
                {
                    // Don't test padding, which is tested in DecodingInvalidBytesPadding
                    if ((byte)invalidBytes[i] == Base64TestHelper.s_encodingPad) continue;

                    source[j] = (byte)invalidBytes[i];

                    Assert.Equal(OperationStatus.InvalidData,
                        Base64.DecodeFromUtf8(source, decodedBytes, out int consumed, out int decodedByteCount));
                }
            }
        }

        [Fact]
        public void DecodingInvalidBytesPadding()
        {
            // Only last 2 bytes can be padding, all other occurrence of padding is invalid
            for (int j = 0; j < 7; j++)
            {
                Span<byte> source = new byte[] { 50, 50, 50, 50, 80, 80, 80, 80 };
                Span<byte> decodedBytes = new byte[Base64.GetMaxDecodedFromUtf8Length(source.Length)];
                source[j] = Base64TestHelper.s_encodingPad;
                Assert.Equal(OperationStatus.InvalidData,
                    Base64.DecodeFromUtf8(source, decodedBytes, out int consumed, out int decodedByteCount));
            }

            {
                Span<byte> source = new byte[] { 50, 50, 50, 50, 80, 80, 80, 80 };
                Span<byte> decodedBytes = new byte[Base64.GetMaxDecodedFromUtf8Length(source.Length)];
                source[6] = Base64TestHelper.s_encodingPad;
                source[7] = Base64TestHelper.s_encodingPad;
                Assert.Equal(OperationStatus.Done,
                    Base64.DecodeFromUtf8(source, decodedBytes, out int consumed, out int decodedByteCount));

                source = new byte[] { 50, 50, 50, 50, 80, 80, 80, 80 };
                source[7] = Base64TestHelper.s_encodingPad;
                Assert.Equal(OperationStatus.Done,
                    Base64.DecodeFromUtf8(source, decodedBytes, out consumed, out decodedByteCount));
            }
        }

        [Fact]
        public void DecodingOutputTooSmall()
        {
            Span<byte> source = new byte[1000];
            Base64TestHelper.InitalizeDecodableBytes(source);

            int outputSize = 240;
            int requiredSize = Base64.GetMaxDecodedFromUtf8Length(source.Length);

            Span<byte> decodedBytes = new byte[outputSize];
            Assert.Equal(OperationStatus.DestinationTooSmall, 
                Base64.DecodeFromUtf8(source, decodedBytes, out int consumed, out int decodedByteCount));
            Assert.Equal(decodedBytes.Length, decodedByteCount);
            Assert.Equal(decodedBytes.Length / 3 * 4, consumed);
            decodedBytes = new byte[requiredSize - outputSize];
            Assert.Equal(OperationStatus.Done,
                Base64.DecodeFromUtf8(source.Slice(consumed), decodedBytes, out consumed, out decodedByteCount));
            Assert.Equal(decodedBytes.Length, decodedByteCount);
            Assert.Equal(decodedBytes.Length / 3 * 4, consumed);

            string expectedStr = Text.Encoding.ASCII.GetString(source.ToArray());
            byte[] expectedText = Convert.FromBase64String(expectedStr);
            Assert.True(expectedText.AsSpan().Slice(outputSize).SequenceEqual(decodedBytes));
        }

        [Fact]
        public void ComputeEncodedLength()
        {
            // (int.MaxValue - 4)/(4/3) => 1610612733, otherwise integer overflow
            int[] input = { 0, 1, 2, 3, 4, 5, 6, 1610612728, 1610612729, 1610612730, 1610612731, 1610612732, 1610612733 };
            int[] expected = { 0, 4, 4, 4, 8, 8, 8, 2147483640, 2147483640, 2147483640, 2147483644, 2147483644, 2147483644 };
            for (int i = 0; i < input.Length; i++)
            {
                Assert.Equal(expected[i], Base64.GetMaxEncodedToUtf8Length(input[i]));
            }

            // integer overflow
            Assert.Throws<OverflowException>(() => Base64.GetMaxEncodedToUtf8Length(1610612734));
            Assert.Throws<OverflowException>(() => Base64.GetMaxEncodedToUtf8Length(int.MaxValue));
        }

        [Fact]
        public void ComputeDecodedLength()
        {
            Span<byte> sourceEmpty = Span<byte>.Empty;
            Assert.Equal(0, Base64.GetMaxDecodedFromUtf8Length(0));

            int[] input = { 4, 8, 12, 16, 20 };
            int[] expected = { 3, 6, 9, 12, 15 };

            for (int i = 0; i < input.Length; i++)
            {
                int sourceLength = input[i];
                Assert.Equal(expected[i], Base64.GetMaxDecodedFromUtf8Length(sourceLength));
            }

            // int.MaxValue - (int.MaxValue % 4) => 2147483644, largest multiple of 4 less than int.MaxValue
            // CLR default limit of 2 gigabytes (GB).
            try
            {
                int sourceLength = 2000000000;
                int expectedLength = 1500000000;
                Assert.Equal(expectedLength, Base64.GetMaxDecodedFromUtf8Length(sourceLength));
            }
            catch (OutOfMemoryException)
            {
                // do nothing
            }

            // Lengths that are not a multiple of 4.
            int[] lengthsNotMultipleOfFour = { 1, 2, 3, 5, 6, 7, 9, 10, 11, 13, 14, 15, 1001, 1002, 1003 };
            int[] expectedOutput = { 0, 0, 0, 3, 3, 3, 6, 6, 6, 9, 9, 9, 750, 750, 750 };
            for (int i = 0; i < lengthsNotMultipleOfFour.Length; i++)
            {
                int sourceLength = lengthsNotMultipleOfFour[i];
                Span<byte> source = new byte[sourceLength];
                Assert.Equal(expectedOutput[i], Base64.GetMaxDecodedFromUtf8Length(sourceLength));
            }
        }

        [Fact]
        public void DecodeInPlace()
        {
            var list = new List<byte>();
            for (int value = 0; value < 256; value++)
            {
                list.Add((byte)value);
            }
            var testBytes = list.ToArray();

            for (int value = 0; value < 256; value++)
            {
                var sourceBytes = testBytes.AsSpan().Slice(0, value + 1);
                var buffer = new byte[Base64.GetMaxEncodedToUtf8Length(sourceBytes.Length)];
                var bufferSlice = buffer.AsSpan();

                Base64.EncodeToUtf8(sourceBytes, bufferSlice, out int consumed, out int written);

                var encodedText = Text.Encoding.ASCII.GetString(bufferSlice.ToArray());
                var expectedText = Convert.ToBase64String(testBytes, 0, value + 1);
                Assert.Equal(expectedText, encodedText);

                Assert.Equal(OperationStatus.Done, Base64.DecodeFromUtf8InPlace(bufferSlice, bufferSlice.Length, out int bytesWritten));
                Assert.Equal(sourceBytes.Length, bytesWritten);

                for (int i = 0; i < bytesWritten; i++)
                {
                    Assert.Equal(sourceBytes[i], bufferSlice[i]);
                }
            }
        }

        [Fact(Skip = "Need to propogate isFinalBlock properly to the IBufferOperation Execute method used in Pipe")]
        public void ValidInputOnlyMultiByte()
        {
            Span<byte> inputSpan = new byte[1000];
            Base64TestHelper.InitalizeDecodableBytes(inputSpan);
            int requiredLength = Base64.GetMaxDecodedFromUtf8Length(inputSpan.Length);
            Span<byte> expected = new byte[requiredLength];
            Assert.Equal(OperationStatus.Done, Base64.DecodeFromUtf8(inputSpan, expected, out int bytesConsumed, out int bytesWritten));

            byte[][] input = new byte[10][];

            int[] split = { 100, 102, 98, 101, 2, 97, 101, 1, 2, 396 };

            int sum = 0;
            for (int i = 0; i < split.Length; i++)
            {
                int splitter = split[i];
                input[i] = inputSpan.Slice(sum, splitter).ToArray();
                sum += splitter;
            }
            Assert.Equal(1000, sum);

            var output = new TestOutput();
            Base64.Utf8ToBytesDecoder.Pipe(ReadOnlyBytes.Create(input), output);

            var expectedArray = expected.ToArray();
            var array = output.GetBuffer.ToArray();

            Assert.True(expected.SequenceEqual(output.GetBuffer.Slice(0, requiredLength)));
        }

        [Fact]
        public void EncodeInPlace()
        {
            const int numberOfBytes = 15;

            var list = new List<byte>();
            for (int value = 0; value < numberOfBytes; value++)
            {
                list.Add((byte)value);
            }
            // padding
            for (int i = 0; i < (numberOfBytes / 3) + 2; i++)
            {
                list.Add(0);
            }

            var testBytes = list.ToArray();

            for (int numberOfBytesToTest = 1; numberOfBytesToTest <= numberOfBytes; numberOfBytesToTest++)
            {
                var copy = testBytes.Clone();
                var expectedText = Convert.ToBase64String(testBytes, 0, numberOfBytesToTest);

                Assert.Equal(OperationStatus.Done, Base64.EncodeToUtf8InPlace(testBytes, numberOfBytesToTest, out int bytesWritten));
                Assert.Equal(Base64.GetMaxEncodedToUtf8Length(numberOfBytesToTest), bytesWritten);

                var encodedText = Text.Encoding.ASCII.GetString(testBytes, 0, bytesWritten);

                Assert.Equal(expectedText, encodedText);
            }
        }
    }

    class TestOutput : IOutput
    {
        byte[] _buffer = new byte[1000];

        int _written = 0;

        public Span<byte> GetBuffer => _buffer;

        public Span<byte> Buffer => _buffer.AsSpan().Slice(_written);

        public void Advance(int bytes)
        {
            _written += bytes;
        }

        public void Enlarge(int desiredBufferLength = 0)
        {
        }
    }

}
