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
                    new object[] { TestCaseType.Json400B, TestJson.Json400B},
                    new object[] { TestCaseType.Json4KB, TestJson.Json4KB},
                    new object[] { TestCaseType.Json40KB, TestJson.Json40KB},
                    new object[] { TestCaseType.Json400KB, TestJson.Json400KB}
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
            Json400B,
            Json4KB,
            Json40KB,
            Json400KB,
        }

        // TestCaseType is only used to give the json strings a descriptive name.
        [Theory]
        [MemberData(nameof(TestCases))]
        public static void TestJsonReaderUtf8(TestCaseType type, string jsonString)
        {
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);
            byte[] result = JsonLabReturnBytesHelper(dataUtf8, SymbolTable.InvariantUtf8, out int length);
            string actualStr = Encoding.UTF8.GetString(result.AsSpan(0, length));

            Stream stream = new MemoryStream(dataUtf8);
            TextReader reader = new StreamReader(stream, Encoding.UTF8, false, 1024, true);
            string expectedStr = NewtonsoftReturnStringHelper(reader);

            Assert.Equal(actualStr, expectedStr);

            long memoryBefore = GC.GetAllocatedBytesForCurrentThread();
            JsonLabEmptyLoopHelper(dataUtf8, SymbolTable.InvariantUtf8);
            long memoryAfter = GC.GetAllocatedBytesForCurrentThread();
            Assert.Equal(0, memoryAfter - memoryBefore);
        }

        // TestCaseType is only used to give the json strings a descriptive name.
        [Theory]
        [MemberData(nameof(TestCases))]
        public static void TestJsonReaderUtf16(TestCaseType type, string jsonString)
        {
            byte[] dataUtf16 = Encoding.Unicode.GetBytes(jsonString);
            byte[] result = JsonLabReturnBytesHelper(dataUtf16, SymbolTable.InvariantUtf16, out int length, 2);
            string actualStr = Encoding.Unicode.GetString(result.AsSpan(0, length));

            TextReader reader = new StringReader(jsonString);
            string expectedStr = NewtonsoftReturnStringHelper(reader);

            Assert.Equal(actualStr, expectedStr);

            long memoryBefore = GC.GetAllocatedBytesForCurrentThread();
            JsonLabEmptyLoopHelper(dataUtf16, SymbolTable.InvariantUtf16);
            long memoryAfter = GC.GetAllocatedBytesForCurrentThread();
            Assert.Equal(0, memoryAfter - memoryBefore);
        }

        private static void JsonLabEmptyLoopHelper(byte[] data, SymbolTable symbolTable)
        {
            var json = new JsonReader(data, symbolTable);
            while (json.Read())
            {
                JsonTokenType tokenType = json.TokenType;
                switch (tokenType)
                {
                    case JsonTokenType.StartObject:
                    case JsonTokenType.EndObject:
                    case JsonTokenType.StartArray:
                    case JsonTokenType.EndArray:
                        break;
                    case JsonTokenType.PropertyName:
                        break;
                    case JsonTokenType.Value:
                        JsonValueType valueType = json.ValueType;
                        switch (valueType)
                        {
                            case JsonValueType.Unknown:
                                break;
                            case JsonValueType.Object:
                                break;
                            case JsonValueType.Array:
                                break;
                            case JsonValueType.Number:
                                break;
                            case JsonValueType.String:
                                break;
                            case JsonValueType.True:
                                break;
                            case JsonValueType.False:
                                break;
                            case JsonValueType.Null:
                                break;
                            case JsonValueType.Undefined:
                                break;
                            case JsonValueType.NaN:
                                break;
                            case JsonValueType.Infinity:
                                break;
                            case JsonValueType.NegativeInfinity:
                                break;
                        }
                        break;
                    case JsonTokenType.None:
                        break;
                    case JsonTokenType.Comment:
                        break;
                    case JsonTokenType.Null:
                        break;
                    case JsonTokenType.Undefined:
                        break;
                    default:
                        throw new ArgumentException();
                }
            }
        }

        private static byte[] JsonLabReturnBytesHelper(byte[] data, SymbolTable symbolTable, out int length, int utf16Multiplier = 1)
        {
            byte[] outputArray = new byte[data.Length];

            Span<byte> destination = outputArray;
            var json = new JsonReader(data, symbolTable);
            while (json.Read())
            {
                JsonTokenType tokenType = json.TokenType;
                ReadOnlySpan<byte> valueSpan = json.Value;
                switch (tokenType)
                {
                    case JsonTokenType.PropertyName:
                        valueSpan.CopyTo(destination);
                        destination[valueSpan.Length] = (byte)',';
                        destination[valueSpan.Length + 1 * utf16Multiplier] = (byte)' ';
                        destination = destination.Slice(valueSpan.Length + 2 * utf16Multiplier);
                        break;
                    case JsonTokenType.Value:
                        JsonValueType valueType = json.ValueType;

                        switch (valueType)
                        {
                            case JsonValueType.True:
                                destination[0] = (byte)'T';
                                destination[1 * utf16Multiplier] = (byte)'r';
                                destination[2 * utf16Multiplier] = (byte)'u';
                                destination[3 * utf16Multiplier] = (byte)'e';
                                destination = destination.Slice(4 * utf16Multiplier);
                                break;
                            case JsonValueType.False:
                                destination[0] = (byte)'F';
                                destination[1 * utf16Multiplier] = (byte)'a';
                                destination[2 * utf16Multiplier] = (byte)'l';
                                destination[3 * utf16Multiplier] = (byte)'s';
                                destination[4 * utf16Multiplier] = (byte)'e';
                                destination = destination.Slice(5 * utf16Multiplier);
                                break;
                        }

                        valueSpan.CopyTo(destination);
                        destination[valueSpan.Length] = (byte)',';
                        destination[valueSpan.Length + 1 * utf16Multiplier] = (byte)' ';
                        destination = destination.Slice(valueSpan.Length + 2 * utf16Multiplier);
                        break;
                    default:
                        break;
                }
            }
            length = outputArray.Length - destination.Length;
            return outputArray;
        }

        private static string NewtonsoftReturnStringHelper(TextReader reader)
        {
            StringBuilder sb = new StringBuilder();
            var json = new Newtonsoft.Json.JsonTextReader(reader);
            while (json.Read())
            {
                if (json.Value != null)
                {
                    sb.Append(json.Value).Append(", ");
                }
            }

            return sb.ToString();
        }
    }
}
