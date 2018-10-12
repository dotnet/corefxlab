// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Text;
using System.Numerics;

namespace System.Text.JsonLab
{
    public static class SpanExtensions
    {
        public static string ConvertToString(this ReadOnlySpan<byte> span)
        {
            return Encodings.Utf8.ToString(span);
        }

        public static object ConvertToNumber(this ReadOnlySpan<byte> span)
        {
            if (Utf8Parser.TryParse(span, out int intVal, out int bytesConsumed))
            {
                if (span.Length == bytesConsumed)
                {
                    return intVal;
                }
            }
            if (Utf8Parser.TryParse(span, out long longVal, out bytesConsumed))
            {
                if (span.Length == bytesConsumed)
                {
                    return longVal;
                }
            }

            if (span.IndexOfAny((byte)'e', (byte)'E') == -1)
            {
                if (Utf8Parser.TryParse(span, out decimal value, out bytesConsumed))
                {
                    if (span.Length == bytesConsumed)
                    {
                        return span.ConvertToDecimal();
                    }
                }
            }
            else
            {
                if (Utf8Parser.TryParse(span, out decimal value, out bytesConsumed, 'e'))
                {
                    if (span.Length == bytesConsumed)
                    {
                        return span.ConvertToDecimal();
                    }
                }
            }

            // Number too large, try BigInteger
            /*if (Utf8Parser.TryParse(span, out BigInteger bigIntegerVal, out bytesConsumed))
            {
                if (span.Length == bytesConsumed)
                {
                    return (NumberType.BigInteger, bigIntegerVal);
                }
            }*/

            // Number too large for .NET
            JsonThrowHelper.ThrowInvalidCastException();
            return default;
        }

        public static int ConvertToInt32(this ReadOnlySpan<byte> span)
        {
            if (Utf8Parser.TryParse(span, out int value, out int bytesConsumed))
            {
                if (span.Length == bytesConsumed)
                {
                    return value;
                }
            }
            JsonThrowHelper.ThrowInvalidCastException();
            return default;
        }

        public static long ConvertToInt64(this ReadOnlySpan<byte> span)
        {
            if (Utf8Parser.TryParse(span, out long value, out int bytesConsumed))
            {
                if (span.Length == bytesConsumed)
                {
                    return value;
                }
            }
            JsonThrowHelper.ThrowInvalidCastException();
            return default;
        }

        public static float ConvertToFloat(this ReadOnlySpan<byte> span)
        {
            if (span.IndexOfAny((byte)'e', (byte)'E') == -1)
            {
                if (Utf8Parser.TryParse(span, out float value, out int bytesConsumed))
                {
                    if (span.Length == bytesConsumed)
                    {
                        return value;
                    }
                }
            }
            else
            {
                if (Utf8Parser.TryParse(span, out float value, out int bytesConsumed, 'e'))
                {
                    if (span.Length == bytesConsumed)
                    {
                        return value;
                    }
                }
            }
            JsonThrowHelper.ThrowInvalidCastException();
            return default;
        }

        public static double ConvertToDouble(this ReadOnlySpan<byte> span)
        {
            if (span.IndexOfAny((byte)'e', (byte)'E') == -1)
            {
                if (Utf8Parser.TryParse(span, out double value, out int bytesConsumed))
                {
                    if (span.Length == bytesConsumed)
                    {
                        return value;
                    }
                }
            }
            else
            {
                if (Utf8Parser.TryParse(span, out double value, out int bytesConsumed, 'e'))
                {
                    if (span.Length == bytesConsumed)
                    {
                        return value;
                    }
                }
            }
            JsonThrowHelper.ThrowInvalidCastException();
            return default;
        }

        public static decimal ConvertToDecimal(this ReadOnlySpan<byte> span)
        {
            if (span.IndexOfAny((byte)'e', (byte)'E') == -1)
            {
                if (Utf8Parser.TryParse(span, out decimal value, out int bytesConsumed))
                {
                    if (span.Length == bytesConsumed)
                    {
                        return value;
                    }
                }
            }
            else
            {
                if (Utf8Parser.TryParse(span, out decimal value, out int bytesConsumed, 'e'))
                {
                    if (span.Length == bytesConsumed)
                    {
                        return value;
                    }
                }
            }
            JsonThrowHelper.ThrowInvalidCastException();
            return default;
        }

        public static BigInteger ConvertToBigInteger(this ReadOnlySpan<byte> span)
        {
            //if (span.IndexOfAny((byte)'e', (byte)'E') == -1)
            //{
            //    if (Utf8Parser.TryParse(span, out BigInteger value, out int bytesConsumed))
            //    {
            //        if (span.Length == bytesConsumed)
            //        {
            //            return value;
            //        }
            //    }
            //}
            //else
            //{
            //    if (Utf8Parser.TryParse(span, out BigInteger value, out int bytesConsumed, 'e'))
            //    {
            //        if (span.Length == bytesConsumed)
            //        {
            //            return value;
            //        }
            //    }
            //}
            //JsonThrowHelper.ThrowInvalidCastException();
            //return default;

            throw new NotImplementedException();
        }

        public static bool ConvertToBool(this ReadOnlySpan<byte> span)
        {
            if (Utf8Parser.TryParse(span, out bool value, out int bytesConsumed))
            {
                if (span.Length == bytesConsumed)
                {
                    return value;
                }
            }
            JsonThrowHelper.ThrowInvalidCastException();
            return default;
        }
    }
}
