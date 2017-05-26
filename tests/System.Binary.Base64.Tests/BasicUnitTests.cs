// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Xunit;
using System.Collections.Generic;
using System.Buffers;

namespace System.Binary.Base64.Tests
{
    public class Base64Tests
    {
        static string s_characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";

        // Pre-computing this table using a custom string(s_characters) and GenerateEncodingMapAndVerify (found in tests)
        static readonly byte[] s_encodingMap = {
            65, 66, 67, 68, 69, 70, 71, 72,         //A..H
            73, 74, 75, 76, 77, 78, 79, 80,         //I..P
            81, 82, 83, 84, 85, 86, 87, 88,         //Q..X
            89, 90, 97, 98, 99, 100, 101, 102,      //Y..Z, a..f
            103, 104, 105, 106, 107, 108, 109, 110, //g..n
            111, 112, 113, 114, 115, 116, 117, 118, //o..v
            119, 120, 121, 122, 48, 49, 50, 51,     //w..z, 0..3
            52, 53, 54, 55, 56, 57, 43, 47,         //4..9, +, /
            61                                      // =
        };

        // Pre-computing this table using a custom string(s_characters) and GenerateDecodingMapAndVerify (found in tests)
        static readonly byte[] s_decodingMap = {
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 62, 255, 255, 255, 63,   //62 is placed at index 43 (for +), 63 at index 47 (for /)
            52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 255, 255, 255, 64, 255, 255,            //52-61 are placed at index 48-57 (for 0-9), 64 at index 61 (for =)
            255, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14,
            15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 255, 255, 255, 255, 255,            //0-25 are placed at index 65-90 (for A-Z)
            255, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40,
            41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 255, 255, 255, 255, 255,            //26-51 are placed at index 97-122 (for a-z)
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, // Bytes over 122 ('z') are invalid and cannot be decoded
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, // Hence, padding the map with 255, which indicates invalid input
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
        };

        static readonly byte s_encodingPad = 61;                    // s_encodingMap[64] is '=', for padding

        static readonly byte s_invalidByte = byte.MaxValue;         // Designating 255 for invalid bytes in the decoding map

        static void InitalizeBytes(Span<byte> bytes, int seed = 100)
        {
            var rnd = new Random(seed);
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)rnd.Next(0, byte.MaxValue + 1);
            }
        }

        static void InitalizeDecodableBytes(Span<byte> bytes, int seed = 100)
        {
            var rnd = new Random(seed);
            for (int i = 0; i < bytes.Length; i++)
            {
                int index = (byte)rnd.Next(0, s_encodingMap.Length - 1);    // Do not pick '='
                bytes[i] = s_encodingMap[index];
            }
        }

        [Fact]
        public void StichingTestNoStichingNeeded()
        {
            Span<byte> source = new byte[1000];
            InitalizeDecodableBytes(source);
            Span<byte> expected = new byte[1000];
            Base64Encoder.TryDecode(source, expected, out int expectedConsumed, out int expectedWritten);

            ReadOnlySpan<byte> source1 = source.Slice(0, 400);
            ReadOnlySpan<byte> source2 = source.Slice(400, 600);

            Span<byte> destination = new byte[1000]; // Plenty of space

            if (!Base64Encoder.TryDecode(source1, destination, out int bytesConsumed, out int bytesWritten))
            {
                // this shouldn't happen!
            }
            Base64Encoder.TryDecode(source2, destination.Slice(bytesWritten), out int consumed, out int written);
            bytesConsumed += consumed;
            bytesWritten += written;

            Assert.Equal(expectedConsumed, bytesConsumed);
            Assert.Equal(expectedWritten, bytesWritten);
            Assert.True(expected.SequenceEqual(destination));
        }


        [Fact]
        public void StichingTestStichingRequired()
        {
            Span<byte> source = new byte[1000];
            InitalizeDecodableBytes(source);
            Span<byte> expected = new byte[1000];
            Base64Encoder.TryDecode(source, expected, out int expectedConsumed, out int expectedWritten);

            int stackSize = 32;

            ReadOnlySpan<byte> source1 = source.Slice(0, 402);
            ReadOnlySpan<byte> source2 = source.Slice(402, 598);

            Span<byte> stackSpan;

            unsafe
            {
                byte* stackBytes = stackalloc byte[stackSize];
                stackSpan = new Span<byte>(stackBytes, stackSize);
            }

            Span<byte> destination = new byte[1000]; // Plenty of space

            int afterMergeSlice = 0;
            if (!Base64Encoder.TryDecode(source1, destination, out int bytesConsumed, out int bytesWritten))
            {
                int leftOverBytes = source1.Length - bytesConsumed;
                if (leftOverBytes < 4)
                {
                    source1.Slice(bytesConsumed).CopyTo(stackSpan);
                    int amountToCopy = Math.Min(source2.Length, stackSpan.Length - leftOverBytes);
                    source2.Slice(0, amountToCopy).CopyTo(stackSpan.Slice(leftOverBytes));

                    Base64Encoder.TryDecode(stackSpan, destination.Slice(bytesWritten), out int consumed, out int written);
                    afterMergeSlice = consumed - leftOverBytes;
                    bytesConsumed += consumed;
                    bytesWritten += written;
                }
            }
            Base64Encoder.TryDecode(source2.Slice(afterMergeSlice), destination.Slice(bytesWritten), out int consumed3, out int written3);
            bytesConsumed += consumed3;
            bytesWritten += written3;

            Assert.Equal(expectedConsumed, bytesConsumed);
            Assert.Equal(expectedWritten, bytesWritten);
            Assert.True(expected.SequenceEqual(destination));
        }


        [Fact]
        public void StichingTestNoThirdCall()
        {
            Span<byte> source = new byte[1000];
            InitalizeDecodableBytes(source);
            Span<byte> expected = new byte[1000];
            Base64Encoder.TryDecode(source, expected, out int expectedConsumed, out int expectedWritten);

            ReadOnlySpan<byte> source1 = source.Slice(0, 402);
            ReadOnlySpan<byte> source2 = source.Slice(402, 598);

            Span<byte> stackSpan;

            unsafe
            {
                byte* stackBytes = stackalloc byte[600];
                stackSpan = new Span<byte>(stackBytes, 600);
            }

            Span<byte> destination = new byte[1000]; // Plenty of space

            if (!Base64Encoder.TryDecode(source1, destination, out int bytesConsumed, out int bytesWritten))
            {
                int leftOverBytes = source1.Length - bytesConsumed;
                if (leftOverBytes < 4)
                {
                    source1.Slice(bytesConsumed).CopyTo(stackSpan);
                    int amountToCopy = Math.Min(source2.Length, stackSpan.Length - leftOverBytes);
                    source2.Slice(0, amountToCopy).CopyTo(stackSpan.Slice(leftOverBytes));

                    Base64Encoder.TryDecode(stackSpan, destination.Slice(bytesWritten), out int consumed, out int written);
                    bytesConsumed += consumed;
                    bytesWritten += written;
                }
            }

            Assert.Equal(expectedConsumed, bytesConsumed);
            Assert.Equal(expectedWritten, bytesWritten);
            Assert.True(expected.SequenceEqual(destination));
        }

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
                InitalizeBytes(source, numBytes);

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
            InitalizeBytes(source);

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
                InitalizeDecodableBytes(source, numBytes);

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
            InitalizeDecodableBytes(source);

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
        public void SampleUsage1()
        {
            Span<byte> source = new byte[1000];
            InitalizeDecodableBytes(source);

            Span<byte> decodedBytes = new byte[Base64Encoder.ComputeDecodedLength(source)];

            Assert.True(Base64Encoder.TryDecode(source, decodedBytes, out int consumed, out int written));
            Assert.Equal(source.Length, consumed);
            Assert.Equal(decodedBytes.Length, written);
        }

        [Fact]
        public void SampleUsage2()
        {
            var rnd = new Random(42);
            Span<byte> source = new byte[1000];
            InitalizeDecodableBytes(source);

            int numBytes = rnd.Next(100, 251);
            Span<byte> decodedBytes = new byte[numBytes];

            int bytesConsumed = 0;
            int bytesWritten = 0;

            bool success = false;
            do
            {
                success = Base64Encoder.TryDecode(source.Slice(bytesConsumed), decodedBytes, out int consumed, out int written);
                bytesConsumed += consumed;
                bytesWritten += written;
                numBytes = rnd.Next(100, 251);
                decodedBytes = new byte[numBytes];
            } while (!success);

            Assert.Equal(source.Length, bytesConsumed);
            Assert.Equal(Base64Encoder.ComputeDecodedLength(source), bytesWritten);
        }

        [Fact]
        public void SampleUsage3()
        {
            Span<byte> inputBuffer = new byte[] { 84, 85, 49, 78, 89, 87, 70, 104, 80, 81, 61, 61 };
            Span<byte> outputBuffer = new byte[7];
            Span<byte> expectedBuffer = new byte[] { 77, 77, 77, 97, 97, 97, 61 };

            // 77 77 77 97 97 97 61 <=> T U 1 N Y W F h P Q = =

            // T U 1 N Y W F h P Q = = => 84 85 49 78 89 87 70 104 80 81 61 61 
            // T U 1 N => 77 77 77
            // Y W => null => .
            // Y W F h P Q => Y W F h => 97 97 97
            // P Q = = => 61

            Span<byte> span1 = inputBuffer.Slice(0, 4);
            Span<byte> span2 = inputBuffer.Slice(4, 2);
            Span<byte> span3 = inputBuffer.Slice(6, 4);
            Span<byte> span4 = inputBuffer.Slice(8, 4);

            int[] sourceSegment = { 4, 2, 6, 4 };
            int[] destinationSegment = { 3, 3, 4, 1 };

            int counter = 0;
            int bytesConsumed = 0;
            int bytesWritten = 0;
            bool successfulDecode = false;
            do
            {
                ReadOnlySpan<byte> source = inputBuffer.Slice(bytesConsumed, sourceSegment[counter]);
                Span<byte> decodedBytes = outputBuffer.Slice(bytesWritten, destinationSegment[counter]);
                successfulDecode = Base64Encoder.TryDecode(source, decodedBytes, out int consumed, out int written);
                bytesConsumed += consumed;
                bytesWritten += written;
                counter++;
            } while (!successfulDecode || bytesConsumed != inputBuffer.Length);

            Assert.True(expectedBuffer.SequenceEqual(outputBuffer));
        }

        public void SampleUsage4()
        {
            // Assume this is the entire multi-span buffer.
            Span<byte> inputBuffer = new byte[] { 84, 85, 49, 78, 89, 87, 70, 104, 80, 81, 61, 61 };
            Span<byte> outputBuffer = new byte[7];
            Span<byte> expectedBuffer = new byte[] { 77, 77, 77, 97, 97, 97, 61 };

            // 77 77 77 97 97 97 61 <=> T U 1 N Y W F h P Q = =

            // T U 1 N Y W F h P Q = = => 84 85 49 78 89 87 70 104 80 81 61 61 
            // T U 1 N => 77 77 77
            // Y W => null => .
            // Y W F h P Q => Y W F h => 97 97 97
            // P Q = = => 61

            Span<byte> firstSegment = inputBuffer.Slice(0, 4);  // T U 1 N => 77 77 77
            Span<byte> span2 = inputBuffer.Slice(4, 2);         // Y W => null => .
            Span<byte> span3 = inputBuffer.Slice(6, 4);         // F h P Q => Y W F h P Q => Y W F h => 97 97 97
            Span<byte> span4 = inputBuffer.Slice(8, 4);         // P Q = = => 61

            bool successfulDecode = false;

            ReadOnlySpan<byte> source = firstSegment;
            Span<byte> decodedBytes = outputBuffer.Slice(0, 3);

            int consumedFromMerge = 0;
            do
            {
                successfulDecode = Base64Encoder.TryDecode(source.Slice(consumedFromMerge), decodedBytes, out int consumed, out int written);

                if (successfulDecode)
                {
                    source = GetNextInputSpan();
                    if (source.IsEmpty)
                    {
                        break;  // no more input
                    }
                    decodedBytes = decodedBytes.Slice(written);
                }
                else
                {
                    if (decodedBytes.Length - written < 3)   // output buffer is too small
                    {
                        var temp = decodedBytes.Slice(written); // + GetNextOutputSpan, i.e. patch it together, not enough output
                        Span<byte> stackSpan;

                        unsafe
                        {
                            byte* stackBytes = stackalloc byte[stackLength];
                            stackSpan = new Span<byte>(stackBytes, stackLength);
                        }

                        temp.CopyTo(stackSpan);
                        decodedBytes = GetNextOutputSpan();
                        if (decodedBytes.IsEmpty)
                        {
                            break;  // no more output space available
                        }
                        // decodedBytes + temp

                        source = source.Slice(consumed);
                    }
                    else    // not enough input, or invalid input
                    {
                        if ((source.Length - consumed) >= 4)
                        {
                            break;  // input is invalid! do something
                        }

                        var temp = source.Slice(consumed);
                        Span<byte> stackSpan;

                        unsafe
                        {
                            byte* stackBytes = stackalloc byte[stackLength];
                            stackSpan = new Span<byte>(stackBytes, stackLength);
                        }

                        temp.CopyTo(stackSpan);
                        source = GetNextInputSpan();

                        if (source.IsEmpty)
                        {
                            break;  // no more input, left over is invalid
                        }

                        source.Slice(0, source.Length - temp.Length).CopyTo(stackSpan.Slice(temp.Length));
                        Base64Encoder.TryDecode(stackSpan, decodedBytes, out consumed, out written);
                        // What next?
                        decodedBytes = decodedBytes.Slice(written);
                    }
                }

            } while (!successfulDecode);

            Assert.True(expectedBuffer.SequenceEqual(outputBuffer));
        }

        const int stackLength = 32;

        void Decode(IEnumerable<Buffer<byte>> source, IOutput destination)
        {
            IEnumerator<Buffer<byte>> enumerator = source.GetEnumerator();
            int afterMergeSlice = 0;
            while(true)
            {
                Buffer<byte> sourceBuffer = enumerator.Current;
                Span<byte> outputSpan = destination.Buffer;
                Span<byte> sourceSpan = sourceBuffer.Span;
                bool result = Base64Encoder.TryDecode(sourceSpan.Slice(afterMergeSlice), outputSpan, out int consumed, out int written);
                afterMergeSlice = 0;
                destination.Advance(written);

                if (result)
                {
                    if (!enumerator.MoveNext()) break;
                    continue;
                }

                if (outputSpan.Length - written < 3)
                {
                    destination.Enlarge();  // output buffer is too small
                }
                else
                {
                    int leftOverBytes = sourceSpan.Length - consumed;
                    if (leftOverBytes >= 4)
                    {
                        if (!enumerator.MoveNext()) break;
                        continue; // source buffer contains invalid bytes
                    }

                    // left over bytes in source span
                    Span<byte> stackSpan;

                    unsafe
                    {
                        byte* stackBytes = stackalloc byte[stackLength];
                        stackSpan = new Span<byte>(stackBytes, stackLength);
                    }

                    sourceSpan.Slice(consumed).CopyTo(stackSpan);
                    
                    // Copy some bytes from next sourceBuffer into stackSpan
                    if (!enumerator.MoveNext()) break;

                    Buffer<byte> nextSourceBuffer = enumerator.Current;
                    int amountToCopy = Math.Min(nextSourceBuffer.Length, stackSpan.Length - leftOverBytes);
                    nextSourceBuffer.Slice(0, amountToCopy).CopyTo(stackSpan.Slice(leftOverBytes));

                    if (!Base64Encoder.TryDecode(stackSpan, outputSpan, out consumed, out written))
                    {
                        if (!enumerator.MoveNext()) break;
                        continue;   // invalid state?
                    }

                    afterMergeSlice = consumed - leftOverBytes;
                    continue;
                }
                if (!enumerator.MoveNext()) break;
            }
        }





        public Span<byte> GetNextInputSpan() { return default(Span<byte>); }
        public Span<byte> GetNextOutputSpan() { return default(Span<byte>); }

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
                source[sourceLength - 1] = s_encodingPad;                          // single character padding
                Assert.Equal(expected[i] - 1, Base64Encoder.ComputeDecodedLength(source));
                source[sourceLength - 2] = s_encodingPad;                          // two characters padding
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
                source[sourceLength - 1] = s_encodingPad;
                Assert.Equal(expectedOutput[i], Base64Encoder.ComputeDecodedLength(source));
                if (sourceLength > 1)
                {
                    source[sourceLength - 2] = s_encodingPad;
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

        [Fact]
        public void GenerateEncodingMapAndVerify()
        {
            var data = new byte[65]; // Base64 + pad character
            for (int i = 0; i < s_characters.Length; i++)
            {
                data[i] = (byte)s_characters[i];
            }
            Assert.True(s_encodingMap.AsSpan().SequenceEqual(data));
            Assert.Equal(s_encodingPad, s_encodingMap[64]);
        }

        [Fact]
        public void GenerateDecodingMapAndVerify()
        {
            var data = new byte[256]; // 0 to byte.MaxValue (255)
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = s_invalidByte;
            }
            for (int i = 0; i < s_characters.Length; i++)
            {
                data[s_characters[i]] = (byte)i;
            }
            Assert.True(s_decodingMap.AsSpan().SequenceEqual(data));
        }
    }
}
