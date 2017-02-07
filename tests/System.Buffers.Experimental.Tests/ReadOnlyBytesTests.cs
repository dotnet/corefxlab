// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Buffers;
using System.Collections.Generic;
using System.Collections.Sequences;
using System.Text;
using Xunit;

namespace System.Slices.Tests
{
    public partial class ReadOnlyBytesTests
    {
        [Fact(Skip = "flaky")]
        public void ReadOnlyBytesBasics()
        {
            ReadOnlyMemory<byte> buffer = new byte[] { 1, 2, 3, 4, 5, 6 };
            var bytes = new ReadOnlyBytes(buffer);
            bytes = bytes.Slice(1, 3);
            Assert.Equal(2, bytes.First.Span[0]);
        }

        [Fact(Skip = "flaky")]
        public void ReadOnlyBytesIndexOf()
        {
            var bytes = ReadOnlyBytes.Create(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, new byte[] { 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 });
            Assert.Equal(10, bytes.First.Length);
            Assert.Equal(9, bytes.First.Span[9]);
            Assert.NotEqual(null, bytes.Rest);

            var index = bytes.IndexOf(new byte[] { 2, 3 });
            Assert.Equal(2, index);

            index = bytes.IndexOf(new byte[] { 8, 9, 10 });
            Assert.Equal(8, index);

            index = bytes.IndexOf(new byte[] { 11, 12, 13, 14 });
            Assert.Equal(11, index);
        }

        [Fact(Skip = "flaky")]
        public void ReadOnlyBytesEnumeration()
        {
            var buffer = new byte[] { 1, 2, 3, 4, 5, 6 };
            var bytes = new ReadOnlyBytes(buffer);
            var position = new Position();
            int length = 0;
            ReadOnlyMemory<byte> segment;
            while (bytes.TryGet(ref position, out segment))
            {
                length += segment.Length;
            }
            Assert.Equal(buffer.Length, length);

            var multibytes = Parse("A|CD|EFG");
            position = new Position();
            length = 0;
            while (multibytes.TryGet(ref position, out segment))
            {
                length += segment.Length;
            }
            Assert.Equal(6, length);
        }

        [Fact(Skip = "flaky")]
        public void ReadOnlyTailBytesEnumeration()
        {
            for (int i = 0; i < 6; i++)
            {
                var multibytes = Parse("A|CD|EFG");
                multibytes = multibytes.Slice(i);
                var position = new Position();
                var length = 0;
                ReadOnlyMemory<byte> segment;
                while (multibytes.TryGet(ref position, out segment))
                {
                    length += segment.Length;
                }
                Assert.Equal(6 - i, length);
            }
        }

        [Fact(Skip = "flaky")]
        public void ReadOnlyFrontBytesEnumeration()
        {
            for (int i = 0; i < 7; i++)
            {
                var multibytes = Parse("A|CD|EFG");
                multibytes = multibytes.Slice(0, i);
                var position = new Position();
                var length = 0;
                ReadOnlyMemory<byte> segment;
                while (multibytes.TryGet(ref position, out segment))
                {
                    length += segment.Length;
                }
                Assert.Equal(i, length);
            }
        }

        [Fact(Skip = "flaky")]
        public void SegmentedReadOnlyBytesBasics()
        {
            var bytes = Parse("A|CD|EFG");
            bytes = bytes.Slice(2, 3);
            Assert.Equal((byte)'D', bytes.First.Span[0]);
        }

        private ReadOnlyBytes Create(params string[] segments)
        {
            var buffers = new List<byte[]>();
            foreach (var segment in segments)
            {
                buffers.Add(Encoding.UTF8.GetBytes(segment));
            }
            return ReadOnlyBytes.Create(buffers.ToArray());
        }

        private ReadOnlyBytes Parse(string text)
        {
            var segments = text.Split('|');
            var buffers = new List<byte[]>();
            foreach (var segment in segments)
            {
                buffers.Add(Encoding.UTF8.GetBytes(segment));
            }
            return ReadOnlyBytes.Create(buffers.ToArray());
        }
    }
}