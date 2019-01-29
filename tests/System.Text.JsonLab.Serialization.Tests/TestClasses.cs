// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System.Text.Json.Serialization.Tests
{
    public enum SampleEnum
    {
        One = 1,
        Two = 2
    }
    public class SimpleTestClass
    {
        public short MyInt16 { get; set; }
        public int MyInt32 { get; set; }
        public long MyInt64 { get; set; }
        public ushort MyUInt16 { get; set; }
        public uint MyUInt32 { get; set; }
        public ulong MyUInt64 { get; set; }
        public byte MyByte { get; set; }
        public char MyChar { get; set; }
        public string MyString { get; set; }
        public decimal MyDecimal { get; set; }
        public bool MyBooleanTrue { get; set; }
        public bool MyBooleanFalse { get; set; }
        public float MySingle { get; set; }
        public double MyDouble { get; set; }
        public DateTime MyDateTime { get; set; }
        public SampleEnum MyEnum { get; set; }

        public static readonly string s_json =
                @"{" +
                @"""MyInt16"" : 1," +
                @"""MyInt32"" : 2," +
                @"""MyInt64"" : 3," +
                @"""MyUInt16"" : 4," +
                @"""MyUInt32"" : 5," +
                @"""MyUInt64"" : 6," +
                @"""MyByte"" : 7," +
                @"""MyChar"" : ""a""," +
                @"""MyString"" : ""Hello""," +
                @"""MyBooleanTrue"" : true," +
                @"""MyBooleanFalse"" : false," +
                @"""MySingle"" : 1.1," +
                @"""MyDouble"" : 2.2," +
                @"""MyDecimal"" : 3.3," +
                @"""MyDateTime"" : ""2019-01-30T12:01:02.0000000Z""," +
                @"""MyEnum"" : 2" + // int by default
                @"}";

        public static readonly byte[] s_data = Encoding.UTF8.GetBytes(s_json);

        public void Verify()
        {
            Assert.Equal(MyInt16, (short)1);
            Assert.Equal(MyInt32, (int)2);
            Assert.Equal(MyInt64, (long)3);
            Assert.Equal(MyUInt16, (ushort)4);
            Assert.Equal(MyUInt32, (uint)5);
            Assert.Equal(MyUInt64, (ulong)6);
            Assert.Equal(MyByte, (byte)7);
            Assert.Equal(MyChar, 'a');
            Assert.Equal(MyString, "Hello");
            Assert.Equal(MyDecimal, 3.3m);
            Assert.Equal(MyBooleanFalse, false);
            Assert.Equal(MyBooleanTrue, true);
            Assert.Equal(MySingle, 1.1f);
            Assert.Equal(MyDouble, 2.2d);
            Assert.Equal(MyDateTime, new DateTime(2019, 1, 30, 12, 1, 2, DateTimeKind.Utc));
            Assert.Equal(MyEnum, SampleEnum.Two);
        }
    }
}
