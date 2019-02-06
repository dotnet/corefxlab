// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Xunit;

namespace System.Text.Json.Serialization.Tests
{
    public partial class ToJsonTests
    {
        [Fact]
        public static void Primitives()
        {
            {
                string json = JsonConverter.ToJsonString(1);
                Assert.Equal("1", json);
            }

            {
                Span<byte> json = JsonConverter.ToJson(1);
                Assert.Equal(Encoding.UTF8.GetBytes("1"), json.ToArray());
            }

            {
                string json = JsonConverter.ToJsonString(long.MaxValue);
                Assert.Equal(long.MaxValue.ToString(), json);
            }

            {
                Span<byte> json = JsonConverter.ToJson(long.MaxValue);
                Assert.Equal(Encoding.UTF8.GetBytes(long.MaxValue.ToString()), json.ToArray());
            }

            {
                string json = JsonConverter.ToJsonString("Hello");
                Assert.Equal(@"""Hello""", json);
            }

            {
                Span<byte> json = JsonConverter.ToJson("Hello");
                Assert.Equal(Encoding.UTF8.GetBytes(@"""Hello"""), json.ToArray());
            }
        }

        [Fact]
        public static void PrimitiveArray()
        {
            var input = new int[] { 0, 1 };
            string json = JsonConverter.ToJsonString(input);
            Assert.Equal("[0,1]", json);
        }

        [Fact]
        public static void ArrayWithEnums()
        {
            var input = new SampleEnum[] { SampleEnum.One, SampleEnum.Two };
            string json = JsonConverter.ToJsonString(input);
            Assert.Equal("[1,2]", json);
        }

        [Fact]
        public static void ObjectArray()
        {
            string json;

            {
                SimpleTestClass[] input = new SimpleTestClass[] { new SimpleTestClass(), new SimpleTestClass() };
                input[0].Initialize();
                input[0].Verify();

                input[1].Initialize();
                input[1].Verify();

                json = JsonConverter.ToJsonString(input);
            }

            {
                SimpleTestClass[] output = JsonConverter.FromJson<SimpleTestClass[]>(json);
                Assert.Equal(2, output.Length);
                output[0].Verify();
                output[1].Verify();
            }
        }

        [Fact]
        public static void PrimitiveJaggedArray()
        {
            var input = new int[2][];
            input[0] = new int[] { 1, 2 };
            input[1] = new int[] { 3, 4 };

            string json = JsonConverter.ToJsonString(input);
            Assert.Equal("[[1,2],[3,4]]", json);
        }

        [Fact]
        public static void ListOfList()
        {
            var input = new List<List<int>>();
            input.Add(new List<int>() { 1, 2 });
            input.Add(new List<int>() { 3, 4 });

            string json = JsonConverter.ToJsonString(input);
            Assert.Equal("[[1,2],[3,4]]", json);
        }

        [Fact]
        public static void ListOfArray()
        {
            var input = new List<int[]>();
            input.Add(new int[] { 1, 2 });
            input.Add(new int[] { 3, 4 });

            string json = JsonConverter.ToJsonString(input);
            Assert.Equal("[[1,2],[3,4]]", json);
        }

        [Fact]
        public static void ArrayOfList()
        {
            var input = new List<int>[2];
            input[0] = new List<int>() { 1, 2 };
            input[1] = new List<int>() { 3, 4 };

            string json = JsonConverter.ToJsonString(input);
            Assert.Equal("[[1,2],[3,4]]", json);
        }

        [Fact]
        public static void PrimitiveList()
        {
            var input = new List<int> { 1, 2 };

            string json = JsonConverter.ToJsonString(input);
            Assert.Equal("[1,2]", json);
        }
    }
}
