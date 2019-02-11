// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System.Text.Json.Serialization.Tests
{
    public partial class ArrayTests
    {
        [Fact]
        public static void FromJsonClassWithStringArray()
        {
            TestClassWithStringArray obj = JsonConverter.FromJson<TestClassWithStringArray>(TestClassWithStringArray.s_data);
            obj.Verify();
        }

        [Fact]
        public static void FromJsonClassWithObjectList()
        {
            TestClassWithObjectList obj = JsonConverter.FromJson<TestClassWithObjectList>(TestClassWithObjectList.s_data);
            obj.Verify();
        }

        [Fact]
        public static void FromJsonClassWithObjectArray()
        {
            TestClassWithObjectArray obj = JsonConverter.FromJson<TestClassWithObjectArray>(TestClassWithObjectArray.s_data);
            obj.Verify();
        }

        [Fact]
        public static void FromJsonClassWithGenericList()
        {
            TestClassWithGenericList obj = JsonConverter.FromJson<TestClassWithGenericList>(TestClassWithGenericList.s_data);
            obj.Verify();
        }
    }
}
