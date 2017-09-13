// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Collections.Generic;
using System.Collections.Sequences;
using System.Text;
using Xunit;

namespace System.Buffers.Tests
{
    public partial class ReadOnlyBytesTests
    {
        [Fact]
        public void SingleSegmentBasics()
        {
            ReadOnlyMemory<byte> buffer = new byte[] { 1, 2, 3, 4, 5, 6 };
            var bytes = new ReadOnlyBytes(buffer);
            var sliced  = bytes.Slice(1, 3);
            var span = sliced.First.Span;
            Assert.Equal((byte)2, span[0]);

            Assert.Equal(buffer.Length, bytes.Length.Value);
            Assert.Equal(buffer.Length, bytes.ComputeLength());

            bytes = new ReadOnlyBytes(buffer, null, -1);
            Assert.False(bytes.Length.HasValue);
            Assert.Equal(buffer.Length, bytes.ComputeLength());
            Assert.Equal(buffer.Length, bytes.Length.Value);
        }

        [Fact]
        public void MultiSegmentBasics()
        {
            var bytes = Parse("A|CD|EFG");
            bytes = bytes.Slice(2, 3);
            Assert.Equal((byte)'D', bytes.First.Span[0]);

            bytes = Parse("A|CD|EFG");

            Assert.False(bytes.Length.HasValue);
            Assert.Equal(6, bytes.ComputeLength());
            Assert.Equal(6, bytes.Length.Value);
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
            var totalLength = array1.Length + array2.Length;
            ReadOnlyBytes bytes = ReadOnlyBytes.Create(array1, array2);

            ReadOnlyBytes sliced = bytes;
            ReadOnlySpan<byte> span;
            for(int i=1; i<=totalLength; i++) {
                sliced  = bytes.Slice(i);
                span = sliced.First.Span;
                Assert.Equal(totalLength - i, sliced.ComputeLength());
                if(i!=totalLength) Assert.Equal(i, span[0]);
            }
            Assert.Equal(0, span.Length);
        }

        [Fact]
        public void SingleSegmentCopyToKnownLength()
        {
            var array = new byte[] { 0, 1, 2, 3, 4, 5, 6 };
            ReadOnlyMemory<byte> buffer = array;
            var bytes = new ReadOnlyBytes(buffer, null, array.Length);

            { // copy to equal
                var copy = new byte[array.Length];
                var copied = bytes.CopyTo(copy);
                Assert.Equal(array.Length, copied);
                Assert.Equal(array, copy);
            }

            { // copy to smaller
                var copy = new byte[array.Length - 1];
                var copied = bytes.CopyTo(copy);
                Assert.Equal(copy.Length, copied);
                for(int i=0; i<copied; i++) {
                    Assert.Equal(array[i], copy[i]);
                }
            }

            { // copy to larger
                var copy = new byte[array.Length + 1];
                var copied = bytes.CopyTo(copy);
                Assert.Equal(array.Length, copied);
                for (int i = 0; i < copied; i++) {
                    Assert.Equal(array[i], copy[i]);
                }
            }
        }

        [Fact]
        public void SingleSegmentCopyToUnknownLength()
        {
            var array = new byte[] { 0, 1, 2, 3, 4, 5, 6 };
            ReadOnlyMemory<byte> buffer = array;
            var bytes = new ReadOnlyBytes(buffer, null, -1);

            { // copy to equal
                var copy = new byte[array.Length];
                var copied = bytes.CopyTo(copy);
                Assert.Equal(array.Length, copied);
                Assert.Equal(array, copy);
            }

            { // copy to smaller
                var copy = new byte[array.Length - 1];
                var copied = bytes.CopyTo(copy);
                Assert.Equal(copy.Length, copied);
                for (int i = 0; i < copied; i++) {
                    Assert.Equal(array[i], copy[i]);
                }
            }

            { // copy to larger
                var copy = new byte[array.Length + 1];
                var copied = bytes.CopyTo(copy);
                Assert.Equal(array.Length, copied);
                for (int i = 0; i < copied; i++) {
                    Assert.Equal(array[i], copy[i]);
                }
            }
        }

        [Fact]
        public void MultiSegmentCopyToUnknownLength()
        {
            var array1 = new byte[] { 0, 1 };
            var array2 = new byte[] { 2, 3 };
            var totalLength = array1.Length + array2.Length;
            ReadOnlyBytes bytes = ReadOnlyBytes.Create(array1, array2);

            { // copy to equal
                var copy = new byte[totalLength];
                var copied = bytes.CopyTo(copy);
                Assert.Equal(totalLength, copied);
                for (int i = 0; i < totalLength; i++) {
                    Assert.Equal(i, copy[i]);
                }
            }

            { // copy to smaller
                var copy = new byte[totalLength - 1];
                var copied = bytes.CopyTo(copy);
                Assert.Equal(copy.Length, copied);
                for (int i = 0; i < copied; i++) {
                    Assert.Equal(i, copy[i]);
                }
            }

            { // copy to larger
                var copy = new byte[totalLength + 1];
                var copied = bytes.CopyTo(copy);
                Assert.Equal(totalLength, copied);
                for (int i = 0; i < totalLength; i++) {
                    Assert.Equal(i, copy[i]);
                }
            }
        }

        [Fact]
        public void MultiSegmentIndexOfSpan()
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

            index = bytes.IndexOf(new byte[] { 19 });
            Assert.Equal(19, index);

            index = bytes.IndexOf(new byte[] { 0 });
            Assert.Equal(0, index);

            index = bytes.IndexOf(new byte[] { 9 });
            Assert.Equal(9, index);

            index = bytes.IndexOf(new byte[] { 10 });
            Assert.Equal(10, index);
        }

        [Fact]
        public void MultiSegmentIndexOfByte()
        {
            var bytes = ReadOnlyBytes.Create(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, new byte[] { 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 });
            Assert.Equal(10, bytes.First.Length);
            Assert.Equal(9, bytes.First.Span[9]);
            Assert.NotEqual(null, bytes.Rest);

            for(int i=0; i<20; i++){
                var index = bytes.IndexOf((byte)i);
                Assert.Equal(i, index);
            }
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
