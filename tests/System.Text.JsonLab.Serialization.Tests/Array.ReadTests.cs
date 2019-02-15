// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System.Text.Json.Serialization.Tests
{
    public partial class ArrayTests
    {
        [Fact]
        public static void ReadClassWithStringArray()
        {
            TestClassWithStringArray obj = JsonSerializer.Read<TestClassWithStringArray>(TestClassWithStringArray.s_data);
            obj.Verify();
        }

        [Fact]
        public static void ReadClassWithObjectList()
        {
            TestClassWithObjectList obj = JsonSerializer.Read<TestClassWithObjectList>(TestClassWithObjectList.s_data);
            obj.Verify();
        }

        [Fact]
        public static void ReadClassWithObjectArray()
        {
            TestClassWithObjectArray obj = JsonSerializer.Read<TestClassWithObjectArray>(TestClassWithObjectArray.s_data);
            obj.Verify();
        }

        [Fact]
        public static void ReadClassWithGenericList()
        {
            TestClassWithGenericList obj = JsonSerializer.Read<TestClassWithGenericList>(TestClassWithGenericList.s_data);
            obj.Verify();
        }
    }
}
