using System.Collections.Generic;
using System.Linq;
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
            var result = Read(jsonReader);
            var age = new Utf8String("age");
            var balance = new Utf8String("balance");
            var first = new Utf8String("first");
            var last = new Utf8String("last");
            var phoneNumbers = new Utf8String("phoneNumbers");
            var street = new Utf8String("street");
            var city = new Utf8String("city");
            var zip = new Utf8String("zip");
            var ids = new Utf8String("IDs");

            uint actualAge = 0;
            foreach (var t in result.Where(t => t.Key == age))
            {
                actualAge = Convert.ToUInt32(t.Value);
            }

            var actualBalance = 0;
            foreach (var t in result.Where(t => t.Key == balance))
            {
                actualBalance = Convert.ToInt32(t.Value);
            }

            var actualFirstName = new Utf8String();
            foreach (var t in result.Where(t => t.Key == first))
            {
                actualFirstName = new Utf8String(t.Value.ToString());
            }

            var actualLastName = new Utf8String();
            foreach (var t in result.Where(t => t.Key == last))
            {
                actualLastName = new Utf8String(t.Value.ToString());
            }

            var actualStreet = new Utf8String();
            foreach (var t in result.Where(t => t.Key == street))
            {
                actualStreet = new Utf8String(t.Value.ToString());
            }

            var actualCity = new Utf8String();
            foreach (var t in result.Where(t => t.Key == city))
            {
                actualCity = new Utf8String(t.Value.ToString());
            }

            var actualZip = new Utf8String();
            foreach (var t in result.Where(t => t.Key == zip))
            {
                actualZip = new Utf8String(t.Value.ToString());
            }

            var actualPhoneList = new List<Utf8String>();
            foreach (var t in result.Where(t => t.Key == phoneNumbers))
            {
                actualPhoneList.Add(new Utf8String(t.Value.ToString()));
            }

            var actualIDs = new List<object>();
            foreach (var t in result.Where(t => t.Key == ids))
            {
                actualIDs.Add(t.Value);
            }

            Assert.Equal(ExpectedAge, actualAge);
            Assert.Equal(ExpectedBalance, actualBalance);
            Assert.Equal(_expectedFullName, new Utf8String(actualFirstName + " " + actualLastName));
            Assert.Equal(ExpectedPhone, actualPhoneList.ToArray());
            Assert.Equal(_expectedAddress, new Utf8String(actualStreet + ", " + actualCity + ", " + actualZip));
            Assert.Equal(ExpectedId1, Convert.ToUInt32(actualIDs[0]));
            Assert.Equal(ExpectedId2, Convert.ToInt32(actualIDs[1]));
            Assert.Equal(ExpectedId3, Convert.ToInt64(actualIDs[2]));
        }

        private const string Json =
            "{\"age\":30,\"balance\":-1000,\"first\":\"John\",\"last\":\"Smith\",\"phoneNumbers\":[\"425-000-1212\",\"425-000-1213\"],\"address\":{\"street\":\"1 Microsoft Way\",\"city\":\"Redmond\",\"zip\":98052}\"IDs\" \n : [ \t 0425 \n , -70 \n, 9223372036854775807 ] \t }";

        private const uint ExpectedAge = 30;
        private const int ExpectedBalance = -1000;
        private const uint ExpectedId1 = 425;
        private const int ExpectedId2 = -70;
        private const long ExpectedId3 = 9223372036854775807;
        private readonly Utf8String _expectedFullName = new Utf8String("John Smith");

        private static readonly Utf8String[] ExpectedPhone =
        {
            new Utf8String("425-000-1212"),
            new Utf8String("425-000-1213")
        };

        private readonly Utf8String _expectedAddress = new Utf8String("1 Microsoft Way, Redmond, 98052");

        private static List<KeyValuePair<Utf8String, object>> Read(JsonReader json)
        {
            var jsonOutput = new List<KeyValuePair<Utf8String, object>>();
            var property = new Utf8String("");
            while (json.Read())
            {
                object value;
                switch (json.TokenType)
                {
                    case JsonReader.JsonTokenType.ObjectStart:
                        json.ReadObjectStart();
                        value = null;
                        break;
                    case JsonReader.JsonTokenType.ObjectEnd:
                        json.ReadObjectEnd();
                        property = new Utf8String("");
                        value = null;
                        break;
                    case JsonReader.JsonTokenType.ArrayStart:
                        json.ReadArrayStart();
                        value = null;
                        break;
                    case JsonReader.JsonTokenType.ArrayEnd:
                        json.ReadArrayEnd();
                        property = new Utf8String("");
                        value = null;
                        break;
                    case JsonReader.JsonTokenType.PropertyName:
                        property = json.ReadPropertyAsString();
                        value = null;
                        break;
                    case JsonReader.JsonTokenType.PropertyValueAsString:
                        value = json.ReadPropertyAsString();
                        break;
                    case JsonReader.JsonTokenType.PropertyValueAsInt:
                        value = json.ReadPropertyValueAsInt();
                        break;
                    default:
                        property = new Utf8String("");
                        value = null;
                        break;
                }
                if (property != new Utf8String("") && value != null)
                {
                    jsonOutput.Add(new KeyValuePair<Utf8String, object>(property, value));
                }
            }

            return jsonOutput;
        }
    }
}
