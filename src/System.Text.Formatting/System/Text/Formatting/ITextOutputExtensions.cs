// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Text.Utf8;

namespace System.Text.Formatting
{
    public static class ITextOutputExtensions
    {
        public static void Append<TFormatter, T>(this TFormatter formatter, T value, TextFormat format = default(TextFormat)) where T : IBufferFormattable where TFormatter : ITextOutput
        {
            while(!formatter.TryAppend(value, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter, T>(this TFormatter formatter, T value, TextFormat format = default(TextFormat)) where T : IBufferFormattable where TFormatter : ITextOutput
        {
            int bytesWritten;
            if(!value.TryFormat(formatter.Buffer, out bytesWritten, format, formatter.Encoding)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, byte value, TextFormat format = default(TextFormat)) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, byte value, TextFormat format = default(TextFormat)) where TFormatter : ITextOutput
        {
            int bytesWritten;
            if(!value.TryFormat(formatter.Buffer, out bytesWritten, format, formatter.Encoding)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, sbyte value, TextFormat format = default(TextFormat)) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, sbyte value, TextFormat format = default(TextFormat)) where TFormatter : ITextOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, formatter.Encoding)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, ushort value, TextFormat format = default(TextFormat)) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, ushort value, TextFormat format = default(TextFormat)) where TFormatter : ITextOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, formatter.Encoding)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, short value, TextFormat format = default(TextFormat)) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, short value, TextFormat format = default(TextFormat)) where TFormatter : ITextOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, formatter.Encoding)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, uint value, TextFormat format = default(TextFormat)) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, uint value, TextFormat format = default(TextFormat)) where TFormatter : ITextOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, formatter.Encoding)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, int value, TextFormat format = default(TextFormat)) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, int value, TextFormat format = default(TextFormat)) where TFormatter : ITextOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, formatter.Encoding)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, ulong value, TextFormat format = default(TextFormat)) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, ulong value, TextFormat format = default(TextFormat)) where TFormatter : ITextOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, formatter.Encoding)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, long value, TextFormat format = default(TextFormat)) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, long value, TextFormat format = default(TextFormat)) where TFormatter : ITextOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, formatter.Encoding)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, char value) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, char value) where TFormatter : ITextOutput
        {
            int bytesWritten;
            if (!formatter.Encoding.TextEncoder.TryEncodeChar(value, formatter.Buffer, out bytesWritten)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, ReadOnlySpan<char> value) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, ReadOnlySpan<char> value) where TFormatter : ITextOutput
        {
            int bytesWritten;
            if (!formatter.Encoding.TextEncoder.TryEncodeFromUtf16(value, formatter.Buffer, out bytesWritten)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, string value) where TFormatter : ITextOutput
        {
            if (value.Length < 256) {
                while (!formatter.TryAppend(value)) {
                    formatter.Enlarge();
                }
            }
            else { // slice the string and write piece by piece, otherwise enlarge might fail
                var leftToWrite = value.Slice();
                while(leftToWrite.Length > 0) {
                    var nextChunkLength = leftToWrite.Length < 256 ? leftToWrite.Length : 256;
                    if (char.IsHighSurrogate(leftToWrite[nextChunkLength - 1])){
                        nextChunkLength--;
                        if (nextChunkLength == 0) throw new Exception("value ends in high surrogate");
                    }
                    var chunk = leftToWrite.Slice(0, nextChunkLength);
                    formatter.Append(chunk);
                    leftToWrite = leftToWrite.Slice(nextChunkLength);
                }
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, string value) where TFormatter : ITextOutput
        {
            int bytesWritten;
            if (!formatter.Encoding.TextEncoder.TryEncodeString(value, formatter.Buffer, out bytesWritten)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, Utf8String value) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, Utf8String value) where TFormatter : ITextOutput
        {
            int bytesWritten;
            if (!formatter.Encoding.TextEncoder.TryEncodeFromUtf8(value, formatter.Buffer, out bytesWritten)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, Guid value, TextFormat format = default(TextFormat)) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, Guid value, TextFormat format = default(TextFormat)) where TFormatter : ITextOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, formatter.Encoding)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, DateTime value, TextFormat format = default(TextFormat)) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, DateTime value, TextFormat format = default(TextFormat)) where TFormatter : ITextOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, formatter.Encoding)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, DateTimeOffset value, TextFormat format = default(TextFormat)) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, DateTimeOffset value, TextFormat format = default(TextFormat)) where TFormatter : ITextOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, formatter.Encoding)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, TimeSpan value, TextFormat format = default(TextFormat)) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, TimeSpan value, TextFormat format = default(TextFormat)) where TFormatter : ITextOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, formatter.Encoding)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, float value, TextFormat format = default(TextFormat)) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, float value, TextFormat format = default(TextFormat)) where TFormatter : ITextOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, formatter.Encoding)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, double value, TextFormat format = default(TextFormat)) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, double value, TextFormat format = default(TextFormat)) where TFormatter : ITextOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, formatter.Encoding)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }
    }
}
