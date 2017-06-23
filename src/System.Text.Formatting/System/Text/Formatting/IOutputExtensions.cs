// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Text.Utf8;

namespace System.Text.Formatting
{
    public static class IOutputExtensions
    {
        public static void Append<TFormatter, T>(this TFormatter formatter, T value, TextEncoder encoder, TextFormat format = default) where T : IBufferFormattable where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, encoder, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter, T>(this TFormatter formatter, T value, TextEncoder encoder, TextFormat format = default) where T : IBufferFormattable where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, encoder)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, string value, TextEncoder encoder) where TFormatter : IOutput
        {
            if (value.Length < 256) {
                while (!formatter.TryAppend(value, encoder)) {
                    formatter.Enlarge();
                }
            }
            else { // slice the string and write piece by piece, otherwise enlarge might fail
                var leftToWrite = value.AsSpan();
                while (leftToWrite.Length > 0) {
                    var nextChunkLength = leftToWrite.Length < 256 ? leftToWrite.Length : 256;
                    if (char.IsHighSurrogate(leftToWrite[nextChunkLength - 1])) {
                        nextChunkLength--;
                        if (nextChunkLength == 0) throw new Exception("value ends in high surrogate");
                    }
                    var chunk = leftToWrite.Slice(0, nextChunkLength);
                    formatter.Append(chunk, encoder);
                    leftToWrite = leftToWrite.Slice(nextChunkLength);
                }
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, string value, TextEncoder encoder) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!encoder.TryEncode(value, formatter.Buffer, out bytesWritten)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, ReadOnlySpan<char> value, TextEncoder encoder) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, encoder)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, ReadOnlySpan<char> value, TextEncoder encoder) where TFormatter : IOutput
        {
            int bytesWritten;
            int consumed;
            if (!encoder.TryEncode(value, formatter.Buffer, out consumed, out bytesWritten)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, char value, TextEncoder encoder) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, encoder)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, char value, TextEncoder encoder) where TFormatter : IOutput
        {
            int consumed;
            int bytesWritten;

            unsafe
            {
                ReadOnlySpan<char> charSpan = new ReadOnlySpan<char>(&value, 1);

                if (!encoder.TryEncode(charSpan, formatter.Buffer, out consumed, out bytesWritten))
                {
                    return false;
                }
            }

            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, Utf8String value, TextEncoder encoder) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, encoder)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, Utf8String value, TextEncoder encoder) where TFormatter : IOutput
        {
            int bytesWritten;
            int consumed;
            if (!encoder.TryEncode(value, formatter.Buffer, out consumed, out bytesWritten)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, uint value, TextEncoder encoder, TextFormat format = default) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, encoder, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, uint value, TextEncoder encoder, TextFormat format = default) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, encoder)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, ulong value, TextEncoder encoder, TextFormat format = default) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, encoder, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, ulong value, TextEncoder encoder, TextFormat format = default) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, encoder)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, int value, TextEncoder encoder, TextFormat format = default) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, encoder, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, int value, TextEncoder encoder, TextFormat format = default) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, encoder)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, long value, TextEncoder encoder, TextFormat format = default) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, encoder, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, long value, TextEncoder encoder, TextFormat format = default) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, encoder)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, byte value, TextEncoder encoder, TextFormat format = default) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, encoder, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, byte value, TextEncoder encoder, TextFormat format = default) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, encoder)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, sbyte value, TextEncoder encoder, TextFormat format = default) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, encoder, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, sbyte value, TextEncoder encoder, TextFormat format = default) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, encoder)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, ushort value, TextEncoder encoder, TextFormat format = default) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, encoder, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, ushort value, TextEncoder encoder, TextFormat format = default) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, encoder)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, short value, TextEncoder encoder, TextFormat format = default) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, encoder, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, short value, TextEncoder encoder, TextFormat format = default) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, encoder)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, Guid value, TextEncoder encoder, TextFormat format = default) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, encoder, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, Guid value, TextEncoder encoder, TextFormat format = default) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, encoder)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, DateTime value, TextEncoder encoder, TextFormat format = default) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, encoder, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, DateTime value, TextEncoder encoder, TextFormat format = default) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, encoder)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, DateTimeOffset value, TextEncoder encoder, TextFormat format = default) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, encoder, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, DateTimeOffset value, TextEncoder encoder, TextFormat format = default) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, encoder)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, TimeSpan value, TextEncoder encoder, TextFormat format = default) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, encoder, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, TimeSpan value, TextEncoder encoder, TextFormat format = default) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, encoder)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, float value, TextEncoder encoder, TextFormat format = default) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, encoder, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, float value, TextEncoder encoder, TextFormat format = default) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, encoder)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, double value, TextEncoder encoder, TextFormat format = default) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, encoder, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, double value, TextEncoder encoder, TextFormat format = default) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, encoder)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }
    }
}
