// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Xunit;

namespace System.Text.Json.Serialization.Tests
{
    public enum SampleEnum
    {
        Foo = 1,
        Bar = 2
    }

    public class SimpleTestClass
    {
        public Int16 MyInt16 { get; set; }
        public Int32 MyInt32 { get; set; }
        public Int64 MyInt64 { get; set; }
        public String MyString { get; set; }
        public Decimal MyDecimal { get; set; }
        public Boolean MyBooleanTrue { get; set; }
        public Boolean MyBooleanFalse { get; set; }
        public Single MySingle { get; set; }
        public Double MyDouble { get; set; }
        public DateTime MyDateTime { get; set; }
        public SampleEnum MyEnum { get; set; }

        public static readonly string s_json =
                @"{" +
                @"""MyInt16"" : 1," +
                @"""MyInt32"" : 2," +
                @"""MyInt64"" : 3," +
                @"""MyString"" : ""Hello""," +
                @"""MyBooleanTrue"" : true," +
                @"""MyBooleanFalse"" : false," +
                @"""MySingle"" : 1.1," +
                @"""MyDouble"" : 2.2," +
                @"""MyDecimal"" : 3.3," +
                @"""MyDateTime"" : ""2019-01-30T12:01:02.0000000Z""," +
                @"""MyEnum"" : ""2""" +
                @"}";

        public static readonly byte[] s_data = Encoding.UTF8.GetBytes(s_json);

        public void Verify()
        {
            Assert.Equal(MyInt16, 1);
            Assert.Equal(MyInt32, 2);
            Assert.Equal(MyInt64, 3);
            Assert.Equal(MyString, "Hello");
            Assert.Equal(MyDecimal, 3.3m);
            Assert.Equal(MyBooleanFalse, false);
            Assert.Equal(MyBooleanTrue, true);
            Assert.Equal(MySingle, 1.1f);
            Assert.Equal(MyDouble, 2.2d);
            Assert.Equal(MyDateTime, new DateTime(2019, 1, 30, 12, 1, 2, DateTimeKind.Utc));
        }
    }
}
