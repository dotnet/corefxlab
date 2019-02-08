// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Xunit;

namespace System.Text.Json.Serialization.Tests
{
    public partial class ToJsonTests
    {
        public static IEnumerable<object[]> SuccessCases
        {
            get
            {
                yield return new object[] { "SimpleTestClass", new SimpleTestClass() };
                yield return new object[] { "SimpleTestClassWithNullables", new SimpleTestClassWithNullables() };
                yield return new object[] { "TestClassWithNestedObjectInner", new TestClassWithNestedObjectInner() };
                yield return new object[] { "TestClassWithNestedObjectOuter", new TestClassWithNestedObjectOuter() };
                yield return new object[] { "TestClassWithObjectArray", new TestClassWithObjectArray() };
                yield return new object[] { "TestClassWithStringArray", new TestClassWithStringArray() };
                yield return new object[] { "TestClassWithGenericList", new TestClassWithGenericList() };
            }
        }

        [Theory]
        [MemberData(nameof(SuccessCases))]
        public static void FromJson(string className, ITestClass testObj)
        {
            string json;

            {
                testObj.Initialize();
                testObj.Verify();
                json = JsonConverter.ToJsonString(testObj);
            }

            {
                ITestClass obj  = (ITestClass)JsonConverter.FromJson(json, testObj.GetType());
                obj.Verify();
            }
        }

        [Fact]
        public static void CyclicFail()
        {
            TestClassWithCycle obj = new TestClassWithCycle();
            obj.Initialize();

            // We don't allow cycles; we throw InvalidOperation however instead of unrecoverable StackOverflow
            Assert.Throws<InvalidOperationException>(() => JsonConverter.ToJson(obj));
        }
    }
}
