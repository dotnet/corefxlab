// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Pipelines.Testing;
using System.Numerics;
using Xunit;
using System.Collections.Sequences;

namespace System.IO.Pipelines.Tests
{
    public abstract class SeekTests
    {
        public class Array: SingleSegment
        {
            public Array() : base(ReadOnlyBufferFactory.Array) { }
            internal Array(ReadOnlyBufferFactory factory) : base(factory) { }
        }

        public class OwnedMemory: SingleSegment
        {
            public OwnedMemory() : base(ReadOnlyBufferFactory.OwnedMemory) { }
        }

        public class SingleSegment: SegmentPerByte
        {
            public SingleSegment() : base(ReadOnlyBufferFactory.SingleSegment) { }
            internal SingleSegment(ReadOnlyBufferFactory factory) : base(factory) { }
        }

        public class SegmentPerByte: SeekTests
        {
            public SegmentPerByte() : base(ReadOnlyBufferFactory.SegmentPerByte) { }
            internal SegmentPerByte(ReadOnlyBufferFactory factory) : base(factory) { }
        }

        internal ReadOnlyBufferFactory Factory { get; }

        internal SeekTests(ReadOnlyBufferFactory factory)
        {
            Factory = factory;
        }

        [Theory]
        [InlineData("a", "a", 'a', 0)]
        [InlineData("ab", "a", 'a', 0)]
        [InlineData("aab", "a", 'a', 0)]
        [InlineData("acab", "a", 'a', 0)]
        [InlineData("acab", "c", 'c', 1)]
        [InlineData("abcdefghijklmnopqrstuvwxyz", "lo", 'l', 11)]
        [InlineData("abcdefghijklmnopqrstuvwxyz", "ol", 'l', 11)]
        [InlineData("abcdefghijklmnopqrstuvwxyz", "ll", 'l', 11)]
        [InlineData("abcdefghijklmnopqrstuvwxyz", "lmr", 'l', 11)]
        [InlineData("abcdefghijklmnopqrstuvwxyz", "rml", 'l', 11)]
        [InlineData("abcdefghijklmnopqrstuvwxyz", "mlr", 'l', 11)]
        [InlineData("abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz", "lmr", 'l', 11)]
        [InlineData("aaaaaaaaaaalmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz", "lmr", 'l', 11)]
        [InlineData("aaaaaaaaaaacmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz", "lmr", 'm', 12)]
        [InlineData("aaaaaaaaaaarmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz", "lmr", 'r', 11)]
        [InlineData("/localhost:5000/PATH/%2FPATH2/ HTTP/1.1", " %?", '%', 21)]
        [InlineData("/localhost:5000/PATH/%2FPATH2/?key=value HTTP/1.1", " %?", '%', 21)]
        [InlineData("/localhost:5000/PATH/PATH2/?key=value HTTP/1.1", " %?", '?', 27)]
        [InlineData("/localhost:5000/PATH/PATH2/ HTTP/1.1", " %?", ' ', 27)]
        public void MemorySeek(string raw, string search, char expectResult, int expectIndex)
        {
            var cursors = Factory.CreateWithContent(raw);
            Position result = default;

            var searchFor = search.ToCharArray();

            int found = -1;
            if (searchFor.Length == 1)
            {
                found = cursors.Seek(out result, (byte)searchFor[0]);
            }
            else if (searchFor.Length == 2)
            {
                found = cursors.Seek(out result, (byte)searchFor[0], (byte)searchFor[1]);
            }
            else if (searchFor.Length == 3)
            {
                found = cursors.Seek(out result, (byte)searchFor[0], (byte)searchFor[1], (byte)searchFor[2]);
            }
            else
            {
                Assert.False(true, "Invalid test sample.");
            }

            Assert.Equal(expectResult, found);
            Assert.Equal(cursors.Slice(result).ToArray(), Encoding.ASCII.GetBytes(raw.Substring(expectIndex)));
        }

        [Theory]
        [MemberData(nameof(SeekByteLimitData))]
        public void TestSeekByteLimitWithinSameBlock(string input, char seek, int limit, int expectedBytesScanned, int expectedReturnValue)
        {
            // Arrange
            var buffer = Factory.CreateWithContent(input);

            // Act
            buffer = limit > input.Length ? buffer : buffer.Slice(0, limit);

            var returnValue = buffer.Seek(out Position result, (byte)seek);
            var returnValue_1 = buffer.Seek(out result, (byte)seek, (byte)seek);
            var returnValue_2 = buffer.Seek(out result, (byte)seek, (byte)seek, (byte)seek);

            // Assert
            Assert.Equal(expectedReturnValue, returnValue);
            Assert.Equal(expectedReturnValue, returnValue_1);
            Assert.Equal(expectedReturnValue, returnValue_2);

            if (expectedReturnValue != -1)
            {
                Assert.Equal(buffer.Slice(result).ToArray(), Encoding.ASCII.GetBytes(input.Substring(expectedBytesScanned - 1)));
            }
        }

        [Theory]
        [MemberData(nameof(SeekIteratorLimitData))]
        public void TestSeekIteratorLimitWithinSameBlock(string input, char seek, char limitAfter, int expectedReturnValue)
        {

            // Arrange
            var afterSeek = (byte)'B';

            var buffer = Factory.CreateWithContent(input);

            var start = buffer.Start;
            var scan1 = buffer.Start;
            var veryEnd = buffer.End;
            var scan2_1 = scan1;
            var scan2_2 = scan1;
            var scan3_1 = scan1;
            var scan3_2 = scan1;
            var scan3_3 = scan1;
            var end = buffer.End;

            // Act
            var endReturnValue = buffer.Seek(out end, (byte)limitAfter);
            if (endReturnValue != -1)
            {
                buffer = buffer.Slice(end, 1);
            }

            var returnValue1 = buffer.Seek(out scan1, (byte)seek);
            var returnValue2_1 = buffer.Seek(out scan2_1, (byte)seek, afterSeek);
            var returnValue2_2 = buffer.Seek(out scan2_2, afterSeek, (byte)seek);
            var returnValue3_1 = buffer.Seek(out scan3_1, (byte)seek, afterSeek, afterSeek);
            var returnValue3_2 = buffer.Seek(out scan3_2, afterSeek, (byte)seek, afterSeek);
            var returnValue3_3 = buffer.Seek(out scan3_3, afterSeek, afterSeek, (byte)seek);


            // Assert
            Assert.Equal(input.Contains(limitAfter) ? limitAfter : -1, endReturnValue);
            Assert.Equal(expectedReturnValue, returnValue1);
            Assert.Equal(expectedReturnValue, returnValue2_1);
            Assert.Equal(expectedReturnValue, returnValue2_2);
            Assert.Equal(expectedReturnValue, returnValue3_1);
            Assert.Equal(expectedReturnValue, returnValue3_2);
            Assert.Equal(expectedReturnValue, returnValue3_3);

            if (expectedReturnValue != -1)
            {
                var expectedEndIndex = input.IndexOf(seek);

                Assert.Equal(buffer.Slice(scan1).ToArray(), Encoding.ASCII.GetBytes(input.Substring(expectedEndIndex)));
                Assert.Equal(buffer.Slice(scan2_1).ToArray(), Encoding.ASCII.GetBytes(input.Substring(expectedEndIndex)));
                Assert.Equal(buffer.Slice(scan2_2).ToArray(), Encoding.ASCII.GetBytes(input.Substring(expectedEndIndex)));
                Assert.Equal(buffer.Slice(scan3_1).ToArray(), Encoding.ASCII.GetBytes(input.Substring(expectedEndIndex)));
                Assert.Equal(buffer.Slice(scan3_2).ToArray(), Encoding.ASCII.GetBytes(input.Substring(expectedEndIndex)));
                Assert.Equal(buffer.Slice(scan3_3).ToArray(), Encoding.ASCII.GetBytes(input.Substring(expectedEndIndex)));
            }
        }

        public static IEnumerable<object[]> SeekByteLimitData
        {
            get
            {
                var vectorSpan = Vector<byte>.Count;
                // string input, char seek, int limit, int expectedBytesScanned, int expectedReturnValue
                var data = new List<object[]>();

                // Non-vector inputs

                data.Add(new object[] { "hello, world", 'h', 12, 1, 'h' });
                data.Add(new object[] { "hello, world", ' ', 12, 7, ' ' });
                data.Add(new object[] { "hello, world", 'd', 12, 12, 'd' });
                data.Add(new object[] { "hello, world", '!', 12, 12, -1 });
                data.Add(new object[] { "hello, world", 'h', 13, 1, 'h' });
                data.Add(new object[] { "hello, world", ' ', 13, 7, ' ' });
                data.Add(new object[] { "hello, world", 'd', 13, 12, 'd' });
                data.Add(new object[] { "hello, world", '!', 13, 12, -1 });
                data.Add(new object[] { "hello, world", 'h', 5, 1, 'h' });
                data.Add(new object[] { "hello, world", 'o', 5, 5, 'o' });
                data.Add(new object[] { "hello, world", ',', 5, 5, -1 });
                data.Add(new object[] { "hello, world", 'd', 5, 5, -1 });
                data.Add(new object[] { "abba", 'a', 4, 1, 'a' });
                data.Add(new object[] { "abba", 'b', 4, 2, 'b' });

                // Vector inputs

                // Single vector, no seek char in input, expect failure
                data.Add(new object[] { new string('a', vectorSpan), 'b', vectorSpan, vectorSpan, -1 });
                // Two vectors, no seek char in input, expect failure
                data.Add(new object[] { new string('a', vectorSpan * 2), 'b', vectorSpan * 2, vectorSpan * 2, -1 });
                // Two vectors plus non vector length (thus hitting slow path too), no seek char in input, expect failure
                data.Add(new object[] { new string('a', vectorSpan * 2 + vectorSpan / 2), 'b', vectorSpan * 2 + vectorSpan / 2, vectorSpan * 2 + vectorSpan / 2, -1 });

                // For each input length from 1/2 to 3 1/2 vector spans in 1/2 vector span increments...
                for (var length = vectorSpan / 2; length <= vectorSpan * 3 + vectorSpan / 2; length += vectorSpan / 2)
                {
                    // ...place the seek char at vector and input boundaries...
                    for (var i = Math.Min(vectorSpan - 1, length - 1); i < length; i += ((i + 1) % vectorSpan == 0) ? 1 : Math.Min(i + (vectorSpan - 1), length - 1))
                    {
                        var input = new StringBuilder(new string('a', length));
                        input[i] = 'b';

                        // ...and check with a seek byte limit before, at, and past the seek char position...
                        for (var limitOffset = -1; limitOffset <= 1; limitOffset++)
                        {
                            var limit = (i + 1) + limitOffset;

                            if (limit >= i + 1)
                            {
                                // ...that Seek() succeeds when the seek char is within that limit...
                                data.Add(new object[] { input.ToString(), 'b', limit, i + 1, 'b' });
                            }
                            else
                            {
                                // ...and fails when it's not.
                                data.Add(new object[] { input.ToString(), 'b', limit, Math.Min(length, limit), -1 });
                            }
                        }
                    }
                }

                return data;
            }
        }

        public static IEnumerable<object[]> SeekIteratorLimitData
        {
            get
            {
                var vectorSpan = Vector<byte>.Count;

                // string input, char seek, char limitAt, int expectedReturnValue
                var data = new List<object[]>();

                // Non-vector inputs

                data.Add(new object[] { "hello, world", 'h', 'd', 'h' });
                data.Add(new object[] { "hello, world", ' ', 'd', ' ' });
                data.Add(new object[] { "hello, world", 'd', 'd', 'd' });
                data.Add(new object[] { "hello, world", '!', 'd', -1 });
                data.Add(new object[] { "hello, world", 'h', 'w', 'h' });
                data.Add(new object[] { "hello, world", 'o', 'w', 'o' });
                data.Add(new object[] { "hello, world", 'r', 'w', -1 });
                data.Add(new object[] { "hello, world", 'd', 'w', -1 });

                // Vector inputs

                // Single vector, no seek char in input, expect failure
                data.Add(new object[] { new string('a', vectorSpan), 'b', 'b', -1 });
                // Two vectors, no seek char in input, expect failure
                data.Add(new object[] { new string('a', vectorSpan * 2), 'b', 'b', -1 });
                // Two vectors plus non vector length (thus hitting slow path too), no seek char in input, expect failure
                data.Add(new object[] { new string('a', vectorSpan * 2 + vectorSpan / 2), 'b', 'b', -1 });

                // For each input length from 1/2 to 3 1/2 vector spans in 1/2 vector span increments...
                for (var length = vectorSpan / 2; length <= vectorSpan * 3 + vectorSpan / 2; length += vectorSpan / 2)
                {
                    // ...place the seek char at vector and input boundaries...
                    for (var i = Math.Min(vectorSpan - 1, length - 1); i < length; i += ((i + 1) % vectorSpan == 0) ? 1 : Math.Min(i + (vectorSpan - 1), length - 1))
                    {
                        var input = new StringBuilder(new string('a', length));
                        input[i] = 'b';

                        // ...along with sentinel characters to seek the limit iterator to...
                        input[i - 1] = 'A';
                        if (i < length - 1) input[i + 1] = 'B';

                        // ...and check that Seek() succeeds with a limit iterator at or past the seek char position...
                        data.Add(new object[] { input.ToString(), 'b', 'b', 'b' });
                        if (i < length - 1) data.Add(new object[] { input.ToString(), 'b', 'B', 'b' });

                        // ...and fails with a limit iterator before the seek char position.
                        data.Add(new object[] { input.ToString(), 'b', 'A', -1 });
                    }
                }

                return data;
            }
        }
    }
}
