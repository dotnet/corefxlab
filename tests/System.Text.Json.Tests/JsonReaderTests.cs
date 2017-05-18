// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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

        [Fact(Skip = "This test is injecting invalid characters into the stream and needs to be re-visited")]
        public void ReadJsonSpecialStrings()
        {
            var readJson = ReadJson(TestJson.JsonWithSpecialStrings);
            var json = readJson.ToString();
            Assert.Equal(json, TestJson.ExpectedJsonWithSpecialStrings);
        }

        [Fact(Skip = "The current primitive parsers do not support E-notation for numbers.")]
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

        private static TestDom CreateJson()
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
            var json = new TestDom {Object = obj};

            return json;
        }

        private static TestDom ReadJson(string jsonString)
        {
            var json = new TestDom();
            if (string.IsNullOrEmpty(jsonString))
            {
                return json;
            }

            var jsonReader = new JsonReader(jsonString.AsSpan().AsBytes(), TextEncoder.Utf16);
            jsonReader.Read();
            switch (jsonReader.TokenType)
            {
                case JsonTokenType.StartArray:
                    json.Array = ReadArray(ref jsonReader);
                    break;

                case JsonTokenType.StartObject:
                    json.Object = ReadObject(ref jsonReader);
                    break;

                default:
                    Assert.True(false, "The test JSON does not start with an array or object token");
                    break;
            }

            return json;
        }

        private static Value GetValue(ref JsonReader jsonReader)
        {
            int consumed;
            var value = new Value { Type = MapValueType(jsonReader.ValueType) };
            switch (value.Type)
            {
                case Value.ValueType.String:
                    jsonReader.Encoder.TryDecode(jsonReader.Value, out string str, out consumed);
                    value.StringValue = str;
                    break;
                case Value.ValueType.Number:
                    PrimitiveParser.TryParseDecimal(jsonReader.Value, out decimal num, out consumed, jsonReader.Encoder);
                    value.NumberValue = Convert.ToDouble(num);
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

        private static Value.ValueType MapValueType(JsonValueType type)
        {
            switch (type)
            {
                case JsonValueType.False:
                    return Value.ValueType.False;
                case JsonValueType.True:
                    return Value.ValueType.True;
                case JsonValueType.Null:
                    return Value.ValueType.Null;
                case JsonValueType.Number:
                    return Value.ValueType.Number;
                case JsonValueType.String:
                    return Value.ValueType.String;
                case JsonValueType.Array:
                    return Value.ValueType.Array;
                case JsonValueType.Object:
                    return Value.ValueType.Object;
                default:
                    throw new ArgumentException();
            }
        }

        private static Object ReadObject(ref JsonReader jsonReader)
        {
            // NOTE: We should be sitting on a StartObject token.
            Assert.Equal(JsonTokenType.StartObject, jsonReader.TokenType);

            var jsonObject = new Object();
            List<Pair> jsonPairs = new List<Pair>();

            while (jsonReader.Read())
            {
                switch (jsonReader.TokenType)
                {
                    case JsonTokenType.EndObject:
                        jsonObject.Pairs = jsonPairs;
                        return jsonObject;
                    case JsonTokenType.PropertyName:
                        Assert.True(jsonReader.Encoder.TryDecode(jsonReader.Value, out string name, out int consumed));
                        jsonReader.Read(); // Move to value token
                        var pair = new Pair
                        {
                            Name = name,
                            Value = GetValue(ref jsonReader)
                        };
                        if (jsonPairs != null) jsonPairs.Add(pair);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            throw new FormatException("Json object was started but never ended.");
        }

        private static Array ReadArray(ref JsonReader jsonReader)
        {
            // NOTE: We should be sitting on a StartArray token.
            Assert.Equal(JsonTokenType.StartArray, jsonReader.TokenType);

            Array jsonArray = new Array();
            List<Value> jsonValues = new List<Value>();

            while (jsonReader.Read())
            {
                switch (jsonReader.TokenType)
                {
                    case JsonTokenType.EndArray:
                        jsonArray.Values = jsonValues;
                        return jsonArray;
                    case JsonTokenType.StartArray:
                    case JsonTokenType.StartObject:
                    case JsonTokenType.Value:
                        jsonValues.Add(GetValue(ref jsonReader));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            throw new FormatException("Json array was started but never ended.");
        }
    }
}
