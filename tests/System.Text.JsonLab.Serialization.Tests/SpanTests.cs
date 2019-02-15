// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Xunit;

namespace System.Text.Json.Serialization.Tests
{
    public class SpanTests
    {
        [Fact]
        public static void NullObjectInputFail()
        {
            Assert.Throws<ArgumentNullException>(() => JsonSerializer.Read<string>((ReadOnlySpan<byte>)null));
        }

        [Theory]
        [MemberData(nameof(ReadSuccessCases))]
        public static void Read(Type classType, byte[] data)
        {
            object obj = JsonSerializer.Read(data, classType);
            Assert.IsAssignableFrom(typeof(ITestClass), obj);
            ((ITestClass)obj).Verify();
        }

        [Fact]
        public static void ReadGenericApi()
        {
            SimpleTestClass obj = JsonSerializer.Read<SimpleTestClass>(SimpleTestClass.s_data);
            obj.Verify();
        }

        [Fact]
        public static void VerifyValueFail()
        {
            Assert.Throws<ArgumentNullException>(() => JsonSerializer.Write(new object(), (Type)null));
        }

        [Fact]
        public static void VerifyTypeFail()
        {
            Assert.Throws<ArgumentException>(() => JsonSerializer.Write(1, typeof(string)));
        }

        [Fact]
        public static void NullObjectOutput()
        {
            var encodedNull = Encoding.UTF8.GetBytes(@"null");

            {
                byte[] output = JsonSerializer.Write(null);
                Assert.Equal(encodedNull, output);
            }

            {
                byte[] output = JsonSerializer.Write(null, typeof(NullTests));
                Assert.Equal(encodedNull, output);
            }
        }

        public static IEnumerable<object[]> ReadSuccessCases
        {
            get
            {
                return TestData.ReadSuccessCases;
            }
        }
    }
}
