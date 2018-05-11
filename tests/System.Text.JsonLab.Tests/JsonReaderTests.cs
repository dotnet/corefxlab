// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;
using System.Text.JsonLab.Tests.Resources;
using Xunit;

namespace System.Text.JsonLab.Tests
{
    public class JsonReaderTests
    {
        public static IEnumerable<object[]> TestCases
        {
            get
            {
                return new List<object[]>
                {
                    new object[] { TestCaseType.Basic, TestJson.BasicJson},
                    new object[] { TestCaseType.BasicLargeNum, TestJson.BasicJsonWithLargeNum}, // Json.NET treats numbers starting with 0 as octal (0425 becomes 277)
                    new object[] { TestCaseType.BroadTree, TestJson.BroadTree}, // \r\n behavior is different between Json.NET and JsonLab
                    new object[] { TestCaseType.DeepTree, TestJson.DeepTree},
                    //new object[] { TestCaseType.FullSchema1, TestJson.FullJsonSchema1},   // Behavior of null values is different between Json.NET and JsonLab
                    //new object[] { TestCaseType.FullSchema2, TestJson.FullJsonSchema2},   // Behavior of null values is different between Json.NET and JsonLab
                    new object[] { TestCaseType.HelloWorld, TestJson.HelloWorld},
                    new object[] { TestCaseType.LotsOfNumbers, TestJson.LotsOfNumbers},
                    new object[] { TestCaseType.LotsOfStrings, TestJson.LotsOfStrings},
                    new object[] { TestCaseType.ProjectLockJson, TestJson.ProjectLockJson},
                    //new object[] { TestCaseType.SpecialStrings, TestJson.JsonWithSpecialStrings},    // JsonLab doesn't support this yet.
                    //new object[] { TestCaseType.SpecialNumForm, TestJson.JsonWithSpecialNumFormat},    // Behavior of E-notation is different between Json.NET and JsonLab
                    new object[] { TestCaseType.Json300B, TestJson.Json300B},
                    new object[] { TestCaseType.Json3KB, TestJson.Json3KB},
                    new object[] { TestCaseType.Json30KB, TestJson.Json30KB},
                    new object[] { TestCaseType.Json300KB, TestJson.Json300KB},
                    new object[] { TestCaseType.Json3MB, TestJson.Json3MB}
                };
            }
        }

        public enum TestCaseType
        {
            HelloWorld,
            Basic,
            BasicLargeNum,
            SpecialNumForm,
            SpecialStrings,
            ProjectLockJson,
            FullSchema1,
            FullSchema2,
            DeepTree,
            BroadTree,
            LotsOfNumbers,
            LotsOfStrings,
            Json300B,
            Json3KB,
            Json30KB,
            Json300KB,
            Json3MB,
        }

        // TestCaseType is only used to give the json strings a descriptive name.
        [Theory]
        [MemberData(nameof(TestCases))]
        public static void TestJsonReaderUtf8(TestCaseType type, string jsonString)
        {
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);
            byte[] result = ReaderSystemTextJsonLab(dataUtf8, out int length);
            string str = Encoding.UTF8.GetString(result.AsSpan(0, length));
            string newtonsoftStr = ReaderNewtonsoftReader(jsonString);

            Assert.Equal(str, newtonsoftStr);
        }

        // TestCaseType is only used to give the json strings a descriptive name.
        [Theory]
        [MemberData(nameof(TestCases))]
        public static void TestJsonReaderUtf16(TestCaseType type, string jsonString)
        {
            byte[] dataUtf16 = Encoding.Unicode.GetBytes(jsonString);
            byte[] result = ReaderSystemTextJsonLabUtf16(dataUtf16, out int length);
            string str = Encoding.Unicode.GetString(result.AsSpan(0, length));
            string newtonsoftStr = ReaderNewtonsoftReaderUtf16(jsonString);

            Assert.Equal(str, newtonsoftStr);
        }

        private static byte[] ReaderSystemTextJsonLab(byte[] dataUtf8, out int length)
        {
            byte[] outputArray = new byte[dataUtf8.Length];
            Span<byte> destination = outputArray;

            var json = new JsonReader(dataUtf8, SymbolTable.InvariantUtf8);
            while (json.Read())
            {
                var tokenType = json.TokenType;

                switch (tokenType)
                {
                    case JsonTokenType.PropertyName:
                        json.Value.CopyTo(destination);
                        destination[json.Value.Length] = (byte)',';
                        destination[json.Value.Length + 1] = (byte)' ';
                        destination = destination.Slice(json.Value.Length + 2);
                        break;
                    case JsonTokenType.Value:
                        var valueType = json.ValueType;

                        switch (valueType)
                        {
                            case JsonValueType.True:
                                destination[0] = (byte)'T';
                                destination[1] = (byte)'r';
                                destination[2] = (byte)'u';
                                destination[3] = (byte)'e';
                                destination = destination.Slice(4);
                                break;
                            case JsonValueType.False:
                                destination[0] = (byte)'F';
                                destination[1] = (byte)'a';
                                destination[2] = (byte)'l';
                                destination[3] = (byte)'s';
                                destination[4] = (byte)'e';
                                destination = destination.Slice(5);
                                break;
                        }

                        json.Value.CopyTo(destination);
                        destination[json.Value.Length] = (byte)',';
                        destination[json.Value.Length + 1] = (byte)' ';
                        destination = destination.Slice(json.Value.Length + 2);
                        break;
                    default:
                        break;
                }
            }
            length = outputArray.Length - destination.Length;
            return outputArray;
        }

        private static byte[] ReaderSystemTextJsonLabUtf16(byte[] dataUtf16, out int length)
        {
            byte[] outputArray = new byte[dataUtf16.Length];
            Span<byte> destination = outputArray;

            var json = new JsonReader(dataUtf16, SymbolTable.InvariantUtf16);
            while (json.Read())
            {
                var tokenType = json.TokenType;

                switch (tokenType)
                {
                    case JsonTokenType.PropertyName:
                        json.Value.CopyTo(destination);
                        destination[json.Value.Length] = (byte)',';
                        destination[json.Value.Length + 2] = (byte)' ';
                        destination = destination.Slice(json.Value.Length + 4);
                        break;
                    case JsonTokenType.Value:
                        var valueType = json.ValueType;

                        switch (valueType)
                        {
                            case JsonValueType.True:
                                destination[0] = (byte)'T';
                                destination[2] = (byte)'r';
                                destination[4] = (byte)'u';
                                destination[6] = (byte)'e';
                                destination = destination.Slice(8);
                                break;
                            case JsonValueType.False:
                                destination[0] = (byte)'F';
                                destination[2] = (byte)'a';
                                destination[4] = (byte)'l';
                                destination[6] = (byte)'s';
                                destination[8] = (byte)'e';
                                destination = destination.Slice(10);
                                break;
                        }

                        json.Value.CopyTo(destination);
                        destination[json.Value.Length] = (byte)',';
                        destination[json.Value.Length + 2] = (byte)' ';
                        destination = destination.Slice(json.Value.Length + 4);
                        break;
                    default:
                        break;
                }
            }
            length = outputArray.Length - destination.Length;
            return outputArray;
        }

        private static string ReaderNewtonsoftReader(string str)
        {
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(str);
            MemoryStream stream = new MemoryStream(dataUtf8);
            StreamReader reader = new StreamReader(stream, Encoding.UTF8, false, 1024, true);

            StringBuilder sb = new StringBuilder();
            stream.Seek(0, SeekOrigin.Begin);
            using (var json = new Newtonsoft.Json.JsonTextReader(reader))
                while (json.Read())
                {
                    if (json.Value != null)
                    {
                        sb.Append(json.Value + ", ");
                    }
                }
            return sb.ToString();
        }

        private static string ReaderNewtonsoftReaderUtf16(string str)
        {
            StringBuilder sb = new StringBuilder();
            using (var json = new Newtonsoft.Json.JsonTextReader(new StringReader(str)))
                while (json.Read())
                {
                    if (json.Value != null)
                    {
                        sb.Append(json.Value + ", ");
                    }
                }
            return sb.ToString();
        }
    }
}
