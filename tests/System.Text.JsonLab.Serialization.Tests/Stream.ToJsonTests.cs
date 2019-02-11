// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace System.Text.Json.Serialization.Tests
{
    public partial class StreamTests
    {
        [Fact]
        public static async Task RoundTripAsync()
        {
            byte[] buffer;

            using (TestStream stream = new TestStream(1))
            {
                await ToJsonAsync(stream);

                // Make a copy
                buffer = stream.ToArray();
            }

            using (TestStream stream = new TestStream(buffer))
            {
                await FromJsonAsync(stream);
            }
        }

        private static async Task ToJsonAsync(TestStream stream)
        {
            JsonConverterSettings settings = new JsonConverterSettings
            {
                // Will likely default to 4K due to buffer pooling.
                DefaultBufferSize = 1
            };

            {
                LargeDataTestClass obj = new LargeDataTestClass();
                obj.Initialize();
                obj.Verify();

                await JsonConverter.ToJsonAsync(stream, obj, settings);
            }

            // Must be changed if the test classes change:
            Assert.Equal(551368, stream.TestWriteBytesCount);

            // We should have more than one write called due to the large byte count.
            Assert.True(stream.TestWriteCount > 0);

            // We don't auto-flush.
            Assert.True(stream.TestFlushCount == 0);
        }

        private static async Task FromJsonAsync(TestStream stream)
        {
            JsonConverterSettings settings = new JsonConverterSettings
            {
                // Will likely default to 4K due to buffer pooling.
                DefaultBufferSize = 1
            };

            LargeDataTestClass obj = await JsonConverter.FromJsonAsync<LargeDataTestClass>(stream, settings);
            // Must be changed if the test classes change; may be larger since last read may not have filled buffer.
            Assert.True(stream.TestRequestedReadBytesCount >= 551368);

            // We should have more than one read called due to the large byte count.
            Assert.True(stream.TestReadCount > 0);

            // We don't auto-flush.
            Assert.True(stream.TestFlushCount == 0);

            obj.Verify();
        }

        [Fact]
        public static async Task ToJsonPrimitivesAsync()
        {
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(@"1"));
            JsonConverterSettings settings = new JsonConverterSettings
            {
                DefaultBufferSize = 1
            };

            int i = await stream.FromJsonAsync<int>(settings);
            Assert.Equal(1, i);
        }
    }

    public class TestStream : MemoryStream
    {
        public int TestFlushCount { get; private set; }

        public int TestWriteCount { get; private set; }
        public int TestWriteBytesCount { get; private set; }

        public int TestReadCount { get; private set; }
        public int TestRequestedReadBytesCount { get; private set; }

        public TestStream(int capacity) : base(capacity) { }

        public TestStream(byte[] buffer) : base(buffer) { }

        public override Task FlushAsync(CancellationToken cancellationToken)
        {
            TestFlushCount++;
            return base.FlushAsync(cancellationToken);
        }

        public override ValueTask WriteAsync(ReadOnlyMemory<byte> source, CancellationToken cancellationToken)
        {
            TestWriteCount++;
            TestWriteBytesCount += source.Length;
            return base.WriteAsync(source, cancellationToken);
        }

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            TestReadCount++;
            TestRequestedReadBytesCount += count;

            return base.ReadAsync(buffer, offset, count, cancellationToken);
        }
    }
}
