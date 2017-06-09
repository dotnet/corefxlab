// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Xunit;

namespace System.Buffers.Tests
{
    internal static class TestHelpers
    {
        public static void SequenceEqualValueType<T>(ReadOnlyBuffer<T> buffer, params T[] expected) where T: struct, IEquatable<T>
        {
            Assert.True(buffer.Span.SequenceEqual(expected));
        }

        public static void SequenceEqual<T>(ReadOnlyBuffer<T> buffer, params T[] expected)
        {
            T[] bufferArray = buffer.ToArray();
            Assert.Equal(buffer.Length, expected.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                T actual = bufferArray[i];
                Assert.Same(expected[i], actual);
            }
        }

        public sealed class TestClass
        {
            private double _d;
            public char C0;
            public char C1;
            public char C2;
            public char C3;
            public char C4;
        }
    }
}
