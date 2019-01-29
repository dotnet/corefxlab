// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System.Text.Json.Serialization.Tests
{
    public partial class SerializeTests
    {
        [Fact]
        public static void SimpleObject()
        {
            SimpleTestClass obj = JsonConverter.FromJson<SimpleTestClass>(SimpleTestClass.s_data);
            obj.Verify();
        }

        [Fact]
        public static void SerializePrimitivesNonGeneric()
        {
            SimpleTestClass obj = (SimpleTestClass)JsonConverter.FromJson(SimpleTestClass.s_data, typeof(SimpleTestClass));
            obj.Verify();
        }

        public class TestClassWithNestedObjectInner
        {
            public SimpleTestClass MyData { get; set; }

            public static readonly string s_json =
                @"{" +
                    @"""MyData"":" + SimpleTestClass.s_json +
                @"}";

            public static readonly byte[] s_data = Encoding.UTF8.GetBytes(s_json);
        }

        [Fact]
        public static void InnerObject()
        {
            TestClassWithNestedObjectInner root = JsonConverter.FromJson<TestClassWithNestedObjectInner>(TestClassWithNestedObjectInner.s_data);
            Assert.NotNull(root.MyData);
            root.MyData.Verify();
        }

        public class TestClassWithNestedObjectOuter
        {
            public TestClassWithNestedObjectInner MyData { get; set; }

            public static readonly byte[] s_data = Encoding.UTF8.GetBytes(
                @"{" +
                    @"""MyData"":" + TestClassWithNestedObjectInner.s_json +
                @"}");
        }

        [Fact]
        public static void NestedNestedObject()
        {
            TestClassWithNestedObjectOuter root = JsonConverter.FromJson<TestClassWithNestedObjectOuter>(TestClassWithNestedObjectOuter.s_data);
            Assert.NotNull(root.MyData);
            Assert.NotNull(root.MyData.MyData);
            root.MyData.MyData.Verify();
        }
    }
}
