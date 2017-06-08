// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.IO.Compression;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace System.IO.Compression.Tests
{
    public class BrotliStreamTests
    {
        static string brTestFile(string fileName) => Path.Combine("BrotliTestData", fileName);

        [Fact]
        public void BaseStreamCompress()
        {
            var writeStream = new MemoryStream();
            var bro = new BrotliStream(writeStream, CompressionMode.Compress);
            Assert.Same(bro._stream, writeStream);
            writeStream.Dispose();
        }

        [Fact]
        public void BaseStreamDecompress()
        {
            var writeStream = new MemoryStream();
            var bro = new BrotliStream(writeStream, CompressionMode.Decompress);
            Assert.Same(bro._stream, writeStream);
            writeStream.Dispose();
        }

        [Fact]
        public void DecompressCanRead()
        {
            var ms = new MemoryStream();
            var bro = new BrotliStream(ms, CompressionMode.Decompress);

            Assert.True(bro.CanRead);

            bro.Dispose();
            Assert.False(bro.CanRead);
        }

        [Fact]
        public void CompressCanWrite()
        {
            var ms = new MemoryStream();
            var bro = new BrotliStream(ms, CompressionMode.Compress);
            Assert.True(bro.CanWrite);

            bro.Dispose();
            Assert.False(bro.CanWrite);
        }

        [Fact]
        public void CanDisposeBaseStream()
        {
            var ms = new MemoryStream();
            var bro = new BrotliStream(ms, CompressionMode.Compress);
            ms.Dispose(); // This would throw if this was invalid
        }

        [Fact]
        public void CanDisposeBrotliStream()
        {
            var ms = new MemoryStream();
            using (var bro = new BrotliStream(ms, CompressionMode.Compress))
            {
                bro.Dispose();
                Assert.Null(bro._stream);  // Base Stream should be null after dispose
                bro.Dispose(); // Should be a no-op
            }
            using (var bro = new BrotliStream(ms, CompressionMode.Decompress))
            {
                bro.Dispose();
                Assert.Null(bro._stream);  // Base Stream should be null after dispose
                bro.Dispose(); // Should be a no-op
            }
        }

        [Fact]
        public void Flush()
        {
            var ms = new MemoryStream();
            var ds = new BrotliStream(ms, CompressionMode.Compress);
            ds.Flush();
        }

        [Fact]
        public void DoubleFlush()
        {
            var ms = new MemoryStream();
            var ds = new BrotliStream(ms, CompressionMode.Compress);
            ds.Flush();
            ds.Flush();
        }

        [Fact]
        public void DoubleDispose()
        {
            var ms = new MemoryStream();
            using (var ds = new BrotliStream(ms, CompressionMode.Compress))
            {
                ds.Dispose();
                ds.Dispose();
            }
            using (var ds = new BrotliStream(ms, CompressionMode.Decompress))
            {
                ds.Dispose();
                ds.Dispose();
            }
        }

        [Fact]
        public void FlushThenDispose()
        {
            var ms = new MemoryStream();
            using (var ds = new BrotliStream(ms, CompressionMode.Compress))
            {
                ds.Flush();
                ds.Dispose();
            }
            using (var ds = new BrotliStream(ms, CompressionMode.Decompress))
            {
                ds.Flush();
                ds.Dispose();
            }
        }

        [Fact]
        public void TestSeekMethodsDecompress()
        {
            var ms = new MemoryStream();
            var bro = new BrotliStream(ms, CompressionMode.Decompress);
            Assert.False(bro.CanSeek, "CanSeek should be false");
            Assert.Throws<NotSupportedException>(delegate { long value = bro.Length; });
            Assert.Throws<NotSupportedException>(delegate { long value = bro.Position; });
            Assert.Throws<NotSupportedException>(delegate { bro.Position = 100L; });
            Assert.Throws<NotSupportedException>(delegate { bro.SetLength(100L); });
            Assert.Throws<NotSupportedException>(delegate { bro.Seek(100L, SeekOrigin.Begin); });
        }

        [Fact]
        public void TestSeekMethodsCompress()
        {
            var ms = new MemoryStream();
            var bro = new BrotliStream(ms, CompressionMode.Compress);
            Assert.False(bro.CanSeek, "CanSeek should be false");
            Assert.Throws<NotSupportedException>(delegate { long value = bro.Length; });
            Assert.Throws<NotSupportedException>(delegate { long value = bro.Position; });
            Assert.Throws<NotSupportedException>(delegate { bro.Position = 100L; });
            Assert.Throws<NotSupportedException>(delegate { bro.SetLength(100L); });
            Assert.Throws<NotSupportedException>(delegate { bro.Seek(100L, SeekOrigin.Begin); });
        }

        [Fact]
        public void CanReadBaseStreamAfterDispose()
        {
            var ms = new MemoryStream(File.ReadAllBytes(brTestFile("BrotliTest.txt.br")));
            var bro = new BrotliStream(ms, CompressionMode.Decompress, true);
            var baseStream = bro._stream;
            bro.Dispose();
            int size = 1024;
            byte[] bytes = new byte[size];
            baseStream.Read(bytes, 0, size); // This will throw if the underlying stream is not writable as expected
        }

        [Fact]
        public void ReadWriteArgumentValidation()
        {
            using (var ds = new BrotliStream(new MemoryStream(), CompressionMode.Compress))
            {
                Assert.Throws<ArgumentNullException>(() => ds.Write(null, 0, 0));
                Assert.Throws<ArgumentOutOfRangeException>(() => ds.Write(new byte[1], -1, 0));
                Assert.Throws<ArgumentOutOfRangeException>(() => ds.Write(new byte[1], 0, -1));
                Assert.Throws<ArgumentOutOfRangeException>(() => ds.Write(new byte[1], 0, 2));
                Assert.Throws<ArgumentOutOfRangeException>(() => ds.Write(new byte[1], 1, 1));
                Assert.Throws<InvalidOperationException>(() => ds.Read(new byte[1], 0, 1));
                ds.Write(new byte[1], 0, 0);
            }
            using (var ds = new BrotliStream(new MemoryStream(), CompressionMode.Decompress))
            {
                Assert.Throws<ArgumentNullException>(() => ds.Read(null, 0, 0));
                Assert.Throws<ArgumentOutOfRangeException>(() => ds.Read(new byte[1], -1, 0));
                Assert.Throws<ArgumentOutOfRangeException>(() => ds.Read(new byte[1], 0, -1));
                Assert.Throws<ArgumentOutOfRangeException>(() => ds.Read(new byte[1], 0, 2));
                Assert.Throws<ArgumentOutOfRangeException>(() => ds.Read(new byte[1], 1, 1));
                Assert.Throws<InvalidOperationException>(() => ds.Write(new byte[1], 0, 1));
                var data = new byte[1] { 42 };
                Assert.Equal(0, ds.Read(data, 0, 0));
                Assert.Equal(42, data[0]);
            }
        }

        [Theory]
        [InlineData(1, 5)]
        [InlineData(1023, 1023 * 10)]
        [InlineData(1024 * 1024, 1024 * 1024)]
        public void RoundtripCompressDecompress(int chunkSize, int totalSize)
        {
            byte[] data = new byte[totalSize];
            new Random(42).NextBytes(data);
            var compressed = new MemoryStream();
            using (var compressor = new BrotliStream(compressed, CompressionMode.Compress, true))
            {
                for (int i = 0; i < data.Length; i += chunkSize)
                {
                    compressor.Write(data, i, chunkSize);
                }
            }
            compressed.Position = 0;
            ValidateCompressedData(chunkSize, compressed, data);
            compressed.Dispose();
        }

        private void ValidateCompressedData(int chunkSize, MemoryStream compressed, byte[] expected)
        {
            using (MemoryStream decompressed = new MemoryStream())
            using (var decompressor = new BrotliStream(compressed, CompressionMode.Decompress, true))
            {
                decompressor.CopyTo(decompressed, chunkSize);

                Assert.Equal<byte>(expected, decompressed.ToArray());
            }
        }
    }
}
