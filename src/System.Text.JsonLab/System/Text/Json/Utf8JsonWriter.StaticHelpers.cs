// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.IO;
using System.Runtime.CompilerServices;

namespace System.Text.JsonLab
{
    public static class Utf8JsonWriter2
    {
        public static Utf8JsonWriter2<IBufferWriter<byte>> CreateFromStream(Stream stream, JsonWriterState state)
        {
            var writer = new StreamFormatter(stream);
            return new Utf8JsonWriter2<IBufferWriter<byte>>(writer, state);
        }

        public static Utf8JsonWriter2<IBufferWriter<byte>> CreateFromMemory(Memory<byte> memory, JsonWriterState state)
        {
            var writer = new MemoryFormatter(memory);
            return new Utf8JsonWriter2<IBufferWriter<byte>>(writer, state);
        }

        // TODO: Either implement the escaping helpers from scratch or leverage the upcoming System.Text.Encodings.Web.TextEncoder APIs
        public static OperationStatus EscapeString(string value, Span<byte> destination, out int consumed, out int bytesWritten)
        {
            throw new NotImplementedException();
        }

        public static OperationStatus EscapeString(ReadOnlySpan<char> value, Span<byte> destination, out int consumed, out int bytesWritten)
        {
            //    // TODO:
            //    // if destination too small, return
            //    // stack-alloc for small values.
            //    // Check if there is no need to escape and encode directly into destination

            //    byte[] utf8Intermediary = ArrayPool<byte>.Shared.Rent(destination.Length);
            //    Span<byte> utf8Scratch = utf8Intermediary;
            //    OperationStatus status = Encodings.Utf16.ToUtf8(MemoryMarshal.AsBytes(value), utf8Scratch, out int encodingConsumed, out int encodingWritten);

            //    if (status != OperationStatus.Done)
            //    {
            //        consumed = 0;
            //        bytesWritten = 0;
            //        goto Done;
            //    }

            //    utf8Scratch = utf8Scratch.Slice(0, encodingWritten);

            //    // TODO: Divide by 2?
            //    consumed = encodingConsumed;
            //    bytesWritten = encodingWritten;

            //    if (JsonWriterHelper.IndexOfAnyEscape(utf8Scratch) != -1)
            //    {
            //        for (int i = 0; i < utf8Scratch.Length; i++)
            //        {
            //            //TODO: Escape all but the white-listed bytes and copy into the destination.
            //            if (i > destination.Length)
            //            {
            //                status = OperationStatus.DestinationTooSmall;
            //                goto Done;
            //            }
            //        }
            //        status = OperationStatus.Done;
            //    }
            //    else
            //    {
            //        if (utf8Scratch.Length <= destination.Length)
            //        {
            //            utf8Scratch.CopyTo(destination);
            //            bytesWritten = utf8Scratch.Length;
            //            status = OperationStatus.Done;
            //        }
            //        else
            //        {
            //            utf8Scratch.Slice(0, destination.Length).CopyTo(destination);
            //            bytesWritten = destination.Length;
            //            status = OperationStatus.DestinationTooSmall;
            //        }
            //    }

            //Done:
            //    ArrayPool<byte>.Shared.Return(utf8Intermediary);
            //    return status;

            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static OperationStatus EscapeString(ReadOnlySpan<byte> value, Span<byte> destination, out int consumed, out int bytesWritten)
        {
            throw new NotImplementedException();
        }
    }
}
