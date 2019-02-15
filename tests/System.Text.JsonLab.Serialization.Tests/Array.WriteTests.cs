// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System.Text.Json.Serialization.Tests
{
    public partial class ArrayTests
    {
        [Fact]
        public static void WriteClassWithStringArray()
        {
            string json;

            {
                TestClassWithStringArray obj = new TestClassWithStringArray();
                obj.Initialize();
                obj.Verify();
                json = JsonSerializer.WriteString(obj);
            }

            {
                TestClassWithStringArray obj = JsonSerializer.ReadString<TestClassWithStringArray>(json);
                obj.Verify();
            }

            {
                TestClassWithStringArray obj = JsonSerializer.Read<TestClassWithStringArray>(TestClassWithStringArray.s_data);
                obj.Verify();
            }
        }

        [Fact]
        public static void WriteClassWithObjectArray()
        {
            string json;

            {
                TestClassWithGenericList obj = new TestClassWithGenericList();
                obj.Initialize();
                obj.Verify();
                json = JsonSerializer.WriteString(obj);
            }

            {
                TestClassWithGenericList obj = JsonSerializer.ReadString<TestClassWithGenericList>(json);
                obj.Verify();
            }

            {
                TestClassWithGenericList obj = JsonSerializer.Read<TestClassWithGenericList>(TestClassWithGenericList.s_data);
                obj.Verify();
            }
        }

        [Fact]
        public static void WriteClassWithGenericList()
        {
            string json;

            {
                TestClassWithObjectList obj = new TestClassWithObjectList();
                obj.Initialize();
                obj.Verify();
                json = JsonSerializer.WriteString(obj);
            }

            {
                TestClassWithObjectList obj = JsonSerializer.ReadString<TestClassWithObjectList>(json);
                obj.Verify();
            }

            {
                TestClassWithObjectList obj = JsonSerializer.Read<TestClassWithObjectList>(TestClassWithObjectList.s_data);
                obj.Verify();
            }
        }
    }
}
