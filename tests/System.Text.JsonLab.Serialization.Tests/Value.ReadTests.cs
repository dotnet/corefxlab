// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Xunit;

namespace System.Text.Json.Serialization.Tests
{
    public partial class ReadTests
    {
        [Fact]
        public static void Primitives()
        {
            int i = JsonSerializer.Read<int>(Encoding.UTF8.GetBytes(@"1"));
            Assert.Equal(1, i);

            int i2 = JsonSerializer.ReadString<int>("2");
            Assert.Equal(2, i2);

            long l = JsonSerializer.Read<long>(Encoding.UTF8.GetBytes(long.MaxValue.ToString()));
            Assert.Equal(long.MaxValue, l);

            long l2 = JsonSerializer.ReadString<long>(long.MaxValue.ToString());
            Assert.Equal(long.MaxValue, l2);

            string s = JsonSerializer.Read<string>(Encoding.UTF8.GetBytes(@"""Hello"""));
            Assert.Equal("Hello", s);

            string s2 = JsonSerializer.ReadString<string>(@"""Hello""");
            Assert.Equal("Hello", s2);
        }

        [Fact]
        public static void PrimitivesFail()
        {
            Assert.Throws<JsonReaderException>(() => JsonSerializer.Read<int>(Encoding.UTF8.GetBytes(@"a")));
            Assert.Throws<JsonReaderException>(() => JsonSerializer.Read<int[]>(Encoding.UTF8.GetBytes(@"[1,a]")));
        }

        [Fact]
        public static void PrimitiveArray()
        {
            int[] i = JsonSerializer.Read<int[]>(Encoding.UTF8.GetBytes(@"[1,2]"));
            Assert.Equal(1, i[0]);
            Assert.Equal(2, i[1]);
        }

        [Fact]
        public static void ArrayWithEnums()
        {
            SampleEnum[] i = JsonSerializer.Read<SampleEnum[]>(Encoding.UTF8.GetBytes(@"[1,2]"));
            Assert.Equal(SampleEnum.One, i[0]);
            Assert.Equal(SampleEnum.Two, i[1]);
        }

        [Fact]
        public static void PrimitiveArrayFail()
        {
            // Invalid data
            Assert.Throws<InvalidOperationException>(() => JsonSerializer.Read<int[]>(Encoding.UTF8.GetBytes(@"[1,""a""]")));

            // Multidimensional arrays currently not supported
            Assert.Throws<InvalidOperationException>(() => JsonSerializer.Read<int[,]>(Encoding.UTF8.GetBytes(@"[[1,2],[3,4]]")));
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

            SimpleTestClass[] i = JsonSerializer.Read<SimpleTestClass[]>(Encoding.UTF8.GetBytes(data));

            i[0].Verify();
            i[1].Verify();
        }

        [Fact]
        public static void PrimitiveJaggedArray()
        {
            int[][] i = JsonSerializer.Read<int[][]>(Encoding.UTF8.GetBytes(@"[[1,2],[3,4]]"));
            Assert.Equal(1, i[0][0]);
            Assert.Equal(2, i[0][1]);
            Assert.Equal(3, i[1][0]);
            Assert.Equal(4, i[1][1]);
        }

        [Fact]
        public static void ListOfList()
        {
            List<List<int>> result = JsonSerializer.Read<List<List<int>>>(Encoding.UTF8.GetBytes(@"[[1,2],[3,4]]"));

            Assert.Equal(1, result[0][0]);
            Assert.Equal(2, result[0][1]);
            Assert.Equal(3, result[1][0]);
            Assert.Equal(4, result[1][1]);
        }

        [Fact]
        public static void ListOfArray()
        {
            List<int[]> result = JsonSerializer.Read<List<int[]>>(Encoding.UTF8.GetBytes(@"[[1,2],[3,4]]"));

            Assert.Equal(1, result[0][0]);
            Assert.Equal(2, result[0][1]);
            Assert.Equal(3, result[1][0]);
            Assert.Equal(4, result[1][1]);
        }

        [Fact]
        public static void ArrayOfList()
        {
            List<int>[] result = JsonSerializer.Read<List<int>[]> (Encoding.UTF8.GetBytes(@"[[1,2],[3,4]]"));

            Assert.Equal(1, result[0][0]);
            Assert.Equal(2, result[0][1]);
            Assert.Equal(3, result[1][0]);
            Assert.Equal(4, result[1][1]);
        }

        [Fact]
        public static void PrimitiveList()
        {
            List<int> i = JsonSerializer.Read<List<int>>(Encoding.UTF8.GetBytes(@"[1,2]"));
            Assert.Equal(1, i[0]);
            Assert.Equal(2, i[1]);
        }
    }
}
