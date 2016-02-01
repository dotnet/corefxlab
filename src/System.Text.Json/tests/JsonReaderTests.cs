using System.Collections.Generic;
using System.Text.Json.Tests.Resources;
using System.Text.Utf8;
using Xunit;

namespace System.Text.Json.Tests
{
    public class JsonReaderTests
    {
        [Fact]
        public void ParseBasicJson()
        {
            var json = ReadJson(TestJson.ParseJson);
            var person = json[0];
            var age = (double)person["age"];
            var first = (string)person["first"];
            var last = (string)person["last"];
            var phoneNums = person["phoneNumbers"];
            var phoneNum1 = (string)phoneNums[0];
            var phoneNum2 = (string)phoneNums[1];
            var address = person["address"];
            var street = (string)address["street"];
            var city = (string)address["city"];
            var zipCode = (double)address["zip"];

            // Exceptional use case
            //var a = json[1];                          // IndexOutOfRangeException
            //var b = json["age"];                      // NullReferenceException
            //var c = person[0];                        // NullReferenceException
            //var d = address["cit"];                   // KeyNotFoundException
            //var e = address[0];                       // NullReferenceException
            //var f = (double)address["city"];          // InvalidCastException
            //var g = (bool)address["city"];            // InvalidCastException
            //var h = (string)address["zip"];           // InvalidCastException
            //var i = (string)person["phoneNumbers"];   // NullReferenceException
            //var j = (string)person;                   // NullReferenceException

            Assert.Equal(age, 30);
            Assert.Equal(first, "John");
            Assert.Equal(last, "Smith");
            Assert.Equal(phoneNum1, "425-000-1212");
            Assert.Equal(phoneNum2, "425-000-1213");
            Assert.Equal(street, "1 Microsoft Way");
            Assert.Equal(city, "Redmond");
            Assert.Equal(zipCode, 98052);
        }


        [Fact]
        public void ParseSimpleJson()
        {
            var str = TestJson.SimpleArrayJson;
            int strLength = str.Length;
            var buffer = new byte[4096];
            for (var i = 0; i < strLength; i++)
            {
                buffer[i] = (byte)str[i];
            }

            var json = new JsonParser(buffer, strLength);
            var parseObject = json.Parse();
            var phoneNum = (string)parseObject[0];
            var age = (int)parseObject[1];
            
            Assert.Equal(phoneNum, "425-214-3151");
            Assert.Equal(age, 25);

            str = TestJson.SimpleObjectJson;
            strLength = str.Length;
            buffer = new byte[4096];
            for (var i = 0; i < strLength; i++)
            {
                buffer[i] = (byte)str[i];
            }

            json = new JsonParser(buffer, strLength);
            parseObject = json.Parse();
            age = (int)parseObject["age"];
            var ageStr = (string)parseObject["age"];
            var first = (string)parseObject["first"];
            var last = (string)parseObject["last"];
            var phoneNumber = (string)parseObject["phoneNumber"];
            var street = (string)parseObject["street"];
            var city = (string)parseObject["city"];
            var zip = (int)parseObject["zip"];

            Assert.Equal(age, 30);
            Assert.Equal(ageStr, "30");
            Assert.Equal(first, "John");
            Assert.Equal(last, "Smith");
            Assert.Equal(phoneNumber, "425-214-3151");
            Assert.Equal(street, "1 Microsoft Way");
            Assert.Equal(city, "Redmond");
            Assert.Equal(zip, 98052);
        }

        [Fact]
        public void ParseNestedJson()
        {
            var str = TestJson.ParseJson;
            int strLength = str.Length;
            var buffer = new byte[4096];
            for (var i = 0; i < strLength; i++)
            {
                buffer[i] = (byte)str[i];
            }

            var json = new JsonParser(buffer, strLength);
            var x = json.Parse();
            var person = x[0];
            var age = (double)person["age"];
            var first = (string)person["first"];
            var last = (string)person["last"];
            var phoneNums = person["phoneNumbers"];
            var phoneNum1 = (string)phoneNums[0];
            var phoneNum2 = (string)phoneNums[1];
            var address = person["address"];
            var street = (string)address["street"];
            var city = (string)address["city"];
            var zipCode = (double)address["zip"];

            // Exceptional use case
            //var a = x[1];                             // IndexOutOfRangeException
            //var b = x["age"];                         // NullReferenceException
            //var c = person[0];                        // NullReferenceException
            //var d = address["cit"];                   // KeyNotFoundException
            //var e = address[0];                       // NullReferenceException
            //var f = (double)address["city"];          // InvalidCastException
            //var g = (bool)address["city"];            // InvalidCastException
            //var h = (string)address["zip"];           // Integer converted to string implicitly
            //var i = (string)person["phoneNumbers"];   // InvalidCastException
            //var j = (string)person;                   // InvalidCastException

            Assert.Equal(age, 30);
            Assert.Equal(first, "John");
            Assert.Equal(last, "Smith");
            Assert.Equal(phoneNum1, "425-000-1212");
            Assert.Equal(phoneNum2, "425-000-1213");
            Assert.Equal(street, "1 Microsoft Way");
            Assert.Equal(city, "Redmond");
            Assert.Equal(zipCode, 98052);
        }

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
            Assert.Equal(emptyObject[0].ObjectValue.Pairs.Count, 0);
            var arrayString = readJson.GetValueFromPropertyName("arrayString");
            Assert.Equal(arrayString[0].ArrayValue.Values.Count, 2);
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
            var arrInner = new Array {Values = values};

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
            var objInner = new Object {Pairs = pairsInner};

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
            var obj = new Object {Pairs = pairs};
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
            var jsonMembersMain = new List<Pair>();
            var jsonArrayMain = new Array();
            var jsonElementsMain = new List<Value>();

            if (jsonString.Trim().Substring(0, 1) == "[")
            {
                ReadJsonHelper(jsonReader, jsonElementsMain);

                jsonArrayMain.Values = jsonElementsMain;
                json.Array = jsonArrayMain;
            }
            else
            {
                ReadJsonHelper(jsonReader, jsonMembersMain);

                jsonObjectMain.Pairs = jsonMembersMain;
                json.Object = jsonObjectMain;
            }

            return json;
        }

        private static void ReadJsonHelper(JsonReader jsonReader, List<Pair> jsonMembersMain)
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
                            jsonMembersMain.AddRange(jsonPairs);
                        }
                        break;
                    case JsonReader.JsonTokenType.ArrayStart:
                        jsonArray = new Array();
                        jsonValues = new List<Value>();
                        break;
                    case JsonReader.JsonTokenType.ArrayEnd:
                        if (jsonArray != null)
                        {
                            jsonArray.Values = jsonValues;
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

        private static void ReadJsonHelper(JsonReader jsonReader, List<Value> jsonElementsMain)
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
                            jsonObject.Pairs = jsonPairs;
                        }
                        break;
                    case JsonReader.JsonTokenType.ArrayStart:
                        jsonValues = new List<Value>();
                        break;
                    case JsonReader.JsonTokenType.ArrayEnd:
                        if (jsonValues != null)
                        {
                            jsonElementsMain.AddRange(jsonValues);
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
                            jsonObject.Pairs = jsonPairs;
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
                            jsonArray.Values = jsonValues;
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