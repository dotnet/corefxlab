// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Xunit;

namespace System.IO.Compression.Tests
{
    public class BrotliStreamTests
    {
        static string brTestFile(string fileName) => Path.Combine("BrotliTestData", fileName);

        [Fact(Skip = "Fails in VS - System.BadImageFormatException : An attempt was made to load a program with an incorrect format.")]
        public void GetSetReadWriteTimeout()
        {
            int sec = 10;
            var writeStream = new MemoryStream();
            var brotliCompressStream = new BrotliStream(writeStream, CompressionMode.Compress);
            brotliCompressStream.ReadTimeout = sec;
            brotliCompressStream.WriteTimeout = sec;
            Assert.Equal(brotliCompressStream.ReadTimeout, sec);
            Assert.Equal(brotliCompressStream.WriteTimeout, sec);
        }

        [Fact(Skip = "Fails in VS - System.BadImageFormatException : An attempt was made to load a program with an incorrect format.")]
        public void BaseStreamCompress()
        {
            var writeStream = new MemoryStream();
            var brotliCompressStream = new BrotliStream(writeStream, CompressionMode.Compress);
            writeStream.Dispose();
        }

        [Fact(Skip = "Fails in VS - System.BadImageFormatException : An attempt was made to load a program with an incorrect format.")]
        public void BaseStreamDecompress()
        {
            var writeStream = new MemoryStream();
            var brotliCompressStream = new BrotliStream(writeStream, CompressionMode.Decompress);
            writeStream.Dispose();
        }

        [Fact(Skip = "Fails in VS - System.BadImageFormatException : An attempt was made to load a program with an incorrect format.")]
        public void DecompressCanRead()
        {
            var memoryInputStream = new MemoryStream();
            var brotliCompressStream = new BrotliStream(memoryInputStream, CompressionMode.Decompress);
            Assert.True(brotliCompressStream.CanRead);
            brotliCompressStream.Dispose();
            Assert.False(brotliCompressStream.CanRead);
        }

        [Fact(Skip = "Fails in VS - System.BadImageFormatException : An attempt was made to load a program with an incorrect format.")]
        public void CompressCanWrite()
        {
            var memoryInputStream = new MemoryStream();
            var brotliCompressStream = new BrotliStream(memoryInputStream, CompressionMode.Compress);
            Assert.True(brotliCompressStream.CanWrite);

            brotliCompressStream.Dispose();
            Assert.False(brotliCompressStream.CanWrite);
        }

        [Fact(Skip = "Fails in VS - System.BadImageFormatException : An attempt was made to load a program with an incorrect format.")]
        public void CanDisposeBaseStream()
        {
            var memoryInputStream = new MemoryStream();
            var brotliCompressStream = new BrotliStream(memoryInputStream, CompressionMode.Compress);
            memoryInputStream.Dispose(); // This would throw if this was invalid
        }

        [Fact(Skip = "Fails in VS - System.BadImageFormatException : An attempt was made to load a program with an incorrect format.")]
        public void CanDisposeBrotliStream()
        {
            var memoryInputStream = new MemoryStream();
            using (var brotliCompressStream = new BrotliStream(memoryInputStream, CompressionMode.Compress))
            {
                brotliCompressStream.Dispose();
                Assert.Throws<ObjectDisposedException>( delegate { brotliCompressStream.CopyTo(memoryInputStream); }); 
                brotliCompressStream.Dispose(); // Should be a no-op
            }
            memoryInputStream = new MemoryStream();
            using (var brotliCompressStream = new BrotliStream(memoryInputStream, CompressionMode.Decompress))
            {
                brotliCompressStream.Dispose();
                Assert.Throws<ObjectDisposedException>(delegate { brotliCompressStream.CopyTo(memoryInputStream); });   // Base Stream should be null after dispose
                brotliCompressStream.Dispose(); // Should be a no-op
            }
        }

        [Fact(Skip = "Fails in VS - System.BadImageFormatException : An attempt was made to load a program with an incorrect format.")]
        public void Flush()
        {
            var memoryInputStream = new MemoryStream();
            var brotliCompressStream = new BrotliStream(memoryInputStream, CompressionMode.Compress);
            brotliCompressStream.Flush();
        }

        [Fact(Skip = "Fails in VS - System.BadImageFormatException : An attempt was made to load a program with an incorrect format.")]
        public void DoubleFlush()
        {
            var memoryInputStream = new MemoryStream();
            var brotliCompressStream = new BrotliStream(memoryInputStream, CompressionMode.Compress);
            brotliCompressStream.Flush();
            brotliCompressStream.Flush();
        }

        [Fact(Skip = "Fails in VS - System.BadImageFormatException : An attempt was made to load a program with an incorrect format.")]
        public void DoubleDispose()
        {
            var memoryInputStream = new MemoryStream();
            using (var brotliCompressStream = new BrotliStream(memoryInputStream, CompressionMode.Compress))
            {
                brotliCompressStream.Dispose();
                brotliCompressStream.Dispose();
            }
            using (var brotliCompressStream = new BrotliStream(memoryInputStream, CompressionMode.Decompress))
            {
                brotliCompressStream.Dispose();
                brotliCompressStream.Dispose();
            }
        }

        [Fact(Skip = "Fails in VS - System.BadImageFormatException : An attempt was made to load a program with an incorrect format.")]
        public void FlushThenDispose()
        {
            var memoryInputStream = new MemoryStream();
            using (var brotliCompressStream = new BrotliStream(memoryInputStream, CompressionMode.Compress))
            {
                brotliCompressStream.Flush();
                brotliCompressStream.Dispose();
            }
            using (var brotliCompressStream = new BrotliStream(memoryInputStream, CompressionMode.Decompress))
            {
                brotliCompressStream.Flush();
                brotliCompressStream.Dispose();
            }
        }

        [Fact(Skip = "Fails in VS - System.BadImageFormatException : An attempt was made to load a program with an incorrect format.")]
        public void TestSeekMethobrotliCompressStreamDecompress()
        {
            var memoryInputStream = new MemoryStream();
            var brotliCompressStream = new BrotliStream(memoryInputStream, CompressionMode.Decompress);
            Assert.False(brotliCompressStream.CanSeek, "CanSeek should be false");
            Assert.Throws<NotSupportedException>(delegate { long value = brotliCompressStream.Length; });
            Assert.Throws<NotSupportedException>(delegate { long value = brotliCompressStream.Position; });
            Assert.Throws<NotSupportedException>(delegate { brotliCompressStream.Position = 100L; });
            Assert.Throws<NotSupportedException>(delegate { brotliCompressStream.SetLength(100L); });
            Assert.Throws<NotSupportedException>(delegate { brotliCompressStream.Seek(100L, SeekOrigin.Begin); });
        }

        [Fact(Skip = "Fails in VS - System.BadImageFormatException : An attempt was made to load a program with an incorrect format.")]
        public void TestSeekMethobrotliCompressStreamCompress()
        {
            var memoryInputStream = new MemoryStream();
            var brotliCompressStream = new BrotliStream(memoryInputStream, CompressionMode.Compress);
            Assert.False(brotliCompressStream.CanSeek, "CanSeek should be false");
            Assert.Throws<NotSupportedException>(delegate { long value = brotliCompressStream.Length; });
            Assert.Throws<NotSupportedException>(delegate { long value = brotliCompressStream.Position; });
            Assert.Throws<NotSupportedException>(delegate { brotliCompressStream.Position = 100L; });
            Assert.Throws<NotSupportedException>(delegate { brotliCompressStream.SetLength(100L); });
            Assert.Throws<NotSupportedException>(delegate { brotliCompressStream.Seek(100L, SeekOrigin.Begin); });
        }

        [Fact(Skip = "Fails in VS - System.BadImageFormatException : An attempt was made to load a program with an incorrect format.")]
        public void CanReadBaseStreamAfterDispose()
        {
            var memoryInputStream = new MemoryStream(File.ReadAllBytes(brTestFile("BrotliTest.txt.br")));
            var brotliCompressStream = new BrotliStream(memoryInputStream, CompressionMode.Decompress, true);
            var baseStream = memoryInputStream;
            brotliCompressStream.Dispose();
            int size = 1024;
            byte[] bytes = new byte[size];
            baseStream.Read(bytes, 0, size); // This will throw if the underlying stream is not writable as expected
        }

        [Fact(Skip = "Fails in VS - System.BadImageFormatException : An attempt was made to load a program with an incorrect format.")]
        public void ReadWriteArgumentValidation()
        {
            using (var brotliCompressStream = new BrotliStream(new MemoryStream(), CompressionMode.Compress))
            {
                Assert.Throws<ArgumentNullException>(() => brotliCompressStream.Write(null, 0, 0));
                Assert.Throws<ArgumentOutOfRangeException>(() => brotliCompressStream.Write(new byte[1], -1, 0));
                Assert.Throws<ArgumentOutOfRangeException>(() => brotliCompressStream.Write(new byte[1], 0, -1));
                Assert.Throws<ArgumentOutOfRangeException>(() => brotliCompressStream.Write(new byte[1], 0, 2));
                Assert.Throws<ArgumentOutOfRangeException>(() => brotliCompressStream.Write(new byte[1], 1, 1));
                Assert.Throws<InvalidOperationException>(() => brotliCompressStream.Read(new byte[1], 0, 1));
                brotliCompressStream.Write(new byte[1], 0, 0);
            }
            using (var brotliCompressStream = new BrotliStream(new MemoryStream(), CompressionMode.Decompress))
            {
                Assert.Throws<ArgumentNullException>(() => brotliCompressStream.Read(null, 0, 0));
                Assert.Throws<ArgumentOutOfRangeException>(() => brotliCompressStream.Read(new byte[1], -1, 0));
                Assert.Throws<ArgumentOutOfRangeException>(() => brotliCompressStream.Read(new byte[1], 0, -1));
                Assert.Throws<ArgumentOutOfRangeException>(() => brotliCompressStream.Read(new byte[1], 0, 2));
                Assert.Throws<ArgumentOutOfRangeException>(() => brotliCompressStream.Read(new byte[1], 1, 1));
                Assert.Throws<InvalidOperationException>(() => brotliCompressStream.Write(new byte[1], 0, 1));
                var data = new byte[1] { 42 };
                Assert.Equal(0, brotliCompressStream.Read(data, 0, 0));
                Assert.Equal(42, data[0]);
            }
        }

        [Theory(Skip = "Fails in VS - System.BadImageFormatException : An attempt was made to load a program with an incorrect format.")]
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
                compressor.Dispose();
            }
            compressed.Position = 0;
            ValidateCompressedData(chunkSize, compressed.ToArray(), data);
            compressed.Dispose();
        }

        private void ValidateCompressedData(int chunkSize, byte[] compressedData, byte[] expected)
        {
            MemoryStream compressed = new MemoryStream(compressedData);
            using (MemoryStream decompressed = new MemoryStream())
            using (var decompressor = new BrotliStream(compressed, CompressionMode.Decompress, true))
            {
                decompressor.CopyTo(decompressed, expected.Length);
                Assert.Equal<byte>(expected, decompressed.ToArray());
            }
        }
    }
}
