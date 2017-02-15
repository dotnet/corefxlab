// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Text.Utf8;

namespace System.Text.Formatting
{
    public static class IOutputExtensions
    {
        public static void Append<TFormatter, T>(this TFormatter formatter, T value, TextEncoder encoding, TextFormat format = default(TextFormat)) where T : IBufferFormattable where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, encoding, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter, T>(this TFormatter formatter, T value, TextEncoder encoding, TextFormat format = default(TextFormat)) where T : IBufferFormattable where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, encoding)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, string value, TextEncoder encoding) where TFormatter : IOutput
        {
            if (value.Length < 256) {
                while (!formatter.TryAppend(value, encoding)) {
                    formatter.Enlarge();
                }
            }
            else { // slice the string and write piece by piece, otherwise enlarge might fail
                var leftToWrite = value.Slice();
                while (leftToWrite.Length > 0) {
                    var nextChunkLength = leftToWrite.Length < 256 ? leftToWrite.Length : 256;
                    if (char.IsHighSurrogate(leftToWrite[nextChunkLength - 1])) {
                        nextChunkLength--;
                        if (nextChunkLength == 0) throw new Exception("value ends in high surrogate");
                    }
                    var chunk = leftToWrite.Slice(0, nextChunkLength);
                    formatter.Append(chunk, encoding);
                    leftToWrite = leftToWrite.Slice(nextChunkLength);
                }
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, string value, TextEncoder encoding) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!encoding.TryEncode(value, formatter.Buffer, out bytesWritten)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, ReadOnlySpan<char> value, TextEncoder encoding) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, encoding)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, ReadOnlySpan<char> value, TextEncoder encoding) where TFormatter : IOutput
        {
            int bytesWritten;
            int consumed;
            if (!encoding.TryEncode(value, formatter.Buffer, out consumed, out bytesWritten)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, char value, TextEncoder encoding) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, encoding)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, char value, TextEncoder encoding) where TFormatter : IOutput
        {
            int consumed;
            int bytesWritten;

            unsafe
            {
                ReadOnlySpan<char> charSpan = new ReadOnlySpan<char>(&value, 1);

                if (!encoding.TryEncode(charSpan, formatter.Buffer, out consumed, out bytesWritten))
                {
                    return false;
                }
            }

            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, Utf8String value, TextEncoder encoding) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, encoding)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, Utf8String value, TextEncoder encoding) where TFormatter : IOutput
        {
            int bytesWritten;
            int consumed;
            if (!encoding.TryEncode(value, formatter.Buffer, out consumed, out bytesWritten)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, uint value, TextEncoder encoding, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, encoding, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, uint value, TextEncoder encoding, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, encoding)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, ulong value, TextEncoder encoding, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, encoding, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, ulong value, TextEncoder encoding, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, encoding)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, int value, TextEncoder encoding, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, encoding, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, int value, TextEncoder encoding, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, encoding)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, long value, TextEncoder encoding, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, encoding, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, long value, TextEncoder encoding, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, encoding)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, byte value, TextEncoder encoding, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, encoding, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, byte value, TextEncoder encoding, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, encoding)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, sbyte value, TextEncoder encoding, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, encoding, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, sbyte value, TextEncoder encoding, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, encoding)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, ushort value, TextEncoder encoding, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, encoding, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, ushort value, TextEncoder encoding, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, encoding)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, short value, TextEncoder encoding, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, encoding, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, short value, TextEncoder encoding, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, encoding)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, Guid value, TextEncoder encoding, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, encoding, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, Guid value, TextEncoder encoding, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, encoding)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, DateTime value, TextEncoder encoding, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, encoding, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, DateTime value, TextEncoder encoding, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, encoding)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, DateTimeOffset value, TextEncoder encoding, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, encoding, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, DateTimeOffset value, TextEncoder encoding, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, encoding)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, TimeSpan value, TextEncoder encoding, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, encoding, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, TimeSpan value, TextEncoder encoding, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, encoding)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, float value, TextEncoder encoding, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, encoding, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, float value, TextEncoder encoding, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, encoding)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, double value, TextEncoder encoding, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, encoding, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, double value, TextEncoder encoding, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, encoding)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }
    }
}
