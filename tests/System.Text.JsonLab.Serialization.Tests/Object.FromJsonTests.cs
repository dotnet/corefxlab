// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Xunit;

namespace System.Text.Json.Serialization.Tests
{
    public partial class ObjectTests
    {
        public static IEnumerable<object[]> FromJsonSuccessCases
        {
            get
            {
                yield return new object[] { "SimpleTestClass", typeof(SimpleTestClass), SimpleTestClass.s_data };
                yield return new object[] { "SimpleTestClassWithNullables", typeof(SimpleTestClassWithNullables), SimpleTestClassWithNullables.s_data };
                yield return new object[] { "TestClassWithNestedObjectInner", typeof(TestClassWithNestedObjectInner), TestClassWithNestedObjectInner.s_data };
                yield return new object[] { "TestClassWithNestedObjectOuter", typeof(TestClassWithNestedObjectOuter), TestClassWithNestedObjectOuter.s_data };
                yield return new object[] { "TestClassWithObjectArray", typeof(TestClassWithObjectArray), TestClassWithObjectArray.s_data };
                yield return new object[] { "TestClassWithStringArray", typeof(TestClassWithStringArray), TestClassWithStringArray.s_data };
                yield return new object[] { "TestClassWithGenericList", typeof(TestClassWithGenericList), TestClassWithGenericList.s_data };

            }
        }

        [Theory]
        [MemberData(nameof(FromJsonSuccessCases))]
        public static void FromJson(string className, Type classType, byte[] data)
        {
            object obj = JsonConverter.FromJson(data, classType);
            Assert.IsAssignableFrom(typeof(ITestClass), obj);
            ((ITestClass)obj).Verify();
        }

        [Fact]
        public static void FromJsonGenericApi()
        {
            SimpleTestClass obj = JsonConverter.FromJson<SimpleTestClass>(SimpleTestClass.s_data);
            obj.Verify();
        }

        [Fact]
        public static void FromJsonStringApi()
        {
            SimpleTestClass obj = JsonConverter.FromJson<SimpleTestClass>(SimpleTestClass.s_json);
            obj.Verify();
        }
    }
}
