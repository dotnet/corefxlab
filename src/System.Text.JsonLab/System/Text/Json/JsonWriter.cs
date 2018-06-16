// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Buffers.Writer;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Text.JsonLab
{
    public static class JsonWriter
    {
        internal static readonly byte[] s_newLineUtf8 = Encoding.UTF8.GetBytes(Environment.NewLine);
        internal static readonly int s_newLineUtf8Length = s_newLineUtf8.Length;
        internal static readonly char[] s_newLineUtf16 = Environment.NewLine.ToCharArray();
        internal static readonly int s_newLineUtf16Length = s_newLineUtf16.Length;

        public static JsonWriterUtf8<TBufferWriter> CreateUtf8<TBufferWriter>(
            TBufferWriter bufferWriter,
            bool prettyPrint = false) where TBufferWriter : IBufferWriter<byte>
        {
            return new JsonWriterUtf8<TBufferWriter>(BufferWriter.Create(bufferWriter), prettyPrint);
        }

        public static JsonWriterUtf8<TBufferWriter> Create<TBufferWriter>(
            BufferWriter<TBufferWriter> bufferWriter,
            bool prettyPrint = false) where TBufferWriter : IBufferWriter<byte>
        {
            return new JsonWriterUtf8<TBufferWriter>(bufferWriter, prettyPrint);
        }

        public static JsonWriterUtf16<TBufferWriter> CreateUtf16<TBufferWriter>(
            TBufferWriter bufferWriter,
            bool prettyPrint = false) where TBufferWriter : IBufferWriter<byte>
        {
            return new JsonWriterUtf16<TBufferWriter>(BufferWriter.Create(bufferWriter), prettyPrint);
        }

        public static JsonWriterUtf16<TBufferWriter> CreateUtf16<TBufferWriter>(
            BufferWriter<TBufferWriter> bufferWriter,
            bool prettyPrint = false) where TBufferWriter : IBufferWriter<byte>
        {
            return new JsonWriterUtf16<TBufferWriter>(bufferWriter, prettyPrint);
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
    }
}
