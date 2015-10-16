using System.Globalization;
using System.Text.Utf8;
using Xunit;

namespace System.Text.Json.Tests
{
    public class JsonReaderTests
    {
        [Fact]
        public void Test1ReadJsonUtf8()
        {
            Console.WriteLine(TestJson1);
            var str = new Utf8String(TestJson1);
            var jsonReader = new JsonReader(str);
            var actualJson = Read(jsonReader);
            Console.WriteLine(actualJson);
            Assert.Equal(ExpectedJson1, actualJson);
        }

        [Fact]
        public void Test2ReadJsonUtf8()
        {
            Console.WriteLine(TestJson2);
            var str = new Utf8String(TestJson2);
            var jsonReader = new JsonReader(str);
            var actualJson = Read(jsonReader);
            Console.WriteLine(actualJson);
            Assert.Equal(ExpectedJson2, actualJson);
        }

        [Fact]
        public void Test3ReadJsonUtf8()
        {
            Console.WriteLine(TestJson3);
            var str = new Utf8String(TestJson3);
            var jsonReader = new JsonReader(str);
            var actualJson = Read(jsonReader);
            Console.WriteLine(actualJson);
            Assert.Equal(ExpectedJson3, actualJson);
        }

        private const string TestJson1 =
            "{\"age\":30,\"first\":\"John\",\"last\":\"Smith\",\"phoneNumbers\":[\"425-000-1212\",\"425-000-1213\"],\"address\":{\"street\":\"1MicrosoftWay\",\"city\":\"Redmond\",\"zip\":98052},\"IDs\":[0425,-70,9223372036854775807]}";

        private const string ExpectedJson1 =
            "{\"age\":30,\"first\":\"John\",\"last\":\"Smith\",\"phoneNumbers\":[\"425-000-1212\",\"425-000-1213\"],\"address\":{\"street\":\"1MicrosoftWay\",\"city\":\"Redmond\",\"zip\":98052},\"IDs\":[425,-70,9.22337203685478E+18]}";

        private const string TestJson2 =
            "{\"string\":\"string\",\"number\":5,\"decimal\":3516512.13512,\"long\":9223372036854776000.1200,\"notLong\":922854776000.1200,\"boolean\":false,\"object\":{},\"array\":[],\"null\":null,\"emptyArray\":[],\"emptyObject\":{},\"arrayString\":[\"alpha\",\"beta\"],\"arrayNum\":[1,212512.01,3.00],\"arrayBool\":[false,true,true],\"arrayNull\":[null,null],\"arrayObject\":[{\"firstName\":\"name1\",\"lastName\":\"name\"},{\"firstName\":\"name1\",\"lastName\":\"name\"},{\"firstName\":\"name2\",\"lastName\":\"name\"},{\"firstName\":\"name3\",\"lastName\":\"name1\"}],\"arrayArray\":[[null,false,5,\"-0215.512501\",9223372036854776000],[{},true,null,125651,\"simple\"],[{\"field\":null},\"hi\"]]}";

        private const string ExpectedJson2 =
            "{\"string\":\"string\",\"number\":5,\"decimal\":3516512.13512,\"long\":9.22337203685478E+18,\"notLong\":922854776000.12,\"boolean\":false,\"object\":{},\"array\":[],\"null\":null,\"emptyArray\":[],\"emptyObject\":{},\"arrayString\":[\"alpha\",\"beta\"],\"arrayNum\":[1,212512.01,3],\"arrayBool\":[false,true,true],\"arrayNull\":[null,null],\"arrayObject\":[{\"firstName\":\"name1\",\"lastName\":\"name\"},{\"firstName\":\"name1\",\"lastName\":\"name\"},{\"firstName\":\"name2\",\"lastName\":\"name\"},{\"firstName\":\"name3\",\"lastName\":\"name1\"}],\"arrayArray\":[[null,false,5,\"-0215.512501\",9.22337203685478E+18],[{},true,null,125651,\"simple\"],[{\"field\":null},\"hi\"]]}";

        private const string TestJson3 =
            "{\"age\":30,\"first\":\"John\",\"last\":\"Smith\",\"phoneNumbers\":[\"425-000-1212\",\"425-000-1213\"],\"address\":{\"street\":\"1MicrosoftWay\",\"city\":\"Redmond\",\"zip\":98052},\"IDs\":[425,-70,9223372036854776000],\"arrayWithObjects\":[\"text\",14,[],null,false,{},{\"time\":24},[\"1\",\"2\",\"3\"]],\"boolean\":false,\"null\":null,\"objectName\":{\"group\":{\"array\":[false],\"field\":\"simple\",\"anotherFieldNum\":5,\"anotherFieldBool\":true,\"lastField\":null}},\"emptyObject\":{}}";

        private const string ExpectedJson3 =
            "{\"age\":30,\"first\":\"John\",\"last\":\"Smith\",\"phoneNumbers\":[\"425-000-1212\",\"425-000-1213\"],\"address\":{\"street\":\"1MicrosoftWay\",\"city\":\"Redmond\",\"zip\":98052},\"IDs\":[425,-70,9.22337203685478E+18],\"arrayWithObjects\":[\"text\",14,[],null,false,{},{\"time\":24},[\"1\",\"2\",\"3\"]],\"boolean\":false,\"null\":null,\"objectName\":{\"group\":{\"array\":[false],\"field\":\"simple\",\"anotherFieldNum\":5,\"anotherFieldBool\":true,\"lastField\":null}},\"emptyObject\":{}}";

        private static string Read(JsonReader json)
        {
            var jsonOutput = new StringBuilder();
            while (json.Read())
            {
                Utf8String tempString;
                switch (json.TokenType)
                {
                    case JsonReader.JsonTokenType.ObjectStart:
                        tempString = json.ReadObjectStart();
                        jsonOutput.Append(tempString);
                        break;
                    case JsonReader.JsonTokenType.ObjectEnd:
                        tempString = json.ReadObjectEnd();
                        jsonOutput.Append(tempString);
                        break;
                    case JsonReader.JsonTokenType.ArrayStart:
                        tempString = json.ReadArrayStart();
                        jsonOutput.Append(tempString);
                        break;
                    case JsonReader.JsonTokenType.ArrayEnd:
                        tempString = json.ReadArrayEnd();
                        jsonOutput.Append(tempString);
                        break;
                    case JsonReader.JsonTokenType.String:
                        tempString = json.ReadString();
                        jsonOutput.Append("\"" + tempString + "\"");
                        break;
                    case JsonReader.JsonTokenType.Colon:
                        tempString = json.ReadColon();
                        jsonOutput.Append(tempString);
                        break;
                    case JsonReader.JsonTokenType.Comma:
                        tempString = json.ReadComma();
                        jsonOutput.Append(tempString);
                        break;
                    case JsonReader.JsonTokenType.Value:
                        json.ReadValue();
                        break;
                    case JsonReader.JsonTokenType.True:
                        tempString = new Utf8String(json.ReadTrue().ToString().ToLower());
                        jsonOutput.Append(tempString);
                        break;
                    case JsonReader.JsonTokenType.False:
                        tempString = new Utf8String(json.ReadFalse().ToString().ToLower());
                        jsonOutput.Append(tempString);
                        break;
                    case JsonReader.JsonTokenType.Null:
                        tempString = new Utf8String("null");
                        json.ReadNull();
                        jsonOutput.Append(tempString);
                        break;
                    case JsonReader.JsonTokenType.Number:
                        tempString = new Utf8String(json.ReadNumber().ToString(CultureInfo.InvariantCulture));
                        jsonOutput.Append(tempString);
                        break;
                    case JsonReader.JsonTokenType.Start:
                        json.ReadStart();
                        break;
                    case JsonReader.JsonTokenType.Finish:
                        json.ReadFinish();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return jsonOutput.ToString();
        }
    }
}
