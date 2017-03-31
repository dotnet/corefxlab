// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using Xunit;

namespace System.Text.Utf8.Tests
{
    public class TypeConstraintsTests
    {
        [Fact(Skip = "System.InvalidProgramException : Common Language Runtime detected an invalid program.")]
        public void Utf8StringIsAStruct()
        {
            Utf8String _anyUtf8String = new Utf8String("anyString");
            Assert.True(_anyUtf8String.GetType().GetTypeInfo().IsValueType);
        }

        [Fact(Skip = "System.InvalidProgramException : Common Language Runtime detected an invalid program.")]
        public void Utf8StringCodeUnitsEnumeratorIsAStruct()
        {
            Utf8String _anyUtf8String = new Utf8String("anyString");
            var utf8CodeUnitsEnumerator = _anyUtf8String.GetEnumerator();
            Assert.True(_anyUtf8String.GetType().GetTypeInfo().IsValueType);
        }

        [Fact(Skip = "System.InvalidProgramException : Common Language Runtime detected an invalid program.")]
        public void Utf8StringCodePointEnumerableIsAStruct()
        {
            Utf8String _anyUtf8String = new Utf8String("anyString");
            Assert.True(_anyUtf8String.CodePoints.GetType().GetTypeInfo().IsValueType);
        }

        [Fact(Skip = "System.InvalidProgramException : Common Language Runtime detected an invalid program.")]
        public void Utf8StringCodePointEnumeratorIsAStruct()
        {
            Utf8String _anyUtf8String = new Utf8String("anyString");
            var utf8CodePointEnumerator = _anyUtf8String.CodePoints.GetEnumerator();
            Assert.True(utf8CodePointEnumerator.GetType().GetTypeInfo().IsValueType);
        }

        [Fact(Skip = "System.InvalidProgramException : Common Language Runtime detected an invalid program.")]
        public void Utf8StringReverseCodePointEnumeratorIsAStruct()
        {
            Utf8String _anyUtf8String = new Utf8String("anyString");
            var utf8CodePointEnumerator = _anyUtf8String.CodePoints.GetReverseEnumerator();
            Assert.True(utf8CodePointEnumerator.GetType().GetTypeInfo().IsValueType);
        }
    }
}
