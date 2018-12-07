// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;
using System.Text.Formatting;
using System.Buffers.Text;
using System.IO;

namespace System.Text.JsonLab.Tests
{
    public class JsonWriterTests
    {
        private const int ExtraArraySize = 500;

        /*[Fact]
        public void WriteJsonUtf8()
        {
            var formatter = new ArrayFormatterWrapper(1024, SymbolTable.InvariantUtf8);
            var json = new Utf8JsonWriter<ArrayFormatterWrapper>(formatter, prettyPrint: false);
            Write(ref json);

            var formatted = formatter.Formatted;
            var str = Encoding.UTF8.GetString(formatted.Array, formatted.Offset, formatted.Count);
            Assert.Equal(expected, str.Replace(" ", ""));

            formatter.Clear();
            json = new Utf8JsonWriter<ArrayFormatterWrapper>(formatter, prettyPrint: true);
            Write(ref json);

            formatted = formatter.Formatted;
            str = Encoding.UTF8.GetString(formatted.Array, formatted.Offset, formatted.Count);
            Assert.Equal(expected, str.Replace("\r\n", "").Replace("\n", "").Replace(" ", ""));
        }

        static readonly string expected = "{\"age\":30,\"first\":\"John\",\"last\":\"Smith\",\"phoneNumbers\":[\"425-000-1212\",\"425-000-1213\",null],\"address\":{\"street\":\"1MicrosoftWay\",\"city\":\"Redmond\",\"zip\":98052},\"values\":[425121,-425122,425123]}";

        static void Write(ref Utf8JsonWriter<ArrayFormatterWrapper> json)
        {
            int[] values = { 425121, -425122, 425123 };
            byte[] valueString = Encoding.UTF8.GetBytes("values");
            json.WriteObjectStart();
            json.WriteAttribute("age", 30);
            json.WriteAttribute("first", "John");
            json.WriteAttribute("last", "Smith");
            json.WriteArrayStart("phoneNumbers");
            json.WriteValue("425-000-1212");
            json.WriteValue("425-000-1213");
            json.WriteNull();
            json.WriteArrayEnd();
            json.WriteObjectStart("address");
            json.WriteAttribute("street", "1 Microsoft Way");
            json.WriteAttribute("city", "Redmond");
            json.WriteAttribute("zip", 98052);
            json.WriteObjectEnd();
            json.WriteArrayUtf8(valueString, values);
            json.WriteObjectEnd();
            json.Flush();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void WriteHelloWorldJsonUtf8(bool prettyPrint)
        {
            string expectedStr = GetHelloWorldExpectedString(prettyPrint);

            var output = new ArrayFormatterWrapper(1024, SymbolTable.InvariantUtf8);
            var jsonUtf8 = new Utf8JsonWriter<ArrayFormatterWrapper>(output, prettyPrint);

            jsonUtf8.WriteObjectStart();
            jsonUtf8.WriteAttribute("message", "Hello, World!");
            jsonUtf8.WriteObjectEnd();
            jsonUtf8.Flush();

            ArraySegment<byte> formatted = output.Formatted;
            string actualStr = Encoding.UTF8.GetString(formatted.Array, formatted.Offset, formatted.Count);

            Assert.Equal(expectedStr, actualStr);
        }*/

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void InvalidJsonMismatch(bool formatted, bool skipValidation)
        {
            var options = new JsonWriterOptions
            {
                Formatted = formatted,
                SkipValidation = skipValidation
            };

            var output = new ArrayFormatterWrapper(1024, SymbolTable.InvariantUtf8);

            var jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, options);
            try
            {
                jsonUtf8.WriteEndArray();
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) {}

            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, options);
            try
            {
                jsonUtf8.WriteEndObject();
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }

            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, options);
            try
            {
                jsonUtf8.WriteStartArray("property at start");
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }

            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, options);
            try
            {
                jsonUtf8.WriteStartObject("property at start");
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }

            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, options);
            try
            {
                jsonUtf8.WriteStartArray();
                jsonUtf8.WriteStartArray("property inside array");
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }

            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, options);
            try
            {
                jsonUtf8.WriteStartObject();
                jsonUtf8.WriteStartObject();
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }

            // TODO: Need write value
            /*jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, options);
            try
            {
                jsonUtf8.WriteEndArray();
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }

            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, options);
            try
            {
                jsonUtf8.WriteEndObject();
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }*/

            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, options);
            try
            {
                jsonUtf8.WriteStartArray();
                jsonUtf8.WriteEndObject();
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }

            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, options);
            try
            {
                jsonUtf8.WriteStartObject();
                jsonUtf8.WriteEndArray();
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }

            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, options);
            try
            {
                jsonUtf8.WriteStartArray();
                jsonUtf8.WriteStartArray();
                jsonUtf8.WriteEndArray();
                jsonUtf8.WriteEndObject();
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }

            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, options);
            try
            {
                jsonUtf8.WriteStartObject();
                jsonUtf8.WriteStartObject("some object");
                jsonUtf8.WriteEndObject();
                jsonUtf8.WriteEndArray();
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }

            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, options);
            try
            {
                jsonUtf8.WriteStartArray();
                jsonUtf8.WriteStartObject("some object");
                jsonUtf8.WriteEndObject();
                jsonUtf8.WriteEndObject();
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }

            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, options);
            try
            {
                jsonUtf8.WriteStartObject();
                jsonUtf8.WriteStartArray("test array");
                jsonUtf8.WriteEndArray();
                jsonUtf8.WriteEndArray();
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }

            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, options);
            try
            {
                jsonUtf8.WriteStartArray();
                jsonUtf8.WriteEndArray();
                jsonUtf8.WriteEndArray();
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }

            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, options);
            try
            {
                jsonUtf8.WriteStartObject();
                jsonUtf8.WriteEndObject();
                jsonUtf8.WriteEndObject();
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }

            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, options);
            try
            {
                jsonUtf8.WriteStartArray();
                jsonUtf8.WriteStartArray();
                jsonUtf8.WriteEndObject();
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }

            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, options);
            try
            {
                jsonUtf8.WriteStartObject();
                jsonUtf8.WriteStartObject("test object");
                jsonUtf8.WriteEndArray();
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }
        }

        private static void WriterDidNotThrow(bool skipValidation)
        {
            if (skipValidation)
                Assert.True(true, "Did not expect FormatException to be thrown since validation was skipped.");
            else
                Assert.True(false, "Expected FormatException to be thrown when validation is enabled.");
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void WriteHelloWorldWithOptions(bool formatted, bool skipValidation)
        {
            string expectedStr = GetHelloWorldExpectedString(prettyPrint: formatted);

            var options = new JsonWriterOptions
            {
                Formatted = formatted,
                SkipValidation = skipValidation
            };

            for (int i = 0; i < 9; i++)
            {
                var output = new ArrayFormatterWrapper(1024, SymbolTable.InvariantUtf8);
                var jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, options);

                jsonUtf8.WriteStartObject();

                switch (i)
                {
                    case 0:
                        jsonUtf8.WriteString("message", "Hello, World!");
                        break;
                    case 1:
                        jsonUtf8.WriteString("message", "Hello, World!".AsSpan());
                        break;
                    case 2:
                        jsonUtf8.WriteString("message", Encoding.UTF8.GetBytes("Hello, World!"));
                        break;
                    case 3:
                        jsonUtf8.WriteString("message".AsSpan(), "Hello, World!");
                        break;
                    case 4:
                        jsonUtf8.WriteString("message".AsSpan(), "Hello, World!".AsSpan());
                        break;
                    case 5:
                        jsonUtf8.WriteString("message".AsSpan(), Encoding.UTF8.GetBytes("Hello, World!"));
                        break;
                    case 6:
                        jsonUtf8.WriteString(Encoding.UTF8.GetBytes("message"), "Hello, World!");
                        break;
                    case 7:
                        jsonUtf8.WriteString(Encoding.UTF8.GetBytes("message"), "Hello, World!".AsSpan());
                        break;
                    case 8:
                        jsonUtf8.WriteString(Encoding.UTF8.GetBytes("message"), Encoding.UTF8.GetBytes("Hello, World!"));
                        break;
                }
                
                jsonUtf8.WriteEndObject();
                jsonUtf8.Flush();

                ArraySegment<byte> arraySegment = output.Formatted;
                string actualStr = Encoding.UTF8.GetString(arraySegment.Array, arraySegment.Offset, arraySegment.Count);

                Assert.True(expectedStr == actualStr, $"Case: {i}, | Expected: {expectedStr}, | Actual: {actualStr}");
            }
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, false, true)]
        [InlineData(false, true, true)]
        [InlineData(false, false, true)]
        [InlineData(true, true, false)]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, false)]
        public void WriteBooleanValueWithOptions(bool formatted, bool skipValidation, bool value)
        {
            string expectedStr = GetBooleanExpectedString(prettyPrint: formatted, value);

            var options = new JsonWriterOptions
            {
                Formatted = formatted,
                SkipValidation = skipValidation
            };

            for (int i = 0; i < 3; i++)
            {
                var output = new ArrayFormatterWrapper(1024, SymbolTable.InvariantUtf8);
                var jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, options);

                jsonUtf8.WriteStartObject();

                switch (i)
                {
                    case 0:
                        jsonUtf8.WriteBoolean("message", value);
                        break;
                    case 1:
                        jsonUtf8.WriteBoolean("message".AsSpan(), value);
                        break;
                    case 2:
                        jsonUtf8.WriteBoolean(Encoding.UTF8.GetBytes("message"), value);
                        break;
                }

                jsonUtf8.WriteEndObject();
                jsonUtf8.Flush();

                ArraySegment<byte> arraySegment = output.Formatted;
                string actualStr = Encoding.UTF8.GetString(arraySegment.Array, arraySegment.Offset, arraySegment.Count);

                Assert.True(expectedStr == actualStr, $"Case: {i}, | Expected: {expectedStr}, | Actual: {actualStr}, | Value: {value}");
            }
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void WriteNullValueWithOptions(bool formatted, bool skipValidation)
        {
            string expectedStr = GetNullExpectedString(prettyPrint: formatted);

            var options = new JsonWriterOptions
            {
                Formatted = formatted,
                SkipValidation = skipValidation
            };

            for (int i = 0; i < 3; i++)
            {
                var output = new ArrayFormatterWrapper(1024, SymbolTable.InvariantUtf8);
                var jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, options);

                jsonUtf8.WriteStartObject();

                switch (i)
                {
                    case 0:
                        jsonUtf8.WriteNull("message");
                        break;
                    case 1:
                        jsonUtf8.WriteNull("message".AsSpan());
                        break;
                    case 2:
                        jsonUtf8.WriteNull(Encoding.UTF8.GetBytes("message"));
                        break;
                }

                jsonUtf8.WriteEndObject();
                jsonUtf8.Flush();

                ArraySegment<byte> arraySegment = output.Formatted;
                string actualStr = Encoding.UTF8.GetString(arraySegment.Array, arraySegment.Offset, arraySegment.Count);

                Assert.True(expectedStr == actualStr, $"Case: {i}, | Expected: {expectedStr}, | Actual: {actualStr}");
            }
        }

        [Theory]
        [InlineData(true, true, 0)]
        [InlineData(true, false, 0)]
        [InlineData(false, true, 0)]
        [InlineData(false, false, 0)]
        [InlineData(true, true, -1)]
        [InlineData(true, false, -1)]
        [InlineData(false, true, -1)]
        [InlineData(false, false, -1)]
        [InlineData(true, true, 1)]
        [InlineData(true, false, 1)]
        [InlineData(false, true, 1)]
        [InlineData(false, false, 1)]
        [InlineData(true, true, int.MaxValue)]
        [InlineData(true, false, int.MaxValue)]
        [InlineData(false, true, int.MaxValue)]
        [InlineData(false, false, int.MaxValue)]
        [InlineData(true, true, int.MinValue)]
        [InlineData(true, false, int.MinValue)]
        [InlineData(false, true, int.MinValue)]
        [InlineData(false, false, int.MinValue)]
        [InlineData(true, true, 12345)]
        [InlineData(true, false, 12345)]
        [InlineData(false, true, 12345)]
        [InlineData(false, false, 12345)]
        public void WriteNumberValueWithOptions(bool formatted, bool skipValidation, int value)
        {
            string expectedStr = GetNumberExpectedString(prettyPrint: formatted, value);

            var options = new JsonWriterOptions
            {
                Formatted = formatted,
                SkipValidation = skipValidation
            };

            for (int i = 0; i < 3; i++)
            {
                var output = new ArrayFormatterWrapper(1024, SymbolTable.InvariantUtf8);
                var jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, options);

                jsonUtf8.WriteStartObject();

                switch (i)
                {
                    case 0:
                        jsonUtf8.WriteNumber("message", value);
                        break;
                    case 1:
                        jsonUtf8.WriteNumber("message".AsSpan(), value);
                        break;
                    case 2:
                        jsonUtf8.WriteNumber(Encoding.UTF8.GetBytes("message"), value);
                        break;
                }

                jsonUtf8.WriteEndObject();
                jsonUtf8.Flush();

                ArraySegment<byte> arraySegment = output.Formatted;
                string actualStr = Encoding.UTF8.GetString(arraySegment.Array, arraySegment.Offset, arraySegment.Count);

                Assert.True(expectedStr == actualStr, $"Case: {i}, | Expected: {expectedStr}, | Actual: {actualStr}, | Value: {value}");
            }
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void WriteStartEndWithOptions(bool formatted, bool skipValidation)
        {
            string expectedStr = GetStartEndExpectedString(prettyPrint: formatted);

            var output = new ArrayFormatterWrapper(1024, SymbolTable.InvariantUtf8);

            var options = new JsonWriterOptions
            {
                Formatted = formatted,
                SkipValidation = skipValidation
            };

            var jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, options);

            jsonUtf8.WriteStartArray();
            jsonUtf8.WriteStartObject();
            jsonUtf8.WriteEndObject();
            jsonUtf8.WriteEndArray();
            jsonUtf8.Flush();

            ArraySegment<byte> arraySegment = output.Formatted;
            string actualStr = Encoding.UTF8.GetString(arraySegment.Array, arraySegment.Offset, arraySegment.Count);

            Assert.Equal(expectedStr, actualStr);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void WriteArrayWithProperty(bool formatted, bool skipValidation)
        {
            string expectedStr = GetNestedArrayExpectedString(prettyPrint: formatted);

            var options = new JsonWriterOptions
            {
                Formatted = formatted,
                SkipValidation = skipValidation
            };

            for (int i = 0; i < 3; i++)
            {
                var output = new ArrayFormatterWrapper(1024, SymbolTable.InvariantUtf8);

                var jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, options);
                jsonUtf8.WriteStartObject();

                switch (i)
                {
                    case 0:
                        jsonUtf8.WriteStartArray("message");
                        break;
                    case 1:
                        jsonUtf8.WriteStartArray("message".AsSpan());
                        break;
                    case 2:
                        jsonUtf8.WriteStartArray(Encoding.UTF8.GetBytes("message"));
                        break;
                }

                jsonUtf8.WriteEndArray();
                jsonUtf8.WriteEndObject();
                jsonUtf8.Flush();

                ArraySegment<byte> arraySegment = output.Formatted;
                string actualStr = Encoding.UTF8.GetString(arraySegment.Array, arraySegment.Offset, arraySegment.Count);

                Assert.True(expectedStr == actualStr, $"Case: {i}, | Expected: {expectedStr}, | Actual: {actualStr}");
            }
        }

        /*[Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void WriteBasicJsonUtf8(bool prettyPrint)
        {
            int[] data = GetData(ExtraArraySize, 42, -10000, 10000);
            byte[] ExtraArray = Encoding.UTF8.GetBytes("ExtraArray");
            string expectedStr = GetExpectedString(prettyPrint, isUtf8: true, data);

            var output = new ArrayFormatterWrapper(1024, SymbolTable.InvariantUtf8);
            var jsonUtf8 = new Utf8JsonWriter<ArrayFormatterWrapper>(output, prettyPrint);

            jsonUtf8.WriteObjectStart();
            jsonUtf8.WriteAttribute("age", 42);
            jsonUtf8.WriteAttribute("first", null);
            jsonUtf8.WriteAttribute("last", "Smith");
            jsonUtf8.WriteArrayStart("phoneNumbers");
            jsonUtf8.WriteValue("425-000-1212");
            jsonUtf8.WriteValue("425-000-1213");
            jsonUtf8.WriteArrayEnd();
            jsonUtf8.WriteObjectStart("address");
            jsonUtf8.WriteAttribute("street", "1 Microsoft Way");
            jsonUtf8.WriteAttribute("city", "Redmond");
            jsonUtf8.WriteAttribute("zip", 98052);
            jsonUtf8.WriteObjectEnd();

            // Add a large array of values
            jsonUtf8.WriteArrayUtf8(ExtraArray, data);

            jsonUtf8.WriteObjectEnd();
            jsonUtf8.Flush();

            ArraySegment<byte> formatted = output.Formatted;
            string actualStr = Encoding.UTF8.GetString(formatted.Array, formatted.Offset, formatted.Count);

            Assert.Equal(expectedStr, actualStr);
        }

        private static int[] GetData(int size, int seed, int minValue, int maxValue)
        {
            int[] data = new int[size];
            Random rand = new Random(seed);

            for (int i = 0; i < ExtraArraySize; i++)
            {
                data[i] = rand.Next(minValue, maxValue);
            }

            return data;
        }*/

        private static string GetHelloWorldExpectedString(bool prettyPrint)
        {
            MemoryStream ms = new MemoryStream();
            TextWriter streamWriter = new StreamWriter(ms, new UTF8Encoding(false), 1024, true);

            var json = new Newtonsoft.Json.JsonTextWriter(streamWriter)
            {
                Formatting = prettyPrint ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None
            };

            json.WriteStartObject();
            json.WritePropertyName("message");
            json.WriteValue("Hello, World!");
            json.WriteEnd();

            json.Flush();

            return Encoding.UTF8.GetString(ms.ToArray());
        }

        private static string GetBooleanExpectedString(bool prettyPrint, bool value)
        {
            MemoryStream ms = new MemoryStream();
            TextWriter streamWriter = new StreamWriter(ms, new UTF8Encoding(false), 1024, true);

            var json = new Newtonsoft.Json.JsonTextWriter(streamWriter)
            {
                Formatting = prettyPrint ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None
            };

            json.WriteStartObject();
            json.WritePropertyName("message");
            json.WriteValue(value);
            json.WriteEnd();

            json.Flush();

            return Encoding.UTF8.GetString(ms.ToArray());
        }

        private static string GetNullExpectedString(bool prettyPrint)
        {
            MemoryStream ms = new MemoryStream();
            TextWriter streamWriter = new StreamWriter(ms, new UTF8Encoding(false), 1024, true);

            var json = new Newtonsoft.Json.JsonTextWriter(streamWriter)
            {
                Formatting = prettyPrint ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None
            };

            json.WriteStartObject();
            json.WritePropertyName("message");
            json.WriteNull();
            json.WriteEnd();

            json.Flush();

            return Encoding.UTF8.GetString(ms.ToArray());
        }

        private static string GetNumberExpectedString(bool prettyPrint, int value)
        {
            MemoryStream ms = new MemoryStream();
            TextWriter streamWriter = new StreamWriter(ms, new UTF8Encoding(false), 1024, true);

            var json = new Newtonsoft.Json.JsonTextWriter(streamWriter)
            {
                Formatting = prettyPrint ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None
            };

            json.WriteStartObject();
            json.WritePropertyName("message");
            json.WriteValue(value);
            json.WriteEnd();

            json.Flush();

            return Encoding.UTF8.GetString(ms.ToArray());
        }

        private static string GetStartEndExpectedString(bool prettyPrint)
        {
            MemoryStream ms = new MemoryStream();
            TextWriter streamWriter = new StreamWriter(ms, new UTF8Encoding(false), 1024, true);

            var json = new Newtonsoft.Json.JsonTextWriter(streamWriter)
            {
                Formatting = prettyPrint ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None
            };

            json.WriteStartArray();
            json.WriteStartObject();
            json.WriteEnd();
            json.WriteEnd();

            json.Flush();

            return Encoding.UTF8.GetString(ms.ToArray());
        }

        private static string GetNestedArrayExpectedString(bool prettyPrint)
        {
            MemoryStream ms = new MemoryStream();
            TextWriter streamWriter = new StreamWriter(ms, new UTF8Encoding(false), 1024, true);

            var json = new Newtonsoft.Json.JsonTextWriter(streamWriter)
            {
                Formatting = prettyPrint ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None
            };

            json.WriteStartObject();
            json.WritePropertyName("message");
            json.WriteStartArray();
            json.WriteEndArray();
            json.WriteEndObject();
            json.Flush();

            return Encoding.UTF8.GetString(ms.ToArray());
        }

        /*private static string GetExpectedString(bool prettyPrint, bool isUtf8, int[] data)
        {
            MemoryStream ms = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(ms, new UTF8Encoding(false), 1024, true);

            StringBuilder sb = new StringBuilder();
            StringWriter stringWriter = new StringWriter(sb);

            TextWriter writer = isUtf8 ? streamWriter : (TextWriter)stringWriter;

            var json = new Newtonsoft.Json.JsonTextWriter(writer)
            {
                Formatting = prettyPrint ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None
            };

            json.WriteStartObject();
            json.WritePropertyName("age");
            json.WriteValue(42);
            json.WritePropertyName("first");
            json.WriteValue((string)null);
            json.WritePropertyName("last");
            json.WriteValue("Smith");
            json.WritePropertyName("phoneNumbers");
            json.WriteStartArray();
            json.WriteValue("425-000-1212");
            json.WriteValue("425-000-1213");
            json.WriteEnd();
            json.WritePropertyName("address");
            json.WriteStartObject();
            json.WritePropertyName("street");
            json.WriteValue("1 Microsoft Way");
            json.WritePropertyName("city");
            json.WriteValue("Redmond");
            json.WritePropertyName("zip");
            json.WriteValue(98052);
            json.WriteEnd();

            // Add a large array of values
            json.WritePropertyName("ExtraArray");
            json.WriteStartArray();
            for (var i = 0; i < ExtraArraySize; i++)
            {
                json.WriteValue(data[i]);
            }
            json.WriteEnd();

            json.WriteEnd();

            json.Flush();

            return isUtf8 ? Encoding.UTF8.GetString(ms.ToArray()) : sb.ToString();
        }*/
    }
}
