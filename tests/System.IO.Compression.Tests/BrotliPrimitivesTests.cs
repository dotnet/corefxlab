// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Buffers;
using Xunit;

namespace System.IO.Compression.Tests
{
    public class BrotliTests
    {
        static string brTestFile(string fileName) => Path.Combine("BrotliTestData", fileName);

        const string testFile = "BrotliTest.txt";

        [Fact]
        public void CanDisposeState()
        {
            Brotli.State state = new Brotli.State();
            state.Dispose();
        }

        [Fact]
        public void DoubleDisposeState()
        {
            Brotli.State state = new Brotli.State();
            state.Dispose();
            state.Dispose();
        }

        [Fact]
        public void SimpleSetQuality()
        {
            Brotli.State state = new Brotli.State();
            state.SetQuality(1);
            Assert.True(state.CompressMode);
        }

        [Fact]
        public void SimpleSetWindowSize()
        {
            Brotli.State state = new Brotli.State();
            state.SetWindow(11);
            Assert.True(state.CompressMode);
        }

        [Fact]
        public void DecompressSetQuality()
        {
            Brotli.State state = new Brotli.State();
            state.SetQuality(1);
            Assert.Throws<System.Exception>(delegate { Brotli.Decompress(Span<byte>.Empty, Span<byte>.Empty, out int consumed, out int written, ref state); });
        }

        [Fact]
        public void DecompressSetWindow()
        {
            Brotli.State state = new Brotli.State();
            state.SetQuality(1);
            Assert.Throws<System.Exception>(delegate { Brotli.Decompress(Span<byte>.Empty, Span<byte>.Empty, out int consumed, out int written, ref state); });
        }

        [Fact]
        public void CompressAfterDecompress()
        {
            Brotli.State state = new Brotli.State();
            Brotli.Decompress(Span<byte>.Empty, Span<byte>.Empty, out int consumed, out int written, ref state);
            Assert.Throws<System.Exception>(delegate { Brotli.Compress(Span<byte>.Empty, Span<byte>.Empty, out consumed, out written, ref state); });
        }

        [Fact]
        public void DecompressAfterCompress()
        {
            Brotli.State state = new Brotli.State();
            Brotli.Compress(Span<byte>.Empty, Span<byte>.Empty, out int consumed, out int written, ref state);
            Assert.Throws<System.Exception>(delegate { Brotli.Decompress(Span<byte>.Empty, Span<byte>.Empty, out consumed, out written, ref state); });
        }

        [Fact]
        public void DecompressAfterFlush()
        {
            Brotli.State state = new Brotli.State();
            Brotli.FlushEncoder(Span<byte>.Empty, Span<byte>.Empty, out int consumed, out int written, ref state);
            Assert.Throws<System.Exception>(delegate { Brotli.Decompress(Span<byte>.Empty, Span<byte>.Empty, out consumed, out written, ref state); });
        }

        [Fact]
        public void FlushAfterDecompress()
        {
            Brotli.State state = new Brotli.State();
            Brotli.FlushEncoder(Span<byte>.Empty, Span<byte>.Empty, out int consumed, out int written, ref state);
            Assert.Throws<System.Exception>(delegate { Brotli.Decompress(Span<byte>.Empty, Span<byte>.Empty, out consumed, out written, ref state); });
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1024, 1030)]
        [InlineData(1 << 30, 1073742086)]
        [InlineData(2147483132, int.MaxValue)]
        public void CorrectMaxSize(int totalSize, int maxCompressedSize)
        {
            Assert.Equal(Brotli.GetMaximumCompressedSize(totalSize), maxCompressedSize);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData(2147483133)]
        public void WrongMaxSize(int totalSize)
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(delegate { Brotli.GetMaximumCompressedSize(totalSize); });
        }

        [Theory]
        [InlineData(int.MaxValue)]
        [InlineData(12)]
        public void WrongQuality(uint quality)
        {
            Brotli.State state = new Brotli.State();
            Assert.Throws<System.ArgumentOutOfRangeException>(delegate { state.SetQuality(quality); });
        }

        [Theory]
        [InlineData(int.MaxValue)]
        [InlineData(0)]
        [InlineData(25)]
        [InlineData(9)]
        public void WrongWindowSize(uint windowSize)
        {
            Brotli.State state = new Brotli.State();
            Assert.Throws<System.ArgumentOutOfRangeException>(delegate { state.SetWindow(windowSize); });
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(1024)]
        public void DecompressRandomInvalidData(int totalSize)
        {
            Brotli.State state = new Brotli.State();
            byte[] data = new byte[totalSize];
            new Random(42).NextBytes(data);
            Assert.Throws<System.IO.IOException>(delegate { Brotli.Decompress(data, Array.Empty<byte>(), out int consumed, out int written, ref state);  });
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(10)]
        public void DecompressPartlyInvalidData(int correctPart)
        {
            Brotli.State state = new Brotli.State();
            byte[] data = File.ReadAllBytes(Path.Combine("BrotliTestData", testFile + ".br"));
            Span<byte> source = new Span<byte>(data, 0, data.Length / correctPart);
            int expected = (int)new FileInfo(Path.Combine("BrotliTestData", testFile)).Length;
            byte[] decompressed = new byte[expected];
            OperationStatus result = Brotli.Decompress(source, decompressed, out int consumed, out int written, ref state);
            new Random(42).NextBytes(data);
            Assert.Throws<System.IO.IOException>(delegate { Brotli.Decompress(data, Array.Empty<byte>(), out consumed, out written, ref state); } );
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(1023)]
        [InlineData(1024 * 1024)]
        public void RoundtripCompressDecompress(int totalSize)
        {
            byte[] data = new byte[totalSize];
            new Random(42).NextBytes(data);
            byte[] compressed = new byte[Brotli.GetMaximumCompressedSize(totalSize)];
            Assert.NotEqual(compressed.Length, 0);
            Brotli.State state = new Brotli.State();
            OperationStatus result = Brotli.Compress(data, compressed, out int consumed, out int written, ref state);
            while (consumed != 0 || result != OperationStatus.Done)
            {
                result = Brotli.Compress(data, compressed, out consumed, out written, ref state);
            }
            byte[] flush = new byte[0];
            result = Brotli.FlushEncoder(flush, compressed, out consumed, out written, ref state);
            Assert.Equal(OperationStatus.Done, result);
            Assert.Equal(consumed, 0);
            byte[] resultCompressed = new byte[written];
            Array.Copy(compressed, resultCompressed, written);
            ValidateCompressedData(resultCompressed, data);
        }

        private void ValidateCompressedData(byte[] data, byte[] expected)
        {
            byte[] decompressed = new byte[expected.Length];
            Brotli.State state = new Brotli.State();
            OperationStatus result = Brotli.Decompress(data, decompressed, out int consumed, out int written, ref state);
            Assert.Equal<OperationStatus>(OperationStatus.Done, result);
            Assert.Equal<long>(expected.Length, written);
            Assert.Equal<long>(consumed, 0);
            Assert.Equal<byte>(expected, decompressed);
        }

    }
}
