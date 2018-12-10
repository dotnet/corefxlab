// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;
using System.Text.Formatting;
using System.Buffers.Text;
using System.IO;

namespace System.Text.JsonLab.Tests
{
    public class Utf8JsonWriterTests
    {
        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void InvalidJsonMismatch(bool formatted, bool skipValidation)
        {
            var state = new JsonWriterState(options: new JsonWriterOptions { Formatted = formatted, SkipValidation = skipValidation });

            var output = new ArrayFormatterWrapper(1024, SymbolTable.InvariantUtf8);

            var jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);
            try
            {
                jsonUtf8.WriteEndArray();
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }

            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);
            try
            {
                jsonUtf8.WriteEndObject();
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }

            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);
            try
            {
                jsonUtf8.WriteStartArray("property at start");
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }

            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);
            try
            {
                jsonUtf8.WriteStartObject("property at start");
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }

            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);
            try
            {
                jsonUtf8.WriteStartArray();
                jsonUtf8.WriteStartArray("property inside array");
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }

            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);
            try
            {
                jsonUtf8.WriteStartObject();
                jsonUtf8.WriteStartObject();
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }

            // TODO: Need write value
            /*jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);
            try
            {
                jsonUtf8.WriteEndArray();
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }

            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);
            try
            {
                jsonUtf8.WriteEndObject();
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }*/

            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);
            try
            {
                jsonUtf8.WriteStartArray();
                jsonUtf8.WriteEndObject();
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }

            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);
            try
            {
                jsonUtf8.WriteStartObject();
                jsonUtf8.WriteEndArray();
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }

            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);
            try
            {
                jsonUtf8.WriteStartArray();
                jsonUtf8.WriteStartArray();
                jsonUtf8.WriteEndArray();
                jsonUtf8.WriteEndObject();
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }

            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);
            try
            {
                jsonUtf8.WriteStartObject();
                jsonUtf8.WriteStartObject("some object");
                jsonUtf8.WriteEndObject();
                jsonUtf8.WriteEndArray();
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }

            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);
            try
            {
                jsonUtf8.WriteStartArray();
                jsonUtf8.WriteStartObject("some object");
                jsonUtf8.WriteEndObject();
                jsonUtf8.WriteEndObject();
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }

            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);
            try
            {
                jsonUtf8.WriteStartObject();
                jsonUtf8.WriteStartArray("test array");
                jsonUtf8.WriteEndArray();
                jsonUtf8.WriteEndArray();
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }

            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);
            try
            {
                jsonUtf8.WriteStartArray();
                jsonUtf8.WriteEndArray();
                jsonUtf8.WriteEndArray();
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }

            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);
            try
            {
                jsonUtf8.WriteStartObject();
                jsonUtf8.WriteEndObject();
                jsonUtf8.WriteEndObject();
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }

            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);
            try
            {
                jsonUtf8.WriteStartArray();
                jsonUtf8.WriteStartArray();
                jsonUtf8.WriteEndObject();
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }

            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);
            try
            {
                jsonUtf8.WriteStartObject();
                jsonUtf8.WriteStartObject("test object");
                jsonUtf8.WriteEndArray();
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void InvalidJsonPrimitive(bool formatted, bool skipValidation)
        {
            var state = new JsonWriterState(options: new JsonWriterOptions { Formatted = formatted, SkipValidation = skipValidation });

            var output = new ArrayFormatterWrapper(1024, SymbolTable.InvariantUtf8);

            var jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);
            try
            {
                jsonUtf8.WriteValue(12345);
                jsonUtf8.WriteValue(12345);
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }

            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);
            try
            {
                jsonUtf8.WriteValue(12345);
                jsonUtf8.WriteStartArray();
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }

            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);
            try
            {
                jsonUtf8.WriteValue(12345);
                jsonUtf8.WriteStartObject();
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }

            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);
            try
            {
                jsonUtf8.WriteValue(12345);
                jsonUtf8.WriteStartArray("property name");
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }

            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);
            try
            {
                jsonUtf8.WriteValue(12345);
                jsonUtf8.WriteStartObject("property name");
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }

            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);
            try
            {
                jsonUtf8.WriteValue(12345);
                jsonUtf8.WriteString("property name", "value");
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }

            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);
            try
            {
                jsonUtf8.WriteValue(12345);
                jsonUtf8.WriteEndArray();
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }

            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);
            try
            {
                jsonUtf8.WriteValue(12345);
                jsonUtf8.WriteEndObject();
                WriterDidNotThrow(skipValidation);
            }
            catch (JsonWriterException) { }
        }

        private static void WriterDidNotThrow(bool skipValidation)
        {
            if (skipValidation)
                Assert.True(true, "Did not expect JsonWriterException to be thrown since validation was skipped.");
            else
                Assert.True(false, "Expected JsonWriterException to be thrown when validation is enabled.");
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void WriteSingleValueWithOptions(bool formatted, bool skipValidation)
        {
            string expectedStr = "123456789012345";

            var state = new JsonWriterState(options: new JsonWriterOptions { Formatted = formatted, SkipValidation = skipValidation });

            for (int i = 0; i < 3; i++)
            {
                var output = new ArrayFormatterWrapper(1024, SymbolTable.InvariantUtf8);
                var jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);

                jsonUtf8.WriteValue(123456789012345);

                jsonUtf8.Flush();

                ArraySegment<byte> arraySegment = output.Formatted;
                string actualStr = Encoding.UTF8.GetString(arraySegment.Array, arraySegment.Offset, arraySegment.Count);

                Assert.True(expectedStr == actualStr, $"Case: {i}, | Expected: {expectedStr}, | Actual: {actualStr}");
            }
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void WriteHelloWorldWithOptions(bool formatted, bool skipValidation)
        {
            string expectedStr = GetHelloWorldExpectedString(prettyPrint: formatted);

            var state = new JsonWriterState(options: new JsonWriterOptions { Formatted = formatted, SkipValidation = skipValidation });

            for (int i = 0; i < 9; i++)
            {
                var output = new ArrayFormatterWrapper(1024, SymbolTable.InvariantUtf8);
                var jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);

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
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void WriteStartEndWithOptions(bool formatted, bool skipValidation)
        {
            string expectedStr = GetStartEndExpectedString(prettyPrint: formatted);

            var output = new ArrayFormatterWrapper(1024, SymbolTable.InvariantUtf8);

            var state = new JsonWriterState(options: new JsonWriterOptions { Formatted = formatted, SkipValidation = skipValidation });

            var jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);

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
        public void WriteArrayWithPropertyWithOptions(bool formatted, bool skipValidation)
        {
            string expectedStr = GetArrayWithPropertyExpectedString(prettyPrint: formatted);

            var state = new JsonWriterState(options: new JsonWriterOptions { Formatted = formatted, SkipValidation = skipValidation });

            for (int i = 0; i < 3; i++)
            {
                var output = new ArrayFormatterWrapper(1024, SymbolTable.InvariantUtf8);

                var jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);
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

            var state = new JsonWriterState(options: new JsonWriterOptions { Formatted = formatted, SkipValidation = skipValidation });

            for (int i = 0; i < 3; i++)
            {
                var output = new ArrayFormatterWrapper(1024, SymbolTable.InvariantUtf8);
                var jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);

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

            var state = new JsonWriterState(options: new JsonWriterOptions { Formatted = formatted, SkipValidation = skipValidation });

            for (int i = 0; i < 3; i++)
            {
                var output = new ArrayFormatterWrapper(1024, SymbolTable.InvariantUtf8);
                var jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);

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
        public void WriteIntegerValueWithOptions(bool formatted, bool skipValidation, int value)
        {
            string expectedStr = GetIntegerExpectedString(prettyPrint: formatted, value);

            var state = new JsonWriterState(options: new JsonWriterOptions { Formatted = formatted, SkipValidation = skipValidation });

            for (int i = 0; i < 3; i++)
            {
                var output = new ArrayFormatterWrapper(1024, SymbolTable.InvariantUtf8);
                var jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);

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
        public void WriteNumbersWithOptions(bool formatted, bool skipValidation)
        {
            var random = new Random(42);
            const int numberOfItems = 1_000;

            var ints = new int[numberOfItems];
            ints[0] = 0;
            ints[1] = int.MaxValue;
            ints[2] = int.MinValue;
            ints[3] = 12345;
            ints[4] = -12345;
            for (int i = 5; i < numberOfItems; i++)
            {
                ints[i] = random.Next(int.MinValue, int.MaxValue);
            }

            var uints = new uint[numberOfItems];
            uints[0] = uint.MaxValue;
            uints[1] = uint.MinValue;
            uints[2] = 3294967295;
            for (int i = 3; i < numberOfItems; i++)
            {
                uint thirtyBits = (uint)random.Next(1 << 30);
                uint twoBits = (uint)random.Next(1 << 2);
                uint fullRange = (thirtyBits << 2) | twoBits;
                uints[i] = fullRange;
            }

            var longs = new long[numberOfItems];
            longs[0] = 0;
            longs[1] = long.MaxValue;
            longs[2] = long.MinValue;
            longs[3] = 12345678901;
            longs[4] = -12345678901;
            for (int i = 5; i < numberOfItems; i++)
            {
                long value = random.Next(int.MinValue, int.MaxValue);
                value += value < 0 ? int.MinValue : int.MaxValue;
                longs[i] = value;
            }

            var ulongs = new ulong[numberOfItems];
            ulongs[0] = ulong.MaxValue;
            ulongs[1] = ulong.MinValue;
            ulongs[2] = 10446744073709551615;
            for (int i = 3; i < numberOfItems; i++)
            {

            }

            var doubles = new double[numberOfItems * 2];
            doubles[0] = 0.00;
            doubles[1] = double.MaxValue;
            doubles[2] = double.MinValue;
            doubles[3] = 12.345e1;
            doubles[4] = -123.45e1;
            for (int i = 5; i < numberOfItems; i++)
            {
                var value = random.NextDouble();
                if (value < 0.5)
                {
                    doubles[i] = random.NextDouble() * double.MinValue;
                }
                else
                {
                    doubles[i] = random.NextDouble() * double.MaxValue;
                }
            }

            for (int i = numberOfItems; i < numberOfItems * 2; i++)
            {
                var value = random.NextDouble();
                if (value < 0.5)
                {
                    doubles[i] = random.NextDouble() * -1_000_000;
                }
                else
                {
                    doubles[i] = random.NextDouble() * 1_000_000;
                }
            }

            var floats = new float[numberOfItems];
            floats[0] = 0.00f;
            floats[1] = float.MaxValue;
            floats[2] = float.MinValue;
            floats[3] = 12.345e1f;
            floats[4] = -123.45e1f;
            for (int i = 5; i < numberOfItems; i++)
            {
                double mantissa = (random.NextDouble() * 2.0) - 1.0;
                double exponent = Math.Pow(2.0, random.Next(-126, 128));
                floats[i] = (float)(mantissa * exponent);
            }

            var decimals = new decimal[numberOfItems * 2];
            decimals[0] = (decimal)0.00;
            decimals[1] = decimal.MaxValue;
            decimals[2] = decimal.MinValue;
            decimals[3] = (decimal)12.345e1;
            decimals[4] = (decimal)-123.45e1;
            for (int i = 5; i < numberOfItems; i++)
            {
                var value = random.NextDouble();
                if (value < 0.5)
                {
                    decimals[i] = (decimal)(random.NextDouble() * -78E14);
                }
                else
                {
                    decimals[i] = (decimal)(random.NextDouble() * 78E14);
                }
            }

            for (int i = numberOfItems; i < numberOfItems * 2; i++)
            {
                var value = random.NextDouble();
                if (value < 0.5)
                {
                    decimals[i] = (decimal)(random.NextDouble() * -1_000_000);
                }
                else
                {
                    decimals[i] = (decimal)(random.NextDouble() * 1_000_000);
                }
            }

            string expectedStr = GetNumbersExpectedString(prettyPrint: formatted, ints, uints, longs, ulongs, floats, doubles, decimals);

            var state = new JsonWriterState(options: new JsonWriterOptions { Formatted = formatted, SkipValidation = skipValidation });

            for (int j = 0; j < 3; j++)
            {
                var output = new ArrayFormatterWrapper(1024, SymbolTable.InvariantUtf8);
                var jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);

                string keyString = "message";
                ReadOnlySpan<char> keyUtf16 = keyString;
                ReadOnlySpan<byte> keyUtf8 = Encoding.UTF8.GetBytes(keyString);

                jsonUtf8.WriteStartObject();

                switch (j)
                {
                    case 0:
                        for (int i = 0; i < floats.Length; i++)
                            jsonUtf8.WriteNumber(keyString, floats[i]);
                        for (int i = 0; i < ints.Length; i++)
                            jsonUtf8.WriteNumber(keyString, ints[i]);
                        for (int i = 0; i < uints.Length; i++)
                            jsonUtf8.WriteNumber(keyString, uints[i]);
                        for (int i = 0; i < doubles.Length; i++)
                            jsonUtf8.WriteNumber(keyString, doubles[i]);
                        for (int i = 0; i < longs.Length; i++)
                            jsonUtf8.WriteNumber(keyString, longs[i]);
                        for (int i = 0; i < ulongs.Length; i++)
                            jsonUtf8.WriteNumber(keyString, ulongs[i]);
                        for (int i = 0; i < decimals.Length; i++)
                            jsonUtf8.WriteNumber(keyString, decimals[i]);
                        jsonUtf8.WriteStartArray(keyString);
                        break;
                    case 1:
                        for (int i = 0; i < floats.Length; i++)
                            jsonUtf8.WriteNumber(keyUtf16, floats[i]);
                        for (int i = 0; i < ints.Length; i++)
                            jsonUtf8.WriteNumber(keyUtf16, ints[i]);
                        for (int i = 0; i < uints.Length; i++)
                            jsonUtf8.WriteNumber(keyUtf16, uints[i]);
                        for (int i = 0; i < doubles.Length; i++)
                            jsonUtf8.WriteNumber(keyUtf16, doubles[i]);
                        for (int i = 0; i < longs.Length; i++)
                            jsonUtf8.WriteNumber(keyUtf16, longs[i]);
                        for (int i = 0; i < ulongs.Length; i++)
                            jsonUtf8.WriteNumber(keyUtf16, ulongs[i]);
                        for (int i = 0; i < decimals.Length; i++)
                            jsonUtf8.WriteNumber(keyUtf16, decimals[i]);
                        jsonUtf8.WriteStartArray(keyUtf16);
                        break;
                    case 2:
                        for (int i = 0; i < floats.Length; i++)
                            jsonUtf8.WriteNumber(keyUtf8, floats[i]);
                        for (int i = 0; i < ints.Length; i++)
                            jsonUtf8.WriteNumber(keyUtf8, ints[i]);
                        for (int i = 0; i < uints.Length; i++)
                            jsonUtf8.WriteNumber(keyUtf8, uints[i]);
                        for (int i = 0; i < doubles.Length; i++)
                            jsonUtf8.WriteNumber(keyUtf8, doubles[i]);
                        for (int i = 0; i < longs.Length; i++)
                            jsonUtf8.WriteNumber(keyUtf8, longs[i]);
                        for (int i = 0; i < ulongs.Length; i++)
                            jsonUtf8.WriteNumber(keyUtf8, ulongs[i]);
                        for (int i = 0; i < decimals.Length; i++)
                            jsonUtf8.WriteNumber(keyUtf8, decimals[i]);
                        jsonUtf8.WriteStartArray(keyUtf8);
                        break;
                }

                jsonUtf8.WriteValue(floats[0]);
                jsonUtf8.WriteValue(ints[0]);
                jsonUtf8.WriteValue(uints[0]);
                jsonUtf8.WriteValue(doubles[0]);
                jsonUtf8.WriteValue(longs[0]);
                jsonUtf8.WriteValue(ulongs[0]);
                jsonUtf8.WriteValue(decimals[0]);
                jsonUtf8.WriteEndArray();

                jsonUtf8.WriteEndObject();
                jsonUtf8.Flush();

                ArraySegment<byte> arraySegment = output.Formatted;
                string actualStr = Encoding.UTF8.GetString(arraySegment.Array, arraySegment.Offset, arraySegment.Count);
            }

            // TODO: The output doesn't match what JSON.NET does (different rounding/e-notation).
            //Assert.Equal(expectedStr, actualStr);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void WriteGuidsValueWithOptions(bool formatted, bool skipValidation)
        {
            var random = new Random(42);
            const int numberOfItems = 1_000;

            var guids = new Guid[numberOfItems];
            for (int i = 0; i < numberOfItems; i++)
                guids[i] = Guid.NewGuid();

            string expectedStr = GetGuidsExpectedString(prettyPrint: formatted, guids);

            var state = new JsonWriterState(options: new JsonWriterOptions { Formatted = formatted, SkipValidation = skipValidation });

            string keyString = "message";
            ReadOnlySpan<char> keyUtf16 = keyString;
            ReadOnlySpan<byte> keyUtf8 = Encoding.UTF8.GetBytes(keyString);

            for (int i = 0; i < 3; i++)
            {
                var output = new ArrayFormatterWrapper(1024, SymbolTable.InvariantUtf8);
                var jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);

                jsonUtf8.WriteStartObject();

                switch (i)
                {
                    case 0:
                        for (int j = 0; j < numberOfItems; j++)
                            jsonUtf8.WriteString(keyString, guids[j]);
                        jsonUtf8.WriteStartArray(keyString);
                        break;
                    case 1:
                        for (int j = 0; j < numberOfItems; j++)
                            jsonUtf8.WriteString(keyUtf16, guids[j]);
                        jsonUtf8.WriteStartArray(keyUtf16);
                        break;
                    case 2:
                        for (int j = 0; j < numberOfItems; j++)
                            jsonUtf8.WriteString(keyUtf8, guids[j]);
                        jsonUtf8.WriteStartArray(keyUtf8);
                        break;
                }

                jsonUtf8.WriteValue(guids[0]);
                jsonUtf8.WriteEndArray();

                jsonUtf8.WriteEndObject();
                jsonUtf8.Flush();

                ArraySegment<byte> arraySegment = output.Formatted;
                string actualStr = Encoding.UTF8.GetString(arraySegment.Array, arraySegment.Offset, arraySegment.Count);

                Assert.Equal(expectedStr, actualStr);
            }
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void WriteDatesValueWithOptions(bool formatted, bool skipValidation)
        {
            var random = new Random(42);
            const int numberOfItems = 1_000;

            var start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;

            var dates = new DateTime[numberOfItems];
            for (int i = 0; i < numberOfItems; i++)
                dates[i] = start.AddDays(random.Next(range));

            string expectedStr = GetDatesExpectedString(prettyPrint: formatted, dates);

            var state = new JsonWriterState(options: new JsonWriterOptions { Formatted = formatted, SkipValidation = skipValidation });

            string keyString = "message";
            ReadOnlySpan<char> keyUtf16 = keyString;
            ReadOnlySpan<byte> keyUtf8 = Encoding.UTF8.GetBytes(keyString);

            for (int i = 0; i < 3; i++)
            {
                var output = new ArrayFormatterWrapper(1024, SymbolTable.InvariantUtf8);
                var jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);

                jsonUtf8.WriteStartObject();

                switch (i)
                {
                    case 0:
                        for (int j = 0; j < numberOfItems; j++)
                            jsonUtf8.WriteString(keyString, dates[j]);
                        jsonUtf8.WriteStartArray(keyString);
                        break;
                    case 1:
                        for (int j = 0; j < numberOfItems; j++)
                            jsonUtf8.WriteString(keyUtf16, dates[j]);
                        jsonUtf8.WriteStartArray(keyUtf16);
                        break;
                    case 2:
                        for (int j = 0; j < numberOfItems; j++)
                            jsonUtf8.WriteString(keyUtf8, dates[j]);
                        jsonUtf8.WriteStartArray(keyUtf8);
                        break;
                }

                jsonUtf8.WriteValue(dates[0]);
                jsonUtf8.WriteEndArray();

                jsonUtf8.WriteEndObject();
                jsonUtf8.Flush();

                ArraySegment<byte> arraySegment = output.Formatted;
                string actualStr = Encoding.UTF8.GetString(arraySegment.Array, arraySegment.Offset, arraySegment.Count);

                Assert.Equal(expectedStr, actualStr);
            }
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void WriteArrayOfInt64ValuesWithOptions(bool formatted, bool skipValidation)
        {
            var random = new Random(42);
            const int numberOfItems = 6;

            var longs = new long[numberOfItems];
            longs[0] = 0;
            longs[1] = long.MaxValue;
            longs[2] = long.MinValue;
            longs[3] = 12345678901;
            longs[4] = -12345678901;
            for (int i = 5; i < numberOfItems; i++)
            {
                long value = random.Next(int.MinValue, int.MaxValue);
                value += value < 0 ? int.MinValue : int.MaxValue;
                longs[i] = value;
            }

            var state = new JsonWriterState(options: new JsonWriterOptions { Formatted = formatted, SkipValidation = skipValidation });

            var output = new ArrayFormatterWrapper(1024, SymbolTable.InvariantUtf8);
            var jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);

            jsonUtf8.WriteStartObject();
            jsonUtf8.WriteStartArray(Encoding.UTF8.GetBytes("numbers"));

            for (int i = 0; i < longs.Length; i++)
                jsonUtf8.WriteValue(longs[i]);

            jsonUtf8.WriteEndArray();
            jsonUtf8.WriteEndObject();
            jsonUtf8.Flush();

            ArraySegment<byte> arraySegment = output.Formatted;
            string actualStr = Encoding.UTF8.GetString(arraySegment.Array, arraySegment.Offset, arraySegment.Count);

            string expectedStr = GetArrayOfInt64ExpectedString(prettyPrint: formatted, longs);
            Assert.Equal(expectedStr, actualStr);

            output = new ArrayFormatterWrapper(1024, SymbolTable.InvariantUtf8);
            jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);

            jsonUtf8.WriteStartObject();
            jsonUtf8.WriteArray(Encoding.UTF8.GetBytes("numbers"), longs);
            jsonUtf8.WriteEndObject();
            jsonUtf8.Flush();

            arraySegment = output.Formatted;
            actualStr = Encoding.UTF8.GetString(arraySegment.Array, arraySegment.Offset, arraySegment.Count);
            Assert.Equal(expectedStr, actualStr);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void WriteGuidArrayWithOptions(bool formatted, bool skipValidation)
        {
            var random = new Random(42);
            const int numberOfItems = 1_000;

            var guids = new Guid[numberOfItems];
            for (int i = 0; i < numberOfItems; i++)
                guids[i] = Guid.NewGuid();

            string expectedStr = GetGuidArrayExpectedString(prettyPrint: formatted, guids);

            var state = new JsonWriterState(options: new JsonWriterOptions { Formatted = formatted, SkipValidation = skipValidation });

            var output = new ArrayFormatterWrapper(1024, SymbolTable.InvariantUtf8);
            var jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);

            jsonUtf8.WriteStartObject();

            jsonUtf8.WriteArray(Encoding.UTF8.GetBytes("guids"), guids);

            jsonUtf8.WriteEndObject();
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
        public void WriteDateArrayWithOptions(bool formatted, bool skipValidation)
        {
            var random = new Random(42);
            const int numberOfItems = 1_000;

            var start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;

            var dates = new DateTime[numberOfItems];
            for (int i = 0; i < numberOfItems; i++)
                dates[i] = start.AddDays(random.Next(range));

            string expectedStr = GetDateArrayExpectedString(prettyPrint: formatted, dates);

            var state = new JsonWriterState(options: new JsonWriterOptions { Formatted = formatted, SkipValidation = skipValidation });

            var output = new ArrayFormatterWrapper(1024, SymbolTable.InvariantUtf8);
            var jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);

            jsonUtf8.WriteStartObject();

            jsonUtf8.WriteArray(Encoding.UTF8.GetBytes("dates"), dates);

            jsonUtf8.WriteEndObject();
            jsonUtf8.Flush();

            ArraySegment<byte> arraySegment = output.Formatted;
            string actualStr = Encoding.UTF8.GetString(arraySegment.Array, arraySegment.Offset, arraySegment.Count);

            Assert.Equal(expectedStr, actualStr);
        }

        // TODO: Move to outerloop
        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void WriteLargeKeyValue(bool formatted, bool skipValidation)
        {
            var state = new JsonWriterState(options: new JsonWriterOptions { Formatted = formatted, SkipValidation = skipValidation });

            Span<byte> key = new byte[1_000_000_001];
            key.Fill((byte)'a');
            Span<byte> value = new byte[1_000_000_001];
            value.Fill((byte)'b');

            WriteTooLargeHelper(state, key, value);
            WriteTooLargeHelper(state, key.Slice(0, 1_000_000_000), value);
            WriteTooLargeHelper(state, key, value.Slice(0, 1_000_000_000));
            WriteTooLargeHelper(state, key.Slice(0, 1_000_000_000 / 3), value.Slice(0, 1_000_000_000 / 3), noThrow: true);
        }

        private static void WriteTooLargeHelper(JsonWriterState state, ReadOnlySpan<byte> key, ReadOnlySpan<byte> value, bool noThrow = false)
        {
            var output = new ArrayFormatterWrapper(1024, SymbolTable.InvariantUtf8);
            var jsonUtf8 = new Utf8JsonWriter2<ArrayFormatterWrapper>(output, state);

            jsonUtf8.WriteStartObject();

            try
            {
                jsonUtf8.WriteString(key, value);

                if (!noThrow)
                {
                    Assert.True(false, $"Expected ArgumentException for data too large wasn't thrown. KeyLength: {key.Length} | ValueLength: {value.Length}");
                }
            }
            catch (ArgumentException)
            {
                if (noThrow)
                {
                    Assert.True(false, $"Expected writing large key/value to succeed. KeyLength: {key.Length} | ValueLength: {value.Length}");
                }
            }

            jsonUtf8.WriteEndObject();
            jsonUtf8.Flush();
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void WriteToStream(bool formatted, bool skipValidation)
        {
            var buffer = new byte[1_000];
            var expectedString = "";
            var actualString = "";

            using (var memoryStream = new MemoryStream(buffer))
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    using (var json = new Newtonsoft.Json.JsonTextWriter(streamWriter))
                    {
                        json.Formatting = formatted ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None;

                        json.WriteStartObject();
                        for (int i = 0; i < 10; i++)
                        {
                            json.WritePropertyName("message");
                            json.WriteValue("Hello, World!");
                        }
                        json.WriteEnd();
                    }
                }
                expectedString = Encoding.UTF8.GetString(memoryStream.ToArray());
            }

            using (var memoryStream = new MemoryStream(buffer))
            {
                var state = new JsonWriterState(options: new JsonWriterOptions { Formatted = formatted, SkipValidation = skipValidation });

                Utf8JsonWriter2<Buffers.IBufferWriter<byte>> json = Utf8JsonWriter2.CreateFromStream(memoryStream, state);
                json.WriteStartObject();
                for (int i = 0; i < 10; i++)
                    json.WriteString("message", "Hello, World!");
                json.WriteEndObject();

                json.Dispose();

                actualString = Encoding.UTF8.GetString(memoryStream.ToArray());
            }

            Assert.Equal(expectedString, actualString);
        }

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

        private static string GetArrayWithPropertyExpectedString(bool prettyPrint)
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

        private static string GetIntegerExpectedString(bool prettyPrint, int value)
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

        private static string GetNumbersExpectedString(bool prettyPrint, int[] ints, uint[] uints, long[] longs, ulong[] ulongs, float[] floats, double[] doubles, decimal[] decimals)
        {
            MemoryStream ms = new MemoryStream();
            TextWriter streamWriter = new StreamWriter(ms, new UTF8Encoding(false), 1024, true);

            var json = new Newtonsoft.Json.JsonTextWriter(streamWriter)
            {
                Formatting = prettyPrint ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None
            };

            json.WriteStartObject();

            for (int i = 0; i < floats.Length; i++)
            {
                json.WritePropertyName("message");
                json.WriteValue(floats[i]);
            }
            for (int i = 0; i < ints.Length; i++)
            {
                json.WritePropertyName("message");
                json.WriteValue(ints[i]);
            }
            for (int i = 0; i < uints.Length; i++)
            {
                json.WritePropertyName("message");
                json.WriteValue(uints[i]);
            }
            for (int i = 0; i < doubles.Length; i++)
            {
                json.WritePropertyName("message");
                json.WriteValue(doubles[i]);
            }
            for (int i = 0; i < longs.Length; i++)
            {
                json.WritePropertyName("message");
                json.WriteValue(longs[i]);
            }
            for (int i = 0; i < ulongs.Length; i++)
            {
                json.WritePropertyName("message");
                json.WriteValue(ulongs[i]);
            }
            for (int i = 0; i < decimals.Length; i++)
            {
                json.WritePropertyName("message");
                json.WriteValue(decimals[i]);
            }

            json.WritePropertyName("message");
            json.WriteStartArray();
            json.WriteValue(floats[0]);
            json.WriteValue(ints[0]);
            json.WriteValue(uints[0]);
            json.WriteValue(doubles[0]);
            json.WriteValue(longs[0]);
            json.WriteValue(ulongs[0]);
            json.WriteValue(decimals[0]);
            json.WriteEndArray();

            json.WriteEnd();

            json.Flush();

            return Encoding.UTF8.GetString(ms.ToArray());
        }

        private static string GetGuidsExpectedString(bool prettyPrint, Guid[] guids)
        {
            MemoryStream ms = new MemoryStream();
            TextWriter streamWriter = new StreamWriter(ms, new UTF8Encoding(false), 1024, true);

            var json = new Newtonsoft.Json.JsonTextWriter(streamWriter)
            {
                Formatting = prettyPrint ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None
            };

            json.WriteStartObject();

            for (int i = 0; i < guids.Length; i++)
            {
                json.WritePropertyName("message");
                json.WriteValue(guids[i]);
            }

            json.WritePropertyName("message");
            json.WriteStartArray();
            json.WriteValue(guids[0]);
            json.WriteEnd();

            json.WriteEnd();

            json.Flush();

            return Encoding.UTF8.GetString(ms.ToArray());
        }

        private static string GetDatesExpectedString(bool prettyPrint, DateTime[] dates)
        {
            MemoryStream ms = new MemoryStream();
            TextWriter streamWriter = new StreamWriter(ms, new UTF8Encoding(false), 1024, true);

            var json = new Newtonsoft.Json.JsonTextWriter(streamWriter)
            {
                Formatting = prettyPrint ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None,
                DateFormatString = "G"
            };

            json.WriteStartObject();

            for (int i = 0; i < dates.Length; i++)
            {
                json.WritePropertyName("message");
                json.WriteValue(dates[i]);
            }

            json.WritePropertyName("message");
            json.WriteStartArray();
            json.WriteValue(dates[0]);
            json.WriteEnd();

            json.WriteEnd();

            json.Flush();

            return Encoding.UTF8.GetString(ms.ToArray());
        }

        private static string GetGuidArrayExpectedString(bool prettyPrint, Guid[] dates)
        {
            MemoryStream ms = new MemoryStream();
            TextWriter streamWriter = new StreamWriter(ms, new UTF8Encoding(false), 1024, true);

            var json = new Newtonsoft.Json.JsonTextWriter(streamWriter)
            {
                Formatting = prettyPrint ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None,
                DateFormatString = "G"
            };

            json.WriteStartObject();
            json.WritePropertyName("guids");
            json.WriteStartArray();
            for (int i = 0; i < dates.Length; i++)
            {
                json.WriteValue(dates[i]);
            }
            json.WriteEnd();
            json.WriteEnd();

            json.Flush();

            return Encoding.UTF8.GetString(ms.ToArray());
        }

        private static string GetDateArrayExpectedString(bool prettyPrint, DateTime[] dates)
        {
            MemoryStream ms = new MemoryStream();
            TextWriter streamWriter = new StreamWriter(ms, new UTF8Encoding(false), 1024, true);

            var json = new Newtonsoft.Json.JsonTextWriter(streamWriter)
            {
                Formatting = prettyPrint ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None,
                DateFormatString = "G"
            };

            json.WriteStartObject();
            json.WritePropertyName("dates");
            json.WriteStartArray();
            for (int i = 0; i < dates.Length; i++)
            {
                json.WriteValue(dates[i]);
            }
            json.WriteEnd();
            json.WriteEnd();

            json.Flush();

            return Encoding.UTF8.GetString(ms.ToArray());
        }

        private static string GetArrayOfInt64ExpectedString(bool prettyPrint, long[] longs)
        {
            MemoryStream ms = new MemoryStream();
            TextWriter streamWriter = new StreamWriter(ms, new UTF8Encoding(false), 1024, true);

            var json = new Newtonsoft.Json.JsonTextWriter(streamWriter)
            {
                Formatting = prettyPrint ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None
            };

            json.WriteStartObject();
            json.WritePropertyName("numbers");
            json.WriteStartArray();

            for (int i = 0; i < longs.Length; i++)
                json.WriteValue(longs[i]);

            json.WriteEnd();
            json.WriteEnd();

            json.Flush();

            return Encoding.UTF8.GetString(ms.ToArray());
        }
    }
}
