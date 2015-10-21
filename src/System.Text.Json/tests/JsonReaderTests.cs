using System.Collections.Generic;
using System.Text.Utf8;
using Xunit;

namespace System.Text.Json.Tests
{
    public class JsonReaderTests
    {
        [Fact]
        public void ReadBasicJson()
        {
            var testJson = CreateJson();
            const string expectedTestJson =
                "{\"age\":30,\"first\":\"John\",\"last\":\"Smith\",\"phoneNumbers\":[\"425-000-1212\",\"425-000-1213\"],\"address\":{\"street\":\"1 Microsoft Way\",\"city\":\"Redmond\",\"zip\":98052}}";
            Assert.Equal(testJson.ToString(), expectedTestJson);

            const string jsonString =
                "{\n   \"age\" : 30,\n   \"first\" : \"John\",\n   \"last\" : \"Smith\",\n   \"phoneNumbers\" " +
                ": [\n      \"425-000-1212\",\n      \"425-000-1213\"\n   ],\n   \"address\" : {\n      \"street\" : " +
                "\"1 Microsoft Way\",\n      \"city\" : \"Redmond\",\n      \"zip\" : 98052\n   }\n}";
            const string expectedJson =
                "{\"age\":30,\"first\":\"John\",\"last\":\"Smith\",\"phoneNumbers\":[\"425-000-1212\",\"425-000-1213\"],\"address\":{\"street\":\"1 Microsoft Way\",\"city\":\"Redmond\",\"zip\":98052}}";
            var readJson = ReadJson(jsonString);
            var json = readJson.ToString();
            Assert.Equal(json, expectedJson);
        }

        [Fact]
        public void ReadBasicJsonWithLongInt()
        {
            const string jsonString =
                "{\"age\":30,\"first\":\"John\",\"last\":\"Smith\",\"phoneNumbers\":[\"425-000-1212\",\"425-000-1213\"],\"address\":{\"street\":\"1MicrosoftWay\",\"city\":\"Redmond\",\"zip\":98052},\"IDs\":[0425,-70,9223372036854775807]}";
            const string expectedJson =
                "{\"age\":30,\"first\":\"John\",\"last\":\"Smith\",\"phoneNumbers\":[\"425-000-1212\",\"425-000-1213\"],\"address\":{\"street\":\"1MicrosoftWay\",\"city\":\"Redmond\",\"zip\":98052},\"IDs\":[425,-70,9.22337203685478E+18]}";
            var readJson = ReadJson(jsonString);
            var json = readJson.ToString();
            Assert.Equal(json, expectedJson);
        }

        [Fact]
        public void ReadFullJsonSchema()
        {
            const string jsonString =
                "{\"age\":30,\"first\":\"John\",\"last\":\"Smith\",\"phoneNumbers\":[\"425-000-1212\",\"425-000-1213\"],\"address\":{\"street\":\"1MicrosoftWay\",\"city\":\"Redmond\",\"zip\":98052},\"IDs\":[425,-70,9223372036854776000],\"arrayWithObjects\":[\"text\",14,[],null,false,{},{\"time\":24},[\"1\",\"2\",\"3\"]],\"boolean\":false,\"null\":null,\"objectName\":{\"group\":{\"array\":[false],\"field\":\"simple\",\"anotherFieldNum\":5,\"anotherFieldBool\":true,\"lastField\":null}},\"emptyObject\":{}}";
            const string expectedJson =
                "{\"age\":30,\"first\":\"John\",\"last\":\"Smith\",\"phoneNumbers\":[\"425-000-1212\",\"425-000-1213\"],\"address\":{\"street\":\"1MicrosoftWay\",\"city\":\"Redmond\",\"zip\":98052},\"IDs\":[425,-70,9.22337203685478E+18],\"arrayWithObjects\":[\"text\",14,[],null,false,{},{\"time\":24},[\"1\",\"2\",\"3\"]],\"boolean\":false,\"null\":null,\"objectName\":{\"group\":{\"array\":[false],\"field\":\"simple\",\"anotherFieldNum\":5,\"anotherFieldBool\":true,\"lastField\":null}},\"emptyObject\":{}}";
            var readJson = ReadJson(jsonString);
            var json = readJson.ToString();
            Assert.Equal(json, expectedJson);
        }

        [Fact]
        public void ReadFullJsonSchemaAndGetValue()
        {
            const string jsonString =
                "{\"string\":\"string\",\"number\":5,\"decimal\":3516512.13512,\"long\":9223372036854776000.1200,\"notLong\":922854776000.1200,\"boolean\":false,\"object\":{},\"array\":[],\"null\":null,\"emptyArray\":[],\"emptyObject\":{},\"arrayString\":[\"alpha\",\"beta\"],\"arrayNum\":[1,212512.01,3.00],\"arrayBool\":[false,true,true],\"arrayNull\":[null,null],\"arrayObject\":[{\"firstName\":\"name1\",\"lastName\":\"name\"},{\"firstName\":\"name1\",\"lastName\":\"name\"},{\"firstName\":\"name2\",\"lastName\":\"name\"},{\"firstName\":\"name3\",\"lastName\":\"name1\"}],\"arrayArray\":[[null,false,5,\"-0215.512501\",9223372036854776000],[{},true,null,125651,\"simple\"],[{\"field\":null},\"hi\"]]}";
            const string expectedJson =
                "{\"string\":\"string\",\"number\":5,\"decimal\":3516512.13512,\"long\":9.22337203685478E+18,\"notLong\":922854776000.12,\"boolean\":false,\"object\":{},\"array\":[],\"null\":null,\"emptyArray\":[],\"emptyObject\":{},\"arrayString\":[\"alpha\",\"beta\"],\"arrayNum\":[1,212512.01,3],\"arrayBool\":[false,true,true],\"arrayNull\":[null,null],\"arrayObject\":[{\"firstName\":\"name1\",\"lastName\":\"name\"},{\"firstName\":\"name1\",\"lastName\":\"name\"},{\"firstName\":\"name2\",\"lastName\":\"name\"},{\"firstName\":\"name3\",\"lastName\":\"name1\"}],\"arrayArray\":[[null,false,5,\"-0215.512501\",9.22337203685478E+18],[{},true,null,125651,\"simple\"],[{\"field\":null},\"hi\"]]}";
            var readJson = ReadJson(jsonString);
            var json = readJson.ToString();
            Assert.Equal(json, expectedJson);

            Assert.Equal(readJson.GetValueFromPropertyName("long")[0].NumberValue, 9.2233720368547758E+18);
            var emptyObject = readJson.GetValueFromPropertyName("emptyObject");
            Assert.Equal(emptyObject[0].ObjectValue.Members[0].Pairs.Count, 0);
            var arrayString = readJson.GetValueFromPropertyName("arrayString");
            Assert.Equal(arrayString[0].ArrayValue.Elements[0].Values.Count, 2);
            Assert.Equal(readJson.GetValueFromPropertyName("firstName").Count, 4);
            Assert.Equal(readJson.GetValueFromPropertyName("propertyDNE").Count, 0);
        }

        [Fact]
        public void ReadJsonSpecialStrings()
        {
            const string jsonString =
                "{\"Here is a string: \\\"\\\"\":\"Here is a hex value -\\u024A\",\"Here is a back slash\\\\\":[\"Multiline\n String \n\",\"\tMul\r\ntiline\r String\",\"\\\"somequote\\\"\tMu\\\"\\\"l\r\ntiline\r\\\"another\\\" String\\\\\"],\"str\":\"\\\"\\\"\"}";
            var readJson = ReadJson(jsonString);
            var json = readJson.ToString();
            Assert.Equal(json, jsonString);
        }

        [Fact]
        public void ReadJsonSpecialNumbers()
        {
            const string jsonString =
                "{\"+testZero+\" : 0,\"+testSmallNum+\" : 0.1,\"+testeZero+\" : 0.1e0,\"+testENegtiveWithZero+\" : 0E-1,\"+testeNegativeWithInt+\" : 2155e-5,\"+testEPositiveWithDecimal+\" : 2152.1541E+2,\"+testePositiveWithLargeInt+\" : 18446744073709551615E109,\"+testeNegativeWithLargeDecimal+\" : 125125612512512.512512e-0123,\"-testZero-\" : -0,\"-testSmallNum-\" : -0.1,\"-testeZero-\" : -0.1e0,\"-testENegtiveWithZero-\" : -0E-1,\"-testeNegativeWithInt-\" : -2155e-5,\"-testEPositiveWithDecimal-\" : -2152.1541E+2,\"-testePositiveWithLargeInt-\" :-18446744073709551615E109,\"-testeNegativeWithLargeDecimal-\" : -125125612512512.512512e-0123}";
            const string expectedJson =
                "{\"+testZero+\":0,\"+testSmallNum+\":0.1,\"+testeZero+\":0.1,\"+testENegtiveWithZero+\":0,\"+testeNegativeWithInt+\":0.02155,\"+testEPositiveWithDecimal+\":215215.41,\"+testePositiveWithLargeInt+\":1.84467440737096E+128,\"+testeNegativeWithLargeDecimal+\":1.25125612512513E-109,\"-testZero-\":0,\"-testSmallNum-\":-0.1,\"-testeZero-\":-0.1,\"-testENegtiveWithZero-\":0,\"-testeNegativeWithInt-\":-0.02155,\"-testEPositiveWithDecimal-\":-215215.41,\"-testePositiveWithLargeInt-\":-1.84467440737096E+128,\"-testeNegativeWithLargeDecimal-\":-1.25125612512513E-109}";
            var readJson = ReadJson(jsonString);
            var json = readJson.ToString();
            Assert.Equal(json, expectedJson);
        }

        private static Json CreateJson()
        {
            var valueAge = new Value
            {
                Type = Value.ValueType.Number,
                NumberValue = 30
            };

            var pairAge = new Pair
            {
                Name = "age",
                Value = valueAge
            };

            var valueFirst = new Value
            {
                Type = Value.ValueType.String,
                StringValue = "John"
            };

            var pairFirst = new Pair
            {
                Name = "first",
                Value = valueFirst
            };

            var valueLast = new Value
            {
                Type = Value.ValueType.String,
                StringValue = "Smith"
            };

            var pairLast = new Pair
            {
                Name = "last",
                Value = valueLast
            };


            var value1 = new Value
            {
                Type = Value.ValueType.String,
                StringValue = "425-000-1212"
            };

            var value2 = new Value
            {
                Type = Value.ValueType.String,
                StringValue = "425-000-1213"
            };

            var values = new List<Value> {value1, value2};
            var elementInner = new Elements {Values = values};
            var elementsInner = new List<Elements> {elementInner};
            var arrInner = new Array {Elements = elementsInner};

            var valuePhone = new Value
            {
                Type = Value.ValueType.Array,
                ArrayValue = arrInner
            };

            var pairPhone = new Pair
            {
                Name = "phoneNumbers",
                Value = valuePhone
            };

            var valueStreet = new Value
            {
                Type = Value.ValueType.String,
                StringValue = "1 Microsoft Way"
            };

            var pairStreet = new Pair
            {
                Name = "street",
                Value = valueStreet
            };

            var valueCity = new Value
            {
                Type = Value.ValueType.String,
                StringValue = "Redmond"
            };

            var pairCity = new Pair
            {
                Name = "city",
                Value = valueCity
            };

            var valueZip = new Value
            {
                Type = Value.ValueType.Number,
                NumberValue = 98052
            };

            var pairZip = new Pair
            {
                Name = "zip",
                Value = valueZip
            };

            var pairsInner = new List<Pair> {pairStreet, pairCity, pairZip};
            var memberInner = new Members {Pairs = pairsInner};
            var membersInner = new List<Members> {memberInner};
            var objInner = new Object {Members = membersInner};

            var valueAddress = new Value
            {
                Type = Value.ValueType.Object,
                ObjectValue = objInner
            };

            var pairAddress = new Pair
            {
                Name = "address",
                Value = valueAddress
            };

            var pairs = new List<Pair> {pairAge, pairFirst, pairLast, pairPhone, pairAddress};
            var member = new Members {Pairs = pairs};
            var members = new List<Members> {member};
            var obj = new Object {Members = members};
            var json = new Json {Object = obj};

            return json;
        }

        private static Json ReadJson(string jsonString)
        {
            var json = new Json();
            if (string.IsNullOrEmpty(jsonString))
            {
                return json;
            }

            var jsonReader = new JsonReader(jsonString);
            var jsonObjectMain = new Object();
            var jsonMembersMain = new List<Members>();

            ReadJsonHelper(jsonReader, jsonMembersMain);

            jsonObjectMain.Members = jsonMembersMain;
            json.Object = jsonObjectMain;

            return json;
        }

        private static void ReadJsonHelper(JsonReader jsonReader, ICollection<Members> jsonMembersMain)
        {
            Array jsonArray = null;
            List<Pair> jsonPairs = null;
            List<Value> jsonValues = null;

            while (jsonReader.Read())
            {
                switch (jsonReader.TokenType)
                {
                    case JsonReader.JsonTokenType.Start:
                        break;
                    case JsonReader.JsonTokenType.ObjectStart:
                        jsonPairs = new List<Pair>();
                        break;
                    case JsonReader.JsonTokenType.ObjectEnd:
                        if (jsonPairs != null)
                        {
                            var jsonMembers = new Members {Pairs = jsonPairs};
                            jsonMembersMain.Add(jsonMembers);
                        }
                        break;
                    case JsonReader.JsonTokenType.ArrayStart:
                        jsonArray = new Array();
                        jsonValues = new List<Value>();
                        break;
                    case JsonReader.JsonTokenType.ArrayEnd:
                        if (jsonArray != null)
                        {
                            var jsonElements = new Elements {Values = jsonValues};
                            jsonArray.Elements = new List<Elements> {jsonElements};
                        }
                        break;
                    case JsonReader.JsonTokenType.Pair:
                        var pair = new Pair
                        {
                            Name = (string) jsonReader.ReadString(),
                            Value = GetValue(ref jsonReader)
                        };
                        if (jsonPairs != null) jsonPairs.Add(pair);
                        break;
                    case JsonReader.JsonTokenType.Value:
                        if (jsonValues != null) jsonValues.Add(GetValue(ref jsonReader));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private static Value GetValue(ref JsonReader jsonReader)
        {
            var value = new Value {Type = (Value.ValueType) jsonReader.GetValueType()};
            var obj = jsonReader.ReadValue();
            switch (value.Type)
            {
                case Value.ValueType.String:
                    value.StringValue = (string) new Utf8String(obj.ToString());
                    break;
                case Value.ValueType.Number:
                    value.NumberValue = (double) obj;
                    break;
                case Value.ValueType.True:
                    break;
                case Value.ValueType.False:
                    break;
                case Value.ValueType.Null:
                    break;
                case Value.ValueType.Object:
                    value.ObjectValue = ReadObject(ref jsonReader);
                    break;
                case Value.ValueType.Array:
                    value.ArrayValue = ReadArray(ref jsonReader);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return value;
        }

        private static Object ReadObject(ref JsonReader jsonReader)
        {
            var jsonObject = new Object();
            var jsonMembersList = new List<Members>();
            List<Pair> jsonPairs = null;

            while (jsonReader.Read())
            {
                switch (jsonReader.TokenType)
                {
                    case JsonReader.JsonTokenType.Start:
                        break;
                    case JsonReader.JsonTokenType.ObjectStart:
                        jsonPairs = new List<Pair>();
                        break;
                    case JsonReader.JsonTokenType.ObjectEnd:
                        if (jsonPairs != null)
                        {
                            var jsonMembers = new Members {Pairs = jsonPairs};
                            jsonMembersList.Add(jsonMembers);
                            jsonObject.Members = jsonMembersList;
                            return jsonObject;
                        }
                        break;
                    case JsonReader.JsonTokenType.ArrayStart:
                        break;
                    case JsonReader.JsonTokenType.ArrayEnd:
                        break;
                    case JsonReader.JsonTokenType.Pair:
                        var pair = new Pair
                        {
                            Name = (string) jsonReader.ReadString(),
                            Value = GetValue(ref jsonReader)
                        };
                        if (jsonPairs != null) jsonPairs.Add(pair);
                        break;
                    case JsonReader.JsonTokenType.Value:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            throw new FormatException("Json object was started but never ended.");
        }

        private static Array ReadArray(ref JsonReader jsonReader)
        {
            Array jsonArray = null;
            List<Value> jsonValues = null;

            while (jsonReader.Read())
            {
                switch (jsonReader.TokenType)
                {
                    case JsonReader.JsonTokenType.Start:
                        break;
                    case JsonReader.JsonTokenType.ObjectStart:
                        break;
                    case JsonReader.JsonTokenType.ObjectEnd:
                        break;
                    case JsonReader.JsonTokenType.ArrayStart:
                        jsonArray = new Array();
                        jsonValues = new List<Value>();
                        break;
                    case JsonReader.JsonTokenType.ArrayEnd:
                        if (jsonArray != null)
                        {
                            var jsonElements = new Elements {Values = jsonValues};
                            jsonArray.Elements = new List<Elements> {jsonElements};
                            return jsonArray;
                        }
                        break;
                    case JsonReader.JsonTokenType.Pair:
                        break;
                    case JsonReader.JsonTokenType.Value:
                        if (jsonValues != null) jsonValues.Add(GetValue(ref jsonReader));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            throw new FormatException("Json array was started but never ended.");
        }
    }
}