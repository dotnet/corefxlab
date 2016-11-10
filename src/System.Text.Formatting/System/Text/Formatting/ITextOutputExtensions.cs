// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Text.Utf8;

namespace System.Text.Formatting
{
    public static class IOutputExtensions
    {
        public static void AppendUtf8<TFormatter, T>(this TFormatter formatter, T value, Format.Parsed format = default(Format.Parsed)) where T : IBufferFormattable where TFormatter : IOutput
        {
            while (!formatter.TryAppendUtf8(value, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppendUtf8<TFormatter, T>(this TFormatter formatter, T value, Format.Parsed format = default(Format.Parsed)) where T : IBufferFormattable where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, format, EncodingData.InvariantUtf8, out bytesWritten)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void AppendUtf8<TFormatter>(this TFormatter formatter, string value) where TFormatter : IOutput
        {
            if (value.Length < 256) {
                while (!formatter.TryAppendUtf8(value)) {
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
                    formatter.AppendUtf8(chunk);
                    leftToWrite = leftToWrite.Slice(nextChunkLength);
                }
            }
        }

        public static bool TryAppendUtf8<TFormatter>(this TFormatter formatter, string value) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, EncodingData.InvariantUtf8, out bytesWritten)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void AppendUtf8<TFormatter>(this TFormatter formatter, ReadOnlySpan<char> value) where TFormatter : IOutput
        {
            while (!formatter.TryAppendUtf8(value)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppendUtf8<TFormatter>(this TFormatter formatter, ReadOnlySpan<char> value) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!PrimitiveFormatter.TryFormat(value, formatter.Buffer, EncodingData.InvariantUtf8, out bytesWritten)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }
    }

    public static class ITextOutputExtensions
    {
        public static void Append<TFormatter, T>(this TFormatter formatter, T value, Format.Parsed format = default(Format.Parsed)) where T : IBufferFormattable where TFormatter : ITextOutput
        {
            while(!formatter.TryAppend(value, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter, T>(this TFormatter formatter, T value, Format.Parsed format = default(Format.Parsed)) where T : IBufferFormattable where TFormatter : ITextOutput
        {
            int bytesWritten;
            if(!value.TryFormat(formatter.Buffer, format, formatter.Encoding, out bytesWritten)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, byte value, Format.Parsed format = default(Format.Parsed)) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, byte value, Format.Parsed format = default(Format.Parsed)) where TFormatter : ITextOutput
        {
            int bytesWritten;
            if(!value.TryFormat(formatter.Buffer, format, formatter.Encoding, out bytesWritten)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, sbyte value, Format.Parsed format = default(Format.Parsed)) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, sbyte value, Format.Parsed format = default(Format.Parsed)) where TFormatter : ITextOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, format, formatter.Encoding, out bytesWritten)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, ushort value, Format.Parsed format = default(Format.Parsed)) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, ushort value, Format.Parsed format = default(Format.Parsed)) where TFormatter : ITextOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, format, formatter.Encoding, out bytesWritten)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, short value, Format.Parsed format = default(Format.Parsed)) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, short value, Format.Parsed format = default(Format.Parsed)) where TFormatter : ITextOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, format, formatter.Encoding, out bytesWritten)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, uint value, Format.Parsed format = default(Format.Parsed)) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, uint value, Format.Parsed format = default(Format.Parsed)) where TFormatter : ITextOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, format, formatter.Encoding, out bytesWritten)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, int value, Format.Parsed format = default(Format.Parsed)) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, int value, Format.Parsed format = default(Format.Parsed)) where TFormatter : ITextOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, format, formatter.Encoding, out bytesWritten)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, ulong value, Format.Parsed format = default(Format.Parsed)) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, ulong value, Format.Parsed format = default(Format.Parsed)) where TFormatter : ITextOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, format, formatter.Encoding, out bytesWritten)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, long value, Format.Parsed format = default(Format.Parsed)) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, long value, Format.Parsed format = default(Format.Parsed)) where TFormatter : ITextOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, format, formatter.Encoding, out bytesWritten)) {
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
            if (!value.TryFormat(formatter.Buffer, formatter.Encoding, out bytesWritten)) {
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
            if (!PrimitiveFormatter.TryFormat(value, formatter.Buffer, formatter.Encoding, out bytesWritten)) {
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
            if (!value.TryFormat(formatter.Buffer, formatter.Encoding, out bytesWritten)) {
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
            if (!value.TryFormat(formatter.Buffer, formatter.Encoding, out bytesWritten)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, Guid value, Format.Parsed format = default(Format.Parsed)) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, Guid value, Format.Parsed format = default(Format.Parsed)) where TFormatter : ITextOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, format, formatter.Encoding, out bytesWritten)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, DateTime value, Format.Parsed format = default(Format.Parsed)) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, DateTime value, Format.Parsed format = default(Format.Parsed)) where TFormatter : ITextOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, format, formatter.Encoding, out bytesWritten)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, DateTimeOffset value, Format.Parsed format = default(Format.Parsed)) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, DateTimeOffset value, Format.Parsed format = default(Format.Parsed)) where TFormatter : ITextOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, format, formatter.Encoding, out bytesWritten)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, TimeSpan value, Format.Parsed format = default(Format.Parsed)) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, TimeSpan value, Format.Parsed format = default(Format.Parsed)) where TFormatter : ITextOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, format, formatter.Encoding, out bytesWritten)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, float value, Format.Parsed format = default(Format.Parsed)) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, float value, Format.Parsed format = default(Format.Parsed)) where TFormatter : ITextOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, format, formatter.Encoding, out bytesWritten)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, double value, Format.Parsed format = default(Format.Parsed)) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, double value, Format.Parsed format = default(Format.Parsed)) where TFormatter : ITextOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, format, formatter.Encoding, out bytesWritten)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }
    }
}
