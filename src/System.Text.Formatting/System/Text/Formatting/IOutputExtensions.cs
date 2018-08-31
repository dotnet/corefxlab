// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Text;
using System.Text.Utf8;

namespace System.Text.Formatting
{
    public static class IOutputExtensions
    {
        public static void Append<TFormatter, T>(this TFormatter formatter, T value, SymbolTable symbolTable, StandardFormat format = default) where T : IBufferFormattable where TFormatter : IBufferWriter<byte>
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.GetMoreMemory();
            }
        }

        public static bool TryAppend<TFormatter, T>(this TFormatter formatter, T value, SymbolTable symbolTable, StandardFormat format = default) where T : IBufferFormattable where TFormatter : IBufferWriter<byte>
        {
            if (!value.TryFormat(formatter.GetSpan(), out int bytesWritten, format, symbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, string value, SymbolTable symbolTable) where TFormatter : IBufferWriter<byte>
        {
            formatter.Append(value.AsSpan(), symbolTable);
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, string value, SymbolTable symbolTable) where TFormatter : IBufferWriter<byte>
        {
            return formatter.TryAppend(value.AsSpan(), symbolTable);
        }

        public static void Append<TFormatter>(this TFormatter formatter, ReadOnlySpan<char> value, SymbolTable symbolTable) where TFormatter : IBufferWriter<byte>
        {
            if (value.Length <= 256)
            {
                while (!formatter.TryAppend(value, symbolTable)) {
                    formatter.GetMoreMemory();
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

        public static bool TryAppend<TFormatter>(this TFormatter formatter, ReadOnlySpan<char> value, SymbolTable symbolTable) where TFormatter : IBufferWriter<byte>
        {
            var result = symbolTable.TryEncode(value, formatter.GetSpan(), out int consumed, out int written);
            if (result)
                formatter.Advance(written);

            return result;
        }

        public static void Append<TFormatter>(this TFormatter formatter, char value, SymbolTable symbolTable) where TFormatter : IBufferWriter<byte>
        {
            while (!formatter.TryAppend(value, symbolTable)) {
                formatter.GetMoreMemory();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, char value, SymbolTable symbolTable) where TFormatter : IBufferWriter<byte>
        {
            unsafe
            {
                ReadOnlySpan<char> input = new ReadOnlySpan<char>(&value, 1);
                return formatter.TryAppend(input, symbolTable);
            }
        }

        public static void Append<TFormatter>(this TFormatter formatter, Utf8Span value, SymbolTable encoder) where TFormatter : IBufferWriter<byte>
        {
            while (!formatter.TryAppend(value, encoder)) {
                formatter.GetMoreMemory();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, Utf8Span value, SymbolTable symbolTable) where TFormatter : IBufferWriter<byte>
        {
            if (!symbolTable.TryEncode(value, formatter.GetSpan(), out int consumed, out int bytesWritten))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, uint value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IBufferWriter<byte>
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.GetMoreMemory();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, uint value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IBufferWriter<byte>
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, symbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, ulong value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IBufferWriter<byte>
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.GetMoreMemory();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, ulong value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IBufferWriter<byte>
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, symbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, int value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IBufferWriter<byte>
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.GetMoreMemory();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, int value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IBufferWriter<byte>
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, symbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, long value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IBufferWriter<byte>
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.GetMoreMemory();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, long value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IBufferWriter<byte>
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, symbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, byte value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IBufferWriter<byte>
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.GetMoreMemory();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, byte value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IBufferWriter<byte>
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, symbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, sbyte value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IBufferWriter<byte>
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.GetMemory();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, sbyte value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IBufferWriter<byte>
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, symbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, ushort value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IBufferWriter<byte>
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.GetMoreMemory();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, ushort value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IBufferWriter<byte>
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, symbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, short value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IBufferWriter<byte>
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.GetMoreMemory();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, short value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IBufferWriter<byte>
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, symbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, Guid value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IBufferWriter<byte>
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.GetMoreMemory();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, Guid value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IBufferWriter<byte>
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, symbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, DateTime value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IBufferWriter<byte>
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.GetMoreMemory();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, DateTime value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IBufferWriter<byte>
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, symbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, DateTimeOffset value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IBufferWriter<byte>
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.GetMoreMemory();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, DateTimeOffset value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IBufferWriter<byte>
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, symbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, TimeSpan value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IBufferWriter<byte>
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.GetMoreMemory();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, TimeSpan value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IBufferWriter<byte>
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, symbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, float value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IBufferWriter<byte>
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.GetMoreMemory();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, float value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IBufferWriter<byte>
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, symbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, double value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IBufferWriter<byte>
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.GetMoreMemory();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, double value, SymbolTable symbolTable, StandardFormat format = default) where TFormatter : IBufferWriter<byte>
        {
            if (!CustomFormatter.TryFormat(value, formatter.GetSpan(), out int bytesWritten, format, symbolTable))
            {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        private static void GetMoreMemory<TFormatter>(this TFormatter formatter) where TFormatter : IBufferWriter<byte>
        {
            formatter.GetSpan(formatter.GetSpan().Length * 2);
        }
    }
}
