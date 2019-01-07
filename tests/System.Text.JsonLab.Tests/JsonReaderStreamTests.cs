// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Tests;
using System.IO;
using Xunit;

namespace System.Text.JsonLab.Tests
{
    public class JsonReaderStreamTests
    {
        [Theory]
        [InlineData(10_000)]
        [InlineData(100_000)]
        [InlineData(1_000_000)]
        [InlineData(10_000_000)]
        // [InlineData(1_000_000_000)] // Allocating 1 GB for a test is too high for inner loop (reserved for outerloop)
        public void StreamMaxTokenSize(int tokenSize)
        {
            byte[] dataUtf8 = new byte[tokenSize];
            System.Array.Fill<byte>(dataUtf8, 97);

            dataUtf8[0] = 34;
            dataUtf8[dataUtf8.Length - 1] = 34;

            var stream = new MemoryStream(dataUtf8);
            var json = new Utf8JsonReaderStream(stream);
            while (json.Read()) ;
            Assert.Equal(dataUtf8.Length, json.Consumed);
            json.Dispose();
        }

        [Fact(Skip = "Allocating 1.5 GB for a test is too high for inner loop (reserved for outerloop).")]
        public void StreamTokenSizeOverflow()
        {
            int tokenSize = 1_500_000_000; // 1.5GB
            byte[] dataUtf8 = new byte[tokenSize];
            System.Array.Fill<byte>(dataUtf8, 97);

            dataUtf8[0] = 34;
            dataUtf8[dataUtf8.Length - 1] = 34;

            var stream = new MemoryStream(dataUtf8);
            var json = new Utf8JsonReaderStream(stream);
            try
            {
                while (json.Read()) ;
                Assert.True(false, "Expected ArgumentException to be thrown since token size larger than 1 GB is not supported.");
            }
            catch (ArgumentException)
            {
                Assert.Equal(0, json.Consumed);
            }
            json.Dispose();
        }

        [Theory]
        [InlineData(250)]   // 1 MB
        //[InlineData(250_000)]    // 1 GB, allocating 1 GB for a test is too high for inner loop (reserved for outerloop)
        //[InlineData(2_500_000)] // 10 GB, takes too long to run (~7 minutes)
        public void MultiSegmentSequenceLarge(int numberOfSegments)
        {
            const int segmentSize = 4_000;
            byte[][] buffers = new byte[numberOfSegments][];

            for (int j = 0; j < numberOfSegments; j++)
            {
                byte[] arr = new byte[segmentSize];

                for (int i = 0; i < segmentSize - 7; i += 7)
                {
                    arr[i] = (byte)'"';
                    arr[i + 1] = (byte)'a';
                    arr[i + 2] = (byte)'a';
                    arr[i + 3] = (byte)'a';
                    arr[i + 4] = (byte)'"';
                    arr[i + 5] = (byte)',';
                    arr[i + 6] = (byte)' ';
                }
                arr[3_997] = (byte)' ';
                arr[3_998] = (byte)' ';
                arr[3_999] = (byte)' ';

                buffers[j] = arr;
            }

            buffers[0][0] = (byte)'[';
            buffers[0][1] = (byte)' ';
            buffers[0][2] = (byte)' ';
            buffers[0][3] = (byte)' ';
            buffers[0][4] = (byte)' ';
            buffers[0][5] = (byte)' ';

            buffers[numberOfSegments - 1][segmentSize - 5] = (byte)']';
            buffers[numberOfSegments - 1][segmentSize - 4] = (byte)' ';
            buffers[numberOfSegments - 1][segmentSize - 3] = (byte)' ';
            buffers[numberOfSegments - 1][segmentSize - 2] = (byte)' ';
            buffers[numberOfSegments - 1][segmentSize - 1] = (byte)' ';

            ReadOnlySequence<byte> sequenceMultiple = BufferFactory.Create(buffers);
            var json = new JsonUtf8Reader(sequenceMultiple);
            while (json.Read()) ;
            Assert.Equal(sequenceMultiple.Length, json.BytesConsumed);

            var stream = new MemoryStream(sequenceMultiple.ToArray());
            var jsonStream = new Utf8JsonReaderStream(stream);
            while (jsonStream.Read()) ;
            Assert.Equal(sequenceMultiple.Length, jsonStream.Consumed);
            jsonStream.Dispose();
        }
    }
}
