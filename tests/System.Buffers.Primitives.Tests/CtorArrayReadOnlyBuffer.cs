// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Xunit;

namespace System.Buffers.Tests
{
    public class CtorArrayReadOnlyBuffer
    {
        [Fact]
        public static void CtorArray1()
        {
            int[] a = { 91, 92, -93, 94 };
            ReadOnlyBuffer<int> buffer;

            buffer = new ReadOnlyBuffer<int>(a);
            TestHelpers.Validate(buffer, 91, 92, -93, 94);

            buffer = new ReadOnlyBuffer<int>(a, 0);
            TestHelpers.Validate(buffer, 91, 92, -93, 94);

            buffer = new ReadOnlyBuffer<int>(a, 0, a.Length);
            TestHelpers.Validate(buffer, 91, 92, -93, 94);
        }

        [Fact]
        public static void CtorArray2()
        {
            long[] a = { 91, -92, 93, 94, -95 };
            ReadOnlyBuffer<long> buffer;

            buffer = new ReadOnlyBuffer<long>(a);
            TestHelpers.Validate(buffer, 91, -92, 93, 94, -95);

            buffer = new ReadOnlyBuffer<long>(a, 0);
            TestHelpers.Validate(buffer, 91, -92, 93, 94, -95);

            buffer = new ReadOnlyBuffer<long>(a, 0, a.Length);
            TestHelpers.Validate(buffer, 91, -92, 93, 94, -95);
        }

        [Fact]
        public static void CtorArray3()
        {
            object o1 = new object();
            object o2 = new object();
            object[] a = { o1, o2 };
            ReadOnlyBuffer<object> buffer;

            buffer = new ReadOnlyBuffer<object>(a);
            TestHelpers.Validate(buffer, o1, o2);

            buffer = new ReadOnlyBuffer<object>(a, 0);
            TestHelpers.Validate(buffer, o1, o2);

            buffer = new ReadOnlyBuffer<object>(a, 0, a.Length);
            TestHelpers.Validate(buffer, o1, o2);
        }

        [Fact]
        public static void CtorArrayZeroLength()
        {
            int[] empty = Array.Empty<int>();
            ReadOnlyBuffer<int> buffer;

            buffer = new ReadOnlyBuffer<int>(empty);
            TestHelpers.Validate(buffer);

            buffer = new ReadOnlyBuffer<int>(empty, 0);
            TestHelpers.Validate(buffer);

            buffer = new ReadOnlyBuffer<int>(empty, 0, empty.Length);
            TestHelpers.Validate(buffer);
        }

        [Fact]
        public static void CtorArrayNullArray()
        {
            Assert.Throws<ArgumentNullException>(() => new ReadOnlyBuffer<int>(null));
            Assert.Throws<ArgumentNullException>(() => new ReadOnlyBuffer<int>(null, 0));
            Assert.Throws<ArgumentNullException>(() => new ReadOnlyBuffer<int>(null, 0, 0));
        }

        [Fact]
        public static void CtorArrayWrongValueType()
        {
            // Can pass variant array, if array type is a valuetype.

            uint[] a = { 42u, 0xffffffffu };
            int[] aAsIntArray = (int[])(object)a;
            ReadOnlyBuffer<int> buffer;

            buffer = new ReadOnlyBuffer<int>(aAsIntArray);
            TestHelpers.Validate(buffer, 42, -1);

            buffer = new ReadOnlyBuffer<int>(aAsIntArray, 0);
            TestHelpers.Validate(buffer, 42, -1);

            buffer = new ReadOnlyBuffer<int>(aAsIntArray, 0, aAsIntArray.Length);
            TestHelpers.Validate(buffer, 42, -1);
        }

        [Fact]
        public static void CtorVariantArrayType()
        {
            // For ReadOnlyBuffer<T>, variant arrays are allowed for string to object
            // and reference type to object.

            ReadOnlyBuffer<object> buffer;

            string[] strArray = { "Hello" };
            buffer = new ReadOnlyBuffer<object>(strArray);
            TestHelpers.Validate(buffer, "Hello");
            buffer = new ReadOnlyBuffer<object>(strArray, 0);
            TestHelpers.Validate(buffer, "Hello");
            buffer = new ReadOnlyBuffer<object>(strArray, 0, strArray.Length);
            TestHelpers.Validate(buffer, "Hello");

            TestHelpers.TestClass c1 = new TestHelpers.TestClass();
            TestHelpers.TestClass c2 = new TestHelpers.TestClass();
            TestHelpers.TestClass[] clsArray = { c1, c2 };
            buffer = new ReadOnlyBuffer<object>(clsArray);
            TestHelpers.Validate(buffer, c1, c2);
            buffer = new ReadOnlyBuffer<object>(clsArray, 0);
            TestHelpers.Validate(buffer, c1, c2);
            buffer = new ReadOnlyBuffer<object>(clsArray, 0, clsArray.Length);
            TestHelpers.Validate(buffer, c1, c2);
        }

        [Fact]
        public static void CtorArrayInt1()
        {
            int[] a = { 90, 91, 92, 93, 94, 95, 96, 97, 98 };
            var buffer = new ReadOnlyBuffer<int>(a, 3);
            TestHelpers.Validate(buffer, 93, 94, 95, 96, 97, 98);
        }

        [Fact]
        public static void CtorArrayInt2()
        {
            long[] a = { 90, 91, 92, 93, 94, 95, 96, 97, 98 };
            var buffer = new ReadOnlyBuffer<long>(a, 3);
            TestHelpers.Validate(buffer, 93, 94, 95, 96, 97, 98);
        }

        [Fact]
        public static void CtorArrayIntNegativeStart()
        {
            int[] a = new int[3];
            Assert.Throws<ArgumentOutOfRangeException>(() => new ReadOnlyBuffer<int>(a, -1));
        }

        [Fact]
        public static void CtorArrayIntStartTooLarge()
        {
            int[] a = new int[3];
            Assert.Throws<ArgumentOutOfRangeException>(() => new ReadOnlyBuffer<int>(a, 4));
        }

        [Fact]
        public static void CtorArrayIntStartEqualsLength()
        {
            // Valid for start to equal the array length. This returns an empty buffer that starts "just past the array."
            int[] a = { 91, 92, 93 };
            var buffer = new ReadOnlyBuffer<int>(a, 3);
            TestHelpers.Validate(buffer);
        }

        [Fact]
        public static void CtorArrayIntInt1()
        {
            int[] a = { 90, 91, 92, 93, 94, 95, 96, 97, 98 };
            var buffer = new ReadOnlyBuffer<int>(a, 3, 2);
            TestHelpers.Validate(buffer, 93, 94);
        }

        [Fact]
        public static void CtorArrayIntInt2()
        {
            long[] a = { 90, 91, 92, 93, 94, 95, 96, 97, 98 };
            var buffer = new ReadOnlyBuffer<long>(a, 4, 3);
            TestHelpers.Validate(buffer, 94, 95, 96);
        }

        [Fact]
        public static void CtorArrayIntIntRangeExtendsToEndOfArray()
        {
            long[] a = { 90, 91, 92, 93, 94, 95, 96, 97, 98 };
            var buffer = new ReadOnlyBuffer<long>(a, 4, 5);
            TestHelpers.Validate(buffer, 94, 95, 96, 97, 98);
        }

        [Fact]
        public static void CtorArrayIntIntNegativeStart()
        {
            int[] a = new int[3];
            Assert.Throws<ArgumentOutOfRangeException>(() => new ReadOnlyBuffer<int>(a, -1, 0));
        }

        [Fact]
        public static void CtorArrayIntIntStartTooLarge()
        {
            int[] a = new int[3];
            Assert.Throws<ArgumentOutOfRangeException>(() => new ReadOnlyBuffer<int>(a, 4, 0));
        }

        [Fact]
        public static void CtorArrayIntIntNegativeLength()
        {
            int[] a = new int[3];
            Assert.Throws<ArgumentOutOfRangeException>(() => new ReadOnlyBuffer<int>(a, 0, -1));
        }

        [Fact]
        public static void CtorArrayIntIntStartAndLengthTooLarge()
        {
            int[] a = new int[3];
            Assert.Throws<ArgumentOutOfRangeException>(() => new ReadOnlyBuffer<int>(a, 3, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new ReadOnlyBuffer<int>(a, 2, 2));
            Assert.Throws<ArgumentOutOfRangeException>(() => new ReadOnlyBuffer<int>(a, 1, 3));
            Assert.Throws<ArgumentOutOfRangeException>(() => new ReadOnlyBuffer<int>(a, 0, 4));
            Assert.Throws<ArgumentOutOfRangeException>(() => new ReadOnlyBuffer<int>(a, int.MaxValue, int.MaxValue));
        }

        [Fact]
        public static void CtorArrayIntIntStartEqualsLength()
        {
            // Valid for start to equal the array length. This returns an empty buffer that starts "just past the array."
            int[] a = { 91, 92, 93 };
            var buffer = new ReadOnlyBuffer<int>(a, 3, 0);
            TestHelpers.Validate(buffer);
        }
    }
}
