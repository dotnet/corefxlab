// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Xunit;
using System.Collections.Generic;
using System.Buffers;

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
                Span<byte> encodedBytes = new byte[Base64Encoder.ComputeEncodedLength(sourceBytes.Length)];
                Assert.True(Base64Encoder.TryEncode(sourceBytes, encodedBytes, out int consumed, out int encodedBytesCount));
                Assert.Equal(encodedBytes.Length, encodedBytesCount);

                string encodedText = Text.Encoding.ASCII.GetString(encodedBytes.ToArray());
                string expectedText = Convert.ToBase64String(bytes, 0, value + 1);
                Assert.Equal(expectedText, encodedText);

                if (encodedBytes.Length % 4 == 0)
                {
                    Span<byte> decodedBytes = new byte[Base64Encoder.ComputeDecodedLength(encodedBytes)];
                    Assert.True(Base64Encoder.TryDecode(encodedBytes, decodedBytes, out consumed, out int decodedByteCount));
                    Assert.Equal(sourceBytes.Length, decodedByteCount);
                    Assert.True(sourceBytes.SequenceEqual(decodedBytes));
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

                Span<byte> encodedBytes = new byte[Base64Encoder.ComputeEncodedLength(source.Length)];
                Assert.True(Base64Encoder.TryEncode(source, encodedBytes, out int consumed, out int encodedBytesCount));
                Assert.Equal(encodedBytes.Length, encodedBytesCount);

                string encodedText = Text.Encoding.ASCII.GetString(encodedBytes.ToArray());
                string expectedText = Convert.ToBase64String(source.ToArray());
                Assert.Equal(expectedText, encodedText);
            }
        }

        [Fact]
        public void EncodingOutputTooSmall()
        {
            Span<byte> source = new byte[750];
            Base64TestHelper.InitalizeBytes(source);

            int outputSize = 320;
            int requiredSize = Base64Encoder.ComputeEncodedLength(source.Length);

            Span<byte> encodedBytes = new byte[outputSize];
            Assert.False(Base64Encoder.TryEncode(source, encodedBytes, out int consumed, out int written));
            Assert.Equal(encodedBytes.Length, written);
            Assert.Equal(encodedBytes.Length / 4 * 3, consumed);
            encodedBytes = new byte[requiredSize - outputSize];
            Assert.True(Base64Encoder.TryEncode(source.Slice(consumed), encodedBytes, out consumed, out written));
            Assert.Equal(encodedBytes.Length, written);
            Assert.Equal(encodedBytes.Length / 4 * 3, consumed);

            string encodedText = Text.Encoding.ASCII.GetString(encodedBytes.ToArray());
            string expectedText = Convert.ToBase64String(source.ToArray()).Substring(outputSize);
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

                Span<byte> decodedBytes = new byte[Base64Encoder.ComputeDecodedLength(source)];
                Assert.True(Base64Encoder.TryDecode(source, decodedBytes, out int consumed, out int decodedByteCount));
                Assert.Equal(decodedBytes.Length, decodedByteCount);

                string expectedStr = Text.Encoding.ASCII.GetString(source.ToArray());
                byte[] expectedText = Convert.FromBase64String(expectedStr);
                Assert.True(expectedText.AsSpan().SequenceEqual(decodedBytes));
            }
        }

        [Fact]
        public void DecodingOutputTooSmall()
        {
            Span<byte> source = new byte[1000];
            Base64TestHelper.InitalizeDecodableBytes(source);

            int outputSize = 240;
            int requiredSize = Base64Encoder.ComputeDecodedLength(source);

            Span<byte> decodedBytes = new byte[outputSize];
            Assert.False(Base64Encoder.TryDecode(source, decodedBytes, out int consumed, out int decodedByteCount));
            Assert.Equal(decodedBytes.Length, decodedByteCount);
            Assert.Equal(decodedBytes.Length / 3 * 4, consumed);
            decodedBytes = new byte[requiredSize - outputSize];
            Assert.True(Base64Encoder.TryDecode(source.Slice(consumed), decodedBytes, out consumed, out decodedByteCount));
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
                Assert.Equal(expected[i], Base64Encoder.ComputeEncodedLength(input[i]));
            }

            Assert.True(Base64Encoder.ComputeEncodedLength(1610612734) < 0);   // integer overflow
        }

        [Fact]
        public void ComputeDecodedLength()
        {
            Span<byte> sourceEmpty = Span<byte>.Empty;
            Assert.Equal(0, Base64Encoder.ComputeDecodedLength(sourceEmpty));

            // int.MaxValue - (int.MaxValue % 4) => 2147483644, largest multiple of 4 less than int.MaxValue
            // CLR default limit of 2 gigabytes (GB).
            int[] input = { 4, 8, 12, 16, 20, 2000000000 };
            int[] expected = { 3, 6, 9, 12, 15, 1500000000 };

            for (int i = 0; i < input.Length; i++)
            {
                int sourceLength = input[i];
                Span<byte> source = new byte[sourceLength];
                Assert.Equal(expected[i], Base64Encoder.ComputeDecodedLength(source));
                source[sourceLength - 1] = Base64TestHelper.s_encodingPad;                          // single character padding
                Assert.Equal(expected[i] - 1, Base64Encoder.ComputeDecodedLength(source));
                source[sourceLength - 2] = Base64TestHelper.s_encodingPad;                          // two characters padding
                Assert.Equal(expected[i] - 2, Base64Encoder.ComputeDecodedLength(source));
            }

            // Lengths that are not a multiple of 4.
            int[] lengthsNotMultipleOfFour = { 1, 2, 3, 5, 6, 7, 9, 10, 11, 13, 14, 15, 1001, 1002, 1003 };
            int[] expectedOutput = { 0, 0, 0, 3, 3, 3, 6, 6, 6, 9, 9, 9, 750, 750, 750 };
            for (int i = 0; i < lengthsNotMultipleOfFour.Length; i++)
            {
                int sourceLength = lengthsNotMultipleOfFour[i];
                Span<byte> source = new byte[sourceLength];
                Assert.Equal(expectedOutput[i], Base64Encoder.ComputeDecodedLength(source));
                source[sourceLength - 1] = Base64TestHelper.s_encodingPad;
                Assert.Equal(expectedOutput[i], Base64Encoder.ComputeDecodedLength(source));
                if (sourceLength > 1)
                {
                    source[sourceLength - 2] = Base64TestHelper.s_encodingPad;
                    Assert.Equal(expectedOutput[i], Base64Encoder.ComputeDecodedLength(source));
                }
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
                var buffer = new byte[Base64Encoder.ComputeEncodedLength(sourceBytes.Length)];
                var bufferSlice = buffer.AsSpan();

                Base64Encoder.TryEncode(sourceBytes, bufferSlice, out int consumed, out int written);

                var encodedText = Text.Encoding.ASCII.GetString(bufferSlice.ToArray());
                var expectedText = Convert.ToBase64String(testBytes, 0, value + 1);
                Assert.Equal(expectedText, encodedText);

                var decodedByteCount = Base64Encoder.DecodeInPlace(bufferSlice);
                Assert.Equal(sourceBytes.Length, decodedByteCount);

                for (int i = 0; i < decodedByteCount; i++)
                {
                    Assert.Equal(sourceBytes[i], buffer[i]);
                }
            }
        }

        [Fact]
        public void ValidInputOnlyMultiByte()
        {
            Span<byte> inputSpan = new byte[1000];
            Base64TestHelper.InitalizeDecodableBytes(inputSpan);
            Buffer<byte> source = inputSpan.ToArray();
            byte[] initialSource = source.ToArray();
            List<Buffer<byte>> inputSource = new List<Buffer<byte>>();


            int[] split = { 100, 102, 98, 101, 2, 97, 101, 1, 2, 396 };

            int sum = 0;
            for (int i = 0; i < split.Length; i++)
            {
                int splitter = split[i];
                Buffer<byte> temp = source.Slice(sum, splitter);
                byte[] x = temp.ToArray();

                inputSource.Add(temp);
                sum += splitter;
            }
            Assert.Equal(1000, sum);

            Assert.Equal(split.Length, inputSource.Count);
            for (int i = 0; i < split.Length; i++)
            {
                Assert.Equal(split[i], inputSource[i].Length);
            }

            byte[] passedArray = new byte[1000];
            int mySum = 0;
            for (int i = 0; i < inputSource.Count; i++)
            {
                var tempArr = inputSource[i].ToArray();
                Array.Copy(tempArr, 0, passedArray, mySum, tempArr.Length);
                mySum += tempArr.Length;
            }

            Assert.Equal(initialSource, passedArray);

            var output = new TestOutput();
            Assert.True(Decode(inputSource, output));


        }

        const int stackLength = 32;

        private bool Decode(IEnumerable<Buffer<byte>> source, IOutput destination)
        {
            int afterMergeSlice = 0;
            Span<byte> remainder;
            Span<byte> stackSpan;

            unsafe
            {
                byte* stackBytes = stackalloc byte[stackLength];
                stackSpan = new Span<byte>(stackBytes, stackLength);
            }

            foreach (var sourceBuffer in source)
            {
                Span<byte> outputSpan = destination.Buffer;
                Span<byte> sourceSpan = sourceBuffer.Span;

                if (!remainder.IsEmpty)
                {
                    int leftOverBytes = remainder.Length;
                    remainder.CopyTo(stackSpan);
                    int amountToCopy = Math.Min(sourceSpan.Length, stackSpan.Length - leftOverBytes);
                    sourceSpan.Slice(0, amountToCopy).CopyTo(stackSpan.Slice(leftOverBytes));
                    int amountOfData = leftOverBytes + amountToCopy;

                    Span<byte> spanToDecode = stackSpan.Slice(0, amountOfData);

                    TryDecodeWithRemainder:
                    if (!Base64Encoder.TryDecode(spanToDecode, outputSpan, out int bytesConsumed, out int bytesWritten))
                    {
                        destination.Advance(bytesWritten);
                        spanToDecode = spanToDecode.Slice(bytesConsumed);
                        // Not successful
                        if (outputSpan.Length - bytesWritten < 3)
                        {
                            destination.Enlarge();  // output buffer is too small
                            outputSpan = destination.Buffer;

                            if (outputSpan.Length - bytesWritten < 3)
                            {
                                return false; // no more output space, user decides what to do.
                            }
                            goto TryDecodeWithRemainder;
                        }
                        else
                        {
                            if (spanToDecode.Length >= 4)
                            {
                                continue; // source buffer contains invalid bytes, user decides what to do for fallback
                            }

                            // left over bytes in stack span
                            remainder = spanToDecode;
                        }
                        continue;
                    }
                    else    // success
                    {
                        afterMergeSlice = bytesConsumed - remainder.Length;
                        remainder = Span<byte>.Empty;
                        destination.Advance(bytesWritten);
                        outputSpan = destination.Buffer;
                    }
                }

                TryDecode:
                bool result = Base64Encoder.TryDecode(sourceSpan.Slice(afterMergeSlice), outputSpan, out int consumed, out int written);
                afterMergeSlice = 0;
                destination.Advance(written);
                sourceSpan = sourceSpan.Slice(consumed);

                if (result) continue;

                // Not successful
                if (outputSpan.Length - written < 3)
                {
                    destination.Enlarge();  // output buffer is too small
                    outputSpan = destination.Buffer;
                    if (outputSpan.Length - written < 3)
                    {
                        return false; // no more output space, user decides what to do.
                    }
                    goto TryDecode;
                }
                else
                {
                    if (sourceSpan.Length >= 4)
                    {
                        continue; // source buffer contains invalid bytes, user decides what to do for fallback
                    }

                    // left over bytes in source span
                    remainder = sourceSpan;
                }
            }
            return true;
        }

        // BasicUnitTests.cs(318,54,318,76): error CS1601: Cannot make reference to variable of type 'Span<byte>'
        /*private int Stitch(ref Span<byte> remainder, ref Span<byte> segment, Span<byte> stackSpan)
        {
            int leftOverBytes = remainder.Length;
            remainder.CopyTo(stackSpan);
            int amountToCopy = Math.Min(segment.Length, stackSpan.Length - leftOverBytes);
            segment.Slice(0, amountToCopy).CopyTo(stackSpan.Slice(leftOverBytes));
            return leftOverBytes + amountToCopy;
        }*/

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

                var encoded = Base64Encoder.EncodeInPlace(testBytes, numberOfBytesToTest);
                var encodedText = Text.Encoding.ASCII.GetString(testBytes, 0, encoded);

                Assert.Equal(expectedText, encodedText);
            }
        }
    }

    class TestOutput : IOutput
    {
        byte[] _buffer = new byte[1000];

        public Span<byte> Buffer => _buffer;

        public void Advance(int bytes)
        {
        }

        public void Enlarge(int desiredBufferLength = 0)
        {
        }
    }

}
