// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System.Text.Json.Serialization.Tests
{
    public partial class ToJsonTests
    {
        [Fact]
        public static void ClassWithStringArray()
        {
            string json;

            {
                TestClassWithStringArray obj = new TestClassWithStringArray();
                obj.Initialize();
                obj.Verify();
                json = JsonConverter.ToJsonString(obj);
            }

            {
                TestClassWithStringArray obj = JsonConverter.FromJson<TestClassWithStringArray>(json);
                obj.Verify();
            }

            {
                TestClassWithStringArray obj = JsonConverter.FromJson<TestClassWithStringArray>(TestClassWithStringArray.s_data);
                obj.Verify();
            }
        }

        [Fact]
        public static void ClassWithObjectArray()
        {
            string json;

            {
                TestClassWithGenericList obj = new TestClassWithGenericList();
                obj.Initialize();
                obj.Verify();
                json = JsonConverter.ToJsonString(obj);
            }

            {
                TestClassWithGenericList obj = JsonConverter.FromJson<TestClassWithGenericList>(json);
                obj.Verify();
            }

            {
                TestClassWithGenericList obj = JsonConverter.FromJson<TestClassWithGenericList>(TestClassWithGenericList.s_data);
                obj.Verify();
            }
        }

        [Fact]
        public static void ClassWithGenericList()
        {
            string json;

            {
                TestClassWithObjectList obj = new TestClassWithObjectList();
                obj.Initialize();
                obj.Verify();
                json = JsonConverter.ToJsonString(obj);
            }

            {
                TestClassWithObjectList obj = JsonConverter.FromJson<TestClassWithObjectList>(json);
                obj.Verify();
            }

            {
                TestClassWithObjectList obj = JsonConverter.FromJson<TestClassWithObjectList>(TestClassWithObjectList.s_data);
                obj.Verify();
            }
        }
    }
}
