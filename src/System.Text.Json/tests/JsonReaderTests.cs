using System.Collections.Generic;
using System.Text.Json.Tests.Resources;
using Xunit;

namespace System.Text.Json.Tests
{
    public class JsonReaderTests
    {
        [Fact]
        public void ReadBasicJson()
        {
            var testJson = CreateJson();
            Assert.Equal(testJson.ToString(), TestJson.ExpectedCreateJson);

            var readJson = ReadJson(TestJson.BasicJson);
            var json = readJson.ToString();
            Assert.Equal(json, TestJson.ExpectedBasicJson);
        }

        [Fact]
        public void ReadBasicJsonWithLongInt()
        {
            var readJson = ReadJson(TestJson.BasicJsonWithLargeNum);
            var json = readJson.ToString();
            Assert.Equal(json, TestJson.ExpectedBasicJsonWithLargeNum);
        }

        [Fact]
        public void ReadFullJsonSchema()
        {
            var readJson = ReadJson(TestJson.FullJsonSchema1);
            var json = readJson.ToString();
            Assert.Equal(json, TestJson.ExpectedFullJsonSchema1);
        }

        [Fact]
        public void ReadFullJsonSchemaAndGetValue()
        {
            var readJson = ReadJson(TestJson.FullJsonSchema2);
            var json = readJson.ToString();
            Assert.Equal(json, TestJson.ExpectedFullJsonSchema2);

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
            var readJson = ReadJson(TestJson.JsonWithSpecialStrings);
            var json = readJson.ToString();
            Assert.Equal(json, TestJson.ExpectedJsonWithSpecialStrings);
        }

        [Fact]
        public void ReadJsonSpecialNumbers()
        {
            var readJson = ReadJson(TestJson.JsonWithSpecialNumFormat);
            var json = readJson.ToString();
            Assert.Equal(json, TestJson.ExpectedJsonWithSpecialNumFormat);
        }

        [Fact]
        public void ReadProjectLockJson()
        {
            var readJson = ReadJson(TestJson.ProjectLockJson);
            var json = readJson.ToString();
            Assert.Equal(json, TestJson.ExpectedProjectLockJson);
        }

        [Fact]
        public void ReadHeavyNestedJson()
        {
            var readJson = ReadJson(TestJson.HeavyNestedJson);
            var json = readJson.ToString();
            Assert.Equal(json, TestJson.ExpectedHeavyNestedJson);
        }

        [Fact]
        public void ReadHeavyNestedJsonWithArray()
        {
            var readJson = ReadJson(TestJson.HeavyNestedJsonWithArray);
            var json = readJson.ToString();
            Assert.Equal(json, TestJson.ExpectedHeavyNestedJsonWithArray);
        }

        [Fact]
        public void ReadLargeJson()
        {
            var readJson = ReadJson(TestJson.LargeJson);
            var json = readJson.ToString();
            Assert.Equal(json, TestJson.ExpectedLargeJson);
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
            var jsonArrayMain = new Array();
            var jsonElementsMain = new List<Elements>();

            if (jsonString.Trim().Substring(0, 1) == "[")
            {
                ReadJsonHelper(jsonReader, jsonElementsMain);

                jsonArrayMain.Elements = jsonElementsMain;
                json.Array = jsonArrayMain;
            }
            else
            {
                ReadJsonHelper(jsonReader, jsonMembersMain);

                jsonObjectMain.Members = jsonMembersMain;
                json.Object = jsonObjectMain;
            }

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
                    case JsonReader.JsonTokenType.Property:
                        var pair = new Pair
                        {
                            Name = (string) jsonReader.GetName(),
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

        private static void ReadJsonHelper(JsonReader jsonReader, ICollection<Elements> jsonElementsMain)
        {
            Object jsonObject = null;
            List<Pair> jsonPairs = null;
            List<Value> jsonValues = null;

            while (jsonReader.Read())
            {
                switch (jsonReader.TokenType)
                {
                    case JsonReader.JsonTokenType.ObjectStart:
                        jsonObject = new Object();
                        jsonPairs = new List<Pair>();
                        break;
                    case JsonReader.JsonTokenType.ObjectEnd:
                        if (jsonObject != null)
                        {
                            var jsonMembers = new Members { Pairs = jsonPairs };
                            jsonObject.Members = new List<Members> { jsonMembers };
                        }
                        break;
                    case JsonReader.JsonTokenType.ArrayStart:
                        jsonValues = new List<Value>();
                        break;
                    case JsonReader.JsonTokenType.ArrayEnd:
                        if (jsonValues != null)
                        {
                            var jsonElements = new Elements { Values = jsonValues };
                            jsonElementsMain.Add(jsonElements);
                        }
                        break;
                    case JsonReader.JsonTokenType.Property:
                        var pair = new Pair
                        {
                            Name = (string)jsonReader.GetName(),
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
            var type = jsonReader.GetJsonValueType();
            var value = new Value {Type = (Value.ValueType) type};
            var obj = jsonReader.GetValue();
            switch (value.Type)
            {
                case Value.ValueType.String:
                    value.StringValue = obj.ToString();
                    break;
                case Value.ValueType.Number:
                    value.NumberValue = Convert.ToDouble(obj.ToString());
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
                    case JsonReader.JsonTokenType.Property:
                        var pair = new Pair
                        {
                            Name = (string) jsonReader.GetName(),
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
                    case JsonReader.JsonTokenType.Property:
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