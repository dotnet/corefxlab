// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System.Text.Json.Serialization.Tests
{
    public partial class FromJsonTests
    {
        [Fact]
        public static void ClassWithStringArray()
        {
            TestClassWithStringArray obj = JsonConverter.FromJson<TestClassWithStringArray>(TestClassWithStringArray.s_data);
            obj.Verify();
        }

        [Fact]
        public static void ClassWithObjectList()
        {
            TestClassWithObjectList obj = JsonConverter.FromJson<TestClassWithObjectList>(TestClassWithObjectList.s_data);
            obj.Verify();
        }

        [Fact]
        public static void ClassWithObjectArray()
        {
            TestClassWithObjectArray obj = JsonConverter.FromJson<TestClassWithObjectArray>(TestClassWithObjectArray.s_data);
            obj.Verify();
        }

        [Fact]
        public static void ClassWithGenericList()
        {
            TestClassWithGenericList obj = JsonConverter.FromJson<TestClassWithGenericList>(TestClassWithGenericList.s_data);
            obj.Verify();
        }
    }
}
