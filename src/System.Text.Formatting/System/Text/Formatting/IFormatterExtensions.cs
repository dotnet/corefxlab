// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;
using System.Text.Utf8;

namespace System.Text.Formatting
{
    public static class IFormatterExtensions
    {
        public static void Append<TFormatter, T>(this TFormatter formatter, T value, Format.Parsed format = default(Format.Parsed)) where T : IBufferFormattable where TFormatter : IFormatter
        {
            int bytesWritten;
            while (!value.TryFormat(formatter.FreeBuffer, format, formatter.FormattingData, out bytesWritten))
            {
                formatter.ResizeBuffer();
                bytesWritten = 0;
            }
            formatter.CommitBytes(bytesWritten);
        }

        public static void Append<TFormatter>(this TFormatter formatter, byte value, Format.Parsed format = default(Format.Parsed)) where TFormatter : IFormatter
        {
            int bytesWritten;
            while (!value.TryFormat(formatter.FreeBuffer, format, formatter.FormattingData, out bytesWritten))
            {
                formatter.ResizeBuffer();
                bytesWritten = 0;
            }
            formatter.CommitBytes(bytesWritten);
        }

        public static void Append<TFormatter>(this TFormatter formatter, sbyte value, Format.Parsed format = default(Format.Parsed)) where TFormatter : IFormatter
        {
            int bytesWritten;
            while (!value.TryFormat(formatter.FreeBuffer, format, formatter.FormattingData, out bytesWritten))
            {
                formatter.ResizeBuffer();
                bytesWritten = 0;
            }
            formatter.CommitBytes(bytesWritten);
        }

        public static void Append<TFormatter>(this TFormatter formatter, ushort value, Format.Parsed format = default(Format.Parsed)) where TFormatter : IFormatter
        {
            int bytesWritten;
            while (!value.TryFormat(formatter.FreeBuffer, format, formatter.FormattingData, out bytesWritten))
            {
                formatter.ResizeBuffer();
                bytesWritten = 0;
            }
            formatter.CommitBytes(bytesWritten);
        }
        public static void Append<TFormatter>(this TFormatter formatter, short value, Format.Parsed format = default(Format.Parsed)) where TFormatter : IFormatter
        {
            int bytesWritten;
            while (!value.TryFormat(formatter.FreeBuffer, format, formatter.FormattingData, out bytesWritten))
            {
                formatter.ResizeBuffer();
                bytesWritten = 0;
            }
            formatter.CommitBytes(bytesWritten);
        }

        public static void Append<TFormatter>(this TFormatter formatter, uint value, Format.Parsed format = default(Format.Parsed)) where TFormatter : IFormatter
        {
            int bytesWritten;
            while (!value.TryFormat(formatter.FreeBuffer, format, formatter.FormattingData, out bytesWritten))
            {
                formatter.ResizeBuffer();
                bytesWritten = 0;
            }
            formatter.CommitBytes(bytesWritten);
        }
        public static void Append<TFormatter>(this TFormatter formatter, int value, Format.Parsed format = default(Format.Parsed)) where TFormatter : IFormatter
        {
            int bytesWritten;
            while (!value.TryFormat(formatter.FreeBuffer, format, formatter.FormattingData, out bytesWritten))
            {
                formatter.ResizeBuffer();
                bytesWritten = 0;
            }
            formatter.CommitBytes(bytesWritten);
        }

        public static void Append<TFormatter>(this TFormatter formatter, ulong value, Format.Parsed format = default(Format.Parsed)) where TFormatter : IFormatter
        {
            int bytesWritten;
            while (!value.TryFormat(formatter.FreeBuffer, format, formatter.FormattingData, out bytesWritten))
            {
                formatter.ResizeBuffer();
                bytesWritten = 0;
            }
            formatter.CommitBytes(bytesWritten);
        }
        public static void Append<TFormatter>(this TFormatter formatter, long value, Format.Parsed format = default(Format.Parsed)) where TFormatter : IFormatter
        {
            int bytesWritten;
            while (!value.TryFormat(formatter.FreeBuffer, format, formatter.FormattingData, out bytesWritten))
            {
                formatter.ResizeBuffer();
                bytesWritten = 0;
            }
            formatter.CommitBytes(bytesWritten);
        }

        public static void Append<TFormatter>(this TFormatter formatter, char value, Format.Parsed format = default(Format.Parsed)) where TFormatter : IFormatter
        {
            int bytesWritten;
            while (!value.TryFormat(formatter.FreeBuffer, format, formatter.FormattingData, out bytesWritten))
            {
                formatter.ResizeBuffer();
                bytesWritten = 0;
            }
            formatter.CommitBytes(bytesWritten);
        }

        public static void Append<TFormatter>(this TFormatter formatter, string value, Format.Parsed format = default(Format.Parsed)) where TFormatter : IFormatter
        {
            int bytesWritten;
            while (!value.TryFormat(formatter.FreeBuffer, format, formatter.FormattingData, out bytesWritten))
            {
                formatter.ResizeBuffer();
                bytesWritten = 0;
            }
            formatter.CommitBytes(bytesWritten);
        }

        public static void Append<TFormatter>(this TFormatter formatter, Utf8String value, Format.Parsed format = default(Format.Parsed)) where TFormatter : IFormatter
        {
            int bytesWritten;
            while (!value.TryFormat(formatter.FreeBuffer, format, formatter.FormattingData, out bytesWritten)) {
                formatter.ResizeBuffer();
                bytesWritten = 0;
            }
            formatter.CommitBytes(bytesWritten);
        }

        public static void Append<TFormatter>(this TFormatter formatter, Guid value, Format.Parsed format = default(Format.Parsed)) where TFormatter : IFormatter
        {
            int bytesWritten;
            while (!value.TryFormat(formatter.FreeBuffer, format, formatter.FormattingData, out bytesWritten))
            {
                formatter.ResizeBuffer();
                bytesWritten = 0;
            }
            formatter.CommitBytes(bytesWritten);
        }

        public static void Append<TFormatter>(this TFormatter formatter, DateTime value, Format.Parsed format = default(Format.Parsed)) where TFormatter : IFormatter
        {
            int bytesWritten;
            while (!value.TryFormat(formatter.FreeBuffer, format, formatter.FormattingData, out bytesWritten))
            {
                formatter.ResizeBuffer();
                bytesWritten = 0;
            }
            formatter.CommitBytes(bytesWritten);
        }

        public static void Append<TFormatter>(this TFormatter formatter, DateTimeOffset value, Format.Parsed format = default(Format.Parsed)) where TFormatter : IFormatter
        {
            int bytesWritten;
            while (!value.TryFormat(formatter.FreeBuffer, format, formatter.FormattingData, out bytesWritten))
            {
                formatter.ResizeBuffer();
                bytesWritten = 0;
            }
            formatter.CommitBytes(bytesWritten);
        }

        public static void Append<TFormatter>(this TFormatter formatter, TimeSpan value, Format.Parsed format = default(Format.Parsed)) where TFormatter : IFormatter
        {
            int bytesWritten;
            while (!value.TryFormat(formatter.FreeBuffer, format, formatter.FormattingData, out bytesWritten))
            {
                formatter.ResizeBuffer();
                bytesWritten = 0;
            }
            formatter.CommitBytes(bytesWritten);
        }

        public static void Append<TFormatter>(this TFormatter formatter, float value, Format.Parsed format = default(Format.Parsed)) where TFormatter : IFormatter
        {
            int bytesWritten;
            while (!value.TryFormat(formatter.FreeBuffer, format, formatter.FormattingData, out bytesWritten))
            {
                formatter.ResizeBuffer();
                bytesWritten = 0;
            }
            formatter.CommitBytes(bytesWritten);
        }

        public static void Append<TFormatter>(this TFormatter formatter, double value, Format.Parsed format = default(Format.Parsed)) where TFormatter : IFormatter
        {
            int bytesWritten;
            while (!value.TryFormat(formatter.FreeBuffer, format, formatter.FormattingData, out bytesWritten))
            {
                formatter.ResizeBuffer();
                bytesWritten = 0;
            }
            formatter.CommitBytes(bytesWritten);
        }
    }
}
