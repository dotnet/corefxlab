// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Xunit;

namespace System.Text.Json.Serialization.Tests
{
    public partial class SerializeTests
    {
        public class TestClassWithStringArray
        {
            public string[] MyData { get; set; }

            public static readonly byte[] s_data = Encoding.UTF8.GetBytes(
                @"{" +
                    @"""MyData"":[" +
                        @"""Hello""," +
                        @"""World""" +
                    @"]" +
                @"}");
        }

        [Fact]
        public static void ArrayWithPrimitives()
        {
            TestClassWithStringArray root = JsonConverter.FromJson<TestClassWithStringArray>(TestClassWithStringArray.s_data);
            Assert.Equal("Hello", root.MyData[0]);
            Assert.Equal("World", root.MyData[1]);
        }

        public class TestClassWithObjectList
        {
            public List<SimpleTestClass> MyData { get; set; }

            public static readonly byte[] s_data = Encoding.UTF8.GetBytes(
                @"{" +
                    @"""MyData"":[" +
                        SimpleTestClass.s_json + "," +
                        SimpleTestClass.s_json +
                    @"]" +
                @"}");
        }

        [Fact]
        public static void ArrayWithObjects()
        {
            TestClassWithObjectList root = JsonConverter.FromJson<TestClassWithObjectList>(TestClassWithObjectList.s_data);
            SimpleTestClass obj0 = root.MyData[0];
            obj0.Verify();

            SimpleTestClass obj1 = root.MyData[1];
            obj1.Verify();
        }

        public class TestClassWithGenericList
        {
            public List<string> MyData { get; set; }

            public static readonly byte[] s_data = Encoding.UTF8.GetBytes(
                @"{" +
                    @"""MyData"":[" +
                        @"""Hello""," +
                        @"""World""" +
                    @"]" +
                @"}");
        }

        [Fact]
        public static void ListWithPrimitives()
        {
            TestClassWithGenericList root = JsonConverter.FromJson<TestClassWithGenericList>(TestClassWithGenericList.s_data);
            Assert.Equal("Hello", root.MyData[0]);
            Assert.Equal("World", root.MyData[1]);
        }
    }
}
