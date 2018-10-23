// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using Xunit;

namespace System.Buffers.Tests
{
    public class Reader_Rewind
    {
        [Fact]
        public void Rewind()
        {
            ReadOnlySequence<byte> bytes = BufferFactory.Create(new byte[][] {
                new byte[] { 0          },
                new byte[] { 1, 2       },
                new byte[] { 3, 4       },
                new byte[] { 5, 6, 7, 8 }
            });

            BufferReader<byte> reader = new BufferReader<byte>(bytes);
            reader.Advance(1);
            BufferReader<byte> copy = reader;
            for (int i = 1; i < bytes.Length; i++)
            {
                reader.Advance(i);
                reader.Rewind(i);

                Assert.Equal(copy.Position, reader.Position);
                Assert.Equal(copy.Consumed, reader.Consumed);
                Assert.Equal(copy.CurrentSpanIndex, reader.CurrentSpanIndex);
                Assert.Equal(copy.End, reader.End);
                Assert.True(copy.CurrentSpan.SequenceEqual(reader.CurrentSpan));
            }
        }

        [Fact]
        public void Rewind_ByOne()
        {
            ReadOnlySequence<byte> bytes = BufferFactory.Create(new byte[][] {
                new byte[] { 0          },
                new byte[] { 1, 2       },
                new byte[] { 3, 4       },
                new byte[] { 5, 6, 7, 8 }
            });

            BufferReader<byte> reader = new BufferReader<byte>(bytes);
            reader.Advance(1);
            BufferReader<byte> copy = reader;
            for (int i = 1; i < bytes.Length; i++)
            {
                reader.Advance(i);
                for (int j = 0; j < i; j++)
                {
                    reader.Rewind(1);
                    Assert.False(reader.End);
                }

                Assert.Equal(copy.Position, reader.Position);
                Assert.Equal(copy.Consumed, reader.Consumed);
                Assert.Equal(copy.CurrentSpanIndex, reader.CurrentSpanIndex);
                Assert.Equal(copy.End, reader.End);
                Assert.True(copy.CurrentSpan.SequenceEqual(reader.CurrentSpan));
            }
        }
    }
}
