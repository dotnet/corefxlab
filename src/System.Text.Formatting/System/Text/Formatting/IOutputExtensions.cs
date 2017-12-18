﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Buffers.Text;
using System.Text.Utf8;

namespace System.Text.Formatting
{
    public static class IOutputExtensions
    {
        public static void Append<TFormatter, T>(this TFormatter formatter, T value, SymbolTable symbolTable, StandardFormat format = default) where T : IBufferFormattable where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter, T>(this TFormatter formatter, T value, SymbolTable symbolTable, StandardFormat format = default) where T : IBufferFormattable where TFormatter : IOutput
        {
            if (!value.TryFormat(formatter.GetSpan(), out int bytesWritten, format, symbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, string value, SymbolTable symbolTable) where TFormatter : IOutput
        {
            formatter.Append(value.AsReadOnlySpan(), symbolTable);
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, string value, SymbolTable symbolTable) where TFormatter : IOutput
        {
            return formatter.TryAppend(value.AsReadOnlySpan(), symbolTable);
        }

        public static void Append<TFormatter>(this TFormatter formatter, ReadOnlySpan<char> value, SymbolTable symbolTable) where TFormatter : IOutput
        {
            if (value.Length <= 256)
            {
                while (!formatter.TryAppend(value, symbolTable)) {
                    formatter.Enlarge();
                }
            }
            else // slice the span into smaller pieces, otherwise the enlarge might fail.
            {
                var leftToWrite = value;
                while (leftToWrite.Length > 0)
                {
                    var chunkLength = leftToWrite.Length < 256 ? leftToWrite.Length : 256;
                    if (char.IsHighSurrogate(leftToWrite[chunkLength - 1]))
                    {
                        chunkLength--;
                        if (chunkLength == 0) throw new Exception("value ends in a high surrogate");
                    }

                    var chunk = leftToWrite.Slice(0, chunkLength);
                    formatter.Append(chunk, symbolTable);
                    leftToWrite = leftToWrite.Slice(chunkLength);
                }
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, ReadOnlySpan<char> value, SymbolTable symbolTable) where TFormatter : IOutput
        {
            var result = symbolTable.TryEncode(value, formatter.GetSpan(), out int consumed, out int written);
            if (result)
                formatter.Advance(written);

            return result;
        }

        public static void Append<TFormatter>(this TFormatter formatter, char value, SymbolTable symbolTable) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, symbolTable)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, char value, SymbolTable symbolTable) where TFormatter : IOutput
        {
            unsafe
            {
                ReadOnlySpan<char> input = new ReadOnlySpan<char>(&value, 1);
                return formatter.TryAppend(input, symbolTable);
            }
        }

        public static void Append<TFormatter>(this TFormatter formatter, Utf8Span value, SymbolTable encoder) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, encoder)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, Utf8Span value, SymbolTable symbolTable) where TFormatter : IOutput
        {
            if (!symbolTable.TryEncode(value, formatter.GetSpan(), out int consumed, out int bytesWritten))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, uint value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, uint value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IOutput
        {   
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, symbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, ulong value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, ulong value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IOutput
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, symbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, int value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, int value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IOutput
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, symbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, long value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, long value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IOutput
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, symbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, byte value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, byte value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IOutput
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, symbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, sbyte value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, sbyte value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IOutput
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, symbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, ushort value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, ushort value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IOutput
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, symbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, short value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, short value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IOutput
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, symbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, Guid value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, Guid value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IOutput
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, symbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, DateTime value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, DateTime value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IOutput
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, symbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, DateTimeOffset value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, DateTimeOffset value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IOutput
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, symbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, TimeSpan value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, TimeSpan value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IOutput
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, symbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, float value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, float value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IOutput
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, symbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, double value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, double value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IOutput
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, symbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }
    }
}
