// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using Xunit;

namespace System.Text.Utf8.Tests
{
    public class TypeConstraintsTests
    {
        [Fact]
        public void Utf8StringIsAStruct()
        {
            Assert.True(typeof(Utf8String).GetTypeInfo().IsValueType);
        }

        [Fact]
        public void Utf8StringCodeUnitsEnumeratorIsAStruct()
        {
            Assert.True(typeof(Utf8String.Enumerator).GetTypeInfo().IsValueType);
        }

        [Fact]
        public void Utf8StringCodePointEnumerableIsAStruct()
        {
            Assert.True(typeof(Utf8String.CodePointEnumerable).GetTypeInfo().IsValueType);
        }

        [Fact]
        public void Utf8StringCodePointEnumeratorIsAStruct()
        {
            Assert.True(typeof(Utf8String.CodePointEnumerator).GetTypeInfo().IsValueType);
        }

        [Fact]
        public void Utf8StringReverseCodePointEnumeratorIsAStruct()
        {
            Assert.True(typeof(Utf8String.CodePointReverseEnumerator).GetTypeInfo().IsValueType);
        }
    }
}
