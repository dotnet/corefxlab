// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Buffers.Writer;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Text.JsonLab
{
    public static class JsonWriter
    {
        internal static readonly byte[] s_newLineUtf8 = Encoding.UTF8.GetBytes(Environment.NewLine);
        internal static readonly int s_newLineUtf8Length = s_newLineUtf8.Length;
        internal static readonly char[] s_newLineUtf16 = Environment.NewLine.ToCharArray();
        internal static readonly int s_newLineUtf16Length = s_newLineUtf16.Length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static JsonWriterUtf8<TBufferWriter> CreateUtf8<TBufferWriter>(
            TBufferWriter bufferWriter,
            bool prettyPrint = false) where TBufferWriter : struct, IBufferWriter<byte>
        {
            return new JsonWriterUtf8<TBufferWriter>(BufferWriter.Create(bufferWriter), prettyPrint);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static JsonWriterUtf8<TBufferWriter> CreateUtf8<TBufferWriter>(
            BufferWriter<TBufferWriter> bufferWriter,
            bool prettyPrint = false) where TBufferWriter : struct, IBufferWriter<byte>
        {
            return new JsonWriterUtf8<TBufferWriter>(bufferWriter, prettyPrint);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static JsonWriterUtf8 CreateUtf8(
            IBufferWriter<byte> bufferWriter,
            bool prettyPrint = false)
        {
            return new JsonWriterUtf8(BufferWriter.Create(new IBufferWriter(bufferWriter)), prettyPrint);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static JsonWriterUtf16<TBufferWriter> CreateUtf16<TBufferWriter>(
            TBufferWriter bufferWriter,
            bool prettyPrint = false) where TBufferWriter : struct, IBufferWriter<byte>
        {
            return new JsonWriterUtf16<TBufferWriter>(BufferWriter.Create(bufferWriter), prettyPrint);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static JsonWriterUtf16<TBufferWriter> CreateUtf16<TBufferWriter>(
            BufferWriter<TBufferWriter> bufferWriter,
            bool prettyPrint = false) where TBufferWriter : struct, IBufferWriter<byte>
        {
            return new JsonWriterUtf16<TBufferWriter>(bufferWriter, prettyPrint);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static JsonWriterUtf16 CreateUtf16(
            IBufferWriter<byte> bufferWriter,
            bool prettyPrint = false)
        {
            return new JsonWriterUtf16(BufferWriter.Create(new IBufferWriter(bufferWriter)), prettyPrint);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int CountDigits(ulong value)
        {
            int digits = 1;
            uint part;
            if (value >= 10000000)
            {
                if (value >= 100000000000000)
                {
                    part = (uint)(value / 100000000000000);
                    digits += 14;
                }
                else
                {
                    part = (uint)(value / 10000000);
                    digits += 7;
                }
            }
            else
            {
                part = (uint)value;
            }

            if (part < 10)
            {
                // no-op
            }
            else if (part < 100)
            {
                digits += 1;
            }
            else if (part < 1000)
            {
                digits += 2;
            }
            else if (part < 10000)
            {
                digits += 3;
            }
            else if (part < 100000)
            {
                digits += 4;
            }
            else if (part < 1000000)
            {
                digits += 5;
            }
            else
            {
                Debug.Assert(part < 10000000);
                digits += 6;
            }

            return digits;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void WriteDigitsUInt64D(ulong value, Span<byte> buffer)
        {
            // We can mutate the 'value' parameter since it's a copy-by-value local.
            // It'll be used to represent the value left over after each division by 10.

            Debug.Assert(JsonWriter.CountDigits(value) == buffer.Length);

            for (int i = buffer.Length - 1; i >= 1; i--)
            {
                ulong temp = '0' + value;
                value /= 10;
                buffer[i] = (byte)(temp - (value * 10));
            }

            Debug.Assert(value < 10);
            buffer[0] = (byte)('0' + value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void WriteDigitsUInt64D(ulong value, Span<char> buffer)
        {
            // We can mutate the 'value' parameter since it's a copy-by-value local.
            // It'll be used to represent the value left over after each division by 10.

            Debug.Assert(JsonWriter.CountDigits(value) == buffer.Length);

            for (int i = buffer.Length - 1; i >= 1; i--)
            {
                ulong temp = '0' + value;
                value /= 10;
                buffer[i] = (char)(temp - (value * 10));
            }

            Debug.Assert(value < 10);
            buffer[0] = (char)('0' + value);
        }
    }

    public ref struct JsonWriterUtf8
    {
        JsonWriterUtf8<IBufferWriter> _jsonWriter;

        internal JsonWriterUtf8(BufferWriter<IBufferWriter> bufferWriter, bool prettyPrint = false)
        {
            _jsonWriter = new JsonWriterUtf8<IBufferWriter>(bufferWriter, prettyPrint);
        }

        public void WriteArrayStart() => _jsonWriter.WriteArrayStart();
        public void WriteArrayStart(string name) => _jsonWriter.WriteArrayStart(name);
        public void WriteArrayEnd() => _jsonWriter.WriteArrayEnd();
        public void WriteAttributeNull(string name) => _jsonWriter.WriteAttributeNull(name);
        public void WriteNull() => _jsonWriter.WriteNull();
        public void WriteObjectStart() => _jsonWriter.WriteObjectStart();
        public void WriteObjectStart(string name) => _jsonWriter.WriteObjectStart(name);
        public void WriteObjectEnd() => _jsonWriter.WriteObjectEnd();
        public void WriteAttribute(string name, string value) => _jsonWriter.WriteAttribute(name, value);
        public void WriteAttribute(string name, bool value) => _jsonWriter.WriteAttribute(name, value);
        public void WriteAttribute(string name, long value) => _jsonWriter.WriteAttribute(name, value);
        public void WriteAttribute(string name, ulong value) => _jsonWriter.WriteAttribute(name, value);
        public void WriteAttribute(string name, DateTime value) => _jsonWriter.WriteAttribute(name, value);
        public void WriteAttribute(string name, DateTimeOffset value) => _jsonWriter.WriteAttribute(name, value);
        public void WriteAttribute(string name, Guid value) => _jsonWriter.WriteAttribute(name, value);
        public void WriteValue(string value) => _jsonWriter.WriteValue(value);
        public void WriteValue(bool value) => _jsonWriter.WriteValue(value);
        public void WriteValue(long value) => _jsonWriter.WriteValue(value);
        public void WriteValue(ulong value) => _jsonWriter.WriteValue(value);
        public void WriteValue(DateTime value) => _jsonWriter.WriteValue(value);
        public void WriteValue(DateTimeOffset value) => _jsonWriter.WriteValue(value);
        public void WriteValue(Guid value) => _jsonWriter.WriteValue(value);
        public void Flush() => _jsonWriter.Flush();
    }


    public ref struct JsonWriterUtf16
    {
        JsonWriterUtf16<IBufferWriter> _jsonWriter;

        internal JsonWriterUtf16(BufferWriter<IBufferWriter> bufferWriter, bool prettyPrint = false)
        {
            _jsonWriter = new JsonWriterUtf16<IBufferWriter>(bufferWriter, prettyPrint);
        }

        public void WriteArrayStart() => _jsonWriter.WriteArrayStart();
        public void WriteArrayStart(string name) => _jsonWriter.WriteArrayStart(name);
        public void WriteArrayEnd() => _jsonWriter.WriteArrayEnd();
        public void WriteAttributeNull(string name) => _jsonWriter.WriteAttributeNull(name);
        public void WriteNull() => _jsonWriter.WriteNull();
        public void WriteObjectStart() => _jsonWriter.WriteObjectStart();
        public void WriteObjectStart(string name) => _jsonWriter.WriteObjectStart(name);
        public void WriteObjectEnd() => _jsonWriter.WriteObjectEnd();
        public void WriteAttribute(string name, string value) => _jsonWriter.WriteAttribute(name, value);
        public void WriteAttribute(string name, bool value) => _jsonWriter.WriteAttribute(name, value);
        public void WriteAttribute(string name, long value) => _jsonWriter.WriteAttribute(name, value);
        public void WriteAttribute(string name, ulong value) => _jsonWriter.WriteAttribute(name, value);
        public void WriteAttribute(string name, DateTime value) => _jsonWriter.WriteAttribute(name, value);
        public void WriteAttribute(string name, DateTimeOffset value) => _jsonWriter.WriteAttribute(name, value);
        public void WriteAttribute(string name, Guid value) => _jsonWriter.WriteAttribute(name, value);
        public void WriteValue(string value) => _jsonWriter.WriteValue(value);
        public void WriteValue(bool value) => _jsonWriter.WriteValue(value);
        public void WriteValue(long value) => _jsonWriter.WriteValue(value);
        public void WriteValue(ulong value) => _jsonWriter.WriteValue(value);
        public void WriteValue(DateTime value) => _jsonWriter.WriteValue(value);
        public void WriteValue(DateTimeOffset value) => _jsonWriter.WriteValue(value);
        public void WriteValue(Guid value) => _jsonWriter.WriteValue(value);
        public void Flush() => _jsonWriter.Flush();
    }

    internal struct IBufferWriter : IBufferWriter<byte>
    {
        public IBufferWriter<byte> _writer;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IBufferWriter(IBufferWriter<byte> writer)
        {
            _writer = writer;
        }

        public void Advance(int count) => _writer.Advance(count);

        public Memory<byte> GetMemory(int sizeHint = 0) => _writer.GetMemory(sizeHint);

        public Span<byte> GetSpan(int sizeHint = 0) => _writer.GetSpan(sizeHint);
    }
}
