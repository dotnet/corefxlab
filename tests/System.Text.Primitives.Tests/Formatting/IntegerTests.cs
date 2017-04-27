// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;
using System.Runtime.CompilerServices;
using Xunit;

namespace System.Text.Primitives.Tests
{
    public class IntegerTests
    {
        const int NumberOfRandomSamples = 1000;

        static readonly TextEncoder[] Encoders = new TextEncoder[]
        {
            TextEncoder.Utf8,
            TextEncoder.Utf16,
        };

        static readonly TextFormat[] Formats = new TextFormat[]
        {
            new TextFormat('N', 0),
            new TextFormat('N', 1),
            new TextFormat('N', 10),
            new TextFormat('N', 30),
            new TextFormat('N', 255),
            new TextFormat('D', 0),
            new TextFormat('D', 1),
            new TextFormat('D', 10),
            new TextFormat('D', 30),
            new TextFormat('D', 255),
            new TextFormat('x', 0),
            new TextFormat('x', 1),
            new TextFormat('x', 10),
            new TextFormat('x', 30),
            new TextFormat('x', 255),
            new TextFormat('X', 0),
            new TextFormat('X', 1),
            new TextFormat('X', 10),
            new TextFormat('X', 30),
            new TextFormat('X', 255),
        };

        [Fact]
        public void SpecificIntegerTests()
        {
            foreach (var encoder in Encoders)
            {
                foreach (var format in Formats)
                {
                    Validate<ulong>(0, format, encoder);
                    Validate<ulong>(1, format, encoder);
                    Validate<ulong>(999999999999, format, encoder);
                    Validate<ulong>(1000000000000, format, encoder);
                    Validate<ulong>(ulong.MaxValue, format, encoder);

                    Validate<uint>(0, format, encoder);
                    Validate<uint>(1, format, encoder);
                    Validate<uint>(999999999, format, encoder);
                    Validate<uint>(1000000000, format, encoder);
                    Validate<uint>(uint.MaxValue, format, encoder);

                    Validate<ushort>(0, format, encoder);
                    Validate<ushort>(1, format, encoder);
                    Validate<ushort>(9999, format, encoder);
                    Validate<ushort>(10000, format, encoder);
                    Validate<ushort>(ushort.MaxValue, format, encoder);

                    Validate<byte>(0, format, encoder);
                    Validate<byte>(1, format, encoder);
                    Validate<byte>(99, format, encoder);
                    Validate<byte>(100, format, encoder);
                    Validate<byte>(byte.MaxValue, format, encoder);

                    Validate<long>(long.MinValue, format, encoder);
                    Validate<long>(-1000000000000, format, encoder);
                    Validate<long>(-999999999999, format, encoder);
                    Validate<long>(-1, format, encoder);
                    Validate<long>(0, format, encoder);
                    Validate<long>(1, format, encoder);
                    Validate<long>(999999999999, format, encoder);
                    Validate<long>(1000000000000, format, encoder);
                    Validate<long>(long.MaxValue, format, encoder);

                    Validate<int>(int.MinValue, format, encoder);
                    Validate<int>(-1000000000, format, encoder);
                    Validate<int>(-999999999, format, encoder);
                    Validate<int>(-1, format, encoder);
                    Validate<int>(0, format, encoder);
                    Validate<int>(1, format, encoder);
                    Validate<int>(999999999, format, encoder);
                    Validate<int>(1000000000, format, encoder);
                    Validate<int>(int.MaxValue, format, encoder);

                    Validate<short>(short.MinValue, format, encoder);
                    Validate<short>(-10000, format, encoder);
                    Validate<short>(-9999, format, encoder);
                    Validate<short>(-1, format, encoder);
                    Validate<short>(0, format, encoder);
                    Validate<short>(1, format, encoder);
                    Validate<short>(9999, format, encoder);
                    Validate<short>(10000, format, encoder);
                    Validate<short>(short.MaxValue, format, encoder);

                    Validate<sbyte>(sbyte.MaxValue, format, encoder);
                    Validate<sbyte>(-100, format, encoder);
                    Validate<sbyte>(-99, format, encoder);
                    Validate<sbyte>(-1, format, encoder);
                    Validate<sbyte>(0, format, encoder);
                    Validate<sbyte>(1, format, encoder);
                    Validate<sbyte>(99, format, encoder);
                    Validate<sbyte>(100, format, encoder);
                    Validate<sbyte>(sbyte.MaxValue, format, encoder);
                }
            }
        }

        [Fact]
        public void RandomIntegerTests()
        {
            for (var i = 0; i < NumberOfRandomSamples; i++)
            {
                foreach (var encoder in Encoders)
                {
                    foreach (var format in Formats)
                    {
                        ValidateRandom<ulong>(format, encoder);
                        ValidateRandom<uint>(format, encoder);
                        ValidateRandom<ushort>(format, encoder);
                        ValidateRandom<byte>(format, encoder);
                        ValidateRandom<long>(format, encoder);
                        ValidateRandom<int>(format, encoder);
                        ValidateRandom<short>(format, encoder);
                        ValidateRandom<sbyte>(format, encoder);
                    }
                }
            }
        }

        static void ValidateRandom<T>(TextFormat format, TextEncoder encoder)
        {
            Validate<T>(GetRandom<T>(), format, encoder);
        }

        static void Validate<T>(long value, TextFormat format, TextEncoder encoder)
        {
            Validate<T>(unchecked((ulong)value), format, encoder);
        }

        static void Validate<T>(ulong value, TextFormat format, TextEncoder encoder)
        {
            var formatString = format.Precision == 255 ? $"{format.Symbol}" : $"{format.Symbol}{format.Precision}";

            var span = new Span<byte>(new byte[128]);
            string expected;
            int written;

            if (typeof(T) == typeof(ulong))
            {
                expected = value.ToString(formatString, CultureInfo.InvariantCulture);
                Assert.True(PrimitiveFormatter.TryFormat(value, span, out written, format, encoder));
            }
            else if (typeof(T) == typeof(uint))
            {
                expected = ((uint)value).ToString(formatString, CultureInfo.InvariantCulture);
                Assert.True(PrimitiveFormatter.TryFormat((uint)value, span, out written, format, encoder));
            }
            else if (typeof(T) == typeof(ushort))
            {
                expected = ((ushort)value).ToString(formatString, CultureInfo.InvariantCulture);
                Assert.True(PrimitiveFormatter.TryFormat((ushort)value, span, out written, format, encoder));
            }
            else if (typeof(T) == typeof(byte))
            {
                expected = ((byte)value).ToString(formatString, CultureInfo.InvariantCulture);
                Assert.True(PrimitiveFormatter.TryFormat((byte)value, span, out written, format, encoder));
            }
            else if (typeof(T) == typeof(long))
            {
                expected = ((long)value).ToString(formatString, CultureInfo.InvariantCulture);
                Assert.True(PrimitiveFormatter.TryFormat((long)value, span, out written, format, encoder));
            }
            else if (typeof(T) == typeof(int))
            {
                expected = ((int)value).ToString(formatString, CultureInfo.InvariantCulture);
                Assert.True(PrimitiveFormatter.TryFormat((int)value, span, out written, format, encoder));
            }
            else if (typeof(T) == typeof(short))
            {
                expected = ((short)value).ToString(formatString, CultureInfo.InvariantCulture);
                Assert.True(PrimitiveFormatter.TryFormat((short)value, span, out written, format, encoder));
            }
            else if (typeof(T) == typeof(sbyte))
            {
                expected = ((sbyte)value).ToString(formatString, CultureInfo.InvariantCulture);
                Assert.True(PrimitiveFormatter.TryFormat((sbyte)value, span, out written, format, encoder));
            }
            else
                throw new NotSupportedException();

            string actual = TestHelper.SpanToString(span.Slice(0, written), encoder);
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
