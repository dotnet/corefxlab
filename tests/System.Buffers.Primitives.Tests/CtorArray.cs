// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Xunit;

namespace System.Buffers.Tests
{
    public class CtorArray
    {
        [Fact]
        public static void CtorArray1()
        {
            int[] a = { 91, 92, -93, 94 };
            Buffer<int> buffer;

            buffer = new Buffer<int>(a);
            TestHelpers.Validate(buffer, 91, 92, -93, 94);

            buffer = new Buffer<int>(a, 0);
            TestHelpers.Validate(buffer, 91, 92, -93, 94);

            buffer = new Buffer<int>(a, 0, a.Length);
            TestHelpers.Validate(buffer, 91, 92, -93, 94);
        }

        [Fact]
        public static void CtorArray2()
        {
            long[] a = { 91, -92, 93, 94, -95 };
            Buffer<long> buffer;

            buffer = new Buffer<long>(a);
            TestHelpers.Validate(buffer, 91, -92, 93, 94, -95);

            buffer = new Buffer<long>(a, 0);
            TestHelpers.Validate(buffer, 91, -92, 93, 94, -95);

            buffer = new Buffer<long>(a, 0, a.Length);
            TestHelpers.Validate(buffer, 91, -92, 93, 94, -95);
        }

        [Fact]
        public static void CtorArray3()
        {
            object o1 = new object();
            object o2 = new object();
            object[] a = { o1, o2 };
            Buffer<object> buffer;

            buffer = new Buffer<object>(a);
            TestHelpers.Validate(buffer, o1, o2);

            buffer = new Buffer<object>(a, 0);
            TestHelpers.Validate(buffer, o1, o2);

            buffer = new Buffer<object>(a, 0, a.Length);
            TestHelpers.Validate(buffer, o1, o2);
        }

        [Fact]
        public static void CtorArrayZeroLength()
        {
            int[] empty = Array.Empty<int>();
            Buffer<int> buffer;

            buffer = new Buffer<int>(empty);
            TestHelpers.Validate(buffer);

            buffer = new Buffer<int>(empty, 0);
            TestHelpers.Validate(buffer);

            buffer = new Buffer<int>(empty, 0, empty.Length);
            TestHelpers.Validate(buffer);
        }

        [Fact]
        public static void CtorArrayNullArray()
        {
            Assert.Throws<ArgumentNullException>(() => new Buffer<int>(null));
            Assert.Throws<ArgumentNullException>(() => new Buffer<int>(null, 0));
            Assert.Throws<ArgumentNullException>(() => new Buffer<int>(null, 0, 0));
        }

        [Fact]
        public static void CtorArrayWrongArrayType()
        {
            // Cannot pass variant array, if array type is not a valuetype.
            string[] a = { "Hello" };
            Assert.Throws<ArrayTypeMismatchException>(() => new Buffer<object>(a));
            Assert.Throws<ArrayTypeMismatchException>(() => new Buffer<object>(a, 0));
            Assert.Throws<ArrayTypeMismatchException>(() => new Buffer<object>(a, 0, a.Length));
        }

        [Fact]
        public static void CtorArrayWrongValueType()
        {
            // Can pass variant array, if array type is a valuetype.

            uint[] a = { 42u, 0xffffffffu };
            int[] aAsIntArray = (int[])(object)a;
            Buffer<int> buffer;

            buffer = new Buffer<int>(aAsIntArray);
            TestHelpers.Validate(buffer, 42, -1);

            buffer = new Buffer<int>(aAsIntArray, 0);
            TestHelpers.Validate(buffer, 42, -1);

            buffer = new Buffer<int>(aAsIntArray, 0, aAsIntArray.Length);
            TestHelpers.Validate(buffer, 42, -1);
        }

        [Fact]
        public static void CtorArrayInt1()
        {
            int[] a = { 90, 91, 92, 93, 94, 95, 96, 97, 98 };
            var buffer = new Buffer<int>(a, 3);
            TestHelpers.Validate(buffer, 93, 94, 95, 96, 97, 98);
        }

        [Fact]
        public static void CtorArrayInt2()
        {
            long[] a = { 90, 91, 92, 93, 94, 95, 96, 97, 98 };
            var buffer = new Buffer<long>(a, 3);
            TestHelpers.Validate(buffer, 93, 94, 95, 96, 97, 98);
        }

        [Fact]
        public static void CtorArrayIntNegativeStart()
        {
            int[] a = new int[3];
            Assert.Throws<ArgumentOutOfRangeException>(() => new Buffer<int>(a, -1));
        }

        [Fact]
        public static void CtorArrayIntStartTooLarge()
        {
            int[] a = new int[3];
            Assert.Throws<ArgumentOutOfRangeException>(() => new Buffer<int>(a, 4));
        }

        [Fact]
        public static void CtorArrayIntStartEqualsLength()
        {
            // Valid for start to equal the array length. This returns an empty buffer that starts "just past the array."
            int[] a = { 91, 92, 93 };
            var buffer = new Buffer<int>(a, 3);
            TestHelpers.Validate(buffer);
        }

        [Fact]
        public static void CtorArrayIntInt1()
        {
            int[] a = { 90, 91, 92, 93, 94, 95, 96, 97, 98 };
            var buffer = new Buffer<int>(a, 3, 2);
            TestHelpers.Validate(buffer, 93, 94);
        }

        [Fact]
        public static void CtorArrayIntInt2()
        {
            long[] a = { 90, 91, 92, 93, 94, 95, 96, 97, 98 };
            var buffer = new Buffer<long>(a, 4, 3);
            TestHelpers.Validate(buffer, 94, 95, 96);
        }

        [Fact]
        public static void CtorArrayIntIntRangeExtendsToEndOfArray()
        {
            long[] a = { 90, 91, 92, 93, 94, 95, 96, 97, 98 };
            var buffer = new Buffer<long>(a, 4, 5);
            TestHelpers.Validate(buffer, 94, 95, 96, 97, 98);
        }

        [Fact]
        public static void CtorArrayIntIntNegativeStart()
        {
            int[] a = new int[3];
            Assert.Throws<ArgumentOutOfRangeException>(() => new Buffer<int>(a, -1, 0));
        }

        [Fact]
        public static void CtorArrayIntIntStartTooLarge()
        {
            int[] a = new int[3];
            Assert.Throws<ArgumentOutOfRangeException>(() => new Buffer<int>(a, 4, 0));
        }

        [Fact]
        public static void CtorArrayIntIntNegativeLength()
        {
            int[] a = new int[3];
            Assert.Throws<ArgumentOutOfRangeException>(() => new Buffer<int>(a, 0, -1));
        }

        [Fact]
        public static void CtorArrayIntIntStartAndLengthTooLarge()
        {
            int[] a = new int[3];
            Assert.Throws<ArgumentOutOfRangeException>(() => new Buffer<int>(a, 3, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Buffer<int>(a, 2, 2));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Buffer<int>(a, 1, 3));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Buffer<int>(a, 0, 4));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Buffer<int>(a, int.MaxValue, int.MaxValue));
        }

        [Fact]
        public static void CtorArrayIntIntStartEqualsLength()
        {
            // Valid for start to equal the array length. This returns an empty buffer that starts "just past the array."
            int[] a = { 91, 92, 93 };
            var buffer = new Buffer<int>(a, 3, 0);
            TestHelpers.Validate(buffer);
        }
    }
}
