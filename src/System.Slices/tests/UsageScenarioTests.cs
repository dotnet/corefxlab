// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Xunit;
using Xunit.Abstractions;

namespace System.Slices.Tests
{
    public class UsageScenarioTests
    {
        private readonly ITestOutputHelper output;

        public UsageScenarioTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        private struct MyByte
        {
            public MyByte(byte value)
            {
                Value = value;
            }

            public byte Value { get; private set; }
        }

        [Theory]
        [InlineData(new byte[] { })]
        [InlineData(new byte[] { 0 })]
        [InlineData(new byte[] { 0, 1 })]
        [InlineData(new byte[] { 0, 1, 2 })]
        [InlineData(new byte[] { 0, 1, 2, 3 })]
        [InlineData(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 })]
        [CLSCompliant(false)] // TODO: find out why the assembly-level CLSCompliant=false doesn't work
        public void CtorSpanOverByteArrayValidCasesWithPropertiesAndBasicOperationsChecks(byte[] array)
        {
            Span<byte> span = new Span<byte>(array);
            Assert.Equal(array.Length, span.Length);

            Assert.NotSame(array, span.CreateArray());
            Assert.True(span.ReferenceEquals(span));
            Assert.True(span.Equals(span));
            Assert.True(span.Equals((object)span));
            Assert.Equal(span.GetHashCode(), span.GetHashCode());
            Assert.False(span.Equals(array));

            Span<byte>.Enumerator it = span.GetEnumerator();
            for (int i = 0; i < span.Length; i++)
            {
                Assert.True(it.MoveNext());
                Assert.Equal(array[i], it.Current);
                Assert.Equal(array[i], span.Slice(i).Read<byte>());
                Assert.Equal(array[i], span.Slice(i).Read<MyByte>().Value);

                array[i] = unchecked((byte)(array[i] + 1));
                Assert.Equal(array[i], it.Current);
                Assert.Equal(array[i], span.Slice(i).Read<byte>());
                Assert.Equal(array[i], span.Slice(i).Read<MyByte>().Value);

                span.Slice(i).Write<byte>(unchecked((byte)(array[i] + 1)));
                Assert.Equal(array[i], it.Current);
                Assert.Equal(array[i], span.Slice(i).Read<byte>());
                Assert.Equal(array[i], span.Slice(i).Read<MyByte>().Value);

                span.Slice(i).Write<MyByte>(unchecked(new MyByte((byte)(array[i] + 1))));
                Assert.Equal(array[i], it.Current);
                Assert.Equal(array[i], span.Slice(i).Read<byte>());
                Assert.Equal(array[i], span.Slice(i).Read<MyByte>().Value);
            }
            Assert.False(it.MoveNext());

            it.Reset();
            for (int i = 0; i < span.Length; i++)
            {
                Assert.True(it.MoveNext());
                Assert.Equal(array[i], it.Current);
            }
            Assert.False(it.MoveNext());

            {
                Span<byte> sameSpan = new Span<byte>(array);
                Assert.Equal(span.GetHashCode(), sameSpan.GetHashCode());
                Assert.True(span.ReferenceEquals(sameSpan));
                Assert.True(span.Equals(sameSpan));
                Assert.True(span.Equals((object)sameSpan));
            }

            {
                Span<byte> structCopy = span;
                Assert.Equal(span.GetHashCode(), structCopy.GetHashCode());
                Assert.True(span.ReferenceEquals(structCopy));
                Assert.True(span.Equals(structCopy));
                Assert.True(span.Equals((object)structCopy));
            }

            {
                byte[] differentArray = new byte[array.Length * 2];
                for (int i = 0; i < array.Length; i++)
                {
                    differentArray[i] = unchecked((byte)(array[i] + 1));
                    differentArray[array.Length + i] = array[i];
                }
                {
                    Span<byte> equivalentSpan = new Span<byte>(differentArray, array.Length, array.Length);
                    Assert.Equal(span.GetHashCode(), equivalentSpan.GetHashCode());
                    Assert.False(span.ReferenceEquals(equivalentSpan));
                    Assert.True(span.Equals(equivalentSpan));
                    Assert.True(span.Equals((object)equivalentSpan));

                    if (equivalentSpan.Length > 0)
                    {
                        Span<byte> similarSpan = equivalentSpan.Slice(0, equivalentSpan.Length - 1);
                        Assert.False(span.ReferenceEquals(similarSpan));
                        Assert.False(span.Equals(similarSpan));
                        Assert.False(span.Equals((object)similarSpan));
                    }
                }

                {
                    Span<byte> differentSpan = new Span<byte>(differentArray, 0, array.Length);
                    Assert.False(span.ReferenceEquals(differentSpan));
                    // This can be simplified although it is harder to understand after simplification
                    if (array.Length == 0)
                    {
                        Assert.True(span.Equals(differentSpan));
                        Assert.True(span.Equals((object)differentSpan));
                    }
                    else
                    {
                        Assert.False(span.Equals(differentSpan));
                        Assert.False(span.Equals((object)differentSpan));
                    }
                }
            }
        }

        [Fact]
        public void SpanOfByteEqualOtherSpanOfByte()
        {
            byte[] bytes1 = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            //                              ^span1^
            //                                 ^span4^

            byte[] bytes2 = new byte[] { 0, 1, 2, 3, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
            //                              ^span2^     ^span3^
            //                                 ^span5^

            Span<byte> spanOfAllBytes2 = new Span<byte>(bytes2);

            Span<byte> span1 = new Span<byte>(bytes1, 1, 3);
            Span<byte> span2 = new Span<byte>(bytes2, 1, 3);
            Span<byte> span3 = spanOfAllBytes2.Slice(4).Slice(1, 3);
            Span<byte> span4 = new Span<byte>(bytes1, 2, 3);
            Span<byte> span5 = new Span<byte>(bytes2, 2, 3);
            
            Assert.Equal(span1, span1);
            Assert.Equal(span1, span2);
            Assert.Equal(span1, span3);
            Assert.NotEqual(span1, span4);
            Assert.NotEqual(span1, span5);
            
            Assert.Equal(span2, span1);
            Assert.Equal(span2, span2);
            Assert.Equal(span2, span3);
            Assert.NotEqual(span2, span4);
            Assert.NotEqual(span2, span5);

            Assert.Equal(span3, span1);
            Assert.Equal(span3, span2);
            Assert.Equal(span3, span3);
            Assert.NotEqual(span3, span4);
            Assert.NotEqual(span3, span5);

            Assert.NotEqual(span4, span1);
            Assert.NotEqual(span4, span2);
            Assert.NotEqual(span4, span3);
            Assert.Equal(span4, span4);
            Assert.NotEqual(span4, span5);

            Assert.NotEqual(span5, span1);
            Assert.NotEqual(span5, span2);
            Assert.NotEqual(span5, span3);
            Assert.NotEqual(span5, span4);
            Assert.Equal(span5, span5);
        }

        [Fact]
        public void DefaultSpanNullSpanEmptySpanEqualsTo()
        {
            byte[] bytes1 = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            //                              ^span1^

            Span<byte> span1 = new Span<byte>(bytes1, 1, 3);

            Span<byte> defaultSpan = default(Span<byte>);
            Assert.NotEqual(defaultSpan, span1);

            Assert.NotEqual(span1, defaultSpan);

            byte[] emptyArray = new byte[0];
            Span<byte> emptySpan = new Span<byte>(emptyArray);
            Assert.Equal(emptySpan, emptySpan);
            Assert.NotEqual(emptySpan, span1);
            Assert.NotEqual(span1, emptySpan);

            Assert.Equal(emptySpan, defaultSpan);
            Assert.Equal(defaultSpan, emptySpan);

            // TODO: Not so sure if this should be forbidden
            //byte[] nullBytes = null;
            //Span<byte> nullSpan = new Span<byte>(nullBytes, 0);
            //Assert.Equal(nullSpan, nullSpan);

            //Assert.Equal(nullSpan, defaultSpan);
            //Assert.Equal(nullSpan, emptySpan);
            //Assert.Equal(defaultSpan, nullSpan);
            //Assert.Equal(emptySpan, nullSpan);

            //Assert.NotEqual(nullSpan, span1);

            //Assert.NotEqual(span1, nullSpan);
        }

        [Theory]
        // copy whole buffer
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 1, 2, 3, 4, 5, 6 }, 0, 6,
            new byte[] { 7, 7, 7, 7, 7, 7 }, 0, 6)]
        // copy first half to first half (length match)
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 1, 2, 3, 7, 7, 7 }, 0, 3,
            new byte[] { 7, 7, 7, 4, 5, 6 }, 0, 3)]
        // copy second half to second half (length match)
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 7, 7, 7, 4, 5, 6 }, 3, 3,
            new byte[] { 1, 2, 3, 7, 7, 7 }, 3, 3)]
        // copy first half to first half
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 1, 2, 3, 7, 7, 7 }, 0, 3,
            new byte[] { 7, 7, 7, 4, 5, 6 }, 0, 6)]
        // copy no bytes starting from index 0
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 7, 7, 7, 7, 7, 7 }, 0, 0,
            new byte[] { 1, 2, 3, 4, 5, 6 }, 0, 6)]
        // copy no bytes starting from index 3
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 7, 7, 7, 7, 7, 7 }, 3, 0,
            new byte[] { 1, 2, 3, 4, 5, 6 }, 0, 6)]
        // copy no bytes starting at the end
        [InlineData(
            new byte[] { 7, 7, 7, 4, 5, 6 },
            new byte[] { 1, 2, 3, 7, 7, 7 }, 6, 0,
            new byte[] { 7, 7, 7, 4, 5, 6 }, 0, 6)]
        // copy first byte of 1 element array to last position
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 6 }, 0, 1,
            new byte[] { 1, 2, 3, 4, 5, 7 }, 5, 1)]
        // copy first two bytes of 2 element array to last two positions
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 5, 6 }, 0, 2,
            new byte[] { 1, 2, 3, 4, 7, 7 }, 4, 2)]
        // copy first two bytes of 3 element array to last two positions
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 5, 6, 7 }, 0, 2,
            new byte[] { 1, 2, 3, 4, 7, 7 }, 4, 2)]
        // copy last two bytes of 3 element array to last two positions
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 7, 5, 6 }, 1, 2,
            new byte[] { 1, 2, 3, 4, 7, 7 }, 4, 2)]
        // copy first two bytes of 2 element array to the middle of other array
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 3, 4 }, 0, 2,
            new byte[] { 1, 2, 7, 7, 5, 6 }, 2, 3)]
        // copy one byte from the beginning at the end of other array
        [InlineData(
            (byte[])null,
            new byte[] { 7, 7, 7, 7, 7, 7 }, 0, 1,
            new byte[] { 7, 7, 7, 7, 7, 7 }, 6, 0)]
        // copy two bytes from the beginning at 5th element
        [InlineData(
            (byte[])null,
            new byte[] { 7, 7, 7, 7, 7, 7 }, 0, 2,
            new byte[] { 7, 7, 7, 7, 7, 7 }, 5, 1)]
        // copy one byte from the beginning at the end of other array
        [InlineData(
            (byte[])null,
            new byte[] { 7, 7, 7, 7, 7, 7 }, 5, 1,
            new byte[] { 7, 7, 7, 7, 7, 7 }, 6, 0)]
        // copy two bytes from the beginning at 5th element
        [InlineData(
            (byte[])null,
            new byte[] { 7, 7, 7, 7, 7, 7 }, 4, 2,
            new byte[] { 7, 7, 7, 7, 7, 7 }, 5, 1)]
        [CLSCompliant(false)] // TODO: find out why the assembly-level CLSCompliant=false doesn't work
        public void SpanOfByteCopyToAnotherSpanOfByteTwoDifferentBuffersValidCases(byte[] expected, byte[] a, int aidx, int acount, byte[] b, int bidx, int bcount)
        {
            if (expected != null)
            {
                Span<byte> spanA = new Span<byte>(a, aidx, acount);
                Span<byte> spanB = new Span<byte>(b, bidx, bcount);

                Assert.True(spanA.TryCopyTo(spanB));
                Assert.Equal(expected, b);

                Span<byte> spanExpected = new Span<byte>(expected);
                Span<byte> spanBAll = new Span<byte>(b);
                Assert.Equal(spanExpected, spanBAll);
            }
            else
            {
                Span<byte> spanA = new Span<byte>(a, aidx, acount);
                Span<byte> spanB = new Span<byte>(b, bidx, bcount);
                Assert.False(spanA.TryCopyTo(spanB));
            }
        }
    }
}
