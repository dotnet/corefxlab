// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Buffers.Text;
using System.Text.Utf8;

namespace System.Text.Formatting
{
    public static class ITextOutputExtensions
    {
        public static void Append<TFormatter, T>(this TFormatter formatter, T value, StandardFormat format = default) where T : IBufferFormattable where TFormatter : ITextOutput
        {
            while(!formatter.TryAppend(value, format)) {
                formatter.GetMoreMemory();
            }
        }

        public static bool TryAppend<TFormatter, T>(this TFormatter formatter, T value, StandardFormat format = default) where T : IBufferFormattable where TFormatter : ITextOutput
        {
            if (!value.TryFormat(formatter.GetSpan(), out int bytesWritten, format, formatter.SymbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, byte value, StandardFormat format = default) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.GetMoreMemory();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, byte value, StandardFormat format = default) where TFormatter : ITextOutput
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, formatter.SymbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, sbyte value, StandardFormat format = default) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.GetMoreMemory();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, sbyte value, StandardFormat format = default) where TFormatter : ITextOutput
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, formatter.SymbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, ushort value, StandardFormat format = default) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.GetMoreMemory();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, ushort value, StandardFormat format = default) where TFormatter : ITextOutput
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, formatter.SymbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, short value, StandardFormat format = default) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.GetMoreMemory();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, short value, StandardFormat format = default) where TFormatter : ITextOutput
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, formatter.SymbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, uint value, StandardFormat format = default) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.GetMoreMemory();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, uint value, StandardFormat format = default) where TFormatter : ITextOutput
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, formatter.SymbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, int value, StandardFormat format = default) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.GetMoreMemory();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, int value, StandardFormat format = default) where TFormatter : ITextOutput
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, formatter.SymbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, ulong value, StandardFormat format = default) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.GetMoreMemory();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, ulong value, StandardFormat format = default) where TFormatter : ITextOutput
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, formatter.SymbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, long value, StandardFormat format = default) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.GetMoreMemory();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, long value, StandardFormat format = default) where TFormatter : ITextOutput
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, formatter.SymbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, char value) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value)) {
                formatter.GetMoreMemory();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, char value) where TFormatter : ITextOutput
        {
            return formatter.TryAppend(value, formatter.SymbolTable);
        }

        public static void Append<TFormatter>(this TFormatter formatter, ReadOnlySpan<char> value) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value)) {
                formatter.GetMoreMemory();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, ReadOnlySpan<char> value) where TFormatter : ITextOutput
        {
            return formatter.TryAppend(value, formatter.SymbolTable);
        }

        public static void Append<TFormatter>(this TFormatter formatter, string value) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value)) {
                formatter.GetMoreMemory();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, string value) where TFormatter : ITextOutput
        {
            return formatter.TryAppend(value, formatter.SymbolTable);
        }

        public static void Append<TFormatter>(this TFormatter formatter, Utf8Span value) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value)) {
                formatter.GetMoreMemory();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, Utf8Span value) where TFormatter : ITextOutput
        {
            if (!formatter.SymbolTable.TryEncode(value, formatter.GetSpan(), out int consumed, out int bytesWritten))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, Guid value, StandardFormat format = default) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.GetMoreMemory();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, Guid value, StandardFormat format = default) where TFormatter : ITextOutput
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, formatter.SymbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, DateTime value, StandardFormat format = default) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.GetMoreMemory();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, DateTime value, StandardFormat format = default) where TFormatter : ITextOutput
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, formatter.SymbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, DateTimeOffset value, StandardFormat format = default) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.GetMoreMemory();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, DateTimeOffset value, StandardFormat format = default) where TFormatter : ITextOutput
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, formatter.SymbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, TimeSpan value, StandardFormat format = default) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.GetMoreMemory();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, TimeSpan value, StandardFormat format = default) where TFormatter : ITextOutput
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, formatter.SymbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, float value, StandardFormat format = default) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.GetMoreMemory();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, float value, StandardFormat format = default) where TFormatter : ITextOutput
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, formatter.SymbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, double value, StandardFormat format = default) where TFormatter : ITextOutput
        {
            while (!formatter.TryAppend(value, format)) {
                formatter.GetMoreMemory();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, double value, StandardFormat format = default) where TFormatter : ITextOutput
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, formatter.SymbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        private static void GetMoreMemory<TFormatter>(this TFormatter formatter) where TFormatter : ITextOutput
        {
            formatter.GetMemory(formatter.GetMemory().Length * 2);
        }
    }
}
