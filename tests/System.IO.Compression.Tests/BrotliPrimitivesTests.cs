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

        [Fact(Skip = "Fails in VS - System.BadImageFormatException : An attempt was made to load a program with an incorrect format.")]
        public void CanDisposeState()
        {
            Brotli.State state = new Brotli.State();
            state.Dispose();
        }

        [Fact(Skip = "Fails in VS - System.BadImageFormatException : An attempt was made to load a program with an incorrect format.")]
        public void DoubleDisposeState()
        {
            Brotli.State state = new Brotli.State();
            state.Dispose();
            state.Dispose();
        }

        [Fact(Skip = "Fails in VS - System.BadImageFormatException : An attempt was made to load a program with an incorrect format.")]
        public void SimpleSetQuality()
        {
            Brotli.State state = new Brotli.State();
            state.SetQuality();
            Assert.True(state.CompressMode);
        }

        [Fact(Skip = "Fails in VS - System.BadImageFormatException : An attempt was made to load a program with an incorrect format.")]
        public void SimpleSetWindowSize()
        {
            Brotli.State state = new Brotli.State();
            state.SetWindow();
            Assert.True(state.CompressMode);
        }

        [Fact(Skip = "Fails in VS - System.BadImageFormatException : An attempt was made to load a program with an incorrect format.")]
        public void DecompressSetQuality()
        {
            Brotli.State state = new Brotli.State();
            state.SetQuality();
            Assert.Throws<System.Exception>(delegate { Brotli.Decompress(Span<byte>.Empty, Span<byte>.Empty, out int consumed, out int written, ref state); });
        }

        [Fact(Skip = "Fails in VS - System.BadImageFormatException : An attempt was made to load a program with an incorrect format.")]
        public void DecompressSetWindow()
        {
            Brotli.State state = new Brotli.State();
            state.SetQuality();
            Assert.Throws<System.Exception>(delegate { Brotli.Decompress(Span<byte>.Empty, Span<byte>.Empty, out int consumed, out int written, ref state); });
        }

        [Fact(Skip = "Fails in VS - System.BadImageFormatException : An attempt was made to load a program with an incorrect format.")]
        public void CompressAfterDecompress()
        {
            Brotli.State state = new Brotli.State();
            Brotli.Decompress(Span<byte>.Empty, Span<byte>.Empty, out int consumed, out int written, ref state);
            Assert.Throws<System.Exception>(delegate { Brotli.Compress(Span<byte>.Empty, Span<byte>.Empty, out consumed, out written, ref state); });
        }

        [Fact(Skip = "Fails in VS - System.BadImageFormatException : An attempt was made to load a program with an incorrect format.")]
        public void DecompressAfterCompress()
        {
            Brotli.State state = new Brotli.State();
            Brotli.Compress(Span<byte>.Empty, Span<byte>.Empty, out int consumed, out int written, ref state);
            Assert.Throws<System.Exception>(delegate { Brotli.Decompress(Span<byte>.Empty, Span<byte>.Empty, out consumed, out written, ref state); });
        }

        [Fact(Skip = "Fails in VS - System.BadImageFormatException : An attempt was made to load a program with an incorrect format.")]
        public void DecompressAfterFlush()
        {
            Brotli.State state = new Brotli.State();
            Brotli.FlushEncoder(Span<byte>.Empty, Span<byte>.Empty, out int consumed, out int written, ref state);
            Assert.Throws<System.Exception>(delegate { Brotli.Decompress(Span<byte>.Empty, Span<byte>.Empty, out consumed, out written, ref state); });
        }

        [Fact(Skip = "Fails in VS - System.BadImageFormatException : An attempt was made to load a program with an incorrect format.")]
        public void FlushAfterDecompress()
        {
            Brotli.State state = new Brotli.State();
            Brotli.FlushEncoder(Span<byte>.Empty, Span<byte>.Empty, out int consumed, out int written, ref state);
            Assert.Throws<System.Exception>(delegate { Brotli.Decompress(Span<byte>.Empty, Span<byte>.Empty, out consumed, out written, ref state); });
        }

        [Theory(Skip = "Fails in VS - System.BadImageFormatException : An attempt was made to load a program with an incorrect format.")]
        [InlineData(0, 1)]
        [InlineData(1024, 1030)]
        [InlineData(1 << 30, 1073742086)]
        [InlineData(2147483132, int.MaxValue)]
        public void CorrectMaxSize(int totalSize, int maxCompressedSize)
        {
            Assert.Equal(Brotli.GetMaximumCompressedSize(totalSize), maxCompressedSize);
        }

        [Theory(Skip = "Fails in VS - System.BadImageFormatException : An attempt was made to load a program with an incorrect format.")]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData(2147483133)]
        public void WrongMaxSize(int totalSize)
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(delegate { Brotli.GetMaximumCompressedSize(totalSize); });
        }

        [Theory(Skip = "Fails in VS - System.BadImageFormatException : An attempt was made to load a program with an incorrect format.")]
        [InlineData(int.MaxValue)]
        [InlineData(12)]
        public void WrongQuality(uint quality)
        {
            Brotli.State state = new Brotli.State();
            Assert.Throws<System.ArgumentOutOfRangeException>(delegate { state.SetQuality(quality); });
        }

        [Theory(Skip = "Fails in VS - System.BadImageFormatException : An attempt was made to load a program with an incorrect format.")]
        [InlineData(int.MaxValue)]
        [InlineData(0)]
        [InlineData(25)]
        [InlineData(9)]
        public void WrongWindowSize(uint windowSize)
        {
            Brotli.State state = new Brotli.State();
            Assert.Throws<System.ArgumentOutOfRangeException>(delegate { state.SetWindow(windowSize); });
        }

        [Theory(Skip = "Fails in VS - System.BadImageFormatException : An attempt was made to load a program with an incorrect format.")]
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
            TransformationStatus result = Brotli.Compress(data, compressed, out int consumed, out int written, ref state);
            while (consumed != 0 || result != TransformationStatus.Done)
            {
                result = Brotli.Compress(data, compressed, out consumed, out written, ref state);
            }
            byte[] flush = new byte[0];
            result = Brotli.FlushEncoder(flush, compressed, out consumed, out written, ref state);
            Assert.Equal(TransformationStatus.Done, result);
            Assert.Equal(consumed, 0);
            byte[] resultCompressed = new byte[written];
            Array.Copy(compressed, resultCompressed, written);
            ValidateCompressedData(resultCompressed, data);
        }

        private void ValidateCompressedData(byte[] data, byte[] expected)
        {
            byte[] decompressed = new byte[expected.Length];
            Brotli.State state = new Brotli.State();
            TransformationStatus result = Brotli.Decompress(data, decompressed, out int consumed, out int written, ref state);
            Assert.Equal<TransformationStatus>(TransformationStatus.Done, result);
            Assert.Equal<long>(expected.Length, written);
            Assert.Equal<long>(consumed, 0);
            Assert.Equal<byte>(expected, decompressed);
        }

    }
}
