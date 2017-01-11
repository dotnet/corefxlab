// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Buffers;
using System.Collections.Generic;
using System.Collections.Sequences;
using System.Text;
using Xunit;

namespace System.Slices.Tests
{
    public class ReadOnlyBytesTests
    {
        [Fact]
        public void ReadOnlyBytesBasics()
        {
            var buffer = new byte[] { 1, 2, 3, 4, 5, 6 };
            var bytes = new ReadOnlyBytes(buffer);
            bytes = bytes.Slice(1, 3);
            Assert.Equal(2, bytes.First.Span[0]);
        }

        [Fact]
        public void ReadOnlyBytesIndexOf()
        {
            var bytes = Create(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, new byte[] { 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 });

            var index = bytes.IndexOf(new byte[] { 2, 3 });
            Assert.Equal(2, index);

            index = bytes.IndexOf(new byte[] { 8, 9, 10 });
            Assert.Equal(8, index);

            index = bytes.IndexOf(new byte[] { 11, 12, 13, 14 });
            Assert.Equal(11, index);
        }

        [Fact]
        public void ReadOnlyBytesEnumeration()
        {
            var buffer = new byte[] { 1, 2, 3, 4, 5, 6 };
            var bytes = new ReadOnlyBytes(buffer);
            var position = new Position();
            int length = 0;
            ReadOnlyMemory<byte> segment;
            while (bytes.TryGet(ref position, out segment, true))
            {
                length += segment.Length;
            }
            Assert.Equal(buffer.Length, length);

            var multibytes = Parse("A|CD|EFG");
            position = new Position();
            length = 0;
            while (multibytes.TryGet(ref position, out segment, true))
            {
                length += segment.Length;
            }
            Assert.Equal(6, length);
        }

        [Fact]
        public void ReadOnlyTailBytesEnumeration()
        {
            for (int i = 0; i < 6; i++)
            {
                var multibytes = Parse("A|CD|EFG");
                multibytes = multibytes.Slice(i);
                var position = new Position();
                var length = 0;
                ReadOnlyMemory<byte> segment;
                while (multibytes.TryGet(ref position, out segment, true))
                {
                    length += segment.Length;
                }
                Assert.Equal(6 - i, length);
            }
        }

        [Fact]
        public void ReadOnlyFrontBytesEnumeration()
        {
            for (int i = 0; i < 7; i++)
            {
                var multibytes = Parse("A|CD|EFG");
                multibytes = multibytes.Slice(0, i);
                var position = new Position();
                var length = 0;
                ReadOnlyMemory<byte> segment;
                while (multibytes.TryGet(ref position, out segment, true))
                {
                    length += segment.Length;
                }
                Assert.Equal(i, length);
            }
        }

        [Fact]
        public void SegmentedReadOnlyBytesBasics()
        {
            var bytes = Parse("A|CD|EFG");
            bytes = bytes.Slice(2, 3);
            Assert.Equal((byte)'D', bytes.First.Span[0]);
        }

        class MemoryListNode : IReadOnlyMemoryList<byte>
        {
            internal ReadOnlyMemory<byte> _first;
            internal MemoryListNode _rest;
            public ReadOnlyMemory<byte> First => _first;

            public int? Length
            {
                get {
                    throw new NotImplementedException();
                }
            }

            public IReadOnlyMemoryList<byte> Rest => _rest;

            public int CopyTo(Span<byte> buffer)
            {
                int copied = 0;
                var position = Position.First;
                ReadOnlyMemory<byte> segment;
                var free = buffer;
                while (TryGet(ref position, out segment, true))
                {
                    if (segment.Length > free.Length)
                    {
                        segment.Span.Slice(0, free.Length).CopyTo(free);
                        copied += free.Length;
                    }
                    else
                    {
                        segment.CopyTo(free);
                        copied += segment.Length;
                    }
                    free = buffer.Slice(copied);
                    if (free.Length == 0) break;
                }
                return copied;
            }

            public bool TryGet(ref Position position, out ReadOnlyMemory<byte> item, bool advance = false)
            {
                if (position == Position.First)
                {
                    item = _first;
                    if (advance) { position.IntegerPosition++; position.ObjectPosition = _rest; }
                    return true;
                }
                else if (position.ObjectPosition == null) { item = default(ReadOnlyMemory<byte>); return false; }

                var sequence = (MemoryListNode)position.ObjectPosition;
                item = sequence._first;
                if (advance)
                {
                    if (position == Position.First)
                    {
                        position.ObjectPosition = _rest;
                    }
                    else
                    {
                        position.ObjectPosition = sequence._rest;
                    }
                    position.IntegerPosition++;
                }
                return true;
            }
        }

        private ReadOnlyBytes Create(params byte[][] buffers)
        {
            MemoryListNode first = null;
            MemoryListNode current = null;
            foreach (var buffer in buffers)
            {
                if (first == null)
                {
                    current = new MemoryListNode();
                    first = current;
                }
                else
                {
                    current._rest = new MemoryListNode();
                    current = current._rest;
                }
                current._first = buffer;
            }
            return new ReadOnlyBytes(first);
        }

        private ReadOnlyBytes Parse(string text)
        {
            var segments = text.Split('|');
            var buffers = new List<byte[]>();
            foreach (var segment in segments)
            {
                buffers.Add(Encoding.UTF8.GetBytes(segment));
            }
            return Create(buffers.ToArray());
        }
    }
}