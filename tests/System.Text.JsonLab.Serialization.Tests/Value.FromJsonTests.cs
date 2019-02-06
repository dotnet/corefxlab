// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Xunit;

namespace System.Text.Json.Serialization.Tests
{
    public partial class FromJsonTests
    {
        [Fact]
        public static void Primitives()
        {
            int i = JsonConverter.FromJson<int>(Encoding.UTF8.GetBytes(@"1"));
            Assert.Equal(1, i);

            int i2 = JsonConverter.FromJson<int>("2");
            Assert.Equal(2, i2);

            long l = JsonConverter.FromJson<long>(Encoding.UTF8.GetBytes(long.MaxValue.ToString()));
            Assert.Equal(long.MaxValue, l);

            long l2 = JsonConverter.FromJson<long>(long.MaxValue.ToString());
            Assert.Equal(long.MaxValue, l2);

            string s = JsonConverter.FromJson<string>(Encoding.UTF8.GetBytes(@"""Hello"""));
            Assert.Equal("Hello", s);

            string s2 = JsonConverter.FromJson<string>(@"""Hello""");
            Assert.Equal("Hello", s2);
        }

        [Fact]
        public static void PrimitivesFail()
        {
            Assert.Throws<JsonReaderException>(() => JsonConverter.FromJson<int>(Encoding.UTF8.GetBytes(@"a")));
            Assert.Throws<JsonReaderException>(() => JsonConverter.FromJson<int[]>(Encoding.UTF8.GetBytes(@"[1,a]")));
        }

        [Fact]
        public static void PrimitiveArray()
        {
            int[] i = JsonConverter.FromJson<int[]>(Encoding.UTF8.GetBytes(@"[1,2]"));
            Assert.Equal(1, i[0]);
            Assert.Equal(2, i[1]);
        }

        [Fact]
        public static void ArrayWithEnums()
        {
            SampleEnum[] i = JsonConverter.FromJson<SampleEnum[]>(Encoding.UTF8.GetBytes(@"[1,2]"));
            Assert.Equal(SampleEnum.One, i[0]);
            Assert.Equal(SampleEnum.Two, i[1]);
        }

        [Fact]
        public static void PrimitiveArrayFail()
        {
            // Invalid data
            Assert.Throws<InvalidOperationException>(() => JsonConverter.FromJson<int[]>(Encoding.UTF8.GetBytes(@"[1,""a""]")));

            // Multidimensional arrays currently not supported
            Assert.Throws<InvalidOperationException>(() => JsonConverter.FromJson<int[,]>(Encoding.UTF8.GetBytes(@"[[1,2],[3,4]]")));
        }

        [Fact]
        public static void ObjectArray()
        {
            string data =
                "[" +
                SimpleTestClass.s_json +
                "," +
                SimpleTestClass.s_json +
                "]";

            SimpleTestClass[] i = JsonConverter.FromJson<SimpleTestClass[]>(Encoding.UTF8.GetBytes(data));

            i[0].Verify();
            i[1].Verify();
        }

        [Fact]
        public static void PrimitiveJaggedArray()
        {
            int[][] i = JsonConverter.FromJson<int[][]>(Encoding.UTF8.GetBytes(@"[[1,2],[3,4]]"));
            Assert.Equal(1, i[0][0]);
            Assert.Equal(2, i[0][1]);
            Assert.Equal(3, i[1][0]);
            Assert.Equal(4, i[1][1]);
        }

        [Fact]
        public static void ListOfList()
        {
            List<List<int>> result = JsonConverter.FromJson<List<List<int>>>(Encoding.UTF8.GetBytes(@"[[1,2],[3,4]]"));

            Assert.Equal(1, result[0][0]);
            Assert.Equal(2, result[0][1]);
            Assert.Equal(3, result[1][0]);
            Assert.Equal(4, result[1][1]);
        }

        [Fact]
        public static void ListOfArray()
        {
            List<int[]> result = JsonConverter.FromJson<List<int[]>>(Encoding.UTF8.GetBytes(@"[[1,2],[3,4]]"));

            Assert.Equal(1, result[0][0]);
            Assert.Equal(2, result[0][1]);
            Assert.Equal(3, result[1][0]);
            Assert.Equal(4, result[1][1]);
        }

        [Fact]
        public static void ArrayOfList()
        {
            List<int>[] result = JsonConverter.FromJson<List<int>[]> (Encoding.UTF8.GetBytes(@"[[1,2],[3,4]]"));

            Assert.Equal(1, result[0][0]);
            Assert.Equal(2, result[0][1]);
            Assert.Equal(3, result[1][0]);
            Assert.Equal(4, result[1][1]);
        }

        [Fact]
        public static void PrimitiveList()
        {
            List<int> i = JsonConverter.FromJson<List<int>>(Encoding.UTF8.GetBytes(@"[1,2]"));
            Assert.Equal(1, i[0]);
            Assert.Equal(2, i[1]);
        }
    }
}
