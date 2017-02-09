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
        [Fact]
        public void ReadOnlyBytesBasics()
        {
            ReadOnlyMemory<byte> buffer = new byte[] { 1, 2, 3, 4, 5, 6 };
            var bytes = new ReadOnlyBytes(buffer);
            var sliced  = bytes.Slice(1, 3);
            var span = sliced.First.Span;
            Assert.Equal((byte)2, span[0]);
        }

        [Fact]
        public void SingleSegmentSlicing()
        {
            var array = new byte[] { 0, 1, 2, 3, 4, 5, 6 };
            ReadOnlyMemory<byte> buffer = array;
            var bytes = new ReadOnlyBytes(buffer);

            ReadOnlyBytes sliced = bytes;
            ReadOnlySpan<byte> span;
            for(int i=1; i<=array.Length; i++) {
                sliced  = bytes.Slice(i);
                span = sliced.First.Span;
                Assert.Equal(array.Length - i, span.Length);
                if(i!=array.Length) Assert.Equal(i, span[0]);
            }
            Assert.Equal(0, span.Length);
        }

        [Fact]
        public void MultiSegmentSlicing()
        {
            var array1 = new byte[] { 0, 1 };
            var array2 = new byte[] { 2, 3 };
            var totalLenght = array1.Length + array2.Length;
            ReadOnlyBytes bytes = ReadOnlyBytes.Create(array1, array2);

            ReadOnlyBytes sliced = bytes;
            ReadOnlySpan<byte> span;
            for(int i=1; i<=totalLenght; i++) {
                sliced  = bytes.Slice(i);
                span = sliced.First.Span;
                Assert.Equal(totalLenght - i, sliced.ComputeLength());
                if(i!=totalLenght) Assert.Equal(i, span[0]);
            }
            Assert.Equal(0, span.Length);
        }


        [Fact]
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

        [Fact]
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
                while (multibytes.TryGet(ref position, out segment))
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
                while (multibytes.TryGet(ref position, out segment))
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