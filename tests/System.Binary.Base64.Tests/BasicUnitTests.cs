// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Xunit;
using System.Buffers;
using System.Buffers.Text;

namespace System.Binary.Base64Experimental.Tests
{
    public class Base64Tests
    {
        [Fact]
        public void BasicEncodingWithLineBreaksMime()
        {
            var format = new StandardFormat('M');
            for (int numBytes = 58; numBytes < 1000; numBytes++)
            {
                Span<byte> source = new byte[numBytes];
                Base64TestHelper.InitalizeBytes(source, numBytes);

                char[] charArray = new char[numBytes * 10];
                Span<byte> encodedBytes = new byte[Base64.GetMaxEncodedToUtf8Length(source.Length)];
                Span<byte> encodedBytesWithLineBreaks = new byte[Base64Experimental.GetMaxEncodedToUtf8Length(source.Length, format)];
                Assert.Equal(OperationStatus.Done, Base64.EncodeToUtf8(source, encodedBytes, out int consumed, out int encodedBytesCount));
                Assert.Equal(encodedBytes.Length, encodedBytesCount);
                Assert.True(OperationStatus.Done == Base64Experimental.EncodeToUtf8(source, encodedBytesWithLineBreaks, out consumed, out encodedBytesCount, format), "At index: " + numBytes);
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
            var format = new StandardFormat('N', 64);
            for (int numBytes = 0; numBytes < 1000; numBytes++)
            {
                Span<byte> source = new byte[numBytes];
                Base64TestHelper.InitalizeBytes(source, numBytes);

                char[] charArray = new char[numBytes * 10];
                Span<byte> encodedBytes = new byte[Base64.GetMaxEncodedToUtf8Length(source.Length)];
                Span<byte> encodedBytesWithLineBreaks = new byte[Base64Experimental.GetMaxEncodedToUtf8Length(source.Length, format)];
                Assert.True(OperationStatus.Done == Base64Experimental.EncodeToUtf8(source, encodedBytesWithLineBreaks, out int consumed, out int encodedBytesCount, format), "At index: " + numBytes);
                Assert.Equal(encodedBytesWithLineBreaks.Length, encodedBytesCount);

                string encodedTextWithLineBreaks = Text.Encoding.ASCII.GetString(encodedBytesWithLineBreaks.ToArray());
                Assert.Equal(encodedTextWithLineBreaks, encodedTextWithLineBreaks);
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

            var (first, last) = BufferList.Create(input);

            var output = new TestBufferWriter();
            Base64Experimental.Utf8ToBytesDecoder.Pipe(new ReadOnlySequence<byte>(first, 0, last, last.Memory.Length), output);

            var expectedArray = expected.ToArray();
            var array = output.GetBuffer.ToArray();

            Assert.True(expected.SequenceEqual(output.GetBuffer.Slice(0, requiredLength)));
        }
    }

    class TestBufferWriter : IBufferWriter<byte>
    {
        byte[] _buffer = new byte[1000];

        int _written = 0;

        public Span<byte> GetBuffer => _buffer;

        public void Advance(int bytes)
        {
            _written += bytes;
        }

        public Memory<byte> GetMemory(int minimumLength = 0)
        {
            return ((Memory<byte>)_buffer).Slice(_written);
        }

        public Span<byte> GetSpan(int minimumLength)
        {
            return ((Span<byte>)_buffer).Slice(_written);
        }

        public int MaxBufferSize { get; } = Int32.MaxValue;
    }
}
