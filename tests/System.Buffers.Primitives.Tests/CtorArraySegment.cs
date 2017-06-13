// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Xunit;

namespace System.Buffers.Tests
{
    public class CtorArraySegment
    {
        [Fact]
        public static void CtorImplicitArraySegment()
        {
            int[] a = { 19, -17 };
            ArraySegment<int> segmentInt = new ArraySegment<int>(a, 1, 1);
            TestHelpers.SequenceEqualValueType(segmentInt, -17);

            long[] b = { 1, -3, 7, -15, 31 };
            ArraySegment<long> segmentLong = new ArraySegment<long>(b, 1, 3);
            TestHelpers.SequenceEqualValueType<long>(segmentLong, -3, 7, -15);

            object o1 = new object();
            object o2 = new object();
            object o3 = new object();
            object o4 = new object();
            object[] c = { o1, o2, o3, o4 };
            ArraySegment<object> segmentObject = new ArraySegment<object>(c, 0, 2);
            TestHelpers.SequenceEqual(segmentObject, o1, o2);
        }

        [Fact]
        public static void CtorImplicitZeroLengthArraySegment()
        {
            int[] empty = Array.Empty<int>();
            ArraySegment<int> emptySegment = new ArraySegment<int>(empty);
            TestHelpers.SequenceEqual<int>(emptySegment);

            int[] a = { 19, -17 };
            ArraySegment<int> segmentInt = new ArraySegment<int>(a, 1, 0);
            TestHelpers.SequenceEqual<int>(segmentInt);
        }
    }
}
