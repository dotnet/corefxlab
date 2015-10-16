using System.Globalization;
using System.Text.Utf8;
using Xunit;

namespace System.Text.Json.Tests
{
    public class JsonReaderTests
    {
        [Fact]
        public void ReadJsonUtf8()
        {
            Console.WriteLine(Json);
            var str = new Utf8String(Json);
            var jsonReader = new JsonReader(str);
            var actualJson = Read(jsonReader);
            Console.WriteLine(actualJson);
            Assert.Equal(ExpectedJson, actualJson);
        }

        private const string Json =
            "{\"age\":30,\"balance\":-1000,\"first\":\"John\",\"last\":\"Smith\",\"phoneNumbers\":[\"425-000-1212\",\"425-000-1213\"],\"address\":{\"street\":\"1 Microsoft Way\",\"city\":\"Redmond\",\"zip\":98052}\"IDs\" \n : [ \t 0425 \n , -70 \n, 9223372036854775807 ] \t }";

        private const string ExpectedJson =
            "{\"age\":30,\"balance\":-1000,\"first\":\"John\",\"last\":\"Smith\",\"phoneNumbers\":[\"425-000-1212\",\"425-000-1213\"],\"address\":{\"street\":\"1 Microsoft Way\",\"city\":\"Redmond\",\"zip\":98052}\"IDs\":[425,-70,9.22337203685478E+18]}";

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
                        Console.WriteLine(JsonReader.JsonTokenType.ObjectStart + " - {0}", tempString);
                        jsonOutput.Append(tempString);
                        break;
                    case JsonReader.JsonTokenType.ObjectEnd:
                        tempString = json.ReadObjectEnd();
                        Console.WriteLine(JsonReader.JsonTokenType.ObjectEnd + " - {0}", tempString);
                        jsonOutput.Append(tempString);
                        break;
                    case JsonReader.JsonTokenType.ArrayStart:
                        tempString = json.ReadArrayStart();
                        Console.WriteLine(JsonReader.JsonTokenType.ArrayStart + " - {0}", tempString);
                        jsonOutput.Append(tempString);
                        break;
                    case JsonReader.JsonTokenType.ArrayEnd:
                        tempString = json.ReadArrayEnd();
                        Console.WriteLine(JsonReader.JsonTokenType.ArrayEnd + " - {0}", tempString);
                        jsonOutput.Append(tempString);
                        break;
                    case JsonReader.JsonTokenType.String:
                        tempString = json.ReadString();
                        Console.WriteLine(JsonReader.JsonTokenType.String + " - {0}", tempString);
                        jsonOutput.Append("\"" + tempString + "\"");
                        break;
                    case JsonReader.JsonTokenType.Colon:
                        tempString = json.ReadColon();
                        Console.WriteLine(JsonReader.JsonTokenType.Colon + " - {0}", tempString);
                        jsonOutput.Append(tempString);
                        break;
                    case JsonReader.JsonTokenType.Comma:
                        tempString = json.ReadComma();
                        Console.WriteLine(JsonReader.JsonTokenType.Comma + " - {0}", tempString);
                        jsonOutput.Append(tempString);
                        break;
                    case JsonReader.JsonTokenType.Value:
                        Console.WriteLine(JsonReader.JsonTokenType.Value + " - {0}", json.ReadValue());
                        break;
                    case JsonReader.JsonTokenType.True:
                        tempString = new Utf8String(json.ReadTrue().ToString());
                        Console.WriteLine(JsonReader.JsonTokenType.True + " - {0}", tempString);
                        jsonOutput.Append(tempString);
                        break;
                    case JsonReader.JsonTokenType.False:
                        tempString = new Utf8String(json.ReadFalse().ToString());
                        Console.WriteLine(JsonReader.JsonTokenType.False + " - {0}", tempString);
                        jsonOutput.Append(tempString);
                        break;
                    case JsonReader.JsonTokenType.Null:
                        tempString = new Utf8String("null");
                        Console.WriteLine(JsonReader.JsonTokenType.Null + " - {0}", json.ReadNull());
                        jsonOutput.Append(tempString);
                        break;
                    case JsonReader.JsonTokenType.Number:
                        tempString = new Utf8String(json.ReadNumber().ToString(CultureInfo.InvariantCulture));
                        Console.WriteLine(JsonReader.JsonTokenType.Number + " - {0}", tempString);
                        jsonOutput.Append(tempString);
                        break;
                    case JsonReader.JsonTokenType.Start:
                        Console.WriteLine(JsonReader.JsonTokenType.Start + " - {0}", json.ReadStart());
                        break;
                    case JsonReader.JsonTokenType.Finish:
                        Console.WriteLine(JsonReader.JsonTokenType.Finish + " - {0}", json.ReadFinish());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return jsonOutput.ToString();
        }
    }
}
