// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Collections.Generic;
using System.Collections.Sequences;
using System.Text;
using Xunit;

namespace System.Buffers.Tests
{
    public partial class BytesReaderTests
    {
        [Fact]
        public void SingleSegmentBasics()
        {
            ReadOnlyMemory<byte> buffer = new byte[] { 1, 2, 3, 4, 5, 6 };
            var bytes = new ReadOnlyBytes(buffer);
            var sliced  = bytes.Slice(1, 3);
            var span = sliced.First.Span;
            Assert.Equal((byte)2, span[0]);

            Assert.Equal(buffer.Length, bytes.Length);
        }

        [Fact]
        public void MultiSegmentBasics()
        {
            var bytes = Parse("A|CD|EFG");
            bytes = bytes.Slice(2, 3);
            Assert.Equal((byte)'D', bytes.First.Span[0]);

            bytes = Parse("A|CD|EFG");

            Assert.Equal(6, bytes.Length);
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
                Assert.Equal(totalLength - i, sliced.Length);
                if(i!=totalLength) Assert.Equal(i, span[0]);
            }
            Assert.Equal(0, span.Length);
        }

        [Fact]
        public void MultiSegmentCursorSlicing()
        {
            var array1 = new byte[] { 0, 1 };
            var array2 = new byte[] { 2, 3 };
            ReadOnlyBytes allBytes = ReadOnlyBytes.Create(array1, array2);

            ReadOnlyBytes allBytesSlice1 = allBytes.Slice(1);
            ReadOnlyBytes allBytesSlice2 = allBytes.Slice(2);
            ReadOnlyBytes allBytesSlice3 = allBytes.Slice(3);

            var cursorOf3 = allBytes.CursorOf(3);
            var cursorOf1 = allBytes.CursorOf(1);

            // all bytes
            {
                var slice = allBytes.Slice(cursorOf3);
                Assert.Equal(1, slice.Length);
                Assert.Equal(3, slice.First.Span[0]);
            }

            {
                var slice = allBytes.Slice(cursorOf1);
                Assert.Equal(3, slice.Length);
                Assert.Equal(1, slice.First.Span[0]);
            }

            // allBytesSlice1
            {
                var slice = allBytesSlice1.Slice(cursorOf3);
                Assert.Equal(1, slice.Length);
                Assert.Equal(3, slice.First.Span[0]);
            }

            {
                var slice = allBytesSlice1.Slice(cursorOf1);
                Assert.Equal(3, slice.Length);
                Assert.Equal(1, slice.First.Span[0]);
            }

            // allBytesSlice2
            {
                var slice = allBytesSlice2.Slice(cursorOf3);
                Assert.Equal(1, slice.Length);
                Assert.Equal(3, slice.First.Span[0]);
            }

            {
                var slice = allBytesSlice2.Slice(cursorOf1);
                Assert.Equal(3, slice.Length);
                Assert.Equal(1, slice.First.Span[0]);
            }

            // allBytesSlice3
            {
                var slice = allBytesSlice3.Slice(cursorOf3);
                Assert.Equal(1, slice.Length);
                Assert.Equal(3, slice.First.Span[0]);
            }

            {
                var slice = allBytesSlice3.Slice(cursorOf1);
                Assert.Equal(3, slice.Length);
                Assert.Equal(1, slice.First.Span[0]);
            }
        }

        [Fact]
        public void SingleSegmentCursorSlicing()
        {
            var array1 = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            ReadOnlyBytes allBytes = new ReadOnlyBytes(array1);

            ReadOnlyBytes allBytesSlice = allBytes.Slice(1);

            var cursorOf3 = allBytes.CursorOf(3);
            var cursorOf1 = allBytes.CursorOf(1);

            // all bytes
            {
                var slice = allBytes.Slice(cursorOf3);
                Assert.Equal(6, slice.Length);
                Assert.Equal(3, slice.First.Span[0]);
            }

            {
                var slice = allBytes.Slice(cursorOf1);
                Assert.Equal(8, slice.Length);
                Assert.Equal(1, slice.First.Span[0]);
            }

            // allBytesSlice1
            {
                var slice = allBytesSlice.Slice(cursorOf3);
                Assert.Equal(6, slice.Length);
                Assert.Equal(3, slice.First.Span[0]);
            }

            {
                var slice = allBytesSlice.Slice(cursorOf1);
                Assert.Equal(8, slice.Length);
                Assert.Equal(1, slice.First.Span[0]);
            }
        }

        [Fact]
        public void PavelsScenarioCursorSlicing()
        {
            // single segment
            {
                var rob = ReadOnlyBytes.Create(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 });
                var c = rob.CursorOf(5);

                var slice1 = rob.Slice(2).Slice(c);
                var slice2 = rob.Slice(c).Slice(2);

                Assert.NotEqual(slice1.First.Span[0], slice2.First.Span[0]);
                Assert.Equal(5, slice1.First.Span[0]);
                Assert.Equal(7, slice2.First.Span[0]);
            }

            // multi segment
            {
                var rob = ReadOnlyBytes.Create(new byte[] { 1, 2, 3 }, new byte[] { 4, 5, 6, 7, 8 });
                var c = rob.CursorOf(5);

                var slice1 = rob.Slice(2).Slice(c);
                var slice2 = rob.Slice(c).Slice(2);

                Assert.NotEqual(slice1.First.Span[0], slice2.First.Span[0]);
                Assert.Equal(5, slice1.First.Span[0]);
                Assert.Equal(7, slice2.First.Span[0]);
            }
        }

        [Fact]
        public void LengthCursorSlicing()
        {
            // single segment
            {
                var rob = new ReadOnlyBytes(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 });
                var c2 = rob.CursorOf(2);
                var c5 = rob.CursorOf(5);

                var slice = rob.Slice(c2, c5);

                Assert.Equal(2, slice.First.Span[0]);
                Assert.Equal(4, slice.Length);
            }

            // multi segment
            {
                var rob = ReadOnlyBytes.Create(new byte[] { 1, 2, 3 }, new byte[] { 4, 5, 6, 7, 8 });
                var c2 = rob.CursorOf(2);
                var c5 = rob.CursorOf(5);

                var slice = rob.Slice(c2, c5);

                Assert.Equal(2, slice.First.Span[0]);
                Assert.Equal(3, slice.Length);
            }
        }

        [Fact]
        public void SingleSegmentCopyToKnownLength()
        {
            var array = new byte[] { 0, 1, 2, 3, 4, 5, 6 };
            ReadOnlyMemory<byte> buffer = array;
            var bytes = new ReadOnlyBytes(buffer);

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
        public void EmptyReadOnlyBytesEnumeration()
        {
            var bytes = ReadOnlyBytes.Empty;
            var position = new Position();
            ReadOnlyMemory<byte> segment;
            Assert.False(bytes.TryGet(ref position, out segment));
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
