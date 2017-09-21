// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Buffers.Text;
using System.Globalization;
using System.Runtime.CompilerServices;
using Xunit;

namespace System.Text.Primitives.Tests
{
    public class IntegerTests
    {
        const int NumberOfRandomSamples = 1000;

        static readonly SymbolTable[] SymbolTables = new SymbolTable[]
        {
            SymbolTable.InvariantUtf8,
            SymbolTable.InvariantUtf16,
        };

        static readonly ParsedFormat[] Formats = new ParsedFormat[]
        {
            new ParsedFormat('N', 0),
            new ParsedFormat('N', 1),
            new ParsedFormat('N', 10),
            new ParsedFormat('N', 30),
            new ParsedFormat('N', 255),
            new ParsedFormat('D', 0),
            new ParsedFormat('D', 1),
            new ParsedFormat('D', 10),
            new ParsedFormat('D', 30),
            new ParsedFormat('D', 255),
            new ParsedFormat('x', 0),
            new ParsedFormat('x', 1),
            new ParsedFormat('x', 10),
            new ParsedFormat('x', 30),
            new ParsedFormat('x', 255),
            new ParsedFormat('X', 0),
            new ParsedFormat('X', 1),
            new ParsedFormat('X', 10),
            new ParsedFormat('X', 30),
            new ParsedFormat('X', 255),
        };

        [Fact]
        public void SpecificIntegerTests()
        {
            foreach (var symbolTable in SymbolTables)
            {
                foreach (var format in Formats)
                {
                    Validate<ulong>(0, format, symbolTable);
                    Validate<ulong>(1, format, symbolTable);
                    Validate<ulong>(999999999999, format, symbolTable);
                    Validate<ulong>(1000000000000, format, symbolTable);
                    Validate<ulong>(ulong.MaxValue, format, symbolTable);

                    Validate<uint>(0, format, symbolTable);
                    Validate<uint>(1, format, symbolTable);
                    Validate<uint>(999999999, format, symbolTable);
                    Validate<uint>(1000000000, format, symbolTable);
                    Validate<uint>(uint.MaxValue, format, symbolTable);

                    Validate<ushort>(0, format, symbolTable);
                    Validate<ushort>(1, format, symbolTable);
                    Validate<ushort>(9999, format, symbolTable);
                    Validate<ushort>(10000, format, symbolTable);
                    Validate<ushort>(ushort.MaxValue, format, symbolTable);

                    Validate<byte>(0, format, symbolTable);
                    Validate<byte>(1, format, symbolTable);
                    Validate<byte>(99, format, symbolTable);
                    Validate<byte>(100, format, symbolTable);
                    Validate<byte>(byte.MaxValue, format, symbolTable);

                    Validate<long>(long.MinValue, format, symbolTable);
                    Validate<long>(-1000000000000, format, symbolTable);
                    Validate<long>(-999999999999, format, symbolTable);
                    Validate<long>(-1, format, symbolTable);
                    Validate<long>(0, format, symbolTable);
                    Validate<long>(1, format, symbolTable);
                    Validate<long>(999999999999, format, symbolTable);
                    Validate<long>(1000000000000, format, symbolTable);
                    Validate<long>(long.MaxValue, format, symbolTable);

                    Validate<int>(int.MinValue, format, symbolTable);
                    Validate<int>(-1000000000, format, symbolTable);
                    Validate<int>(-999999999, format, symbolTable);
                    Validate<int>(-1, format, symbolTable);
                    Validate<int>(0, format, symbolTable);
                    Validate<int>(1, format, symbolTable);
                    Validate<int>(999999999, format, symbolTable);
                    Validate<int>(1000000000, format, symbolTable);
                    Validate<int>(int.MaxValue, format, symbolTable);

                    Validate<short>(short.MinValue, format, symbolTable);
                    Validate<short>(-10000, format, symbolTable);
                    Validate<short>(-9999, format, symbolTable);
                    Validate<short>(-1, format, symbolTable);
                    Validate<short>(0, format, symbolTable);
                    Validate<short>(1, format, symbolTable);
                    Validate<short>(9999, format, symbolTable);
                    Validate<short>(10000, format, symbolTable);
                    Validate<short>(short.MaxValue, format, symbolTable);

                    Validate<sbyte>(sbyte.MaxValue, format, symbolTable);
                    Validate<sbyte>(-100, format, symbolTable);
                    Validate<sbyte>(-99, format, symbolTable);
                    Validate<sbyte>(-1, format, symbolTable);
                    Validate<sbyte>(0, format, symbolTable);
                    Validate<sbyte>(1, format, symbolTable);
                    Validate<sbyte>(99, format, symbolTable);
                    Validate<sbyte>(100, format, symbolTable);
                    Validate<sbyte>(sbyte.MaxValue, format, symbolTable);
                }
            }
        }

        [Fact]
        public void RandomIntegerTests()
        {
            for (var i = 0; i < NumberOfRandomSamples; i++)
            {
                foreach (var symbolTable in SymbolTables)
                {
                    foreach (var format in Formats)
                    {
                        ValidateRandom<ulong>(format, symbolTable);
                        ValidateRandom<uint>(format, symbolTable);
                        ValidateRandom<ushort>(format, symbolTable);
                        ValidateRandom<byte>(format, symbolTable);
                        ValidateRandom<long>(format, symbolTable);
                        ValidateRandom<int>(format, symbolTable);
                        ValidateRandom<short>(format, symbolTable);
                        ValidateRandom<sbyte>(format, symbolTable);
                    }
                }
            }
        }

        static void ValidateRandom<T>(ParsedFormat format, SymbolTable symbolTable)
        {
            Validate<T>(GetRandom<T>(), format, symbolTable);
        }

        static void Validate<T>(long value, ParsedFormat format, SymbolTable symbolTable)
        {
            Validate<T>(unchecked((ulong)value), format, symbolTable);
        }

        static void Validate<T>(ulong value, ParsedFormat format, SymbolTable symbolTable)
        {
            var formatString = format.Precision == 255 ? $"{format.Symbol}" : $"{format.Symbol}{format.Precision}";

            var span = new Span<byte>(new byte[128]);
            string expected;
            int written;

            if (typeof(T) == typeof(ulong))
            {
                expected = value.ToString(formatString, CultureInfo.InvariantCulture);
                Assert.True(Formatters.Custom.TryFormat(value, span, out written, format, symbolTable));
            }
            else if (typeof(T) == typeof(uint))
            {
                expected = ((uint)value).ToString(formatString, CultureInfo.InvariantCulture);
                Assert.True(Formatters.Custom.TryFormat((uint)value, span, out written, format, symbolTable));
            }
            else if (typeof(T) == typeof(ushort))
            {
                expected = ((ushort)value).ToString(formatString, CultureInfo.InvariantCulture);
                Assert.True(Formatters.Custom.TryFormat((ushort)value, span, out written, format, symbolTable));
            }
            else if (typeof(T) == typeof(byte))
            {
                expected = ((byte)value).ToString(formatString, CultureInfo.InvariantCulture);
                Assert.True(Formatters.Custom.TryFormat((byte)value, span, out written, format, symbolTable));
            }
            else if (typeof(T) == typeof(long))
            {
                expected = ((long)value).ToString(formatString, CultureInfo.InvariantCulture);
                Assert.True(Formatters.Custom.TryFormat((long)value, span, out written, format, symbolTable));
            }
            else if (typeof(T) == typeof(int))
            {
                expected = ((int)value).ToString(formatString, CultureInfo.InvariantCulture);
                Assert.True(Formatters.Custom.TryFormat((int)value, span, out written, format, symbolTable));
            }
            else if (typeof(T) == typeof(short))
            {
                expected = ((short)value).ToString(formatString, CultureInfo.InvariantCulture);
                Assert.True(Formatters.Custom.TryFormat((short)value, span, out written, format, symbolTable));
            }
            else if (typeof(T) == typeof(sbyte))
            {
                expected = ((sbyte)value).ToString(formatString, CultureInfo.InvariantCulture);
                Assert.True(Formatters.Custom.TryFormat((sbyte)value, span, out written, format, symbolTable));
            }
            else
                throw new NotSupportedException();

            string actual = TestHelper.SpanToString(span.Slice(0, written), symbolTable);
            Assert.Equal(expected, actual);
        }

        static readonly Random Rnd = new Random(234922);

        static ulong GetRandom<T>()
        {
            int size = Unsafe.SizeOf<T>();
            byte[] data = new byte[size];
            Rnd.NextBytes(data);

            if (typeof(T) == typeof(ulong))
                return BitConverter.ToUInt64(data, 0);
            else if (typeof(T) == typeof(uint))
                return BitConverter.ToUInt32(data, 0);
            else if (typeof(T) == typeof(ushort))
                return BitConverter.ToUInt16(data, 0);
            else if (typeof(T) == typeof(long))
                return (ulong)BitConverter.ToInt64(data, 0);
            else if (typeof(T) == typeof(int))
                return (ulong)BitConverter.ToInt32(data, 0);
            else if (typeof(T) == typeof(short))
                return (ulong)BitConverter.ToInt16(data, 0);
            else if (typeof(T) == typeof(byte) || typeof(T) == typeof(sbyte))
                return data[0];
            else
                throw new NotSupportedException();
        }
    }
}
