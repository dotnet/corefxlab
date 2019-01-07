// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Text;
using System.Collections.Generic;
using System.Text.JsonLab.Tests.Resources;
using Xunit;

namespace System.Text.JsonLab.Tests
{
    public class JsonParserTests
    {
        [Fact]
        public void ParseBasicJson()
        {
            var json = ReadJson(TestJson.ParseJson);
            var person = json[0];
            var age = (double)person[Encoding.UTF8.GetBytes("age")];
            var first = (string)person[Encoding.UTF8.GetBytes("first")];
            var last = (string)person[Encoding.UTF8.GetBytes("last")];
            var phoneNums = person[Encoding.UTF8.GetBytes("phoneNumbers")];
            var phoneNum1 = (string)phoneNums[0];
            var phoneNum2 = (string)phoneNums[1];
            var address = person[Encoding.UTF8.GetBytes("address")];
            var street = (string)address[Encoding.UTF8.GetBytes("street")];
            var city = (string)address[Encoding.UTF8.GetBytes("city")];
            var zipCode = (double)address[Encoding.UTF8.GetBytes("zip")];

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
                Name = Encoding.UTF8.GetBytes("age"),
                Value = valueAge
            };

            var valueFirst = new Value
            {
                Type = Value.ValueType.String,
                StringValue = Encoding.UTF8.GetBytes("John")
            };

            var pairFirst = new Pair
            {
                Name = Encoding.UTF8.GetBytes("first"),
                Value = valueFirst
            };

            var valueLast = new Value
            {
                Type = Value.ValueType.String,
                StringValue = Encoding.UTF8.GetBytes("Smith")
            };

            var pairLast = new Pair
            {
                Name = Encoding.UTF8.GetBytes("last"),
                Value = valueLast
            };


            var value1 = new Value
            {
                Type = Value.ValueType.String,
                StringValue = Encoding.UTF8.GetBytes("425-000-1212")
            };

            var value2 = new Value
            {
                Type = Value.ValueType.String,
                StringValue = Encoding.UTF8.GetBytes("425-000-1213")
            };

            var values = new List<Value> { value1, value2 };
            var arrInner = new Array { Values = values };

            var valuePhone = new Value
            {
                Type = Value.ValueType.Array,
                ArrayValue = arrInner
            };

            var pairPhone = new Pair
            {
                Name = Encoding.UTF8.GetBytes("phoneNumbers"),
                Value = valuePhone
            };

            var valueStreet = new Value
            {
                Type = Value.ValueType.String,
                StringValue = Encoding.UTF8.GetBytes("1 Microsoft Way")
            };

            var pairStreet = new Pair
            {
                Name = Encoding.UTF8.GetBytes("street"),
                Value = valueStreet
            };

            var valueCity = new Value
            {
                Type = Value.ValueType.String,
                StringValue = Encoding.UTF8.GetBytes("Redmond")
            };

            var pairCity = new Pair
            {
                Name = Encoding.UTF8.GetBytes("city"),
                Value = valueCity
            };

            var valueZip = new Value
            {
                Type = Value.ValueType.Number,
                NumberValue = 98052
            };

            var pairZip = new Pair
            {
                Name = Encoding.UTF8.GetBytes("zip"),
                Value = valueZip
            };

            var pairsInner = new List<Pair> { pairStreet, pairCity, pairZip };
            var objInner = new Object { Pairs = pairsInner };

            var valueAddress = new Value
            {
                Type = Value.ValueType.Object,
                ObjectValue = objInner
            };

            var pairAddress = new Pair
            {
                Name = Encoding.UTF8.GetBytes("address"),
                Value = valueAddress
            };

            var pairs = new List<Pair> { pairAge, pairFirst, pairLast, pairPhone, pairAddress };
            var obj = new Object { Pairs = pairs };
            var json = new TestDom { Object = obj };

            return json;
        }

        private static TestDom ReadJson(string jsonString)
        {
            var json = new TestDom();
            if (string.IsNullOrEmpty(jsonString))
            {
                return json;
            }

            var jsonReader = new JsonUtf8Reader(Encoding.UTF8.GetBytes(jsonString));
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

        private static Value GetValue(ref JsonUtf8Reader jsonReader)
        {
            var value = new Value { Type = MapValueType(jsonReader.TokenType) };
            switch (value.Type)
            {
                case Value.ValueType.String:
                    value.StringValue = ReadUtf8String(ref jsonReader);
                    break;
                case Value.ValueType.Number:
                    CustomParser.TryParseDecimal(jsonReader.ValueSpan, out decimal num, out int consumed);
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

        private static Value.ValueType MapValueType(JsonTokenType type)
        {
            switch (type)
            {
                case JsonTokenType.False:
                    return Value.ValueType.False;
                case JsonTokenType.True:
                    return Value.ValueType.True;
                case JsonTokenType.Null:
                    return Value.ValueType.Null;
                case JsonTokenType.Number:
                    return Value.ValueType.Number;
                case JsonTokenType.String:
                    return Value.ValueType.String;
                case JsonTokenType.StartArray:
                    return Value.ValueType.Array;
                case JsonTokenType.StartObject:
                    return Value.ValueType.Object;
                default:
                    throw new ArgumentException();
            }
        }

        private static Object ReadObject(ref JsonUtf8Reader jsonReader)
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
                        ReadOnlyMemory<byte> name = ReadUtf8String(ref jsonReader);
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

        private static Array ReadArray(ref JsonUtf8Reader jsonReader)
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
                    case JsonTokenType.False:
                    case JsonTokenType.True:
                    case JsonTokenType.Null:
                    case JsonTokenType.Number:
                    case JsonTokenType.String:
                        jsonValues.Add(GetValue(ref jsonReader));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            throw new FormatException("Json array was started but never ended.");
        }

        private static byte[] ReadUtf8String(ref JsonUtf8Reader jsonReader)
        {
            return jsonReader.ValueSpan.ToArray();
        }
    }
}
