// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Text.Utf8;

namespace System.Text.Formatting
{
    public static class IOutputExtensions
    {
        public static void Append<TFormatter, T>(this TFormatter formatter, T value, SymbolTable symbolTable, TextFormat format = default(TextFormat)) where T : IBufferFormattable where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter, T>(this TFormatter formatter, T value, SymbolTable symbolTable, TextFormat format = default(TextFormat)) where T : IBufferFormattable where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, symbolTable)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, string value, SymbolTable symbolTable) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, symbolTable)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, string value, SymbolTable symbolTable) where TFormatter : IOutput
        {
            return formatter.TryAppend(value.AsSpan(), symbolTable);
        }

        public static void Append<TFormatter>(this TFormatter formatter, ReadOnlySpan<char> value, SymbolTable symbolTable) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, symbolTable)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, ReadOnlySpan<char> value, SymbolTable symbolTable) where TFormatter : IOutput
        {
            const int BufferSize = 256;

            ReadOnlySpan<byte> source = value.AsBytes();
            Span<byte> destination = formatter.Buffer;

            int sourceLength = source.Length;
            if (sourceLength <= 0)
                return true;

            Span<byte> temp;
            unsafe
            {
                byte* pTemp = stackalloc byte[BufferSize];
                temp = new Span<byte>(pTemp, BufferSize);
            }

            int bytesWritten = 0;
            int bytesConsumed = 0;
            while (sourceLength > bytesConsumed)
            {
                var status = Encoders.Utf8.ConvertFromUtf16(source, temp, out int consumed, out int written);
                if (status == TransformationStatus.InvalidData)
                    goto ExitFailed;

                source = source.Slice(consumed);
                bytesConsumed += consumed;

                if (!symbolTable.TryEncode(temp.Slice(0, written), destination, out consumed, out written))
                    goto ExitFailed;

                destination = destination.Slice(written);
                bytesWritten += written;
            }

            formatter.Advance(bytesWritten);
            return true;

        ExitFailed:
            return false;
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

        public static void Append<TFormatter>(this TFormatter formatter, Utf8String value, SymbolTable encoder) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, encoder)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, Utf8String value, SymbolTable symbolTable) where TFormatter : IOutput
        {
            int bytesWritten;
            int consumed;
            if (!symbolTable.TryEncode(value, formatter.Buffer, out consumed, out bytesWritten)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, uint value, SymbolTable symbolTable, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, uint value, SymbolTable symbolTable, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, symbolTable)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, ulong value, SymbolTable symbolTable, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, ulong value, SymbolTable symbolTable, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, symbolTable)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, int value, SymbolTable symbolTable, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, int value, SymbolTable symbolTable, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, symbolTable)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, long value, SymbolTable symbolTable, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, long value, SymbolTable symbolTable, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, symbolTable)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, byte value, SymbolTable symbolTable, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, byte value, SymbolTable symbolTable, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, symbolTable)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, sbyte value, SymbolTable symbolTable, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, sbyte value, SymbolTable symbolTable, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, symbolTable)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, ushort value, SymbolTable symbolTable, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, ushort value, SymbolTable symbolTable, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, symbolTable)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, short value, SymbolTable symbolTable, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, short value, SymbolTable symbolTable, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, symbolTable)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, Guid value, SymbolTable symbolTable, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, Guid value, SymbolTable symbolTable, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, symbolTable)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, DateTime value, SymbolTable symbolTable, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, DateTime value, SymbolTable symbolTable, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, symbolTable)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, DateTimeOffset value, SymbolTable symbolTable, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, DateTimeOffset value, SymbolTable symbolTable, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, symbolTable)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, TimeSpan value, SymbolTable symbolTable, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, TimeSpan value, SymbolTable symbolTable, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, symbolTable)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, float value, SymbolTable symbolTable, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, float value, SymbolTable symbolTable, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, symbolTable)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }

        public static void Append<TFormatter>(this TFormatter formatter, double value, SymbolTable symbolTable, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            while (!formatter.TryAppend(value, symbolTable, format)) {
                formatter.Enlarge();
            }
        }

        public static bool TryAppend<TFormatter>(this TFormatter formatter, double value, SymbolTable symbolTable, TextFormat format = default(TextFormat)) where TFormatter : IOutput
        {
            int bytesWritten;
            if (!value.TryFormat(formatter.Buffer, out bytesWritten, format, symbolTable)) {
                return false;
            }
            formatter.Advance(bytesWritten);
            return true;
        }
    }
}
